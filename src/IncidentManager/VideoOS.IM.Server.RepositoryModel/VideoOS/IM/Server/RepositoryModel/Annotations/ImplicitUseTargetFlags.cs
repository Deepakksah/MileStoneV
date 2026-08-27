using System;

namespace VideoOS.IM.Server.RepositoryModel.Annotations;

[Flags]
public enum ImplicitUseTargetFlags
{
	Default = 1,
	Itself = 1,
	Members = 2,
	WithMembers = 3
}
