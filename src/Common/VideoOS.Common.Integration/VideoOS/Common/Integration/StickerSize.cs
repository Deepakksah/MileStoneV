using System;

namespace VideoOS.Common.Integration;

[Serializable]
[Obsolete("We need to keep class only because it is used in legacy interfaces.")]
public class StickerSize
{
	public Guid DeviceId { get; set; }

	public long Size { get; set; }

	public StickerSize(Guid deviceId)
	{
		DeviceId = deviceId;
		Size = 0L;
	}

	public StickerSize(Guid deviceId, long size)
	{
		DeviceId = deviceId;
		Size = size;
	}
}
