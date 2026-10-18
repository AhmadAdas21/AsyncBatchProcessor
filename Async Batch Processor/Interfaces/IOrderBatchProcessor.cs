using Async_Batch_Processor.Dtos;

namespace Async_Batch_Processor.Interfaces
{
    public interface IOrderBatchProcessor
    {

        Task<BatchResult> ProcessOrdersAsync(IEnumerable<OrderRequestDto> orders,CancellationToken cancellationToken);
    }  
}
