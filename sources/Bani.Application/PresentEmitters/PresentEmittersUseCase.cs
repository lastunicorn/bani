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
using System.Threading;
using System.Threading.Tasks;
using DustInTheWind.Bani.Domain;
using MediatR;

namespace DustInTheWind.Bani.Application.PresentEmitters
{
    internal class PresentEmittersUseCase : IRequestHandler<PresentEmittersRequest, PresentEmittersResponse>
    {
        private readonly IUnitOfWork unitOfWork;

        public PresentEmittersUseCase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<PresentEmittersResponse> Handle(PresentEmittersRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<Emitter> emitters = unitOfWork.EmitterRepository.GetAll();

            if (!string.IsNullOrEmpty(request.EmitterName))
            {
                string emitterName = request.EmitterName;

                emitters = emitters
                    .Where(x => x.Name?.Contains(emitterName, StringComparison.InvariantCultureIgnoreCase) ?? false);
            }

            PresentEmittersResponse response = new()
            {
                Emitters = emitters
                    .Select(x => new EmitterInfo(x))
                    .ToList()
            };

            return Task.FromResult(response);
        }
    }
}