using Async_Batch_Processor.Dtos;
using Async_Batch_Processor.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Async_Batch_Processor.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderBatchProcessor _batchProcessor;

        public OrdersController(IOrderBatchProcessor batchProcessor)
        {
            _batchProcessor = batchProcessor;
        }

        [HttpPost("batch")]
        public async Task<ActionResult<BatchResult>> ProcessBatch([FromBody] List<OrderRequestDto> orders, CancellationToken cancellationToken)
        {
            var result = await _batchProcessor.ProcessOrdersAsync(orders, cancellationToken);

            return Ok(result);
        }
    }
}
