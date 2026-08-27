using Microsoft.Win32;
using VideoOS.Common.EnvironmentServices;

namespace VideoOS.Utilities.Database;

public static class IRegistryServiceExtension
{
	public static string GetRegistryValueFrom64View(this IRegistryService registryService, string path, RegistryHive hive, string valueName)
	{
		string result = null;
		using (IRegistryKey registryKey = registryService.RegistryKeyService.OpenBaseKey(hive, RegistryView.Registry64))
		{
			IRegistryKey registryKey2 = registryKey.OpenSubKey(path);
			if (registryKey2 != null)
			{
				result = (string)registryKey2.GetValue(valueName);
			}
		}
		return result;
	}

	public static void UpdateRegistryValueIn64View(this IRegistryService registryService, string path, RegistryHive hive, string valueName, string valueData)
	{
		using IRegistryKey registryKey = registryService.RegistryKeyService.OpenBaseKey(hive, RegistryView.Registry64);
		IRegistryKey registryKey2 = registryKey.OpenSubKey(path, writable: true);
		if (registryKey2 == null)
		{
			registryKey2 = registryKey.CreateSubKey(path);
		}
		registryKey2.SetValue(valueName, valueData);
	}

	public static void DeleteRegistryValueFromViews(this IRegistryService registryService, string path, RegistryHive hive, string valueName)
	{
		DeleteValue(registryService, path, hive, valueName, RegistryView.Registry32);
		DeleteValue(registryService, path, hive, valueName, RegistryView.Registry64);
	}

	private static void DeleteValue(IRegistryService registryService, string path, RegistryHive hive, string valueName, RegistryView view)
	{
		using IRegistryKey registryKey = registryService.RegistryKeyService.OpenBaseKey(hive, view);
		registryKey.OpenSubKey(path, writable: true)?.DeleteValue(valueName, throwOnMissingValue: false);
	}
}
