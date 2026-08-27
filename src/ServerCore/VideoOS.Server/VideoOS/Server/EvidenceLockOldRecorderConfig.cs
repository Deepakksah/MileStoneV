using System;
using System.Threading;
using System.Xml;
using VideoOS.Server.ConfigExtensions;

namespace VideoOS.Server;

public sealed class EvidenceLockOldRecorderConfig : IEvidenceLockOldRecorderConfig
{
	public TimeSpan WaitTimeAfterStartup { get; private set; }

	public TimeSpan WaitTimeInCheckLoop { get; private set; }

	public void Initialize(XmlElement evidenceLockOldRecorderElement)
	{
		if (evidenceLockOldRecorderElement == null)
		{
			throw new ServerConfigInvalidException("Evidence lock on old recorder configuration is missing.");
		}
		if (!evidenceLockOldRecorderElement.ReadAttributeAsInt32("WaittimeAfterStartupSec", out var value))
		{
			throw new ServerConfigInvalidException("Evidence lock on old recorder configuration, missing wait time after startup.");
		}
		WaitTimeAfterStartup = ((value >= 0) ? TimeSpan.FromSeconds(value) : Timeout.InfiniteTimeSpan);
		if (!evidenceLockOldRecorderElement.ReadAttributeAsInt32("WaittimeInCheckLoopSec", out value))
		{
			throw new ServerConfigInvalidException("Evidence lock on old recorder configuration, missing wait time in update loop.");
		}
		WaitTimeInCheckLoop = ((value >= 0) ? TimeSpan.FromSeconds(value) : TimeSpan.FromSeconds(30.0));
	}
}
