using System;
using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public static class Fmp4Muxer
{
	public static byte[] Create(IReadOnlyList<IGbdVideoData> listFrames, IFmp4FragmentInfo fmp4FragmentInfo, IMp4MuxerCodecImpl mp4MuxerCodecImplementer)
	{
		SampleEntry child = mp4MuxerCodecImplementer.CreateVisualSampleEntry(listFrames[0], fmp4FragmentInfo.CodecOutputParams);
		RootBox rootBox = new RootBox();
		AddFileTypeBox(rootBox);
		MovieBox movieBox = AddBox<MovieBox>(rootBox);
		AddMovieHeaderBox(movieBox, fmp4FragmentInfo);
		TrackBox trackBox = AddBox<TrackBox>(movieBox);
		AddTrackHeaderBox(trackBox, fmp4FragmentInfo);
		MediaBox mediaBox = AddBox<MediaBox>(trackBox);
		AddMediaHeaderBox(mediaBox, fmp4FragmentInfo);
		AddBox<VideoHandlerReferenceBox>(mediaBox);
		MediaInformationBox parentBox = AddBox<MediaInformationBox>(mediaBox);
		AddBox<VideoMediaHeaderBox>(parentBox);
		AddDataEntryUrlBox(AddBox<DataReferenceBox>(AddBox<DataInformationBox>(parentBox)));
		SampleTableBox parentBox2 = AddBox<SampleTableBox>(parentBox);
		AddBox<SampleDescriptionBox>(parentBox2).AddChild(child);
		AddBox<DecodingTimeToSampleBox>(parentBox2);
		AddBox<SampleToChunkBox>(parentBox2);
		AddBox<SampleSizeBox>(parentBox2);
		AddBox<ChunkOffsetBox>(parentBox2);
		AddTrackExtendsBox(AddBox<MovieExtendsBox>(movieBox), fmp4FragmentInfo);
		return Fmp4Fragmenter.CreateFragment(rootBox, listFrames, fmp4FragmentInfo, mp4MuxerCodecImplementer);
	}

	public static T AddBox<T>(Box parentBox, Action<T> initializer = null) where T : Box, new()
	{
		T val = new T();
		initializer?.Invoke(val);
		parentBox.AddChild(val);
		return val;
	}

	public static FileTypeBox AddFileTypeBox(RootBox rootBox)
	{
		return AddBox(rootBox, delegate(FileTypeBox ftypBox)
		{
			ftypBox.MajorBrand = FileTypeBox.mp42;
			ftypBox.CompatibleBrands.Add(FileTypeBox.mp41);
			ftypBox.CompatibleBrands.Add(FileTypeBox.isom);
		});
	}

	public static MovieHeaderBox AddMovieHeaderBox(MovieBox moovBox, IFmp4FragmentInfo fmp4FragmentInfo)
	{
		return AddBox(moovBox, delegate(MovieHeaderBox mvhdBox)
		{
			mvhdBox.CreationDateTime = DateTime.UtcNow;
			mvhdBox.ModificationDateTime = DateTime.UtcNow;
			mvhdBox.TimeScale = fmp4FragmentInfo.TimeScaleInFmp4;
			mvhdBox.NextTrackId = fmp4FragmentInfo.TrackId + 1;
		});
	}

	public static TrackHeaderBox AddTrackHeaderBox(TrackBox trakBox, IFmp4FragmentInfo fmp4FragmentInfo)
	{
		return AddBox(trakBox, delegate(TrackHeaderBox tkhdBox)
		{
			tkhdBox.CreationTime = DateTime.UtcNow;
			tkhdBox.ModificationTime = DateTime.UtcNow;
			tkhdBox.TrackId = fmp4FragmentInfo.TrackId;
			tkhdBox.Width = (ushort)fmp4FragmentInfo.CodecOutputParams.Width;
			tkhdBox.Height = (ushort)fmp4FragmentInfo.CodecOutputParams.Height;
			tkhdBox.TrackEnabled = true;
		});
	}

	public static MediaHeaderBox AddMediaHeaderBox(MediaBox mdiaBox, IFmp4FragmentInfo fmp4FragmentInfo)
	{
		return AddBox(mdiaBox, delegate(MediaHeaderBox mdhdBox)
		{
			mdhdBox.CreationDateTime = DateTime.UtcNow;
			mdhdBox.ModificationDateTime = DateTime.UtcNow;
			mdhdBox.TimeScale = fmp4FragmentInfo.TimeScaleInFmp4;
			mdhdBox.LanguageCode = 21956;
		});
	}

	public static DataEntryUrlBox AddDataEntryUrlBox(DataReferenceBox drefBox)
	{
		return AddBox(drefBox, delegate(DataEntryUrlBox urlBox)
		{
			urlBox.SelfContained = true;
		});
	}

	public static TrackExtendsBox AddTrackExtendsBox(MovieExtendsBox mvexBox, IFmp4FragmentInfo fmp4FragmentInfo)
	{
		return AddBox(mvexBox, delegate(TrackExtendsBox trexBox)
		{
			trexBox.TrackId = fmp4FragmentInfo.TrackId;
			trexBox.DefaultSampleDescriptionIndex = 1u;
			trexBox.DefaultSampleFlags.IsNotSyncSample = true;
		});
	}
}
