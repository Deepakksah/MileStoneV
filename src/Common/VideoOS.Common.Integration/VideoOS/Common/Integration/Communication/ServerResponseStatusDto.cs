using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public enum ServerResponseStatusDto
{
	Unassigned,
	ResponseReady,
	ServerTooBusy,
	ReconnectRequired
}
