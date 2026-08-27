using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using VideoOS.Installer.Logging;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Management.VmoClient;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ActivateLicensePlugin : ProgressPlugin
{
	public const string DefaultUserName = "MilestonePBUUser";

	public const string DefaultPassword = "ekyo91dlekyQQbp1zT";

	public ActivateLicensePlugin(int serverPort)
		: base(serverPort, 2)
	{
	}

	public bool DoOnlineActivation()
	{
		try
		{
			using ServerConnection serverConnection = new ServerConnection("localhost");
			serverConnection.ServerPort = base.ServerPort;
			using VmoClient vmoClient = new VmoClient(serverConnection);
			ActivateLicenseTask activateLicenseTask = vmoClient.ManagementServer.ActivateLicense("MilestonePBUUser", "ekyo91dlekyQQbp1zT");
			int num = 0;
			while (true)
			{
				if (activateLicenseTask.Status == AsyncTaskStatus.Succeeded)
				{
					OnProgressChanged(100, Resources.ActivateLicense_Success);
					return true;
				}
				if (activateLicenseTask.Status == AsyncTaskStatus.Failed)
				{
					break;
				}
				Thread.Sleep(500);
				activateLicenseTask.Refresh();
				if (num < 99)
				{
					num++;
				}
				OnProgressChanged(num, Resources.ProgressOnlineActivation);
			}
			OnProgressChanged(100, string.Format(CultureInfo.CurrentUICulture, Resources.ActivateLicense_Failed, activateLicenseTask.ErrorResult.ErrorText));
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Online activation failed with error: {0}", activateLicenseTask.ErrorResult.ErrorText), TraceEventType.Information);
			return false;
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
		}
		return false;
	}
}
