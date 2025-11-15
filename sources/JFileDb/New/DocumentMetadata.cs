namespace DustInTheWind.JFileDb.New;

public class DocumentMetadata
{
    private DocumentMetadata parent;

    /// <summary>
    /// Gets or sets the unique identifier that identifies the document type.
    /// It is used in the name of the file that stores the document data.
    /// </summary>
    /// <remarks>
    /// The name of the file is formed by combining a prefix, the TypeId and the extension:
    ///     - the prefix is "m-" from "entity" if the file is placed in its own directory.
    ///     - the prefix is "c-" from "child" if the file is placed in same directory as parent.
    ///     - extension is always ".json"
    /// </remarks>
    public string TypeId { get; set; }

    /// <summary>
    /// Gets or sets the list of directories considered from the parent document.
    /// </summary>
    public List<string> Directories { get; set; } = [];

    /// <summary>
    /// Gets or sets the list of child documents.
    /// </summary>
    public DocumentMetadataCollection Children { get; set; }

    /// <summary>
    /// Gets or sets the parent document metadata. This is null for root-level documents.
    /// </summary>
    public DocumentMetadata Parent
    {
        get => parent;
        set
        {
            DocumentMetadata oldParent = parent;
            if (oldParent != value)
            {
                parent = value;
                OnParentChanged(new ParentChangedEventArgs(oldParent, value));
            }
        }
    }

    /// <summary>
    /// Occurs when the Parent property value changes.
    /// </summary>
    public event EventHandler<ParentChangedEventArgs> ParentChanged;

    public DocumentMetadata()
    {
        Children = new DocumentMetadataCollection(this);
    }

    public string GetFullPath()
    {
        string[] pathParts = EnumeratePathParts()
            .ToArray();

        return Path.Combine(pathParts);
    }

    private IEnumerable<string> EnumeratePathParts()
    {
        IEnumerable<DocumentMetadata> ancestors = EnumerateAncestors().Reverse();

        foreach (DocumentMetadata ancestor in ancestors)
        {
            if (ancestor.Directories != null)
            {
                foreach (string directoryName in ancestor.Directories)
                    yield return directoryName;
            }
        }

        if (Directories != null)
        {
            foreach (string directoryName in Directories)
                yield return directoryName;
        }

        yield return GetFileName();
    }

    private IEnumerable<DocumentMetadata> EnumerateAncestors()
    {
        DocumentMetadata current = Parent;

        while (current != null)
        {
            yield return current;
            current = current.Parent;
        }
    }

    private string GetFileName()
    {
        string prefix = Parent == null ? "m-" : "c-";
        return $"{prefix}{TypeId}.json";
    }

    /// <summary>
    /// Raises the ParentChanged event.
    /// </summary>
    /// <param name="e">The event arguments containing the old and new parent values.</param>
    protected virtual void OnParentChanged(ParentChangedEventArgs e)
    {
        ParentChanged?.Invoke(this, e);
    }
}
