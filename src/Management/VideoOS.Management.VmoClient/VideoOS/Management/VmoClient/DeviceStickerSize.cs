using System;

namespace VideoOS.Management.VmoClient;

public class DeviceStickerSize
{
	public Guid DeviceId { get; private set; }

	public long Size { get; private set; }

	public DeviceStickerSize(Guid deviceId, long size)
	{
		DeviceId = deviceId;
		Size = size;
	}
}
