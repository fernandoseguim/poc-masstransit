using System;
using Orquestration.Saga.Contracts.Commands;

namespace Orquestration.Saga.Commands
{
    public class AnalyzeBankDepositTransactionRequest : IAnalyzeBankDepositTransactionRequest
    {
        public Guid CorrelationId { get; set; }
    }
}
