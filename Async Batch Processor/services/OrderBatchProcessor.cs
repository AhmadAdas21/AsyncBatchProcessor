using Async_Batch_Processor.Dtos;
using Async_Batch_Processor.Interfaces;
using Async_Batch_Processor.Models;

namespace Async_Batch_Processor.services
{
    public class OrderBatchProcessor : IOrderBatchProcessor
    {
        private readonly IOrderProcessor _orderProcessor;

        public OrderBatchProcessor(IOrderProcessor orderProcessor)
        {
            _orderProcessor = orderProcessor;
        }

        public async Task<BatchResult> ProcessOrdersAsync(IEnumerable<OrderRequestDto> orders, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(orders);

            cancellationToken.ThrowIfCancellationRequested();

        
            List<Order> mappedOrders = orders .Select(dto => new Order
                {
                    Id = dto.OrderId,
                    Amount = dto.Amount
                }).ToList();

           
            using var semaphore = new SemaphoreSlim(5, 5);

        
            List<Task<OrderResultDto>> tasks = new();

         
            foreach (var order in mappedOrders)
            {
                Task<OrderResultDto> task = ProcessSingleOrderAsync(
                    order,
                    semaphore,
                    cancellationToken);

                tasks.Add(task);
            }

           
            OrderResultDto[] results = await Task.WhenAll(tasks);

        
            List<OrderResultDto> successful = results
                .Where(result => result.IsSuccess)
                .ToList();

            List<OrderResultDto> failed = results
                .Where(result => !result.IsSuccess)
                .ToList();

           
            return new BatchResult(successful, failed);
        }

        private async Task<OrderResultDto> ProcessSingleOrderAsync(
            Order order,
            SemaphoreSlim semaphore,
            CancellationToken cancellationToken)
        {
           
            await semaphore.WaitAsync(cancellationToken);

            try
            {
               
                await _orderProcessor.ProcessAsync(
                    order,
                    cancellationToken);

                return new OrderResultDto(
                    order.Id,
                    true,
                    null);
            }
            catch (OperationCanceledException)
                when (cancellationToken.IsCancellationRequested)
            {
                throw;
            }
            catch (Exception ex)
            {
              
                return new OrderResultDto(
                    order.Id,
                    false,
                    ex.Message);
            }
            finally
            {
              
                semaphore.Release();
            }
        }
    }
}