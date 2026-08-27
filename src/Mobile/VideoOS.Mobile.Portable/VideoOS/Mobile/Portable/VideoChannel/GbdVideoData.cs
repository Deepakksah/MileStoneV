using System;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Binary;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.VideoChannel;

public class GbdVideoData : VideoDataRaw, IGbdVideoData, IVideoDataRaw, IStreamData, IRawData
{
	private const int GbdHeaderLength = 32;

	public const int GbdCodecH264 = 10;

	public const int GbdCodecJpeg = 1;

	public const int GbdCodecH265 = 14;

	public const int GbdCodecMxpeg = 128;

	private DateTime _nextFrameTimeUtc;

	public DateTime SyncTimeUtc { get; set; }

	public DateTime PicTimeUtc { get; set; }

	public DateTime NextFrameTimeUtc
	{
		get
		{
			return _nextFrameTimeUtc;
		}
		set
		{
			_nextFrameTimeUtc = value;
			if (_nextFrameTimeUtc >= PicTimeUtc)
			{
				Duration = (uint)(_nextFrameTimeUtc - PicTimeUtc).TotalMilliseconds;
			}
		}
	}

	public uint Duration { get; set; }

	public bool IsSyncFrame { get; set; }

	public GbdHeader GbdHeader { get; private set; }

	public ByteBuffer ByteBuffer { get; private set; }

	private void AssignFrame(byte[] frame)
	{
		if (frame == null || frame.Length < 32)
		{
			return;
		}
		try
		{
			ByteBuffer = new ByteBuffer(frame, 32, frame.Length - 32);
			byte[] array = new byte[32];
			Buffer.BlockCopy(frame, 0, array, 0, 32);
			GbdHeader gbdHeader = BinaryFrameHeaderHelper.Instance.ReadStruct<GbdHeader>(array);
			if (BitConverter.IsLittleEndian)
			{
				gbdHeader.CodecType = EndiansInvertor.Invert(gbdHeader.CodecType);
				gbdHeader.Flags = EndiansInvertor.Invert(gbdHeader.Flags);
				gbdHeader.TimestampSync = EndiansInvertor.Invert(gbdHeader.TimestampSync);
				gbdHeader.TimestampPic = EndiansInvertor.Invert(gbdHeader.TimestampPic);
			}
			IsSyncFrame = 1 == (gbdHeader.Flags & 1);
			SyncTimeUtc = TimeConverter.FromLong((long)gbdHeader.TimestampSync);
			PicTimeUtc = TimeConverter.FromLong((long)gbdHeader.TimestampPic);
			GbdHeader = gbdHeader;
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "GBDVideoData.AssignFrame", ex.Message);
		}
	}

	public GbdVideoData(byte[] gbdFrame)
	{
		AssignFrame(gbdFrame);
	}

	public static GbdVideoData FromVideoData(IVideoData vd)
	{
		return new GbdVideoData(vd.Content)
		{
			BeginTimeUtc = vd.BeginTimeUtc,
			EndTimeUtc = vd.EndTimeUtc,
			NextFrameTimeUtc = vd.EndTimeUtc,
			StreamType = StreamParamsHelper.StreamType.Unknown,
			RawVideo = new RawVideoData
			{
				PrivacyMask = vd.RawVideo?.PrivacyMask
			},
			Disposer = vd.Disposer
		};
	}
}
