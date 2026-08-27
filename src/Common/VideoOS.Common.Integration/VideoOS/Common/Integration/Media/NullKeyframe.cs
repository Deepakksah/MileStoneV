using System;

namespace VideoOS.Common.Integration.Media;

public class NullKeyframe : GenericFrame
{
	public NullKeyframe()
		: base(null, DateTime.MinValue, 0)
	{
	}
}
