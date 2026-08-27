using VideoOS.Mobile.Portable.Utilities.Rects;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

public static class BinaryExtensions
{
	public static void Initialize(this VideoParams videoParams, VideoData data)
	{
		videoParams.SrcStrideX = data.SourceWidth;
		videoParams.SrcStrideY = data.SourceHeight;
		if (data.Transcoded != null)
		{
			videoParams.DestWidth = data.Transcoded.Width;
			videoParams.DestHeight = data.Transcoded.Height;
			if (data.Transcoded.CroppingDefined)
			{
				videoParams.SrcRect = new IntRect((int)(data.Transcoded.CroppingRectangle.X * (double)videoParams.SrcStrideX), (int)(data.Transcoded.CroppingRectangle.Y * (double)videoParams.SrcStrideY), (int)(data.Transcoded.CroppingRectangle.Width * (double)videoParams.SrcStrideX), (int)(data.Transcoded.CroppingRectangle.Height * (double)videoParams.SrcStrideY), 0);
			}
			else
			{
				videoParams.SrcRect = new IntRect(0, 0, videoParams.SrcStrideX, videoParams.SrcStrideY);
			}
			if (data.Transcoded.CustomData is JpegLiveCustomData jpegLiveCustomData && data.Transcoded.Width != 0 && data.Transcoded.Height != 0)
			{
				double num = (double)jpegLiveCustomData.DestHeight / (double)data.Transcoded.Height;
				double num2 = (double)jpegLiveCustomData.DestWidth / (double)data.Transcoded.Width;
				videoParams.ResamplingTag = (uint)(((num > num2) ? num2 : num) * (double)jpegLiveCustomData.ResamplingMantisa + 0.5);
			}
		}
		else
		{
			videoParams.DestWidth = data.SourceWidth;
			videoParams.DestHeight = data.SourceHeight;
			videoParams.SrcRect = new IntRect(0, 0, data.SourceWidth, data.SourceHeight);
		}
	}

	public static void InitFromExtended(ref MainHeader header, MainHeaderExt headerExt)
	{
		header.VideoID = headerExt.VideoID;
		header.TimeStampUtcMs = headerExt.TimeStampUtcMs;
		header.FrameCount = headerExt.FrameCount;
		header.FrameSizeBytes = headerExt.FrameSizeBytes;
		header.HeaderSizeBytes = (ushort)headerExt.HeaderSizeBytes;
		header.HeaderExtensionFlags = headerExt.HeaderExtensionFlags;
	}
}
