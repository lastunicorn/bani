namespace DustInTheWind.JFileDb.New;

internal class DocumentFile
{
    private const string MainDocumentPrefix = "m-";
    private const string ChildDocumentPrefix = "c-";
    private const string Extension = ".json";

    public string TypeId { get; }

    public bool IsMainDocument { get; }

    public bool IsChildDocument { get; }

    public bool IsValid => IsMainDocument || IsChildDocument;

    public DocumentFile(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);

        string fileName = Path.GetFileNameWithoutExtension(filePath);

        bool isJsonFile = Path.GetExtension(filePath).Equals(Extension, StringComparison.OrdinalIgnoreCase);
        if (!isJsonFile)
            return;

        bool isMainDocument = fileName.StartsWith(MainDocumentPrefix) && fileName.Length > MainDocumentPrefix.Length;
        if (isMainDocument)
        {
            TypeId = fileName.Substring(MainDocumentPrefix.Length);
            IsMainDocument = true;
        }

        bool isChildDocument = fileName.StartsWith(ChildDocumentPrefix) && fileName.Length > ChildDocumentPrefix.Length;
        if (isChildDocument)
        {
            TypeId = fileName.Substring(ChildDocumentPrefix.Length);
            IsChildDocument = true;
        }
    }
}