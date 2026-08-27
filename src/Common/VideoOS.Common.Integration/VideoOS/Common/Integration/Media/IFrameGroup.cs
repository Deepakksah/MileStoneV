using System;

namespace VideoOS.Common.Integration.Media;

public interface IFrameGroup
{
	int FrameCount { get; }

	DateTime BeginTimestamp { get; }

	DateTime EndTimestamp { get; }

	IFrame GetFrame(int index);
}
