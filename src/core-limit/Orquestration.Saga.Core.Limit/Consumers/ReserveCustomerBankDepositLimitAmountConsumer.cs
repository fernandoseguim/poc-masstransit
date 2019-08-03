using MassTransit;
using Orquestration.Saga.Contracts.Commands;
using Orquestration.Saga.Contracts.Events;
using Orquestration.Saga.Core.Limit.Events;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Orquestration.Saga.Core.Limit.Consumers
{
    public class ReserveCustomerBankDepositLimitAmountConsumer : IConsumer<IReserveCustomerBankDepositLimitAmount>
    {
        private readonly ILogger<ReserveCustomerBankDepositLimitAmountConsumer> _logger;
        public ReserveCustomerBankDepositLimitAmountConsumer(ILogger<ReserveCustomerBankDepositLimitAmountConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IReserveCustomerBankDepositLimitAmount> context)
        {
            _logger.LogInformation($"Request for reserve limit for customer to {context.Message.CorrelationId} was received");
            await context.Publish<ICustomerBankDepositLimitAmountWasReserved>(new CustomerBankDepositLimitAmountWasReserved(context.Message.CorrelationId) { ReservedAmount = 1000 } );
        }
    }
}
