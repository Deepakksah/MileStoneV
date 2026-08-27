using Google.Protobuf.Reflection;

namespace VideoOS.TrayController.Communication;

public enum ServiceState_Communication
{
	[OriginalName("Indeterminate")]
	Indeterminate,
	[OriginalName("ContinuePending")]
	ContinuePending,
	[OriginalName("Paused")]
	Paused,
	[OriginalName("PausePending")]
	PausePending,
	[OriginalName("Running")]
	Running,
	[OriginalName("StartPending")]
	StartPending,
	[OriginalName("Stopped")]
	Stopped,
	[OriginalName("StopPending")]
	StopPending
}
