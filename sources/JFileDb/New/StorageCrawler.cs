namespace DustInTheWind.JFileDb.New;

public class StorageCrawler
{
    private readonly string rootPath;

    public List<DocumentMetadata> Items { get; private set; }

    public StorageCrawler(string rootPath)
    {
        this.rootPath = rootPath ?? throw new ArgumentNullException(nameof(rootPath));
    }

    public void Open()
    {
        Items = Crawl(rootPath).ToList();
    }

    private IEnumerable<DocumentMetadata> Crawl(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
            yield break;

        IEnumerable<DocumentFile> documentFiles = Directory.GetFiles(directoryPath, "*.json")
            .Select(x => new DocumentFile(x))
            .Where(x => x.IsValid);

        List<DocumentFile> mainDocuments = [];
        List<DocumentFile> childDocuments = [];

        foreach (DocumentFile doc in documentFiles)
        {
            if (doc.IsMainDocument)
                mainDocuments.Add(doc);
            else if (doc.IsChildDocument)
                childDocuments.Add(doc);
        }

        // Process main documents
        foreach (DocumentFile documentFile in mainDocuments)
        {
            DocumentMetadata documentMetadata = new()
            {
                TypeId = documentFile.TypeId,
                Directories = GetRelativePath(directoryPath)
            };

            // Add child documents from the same directory to this main document
            foreach (DocumentFile childDocument in childDocuments)
            {
                DocumentMetadata childMetadata = new()
                {
                    TypeId = childDocument.TypeId,
                    Directories = GetRelativePath(directoryPath)
                };
                documentMetadata.Children.Add(childMetadata);
            }

            // For main documents, recursively search all subdirectories for child documents
            string[] subDirectories = Directory.GetDirectories(directoryPath);
            foreach (string subDirectory in subDirectories)
            {
                IEnumerable<DocumentMetadata> children = CrawlAndAttachOrphans(subDirectory, documentMetadata);
                foreach (DocumentMetadata child in children)
                    documentMetadata.Children.Add(child);
            }

            yield return documentMetadata;
        }

        // If no main documents were found in current directory, process child documents as standalone documents
        // and still search subdirectories for any documents
        if (!mainDocuments.Any())
        {
            // Process standalone child documents (use regular path, not series path)
            foreach (DocumentFile childDocument in childDocuments)
            {
                DocumentMetadata childMetadata = new()
                {
                    TypeId = childDocument.TypeId,
                    Directories = GetRelativePath(directoryPath)
                };
                yield return childMetadata;
            }

            // Search subdirectories
            string[] subDirectories = Directory.GetDirectories(directoryPath);
            foreach (string subDirectory in subDirectories)
            {
                foreach (DocumentMetadata document in Crawl(subDirectory))
                    yield return document;
            }
        }
    }

    private IEnumerable<DocumentMetadata> CrawlAndAttachOrphans(string directoryPath, DocumentMetadata parentMainDocument)
    {
        if (!Directory.Exists(directoryPath))
            yield break;

        IEnumerable<DocumentFile> documentFiles = Directory.GetFiles(directoryPath, "*.json")
            .Select(x => new DocumentFile(x))
            .Where(x => x.IsValid);

        List<DocumentFile> mainDocuments = [];
        List<DocumentFile> childDocuments = [];

        foreach (DocumentFile doc in documentFiles)
        {
            if (doc.IsMainDocument)
                mainDocuments.Add(doc);
            else if (doc.IsChildDocument)
                childDocuments.Add(doc);
        }

        // Process main documents normally
        foreach (DocumentFile documentFile in mainDocuments)
        {
            DocumentMetadata documentMetadata = new()
            {
                TypeId = documentFile.TypeId,
                Directories = GetRelativePath(directoryPath)
            };

            // Add child documents from the same directory to this main document
            foreach (DocumentFile childDocument in childDocuments)
            {
                DocumentMetadata childMetadata = new()
                {
                    TypeId = childDocument.TypeId,
                    Directories = GetRelativePath(directoryPath)
                };
                documentMetadata.Children.Add(childMetadata);
            }

            // For main documents, recursively search all subdirectories
            string[] subDirectories = Directory.GetDirectories(directoryPath);
            foreach (string subDirectory in subDirectories)
            {
                IEnumerable<DocumentMetadata> children = CrawlAndAttachOrphans(subDirectory, documentMetadata);
                foreach (DocumentMetadata child in children)
                    documentMetadata.Children.Add(child);
            }

            yield return documentMetadata;
        }

        // If no main documents were found, attach orphaned child documents to the parent main document
        if (!mainDocuments.Any() && childDocuments.Any())
        {
            foreach (DocumentFile childDocument in childDocuments)
            {
                DocumentMetadata childMetadata = new()
                {
                    TypeId = childDocument.TypeId,
                    Directories = GetRelativePath(directoryPath)
                };
                parentMainDocument.Children.Add(childMetadata);
            }
        }

        // If no main documents were found, still search subdirectories but attach any found documents to parent
        if (!mainDocuments.Any())
        {
            string[] subDirectories = Directory.GetDirectories(directoryPath);
            foreach (string subDirectory in subDirectories)
            {
                foreach (DocumentMetadata document in CrawlAndAttachOrphans(subDirectory, parentMainDocument))
                    yield return document;
            }
        }
    }

    private List<string> GetRelativePath(string directoryPath)
    {
        string relativePath = Path.GetRelativePath(rootPath, directoryPath);

        if (relativePath == "." || string.IsNullOrEmpty(relativePath))
            return [];

        return relativePath.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries).ToList();
    }
}
