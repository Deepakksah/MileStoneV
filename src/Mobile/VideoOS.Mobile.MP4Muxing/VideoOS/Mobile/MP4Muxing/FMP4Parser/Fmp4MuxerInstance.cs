using System.Collections.Generic;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class Fmp4MuxerInstance : IMp4Muxer, IControl
{
	private readonly Dictionary<uint, IMp4MuxerCodecImpl> _codecImplementers = new Dictionary<uint, IMp4MuxerCodecImpl>();

	public Fmp4MuxerInstance(IReadOnlyCollection<IMp4MuxerCodecImpl> codecImplementers)
	{
		foreach (IMp4MuxerCodecImpl codecImplementer in codecImplementers)
		{
			_codecImplementers.Add(codecImplementer.CodecType, codecImplementer);
		}
	}

	public void Start()
	{
	}

	public void Stop()
	{
	}

	public byte[] Mux(IReadOnlyList<GbdVideoData> h264Input, Fmp4FragmentInfo fmp4FragmentInfo)
	{
		if (!_codecImplementers.ContainsKey(fmp4FragmentInfo.CodecType))
		{
			return null;
		}
		ValidateFmp4FragmentInfo(h264Input, fmp4FragmentInfo);
		return Fmp4Muxer.Create(h264Input, fmp4FragmentInfo, _codecImplementers[fmp4FragmentInfo.CodecType]);
	}

	public void ValidateFmp4FragmentInfo(IReadOnlyList<GbdVideoData> h264Input, Fmp4FragmentInfo fmp4FragmentInfo)
	{
		if (fmp4FragmentInfo.TimeScaleInFmp4 == 0)
		{
			fmp4FragmentInfo.TimeScaleInFmp4 = 1000u;
		}
		if (fmp4FragmentInfo.TimeScaleInFmp4 == 0 && h264Input.Count != 0)
		{
			double num = fmp4FragmentInfo.WholeSegmentDuration.TotalSeconds / (double)h264Input.Count;
			int num2 = 1000;
			fmp4FragmentInfo.TimeScaleInFmp4 = (uint)((double)num2 / num);
		}
		if (fmp4FragmentInfo.TimeScaleInFmp4 == 0)
		{
			fmp4FragmentInfo.TimeScaleInFmp4 = 30303u;
		}
	}
}
