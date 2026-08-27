using System;

namespace VideoOS.IM.Common.Service.Properties;

[Flags]
public enum ImplicitUseTargetFlags
{
	Default = 1,
	Itself = 1,
	Members = 2,
	WithMembers = 3
}
