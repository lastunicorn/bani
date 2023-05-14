// Bani
// Copyright (C) 2022 Dust in the Wind
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

using System;
using System.Collections.Generic;

namespace DustInTheWind.Bani.DataAccess.JsonFiles;

public abstract class ArtifactDirectory<T>
    where T : JArtifact
{
    private readonly string directoryPath;

    protected abstract string ArtifactFileName { get; }

    public List<T> Artifacts { get; private set; }

    protected ArtifactDirectory(string directoryPath)
    {
        this.directoryPath = directoryPath ?? throw new ArgumentNullException(nameof(directoryPath));
    }

    public void Read()
    {
        ArtifactCrawler<T> artifactCrawler = new()
        {
            ArtifactFileName = ArtifactFileName
        };

        artifactCrawler.Crawl(directoryPath);
        Artifacts = artifactCrawler.Artifacts;
    }
}