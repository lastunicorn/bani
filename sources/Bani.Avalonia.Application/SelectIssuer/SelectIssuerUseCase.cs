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
using DustInTheWind.RequestR;

namespace DustInTheWind.Bani.Avalonia.Application.SelectIssuer;

internal class SelectIssuerUseCase : IUseCase<SelectIssuerRequest>
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

    public Task Execute(SelectIssuerRequest request, CancellationToken cancellationToken)
    {
        Issuer issuer = unitOfWork.IssuerRepository.GetById(request.ItemId);

        if (issuer != null)
        {
            applicationState.CurrentIssuer = issuer;
            RaiseCurrentItemChangedEvent(issuer);
        }
        else
        {
            Emission emission = unitOfWork.EmissionRepository.GetById(request.ItemId);

            if (emission != null)
            {
                applicationState.CurrentEmission = emission;
                RaiseCurrentItemChangedEvent(emission);
            }
            else
            {
                applicationState.RemoveCurrent();
                RaiseCurrentItemChangedEvent();
            }
        }

        return Task.CompletedTask;
    }

    private void RaiseCurrentItemChangedEvent(Issuer issuer)
    {
        CurrentItemChangedEvent ev = new()
        {
            CurrentItem = issuer,
            ItemType = ItemType.Issuer
        };

        eventBus.Publish(ev);
    }

    private void RaiseCurrentItemChangedEvent(Emission emission)
    {
        CurrentItemChangedEvent ev = new()
        {
            CurrentItem = emission,
            ItemType = ItemType.Emission
        };

        eventBus.Publish(ev);
    }

    private void RaiseCurrentItemChangedEvent()
    {
        CurrentItemChangedEvent ev = new()
        {
            ItemType = ItemType.None
        };

        eventBus.Publish(ev);
    }
}