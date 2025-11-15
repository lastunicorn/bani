using System.Collections;

namespace DustInTheWind.JFileDb.New;

public class DocumentMetadataCollection : ICollection<DocumentMetadata>
{
    private readonly List<DocumentMetadata> items = [];
    private readonly DocumentMetadata parent;

    public DocumentMetadataCollection()
    {
    }

    public DocumentMetadataCollection(DocumentMetadata parent)
    {
        this.parent = parent;
    }

    public int Count => items.Count;

    public bool IsReadOnly => false;

    public DocumentMetadata this[int index]
    {
        get => items[index];
        set
        {
            DocumentMetadata oldItem = items[index];
            if (oldItem != null)
            {
                UnsubscribeFromParentChanged(oldItem);
                oldItem.Parent = null;
            }

            if (value != null && parent != null)
            {
                value.Parent = parent;
                SubscribeToParentChanged(value);
            }

            items[index] = value;
        }
    }

    public void Add(DocumentMetadata item)
    {
        if (item == null)
            return;

        if (parent != null)
            item.Parent = parent;

        SubscribeToParentChanged(item);
        items.Add(item);
    }

    public void Clear()
    {
        foreach (DocumentMetadata item in items)
        {
            if (item != null)
            {
                UnsubscribeFromParentChanged(item);
                item.Parent = null;
            }
        }

        items.Clear();
    }

    public bool Contains(DocumentMetadata item)
    {
        return items.Contains(item);
    }

    public void CopyTo(DocumentMetadata[] array, int arrayIndex)
    {
        items.CopyTo(array, arrayIndex);
    }

    public bool Remove(DocumentMetadata item)
    {
        bool removed = items.Remove(item);

        if (removed && item != null)
        {
            UnsubscribeFromParentChanged(item);
            item.Parent = null;
        }

        return removed;
    }

    public IEnumerator<DocumentMetadata> GetEnumerator()
    {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private void SubscribeToParentChanged(DocumentMetadata item)
    {
        item.ParentChanged += HandleChildParentChanged;
    }

    private void UnsubscribeFromParentChanged(DocumentMetadata item)
    {
        item.ParentChanged -= HandleChildParentChanged;
    }

    private void HandleChildParentChanged(object sender, ParentChangedEventArgs e)
    {
        if (sender is DocumentMetadata child && e.NewParent != parent)
        {
            // The child's parent has changed to something other than our parent
            // Remove it from our collection without setting Parent to null (it's already changed)
            UnsubscribeFromParentChanged(child);
            items.Remove(child);
        }
    }
}