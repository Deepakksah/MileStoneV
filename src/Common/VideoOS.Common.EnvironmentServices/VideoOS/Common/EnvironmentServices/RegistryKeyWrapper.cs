using System;
using System.Security.AccessControl;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace VideoOS.Common.EnvironmentServices;

[SupportedOSPlatform("Windows")]
public class RegistryKeyWrapper : IRegistryKey, IDisposable
{
	private readonly RegistryKey _registryKey;

	public string Name => _registryKey.Name;

	public SafeRegistryHandle Handle => _registryKey.Handle;

	public RegistryView View => _registryKey.View;

	public int ValueCount => _registryKey.ValueCount;

	private RegistryKeyWrapper(RegistryKey registryKey)
	{
		_registryKey = registryKey;
	}

	public void Close()
	{
		_registryKey.Close();
	}

	public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions, RegistrySecurity registrySecurity)
	{
		return WrapppedKey(_registryKey.CreateSubKey(subkey, permissionCheck, registryOptions, registrySecurity));
	}

	public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity)
	{
		return WrapppedKey(_registryKey.CreateSubKey(subkey, permissionCheck, registrySecurity));
	}

	public IRegistryKey CreateSubKey(string subkey, bool writable, RegistryOptions options)
	{
		return WrapppedKey(_registryKey.CreateSubKey(subkey, writable, options));
	}

	public IRegistryKey CreateSubKey(string subkey, bool writable)
	{
		return WrapppedKey(_registryKey.CreateSubKey(subkey, writable));
	}

	public IRegistryKey CreateSubKey(string subkey)
	{
		return WrapppedKey(_registryKey.CreateSubKey(subkey));
	}

	public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck)
	{
		return WrapppedKey(_registryKey.CreateSubKey(subkey, permissionCheck));
	}

	public IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions options)
	{
		return WrapppedKey(_registryKey.CreateSubKey(subkey, permissionCheck, options));
	}

	public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
	{
		_registryKey.DeleteSubKey(subkey, throwOnMissingSubKey);
	}

	public void DeleteSubKey(string subkey)
	{
		_registryKey.DeleteSubKey(subkey);
	}

	public void DeleteSubKeyTree(string subkey)
	{
		_registryKey.DeleteSubKeyTree(subkey);
	}

	public void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey)
	{
		_registryKey.DeleteSubKeyTree(subkey, throwOnMissingSubKey);
	}

	public void DeleteValue(string name)
	{
		_registryKey.DeleteValue(name);
	}

	public void DeleteValue(string name, bool throwOnMissingValue)
	{
		_registryKey.DeleteValue(name, throwOnMissingValue);
	}

	public void Dispose()
	{
		_registryKey.Dispose();
	}

	public void Flush()
	{
		_registryKey.Flush();
	}

	public RegistrySecurity GetAccessControl(AccessControlSections includeSections)
	{
		return _registryKey.GetAccessControl(includeSections);
	}

	public RegistrySecurity GetAccessControl()
	{
		return _registryKey.GetAccessControl();
	}

	public string[] GetSubKeyNames()
	{
		return _registryKey.GetSubKeyNames();
	}

	public object GetValue(string name, object defaultValue)
	{
		return _registryKey.GetValue(name, defaultValue);
	}

	public object GetValue(string name, object defaultValue, RegistryValueOptions options)
	{
		return _registryKey.GetValue(name, defaultValue, options);
	}

	public object GetValue(string name)
	{
		return _registryKey.GetValue(name);
	}

	public RegistryValueKind GetValueKind(string name)
	{
		return _registryKey.GetValueKind(name);
	}

	public string[] GetValueNames()
	{
		return _registryKey.GetValueNames();
	}

	public IRegistryKey OpenSubKey(string name)
	{
		return WrapppedKey(_registryKey.OpenSubKey(name));
	}

	public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights)
	{
		return WrapppedKey(_registryKey.OpenSubKey(name, permissionCheck, rights));
	}

	public IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck)
	{
		return WrapppedKey(_registryKey.OpenSubKey(name, permissionCheck));
	}

	public IRegistryKey OpenSubKey(string name, bool writable)
	{
		return WrapppedKey(_registryKey.OpenSubKey(name, writable));
	}

	public IRegistryKey OpenSubKey(string name, RegistryRights rights)
	{
		return WrapppedKey(_registryKey.OpenSubKey(name, rights));
	}

	public void SetAccessControl(RegistrySecurity registrySecurity)
	{
		_registryKey.SetAccessControl(registrySecurity);
	}

	public void SetValue(string name, object value)
	{
		_registryKey.SetValue(name, value);
	}

	public void SetValue(string name, object value, RegistryValueKind valueKind)
	{
		_registryKey.SetValue(name, value, valueKind);
	}

	public static IRegistryKey WrapppedKey(RegistryKey key)
	{
		if (key == null)
		{
			return null;
		}
		return new RegistryKeyWrapper(key);
	}
}
