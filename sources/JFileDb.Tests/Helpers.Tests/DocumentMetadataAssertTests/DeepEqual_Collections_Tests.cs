using DustInTheWind.JFileDb.New;
using Xunit.Sdk;

namespace DustInTheWind.JFileDb.Tests.Helpers.Tests.DocumentMetadataAssertTests;

public class DeepEqual_Collections_Tests
{
    [Fact]
    public void DeepEqual_WithCollections_ComparesCorrectly()
    {
        // Arrange
        List<DocumentMetadata> expected = [
            new DocumentMetadata { TypeId = "issuer" },
            new DocumentMetadata { TypeId = "emission" }
        ];

        List<DocumentMetadata> actual = [
            new DocumentMetadata { TypeId = "issuer" },
            new DocumentMetadata { TypeId = "emission" }
        ];

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_WithCollectionsInDifferentOrder_DoesNotThrow()
    {
        // Arrange
        List<DocumentMetadata> expected = [
            new DocumentMetadata { TypeId = "issuer" },
            new DocumentMetadata { TypeId = "emission" },
            new DocumentMetadata { TypeId = "coin" },
            new DocumentMetadata { TypeId = "banknote" }
        ];

        List<DocumentMetadata> actual = [
            new DocumentMetadata { TypeId = "coin" },
            new DocumentMetadata { TypeId = "issuer" },
            new DocumentMetadata { TypeId = "banknote" },
            new DocumentMetadata { TypeId = "emission" }
        ];

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_WithComplexCollectionsInDifferentOrder_DoesNotThrow()
    {
        // Arrange
        List<DocumentMetadata> expected = [
            new DocumentMetadata 
            { 
                TypeId = "issuer",
                Directories = ["romania", "series1"],
                Children = [
                    new DocumentMetadata { TypeId = "coin" },
                    new DocumentMetadata { TypeId = "banknote" }
                ]
            },
            new DocumentMetadata 
            { 
                TypeId = "emission",
                Directories = ["vintage", "modern"],
                Children = [
                    new DocumentMetadata { TypeId = "collectible" }
                ]
            }
        ];

        List<DocumentMetadata> actual = [
            new DocumentMetadata 
            { 
                TypeId = "emission",
                Directories = ["modern", "vintage"], // Different order in directories
                Children = [
                    new DocumentMetadata { TypeId = "collectible" }
                ]
            },
            new DocumentMetadata 
            { 
                TypeId = "issuer",
                Directories = ["series1", "romania"], // Different order in directories
                Children = [
                    new DocumentMetadata { TypeId = "banknote" }, // Different order in children
                    new DocumentMetadata { TypeId = "coin" }
                ]
            }
        ];

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_WithDifferentCollectionCounts_ThrowsXunitException()
    {
        // Arrange
        List<DocumentMetadata> expected = [
            new DocumentMetadata { TypeId = "issuer" },
            new DocumentMetadata { TypeId = "emission" }
        ];

        List<DocumentMetadata> actual = [
            new DocumentMetadata { TypeId = "issuer" }
        ];

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        Assert.Contains("Expected  to have 2 items, but had 1 items", exception.Message);
    }

    [Fact]
    public void DeepEqual_WithMissingItemInCollection_ThrowsXunitException()
    {
        // Arrange
        List<DocumentMetadata> expected = [
            new DocumentMetadata { TypeId = "issuer" },
            new DocumentMetadata { TypeId = "emission" },
            new DocumentMetadata { TypeId = "coin" }
        ];

        List<DocumentMetadata> actual = [
            new DocumentMetadata { TypeId = "issuer" },
            new DocumentMetadata { TypeId = "banknote" } // Different TypeId instead of 'emission'
        ];

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        Assert.Contains("Expected  to have 3 items, but had 2 items", exception.Message);
    }

    [Fact]
    public void DeepEqual_WithDifferentItemInCollection_ThrowsXunitException()
    {
        // Arrange
        List<DocumentMetadata> expected = [
            new DocumentMetadata { TypeId = "issuer" },
            new DocumentMetadata { TypeId = "emission" }
        ];

        List<DocumentMetadata> actual = [
            new DocumentMetadata { TypeId = "issuer" },
            new DocumentMetadata { TypeId = "coin" } // Different TypeId
        ];

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        Assert.Contains("Expected item at [1] with TypeId 'emission' was not found in the actual collection", exception.Message);
    }

    [Fact]
    public void DeepEqual_WithNestedCollectionOrderDifferences_DoesNotThrow()
    {
        // Arrange
        List<DocumentMetadata> expected = [
            new DocumentMetadata 
            { 
                TypeId = "parent1",
                Children = [
                    new DocumentMetadata 
                    { 
                        TypeId = "child1",
                        Directories = ["dir1", "dir2"],
                        Children = [
                            new DocumentMetadata { TypeId = "grandchild1" },
                            new DocumentMetadata { TypeId = "grandchild2" }
                        ]
                    },
                    new DocumentMetadata { TypeId = "child2" }
                ]
            },
            new DocumentMetadata { TypeId = "parent2" }
        ];

        List<DocumentMetadata> actual = [
            new DocumentMetadata { TypeId = "parent2" }, // Different order at root level
            new DocumentMetadata 
            { 
                TypeId = "parent1",
                Children = [
                    new DocumentMetadata { TypeId = "child2" }, // Different order in children
                    new DocumentMetadata 
                    { 
                        TypeId = "child1",
                        Directories = ["dir2", "dir1"], // Different order in directories
                        Children = [
                            new DocumentMetadata { TypeId = "grandchild2" }, // Different order in grandchildren
                            new DocumentMetadata { TypeId = "grandchild1" }
                        ]
                    }
                ]
            }
        ];

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_WithEmptyCollections_DoesNotThrow()
    {
        // Arrange
        List<DocumentMetadata> expected = [];
        List<DocumentMetadata> actual = [];

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_WithNullCollections_DoesNotThrow()
    {
        // Arrange
        List<DocumentMetadata> expected = null;
        List<DocumentMetadata> actual = null;

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_WithOneNullCollection_ThrowsXunitException()
    {
        // Arrange
        List<DocumentMetadata> expected = [new DocumentMetadata { TypeId = "issuer" }];
        List<DocumentMetadata> actual = null;

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        Assert.Contains("Expected  to have 1 items, but had 0 items", exception.Message);
    }
}