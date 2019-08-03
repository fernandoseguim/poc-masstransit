using System;
using Orquestration.Saga.Contracts.Commands;

namespace Orquestration.Saga.Commands
{
    public class MoveReservedLimitAmountToRegularAccount : IMoveReservedLimitAmountToRegularAccount
    {
        public Guid CorrelationId { get; set; }
    }
}
