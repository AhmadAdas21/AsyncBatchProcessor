namespace Async_Batch_Processor.Dtos
{
    public record BatchResult
    (
        IReadOnlyList<OrderResultDto> Successful,
        IReadOnlyList<OrderResultDto> Failed
    );
}
