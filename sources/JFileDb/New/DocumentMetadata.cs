namespace DustInTheWind.JFileDb.New;

internal class DocumentMetadata
{
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
    public List<DocumentMetadata> Children { get; set; } = [];
}
