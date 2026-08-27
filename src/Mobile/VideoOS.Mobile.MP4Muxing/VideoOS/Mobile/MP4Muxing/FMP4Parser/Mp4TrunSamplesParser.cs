using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public static class Mp4TrunSamplesParser
{
	public static IReadOnlyList<TrackFragmentRunBoxSample> Do(byte[] content)
	{
		RootBox rootBox = new RootBox();
		rootBox.InitFromBuffer(content, 0u, (uint)content.Length, BoxFactory.Mp4TrunSamplesParser);
		Mp4SamplesParser mp4SamplesParser = new Mp4SamplesParser();
		rootBox.Traverse(new BoxVisitor(mp4SamplesParser.Processors).OnBox);
		return mp4SamplesParser.Samples;
	}
}
