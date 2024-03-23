using Ranksterr.Application.Abstractions.Clock;

namespace Ranksterr.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}