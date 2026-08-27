using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class SystemConfigurationChangedDetails
{
	public FQID FQID { get; private set; }

	public ChangeType ChangeType { get; private set; }

	public SystemConfigurationChangedDetails(FQID fqid, ChangeType changeType)
	{
		ChangeType = changeType;
		FQID = fqid;
	}
}
