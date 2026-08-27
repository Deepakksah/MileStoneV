using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class Fmp4Corrector
{
	private readonly Dictionary<string, Action<IBoxParse>> _processors;

	private uint _wholeSamplesDuration;

	public IDictionary<string, Action<IBoxParse>> Processors => _processors;

	public Fmp4Corrector()
	{
		_processors = new Dictionary<string, Action<IBoxParse>>
		{
			{ "traf", OnTrackFragmentBox },
			{ "tfhd", OnTrackFragmentHeaderBox },
			{ "trun", OnTrackFragmentRunBox },
			{ "tfdt", OnTrackFragmentDecodeTimeBox }
		};
	}

	private void OnTrackFragmentBox(IBoxParse box)
	{
		if (box.Children.Count((IBoxParse bx) => bx.BoxType == "tfdt") == 0)
		{
			int index = box.Children.IndexOf(box.Children.First((IBoxParse bx) => bx.BoxType == "trun"));
			box.Children.Insert(index, new TrackFragmentDecodeTimeBox());
		}
	}

	private void OnTrackFragmentHeaderBox(IBoxParse box)
	{
		TrackFragmentHeaderBox obj = box as TrackFragmentHeaderBox;
		obj.BaseDataOffset = null;
		obj.DefaultBaseIsMoof = true;
	}

	private void OnTrackFragmentRunBox(IBoxParse box)
	{
		TrackFragmentRunBox trackFragmentRunBox = box as TrackFragmentRunBox;
		trackFragmentRunBox.DataOffset = 0u;
		trackFragmentRunBox.FinalizeChanges();
		trackFragmentRunBox.DataOffset = trackFragmentRunBox.Parent.Parent.Size + 8;
		_wholeSamplesDuration += (uint)(int)trackFragmentRunBox.Samples.Sum((TrackFragmentRunBoxSample sample) => sample.SampleDuration.GetValueOrDefault());
	}

	private void OnTrackFragmentDecodeTimeBox(IBoxParse box)
	{
		(box as TrackFragmentDecodeTimeBox).BaseMediaDecodeTime = _wholeSamplesDuration;
	}
}
