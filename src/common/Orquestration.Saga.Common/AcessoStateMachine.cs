using System;
using System.Threading.Tasks;
using Automatonymous;

namespace Orquestration.Saga.Common
{
    public class AcessoStateMachine<TSagaState> : MassTransitStateMachine<TSagaState> where TSagaState : class, SagaStateMachineInstance
    {
        protected void UpdateSagaState(TSagaState saga, IEventMessage message)
        {
            if (saga is AcessoSagaStateInstance acessoSaga) { acessoSaga.Events.Add(message); }
            else { throw new InvalidCastException("Current saga instance not is of AcessoSagaStateInstance type"); }
        }

        protected async Task SendCommandAsync<TCommand>(TCommand command, Uri endpoint, BehaviorContext<TSagaState, IMessage> context) where TCommand : class, ICommandMessage
        {
            var sendEndpoint = await context.GetSendEndpoint(endpoint);
            await sendEndpoint.Send<TCommand>(new
            {
                CorrelationId = context.Data.CorrelationId,
                Author = "Fernando Seguim"
            });
        }
    }
}
