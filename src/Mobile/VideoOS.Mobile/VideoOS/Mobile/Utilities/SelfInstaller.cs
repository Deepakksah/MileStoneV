using System;
using System.Configuration.Install;
using System.Reflection;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Utilities;

public static class SelfInstaller
{
	private static readonly string ExePath = Assembly.GetExecutingAssembly().Location;

	public static bool InstallMe()
	{
		return Install(ExePath);
	}

	public static bool Install(string exePath)
	{
		bool result = false;
		try
		{
			ManagedInstallerClass.InstallHelper(new string[1] { exePath });
			result = true;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return result;
	}

	public static bool UninstallMe()
	{
		return Uninstall(ExePath);
	}

	public static bool Uninstall(string exePath)
	{
		bool result = false;
		try
		{
			ManagedInstallerClass.InstallHelper(new string[2] { "/u", exePath });
			result = true;
		}
		catch (Exception ex)
		{
			Log.Error("SelfInstaller.UninstallMe", ex.Message);
		}
		return result;
	}
}
