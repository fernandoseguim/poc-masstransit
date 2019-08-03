using MassTransit;
using Orquestration.Saga.Contracts.Commands;
using Orquestration.Saga.Contracts.Events;
using Orquestration.Saga.Core.AntiFraud.Events;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Orquestration.Saga.Core.AntiFraud.Consumers
{
    public class AnalyzeBankDepositTransactionRequestConsumer : IConsumer<IAnalyzeBankDepositTransactionRequest>
    {
        private readonly ILogger<AnalyzeBankDepositTransactionRequestConsumer> _logger;
        public AnalyzeBankDepositTransactionRequestConsumer(ILogger<AnalyzeBankDepositTransactionRequestConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IAnalyzeBankDepositTransactionRequest> context)
        {
            _logger.LogInformation($"Request for analyze bank deposit transaction to {context.Message.CorrelationId} was received");
            await context.Publish<IBankDepositTransactionWasApproved>(new BankDepositTransactionWasApproved(context.Message.CorrelationId) );
        }
    }

    public interface IAnalyzeBankDepositTransactionRequestConsumer : IConsumer<IAnalyzeBankDepositTransactionRequest>
    {
    }
}
