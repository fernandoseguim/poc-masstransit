using System;
using MassTransit;
using Orquestrador.Saga.Entrypoint.Events;
using System.Threading;
using System.Threading.Tasks;
using Orquestration.Saga.Contracts.Events;

namespace Orquestrador.Saga.Entrypoint.Publishers
{
    public class BankDepositTransactionPublisher : IBankDepositTransactionPublisher
    {
        private readonly IBus _bus;

        public BankDepositTransactionPublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task Publish(IBankDepositTransactionWasReceived @event, CancellationToken cancellationToken)
        {
            await _bus.Publish<IBankDepositTransactionWasReceived>(@event, cancellationToken);
        }
    }
}
