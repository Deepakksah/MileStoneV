using System;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public enum ConnectStatus
{
	Disconnected = 1,
	Reconnecting = 2,
	Connecting = 3,
	Connected = 4,
	[NonSerialized]
	[Obsolete("Do not use this value")]
	Unknown = 255
}
