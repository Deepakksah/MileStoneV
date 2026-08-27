using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using Microsoft.Win32;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Manager;

namespace VideoOS.Installer.RecorderPlugins;

public class PluginUtilities : IPluginUtilities
{
	[Flags]
	public enum MilestoneServerTypes
	{
		None = 0,
		RecordingServer = 1,
		Failover = 2
	}

	private readonly IDictionary<MilestoneServerTypes, string> TypeToName = new Dictionary<MilestoneServerTypes, string>
	{
		{
			MilestoneServerTypes.None,
			"<none>"
		},
		{
			MilestoneServerTypes.RecordingServer,
			OemResources.Instance.Recorder.ServiceName
		},
		{
			MilestoneServerTypes.Failover,
			OemResources.Instance.FailoverServer.ServiceName
		}
	};

	public static IPluginUtilities Instance { get; } = new PluginUtilities();

	public bool FailoverInstallation
	{
		get
		{
			string value2;
			if (GlobalData.TryGetValue("INSTALLATION_MODE", out var value))
			{
				if (string.Compare(value, "Failover", StringComparison.CurrentCultureIgnoreCase) == 0)
				{
					return true;
				}
			}
			else if (GlobalData.TryGetValue("ISFAILOVER", out value2) && string.Compare(value2, bool.TrueString, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsTypical
	{
		get
		{
			if (GlobalData.TryGetValue("INSTALLATION_MODE", out var value) && string.Compare(value, "Typical", StringComparison.CurrentCultureIgnoreCase) == 0)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsCustom
	{
		get
		{
			if (GlobalData.TryGetValue("INSTALLATION_MODE", out var value) && string.Compare(value, "CustomMode", StringComparison.CurrentCultureIgnoreCase) == 0)
			{
				return true;
			}
			return false;
		}
	}

	public string LookupRegistryKey
	{
		get
		{
			if (!FailoverInstallation)
			{
				return "SOFTWARE\\VideoOS\\Recorder\\Installation";
			}
			return "SOFTWARE\\VideoOS\\FailoverServer\\Installation";
		}
	}

	public string LookupUser
	{
		get
		{
			if (!FailoverInstallation)
			{
				return "RecorderUser";
			}
			return "FailoverUser";
		}
	}

	private PluginUtilities()
	{
	}

	public string GetValueFromRegistry(string keyName, string parameter)
	{
		if (keyName == null)
		{
			throw new ArgumentNullException("keyName");
		}
		string registryValue = RegistryHelper.GetRegistryValue(keyName, parameter, RegistryView.Registry64);
		if (string.IsNullOrEmpty(registryValue))
		{
			registryValue = RegistryHelper.GetRegistryValue(keyName, parameter, RegistryView.Registry32);
		}
		return registryValue;
	}

	public void DeleteValueFromRegistry(string keyName, string parameter)
	{
		if (keyName == null)
		{
			throw new ArgumentNullException("keyName");
		}
		try
		{
			RegistryHelper.DeleteRegistryValue(keyName, parameter, RegistryView.Registry64);
		}
		catch
		{
		}
		try
		{
			RegistryHelper.DeleteRegistryValue(keyName, parameter, RegistryView.Registry32);
		}
		catch
		{
		}
	}

	public MilestoneServerTypes InstalledMilestoneServices()
	{
		MilestoneServerTypes milestoneServerTypes = MilestoneServerTypes.None;
		ServiceController[] services = ServiceController.GetServices();
		for (int i = 0; i < services.Length; i++)
		{
			string displayName = services[i].DisplayName;
			foreach (KeyValuePair<MilestoneServerTypes, string> item in TypeToName)
			{
				if (item.Value.Equals(displayName, StringComparison.OrdinalIgnoreCase))
				{
					milestoneServerTypes |= item.Key;
				}
			}
		}
		return milestoneServerTypes;
	}

	public bool IsRecorderInstalledInHWMM()
	{
		PackageRefType packageRefType = InstallerManager.Instance.OldInstallInfo?.InstalledPackages?.FirstOrDefault((PackageRefType p) => p.ID.ToUpperInvariant() == "18451C4D-72B8-4D83-885D-A74B82F6C997");
		if (packageRefType != null)
		{
			return packageRefType.Condition == ConditionType.Finalize;
		}
		return false;
	}
}
