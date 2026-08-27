using System;
using System.IO;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Common.Integration;

public class ReportWebServerResources : ApplicationResources, IReportWebServerResources
{
	public string ApplicationName => GetResourceString("ReportWebServerApplicationName");

	public string LoggingDir => GetResourceString("ReportWebServerLoggingDir");

	public string ProgramFilesDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), OemResources.Instance.App.CompanyName), OemResources.Instance.Server.DefaultInstallDir, "IIS", "Reporting");

	public string ConfigFileName => "Web.config";

	public string ConfigFilePath => Path.GetFullPath(Path.Combine(ProgramFilesDataFolder, ConfigFileName));

	public string CommonApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), OemResources.Instance.App.CompanyName), OemResources.Instance.ReportWebServer.ApplicationName);

	public ReportWebServerResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
