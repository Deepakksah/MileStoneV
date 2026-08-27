using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Server.Admin.Utilities;

namespace VideoOS.ShareService.InstallerPlugin;

public class ServerConfigurator
{
	private Dictionary<ServerParameters, string> _serverParameters;

	private int _maxProgress = 1;

	public Action<int> OnProgressChanged;

	private LogManagerWrapper LogComponent { get; set; }

	public int MaxProgress => _maxProgress;

	public ServerConfigurator(IEnumerable<IPackageParameter> installParameters, bool installation, LogManagerWrapper logComponent)
	{
		LogComponent = logComponent;
		InitParams(installParameters, installation);
	}

	private void InitParams(IEnumerable<IPackageParameter> installParameters, bool installation)
	{
		_serverParameters = new Dictionary<ServerParameters, string>();
		foreach (IPackageParameter installParameter in installParameters)
		{
			switch (installParameter.Target)
			{
			case "INSTALLFOLDER":
				_serverParameters[ServerParameters.ServiceFolder] = installParameter.Value;
				break;
			case "WebSiteId":
				_serverParameters[ServerParameters.WebSiteId] = installParameter.Value;
				break;
			case "WebSiteName":
				_serverParameters[ServerParameters.WebSiteName] = installParameter.Value;
				break;
			case "USERACCOUNT":
				_serverParameters[ServerParameters.UserAccount] = installParameter.Value;
				break;
			case "ACCOUNTNAME":
				_serverParameters[ServerParameters.AccountName] = installParameter.Value;
				break;
			case "ENCRYPTEDPASSWORD":
				if (installation)
				{
					_serverParameters[ServerParameters.AccountPassword] = EncryptHelper.DecryptPassword(installParameter.Value, unicodePassword: true);
				}
				break;
			}
		}
		if (!_serverParameters.ContainsKey(ServerParameters.ServiceFolder))
		{
			LogComponent.Write("Missing parameter installer folder", TraceEventType.Error);
		}
		LogComponent.Write("Installing parameters:");
		foreach (KeyValuePair<ServerParameters, string> serverParameter in _serverParameters)
		{
			if (serverParameter.Key.ToString().IndexOf("password", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				LogComponent.Write(string.Format("{0}={1}", serverParameter.Key, "*"));
			}
			else
			{
				LogComponent.Write($"{serverParameter.Key}={serverParameter.Value}");
			}
		}
	}

	public void RunInstall()
	{
		try
		{
			LogComponent.Write("Star of RunInstall");
			new SetupIIS(LogComponent).Install(serviceAccount: new AccountInfo(_serverParameters[ServerParameters.AccountName], _serverParameters[ServerParameters.AccountPassword], requirePassword: false), virtualDirectoryPhysicalPath: _serverParameters[ServerParameters.ServiceFolder], websiteId: _serverParameters[ServerParameters.WebSiteId], websiteAddress: _serverParameters[ServerParameters.WebSiteName]);
		}
		catch (Exception ex)
		{
			LogComponent.Write(ex.Message);
			LogComponent.Write(ex.StackTrace);
			if (ex.InnerException != null)
			{
				LogComponent.Write(ex.InnerException.Message);
				LogComponent.Write(ex.InnerException.StackTrace);
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Share Service installer plugin INSTALL: Unexpected error ocurred during RunInstall", LogManager.GetLogFilename()), ex);
		}
		finally
		{
			OnProgressChanged?.Invoke(MaxProgress);
		}
		LogComponent.Write("End of RunInstall");
	}

	public void RunUninstall()
	{
		try
		{
			LogComponent.Write("Start of RunUninstall");
			new SetupIIS(LogComponent).Uninstall(_serverParameters[ServerParameters.WebSiteId]);
			OnProgressChanged?.Invoke(1);
		}
		catch (Exception ex)
		{
			LogComponent.Write(ex.Message);
			LogComponent.Write(ex.StackTrace);
			throw;
		}
		LogComponent.Write("End of RunUninstall");
	}
}
