using System.Collections.Generic;
using System.IO;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public static class Fmp4Fragmenter
{
	public static byte[] CreateFragment(RootBox rootBox, IReadOnlyList<IGbdVideoData> listFrames, IFmp4FragmentInfo fmp4FragmentInfo, IMp4MuxerCodecImpl mp4MuxerCodecImplementer)
	{
		Mp4FrameGroup mp4FrameGroup = new Mp4FrameGroup(listFrames, fmp4FragmentInfo, mp4MuxerCodecImplementer);
		MovieFragmentBox moofBox = AddMovieFragmentBox(rootBox);
		AddMovieFragmentHeaderBox(moofBox, fmp4FragmentInfo.FragmenterState);
		TrackFragmentBox trafBox = AddTrackFragmentBox(moofBox);
		AddTrackFragmentHeaderBox(trafBox, fmp4FragmentInfo);
		AddTrackFragmentDecodeTimeBox(trafBox, mp4FrameGroup);
		AddTrackFragmentRunBox(trafBox, mp4FrameGroup);
		AddMediaDataBox(rootBox, mp4FrameGroup);
		byte[] array = new byte[rootBox.Size];
		rootBox.WriteToStream(new MemoryStream(array));
		return array;
	}

	public static MovieFragmentBox AddMovieFragmentBox(RootBox rootBox)
	{
		MovieFragmentBox movieFragmentBox = new MovieFragmentBox();
		rootBox.AddChild(movieFragmentBox);
		return movieFragmentBox;
	}

	public static MovieFragmentHeaderBox AddMovieFragmentHeaderBox(MovieFragmentBox moofBox, Fmp4FragmenterState fmp4FragmenterState)
	{
		MovieFragmentHeaderBox movieFragmentHeaderBox = new MovieFragmentHeaderBox();
		movieFragmentHeaderBox.SequenceNumber = ++fmp4FragmenterState.SequenceNumber;
		moofBox.AddChild(movieFragmentHeaderBox);
		return movieFragmentHeaderBox;
	}

	public static TrackFragmentBox AddTrackFragmentBox(MovieFragmentBox moofBox)
	{
		TrackFragmentBox trackFragmentBox = new TrackFragmentBox();
		moofBox.AddChild(trackFragmentBox);
		return trackFragmentBox;
	}

	public static TrackFragmentHeaderBox AddTrackFragmentHeaderBox(TrackFragmentBox trafBox, IFmp4FragmentInfo fmp4FragmentInfo)
	{
		TrackFragmentHeaderBox trackFragmentHeaderBox = new TrackFragmentHeaderBox();
		trackFragmentHeaderBox.DefaultBaseIsMoof = true;
		trackFragmentHeaderBox.TrackId = fmp4FragmentInfo.TrackId;
		trafBox.AddChild(trackFragmentHeaderBox);
		return trackFragmentHeaderBox;
	}

	public static TrackFragmentDecodeTimeBox AddTrackFragmentDecodeTimeBox(TrackFragmentBox trafBox, IMp4FrameGroup mp4FrameGroup)
	{
		TrackFragmentDecodeTimeBox trackFragmentDecodeTimeBox = new TrackFragmentDecodeTimeBox();
		trackFragmentDecodeTimeBox.BaseMediaDecodeTime = mp4FrameGroup.BaseMediaDecodeTime;
		trafBox.AddChild(trackFragmentDecodeTimeBox);
		return trackFragmentDecodeTimeBox;
	}

	public static TrackFragmentRunBox AddTrackFragmentRunBox(TrackFragmentBox trafBox, IMp4FrameGroup mp4FrameGroup)
	{
		TrackFragmentRunBox trackFragmentRunBox = new TrackFragmentRunBox();
		trackFragmentRunBox.Version = 1u;
		trafBox.AddChild(trackFragmentRunBox);
		foreach (IMp4Frame frame in mp4FrameGroup.Frames)
		{
			trackFragmentRunBox.Samples.Add(frame.TrunSample);
		}
		trackFragmentRunBox.DataOffset = 0u;
		trackFragmentRunBox.FinalizeChanges();
		trackFragmentRunBox.DataOffset = trafBox.Parent.Size + new MediaDataBox().HeaderSize;
		return trackFragmentRunBox;
	}

	public static MediaDataBox AddMediaDataBox(RootBox rootBox, IMp4FrameGroup mp4FrameGroup)
	{
		MediaDataBox mediaDataBox = new MediaDataBox();
		foreach (IMp4Frame frame in mp4FrameGroup.Frames)
		{
			foreach (GenericMultiDataBoxData data in frame.Datas)
			{
				mediaDataBox.Datas.Add(data);
			}
		}
		rootBox.AddChild(mediaDataBox);
		return mediaDataBox;
	}
}
