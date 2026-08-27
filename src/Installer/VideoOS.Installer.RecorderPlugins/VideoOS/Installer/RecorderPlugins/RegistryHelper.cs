using Microsoft.Win32;

namespace VideoOS.Installer.RecorderPlugins;

public static class RegistryHelper
{
	public static string GetRegistryValue(string keyName, string parameter, RegistryView regView)
	{
		string result = "";
		using (RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, regView))
		{
			RegistryKey registryKey2 = registryKey.OpenSubKey(keyName);
			if (registryKey2 != null)
			{
				result = (string)registryKey2.GetValue(parameter);
				registryKey2.Close();
			}
		}
		return result;
	}

	public static void SetRegistryValue(string keyName, string parameter, string regValue, RegistryView regView)
	{
		using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, regView);
		RegistryKey registryKey2 = registryKey.OpenSubKey(keyName, writable: true);
		if (registryKey2 != null)
		{
			registryKey2.SetValue(parameter, regValue);
			registryKey2.Close();
		}
	}

	public static void DeleteRegistryValue(string keyName, string parameter, RegistryView regView)
	{
		using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, regView);
		RegistryKey registryKey2 = registryKey.OpenSubKey(keyName, writable: true);
		if (registryKey2 != null)
		{
			registryKey2.DeleteValue(parameter, throwOnMissingValue: false);
			registryKey2.Close();
		}
	}
}
