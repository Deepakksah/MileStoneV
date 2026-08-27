using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Threading;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public static class ServerConfiguratorTrayController
{
	public static void StartServerServiceTrayController(Dictionary<ServerParameters, string> serverParameters)
	{
		LogManager.Write("StartServerServiceTrayController: Starting...");
		string text = Path.Combine(serverParameters[ServerParameters.DestinationDir], "VideoOS.Server.Service.TrayController.exe");
		if (File.Exists(text))
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "StartServerServiceTrayController: Starting ServerServiceTrayController at {0}", text));
			text = "\"" + text + "\"";
			Process.Start(new ProcessStartInfo(text)
			{
				UseShellExecute = true
			});
			LogManager.Write("StartServerServiceTrayController: ServerServiceTrayController started!");
			return;
		}
		throw new ServerConfiguratorFailedException(string.Format(CultureInfo.InvariantCulture, "Starting ServerServiceTrayController failed because it cannot be found at {0}", text));
	}

	public static void StopServerServiceTrayController()
	{
		Exception ex = null;
		Exception ex2 = null;
		try
		{
			LogManager.Write("StopServerServiceTrayController: Starting...");
			bool createdNew;
			using EventWaitHandle eventWaitHandle = new EventWaitHandle(initialState: false, EventResetMode.ManualReset, "Global\\VideoOS.Server.Service.TrayController", out createdNew);
			if (!createdNew)
			{
				eventWaitHandle.Set();
				LogManager.Write("StopServerServiceTrayController: Stop ServerServiceTrayController, Done!");
				return;
			}
		}
		catch (Exception ex3)
		{
			ex = ex3;
		}
		try
		{
			if (EventWaitHandle.TryOpenExisting("Global\\VideoOS.Server.Service.TrayController", EventWaitHandleRights.Modify | EventWaitHandleRights.Synchronize, out var result))
			{
				result.Set();
				LogManager.Write("StopServerServiceTrayController: Stop ServerServiceTrayController, Done!");
				return;
			}
		}
		catch (Exception ex4)
		{
			ex2 = ex4;
		}
		if (ex != null)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "StopServerServiceTrayController: Stop ServerServiceTrayController failed: {0}", ex));
		}
		if (ex2 != null)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "StopServerServiceTrayController: Stop ServerServiceTrayController failed to get handler: {0}", ex2));
		}
	}
}
