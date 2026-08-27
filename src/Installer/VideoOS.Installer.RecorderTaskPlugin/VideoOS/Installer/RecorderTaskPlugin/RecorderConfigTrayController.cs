using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Threading;
using VideoOS.Common.Integration;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.RecorderTaskPlugin;

public static class RecorderConfigTrayController
{
	public static void StartTrayController(Dictionary<string, string> parameters, string trayControllerFileName, bool isFailover)
	{
		string text = parameters["INSTALLLOCATION"];
		if (isFailover)
		{
			text = text.Replace(OemResources.Instance.Recorder.FullNameInstaller, OemResources.Instance.FailoverServer.DefaultInstallDir);
		}
		if (string.IsNullOrEmpty(text))
		{
			throw new ArgumentException("INSTALLLOCATION parameter is not set");
		}
		string text2 = Path.Combine(text, trayControllerFileName);
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Starting TrayController at {0}", text2));
		if (File.Exists(text2))
		{
			LogManager.Write("Starting TrayController at " + text2);
			Process.Start(new ProcessStartInfo(text2)
			{
				UseShellExecute = true
			});
			return;
		}
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Starting TrayController failed because it cannot be found at {0}", text2));
	}

	public static void StopTrayController(string trayControllerFileName)
	{
		LogManager.Write(FormattableString.Invariant($"Stopping TrayController {trayControllerFileName}"));
		try
		{
			EventWaitHandle result = null;
			try
			{
				EventWaitHandle.TryOpenExisting("Global\\" + trayControllerFileName, EventWaitHandleRights.Modify | EventWaitHandleRights.Synchronize, out result);
				if (result != null)
				{
					result.Set();
					LogManager.Write("StopRecorderServiceTrayController: Stop RecorderServiceTrayController, Done!");
					return;
				}
			}
			finally
			{
				result?.Dispose();
			}
		}
		catch (Exception ex)
		{
			LogManager.Write(FormattableString.Invariant($"StopRecorderServiceTrayController: Stop RecorderServiceTrayController failed to get handler: {ex}"));
		}
		try
		{
			bool createdNew;
			using EventWaitHandle eventWaitHandle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset, "Global\\" + trayControllerFileName, out createdNew);
			if (!createdNew)
			{
				eventWaitHandle.Set();
				LogManager.Write("StopRecorderServiceTrayController: Stop RecorderServiceTrayController, Done!");
			}
		}
		catch (Exception ex2)
		{
			LogManager.Write(FormattableString.Invariant($"StopRecorderServiceTrayController: Stop RecorderServiceTrayController failed: {ex2}"));
		}
	}
}
