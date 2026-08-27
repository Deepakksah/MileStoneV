using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Management.VmoClient;
using VideoOS.Server.Failover.Configurator.Properties;
using VideoOS.Utilities.Telemetry;
using VideoOS.Utilities.Validation;

namespace VideoOS.Server.Failover.Configurator;

public static class LicenseChecker
{
	private static readonly ILogger _log = Telemetry.GetILogger("ManagementServerFailover");

	public static bool CheckIfFailoverFeatureIsLicensed(IServerAccess serverAccess, Action onNotLicensed, Action onUnauthorizedAccess, Action<VmoException> onVmoException)
	{
		serverAccess.NullCheck("", "CheckIfFailoverFeatureIsLicensed", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Server.Failover.Configurator\\LicenseHelper.cs", 27);
		onNotLicensed.NullCheck("", "CheckIfFailoverFeatureIsLicensed", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Server.Failover.Configurator\\LicenseHelper.cs", 28);
		onUnauthorizedAccess.NullCheck("", "CheckIfFailoverFeatureIsLicensed", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Server.Failover.Configurator\\LicenseHelper.cs", 29);
		onVmoException.NullCheck("", "CheckIfFailoverFeatureIsLicensed", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Server.Failover.Configurator\\LicenseHelper.cs", 30);
		using ServerConnection connection = serverAccess.CreateConnection();
		using VmoClient vmoClient = serverAccess.CreateVmoClient(connection);
		try
		{
			if (vmoClient.ManagementServer.IsFeatureLicensed("ManagementServerFailover"))
			{
				return true;
			}
			onNotLicensed();
			return false;
		}
		catch (VmoUnauthorizedAccessException)
		{
			onUnauthorizedAccess();
			return false;
		}
		catch (VmoException obj)
		{
			onVmoException(obj);
			return false;
		}
	}

	public static async Task<string> GetNodeLicenseStatusAsync(string safekitExecutableLocation)
	{
		string commandOutput = string.Empty;
		string licenseInformation;
		try
		{
			Process proc = new Process();
			proc.StartInfo.WorkingDirectory = safekitExecutableLocation;
			proc.StartInfo.FileName = Path.Combine(safekitExecutableLocation, "safekit.exe");
			proc.StartInfo.Arguments = "level";
			proc.StartInfo.RedirectStandardOutput = true;
			proc.StartInfo.UseShellExecute = false;
			proc.StartInfo.CreateNoWindow = true;
			proc.Start();
			await Task.Factory.StartNew(delegate
			{
				commandOutput = proc.StandardOutput.ReadToEnd();
				proc.WaitForExit();
			});
			licenseInformation = GetLicenseInformation(commandOutput);
		}
		catch (IOException ex)
		{
			_log.LogError("Get license status failed with IO error: {ExceptionMessage}", ex.Message);
			return string.Empty;
		}
		catch (Win32Exception ex2)
		{
			_log.LogError("Get license status failed with Win32 error: {ExceptionMessage}", ex2.Message);
			return string.Empty;
		}
		catch (SystemException ex3)
		{
			_log.LogError("Get license status failed with System error: {ExceptionMessage}", ex3.Message);
			return string.Empty;
		}
		return licenseInformation;
	}

	private static string GetLicenseInformation(string licenseData)
	{
		string hostName = Dns.GetHostName();
		if (string.IsNullOrWhiteSpace(licenseData) || !licenseData.Contains("License"))
		{
			_log.LogError("Get license status failed to return a valid result");
			return string.Empty;
		}
		int startIndex = licenseData.IndexOf("License", StringComparison.OrdinalIgnoreCase);
		string text = licenseData.Substring(startIndex);
		int num = text.IndexOf(":", StringComparison.OrdinalIgnoreCase);
		string text2 = text.Substring(num + 1).TrimStart();
		return Resources.FailoverWizardForm_LicenseInfoHost + ": " + hostName + " " + text2;
	}
}
