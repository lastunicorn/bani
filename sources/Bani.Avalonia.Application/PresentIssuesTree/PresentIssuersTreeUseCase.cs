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
using DustInTheWind.Bani.Ports.DataAccess;
using MediatR;

namespace DustInTheWind.Bani.Avalonia.Application.PresentIssuesTree;

internal class PresentIssuersTreeUseCase : IRequestHandler<PresentIssuersTreeRequest, PresentIssuersTreeResponse>
{
    private readonly IUnitOfWork unitOfWork;

    public PresentIssuersTreeUseCase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public Task<PresentIssuersTreeResponse> Handle(PresentIssuersTreeRequest request, CancellationToken cancellationToken)
    {
        IEnumerable<Issuer> issuers = unitOfWork.IssuerRepository.GetAll();

        PresentIssuersTreeResponse response = new()
        {
            Issuers = issuers
                .OrderBy(x => x.Name)
                .Select(x => new IssuerTreeNodeInfo(x))
                .ToList()
        };

        return Task.FromResult(response);
    }
}