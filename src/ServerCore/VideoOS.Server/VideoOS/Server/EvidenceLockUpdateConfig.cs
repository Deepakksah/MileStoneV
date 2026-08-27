using System;
using System.Threading;
using System.Xml;
using VideoOS.Server.ConfigExtensions;

namespace VideoOS.Server;

public sealed class EvidenceLockUpdateConfig : IEvidenceLockUpdateConfig
{
	public TimeSpan TimeBetweenUpdateAllSizes { get; private set; }

	public TimeSpan TimeBetweenUpdateOfUnknownSizes { get; private set; }

	public TimeSpan TimeBetweenBatches { get; private set; }

	public int BatchSize { get; private set; }

	public TimeSpan TimeBetweenEvidenceLocks { get; private set; }

	public void Initialize(XmlElement evidenceLockUpdateElement)
	{
		if (evidenceLockUpdateElement == null)
		{
			throw new ServerConfigInvalidException("Evidence lock size update configuration is missing.");
		}
		if (!evidenceLockUpdateElement.ReadAttributeAsInt32("MinutesBetweenUpdateAllSizes", out var value))
		{
			throw new ServerConfigInvalidException("Evidence lock size update configuration, missing time between update of all sizes.");
		}
		TimeBetweenUpdateAllSizes = ((value >= 0) ? TimeSpan.FromMinutes(value) : Timeout.InfiniteTimeSpan);
		if (!evidenceLockUpdateElement.ReadAttributeAsInt32("MinutesBetweenUpdateUnknownSizes", out value))
		{
			throw new ServerConfigInvalidException("Evidence lock size update configuration, missing time between update of unknown sizes.");
		}
		TimeBetweenUpdateOfUnknownSizes = ((value >= 0) ? TimeSpan.FromMinutes(value) : Timeout.InfiniteTimeSpan);
		if (!evidenceLockUpdateElement.ReadAttributeAsInt32("UpdateBatchSize", out value))
		{
			throw new ServerConfigInvalidException("Evidence lock size update configuration, missing time between update of unknown sizes.");
		}
		if (value <= 0)
		{
			throw new ServerConfigInvalidException("Evidence lock size update configuration, batch size cannot be less than 1.");
		}
		BatchSize = value;
		if (!evidenceLockUpdateElement.ReadAttributeAsInt32("SecondsBetweenBatches", out value))
		{
			throw new ServerConfigInvalidException("Evidence lock size update configuration, missing time between batches.");
		}
		TimeBetweenBatches = ((value >= 0) ? TimeSpan.FromSeconds(value) : Timeout.InfiniteTimeSpan);
		if (!evidenceLockUpdateElement.ReadAttributeAsInt32("MillisecondsBetweenEvidenceLocks", out value))
		{
			throw new ServerConfigInvalidException("Evidence lock size update configuration, missing time between evidence locks.");
		}
		TimeBetweenEvidenceLocks = ((value >= 0) ? TimeSpan.FromMilliseconds(value) : Timeout.InfiniteTimeSpan);
	}
}
