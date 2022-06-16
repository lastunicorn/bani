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
using System.Linq;
using DustInTheWind.Bani.DataAccess;
using DustInTheWind.Bani.Domain;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.Bani
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            //DbContext dbContext = new("/nfs/YubabaData/Alez/projects/Money/database");
            DbContext dbContext = new(@"\\192.168.1.12\Data\Alez\projects\Money\database");
            //DbContext dbContext = new(@"c:\Temp\database");

            IEmitterRepository emitterRepository = new EmitterRepository(dbContext);

            IEnumerable<Emitter> emitters = emitterRepository.GetAll();

            if (args.Length > 0)
            {
                string emitterName = args[0];

                emitters = emitters
                    .Where(x => x.Name?.Contains(emitterName, StringComparison.InvariantCultureIgnoreCase) ?? false);
            }

            foreach (Emitter emitter in emitters)
            {
                HorizontalLine horizontalLine = new()
                {
                    Margin = 0
                };
                horizontalLine.Display();

                Console.WriteLine($"{emitter.Name}");
                Console.WriteLine();

                foreach (Emission emission in emitter.Emissions)
                    DisplayEmission(emission);
            }
        }

        private static void DisplayEmission(Emission emission)
        {
            DataGrid dataGrid = new()
            {
                Title = $"{emission.Name} [{emission.StartYear}-{emission.EndYear}]",
                Border =
                {
                    Template = BorderTemplate.SingleLineBorderTemplate
                }
            };

            dataGrid.Columns.Add("Artifact");

            Column yearColumn = dataGrid.Columns.Add("Year");
            yearColumn.CellHorizontalAlignment = HorizontalAlignment.Right;

            Column issueDateColumn = dataGrid.Columns.Add("Issue Date");
            issueDateColumn.CellHorizontalAlignment = HorizontalAlignment.Right;

            Column countColumn = dataGrid.Columns.Add("Count");
            countColumn.CellHorizontalAlignment = HorizontalAlignment.Right;

            foreach (Item item in emission.Items)
            {
                ContentRow row = new();

                row.AddCell(item.DisplayName);
                row.AddCell(item.Year);
                row.AddCell(item.IssueDate?.ToString("yyyy MM dd"));
                row.AddCell(item.InstanceCount);

                dataGrid.Rows.Add(row);
            }

            dataGrid.Display();
        }
    }
}