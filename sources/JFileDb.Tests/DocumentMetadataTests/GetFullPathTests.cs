using DustInTheWind.JFileDb.New;

namespace DustInTheWind.JFileDb.Tests.DocumentMetadataTests;

public class GetFullPathTests
{
    [Fact]
    public void GetFullPath_WhenDocumentHasNoParentAndNoDirectories_ReturnsFileNameWithMainPrefix()
    {
        // Arrange
        DocumentMetadata documentMetadata = new()
        {
            TypeId = "mydocument"
        };

        // Act
        string result = documentMetadata.GetFullPath();

        // Assert
        Assert.Equal("m-mydocument.json", result);
    }

    [Fact]
    public void GetFullPath_WhenDocumentHasNoParentButHasDirectories_ReturnsPathWithDirectoriesAndMainPrefix()
    {
        // Arrange
        DocumentMetadata documentMetadata = new()
        {
            TypeId = "mydocument",
            Directories = ["folder1", "folder2"]
        };

        // Act
        string result = documentMetadata.GetFullPath();

        // Assert
        Assert.Equal(Path.Combine("folder1", "folder2", "m-mydocument.json"), result);
    }

    [Fact]
    public void GetFullPath_WhenDocumentHasParentButNoDirectories_ReturnsFileNameWithChildPrefix()
    {
        // Arrange
        DocumentMetadata parent = new()
        {
            TypeId = "parent"
        };
        DocumentMetadata child = new()
        {
            TypeId = "child",
            Parent = parent
        };

        // Act
        string result = child.GetFullPath();

        // Assert
        Assert.Equal("c-child.json", result);
    }

    [Fact]
    public void GetFullPath_WhenDocumentHasParentAndDirectories_ReturnsPathWithDirectoriesAndChildPrefix()
    {
        // Arrange
        DocumentMetadata parent = new()
        {
            TypeId = "parent"
        };
        DocumentMetadata child = new()
        {
            TypeId = "child",
            Parent = parent,
            Directories = ["subfolder"]
        };

        // Act
        string result = child.GetFullPath();

        // Assert
        Assert.Equal(Path.Combine("subfolder", "m-child.json"), result);
    }

    [Fact]
    public void GetFullPath_WhenParentHasDirectories_IncludesParentDirectoriesInPath()
    {
        // Arrange
        DocumentMetadata parent = new()
        {
            TypeId = "parent",
            Directories = ["parentfolder1", "parentfolder2"]
        };
        DocumentMetadata child = new()
        {
            TypeId = "child",
            Parent = parent,
            Directories = ["childfolder"]
        };

        // Act
        string result = child.GetFullPath();

        // Assert
        string expected = Path.Combine("parentfolder1", "parentfolder2", "childfolder", "m-child.json");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetFullPath_WhenMultipleLevelsOfHierarchy_BuildsCompletePathFromRootToChild()
    {
        // Arrange
        DocumentMetadata root = new()
        {
            TypeId = "root",
            Directories = ["rootdir"]
        };
        DocumentMetadata middle = new()
        {
            TypeId = "middle",
            Parent = root,
            Directories = ["middledir"]
        };
        DocumentMetadata leaf = new()
        {
            TypeId = "leaf",
            Parent = middle,
            Directories = ["leafdir"]
        };

        // Act
        string result = leaf.GetFullPath();

        // Assert
        string expected = Path.Combine("rootdir", "middledir", "leafdir", "m-leaf.json");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetFullPath_WhenGrandparentHasNoDirectoriesButParentAndChildDo_BuildsCorrectPath()
    {
        // Arrange
        DocumentMetadata grandparent = new()
        {
            TypeId = "grandparent"
        };
        DocumentMetadata parent = new()
        {
            TypeId = "parent",
            Parent = grandparent,
            Directories = ["parentdir"]
        };
        DocumentMetadata child = new()
        {
            TypeId = "child",
            Parent = parent,
            Directories = ["childdir"]
        };

        // Act
        string result = child.GetFullPath();

        // Assert
        string expected = Path.Combine("parentdir", "childdir", "m-child.json");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetFullPath_WhenDirectoriesAreEmpty_ReturnsOnlyFileName()
    {
        // Arrange
        DocumentMetadata documentMetadata = new()
        {
            TypeId = "document",
            Directories = []
        };

        // Act
        string result = documentMetadata.GetFullPath();

        // Assert
        Assert.Equal("m-document.json", result);
    }

    [Fact]
    public void GetFullPath_WhenTypeIdContainsSpecialCharacters_PreservesCharactersInFileName()
    {
        // Arrange
        DocumentMetadata documentMetadata = new()
        {
            TypeId = "my-document_type"
        };

        // Act
        string result = documentMetadata.GetFullPath();

        // Assert
        Assert.Equal("m-my-document_type.json", result);
    }

    [Fact]
    public void GetFullPath_WhenParentHasDirectoriesButChildDoesNot_IncludesOnlyParentDirectories()
    {
        // Arrange
        DocumentMetadata parent = new()
        {
            TypeId = "parent",
            Directories = ["dir1", "dir2"]
        };
        DocumentMetadata child = new()
        {
            TypeId = "child",
            Parent = parent
        };

        // Act
        string result = child.GetFullPath();

        // Assert
        string expected = Path.Combine("dir1", "dir2", "c-child.json");
        Assert.Equal(expected, result);
    }

    [Fact]
    public void GetFullPath_WhenChildHasTwoDirectories_IncludesAllDirectories()
    {
        // Arrange
        DocumentMetadata parent = new()
        {
            TypeId = "parent",
            Directories = ["parentdir"]
        };
        DocumentMetadata child = new()
        {
            TypeId = "child",
            Parent = parent,
            Directories = ["dir1", "dir2"]
        };

        // Act
        string result = child.GetFullPath();

        // Assert
        string expected = Path.Combine("parentdir", "dir1", "dir2", "m-child.json");
        Assert.Equal(expected, result);
    }
}