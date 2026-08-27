using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class DeviceMappingData
{
	public Guid DeviceId;

	public uint SourceTypes;

	public DateTime? Timestamp;

	public DeviceMappingData(Guid deviceId, uint sourceTypes, DateTime? timestamp = null)
	{
		DeviceId = deviceId;
		SourceTypes = sourceTypes;
		Timestamp = timestamp;
	}
}
