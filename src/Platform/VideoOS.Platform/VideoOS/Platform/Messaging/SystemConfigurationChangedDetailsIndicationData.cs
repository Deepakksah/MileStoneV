using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class SystemConfigurationChangedDetailsIndicationData
{
	[Obsolete("Do not use this member. Use DetailsList instead.")]
	public SystemConfigurationChangedDetails[] DefailList { get; }

	public SystemConfigurationChangedDetails[] DetailsList { get; }

	public SystemConfigurationChangedDetailsIndicationData(List<SystemConfigurationChangedDetails> details)
	{
		DetailsList = (DefailList = details.ToArray());
	}
}
