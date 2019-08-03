using System;

namespace Orquestration.Saga.Common
{
    public interface IMessage
    {
        Guid CorrelationId { get; set; }
    }
}