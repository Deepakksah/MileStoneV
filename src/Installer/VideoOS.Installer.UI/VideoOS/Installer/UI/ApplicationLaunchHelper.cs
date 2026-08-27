using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;

namespace VideoOS.Installer.UI;

public static class ApplicationLaunchHelper
{
	public static void OpenClientAction(string dependentPackId, InstallerManager installerManager)
	{
		if (installerManager == null)
		{
			throw new ArgumentNullException("installerManager");
		}
		OpenClient(dependentPackId, installerManager.Parameters["Run Application"], installerManager);
	}

	public static void OpenClientAction(string dependentPackId, string runApp, InstallerManager installerManager)
	{
		if (installerManager == null)
		{
			throw new ArgumentNullException("installerManager");
		}
		OpenClient(dependentPackId, runApp, installerManager);
	}

	private static void OpenClient(string dependentPackId, string runApp, InstallerManager installerManager)
	{
		if (installerManager == null)
		{
			throw new ArgumentNullException("installerManager");
		}
		string text = installerManager.Parameters["TARGETDIR"];
		if (text.Contains(" (x86)") && IsDependentPackage64(dependentPackId, installerManager))
		{
			text = text.Replace(" (x86)", "");
		}
		string text2 = Path.Combine(text, runApp);
		if (File.Exists(text2))
		{
			Process.Start(text2);
		}
		else
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Application path '{0}' doesn't exist.", text2), TraceEventType.Warning);
		}
	}

	public static void OpenGettingStartedAction(InstallerManager installerManager)
	{
		if (installerManager == null)
		{
			throw new ArgumentNullException("installerManager");
		}
		string inString = ((!installerManager.Parameters["Getting Started"].Contains("{0}")) ? installerManager.Parameters["Getting Started"] : string.Format(CultureInfo.InvariantCulture, installerManager.Parameters["Getting Started"], installerManager.Parameters["LANGUAGE"]));
		Process.Start(ExpandParameters(inString, installerManager));
	}

	private static bool IsDependentPackage64(string dependentPackId, InstallerManager installerManager)
	{
		if (!string.IsNullOrEmpty(dependentPackId))
		{
			InstallerPackageType[] installerPackages = installerManager.InstallerConfig.InstallerPackages;
			foreach (InstallerPackageType installerPackageType in installerPackages)
			{
				if (installerPackageType.ID == dependentPackId)
				{
					return installerPackageType.ProductManifest.PackageData.ProductInfo.TargetPlatform.ToUpper(CultureInfo.InvariantCulture) == PlatformType.X64.ToString();
				}
			}
		}
		return false;
	}

	private static string ExpandParameters(string inString, InstallerManager installerManager)
	{
		string text = inString;
		foreach (KeyValuePair<string, string> parameter in installerManager.Parameters)
		{
			string text2 = string.Format(CultureInfo.InvariantCulture, "[{0}]", parameter.Key);
			if (text.IndexOf(text2, StringComparison.OrdinalIgnoreCase) >= 0)
			{
				text = text.Replace(text2, parameter.Value);
			}
		}
		return text;
	}
}
