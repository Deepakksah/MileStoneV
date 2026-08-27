using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.AccessControl;
using System.Security.Principal;
using Microsoft.Win32;

namespace VideoOS.Mobile.Helpers;

public class ModifyRegistry : IDisposable
{
	private readonly RegistryKey _baseRegistryKey;

	private readonly string _subKeyPath;

	[ExcludeFromCodeCoverage]
	public ModifyRegistry(RegistryHive baseRegistryHive, RegistryView baseRegistryView, string subKeyPath)
	{
		_baseRegistryKey = RegistryKey.OpenBaseKey(baseRegistryHive, baseRegistryView);
		_subKeyPath = subKeyPath;
	}

	[ExcludeFromCodeCoverage]
	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	[ExcludeFromCodeCoverage]
	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_baseRegistryKey.Close();
		}
	}

	[ExcludeFromCodeCoverage]
	public string Read(string keyName)
	{
		return ReadT<string>(keyName);
	}

	public T ReadT<T>(string keyName)
	{
		try
		{
			keyName = keyName?.ToUpper(CultureInfo.InvariantCulture) ?? throw new ArgumentNullException("keyName");
			using RegistryKey registryKey = _baseRegistryKey.OpenSubKey(_subKeyPath);
			return (T)registryKey.GetValue(keyName.ToUpper(CultureInfo.InvariantCulture));
		}
		catch (Exception)
		{
			return default(T);
		}
	}

	public bool Write(string keyName, object value)
	{
		try
		{
			keyName = keyName?.ToUpper(CultureInfo.InvariantCulture) ?? throw new ArgumentNullException("keyName");
			using (RegistryKey registryKey = _baseRegistryKey.CreateSubKey(_subKeyPath, writable: true))
			{
				registryKey.SetValue(keyName, value);
			}
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool DeleteKey(string keyName)
	{
		try
		{
			keyName = keyName?.ToUpper(CultureInfo.InvariantCulture) ?? throw new ArgumentNullException("keyName");
			using (RegistryKey registryKey = _baseRegistryKey.OpenSubKey(_subKeyPath, writable: true))
			{
				registryKey.DeleteValue(keyName);
			}
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool DeleteSubKeyTree(string keyName)
	{
		try
		{
			keyName = keyName?.ToUpper(CultureInfo.InvariantCulture) ?? throw new ArgumentNullException("keyName");
			using (RegistryKey registryKey = _baseRegistryKey.OpenSubKey(_subKeyPath, writable: true))
			{
				registryKey.DeleteSubKeyTree(keyName);
			}
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public string[] GetSubKeyNames()
	{
		try
		{
			using RegistryKey registryKey = _baseRegistryKey.OpenSubKey(_subKeyPath);
			return registryKey?.GetSubKeyNames();
		}
		catch (Exception)
		{
			return null;
		}
	}

	public bool SetAccountRight(WellKnownSidType sId, RegistryRights rights = RegistryRights.FullControl, InheritanceFlags inheritanceFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags propagationFlags = PropagationFlags.None, AccessControlType accessControl = AccessControlType.Allow)
	{
		bool result = false;
		try
		{
			RegistryAccessRule rule = new RegistryAccessRule((new SecurityIdentifier(sId, null).Translate(typeof(NTAccount)) as NTAccount).ToString(), rights, inheritanceFlags, propagationFlags, accessControl);
			using (RegistryKey registryKey = _baseRegistryKey.CreateSubKey(_subKeyPath))
			{
				RegistrySecurity accessControl2 = registryKey.GetAccessControl();
				accessControl2.AddAccessRule(rule);
				registryKey.SetAccessControl(accessControl2);
			}
			result = true;
		}
		catch (Exception)
		{
		}
		return result;
	}

	public bool SetAccountRight(string userName, RegistryRights rights = RegistryRights.FullControl, InheritanceFlags inheritanceFlags = InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags propagationFlags = PropagationFlags.None, AccessControlType accessControl = AccessControlType.Allow)
	{
		bool result = false;
		try
		{
			RegistryAccessRule rule = new RegistryAccessRule(userName, rights, inheritanceFlags, propagationFlags, accessControl);
			using (RegistryKey registryKey = _baseRegistryKey.CreateSubKey(_subKeyPath))
			{
				RegistrySecurity accessControl2 = registryKey.GetAccessControl();
				accessControl2.AddAccessRule(rule);
				registryKey.SetAccessControl(accessControl2);
			}
			result = true;
		}
		catch (Exception)
		{
		}
		return result;
	}
}
