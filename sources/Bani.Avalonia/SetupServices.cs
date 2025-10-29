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

using System.Reflection;
using Autofac;
using DustInTheWind.Bani.Adapters.DataAccess;
using DustInTheWind.Bani.Adapters.StateAccess;
using DustInTheWind.Bani.Avalonia.Application.PresentIssuers;
using DustInTheWind.Bani.Avalonia.Presentation.Controls.Issuers;
using DustInTheWind.Bani.Avalonia.Presentation.Controls.Main;
using DustInTheWind.Bani.Infrastructure;
using DustInTheWind.Bani.Ports.DataAccess;
using DustInTheWind.Bani.Ports.StateAccess;
using MediatR.Extensions.Autofac.DependencyInjection;
using MediatR.Extensions.Autofac.DependencyInjection.Builder;
using Microsoft.Extensions.Configuration;

namespace DustInTheWind.Bani.Avalonia;

internal static class SetupServices
{
    public static IContainer BuildContainer()
    {
        ContainerBuilder containerBuilder = new();
        ConfigureServices(containerBuilder);

        return containerBuilder.Build();
    }

    private static void ConfigureServices(ContainerBuilder containerBuilder)
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

        Assembly assembly = typeof(PresentIssuersRequest).Assembly;
        MediatRConfiguration mediatRConfiguration = MediatRConfigurationBuilder.Create("", assembly)
            .WithAllOpenGenericHandlerTypesRegistered()
            .WithRegistrationScope(RegistrationScope.Scoped) // currently only supported values are `Transient` and `Scoped`
            .Build();
        containerBuilder.RegisterMediatR(mediatRConfiguration);

        containerBuilder
            .RegisterType<EventBus>()
            .AsSelf()
            .SingleInstance();

        containerBuilder
            .RegisterType<ApplicationState>()
            .As<IApplicationState>()
            .SingleInstance();

        RegisterPortAdapters(containerBuilder);
        RegisterPresentation(containerBuilder);
    }

    private static void RegisterPresentation(ContainerBuilder containerBuilder)
    {
        containerBuilder.RegisterType<MainViewModel>().AsSelf();
        containerBuilder.RegisterType<IssuersPageViewModel>().AsSelf();
        containerBuilder.RegisterType<SelectIssueCommand>().AsSelf();
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