using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.Media;

public class FrameGroup : List<GenericFrame>, IFrameGroup
{
	public int FrameCount => base.Count;

	public DateTime BeginTimestamp => base[0].Timestamp;

	public DateTime EndTimestamp => base[base.Count - 1].Timestamp;

	public FrameGroup(GenericFrame firstFrame)
	{
		Add(firstFrame);
	}

	public IFrame GetFrame(int index)
	{
		return base[index];
	}

	public void DecrementReferenceCountAllFrames()
	{
		base[0].ParentGop.DecrementReferenceCount(base.Count);
	}
}
