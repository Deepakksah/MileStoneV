using System;

namespace VideoOS.Reporting.Web.Plugin.Business.Model;

public enum ServiceStatus
{
	Indeterminate = 0,
	ContinuePending = 1,
	Paused = 2,
	PausePending = 3,
	Running = 4,
	StartPending = 5,
	Stopped = 6,
	StopPending = 7,
	[NonSerialized]
	[Obsolete("Do not use this value")]
	Unknown = 255
}
