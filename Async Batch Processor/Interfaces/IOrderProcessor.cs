using Async_Batch_Processor.Models;

namespace Async_Batch_Processor.Interfaces
{
    public interface IOrderProcessor
    {
        Task ProcessAsync(Order order,CancellationToken cancellationToken);
    }
}
