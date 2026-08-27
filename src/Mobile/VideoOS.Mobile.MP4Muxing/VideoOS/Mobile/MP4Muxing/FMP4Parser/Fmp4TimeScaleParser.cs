using System;
using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class Fmp4TimeScaleParser
{
	private readonly Dictionary<string, Action<IBoxParse>> _processors;

	public IDictionary<string, Action<IBoxParse>> Processors => _processors;

	public uint TimeScale { get; private set; }

	public uint SequenceNumber { get; private set; }

	public Fmp4TimeScaleParser()
	{
		_processors = new Dictionary<string, Action<IBoxParse>>
		{
			{ "mvhd", OnMovieHeaderBox },
			{ "mfhd", MovieFragmentHeaderBox }
		};
	}

	private void OnMovieHeaderBox(IBoxParse box)
	{
		MovieHeaderBox movieHeaderBox = box as MovieHeaderBox;
		TimeScale = movieHeaderBox.TimeScale;
	}

	private void MovieFragmentHeaderBox(IBoxParse box)
	{
		MovieFragmentHeaderBox movieFragmentHeaderBox = box as MovieFragmentHeaderBox;
		SequenceNumber = movieFragmentHeaderBox.SequenceNumber;
	}
}
