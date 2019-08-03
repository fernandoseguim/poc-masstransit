using System;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orquestration.Saga.Common;

namespace Orquestration.Saga.Core.Limit
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        private static async Task Main()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                     builder
                         .SetBasePath(Environment.CurrentDirectory)
                         .AddJsonFile("appsettings.json")
                         .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    var brokerSettings = new BrokerSettings();
                    hostContext.Configuration.GetSection("RabbitSettings").Bind(brokerSettings);

                    services.AddMassTransit(configure =>
                    {
                        configure.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configureBus =>
                        {
                            var hostBus = configureBus.Host(new Uri(brokerSettings.Host), configureHost =>
                            {
                                configureHost.Username(brokerSettings.User);
                                configureHost.Password(brokerSettings.Password);
                            });

                            configureBus.ReceiveEndpoint(hostBus, brokerSettings.InputQueue, configureEndpoint =>
                            {
                                
                            });

                            configureBus.MessageTopology.SetEntityNameFormatter(new CustomEntityFormatter("custom-saga"));
                        }));
                    });
                    services.AddMassTransitHostedService();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                     configLogging.AddConsole();
                     configLogging.AddDebug();
                })
                .UseConsoleLifetime()
                .Build();

            await host.StartAsync();
        }
    }
}
