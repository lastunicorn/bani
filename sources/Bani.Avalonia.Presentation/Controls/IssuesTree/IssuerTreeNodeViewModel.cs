// Bani
// Copyright (C) 2022-2025 Dust in the Wind
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

using System.Collections.ObjectModel;
using DustInTheWind.Bani.Avalonia.Application.PresentIssuesTree;
using DustInTheWind.Bani.Avalonia.Presentation.Infrastructure;

namespace DustInTheWind.Bani.Avalonia.Presentation.Controls.IssuesTree;

public class IssuerTreeNodeViewModel : ViewModelBase
{
    public string Id { get; }
    
    public string Name { get; }
    
    public string DisplayText { get; }
    
    public TreeNodeType NodeType { get; }
    
    public ObservableCollection<EmissionTreeNodeViewModel> Emissions { get; }
    
    public bool IsExpanded { get; set; } = true;

    public IssuerTreeNodeViewModel(IssuerTreeNodeInfo issueInfo)
    {
        ArgumentNullException.ThrowIfNull(issueInfo);

        Id = issueInfo.Id;
        Name = issueInfo.Name;
        DisplayText = issueInfo.DisplayText;
        NodeType = issueInfo.NodeType;

        IEnumerable<EmissionTreeNodeViewModel> emissionViewModels = issueInfo.Emissions
            .Select(x => new EmissionTreeNodeViewModel(x));

        Emissions = new ObservableCollection<EmissionTreeNodeViewModel>(emissionViewModels);
    }
}