using System;

namespace VideoOS.Common.EnvironmentServices;

public interface IDateTimeService
{
	DateTime Now { get; }

	DateTime UtcNow { get; }

	DateTime Today { get; }
}
