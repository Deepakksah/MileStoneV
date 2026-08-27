using System;

namespace VideoOS.Common.Integration.Media;

public interface IMediaResponse : IDisposable
{
	GenericGop Gop { get; }

	bool[] MotionPrFrame { get; }

	TimeSlice DtPrev { get; }

	TimeSlice DtNext { get; }

	long? RecordSeqNo { get; }

	int? MotionLevel { get; }

	ISignature Signature { get; }
}
