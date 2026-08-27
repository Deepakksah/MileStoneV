namespace VideoOS.Server.Admin.Utilities;

public static class InstallerRegistry
{
	public static class Values
	{
		public const string MajorVersionName = "MajorVersion";

		public const string ClientVersionConfigPathName = "ClientVersionConfigPath";

		public const string ExternalPluginPathName = "ExternalPluginPath";

		public const string PathWWWRoot = "PathWWWRoot";
	}

	public const string IISSubKey = "SOFTWARE\\Microsoft\\InetStp";

	public const string ServerInstallationSubKey = "SOFTWARE\\VideoOS\\Server\\Installation";
}
