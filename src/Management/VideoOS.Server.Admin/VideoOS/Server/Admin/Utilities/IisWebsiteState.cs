namespace VideoOS.Server.Admin.Utilities;

public enum IisWebsiteState
{
	Unknown,
	Starting,
	Started,
	Stopping,
	Stopped,
	Pausing,
	Paused,
	Continuing
}
