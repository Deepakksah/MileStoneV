using System;
using System.Diagnostics;

namespace VideoOS.Common.Integration.Configuration;

public struct DeviceGroupMemberKey
{
	private Guid deviceGroupId;

	private Guid deviceId;

	public Guid DeviceGroupId
	{
		[DebuggerStepThrough]
		get
		{
			return deviceGroupId;
		}
	}

	public Guid Deviceid
	{
		[DebuggerStepThrough]
		get
		{
			return deviceId;
		}
	}

	public DeviceGroupMemberKey(Guid deviceGroupId, Guid deviceId)
	{
		this.deviceGroupId = deviceGroupId;
		this.deviceId = deviceId;
	}

	public override int GetHashCode()
	{
		return deviceGroupId.GetHashCode() + deviceId.GetHashCode();
	}
}
