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

namespace DustInTheWind.Bani.Domain
{
    public class Item
    {
        public string Id { get; set; }

        public string DisplayName { get; set; }

        public float? Value { get; set; }

        public string Unit { get; set; }

        public string Substance { get; set; }

        public string Color { get; set; }
        
        public DateTime? IssueDate { get; set; }
        
        public int? Year { get; set; }

        public Picture Obverse { get; set; }
        
        public Picture Reverse { get; set; }
        
        public int InstanceCount { get; set; }
    }
}