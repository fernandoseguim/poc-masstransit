using MassTransit;
using Orquestration.Saga.Contracts.Events;
using Orquestration.Saga.Core.AntiFraud.Commands;
using Orquestration.Saga.Core.AntiFraud.Events;
using System.Threading.Tasks;

namespace Orquestration.Saga.Core.AntiFraud.Consumers
{
    public class AnalyzeBankDepositTransactionRequestConsumer : IAnalyzeBankDepositTransactionRequestConsumer 
    {
        //private readonly ILogger<AnalyzeBankDepositTransactionRequestConsumer> _logger;
        //public AnalyzeBankDepositTransactionRequestConsumer(ILogger<AnalyzeBankDepositTransactionRequestConsumer> logger)
        //{
        //    _logger = logger;
        //}

        public async Task Consume(ConsumeContext<IAnalyzeBankDepositTransactionRequest> context)
        {
            //_logger.LogInformation($"Request fror analyze bank deposit transaction to {context.Message.CorrelationId} was received");
            await context.Publish<IBankDepositTransactionWasApproved>(new BankDepositTransactionWasApproved(context.Message.CorrelationId) );
        }
    }

    public interface IAnalyzeBankDepositTransactionRequestConsumer : IConsumer<IAnalyzeBankDepositTransactionRequest>
    {
    }
}
