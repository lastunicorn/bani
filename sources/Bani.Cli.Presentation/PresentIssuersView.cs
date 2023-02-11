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
using DustInTheWind.Bani.Cli.Application.PresentIssuers;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.Bani.Cli.Presentation
{
    internal class PresentIssuersView
    {
        public void Display(PresentIssuersResponse response)
        {
            foreach (IssuerInfo issuerInfo in response.Issuers)
                DisplayIssuer(issuerInfo);
        }

        private static void DisplayIssuer(IssuerInfo issuerInfo)
        {
            HorizontalLine horizontalLine = new()
            {
                Margin = 0
            };
            horizontalLine.Display();

            Console.WriteLine($"{issuerInfo.Name}");
            Console.WriteLine();

            foreach (EmissionInfo emissionInfo in issuerInfo.Emissions)
                DisplayEmission(emissionInfo);
        }

        private static void DisplayEmission(EmissionInfo emissionInfo)
        {
            DataGrid dataGrid = new()
            {
                Title = $"{emissionInfo.Name} [{emissionInfo.StartYear}-{emissionInfo.EndYear}]",
                TitleRow =
                {
                    ForegroundColor = ConsoleColor.Black,
                    BackgroundColor = ConsoleColor.Gray
                },
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

                if (artifactInfo.InstanceCount == 0)
                    row.ForegroundColor = ConsoleColor.DarkYellow;

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
