using DustInTheWind.JFileDb.New;
using Xunit.Sdk;

namespace DustInTheWind.JFileDb.Tests.Helpers;

/// <summary>
/// Custom assertions for DocumentMetadata instances providing deep comparison functionality.
/// </summary>
internal static class DocumentMetadataAssert
{
    /// <summary>
    /// Verifies that two DocumentMetadata instances are deeply equal.
    /// Compares TypeId, Directories collection, and Children collection recursively.
    /// Order of items in collections is not considered.
    /// </summary>
    /// <param name="expected">The expected DocumentMetadata instance</param>
    /// <param name="actual">The actual DocumentMetadata instance</param>
    /// <exception cref="XunitException">Thrown when the instances are not deeply equal</exception>
    public static void DeepEqual(DocumentMetadata expected, DocumentMetadata actual)
    {
        DeepEqual(expected, actual, string.Empty);
    }

    /// <summary>
    /// Verifies that two DocumentMetadata collections are deeply equal.
    /// Compares each item in the collections using deep comparison.
    /// Order of items in collections is not considered.
    /// </summary>
    /// <param name="expected">The expected collection of DocumentMetadata instances</param>
    /// <param name="actual">The actual collection of DocumentMetadata instances</param>
    /// <exception cref="XunitException">Thrown when the collections are not deeply equal</exception>
    public static void DeepEqual(IEnumerable<DocumentMetadata> expected, IEnumerable<DocumentMetadata> actual)
    {
        DeepEqual(expected, actual, string.Empty);
    }

    private static void DeepEqual(DocumentMetadata expected, DocumentMetadata actual, string path)
    {
        string currentPath = string.IsNullOrEmpty(path) ? "Root" : path;

        if (expected == null && actual == null)
            return;

        if (expected == null)
            throw new XunitException($"Expected DocumentMetadata at {currentPath} to be null, but was not null.");

        if (actual == null)
            throw new XunitException($"Expected DocumentMetadata at {currentPath} to not be null, but was null.");

        // Compare TypeId
        if (expected.TypeId != actual.TypeId)
            throw new XunitException($"Expected TypeId at {currentPath} to be '{expected.TypeId}', but was '{actual.TypeId}'.");

        // Compare Directories (order-independent)
        DeepEqualOrderIndependent(expected.Directories, actual.Directories, $"{currentPath}.Directories");

        // Compare Children (order-independent)
        DeepEqualOrderIndependent(expected.Children, actual.Children, $"{currentPath}.Children");
    }

    private static void DeepEqual(IEnumerable<DocumentMetadata> expected, IEnumerable<DocumentMetadata> actual, string path)
    {
        DeepEqualOrderIndependent(expected, actual, path);
    }

    private static void DeepEqualOrderIndependent(IEnumerable<DocumentMetadata> expected, IEnumerable<DocumentMetadata> actual, string path)
    {
        List<DocumentMetadata> expectedList = expected?.ToList() ?? [];
        List<DocumentMetadata> actualList = actual?.ToList() ?? [];

        if (expectedList.Count != actualList.Count)
            throw new XunitException($"Expected {path} to have {expectedList.Count} items, but had {actualList.Count} items.");

        // For each expected item, find a matching item in actual list
        List<DocumentMetadata> remainingActual = new(actualList);

        for (int i = 0; i < expectedList.Count; i++)
        {
            DocumentMetadata expectedItem = expectedList[i];
            bool found = false;

            for (int j = remainingActual.Count - 1; j >= 0; j--)
            {
                try
                {
                    // Try to match this item
                    DeepEqual(expectedItem, remainingActual[j], $"{path}[expected:{i}]");
                    // If no exception was thrown, we found a match
                    remainingActual.RemoveAt(j);
                    found = true;
                    break;
                }
                catch (XunitException)
                {
                    // This item doesn't match, continue searching
                }
            }

            if (!found)
            {
                throw new XunitException($"Expected item at {path}[{i}] with TypeId '{expectedItem?.TypeId}' was not found in the actual collection.");
            }
        }
    }

    private static void DeepEqualOrderIndependent(IEnumerable<string> expected, IEnumerable<string> actual, string path)
    {
        List<string> expectedList = expected?.ToList() ?? [];
        List<string> actualList = actual?.ToList() ?? [];

        if (expectedList.Count != actualList.Count)
            throw new XunitException($"Expected {path} to have {expectedList.Count} items, but had {actualList.Count} items.");

        // Use HashSet for efficient order-independent comparison
        HashSet<string> expectedSet = new(expectedList);
        HashSet<string> actualSet = new(actualList);

        // Check for items in expected but not in actual
        HashSet<string> missingInActual = new(expectedSet);
        missingInActual.ExceptWith(actualSet);

        if (missingInActual.Count > 0)
        {
            string missing = string.Join(", ", missingInActual.Select(x => $"'{x}'"));
            throw new XunitException($"Expected {path} to contain items: {missing}, but they were not found.");
        }

        // Check for items in actual but not in expected
        HashSet<string> extraInActual = new(actualSet);
        extraInActual.ExceptWith(expectedSet);

        if (extraInActual.Count > 0)
        {
            string extra = string.Join(", ", extraInActual.Select(x => $"'{x}'"));
            throw new XunitException($"Found unexpected items in {path}: {extra}.");
        }
    }
}