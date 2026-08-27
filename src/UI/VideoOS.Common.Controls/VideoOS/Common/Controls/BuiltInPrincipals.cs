using System;

namespace VideoOS.Common.Controls;

[Flags]
public enum BuiltInPrincipals : uint
{
	Users = 0x80000001u,
	LocalGroups = 0x80000002u,
	GlobalGroups = 0x80000004u,
	Computers = 0x80000008u,
	World = 0x80000010u,
	AuthenticatedUser = 0x80000020u,
	Anonymous = 0x80000040u,
	Batch = 0x80000080u,
	CreatorOwner = 0x80000100u,
	CreatorGroup = 0x80000200u,
	Dialup = 0x80000400u,
	Interactive = 0x80000800u,
	Network = 0x80001000u,
	Service = 0x80002000u,
	System = 0x80004000u,
	ExcludeBuiltinGroups = 0x80008000u,
	TerminalServer = 0x80010000u,
	AllWellknownSids = 0x80020000u,
	LocalService = 0x80040000u,
	NetworkService = 0x80080000u,
	RemoteLogon = 0x80100000u,
	InternetUser = 0x80200000u,
	OwnerRights = 0x80400000u,
	Services = 0x80800000u
}
