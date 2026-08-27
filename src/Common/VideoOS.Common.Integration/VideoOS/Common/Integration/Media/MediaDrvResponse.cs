using System;
using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Media;

public class MediaDrvResponse : IMediaResponse, IDisposable, ISignature
{
	public GenericGop Gop { get; set; }

	public TimeSlice DtPrev { get; set; }

	public TimeSlice DtNext { get; set; }

	public long? RecordSeqNo { get; set; }

	public bool[] MotionPrFrame { get; set; }

	public ReadOnlySharedBuffer Signature { get; set; }

	GenericGop IMediaResponse.Gop => Gop;

	TimeSlice IMediaResponse.DtPrev => DtPrev;

	TimeSlice IMediaResponse.DtNext => DtNext;

	bool[] IMediaResponse.MotionPrFrame => MotionPrFrame;

	long? IMediaResponse.RecordSeqNo => RecordSeqNo;

	int? IMediaResponse.MotionLevel => null;

	ISignature IMediaResponse.Signature
	{
		get
		{
			if (Signature == null)
			{
				return null;
			}
			return this;
		}
	}

	DateTime ISignature.SignatureTime => Gop.BeginTimestamp;

	IReadOnlyBuffer ISignature.SignatureData => Signature;

	void IDisposable.Dispose()
	{
		if (Gop != null)
		{
			Gop.DecrementReferenceCount();
			Gop = null;
		}
		if (Signature != null)
		{
			Signature.Dispose();
			Signature = null;
		}
	}
}
