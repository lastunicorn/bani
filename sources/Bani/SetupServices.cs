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

using System.Reflection;
using Autofac;
using DustInTheWind.Bani.Application.PresentEmitters;
using DustInTheWind.Bani.DataAccess;
using DustInTheWind.Bani.Domain;
using MediatR.Extensions.Autofac.DependencyInjection;

namespace DustInTheWind.Bani
{
    internal class SetupServices
    {
        public static IContainer BuildContainer()
        {
            ContainerBuilder containerBuilder = new();
            ConfigureServices(containerBuilder);

            return containerBuilder.Build();
        }

        private static void ConfigureServices(ContainerBuilder containerBuilder)
        {
            Assembly assembly = typeof(PresentEmittersRequest).Assembly;
            containerBuilder.RegisterMediatR(assembly);

            containerBuilder.RegisterType<BaniDbContext>().AsSelf();
            containerBuilder.Register<BaniDbContext>(builder =>
            {
                const string dbFilePath = "/nfs/YubabaData/Alez/projects/Money/database";
                //const string dbFilePath = @"\\192.168.1.12\Data\Alez\projects\Money\database";
                //const string dbFilePath = @"c:\Temp\database";

                return new BaniDbContext(dbFilePath);
            }).AsSelf();
            containerBuilder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
        }
    }
}