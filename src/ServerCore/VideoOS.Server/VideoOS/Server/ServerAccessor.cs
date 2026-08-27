namespace VideoOS.Server;

public static class ServerAccessor
{
	public static IServerConfig ServerConfig => VideoOS.Server.ServerConfig.Instance;

	public static IServerServiceSupport Instance => Server.Instance;
}
