using System;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.Common.Utilities;

public static class RegistryKeyHelper
{
	public static string GetLocalMachineValue(string keyPath, string parameter)
	{
		try
		{
			string text = null;
			using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(keyPath))
			{
				if (registryKey != null)
				{
					text = (string)registryKey.GetValue(parameter);
					registryKey.Close();
				}
			}
			if (text != null)
			{
				return text;
			}
			string text2 = "";
			string text3 = keyPath.ToLower();
			if (text3.StartsWith("software") && !text3.StartsWith("software\\wow6432node"))
			{
				text2 = Regex.Replace(text3, "^software(.*)", "software\\wow6432node$1");
			}
			LogManager.Write($"Trying to retrieve registry key {text2}");
			using (RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(text2))
			{
				if (registryKey2 != null)
				{
					text = (string)registryKey2.GetValue(parameter);
					registryKey2.Close();
				}
			}
			return text;
		}
		catch (Exception exception)
		{
			LogManager.Write($"Retrieve registry key {keyPath} failed");
			LogManager.Write(exception);
		}
		return null;
	}

	public static RegistryKey OpenLocalMachineKey(string keyPath)
	{
		try
		{
			LogManager.Write($"Trying to retrieve registry key {keyPath}");
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(keyPath);
			if (registryKey != null)
			{
				return registryKey;
			}
		}
		catch
		{
		}
		string text = "";
		string text2 = keyPath.ToLower();
		if (text2.StartsWith("software") && !text2.StartsWith("software\\wow6432node"))
		{
			text = Regex.Replace(text2, "^software(.*)", "software\\wow6432node$1");
		}
		try
		{
			LogManager.Write($"Second attempt: Trying to retrieve registry key {text}");
			RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(text);
			if (registryKey2 != null)
			{
				return registryKey2;
			}
		}
		catch
		{
		}
		return null;
	}
}
