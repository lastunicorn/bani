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
using System.Reflection;
using Newtonsoft.Json;

namespace DustInTheWind.Bani.DataAccess.JsonFiles;

public abstract class JArtifact : ICloneable
{
    [JsonIgnore] public string Location { get; set; }

    public string DisplayName { get; set; }

    public float? Value { get; set; }

    public string Unit { get; set; }

    public string Substance { get; set; }

    public string Color { get; set; }

    public DateTime? IssueDate { get; set; }

    public JPicture Obverse { get; set; }

    public JPicture Reverse { get; set; }

    public int? InstanceCount { get; set; }

    protected JArtifact()
    {
    }

    protected JArtifact(JArtifact artifact)
    {
        if (artifact != null)
        {
            Location = artifact.Location;
            DisplayName = artifact.DisplayName;
            Value = artifact.Value;
            Unit = artifact.Unit;
            Substance = artifact.Substance;
            Color = artifact.Color;
            IssueDate = artifact.IssueDate;
            Obverse = artifact.Obverse;
            Reverse = artifact.Reverse;
            InstanceCount = artifact.InstanceCount;
        }
    }

    public virtual void MergeInto(JArtifact targetArtifact)
    {
        if (Location != null) targetArtifact.Location = Location;
        if (DisplayName != null) targetArtifact.DisplayName = DisplayName;
        if (Value != null) targetArtifact.Value = Value;
        if (Unit != null) targetArtifact.Unit = Unit;
        if (Substance != null) targetArtifact.Substance = Substance;
        if (Color != null) targetArtifact.Color = Color;
        if (IssueDate != null) targetArtifact.IssueDate = IssueDate;
        if (Obverse != null) targetArtifact.Obverse = Obverse;
        if (Reverse != null) targetArtifact.Reverse = Reverse;
        if (InstanceCount != null) targetArtifact.InstanceCount = InstanceCount;
    }

    public object Clone()
    {
        Type type = GetType();
        ConstructorInfo ctor = type.GetConstructor(new[] { type });

        if (ctor != null)
            return ctor.Invoke(new object[] { this });

        return null!;
    }
}