namespace Ranksterr.Infrastructure.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public DateTime OccurredOnUtc { get; set; }
    public string Type { get; set; }
    public string Content { get; set; }
    public DateTime? ProcessedOnUtc { get; set; }
    public string? Error { get; set; }
    
    public OutboxMessage(Guid id, DateTime occuredOnUtc, string type, string content)
    {
        Id = id;
        OccurredOnUtc = occuredOnUtc;
        Content = content;
        Type = type;
    }
}