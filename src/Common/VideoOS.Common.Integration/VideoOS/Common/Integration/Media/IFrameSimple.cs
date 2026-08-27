using System;

namespace VideoOS.Common.Integration.Media;

public interface IFrameSimple
{
	DateTime Timestamp { get; }

	DateTime SyncTimestamp { get; }

	bool IsKeyframe { get; }

	bool IsNullKeyframe { get; }

	ushort SequenceNumber { get; }

	ulong FrameDataLength { get; }
}
