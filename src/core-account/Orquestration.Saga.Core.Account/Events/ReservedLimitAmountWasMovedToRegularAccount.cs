using System;
using Orquestration.Saga.Contracts.Events;

namespace Orquestration.Saga.Core.Account.Events
{
    public class ReservedLimitAmountWasMovedToRegularAccount : IReservedLimitAmountWasMovedToRegularAccount
    {
        public ReservedLimitAmountWasMovedToRegularAccount(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; set; }
    }
}
