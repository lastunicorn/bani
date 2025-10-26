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
using DustInTheWind.Bani.Domain;

namespace DustInTheWind.Bani.Adapters.DataAccess.Infrastructure;

internal class ChangeTracker
{
    private readonly Dictionary<string, ChangeEntry<Issuer>> issuerChanges = [];

    public bool HasChanges => issuerChanges.Count > 0;

    public void TrackAdd(Issuer issuer)
    {
        ArgumentNullException.ThrowIfNull(issuer);

        if (string.IsNullOrEmpty(issuer.Id))
            throw new ArgumentException("Issuer must have an Id", nameof(issuer));

        issuerChanges[issuer.Id] = new ChangeEntry<Issuer>
        {
            Entity = issuer,
            State = EntityState.Added
        };
    }

    public void TrackUpdate(Issuer issuer)
    {
        ArgumentNullException.ThrowIfNull(issuer);

        if (string.IsNullOrEmpty(issuer.Id))
            throw new ArgumentException("Issuer must have an Id", nameof(issuer));

        bool changeItemExists = issuerChanges.ContainsKey(issuer.Id);

        if (changeItemExists)
            return;

        issuerChanges[issuer.Id] = new ChangeEntry<Issuer>
        {
            Entity = issuer,
            State = EntityState.Modified
        };
    }

    public void TrackRemove(Issuer issuer)
    {
        ArgumentNullException.ThrowIfNull(issuer);
        TrackRemove(issuer.Id);
    }

    public void TrackRemove(string issuerId)
    {
        if (string.IsNullOrEmpty(issuerId))
            throw new ArgumentException("Issuer Id cannot be null or empty", nameof(issuerId));

        bool changeItemExists = issuerChanges.TryGetValue(issuerId, out ChangeEntry<Issuer> existingEntry);

        if (changeItemExists)
        {
            switch (existingEntry.State)
            {
                case EntityState.Added:
                    issuerChanges.Remove(issuerId);
                    return;

                case EntityState.Modified:
                    issuerChanges[issuerId] = new ChangeEntry<Issuer>
                    {
                        Entity = existingEntry?.Entity,
                        State = EntityState.Deleted
                    };
                    break;

                case EntityState.Deleted:
                    return;

                default:
                    break;
            }
        }

        issuerChanges[issuerId] = new ChangeEntry<Issuer>
        {
            Entity = existingEntry?.Entity,
            State = EntityState.Deleted
        };
    }

    public IEnumerable<ChangeEntry<Issuer>> GetIssuerChanges()
    {
        return issuerChanges.Values;
    }

    public void Clear()
    {
        issuerChanges.Clear();
    }
}
