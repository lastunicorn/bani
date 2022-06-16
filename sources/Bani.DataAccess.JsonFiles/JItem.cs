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

namespace DustInTheWind.Bani.DataAccess.JsonFiles
{
    public abstract class JItem : ICloneable
    {
        [JsonIgnore]
        public string Location { get; set; }

        public string DisplayName { get; set; }

        public float? Value { get; set; }

        public string Unit { get; set; }

        public string Substance { get; set; }

        public string Color { get; set; }

        public DateTime? IssueDate { get; set; }

        public JPicture Obverse { get; set; }

        public JPicture Reverse { get; set; }

        public int? InstanceCount { get; set; }

        protected JItem()
        {
        }

        protected JItem(JItem item)
        {
            if (item != null)
            {
                Location = item.Location;
                DisplayName = item.DisplayName;
                Value = item.Value;
                Unit = item.Unit;
                Substance = item.Substance;
                Color = item.Color;
                IssueDate = item.IssueDate;
                Obverse = item.Obverse;
                Reverse = item.Reverse;
                InstanceCount = item.InstanceCount;
            }
        }

        public virtual void MergeInto(JItem targetItem)
        {
            if (Location != null) targetItem.Location = Location;
            if (DisplayName != null) targetItem.DisplayName = DisplayName;
            if (Value != null) targetItem.Value = Value;
            if (Unit != null) targetItem.Unit = Unit;
            if (Substance != null) targetItem.Substance = Substance;
            if (Color != null) targetItem.Color = Color;
            if (IssueDate != null) targetItem.IssueDate = IssueDate;
            if (Obverse != null) targetItem.Obverse = Obverse;
            if (Reverse != null) targetItem.Reverse = Reverse;
            if (InstanceCount != null) targetItem.InstanceCount = InstanceCount;
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
}