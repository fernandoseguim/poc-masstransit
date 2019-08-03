using System;
using Orquestration.Saga.Contracts.Commands;

namespace Orquestration.Saga.Commands
{
    public class ReserveCustomerBankDepositLimitAmount : IReserveCustomerBankDepositLimitAmount
    {
        public Guid CorrelationId { get; set; }
    }
}
