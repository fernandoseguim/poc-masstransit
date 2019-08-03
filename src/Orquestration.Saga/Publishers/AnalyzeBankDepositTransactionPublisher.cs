using MassTransit;
using Orquestration.Saga.Contracts.Commands;
using Orquestration.Saga.Publishers.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace Orquestration.Saga.Publishers
{
    public class AnalyzeBankDepositTransactionPublisher : IAnalyzeBankDepositTransactionPublisher
    {
        private readonly IBus _bus;
        public AnalyzeBankDepositTransactionPublisher(IBus bus)
        {
            _bus = bus;
        }

        public async Task Publish(IAnalyzeBankDepositTransactionRequest request)
        {
            await _bus.Publish<IAnalyzeBankDepositTransactionRequest>(request);
        }
    }
}
