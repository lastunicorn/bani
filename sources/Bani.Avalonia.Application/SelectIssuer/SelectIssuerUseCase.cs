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

using DustInTheWind.Bani.Domain;
using DustInTheWind.Bani.Infrastructure;
using DustInTheWind.Bani.Ports.DataAccess;
using DustInTheWind.Bani.Ports.StateAccess;
using MediatR;

namespace DustInTheWind.Bani.Avalonia.Application.SelectIssuer;

internal class SelectIssuerUseCase : IRequestHandler<SelectIssuerRequest>
{
    private readonly IApplicationState applicationState;
    private readonly EventBus eventBus;
    private readonly IUnitOfWork unitOfWork;

    public SelectIssuerUseCase(IApplicationState applicationState, EventBus eventBus, IUnitOfWork unitOfWork)
    {
        this.applicationState = applicationState ?? throw new ArgumentNullException(nameof(applicationState));
        this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public Task Handle(SelectIssuerRequest request, CancellationToken cancellationToken)
    {
        Issuer issuer = unitOfWork.IssuerRepository.GetById(request.IssuerId);

        applicationState.CurrentIssuer = request.IssuerId;

        RaiseIssuerChangedEvent(issuer);

        return Task.FromResult(Unit.Value);
    }

    private void RaiseIssuerChangedEvent(Issuer issuer)
    {
        IssuerChangedEvent ev = new()
        {
            Issuer = issuer
        };

        eventBus.Publish(ev);
    }
}