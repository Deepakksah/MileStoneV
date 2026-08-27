using System;
using System.IO;
using System.Reflection;
using VideoOS.Common.Integration;
using VideoOS.Server.Failover.Configurator.Properties;

namespace VideoOS.Server.Failover.Configurator;

public static class ProductLocationHelper
{
	private const string ServerConfiguratorDirectoryName = "Server Configurator";

	private const string IdpRelativePath = "IIS/IDP";

	public static string ExecutableDirectory => new Uri(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase)).LocalPath;

	private static string MilestoneDirectory => Directory.GetParent(ExecutableDirectory).FullName;

	public static string FailoverServerDirectory => Path.Combine(MilestoneDirectory, OemResources.Instance.App.FailoverManagementServerInstallDirectory);

	public static string ManagementServerDirectory => Path.Combine(MilestoneDirectory, OemResources.Instance.Server.DefaultInstallDir);

	public static string DataCollectorDirectory => Path.Combine(MilestoneDirectory, OemResources.Instance.App.DataCollectorServiceInstallDir);

	public static string IdpDirectory => Path.Combine(ManagementServerDirectory, "IIS/IDP");

	public static string ServerConfiguratorDirectory => Path.Combine(MilestoneDirectory, "Server Configurator");

	public static string CommonApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OemResources.Instance.App.CompanyName), OemResources.Instance.Server.ApplicationNameSimple);

	public static string WebConsoleIconName => OemResources.Instance.ManagementFailoverServer.FailoverManagementServerNameSimpleWithVersion + " " + Resources.FailoverWebConsoleShortcut + ".url";

	public static string GetSqlServerFolderFromDatabase(IServerAccess serverAccess)
	{
		return Directory.GetParent(serverAccess.CreateCorporateSqlAccessor().QueryCurrentDbFolder()).FullName;
	}
}
