using DustInTheWind.JFileDb.New;
using Xunit.Sdk;

namespace DustInTheWind.JFileDb.Tests.Helpers.Tests.DocumentMetadataAssertTests;

public class DeepEqual_Instances_Tests
{
    [Fact]
    public void DeepEqual_WithIdenticalDocumentMetadata_DoesNotThrow()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = ["romania", "series1"],
            Children = [
                new DocumentMetadata
                {
                    TypeId = "emission",
                    Directories = ["child-dir"],
                    Children = []
                }
            ]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = ["romania", "series1"],
            Children = [
                new DocumentMetadata
                {
                    TypeId = "emission",
                    Directories = ["child-dir"],
                    Children = []
                }
            ]
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_WithDifferentTypeId_ThrowsXunitException()
    {
        // Arrange
        DocumentMetadata expected = new() { TypeId = "issuer" };
        DocumentMetadata actual = new() { TypeId = "emission" };

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        Assert.Contains("Expected TypeId at Root to be 'issuer', but was 'emission'", exception.Message);
    }

    [Fact]
    public void DeepEqual_WithDifferentDirectoriesCount_ThrowsXunitException()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = ["romania", "series1"]
        };
        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = ["romania"]
        };

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        Assert.Contains("Expected Root.Directories to have 2 items, but had 1 items", exception.Message);
    }

    [Fact]
    public void DeepEqual_WithDifferentChildrenCount_ThrowsXunitException()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "emission" },
                new DocumentMetadata { TypeId = "coin" }
            ]
        };
        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "emission" }
            ]
        };

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        Assert.Contains("Expected Root.Children to have 2 items, but had 1 items", exception.Message);
    }

    [Fact]
    public void DeepEqual_WithDirectoriesInDifferentOrder_DoesNotThrow()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = ["romania", "series1", "vintage", "commemorative"]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = ["commemorative", "romania", "vintage", "series1"]
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_WithChildrenInDifferentOrder_DoesNotThrow()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "emission" },
                new DocumentMetadata { TypeId = "coin" },
                new DocumentMetadata { TypeId = "banknote" }
            ]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "banknote" },
                new DocumentMetadata { TypeId = "emission" },
                new DocumentMetadata { TypeId = "coin" }
            ]
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_WithNestedChildrenAndDirectoriesInDifferentOrder_DoesNotThrow()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = ["romania", "series1"],
            Children = [
                new DocumentMetadata
                {
                    TypeId = "emission",
                    Directories = ["dir1", "dir2"],
                    Children = [
                        new DocumentMetadata { TypeId = "coin" },
                        new DocumentMetadata { TypeId = "banknote" }
                    ]
                },
                new DocumentMetadata
                {
                    TypeId = "collection",
                    Directories = ["vintage", "modern"],
                    Children = []
                }
            ]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = ["series1", "romania"], // Different order
            Children = [
                new DocumentMetadata
                {
                    TypeId = "collection", // Different order
                    Directories = ["modern", "vintage"], // Different order
                    Children = []
                },
                new DocumentMetadata
                {
                    TypeId = "emission",
                    Directories = ["dir2", "dir1"], // Different order
                    Children = [
                        new DocumentMetadata { TypeId = "banknote" }, // Different order
                        new DocumentMetadata { TypeId = "coin" }
                    ]
                }
            ]
        };

        // Act & Assert
        DocumentMetadataAssert.DeepEqual(expected, actual);
    }

    [Fact]
    public void DeepEqual_WithMissingDirectoryInActual_ThrowsXunitException()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = ["romania", "series1", "vintage"]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = ["romania", "series1"] // 'vintage' is missing
        };

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        Assert.Contains("Expected Root.Directories to have 3 items, but had 2 items", exception.Message);
    }

    [Fact]
    public void DeepEqual_WithExtraDirectoryInActual_ThrowsXunitException()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = ["romania", "series1"]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = ["romania", "series1", "extra"] // 'extra' is unexpected
        };

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        // The error message will be about different counts since we have 2 vs 3 items
        Assert.Contains("Expected Root.Directories to have 2 items, but had 3 items", exception.Message);
    }

    [Fact]
    public void DeepEqual_WithDifferentDirectoryContent_ThrowsXunitException()
    {
        // Arrange - Same count but different content to test the "missing items" message
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Directories = ["romania", "series1", "vintage"]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Directories = ["romania", "series1", "modern"] // 'vintage' missing, 'modern' extra
        };

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        Assert.Contains("Expected Root.Directories to contain items: 'vintage', but they were not found", exception.Message);
    }

    [Fact]
    public void DeepEqual_WithMissingChildInActual_ThrowsXunitException()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "emission" },
                new DocumentMetadata { TypeId = "coin" }
            ]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "emission" }
                // 'coin' child is missing
            ]
        };

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        Assert.Contains("Expected Root.Children to have 2 items, but had 1 items", exception.Message);
    }

    [Fact]
    public void DeepEqual_WithDifferentChildTypeIdInDifferentOrder_ThrowsXunitException()
    {
        // Arrange
        DocumentMetadata expected = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "emission" },
                new DocumentMetadata { TypeId = "coin" }
            ]
        };

        DocumentMetadata actual = new()
        {
            TypeId = "issuer",
            Children = [
                new DocumentMetadata { TypeId = "banknote" }, // Different TypeId
                new DocumentMetadata { TypeId = "emission" }
            ]
        };

        // Act & Assert
        XunitException exception = Assert.Throws<XunitException>(() =>
        {
            DocumentMetadataAssert.DeepEqual(expected, actual);
        });

        Assert.Contains("Expected item at Root.Children[1] with TypeId 'coin' was not found in the actual collection", exception.Message);
    }
}
