using System;

namespace VideoOS.Server.Admin.Utilities;

[Flags]
public enum AccessPolicy
{
	None = 0,
	Read = 1,
	Write = 2,
	Execute = 4,
	Source = 0x10,
	Script = 0x200,
	NoRemoteWrite = 0x400,
	NoRemoteRead = 0x1000,
	NoRemoteExecute = 0x2000,
	NoRemoteScript = 0x4000
}
