using FactoryTalent.Common.Application.Clock;

namespace FactoryTalent.Common.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
