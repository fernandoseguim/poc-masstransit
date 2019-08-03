using Orquestration.Saga.Common;

namespace Orquestration.Saga.Contracts.Events
{
    public interface ICustomerBankDepositLimitAmountWasReserved : IEventMessage
    {
        long ReservedAmount { get; set; }
    }
}