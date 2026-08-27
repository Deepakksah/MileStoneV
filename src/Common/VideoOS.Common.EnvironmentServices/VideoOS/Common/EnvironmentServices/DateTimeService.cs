using System;

namespace VideoOS.Common.EnvironmentServices;

public class DateTimeService : IDateTimeService
{
	public DateTime Now => DateTime.Now;

	public DateTime UtcNow => DateTime.UtcNow;

	public DateTime Today => DateTime.Today;
}
