namespace VideoOS.Platform.Common.Logging;

public static class LogClientHandlerFactory
{
	public static ILogClientHandler Create(string culture, ServerId serverId)
	{
		return new LogClientHandler(culture, serverId);
	}
}
