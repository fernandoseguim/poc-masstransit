using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using MassTransit;
using MassTransit.Saga;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Orquestration.Saga.Common;
using Orquestration.Saga.Sagas;
using Orquestration.Saga.StateMachines;

namespace Orquestration.Saga.Extensions.MassTransit
{
    [ExcludeFromCodeCoverage]
    public static class MassTransitExtension
    {
        public static Dictionary<string, string> BankDepositSaga { get; set; }

        public static void AddMassTransitWithRabbitMq(this IServiceCollection services, IConfiguration configuration)
        {
            BankDepositSaga = configuration.GetSection("SagasSettings").GetSection("BankDepositSaga").GetChildren()
                    .Select(item => new KeyValuePair<string, string>(item.Key, item.Value))
                    .ToDictionary(x => x.Key, x => x.Value);

            var brokerSettings = new BrokerSettings();
            configuration.GetSection("BrokerSettings").Bind(brokerSettings);

            services.AddSingleton<BankDepositTransactionStateMachine>();
            services.AddSingleton<ISagaRepository<BankDepositTransactionSaga>, InMemorySagaRepository<BankDepositTransactionSaga>>();

            services.AddMassTransit(configure =>
            {
                configure.AddSaga<BankDepositTransactionSaga>();
                configure.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(configureBus =>
                {
                    var host = configureBus.Host(new Uri(brokerSettings.Host), configureHost =>
                    {
                        configureHost.Username(brokerSettings.User);
                        configureHost.Password(brokerSettings.Password);
                    });

                    configureBus.ReceiveEndpoint(host, brokerSettings.InputQueue, configureEndpoint =>
                    {
                        var machine = provider.GetService<BankDepositTransactionStateMachine>();
                        var repository = provider.GetService<ISagaRepository<BankDepositTransactionSaga>>();
                        
                        configureEndpoint.StateMachineSaga(machine, repository);
                    });

                    //configureBus.UseInMemoryOutbox();
                    configureBus.UseSerilog();
                    //configureBus.MessageTopology.SetEntityNameFormatter(new CustomEntityFormatter("bankdeposit"));
                }));
            });
            
            services.AddMassTransitHostedService();
        }
    }
}
