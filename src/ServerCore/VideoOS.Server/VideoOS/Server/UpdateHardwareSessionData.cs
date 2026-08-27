using System;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Server;

public sealed class UpdateHardwareSessionData : SessionData
{
	public HardwareSetupInformation HardwareSetupInformation { get; set; }

	public Guid HardwareId { get; set; }

	public UpdateHardwareSessionData()
		: base(TimeSpan.FromMinutes(5.0))
	{
	}

	public static UpdateHardwareSessionData Query(long id)
	{
		return SessionDataManager.Instance.Query(id) as UpdateHardwareSessionData;
	}
}
