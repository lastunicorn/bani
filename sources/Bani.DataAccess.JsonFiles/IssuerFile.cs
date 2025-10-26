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
using System.IO;
using Newtonsoft.Json;

namespace DustInTheWind.Bani.DataAccess.JsonFiles;

public class IssuerFile
{
    private readonly string filePath;

    public IssuerFile(string filePath)
    {
        this.filePath = filePath ?? throw new ArgumentNullException(nameof(filePath));
    }

    public JIssuer Issuer { get; set; }

    public void Open()
    {
        string json = File.ReadAllText(filePath);
        Issuer = JsonConvert.DeserializeObject<JIssuer>(json);
    }

    public void Save()
    {
        if (Issuer == null)
            throw new InvalidOperationException("Cannot save null issuer. Call Open() first or set the Issuer property.");

        string json = JsonConvert.SerializeObject(Issuer, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}