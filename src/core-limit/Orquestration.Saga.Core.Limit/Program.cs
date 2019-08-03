using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orquestration.Saga.Common;
using Orquestration.Saga.Contracts.Commands;
using Orquestration.Saga.Core.Limit.Consumers;
using Serilog;

namespace Orquestration.Saga.Core.Limit
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

                    services.AddSingleton<IConsumer<IReserveCustomerBankDepositLimitAmount>, ReserveCustomerBankDepositLimitAmountConsumer>();

                    services.AddMassTransit(configure =>
                    {
                        configure.AddConsumer<ReserveCustomerBankDepositLimitAmountConsumer>();

                        configure.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configureBus =>
                        {
                            var hostBus = configureBus.Host(new Uri(brokerSettings.Host), configureHost =>
                            {
                                configureHost.Username(brokerSettings.User);
                                configureHost.Password(brokerSettings.Password);
                            });

                            configureBus.ReceiveEndpoint(hostBus, brokerSettings.InputQueue, configureEndpoint =>
                            {
                                configureEndpoint.Consumer<ReserveCustomerBankDepositLimitAmountConsumer>(provider);
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
