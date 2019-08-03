using System;
using Orquestration.Saga.Contracts.Events;

namespace Orquestrador.Saga.Entrypoint.Events
{
    public class BankDepositTransactionWasReceived : IBankDepositTransactionWasReceived
    {
        public BankDepositTransactionWasReceived()
        {
            CorrelationId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; set; }
        public string CashInTransactionId { get; set; }
        public string Document { get; set; }
        public string Company { get; set; }
        public string BankBranch { get; set; }
        public string BankAccount { get; set; }
        public long Amount { get; set; }
        public string Channel { get; set; }
    }
}
