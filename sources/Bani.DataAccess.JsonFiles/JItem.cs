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

        public int? Year { get; set; }

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
                Year = item.Year;
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
            if (Year != null) targetItem.Year = Year;
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