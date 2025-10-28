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

using System.IO;
using System.Reflection;
using Autofac;
using DustInTheWind.Bani.Adapters.DataAccess;
using DustInTheWind.Bani.Cli.Application.PresentIssuers;
using DustInTheWind.Bani.Ports.DataAccess;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.Extensions.Configuration;

namespace DustInTheWind.Bani;

internal class Setup
{
    public static void ConfigureServices(ContainerBuilder containerBuilder)
    {
        containerBuilder
            .Register(builder =>
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                return configuration;
            })
            .AsSelf()
            .SingleInstance();

        Assembly applicationAssembly = typeof(PresentIssuersRequest).Assembly;
        MediatRConfiguration mediatRConfiguration = MediatRConfigurationBuilder.Create("", applicationAssembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .WithRegistrationScope(RegistrationScope.Scoped) // currently only supported values are `Transient` and `Scoped`
            .Build();
        containerBuilder.RegisterMediatR(mediatRConfiguration);

        RegisterPortAdapters(containerBuilder);
    }

    private static void RegisterPortAdapters(ContainerBuilder containerBuilder)
    {
        containerBuilder
            .Register(context =>
            {
                IConfiguration configuration = context.Resolve<IConfiguration>();
                string databasePath = configuration.GetConnectionString("DefaultConnection");
                return new UnitOfWork(databasePath);
            })
            .As<IUnitOfWork>()
            .InstancePerLifetimeScope();
    }
}