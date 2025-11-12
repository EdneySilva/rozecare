using RozeCare.Application.Common.Interfaces;

namespace RozeCare.Infrastructure.Services;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime UtcNow => DateTime.UtcNow;
}
