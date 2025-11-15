namespace DustInTheWind.JFileDb.New;

/// <summary>
/// Provides data for the ParentChanged event.
/// </summary>
public class ParentChangedEventArgs : EventArgs
{
    /// <summary>
    /// Gets the previous parent document metadata.
    /// </summary>
    public DocumentMetadata OldParent { get; }

    /// <summary>
    /// Gets the new parent document metadata.
    /// </summary>
    public DocumentMetadata NewParent { get; }

    /// <summary>
    /// Initializes a new instance of the ParentChangedEventArgs class.
    /// </summary>
    /// <param name="oldParent">The previous parent document metadata.</param>
    /// <param name="newParent">The new parent document metadata.</param>
    public ParentChangedEventArgs(DocumentMetadata oldParent, DocumentMetadata newParent)
    {
        OldParent = oldParent;
        NewParent = newParent;
    }
}