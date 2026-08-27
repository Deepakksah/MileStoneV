using System;
using Microsoft.Win32;
using VideoOS.EventServer.Shared;

namespace VideoOS.EventServer.Utils;

public class RegistryTools : IRegistryTools
{
	public string ReadRegistryValue(string propertyName, string defaultValue)
	{
		try
		{
			object value = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(ProductConstants.InstallRegistryLocation).GetValue(propertyName);
			if (value != null)
			{
				return value.ToString();
			}
		}
		catch (Exception)
		{
		}
		return defaultValue;
	}

	public string ReadRegistryValue(string propertyName)
	{
		return ReadRegistryValue(propertyName, null);
	}

	public bool WriteRegistryValue(string propertyName, string propertyValue)
	{
		try
		{
			RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
			RegistryKey registryKey2 = registryKey.OpenSubKey(ProductConstants.InstallRegistryLocation, writable: true);
			if (registryKey2 == null)
			{
				registryKey2 = registryKey.CreateSubKey(ProductConstants.InstallRegistryLocation);
			}
			registryKey2.SetValue(propertyName, propertyValue);
			return true;
		}
		catch (Exception)
		{
		}
		return false;
	}
}
