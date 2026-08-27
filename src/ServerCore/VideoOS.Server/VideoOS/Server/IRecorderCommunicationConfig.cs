namespace VideoOS.Server;

public interface IRecorderCommunicationConfig
{
	int MaxSimultaneousConfigRequests { get; }

	int ConnectWaitMin { get; }

	int ConnectWaitMax { get; }

	int ConnectionAliveTimeout { get; }
}
