namespace DustInTheWind.JFileDb.Tests.Helpers;

/// <summary>
/// Creates a temporary database directory in the user's temp directory for testing purposes.
/// </summary>
public class TemporaryDatabase : IDisposable
{
    private const string MainDocumentPrefix = "m-";
    private const string ChildDocumentPrefix = "c-";
    private const string Extension = ".json";

    /// <summary>
    /// Gets the full path of the temporary database directory.
    /// </summary>
    public string RootPath { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="TemporaryDatabase"/> class.
    /// Creates a unique temporary directory for the database.
    /// </summary>
    public TemporaryDatabase()
    {
        RootPath = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        Directory.CreateDirectory(RootPath);
    }

    /// <summary>
    /// Creates a main document file (m-prefix) in the root directory or specified subdirectory
    /// </summary>
    /// <param name="typeId">The type identifier for the document</param>
    /// <param name="subdirectoryPath">Optional subdirectory path relative to root</param>
    /// <returns>Full path to the created file</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="typeId"/> is null or empty.</exception>
    public string CreateMainDocument(string typeId, string subdirectoryPath = null)
    {
        if (string.IsNullOrEmpty(typeId))
            throw new ArgumentException("TypeId cannot be null or empty", nameof(typeId));

        return CreateDocumentFile(MainDocumentPrefix + typeId + Extension, subdirectoryPath);
    }

    /// <summary>
    /// Creates a child document file (c-prefix) in the root directory or specified subdirectory
    /// </summary>
    /// <param name="typeId">The type identifier for the document</param>
    /// <param name="subdirectoryPath">Optional subdirectory path relative to root</param>
    /// <returns>Full path to the created file</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="typeId"/> is null or empty.</exception>
    public string CreateChildDocument(string typeId, string subdirectoryPath = null)
    {
        if (string.IsNullOrEmpty(typeId))
            throw new ArgumentException("TypeId cannot be null or empty", nameof(typeId));

        return CreateDocumentFile(ChildDocumentPrefix + typeId + Extension, subdirectoryPath);
    }

    /// <summary>
    /// Creates a document file with custom filename
    /// </summary>
    /// <param name="fileName">The filename including extension</param>
    /// <param name="subdirectoryPath">Optional subdirectory path relative to root</param>
    /// <returns>Full path to the created file</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="fileName"/> is null or empty.</exception>
    public string CreateDocumentFile(string fileName, string subdirectoryPath = null)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException("FileName cannot be null or empty", nameof(fileName));

        string targetDirectory = string.IsNullOrEmpty(subdirectoryPath)
            ? RootPath
            : Path.Combine(RootPath, subdirectoryPath);

        Directory.CreateDirectory(targetDirectory);

        string fullPath = Path.Combine(targetDirectory, fileName);
        File.WriteAllText(fullPath, "{}");
        return fullPath;
    }

    /// <summary>
    /// Creates a subdirectory in the database directory
    /// </summary>
    /// <param name="subdirectoryPath">The subdirectory path relative to root</param>
    /// <returns>Full path to the created directory</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="subdirectoryPath"/> is null or empty.</exception>
    public string CreateSubdirectory(string subdirectoryPath)
    {
        if (string.IsNullOrEmpty(subdirectoryPath))
            throw new ArgumentException("Subdirectory path cannot be null or empty", nameof(subdirectoryPath));

        string fullPath = Path.Combine(RootPath, subdirectoryPath);
        Directory.CreateDirectory(fullPath);
        return fullPath;
    }

    /// <summary>
    /// Creates a non-JSON file for testing purposes
    /// </summary>
    /// <param name="fileName">The filename including extension</param>
    /// <param name="content">Optional file content</param>
    /// <param name="subdirectoryPath">Optional subdirectory path relative to root</param>
    /// <returns>Full path to the created file</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="fileName"/> is null or empty.</exception>
    public string CreateNonJsonFile(string fileName, string content = "test content", string subdirectoryPath = null)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException("FileName cannot be null or empty", nameof(fileName));

        string targetDirectory = string.IsNullOrEmpty(subdirectoryPath)
            ? RootPath
            : Path.Combine(RootPath, subdirectoryPath);

        Directory.CreateDirectory(targetDirectory);

        string fullPath = Path.Combine(targetDirectory, fileName);
        File.WriteAllText(fullPath, content);
        return fullPath;
    }

    /// <summary>
    /// Creates a JSON file with custom content
    /// </summary>
    /// <param name="fileName">The filename including extension</param>
    /// <param name="jsonContent">The JSON content to write</param>
    /// <param name="subdirectoryPath">Optional subdirectory path relative to root</param>
    /// <returns>Full path to the created file</returns>
    /// <exception cref="ArgumentException">Thrown when <paramref name="fileName"/> is null or empty.</exception>
    public string CreateJsonFile(string fileName, string jsonContent, string subdirectoryPath = null)
    {
        if (string.IsNullOrEmpty(fileName))
            throw new ArgumentException("FileName cannot be null or empty", nameof(fileName));

        string targetDirectory = string.IsNullOrEmpty(subdirectoryPath)
            ? RootPath
            : Path.Combine(RootPath, subdirectoryPath);

        Directory.CreateDirectory(targetDirectory);

        string fullPath = Path.Combine(targetDirectory, fileName);
        File.WriteAllText(fullPath, jsonContent);
        return fullPath;
    }

    /// <summary>
    /// Checks if a file exists in the database directory
    /// </summary>
    /// <param name="relativePath">Path relative to the database directory</param>
    /// <returns>True if the file exists</returns>
    public bool FileExists(string relativePath)
    {
        string fullPath = Path.Combine(RootPath, relativePath);
        return File.Exists(fullPath);
    }

    /// <summary>
    /// Checks if a directory exists in the database directory
    /// </summary>
    /// <param name="relativePath">Path relative to the database directory</param>
    /// <returns>True if the directory exists</returns>
    public bool DirectoryExists(string relativePath)
    {
        string fullPath = Path.Combine(RootPath, relativePath);
        return Directory.Exists(fullPath);
    }

    /// <summary>
    /// Gets the full path for a relative path within the database directory
    /// </summary>
    /// <param name="relativePath">Path relative to the database directory</param>
    /// <returns>Full path</returns>
    public string GetFullPath(string relativePath)
    {
        return Path.Combine(RootPath, relativePath);
    }

    /// <summary>
    /// Performs cleanup by deleting the temporary database directory.
    /// </summary>
    public void Dispose()
    {
        if (Directory.Exists(RootPath))
        {
            try
            {
                Directory.Delete(RootPath, true);
            }
            catch
            {
                // Ignore disposal errors - the temp directory will be cleaned up by the OS eventually
            }
        }
    }
}