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
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using DustInTheWind.Bani.Avalonia.Presentation.ViewModels;

namespace DustInTheWind.Bani.Avalonia
{
    public class ViewLocator : IDataTemplate
    {
        public Control Build(object data)
        {
            string name = data.GetType().FullName!.Replace("ViewModel", "View");
            Type type = Type.GetType(name);

            if (type != null)
                return (Control)Activator.CreateInstance(type)!;

            return new TextBlock
            {
                Text = "Not Found: " + name
            };
        }

        public bool Match(object data)
        {
            return data is ViewModelBase;
        }
    }
}