using System;

namespace VideoOS.Rules.Interfaces;

public interface ITimeProvider
{
	DateTime Now { get; }

	DateTime NowUtc { get; }
}
