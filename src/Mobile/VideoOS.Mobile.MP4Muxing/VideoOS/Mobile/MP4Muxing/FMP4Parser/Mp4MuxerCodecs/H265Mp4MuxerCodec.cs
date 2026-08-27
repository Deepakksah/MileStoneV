using System;
using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;
using VideoOS.Mobile.MP4Muxing.VideoParser.H265;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Mp4MuxerCodecs;

public class H265Mp4MuxerCodec : IMp4MuxerCodecImpl
{
	private readonly IH265ParamsSetParser _h265ParamParser;

	public uint CodecType => 1211250229u;

	public H265Mp4MuxerCodec()
		: this(new H265ParamSetParser())
	{
	}

	public H265Mp4MuxerCodec(IH265ParamsSetParser h265ParamParser)
	{
		_h265ParamParser = h265ParamParser;
	}

	public SampleEntry CreateVisualSampleEntry(IGbdVideoData gbdVideoData, Mp4MuxerCodecOutputParameters outputParams)
	{
		H265NaluHeaders paramSetHeader = _h265ParamParser.GetParamSetHeader(gbdVideoData);
		InitOutputParams(paramSetHeader, outputParams);
		return AddHvc1Entry(paramSetHeader);
	}

	public IList<Tuple<uint, uint>> GetStartOffsets(IGbdVideoData gbdVideoData)
	{
		return Mp4FrameHelper.GetStartOffsets(gbdVideoData, H265ParamSetParser.NaluTypeExtractor, H265ParamSetParser.IsDataNalu);
	}

	public static Hvc1Entry AddHvc1Entry(H265NaluHeaders naluHeaders)
	{
		Hvc1Entry obj = new Hvc1Entry
		{
			DataReferenceIndex = 1,
			Width = naluHeaders.Sps.Width,
			Height = naluHeaders.Sps.Height,
			CompressorNameString = "HEVC Coding"
		};
		AddHevcDecoderConfigurationBox(obj, naluHeaders);
		return obj;
	}

	public static HevcDecoderConfigurationBox AddHevcDecoderConfigurationBox(Hvc1Entry hvc1Box, H265NaluHeaders headers)
	{
		return Fmp4Muxer.AddBox(hvc1Box, delegate(HevcDecoderConfigurationBox hvcCBox)
		{
			hvcCBox.ConfigurationVersion = 1;
			hvcCBox.ProfileSpace = headers.Sps.ProfileTierLevel.ProfileTierLevelParams.ProfileSpace;
			hvcCBox.TierFlag = headers.Sps.ProfileTierLevel.ProfileTierLevelParams.TierFlag;
			hvcCBox.ProfileIdc = headers.Sps.ProfileTierLevel.ProfileTierLevelParams.ProfileIdc;
			hvcCBox.ProfileCompatibilityFlags = headers.Sps.ProfileTierLevel.ProfileTierLevelParams.ProfileCompatibilityFlags;
			hvcCBox.LevelIdc = headers.Sps.ProfileTierLevel.LevelIdc;
			hvcCBox.ChromaFormat = headers.Sps.ChromaFormatIdc;
			hvcCBox.BitDepthLumaMinus8 = headers.Sps.BitDepthLuma;
			hvcCBox.BitDepthChromaMinus8 = headers.Sps.BitDepthChroma;
			hvcCBox.ConstantFrameRateValue = HevcDecoderConfigurationBox.ConstantFrameRate.Unknown;
			hvcCBox.LengthSizeMinusOne = HevcDecoderConfigurationBox.NalUnitLenSize.FourBytes;
			hvcCBox.NalUnitArrays.Add(NalUnitArray.FromData(32, headers.Vps.Data));
			hvcCBox.NalUnitArrays.Add(NalUnitArray.FromData(33, headers.Sps.Data));
			hvcCBox.NalUnitArrays.Add(NalUnitArray.FromData(34, headers.Pps.Data));
		});
	}

	public static void InitOutputParams(H265NaluHeaders naluHeaders, Mp4MuxerCodecOutputParameters outputParams)
	{
		if (naluHeaders != null && outputParams != null)
		{
			outputParams.Width = naluHeaders.Sps.Width;
			outputParams.Height = naluHeaders.Sps.Height;
			outputParams.Profile = (uint)(naluHeaders.Sps.ProfileTierLevel.ProfileTierLevelParams.ProfileIdc | (naluHeaders.Sps.ProfileTierLevel.ProfileTierLevelParams.ProfileSpace << 8));
			outputParams.Level = (uint)(naluHeaders.Sps.ProfileTierLevel.LevelIdc | (naluHeaders.Sps.ProfileTierLevel.ProfileTierLevelParams.TierFlag << 8));
			outputParams.Constrains = naluHeaders.Sps.ProfileTierLevel.ProfileTierLevelParams.ConstraintsFlags;
			outputParams.Compatibility = naluHeaders.Sps.ProfileTierLevel.ProfileTierLevelParams.ProfileCompatibilityFlags;
		}
	}
}
