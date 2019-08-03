using System;
using Orquestration.Saga.Contracts.Events;

namespace Orquestration.Saga.Core.Limit.Events
{
    public class CustomerBankDepositLimitAmountWasReserved : ICustomerBankDepositLimitAmountWasReserved
    {
        public CustomerBankDepositLimitAmountWasReserved(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }
        public long ReservedAmount { get; set; }
    }
}
