using System;
using NmMediaProcessing;
using NmTimeStamp;

namespace VideoOS.Platform.MediaProcessing;

public class ByteDataHeaderAnalysisResult
{
	public enum dataType_t
	{
		video,
		audio,
		metadata
	}

	public enum subsamplingCategory_t
	{
		vital,
		important,
		unimportant,
		dontCare
	}

	public DateTime TimeStamp;

	public dataType_t DataType;

	public subsamplingCategory_t SubsamplingCategory;

	public ushort SequenceNumber;

	public bool SyncPoint;

	public ByteDataHeaderAnalysisResult()
	{
		Reset();
	}

	public void Reset()
	{
		DataType = dataType_t.video;
		SubsamplingCategory = subsamplingCategory_t.vital;
		SequenceNumber = 0;
		SyncPoint = false;
	}

	public unsafe void Set(CmByteDataHeaderAnalysisResult* native)
	{
		DateTime dateTime = new DateTime((global::_003CModule_003E.NmTimeStamp_002ECmTimeStamp_002EGetMillisecondsSinceTheEpoch((NmTimeStamp.CmTimeStamp*)native) + 62135596800000L) * 10000, DateTimeKind.Utc);
		DateTime timeStamp = dateTime;
		TimeStamp = timeStamp;
		DataType = *(dataType_t*)((ulong)(nint)native + 8uL);
		SubsamplingCategory = *(subsamplingCategory_t*)((ulong)(nint)native + 12uL);
		SequenceNumber = *(ushort*)((ulong)(nint)native + 16uL);
		SyncPoint = *(bool*)((ulong)(nint)native + 18uL);
	}
}
