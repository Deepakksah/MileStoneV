using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Diagnostics.Logging;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.Failover.Configurator;

public class FailoverConfiguratorSilent
{
	private static class FailoverParameters
	{
		public const string Silent = "-SILENT";

		public const string PrepareSecondary = "-PREPARESECONDARY";

		public const string SetupPrimary = "-SETUPPRIMARY";

		public const string FinalizeSecondary = "-FINALIZESECONDARY";

		public const string Deconfigure = "-DECONFIGURE";

		public const string HttpsPassword = "-HTTPSPASSWORD";

		public const string WebConsolePassword = "-WEBCONSOLEPASSWORD";

		public const string VirtualIp = "-VIRTUALIP";

		public const string SecondaryMachineAddress = "-SECONDARYMACHINEADDRESS";

		public const string DownloadServerCertificate = "-DOWNLOADSERVERCERTIFICATE";

		public const string DownloadFolder = "-DOWNLOADFOLDER";

		public const string UseExternalSql = "-USEEXTERNALSQL";

		public const string SqlDataFolderPath = "-SQLDATAFOLDERPATH";

		public const string SqlLogFolderPath = "-SQLLOGFOLDERPATH";
	}

	private IList<string> _failoverParameters;

	private IServerAccess _failoverConfiguratorAccess;

	private bool _useExternalSqlSetup;

	private FailoverConfiguratorRunner _failoverConfiguratorRunner;

	public static ILogger Logger;

	public FailoverConfiguratorSilent(IList<string> failoverParameters, IServerAccess failoverConfiguratorAccess)
	{
		_failoverParameters = failoverParameters;
		_failoverConfiguratorAccess = failoverConfiguratorAccess;
		LogManager.ModifyLogParameter += FailoverConfigurationHelper.LogManagerOnModifyLogParameter;
		Logger = Telemetry.GetILogger("ManagementServerFailover");
		_failoverConfiguratorRunner = new FailoverConfiguratorRunner(failoverConfiguratorAccess, new DPAPIProtectionProvider(), Logger);
	}

	public void RunCommand()
	{
		if (_failoverParameters == null)
		{
			Logger.LogWarning("Please provide parameters for failover.");
		}
		else if (!FailoverConfigurationHelper.CheckIfFailoverIsInstalled())
		{
			Logger.LogWarning("Feature is not installed.");
		}
		else if (_failoverParameters.Contains("-SILENT"))
		{
			if (_failoverParameters.Contains("-USEEXTERNALSQL"))
			{
				_useExternalSqlSetup = true;
			}
			if (_failoverParameters.Contains("-PREPARESECONDARY"))
			{
				Logger.LogInformation("Preparing secondary");
				PrepareSecondary();
			}
			else if (_failoverParameters.Contains("-SETUPPRIMARY"))
			{
				SetupPrimary();
			}
			else if (_failoverParameters.Contains("-FINALIZESECONDARY"))
			{
				FinalizeSecondary();
			}
			else if (_failoverParameters.Contains("-DECONFIGURE"))
			{
				Deconfigure();
			}
			else if (_failoverParameters.Contains("-DOWNLOADSERVERCERTIFICATE"))
			{
				DownloadServerCertificate();
			}
		}
	}

	private string GetParameterValue(string parameter)
	{
		int num = _failoverParameters.IndexOf(parameter);
		string result = string.Empty;
		if (num != -1 && num < _failoverParameters.Count)
		{
			result = _failoverParameters[num + 1];
		}
		return result;
	}

	private void PrepareSecondary()
	{
		if (!FailoverConfigurationHelper.CheckIfFeatureIsLicensed(_failoverConfiguratorAccess))
		{
			Logger.LogWarning("Feature not licensed. Request a license from your administrator.");
			return;
		}
		string parameterValue = GetParameterValue("-HTTPSPASSWORD");
		string parameterValue2 = GetParameterValue("-WEBCONSOLEPASSWORD");
		if (string.IsNullOrEmpty(parameterValue2))
		{
			Logger.LogWarning("Please provide a web console password.");
		}
		else if (!_failoverConfiguratorRunner.ConfigureSecondaryNodeAsync(!string.IsNullOrEmpty(parameterValue), parameterValue2, parameterValue).Result.Item1)
		{
			Logger.LogError("Secondary Configuration failed.");
		}
	}

	private void SetupPrimary()
	{
		if (!FailoverConfigurationHelper.CheckIfFeatureIsLicensed(_failoverConfiguratorAccess))
		{
			Logger.LogWarning("Feature not licensed. Request a license from your administrator.");
			return;
		}
		string parameterValue = GetParameterValue("-HTTPSPASSWORD");
		string parameterValue2 = GetParameterValue("-WEBCONSOLEPASSWORD");
		string parameterValue3 = GetParameterValue("-VIRTUALIP");
		string parameterValue4 = GetParameterValue("-SECONDARYMACHINEADDRESS");
		string parameterValue5 = GetParameterValue("-SQLDATAFOLDERPATH");
		string parameterValue6 = GetParameterValue("-SQLLOGFOLDERPATH");
		if (string.IsNullOrEmpty(parameterValue2))
		{
			Logger.LogWarning("Please provide a web console password.");
			return;
		}
		if (string.IsNullOrEmpty(parameterValue3))
		{
			Logger.LogWarning("Please provide a virtual IP.");
			return;
		}
		if (string.IsNullOrEmpty(parameterValue4))
		{
			Logger.LogWarning("Please provide a secondary machine address (FQDN).");
			return;
		}
		if (string.IsNullOrEmpty(parameterValue5))
		{
			Logger.LogInformation("VMS SQL Data folder path will be used");
		}
		if (string.IsNullOrEmpty(parameterValue6))
		{
			Logger.LogInformation("VMS SQL Log folder path will be used");
		}
		if (!_failoverConfiguratorRunner.ConfigurePrimaryNodeAsync(!string.IsNullOrEmpty(parameterValue), parameterValue, parameterValue2, parameterValue3, parameterValue4, parameterValue5, parameterValue6, _useExternalSqlSetup).Result)
		{
			Logger.LogError("Secondary Configuration failed");
		}
	}

	private void FinalizeSecondary()
	{
		string parameterValue = GetParameterValue("-HTTPSPASSWORD");
		if (!_failoverConfiguratorRunner.FinalizeSecondaryNodeAsync(!string.IsNullOrEmpty(parameterValue), _useExternalSqlSetup).Result)
		{
			Logger.LogError("Finalize Secondary Failed");
		}
	}

	private void Deconfigure()
	{
		if (!_failoverConfiguratorRunner.DeconfigureAsync(_useExternalSqlSetup).Result)
		{
			Logger.LogError("Deconfiguration Failed");
		}
	}

	private void DownloadServerCertificate()
	{
		string parameterValue = GetParameterValue("-DOWNLOADFOLDER");
		if (!_failoverConfiguratorRunner.Settings.IsPrimary)
		{
			Logger.LogError("Server certificate download is available only on primary node.");
			return;
		}
		Logger.LogInformation("Downloading certificate to {DownloadFolder}", parameterValue);
		if (!_failoverConfiguratorRunner.DownloadServerCertificateAsync(parameterValue).Result)
		{
			Logger.LogError("Download Failed");
		}
	}
}
