using System;
using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;
using VideoOS.Mobile.MP4Muxing.VideoParser.H264;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Mp4MuxerCodecs;

public class H264Mp4MuxerCodec : IMp4MuxerCodecImpl
{
	private readonly IH264ParamsSetParser _h264ParamParser;

	public uint CodecType => 1211250228u;

	public H264Mp4MuxerCodec()
		: this(new H264ParamSetParser())
	{
	}

	public H264Mp4MuxerCodec(IH264ParamsSetParser h264ParamParser)
	{
		_h264ParamParser = h264ParamParser;
	}

	public SampleEntry CreateVisualSampleEntry(IGbdVideoData gbdVideoData, Mp4MuxerCodecOutputParameters outputParams)
	{
		H264NaluHeaders paramSetHeader = _h264ParamParser.GetParamSetHeader(gbdVideoData);
		InitOutputParams(paramSetHeader, outputParams);
		return AddAvc1Entry(paramSetHeader);
	}

	public IList<Tuple<uint, uint>> GetStartOffsets(IGbdVideoData gbdVideoData)
	{
		return Mp4FrameHelper.GetStartOffsets(gbdVideoData, H264ParamSetParser.NaluTypeExtractor, H264ParamSetParser.IsDataNalu);
	}

	public static Avc1Entry AddAvc1Entry(H264NaluHeaders naluHeaders)
	{
		Avc1Entry obj = new Avc1Entry
		{
			DataReferenceIndex = 1,
			Width = naluHeaders.Sps.Width,
			Height = naluHeaders.Sps.Height,
			CompressorNameString = "AVC Coding"
		};
		AddAvcDecoderConfigurationBox(obj, naluHeaders);
		return obj;
	}

	public static AvcDecoderConfigurationBox AddAvcDecoderConfigurationBox(Avc1Entry avc1Box, H264NaluHeaders headers)
	{
		return Fmp4Muxer.AddBox(avc1Box, delegate(AvcDecoderConfigurationBox avcCBox)
		{
			avcCBox.ConfigurationVersion = 1;
			avcCBox.AvcProfileIndication = headers.Sps.ProfileIdc;
			avcCBox.AvcLevelIndication = headers.Sps.LevelIdc;
			avcCBox.NalUnitSize = 4;
			avcCBox.SpsEntryCount = 1;
			avcCBox.SpsData = headers.Sps.Data;
			avcCBox.PpsEntryCount = 1;
			avcCBox.PpsData = headers.Pps.Data;
		});
	}

	public static void InitOutputParams(H264NaluHeaders naluHeaders, Mp4MuxerCodecOutputParameters outputParams)
	{
		if (naluHeaders != null && outputParams != null)
		{
			outputParams.Width = naluHeaders.Sps.Width;
			outputParams.Height = naluHeaders.Sps.Height;
			outputParams.Profile = naluHeaders.Sps.ProfileIdc;
			outputParams.Level = naluHeaders.Sps.LevelIdc;
			outputParams.Constrains = naluHeaders.Sps.Constrains;
			outputParams.Compatibility = 0u;
		}
	}
}
