using Automatonymous;
using Microsoft.Extensions.Logging;
using Orquestration.Saga.Common;
using Orquestration.Saga.Contracts.Events;
using Orquestration.Saga.Sagas;
using System;
using System.Threading.Tasks;
using Orquestration.Saga.Commands;
using Orquestration.Saga.Contracts.Commands;
using Orquestration.Saga.Extensions.MassTransit;
using SagaState = Automatonymous.State;

namespace Orquestration.Saga.StateMachines
{
    public sealed class BankDepositTransactionStateMachine : AcessoStateMachine<BankDepositTransactionSaga>
    {
        private readonly ILogger<BankDepositTransactionStateMachine> _logger;

        public BankDepositTransactionStateMachine(ILogger<BankDepositTransactionStateMachine> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            Name("CREATE_BANK_DEPOSIT_TRANSACTION");
            InstanceState(saga => saga.State);

            State(() => Processing);
            SubState(() => AwaitingFraudAnalyze, Processing);

            Event(() => BankDepositTransactionWasReceived, @event => @event.CorrelateById(context => context.Message.CorrelationId).SelectId(selector => selector.Message.CorrelationId));
            Event(() => BankDepositTransactionWasApproved, @event => @event.CorrelateById(context => context.Message.CorrelationId));
            //Event(() => CustomerBankDepositLimitAmountWasReserved, @event => @event.CorrelateById(context => context.Message.CorrelationId));
            //Event(() => ReservedLimitAmountWasMovedToRegularAccount, @event => @event.CorrelateById(context => context.Message.CorrelationId));
            
            Initially(When(BankDepositTransactionWasReceived)
                .Then(context => UpdateSagaState(context.Instance, context.Data))
                .Then(context => _logger.LogInformation($"Bank deposit transaction to {context.Data.CorrelationId} was received"))
                //.ThenAsync(context => TODO: Send request to anti fraud analyze)
                //.ThenAsync(context => TODO: save bank deposit into event store)
                .ThenAsync(context => SendCommandAsync<IAnalyzeBankDepositTransactionRequest>(new AnalyzeBankDepositTransactionRequest {CorrelationId = context.Data.CorrelationId}, new Uri("rabbitmq://localhost/core-antifraud-queue"), context))
                //.ThenAsync(context => this.RaiseEvent(context.Instance, BankDepositTransactionWasApproved))
                //.Finalize());
                .TransitionTo(AwaitingFraudAnalyze));

            During(AwaitingFraudAnalyze,
                When(BankDepositTransactionWasApproved)
                    .Then(context => UpdateSagaState(context.Instance, context.Data))
                    .Then(context => _logger.LogInformation($"Bank deposit transaction to {context.Data.CorrelationId} was approved"))
                    //.ThenAsync(context => TODO: Send request to reservce limit to the customer)
                    //.ThenAsync(context => TODO: append event to bank deposit into event store)
                    //.ThenAsync(context => SendCommandAsync<IReserveCustomerBankDepositLimitAmount>(new ReserveCustomerBankDepositLimitAmount(), new Uri(""), context))
                    .Finalize());
                    //.TransitionTo(AwaitingLimitReserve));

            //During(Processing, AwaitingLimitReserve,
            //    When(CustomerBankDepositLimitAmountWasReserved)
            //        .Then(context => UpdateSagaState(context.Instance, context.Data))
            //        .Then(context => _logger.LogInformation($"Bank deposit amount limit to {context.Data.CorrelationId} was reserved"))
            //        //.ThenAsync(context => TODO: Send request to move amount to regular account)
            //        //.ThenAsync(context => SendCommandAsync<IMoveReservedLimitAmountToRegularAccount>(new MoveReservedLimitAmountToRegularAccount(), new Uri(""), context))
            //        .TransitionTo(AwaitingMovingAmountToRegularAccount));

            //During(Processing, AwaitingMovingAmountToRegularAccount,
            //    When(ReservedLimitAmountWasMovedToRegularAccount)
            //        .Then(context => UpdateSagaState(context.Instance, context.Data))
            //        .Then(context => _logger.LogInformation($"Reserved limit amount to {context.Data.CorrelationId} was moved to regular account"))
            //        //.ThenAsync(context => TODO: Store Send request to reservce limit to the customer)
            //        //.ThenAsync(context => TODO: append event to bank deposit into event store)
            //        .Finalize());

            SetCompletedWhenFinalized();
        }

        public SagaState Processing { get; private set; }
        public SagaState AwaitingFraudAnalyze { get; private set; }
        //public SagaState AwaitingLimitReserve { get; private set; }
        //public SagaState AwaitingMovingAmountToRegularAccount { get; private set; }
        
        public Event<IBankDepositTransactionWasReceived> BankDepositTransactionWasReceived { get; set; }
        public Event<IBankDepositTransactionWasApproved> BankDepositTransactionWasApproved { get; set; }
        //public Event<ICustomerBankDepositLimitAmountWasReserved> CustomerBankDepositLimitAmountWasReserved { get; set; }
        //public Event<IReservedLimitAmountWasMovedToRegularAccount> ReservedLimitAmountWasMovedToRegularAccount { get; set; }
    }
}
