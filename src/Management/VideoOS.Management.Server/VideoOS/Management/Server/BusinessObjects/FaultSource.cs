using System;

namespace VideoOS.Management.Server.BusinessObjects;

public enum FaultSource
{
	[Obsolete("Not used after changing evidence lock to be part of the configuration rather than created by commands")]
	RecordingServer,
	ManagementServer
}
