using Orquestration.Saga.Contracts.Commands;
using System.Threading;
using System.Threading.Tasks;

namespace Orquestration.Saga.Publishers.Contracts
{
    public interface IAnalyzeBankDepositTransactionPublisher
    {
        Task Publish(IAnalyzeBankDepositTransactionRequest request);
    }
}
