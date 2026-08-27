using System;

namespace VideoOS.Server;

public interface IEvidenceLockUpdateConfig
{
	TimeSpan TimeBetweenUpdateAllSizes { get; }

	TimeSpan TimeBetweenUpdateOfUnknownSizes { get; }

	TimeSpan TimeBetweenBatches { get; }

	int BatchSize { get; }

	TimeSpan TimeBetweenEvidenceLocks { get; }
}
