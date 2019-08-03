using System;
using System.Collections.Generic;
using Automatonymous;

namespace Orquestration.Saga.Common
{
    public abstract class AcessoSagaStateInstance : SagaStateMachineInstance
    {
        protected AcessoSagaStateInstance() { Events = new List<IEventMessage>(); }

        public Guid CorrelationId { get; set; }
        public State State { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IList<IEventMessage> Events { get; set; }
    }
}
