using Microsoft.AspNetCore.Mvc;
using Orquestrador.Saga.Entrypoint.Events;
using Orquestrador.Saga.Entrypoint.Publishers;
using System.Threading;
using System.Threading.Tasks;

namespace Orquestrador.Saga.Entrypoint.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    public class SpbController : ControllerBase
    {
        private readonly IBankDepositTransactionPublisher _publisher;

        public SpbController(IBankDepositTransactionPublisher publisher)
        {
            _publisher = publisher;
        }
        
        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] BankDepositTransactionWasReceived value, CancellationToken cancellationToken)
        {
            await _publisher.Publish(value, cancellationToken);
        }
    }
}
