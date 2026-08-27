using System;

namespace VideoOS.Vms.BackupRestore;

[Flags]
public enum AceFlags : uint
{
	NonInheritAce = 0u,
	ObjectInheritAce = 1u,
	ContainerInheritAce = 2u,
	NoPropagateInheritAce = 4u,
	InheritOnlyAce = 8u,
	InheritedAce = 0x10u
}
