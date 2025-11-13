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
using DustInTheWind.RequestR;

namespace DustInTheWind.Bani.Avalonia.Application.UpdateIssuerComments;

internal class UpdateIssuerCommentsUseCase : IUseCase<UpdateIssuerCommentsRequest>
{
    private readonly IUnitOfWork unitOfWork;

    public UpdateIssuerCommentsUseCase(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task Execute(UpdateIssuerCommentsRequest request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);

        Issuer issuer = RetrieveIssuer(request);
        issuer.Comments = request.Comments;

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static void ValidateRequest(UpdateIssuerCommentsRequest request)
    {
        if (string.IsNullOrEmpty(request.IssuerId))
            throw new ArgumentException("IssuerId cannot be null or empty.", nameof(request));
    }

    private Issuer RetrieveIssuer(UpdateIssuerCommentsRequest request)
    {
        Issuer issuer = unitOfWork.IssuerRepository.GetById(request.IssuerId);

        if (issuer == null)
            throw new InvalidOperationException($"Issuer with id '{request.IssuerId}' not found.");

        return issuer;
    }
}