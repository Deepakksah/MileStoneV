using System;

namespace VideoOS.Server.Tasks;

public sealed class GetDevicePresetsSessionData : SessionData
{
	public string PresetXml { get; set; }

	public string DeviceId { get; set; }

	public GetDevicePresetsSessionData()
		: base(TimeSpan.FromMinutes(5.0))
	{
	}

	public static GetDevicePresetsSessionData Query(long id)
	{
		return SessionDataManager.Instance.Query(id) as GetDevicePresetsSessionData;
	}
}
