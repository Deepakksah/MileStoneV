using System;

namespace VideoOS.Rules.Interfaces;

[Flags]
public enum DefinitionType
{
	Type = 1,
	ConditionList = 2,
	StartActionList = 4,
	StopActionList = 8
}
