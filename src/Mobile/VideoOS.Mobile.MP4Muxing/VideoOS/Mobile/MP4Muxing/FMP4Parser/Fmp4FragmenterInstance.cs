using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class Fmp4FragmenterInstance : IFmp4Fragmenter
{
	private readonly Dictionary<uint, IMp4MuxerCodecImpl> _codecImplementers = new Dictionary<uint, IMp4MuxerCodecImpl>();

	public Fmp4FragmenterInstance(IReadOnlyCollection<IMp4MuxerCodecImpl> codecImplementers)
	{
		foreach (IMp4MuxerCodecImpl codecImplementer in codecImplementers)
		{
			_codecImplementers.Add(codecImplementer.CodecType, codecImplementer);
		}
	}

	public byte[] CreateFragment(RootBox rootBox, IReadOnlyList<IGbdVideoData> listFrames, IFmp4FragmentInfo fmp4FragmentInfo)
	{
		if (!_codecImplementers.ContainsKey(fmp4FragmentInfo.CodecType))
		{
			return null;
		}
		return Fmp4Fragmenter.CreateFragment(rootBox, listFrames, fmp4FragmentInfo, _codecImplementers[fmp4FragmentInfo.CodecType]);
	}
}
