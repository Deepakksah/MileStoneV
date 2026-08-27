using System;

namespace VideoOS.Rules.Interfaces;

[Flags]
public enum RuleType
{
	None = 0,
	Event = 1,
	TimeInterval = 2,
	Timer = 4,
	RecurringTime = 8,
	All = 0xFF
}
