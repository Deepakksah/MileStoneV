using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Logging;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.TrayController.Communication.Recorder;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class RecorderOnlinePlugin : ProgressPlugin
{
	private volatile int _percent;

	public RecorderOnlinePlugin(int serverPort)
		: base(serverPort, 2)
	{
	}

	public bool RecorderOnline()
	{
		IRecorderTrayControllerClient client = RecorderTrayControllerFactory.GetClient(RecorderTrayControllerFactory.GetDefaultPort());
		return RecorderOnline(600, 500, client, LogManager.InnerLog);
	}

	public bool RecorderOnline(int maxTimeSeconds, int sleepMSeconds, IRecorderTrayControllerClient rtcClient, ILogger logger)
	{
		bool result = false;
		try
		{
			OnProgressChanged(0, Resources.RecorderOnlineWaiting);
			double num = 0.0;
			while (true)
			{
				num += (double)(sleepMSeconds / 1000);
				Thread.Sleep(sleepMSeconds);
				if (rtcClient.IsRecorderRunningAndConnected())
				{
					result = true;
					break;
				}
				if (!(num >= (double)maxTimeSeconds))
				{
					_percent = Convert.ToInt32(num / (double)maxTimeSeconds * 100.0);
					OnProgressChanged(_percent, Resources.RecorderOnlineWaiting);
					continue;
				}
				break;
			}
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogInformation(logger, "Scanning Hardware failed " + ex.Message, Array.Empty<object>());
		}
		finally
		{
			OnProgressChanged(100, Resources.RecorderOnlineStatusConnected);
		}
		return result;
	}
}
