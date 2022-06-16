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
            foreach (EmitterInfo emitterInfo in response.Emitters)
                DisplayEmitter(emitterInfo);
        }

        private static void DisplayEmitter(EmitterInfo emitterInfo)
        {
            HorizontalLine horizontalLine = new()
            {
                Margin = 0
            };
            horizontalLine.Display();

            Console.WriteLine($"{emitterInfo.Name}");
            Console.WriteLine();

            foreach (EmissionInfo emissionInfo in emitterInfo.Emissions)
                DisplayEmission(emissionInfo);
        }

        private static void DisplayEmission(EmissionInfo emissionInfo)
        {
            DataGrid dataGrid = new()
            {
                Title = $"{emissionInfo.Name} [{emissionInfo.StartYear}-{emissionInfo.EndYear}]",
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

            dataGrid.Columns.Add("Type");

            foreach (ArtifactInfo artifactInfo in emissionInfo.Artifacts)
            {
                ContentRow row = new();

                row.AddCell(artifactInfo.DisplayName);
                row.AddCell(artifactInfo.Year);
                row.AddCell(artifactInfo.IssueDate?.ToString("yyyy MM dd"));
                row.AddCell(artifactInfo.InstanceCount);
                row.AddCell(artifactInfo.ArtifactType);

                dataGrid.Rows.Add(row);
            }

            dataGrid.Display();
        }
    }
}