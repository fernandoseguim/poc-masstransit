using Orquestration.Saga.Common;

namespace Orquestration.Saga.Contracts.Events
{
    public interface IBankDepositTransactionWasReceived : IEventMessage
    {
        string CashInTransactionId { get; set; }
        string Document { get; set; }
        string Company { get; set; }
        string BankBranch { get; set; }
        string BankAccount { get; set; }
        long Amount { get; set; }
        string Channel { get; set; }
    } 
}
