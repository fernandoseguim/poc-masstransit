using Orquestration.Saga.Contracts.Events;
using System.Threading;
using System.Threading.Tasks;

namespace Orquestrador.Saga.Entrypoint.Publishers
{
    public interface IBankDepositTransactionPublisher
    {
        Task Publish(IBankDepositTransactionWasReceived @event, CancellationToken cancellationToken);
    }
}