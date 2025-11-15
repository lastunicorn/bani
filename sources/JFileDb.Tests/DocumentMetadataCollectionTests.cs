using DustInTheWind.JFileDb.New;

namespace DustInTheWind.JFileDb.Tests;

public class DocumentMetadataCollectionTests
{
    [Fact]
    public void Add_WhenChildIsAdded_ParentIsSetAutomatically()
    {
        // Arrange
        DocumentMetadata parent = new DocumentMetadata { TypeId = "parent" };
        DocumentMetadata child = new DocumentMetadata { TypeId = "child" };

        // Act
        parent.Children.Add(child);

        // Assert
        Assert.Equal(parent, child.Parent);
    }

    [Fact]
    public void Indexer_WhenChildIsSetByIndex_ParentIsSetAutomatically()
    {
        // Arrange
        DocumentMetadata parent = new DocumentMetadata { TypeId = "parent" };
        DocumentMetadata child1 = new DocumentMetadata { TypeId = "child1" };
        DocumentMetadata child2 = new DocumentMetadata { TypeId = "child2" };

        parent.Children.Add(child1);

        // Act
        parent.Children[0] = child2;

        // Assert
        Assert.Equal(parent, child2.Parent);
        Assert.Null(child1.Parent); // Old child should have parent cleared
    }

    [Fact]
    public void Remove_WhenChildIsRemoved_ParentIsCleared()
    {
        // Arrange
        DocumentMetadata parent = new DocumentMetadata { TypeId = "parent" };
        DocumentMetadata child = new DocumentMetadata { TypeId = "child" };

        parent.Children.Add(child);

        // Act
        parent.Children.Remove(child);

        // Assert
        Assert.Null(child.Parent);
    }

    [Fact]
    public void Clear_WhenCollectionIsCleared_AllParentsAreCleared()
    {
        // Arrange
        DocumentMetadata parent = new DocumentMetadata { TypeId = "parent" };
        DocumentMetadata child1 = new DocumentMetadata { TypeId = "child1" };
        DocumentMetadata child2 = new DocumentMetadata { TypeId = "child2" };

        parent.Children.Add(child1);
        parent.Children.Add(child2);

        // Act
        parent.Children.Clear();

        // Assert
        Assert.Null(child1.Parent);
        Assert.Null(child2.Parent);
    }

    [Fact]
    public void ParentChanged_WhenParentIsSet_EventIsRaised()
    {
        // Arrange
        DocumentMetadata child = new DocumentMetadata { TypeId = "child" };
        DocumentMetadata parent = new DocumentMetadata { TypeId = "parent" };
        
        ParentChangedEventArgs eventArgs = null;
        int eventCallCount = 0;
        
        child.ParentChanged += (sender, e) =>
        {
            eventArgs = e;
            eventCallCount++;
        };

        // Act
        child.Parent = parent;

        // Assert
        Assert.Equal(1, eventCallCount);
        Assert.NotNull(eventArgs);
        Assert.Null(eventArgs.OldParent);
        Assert.Equal(parent, eventArgs.NewParent);
    }

    [Fact]
    public void ParentChanged_WhenParentIsChanged_EventIsRaisedWithCorrectValues()
    {
        // Arrange
        DocumentMetadata child = new DocumentMetadata { TypeId = "child" };
        DocumentMetadata oldParent = new DocumentMetadata { TypeId = "oldParent" };
        DocumentMetadata newParent = new DocumentMetadata { TypeId = "newParent" };
        
        child.Parent = oldParent;
        
        ParentChangedEventArgs eventArgs = null;
        int eventCallCount = 0;
        
        child.ParentChanged += (sender, e) =>
        {
            eventArgs = e;
            eventCallCount++;
        };

        // Act
        child.Parent = newParent;

        // Assert
        Assert.Equal(1, eventCallCount);
        Assert.NotNull(eventArgs);
        Assert.Equal(oldParent, eventArgs.OldParent);
        Assert.Equal(newParent, eventArgs.NewParent);
    }

    [Fact]
    public void ParentChanged_WhenParentIsSetToSameValue_EventIsNotRaised()
    {
        // Arrange
        DocumentMetadata child = new DocumentMetadata { TypeId = "child" };
        DocumentMetadata parent = new DocumentMetadata { TypeId = "parent" };
        
        child.Parent = parent;
        
        int eventCallCount = 0;
        
        child.Parent = parent;
        
        child.ParentChanged += (sender, e) =>
        {
            eventCallCount++;
        };

        // Act
        child.Parent = parent; // Set to same value

        // Assert
        Assert.Equal(0, eventCallCount);
    }

