using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[Obsolete("We need to keep class only because it is used in legacy interfaces.")]
public class StickerSizeDto
{
	public Guid DeviceId { get; set; }

	public long Size { get; set; }

	public static StickerSize CreateStickerSize(StickerSizeDto stickerInfoDto)
	{
		if (stickerInfoDto == null)
		{
			throw new ArgumentNullException("stickerInfoDto");
		}
		return new StickerSize(stickerInfoDto.DeviceId, stickerInfoDto.Size);
	}

	public static StickerSizeDto CreateStickerSizeDto(StickerSize stickerInfo)
	{
		if (stickerInfo == null)
		{
			throw new ArgumentNullException("stickerInfo");
		}
		return new StickerSizeDto
		{
			DeviceId = stickerInfo.DeviceId,
			Size = stickerInfo.Size
		};
	}
}
