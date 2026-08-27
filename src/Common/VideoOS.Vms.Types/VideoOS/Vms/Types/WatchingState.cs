using System;

namespace VideoOS.Vms.Types;

[Serializable]
public enum WatchingState
{
	FatalError = 0,
	Starting = 1,
	Waiting = 2,
	Watching = 3,
	Failover = 4,
	Stopped = 5,
	HotStandbyStarting = 6,
	HotStandbyWatching = 7,
	HotStandbyActive = 8,
	HotStandbyStopping = 9,
	[NonSerialized]
	[Obsolete("Do not use this value")]
	Unknown = 255
}