    [Fact]
    public void ParentChanged_WhenParentIsClearedToNull_EventIsRaised()
    {
        // Arrange
        DocumentMetadata child = new DocumentMetadata { TypeId = "child" };
        DocumentMetadata parent = new DocumentMetadata { TypeId = "parent" };
        
        child.Parent = parent;
        
        ParentChangedEventArgs eventArgs = null;
        int eventCallCount = 0;
        
        child.ParentChanged += (sender, e) =>
        {
            eventArgs = e;
            eventCallCount++;
        };

        // Act
        child.Parent = null;

        // Assert
        Assert.Equal(1, eventCallCount);
        Assert.NotNull(eventArgs);
        Assert.Equal(parent, eventArgs.OldParent);
        Assert.Null(eventArgs.NewParent);
    }

    [Fact]
    public void AutoRemoval_WhenChildParentIsChangedToDifferentParent_ChildIsRemovedFromCollection()
    {
        // Arrange
        DocumentMetadata parent1 = new DocumentMetadata { TypeId = "parent1" };
        DocumentMetadata parent2 = new DocumentMetadata { TypeId = "parent2" };
        DocumentMetadata child = new DocumentMetadata { TypeId = "child" };

        parent1.Children.Add(child);

        // Act - Change child's parent to a different parent
        child.Parent = parent2;

        // Assert
        Assert.DoesNotContain(child, parent1.Children);
        Assert.Empty(parent1.Children);
        Assert.Equal(parent2, child.Parent);
    }

    [Fact]
    public void AutoRemoval_WhenChildParentIsSetToNull_ChildIsRemovedFromCollection()
    {
        // Arrange
        DocumentMetadata parent = new DocumentMetadata { TypeId = "parent" };
        DocumentMetadata child = new DocumentMetadata { TypeId = "child" };

        parent.Children.Add(child);

        // Act - Set child's parent to null
        child.Parent = null;

        // Assert
        Assert.DoesNotContain(child, parent.Children);
        Assert.Empty(parent.Children);
        Assert.Null(child.Parent);
    }

    [Fact]
    public void AutoRemoval_WhenMultipleChildrenAndOneParentChanges_OnlyThatChildIsRemoved()
    {
        // Arrange
        DocumentMetadata parent1 = new DocumentMetadata { TypeId = "parent1" };
        DocumentMetadata parent2 = new DocumentMetadata { TypeId = "parent2" };
        DocumentMetadata child1 = new DocumentMetadata { TypeId = "child1" };
        DocumentMetadata child2 = new DocumentMetadata { TypeId = "child2" };

        parent1.Children.Add(child1);
        parent1.Children.Add(child2);

        // Act - Change only child1's parent
        child1.Parent = parent2;

        // Assert
        Assert.DoesNotContain(child1, parent1.Children);
        Assert.Contains(child2, parent1.Children);
        Assert.Single(parent1.Children);
        Assert.Equal(parent2, child1.Parent);
        Assert.Equal(parent1, child2.Parent);
    }

    [Fact]
    public void AutoRemoval_WhenChildIsMovedToAnotherCollection_BothCollectionsAreUpdatedCorrectly()
    {
        // Arrange
        DocumentMetadata parent1 = new DocumentMetadata { TypeId = "parent1" };
        DocumentMetadata parent2 = new DocumentMetadata { TypeId = "parent2" };
        DocumentMetadata child = new DocumentMetadata { TypeId = "child" };

        parent1.Children.Add(child);

        // Act - Add child to parent2's collection (this should change the parent)
        parent2.Children.Add(child);

        // Assert
        Assert.DoesNotContain(child, parent1.Children);
        Assert.Contains(child, parent2.Children);
        Assert.Empty(parent1.Children);
        Assert.Single(parent2.Children);
        Assert.Equal(parent2, child.Parent);
    }

    [Fact]
    public void EventSubscription_WhenChildIsRemovedExplicitly_EventHandlerIsUnsubscribed()
    {
        // Arrange
        DocumentMetadata parent = new DocumentMetadata { TypeId = "parent" };
        DocumentMetadata child = new DocumentMetadata { TypeId = "child" };

        parent.Children.Add(child);

        // Verify event is subscribed (child is automatically removed when parent changes)
        parent.Children.Remove(child);
        
        // Act - Change parent after removal - should not affect the collection
        int originalCount = parent.Children.Count;
        child.Parent = new DocumentMetadata { TypeId = "otherParent" };

        // Assert - Collection count should remain the same
        Assert.Equal(originalCount, parent.Children.Count);
    }
}