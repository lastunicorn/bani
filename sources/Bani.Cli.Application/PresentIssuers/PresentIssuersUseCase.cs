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
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.Bani.Domain;
using DustInTheWind.Bani.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.Bani.Cli.Application.PresentIssuers;

internal class PresentIssuersUseCase : IRequestHandler<PresentIssuersRequest, PresentIssuersResponse>
{
    private readonly IUnitOfWork unitOfWork;

    public PresentIssuersUseCase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public Task<PresentIssuersResponse> Handle(PresentIssuersRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<Issuer> issuers = string.IsNullOrEmpty(request.IssuerName)
            ? unitOfWork.IssuerRepository.GetAll()
            : unitOfWork.IssuerRepository.GetByName(request.IssuerName);

        PresentIssuersResponse response = new()
        {
            Issuers = issuers
                .Select(x => new IssuerInfo(x, request.StartYear, request.EndYear))
                .ToList()
        };

        return Task.FromResult(response);
    }
}