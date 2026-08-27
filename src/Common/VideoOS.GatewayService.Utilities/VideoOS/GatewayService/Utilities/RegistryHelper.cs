using System;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using VideoOS.Common.EnvironmentServices;
using VideoOS.GatewayService.Utilities.Oem;

namespace VideoOS.GatewayService.Utilities;

public class RegistryHelper : IRegistryHelper
{
	private static object _threadLock = new object();

	private static IRegistryService _registryTools;

	private IGatewayServiceOemResource _oemResource;

	private static IRegistryKey _oemGatewayBaseKey;

	private static IRegistryKey _nonOemGatewayBaseKey;

	private static bool _isCleaned = true;

	private static RegistryKeys[] NonOemKeys = new RegistryKeys[1] { RegistryKeys.ServiceUrl };

	private string OemRegistryPath => Path.Combine("SOFTWARE", _oemResource.CompanyNameShort, _oemResource.ApiGatewayInstallDir);

	private static string NonOemRegistryPath => Path.Combine("SOFTWARE", "VideoOS", "GatewayService");

	public RegistryHelper(IGatewayServiceOemResource oemResource)
	{
		_oemResource = oemResource;
		InitClassVariables();
	}

	private void InitClassVariables()
	{
		if (_registryTools == null)
		{
			_registryTools = new RegistryService();
		}
		_oemGatewayBaseKey = _registryTools.LocalMachine.CreateSubKey(OemRegistryPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
		_nonOemGatewayBaseKey = _registryTools.LocalMachine.CreateSubKey(NonOemRegistryPath, RegistryKeyPermissionCheck.ReadWriteSubTree);
		_isCleaned = false;
		if (_oemGatewayBaseKey == null)
		{
			throw new Exception("Could not create or open registry key on path " + OemRegistryPath);
		}
		if (_nonOemGatewayBaseKey == null)
		{
			throw new Exception("Could not create or open registry key on path " + NonOemRegistryPath);
		}
	}

	public string ReadKey(RegistryKeys key)
	{
		lock (_threadLock)
		{
			if (_isCleaned)
			{
				return null;
			}
			return GetBaseKey(key).GetValue(key.ToString()) as string;
		}
	}

	public void WriteKey(RegistryKeys key, string value)
	{
		lock (_threadLock)
		{
			if (_isCleaned)
			{
				InitClassVariables();
			}
			IRegistryKey baseKey = GetBaseKey(key);
			baseKey.SetValue(key.ToString(), value);
			baseKey.Flush();
		}
	}

	public void CleanUpRegistry()
	{
		lock (_threadLock)
		{
			string[] names = Enum.GetNames(typeof(RegistryKeys));
			foreach (string text in names)
			{
				GetBaseKey((RegistryKeys)Enum.Parse(typeof(RegistryKeys), text)).DeleteValue(text, throwOnMissingValue: false);
			}
			names = _oemGatewayBaseKey.GetSubKeyNames();
			foreach (string path in names)
			{
				_registryTools.LocalMachine.DeleteSubKeyTree(Path.Combine(OemRegistryPath, path), throwOnMissingSubKey: false);
			}
			names = _nonOemGatewayBaseKey.GetSubKeyNames();
			foreach (string path2 in names)
			{
				_registryTools.LocalMachine.DeleteSubKeyTree(Path.Combine(NonOemRegistryPath, path2), throwOnMissingSubKey: false);
			}
			_oemGatewayBaseKey.Close();
			_oemGatewayBaseKey = null;
			_nonOemGatewayBaseKey.Close();
			_nonOemGatewayBaseKey = null;
			_isCleaned = true;
			_registryTools.LocalMachine.DeleteSubKey(OemRegistryPath, throwOnMissingSubKey: false);
			_registryTools.LocalMachine.DeleteSubKey(NonOemRegistryPath, throwOnMissingSubKey: false);
			_registryTools.LocalMachine.Close();
			_registryTools = null;
		}
	}

	private static IRegistryKey GetBaseKey(RegistryKeys key)
	{
		if (NonOemKeys.Contains(key))
		{
			return _nonOemGatewayBaseKey;
		}
		return _oemGatewayBaseKey;
	}
}
