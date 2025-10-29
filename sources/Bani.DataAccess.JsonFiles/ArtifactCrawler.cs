// Bani
// Copyright (C) 2022-2025 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

namespace DustInTheWind.Bani.DataAccess.JsonFiles;

internal class ArtifactCrawler<T>
    where T : JArtifact
{
    private readonly Stack<T> stack = new();

    public string ArtifactFileName { get; set; }

    public List<T> Artifacts { get; private set; }

    public void Crawl(string directoryPath)
    {
        stack.Clear();
        Artifacts = [];

        ProcessDirectory(directoryPath);
    }

    private bool ProcessDirectory(string directoryPath)
    {
        string artifactFilePath = Path.Combine(directoryPath, ArtifactFileName);
        bool artifactFileExists = File.Exists(artifactFilePath);

        if (artifactFileExists)
            ReadArtifactAndPushToStack(artifactFilePath);

        IEnumerable<string> subDirectoryPaths = Directory.EnumerateDirectories(directoryPath);

        bool subArtifactWasFound = false;

        foreach (string subDirectoryPath in subDirectoryPaths)
            subArtifactWasFound |= ProcessDirectory(subDirectoryPath);

        if (artifactFileExists)
        {
            T currentItem = stack.Pop();

            if (!subArtifactWasFound)
                Artifacts.Add(currentItem);
        }

        return subArtifactWasFound || artifactFileExists;
    }

    private void ReadArtifactAndPushToStack(string artifactFilePath)
    {
        ArtifactFile<T> artifactFile = new(artifactFilePath);
        artifactFile.Open();

        T currentArtifact = artifactFile.Artifact;

        if (stack.Count > 0)
        {
            T stackArtifact = stack.Peek();
            T newArtifact = stackArtifact.Clone() as T;
            currentArtifact.MergeInto(newArtifact);
            stack.Push(newArtifact);
        }
        else
        {
            stack.Push(currentArtifact);
        }
    }
}