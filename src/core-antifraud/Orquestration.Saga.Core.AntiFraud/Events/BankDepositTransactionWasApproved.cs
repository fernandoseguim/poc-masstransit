using Orquestration.Saga.Contracts.Events;
using System;

namespace Orquestration.Saga.Core.AntiFraud.Events
{
    public class BankDepositTransactionWasApproved : IBankDepositTransactionWasApproved
    {
        public BankDepositTransactionWasApproved(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }
    }
}
