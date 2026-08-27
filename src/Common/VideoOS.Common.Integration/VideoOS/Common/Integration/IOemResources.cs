using VideoOS.Common.Integration.Oem;

namespace VideoOS.Common.Integration;

public interface IOemResources
{
	IAppResources App { get; }

	IServerResources Server { get; }

	IRecorderResources Recorder { get; }

	IFailoverServerResources FailoverServer { get; }

	IManagerResources Manager { get; }

	IManagementFailoverResources ManagementFailoverServer { get; }
}
