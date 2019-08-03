using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orquestration.Saga.Common;
using Orquestration.Saga.Core.AntiFraud.Consumers;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Orquestration.Saga.Contracts.Commands;

namespace Orquestration.Saga.Core.AntiFraud
{
    [ExcludeFromCodeCoverage]
    internal static class Program
    {
        private static async Task Main()
        {
            await new HostBuilder()
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

                    services.AddSingleton<IConsumer<IAnalyzeBankDepositTransactionRequest>, AnalyzeBankDepositTransactionRequestConsumer>();
                    
                    services.AddMassTransit(configure =>
                    {
                        configure.AddConsumer<AnalyzeBankDepositTransactionRequestConsumer>();

                        configure.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configureBus =>
                        {
                            var hostBus = configureBus.Host(new Uri(brokerSettings.Host), configureHost =>
                            {
                                configureHost.Username(brokerSettings.User);
                                configureHost.Password(brokerSettings.Password);
                            });

                            configureBus.ReceiveEndpoint(hostBus, brokerSettings.InputQueue, configureEndpoint =>
                            {
                                configureEndpoint.Consumer<AnalyzeBankDepositTransactionRequestConsumer>(provider);
                            });

                            //configureBus.ConfigureEndpoints(provider);
                            configureBus.UseSerilog();
                            //configureBus.UseInMemoryOutbox();
                            //configureBus.MessageTopology.SetEntityNameFormatter(new CustomEntityFormatter("bankdeposit"));
                        }));

                        //configure.AddRequestClient<IAnalyzeBankDepositTransactionRequest>();
                    });

                    services.AddMassTransitHostedService();
                })
                .ConfigureLogging((hostContext, configLogging) =>
                {
                    configLogging
                        .AddSerilog()
                        .AddConsole()
                        .AddDebug()
                        .SetMinimumLevel(LogLevel.Debug);
                })
                .UseConsoleLifetime()
                .Build().StartAsync();

            
        }
    }
}
