using System;
using VideoOS.Platform.Primitives;

namespace VideoOS.Common.Integration.Drivers;

public class DriverBrowseMediaResponse
{
	public DriverBrowseFrameResponse[] FrameResponses { get; private set; }

	public DateTime DateTimePrevious { get; private set; }

	public DateTime DateTimeNext { get; private set; }

	public long? RecordSeqNo { get; private set; }

	public ReadOnlySharedBuffer Signature { get; private set; }

	public DriverBrowseMediaResponse(DriverBrowseFrameResponse[] frameResponses, DateTime dateTimePrevious, DateTime dateTimeNext, long? recordSeqNo, ReadOnlySharedBuffer signature)
	{
		FrameResponses = frameResponses;
		DateTimePrevious = dateTimePrevious;
		DateTimeNext = dateTimeNext;
		RecordSeqNo = recordSeqNo;
		Signature = signature;
	}
}
