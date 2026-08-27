using System;

namespace VideoOS.Common.Integration;

public class StickerSize2
{
	public Guid DeviceId { get; set; }

	public long Size { get; set; }

	public StickerSize2(Guid deviceId)
	{
		DeviceId = deviceId;
		Size = 0L;
	}

	public StickerSize2(Guid deviceId, long size)
	{
		DeviceId = deviceId;
		Size = size;
	}
}
