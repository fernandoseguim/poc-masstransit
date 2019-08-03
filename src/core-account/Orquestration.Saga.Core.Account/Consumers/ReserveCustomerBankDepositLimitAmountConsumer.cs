using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Orquestration.Saga.Contracts.Commands;
using Orquestration.Saga.Contracts.Events;
using Orquestration.Saga.Core.Account.Events;

namespace Orquestration.Saga.Core.Account.Consumers
{
    public class MoveReservedLimitAmountToRegularAccountConsumer : IConsumer<IMoveReservedLimitAmountToRegularAccount>
    {
        private readonly ILogger<MoveReservedLimitAmountToRegularAccountConsumer> _logger;
        public MoveReservedLimitAmountToRegularAccountConsumer(ILogger<MoveReservedLimitAmountToRegularAccountConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IMoveReservedLimitAmountToRegularAccount> context)
        {
            _logger.LogInformation($"Request to move reserved limit to regular account to {context.Message.CorrelationId} was received");
            await context.Publish<IReservedLimitAmountWasMovedToRegularAccount>(new ReservedLimitAmountWasMovedToRegularAccount(context.Message.CorrelationId));
        }
    }
}
