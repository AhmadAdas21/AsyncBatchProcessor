using Async_Batch_Processor.Interfaces;
using Async_Batch_Processor.Models;

namespace Async_Batch_Processor.services
{
    public class OrderProcessor :IOrderProcessor
    {
        public async Task ProcessAsync(Order order,CancellationToken cancellationToken)
        {
            await Task.Delay(500, cancellationToken);
        }
       

    }
}
