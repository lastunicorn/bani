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
using System.Threading.Tasks;
using DustInTheWind.Bani.Application.PresentIssuers;
using MediatR;

namespace DustInTheWind.Bani.Presentation
{
    public class PresentIssuersCommand
    {
        private readonly IMediator mediator;

        public string IssuerName { get; set; }

        public PresentIssuersCommand(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute()
        {
            PresentIssuersRequest request = new()
            {
                IssuerName = IssuerName
            };
            PresentIssuersResponse response = await mediator.Send(request);

            PresentIssuersView view = new();
            view.Display(response);
        }
    }
}