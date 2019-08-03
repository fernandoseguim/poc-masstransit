using System;
using Orquestration.Saga.Common;

namespace Orquestration.Saga.Sagas
{
    public class BankDepositTransactionSaga : AcessoSagaStateInstance
    {
        public BankDepositTransactionSaga()
        {
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }
    }
}
