using System;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class TimeProvider : ITimeProvider
{
	public DateTime Now => DateTime.Now;

	public DateTime NowUtc => DateTime.UtcNow;
}
