using System;
using VideoOS.Mobile.Portable.Common;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class StreamParamsHelper
{
	public enum AudioParamMask : uint
	{
		[Description("AudioEncoding")]
		AudioEncoding = 536870912u,
		[Description("AudioSamplingRate")]
		AudioSamplingRate = 1073741824u,
		[Description("AudioChannelsNumber")]
		AudioChannelsNumber = 2147483648u,
		[Description("AudioBitsPerSample")]
		AudioBitsPerSample = 0u,
		All = 3758097407u,
		RequestAudioStreamOutMandatory = 536871009u,
		RequestAudioStreamInMandatory = 3758096385u
	}

	public enum AudioEncoding : uint
	{
		[Description("Unknown")]
		Unknown,
		[Description("Pcm")]
		Pcm,
		[Description("Mp3")]
		Mp3
	}

	public enum StreamParamMask : uint
	{
		Nothing = 0u,
		[Description("ItemId")]
		ItemId = 1u,
		[Description("ComprLvl")]
		ComprLvl = 2u,
		[Description("StreamId")]
		StreamID = 4u,
		[Description("StreamDataType")]
		StreamDataType = 8u,
		[Description("StreamHeaders")]
		StreamHeaders = 16u,
		[Description("SignalType")]
		SignalType = 32u,
		[Description("MethodType")]
		MethodType = 64u,
		[Description("StreamType")]
		StreamType = 128u,
		[Description("ByteOrder")]
		ByteOrder = 256u,
		[Description("CloseConnectionOnError")]
		CloseConnectionOnError = 512u,
		Last = 512u,
		All = 1023u,
		RequestStreamMandatory = 32u,
		RequestStreamInMandatory = 32u,
		RequestStreamOutMandatory = 97u,
		RequestStreamOutNativeMandatory = 97u,
		RequestStreamOutCommonPossible = 920u,
		CreateControllerMandatory = 64u,
		GetThumnailMandatory = 1u
	}

	public enum SignalType : uint
	{
		[Description("Unknown")]
		Unknown,
		[Description("Live")]
		Live,
		[Description("Playback")]
		Playback,
		[Description("Upload")]
		Upload
	}

	public enum MethodType : uint
	{
		[Description("Unknown")]
		Unknown,
		[Description("Push")]
		Push,
		[Description("Pull")]
		Pull
	}

	public enum StreamType : uint
	{
		[Description("Unknown")]
		Unknown,
		[Description("Transcoded")]
		Transcoded,
		[Description("FragmentedMP4")]
		FragmentedMP4
	}

	public enum ByteOrder : uint
	{
		[Description("Unknown")]
		Unknown,
		[Description("LittleEndian")]
		LittleEndian,
		[Description("Network")]
		Network
	}

	[Flags]
	public enum StreamIdUsed
	{
		VideoId = 1,
		StreamId = 2,
		Both = 3
	}

	public enum StreamDataType : uint
	{
		[Description("Unknown")]
		Unknown,
		[Description("Video")]
		Video,
		[Description("Audio")]
		Audio,
		[Description("MetaData")]
		MetaData
	}

	public enum StreamHeaders : uint
	{
		[Description("Unknown")]
		Unknown,
		[Description("AllPresent")]
		AllPresent,
		[Description("NoHeaders")]
		NoHeaders
	}

	public enum VideoParamMask : uint
	{
		[Description("DestWidth")]
		DstWidth = 1024u,
		[Description("DestHeight")]
		DstHeight = 2048u,
		[Description("Fps")]
		FPS = 4096u,
		[Description("SrcLeft")]
		SrcLeft = 8192u,
		[Description("SrcTop")]
		SrcTop = 16384u,
		[Description("SrcRight")]
		SrcRight = 32768u,
		[Description("SrcBottom")]
		SrcBottom = 65536u,
		[Description("SrcStrideX")]
		SrcStrideX = 131072u,
		[Description("SrcStrideY")]
		SrcStrideY = 262144u,
		[Description("ResamplingTag")]
		ResamplingTag = 524288u,
		[Description("KeyFramesOnly")]
		KeyFramesOnly = 1048576u,
		[Description("RequestPrevNextSyncTime")]
		RequestPrevNextSyncTime = 2097152u,
		[Description("RequestMotionAndSequenceId")]
		RequestMotionAndSequenceId = 4194304u,
		[Description("RequestSignatures")]
		RequestSignatures = 8388608u,
		[Description("RequestSize")]
		RequestSize = 16777216u,
		[Description("RequiresDownsampling")]
		RequiresDownsampling = 67108864u,
		[Description("CloseOldControllers")]
		CloseOldControllers = 134217728u,
		[Description("FragmentDurationMs")]
		FragmentDurationMs = 268435456u,
		Last = 268435456u,
		All = 503316479u,
		SrcRect = 122880u,
		SrcStrides = 393216u,
		SrcAll = 516096u,
		Resampling = 524288u,
		DstStrides = 3072u,
		DstAll = 3072u,
		TranscodedMandatory = 7170u,
		RequestStreamOutTranscodedMandatory = 7267u,
		ChangeStreamMandatory = 4u,
		RequestStreamOutNativePossible = 31458297u,
		TranscodedPossible = 85060608u,
		RequestStreamOutTranscodedPossible = 85590011u,
		FragmentedPossible = 268435456u,
		RequestStreamOutFragmentedPossible = 268436473u,
		RequestStreamPossible = 368705531u,
		CreateControllerPossible = 134218048u,
		ChangeStreamPosible = 353500166u,
		GetThumnailPossible = 3075u
	}
}
