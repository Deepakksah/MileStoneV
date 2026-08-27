using System;
using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class Mp4SamplesParser
{
	private readonly Dictionary<string, Action<IBoxParse>> _processors;

	private readonly List<TrackFragmentRunBoxSample> _samples = new List<TrackFragmentRunBoxSample>();

	public IDictionary<string, Action<IBoxParse>> Processors => _processors;

	public IReadOnlyList<TrackFragmentRunBoxSample> Samples => _samples;

	public Mp4SamplesParser()
	{
		_processors = new Dictionary<string, Action<IBoxParse>> { { "trun", OnTrunBox } };
	}

	private void OnTrunBox(IBoxParse box)
	{
		if (box is TrackFragmentRunBox trackFragmentRunBox)
		{
			_samples.AddRange(trackFragmentRunBox.Samples);
		}
	}
}
