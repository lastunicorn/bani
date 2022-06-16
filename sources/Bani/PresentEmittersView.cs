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
using Bani.Application.PresentEmitters;
using DustInTheWind.Bani.Domain;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.Bani
{
    internal class PresentEmittersView
    {
        public void Display(PresentEmittersResponse response)
        {
            foreach (Emitter emitter in response.Emitters)
                DisplayEmitter(emitter);
        }

        private static void DisplayEmitter(Emitter emitter)
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