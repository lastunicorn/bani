using System;
using DustInTheWind.Bani.Cli.Application.PresentIssuers;
using DustInTheWind.ConsoleTools.Controls;
using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.Bani.Cli.Presentation;

internal class EmissionControl
{
    public EmissionInfo EmissionInfo { get; set; }

    public void Display()
    {
        DataGrid dataGrid = new()
        {
            Title = $"{EmissionInfo.Name} [{EmissionInfo.StartYear}-{EmissionInfo.EndYear}]",
            TitleRow =
            {
                ForegroundColor = ConsoleColor.Black,
                BackgroundColor = ConsoleColor.Gray
            },
            BorderTemplate = BorderTemplate.SingleLineBorderTemplate
        };

        dataGrid.Columns.Add("Artifact");

        Column yearColumn = dataGrid.Columns.Add("Year");
        yearColumn.CellHorizontalAlignment = HorizontalAlignment.Right;

        Column issueDateColumn = dataGrid.Columns.Add("Issue Date");
        issueDateColumn.CellHorizontalAlignment = HorizontalAlignment.Right;

        Column countColumn = dataGrid.Columns.Add("Count");
        countColumn.CellHorizontalAlignment = HorizontalAlignment.Right;

        dataGrid.Columns.Add("Type");

        foreach (ArtifactInfo artifactInfo in EmissionInfo.Artifacts)
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