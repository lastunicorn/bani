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

using System;
using System.Collections.Generic;

namespace DustInTheWind.Bani.Infrastructure;

public class EventBus
{
    private readonly Dictionary<Type, List<object>> subscribers = new();

    public void Subscribe<TEvent>(Action<TEvent> action)
    {
        List<object> actions;

        if (subscribers.ContainsKey(typeof(TEvent)))
            actions = subscribers[typeof(TEvent)];
        else
        {
            actions = new List<object>();
            subscribers.Add(typeof(TEvent), actions);
        }

        actions.Add(action);
    }

    public void Publish<TEvent>(TEvent @event)
    {
        if (subscribers.ContainsKey(typeof(TEvent)))
        {
            List<object> actions = subscribers[typeof(TEvent)];

            foreach (Action<TEvent> action in actions)
                action(@event);
        }
    }
}