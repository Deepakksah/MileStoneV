using System;

namespace Microsoft.Win32;

public static class RegistryEx
{
	public static readonly RegistryKeyEx ClassesRoot = new RegistryKeyEx(RegistryHive.ClassesRoot, "HKEY_CLASSES_ROOT");

	public static readonly RegistryKeyEx CurrentConfig = new RegistryKeyEx(RegistryHive.CurrentConfig, "HKEY_CURRENT_CONFIG");

	public static readonly RegistryKeyEx CurrentUser = new RegistryKeyEx(RegistryHive.CurrentUser, "HKEY_CURRENT_USER");

	public static readonly RegistryKeyEx DynData = new RegistryKeyEx(RegistryHive.DynData, "HKEY_DYN_DATA");

	public static readonly RegistryKeyEx LocalMachine = new RegistryKeyEx(RegistryHive.LocalMachine, "HKEY_LOCAL_MACHINE");

	public static readonly RegistryKeyEx PerformanceData = new RegistryKeyEx(RegistryHive.PerformanceData, "HKEY_PERFORMANCE_DATA");

	public static readonly RegistryKeyEx Users = new RegistryKeyEx(RegistryHive.Users, "HKEY_USERS");

	private static RegistryKeyEx ToKey(string keyName, bool writable, bool access64BitKey)
	{
		if (keyName == null)
		{
			throw new ArgumentException("Not a valid registry key name", "keyName");
		}
		RegistryKeyEx registryKeyEx = null;
		string[] array = keyName.Split('\\');
		registryKeyEx = array[0] switch
		{
			"HKEY_CLASSES_ROOT" => ClassesRoot, 
			"HKEY_CURRENT_CONFIG" => CurrentConfig, 
			"HKEY_CURRENT_USER" => CurrentUser, 
			"HKEY_DYN_DATA" => DynData, 
			"HKEY_LOCAL_MACHINE" => LocalMachine, 
			"HKEY_PERFORMANCE_DATA" => PerformanceData, 
			"HKEY_USERS" => Users, 
			_ => throw new ArgumentException("Keyname does not start with a valid registry root", "keyName"), 
		};
		for (int i = 1; i < array.Length; i++)
		{
			RegistryKeyEx registryKeyEx2 = registryKeyEx.OpenSubKey(array[i], writable, access64BitKey);
			if (registryKeyEx2 == null)
			{
				if (!writable)
				{
					return null;
				}
				registryKeyEx2 = registryKeyEx.CreateSubKey(array[i], access64BitKey);
			}
			registryKeyEx = registryKeyEx2;
		}
		return registryKeyEx;
	}

	public static void SetValue(string keyName, string valueName, object value, bool access64BitKey)
	{
		RegistryKeyEx registryKeyEx = ToKey(keyName, writable: true, access64BitKey);
		if (valueName.Length > 255)
		{
			throw new ArgumentException("valueName is larger than 255 characters", "valueName");
		}
		if (registryKeyEx == null)
		{
			throw new ArgumentException("cant locate that keyName", "keyName");
		}
		registryKeyEx.SetValue(valueName, value);
	}

	public static void SetValue(string keyName, string valueName, object value, RegistryValueKind valueKind, bool access64BitKey)
	{
		RegistryKeyEx registryKeyEx = ToKey(keyName, writable: true, access64BitKey);
		if (valueName.Length > 255)
		{
			throw new ArgumentException("valueName is larger than 255 characters", "valueName");
		}
		if (registryKeyEx == null)
		{
			throw new ArgumentException("cant locate that keyName", "keyName");
		}
		registryKeyEx.SetValue(valueName, value, valueKind);
	}

	public static object GetValue(string keyName, string valueName, object defaultValue, bool access64BitKey)
	{
		RegistryKeyEx registryKeyEx = ToKey(keyName, writable: false, access64BitKey);
		if (registryKeyEx == null)
		{
			return defaultValue;
		}
		return registryKeyEx.GetValue(valueName, defaultValue);
	}
}
