namespace Async_Batch_Processor.Dtos
{
    public record OrderResultDto(
    int OrderId,
    bool IsSuccess,
    string? ErrorMessage
);
}
