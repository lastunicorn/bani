using DustInTheWind.JFileDb.New;

namespace DustInTheWind.JFileDb.Tests.Helpers.Tests.DocumentMetadataAssertTests;

/// <summary>
/// Tests specifically focused on verifying that order of items in collections doesn't matter.
/// These tests ensure that the DeepEqual method performs order-independent comparisons
/// for both Directories and Children collections.
/// </summary>
public class DeepEqual_OrderIndependence_Tests
{
    [Fact]
    public void DeepEqual_DirectoriesInReverseOrder_DoesNotThrow()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = ["first", "second", "third", "fourth"]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = ["fourth", "third", "second", "first"]
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_DirectoriesInRandomOrder_DoesNotThrow()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = ["alpha", "beta", "gamma", "delta", "epsilon"]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = ["gamma", "alpha", "epsilon", "beta", "delta"]
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_ChildrenInReverseOrder_DoesNotThrow()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "first" },
                new DocumentMetadata { TypeId = "second" },
                new DocumentMetadata { TypeId = "third" }
            ]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "third" },
                new DocumentMetadata { TypeId = "second" },
                new DocumentMetadata { TypeId = "first" }
            ]
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_ChildrenInRandomOrder_DoesNotThrow()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "alpha" },
                new DocumentMetadata { TypeId = "beta" },
                new DocumentMetadata { TypeId = "gamma" },
                new DocumentMetadata { TypeId = "delta" }
            ]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "gamma" },
                new DocumentMetadata { TypeId = "alpha" },
                new DocumentMetadata { TypeId = "delta" },
                new DocumentMetadata { TypeId = "beta" }
            ]
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_BothDirectoriesAndChildrenInDifferentOrder_DoesNotThrow()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = ["dir1", "dir2", "dir3"],
            Children = [
                new DocumentMetadata { TypeId = "child1" },
                new DocumentMetadata { TypeId = "child2" },
                new DocumentMetadata { TypeId = "child3" }
            ]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = ["dir3", "dir1", "dir2"],
            Children = [
                new DocumentMetadata { TypeId = "child2" },
                new DocumentMetadata { TypeId = "child3" },
                new DocumentMetadata { TypeId = "child1" }
            ]
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_MultiLevelNestingWithDifferentOrders_DoesNotThrow()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "root",
            Directories = ["root-dir1", "root-dir2"],
            Children = [
                new DocumentMetadata
                {
                    TypeId = "level1-a",
                    Directories = ["l1a-dir1", "l1a-dir2"],
                    Children = [
                        new DocumentMetadata 
                        { 
                            TypeId = "level2-a1",
                            Directories = ["l2a1-dir1", "l2a1-dir2"]
                        },
                        new DocumentMetadata 
                        { 
                            TypeId = "level2-a2",
                            Directories = ["l2a2-dir1", "l2a2-dir2"]
                        }
                    ]
                },
                new DocumentMetadata
                {
                    TypeId = "level1-b",
                    Directories = ["l1b-dir1", "l1b-dir2"],
                    Children = []
                }
            ]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "root",
            Directories = ["root-dir2", "root-dir1"], // Different order
            Children = [
                new DocumentMetadata
                {
                    TypeId = "level1-b", // Different order
                    Directories = ["l1b-dir2", "l1b-dir1"], // Different order
                    Children = []
                },
                new DocumentMetadata
                {
                    TypeId = "level1-a",
                    Directories = ["l1a-dir2", "l1a-dir1"], // Different order
                    Children = [
                        new DocumentMetadata 
                        { 
                            TypeId = "level2-a2", // Different order
                            Directories = ["l2a2-dir2", "l2a2-dir1"] // Different order
                        },
                        new DocumentMetadata 
                        { 
                            TypeId = "level2-a1",
                            Directories = ["l2a1-dir2", "l2a1-dir1"] // Different order
                        }
                    ]
                }
            ]
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_CollectionWithComplexOrderDifferences_DoesNotThrow()
    {
        // Arrange
        List<DocumentMetadata> expected = [
            new DocumentMetadata
            {
                TypeId = "first-doc",
                Directories = ["first-dir1", "first-dir2", "first-dir3"],
                Children = [
                    new DocumentMetadata { TypeId = "first-child1" },
                    new DocumentMetadata { TypeId = "first-child2" }
                ]
            },
            new DocumentMetadata
            {
                TypeId = "second-doc",
                Directories = ["second-dir1", "second-dir2"],
                Children = [
                    new DocumentMetadata { TypeId = "second-child1" }
                ]
            },
            new DocumentMetadata
            {
                TypeId = "third-doc",
                Directories = ["third-dir1"],
                Children = []
            }
        ];

        List<DocumentMetadata> actual = [
            new DocumentMetadata
            {
                TypeId = "third-doc", // Different position
                Directories = ["third-dir1"],
                Children = []
            },
            new DocumentMetadata
            {
                TypeId = "first-doc", // Different position
                Directories = ["first-dir3", "first-dir1", "first-dir2"], // Different order
                Children = [
                    new DocumentMetadata { TypeId = "first-child2" }, // Different order
                    new DocumentMetadata { TypeId = "first-child1" }
                ]
            },
            new DocumentMetadata
            {
                TypeId = "second-doc", // Different position
                Directories = ["second-dir2", "second-dir1"], // Different order
                Children = [
                    new DocumentMetadata { TypeId = "second-child1" }
                ]
            }
        ];

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_SingleItemCollections_DoesNotThrow()
    {
        // Arrange - Even with single items, order should not matter
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = ["single-directory"],
            Children = [
                new DocumentMetadata { TypeId = "single-child" }
            ]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = ["single-directory"],
            Children = [
                new DocumentMetadata { TypeId = "single-child" }
            ]
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_EmptyCollections_DoesNotThrow()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = [],
            Children = []
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = [],
            Children = []
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }
}