using System;
using System.Security.AccessControl;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace VideoOS.Common.EnvironmentServices;

public interface IRegistryKey : IDisposable
{
	string Name { get; }

	SafeRegistryHandle Handle { get; }

	RegistryView View { get; }

	int ValueCount { get; }

	void Close();

	IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions registryOptions, RegistrySecurity registrySecurity);

	IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistrySecurity registrySecurity);

	IRegistryKey CreateSubKey(string subkey, bool writable, RegistryOptions options);

	IRegistryKey CreateSubKey(string subkey, bool writable);

	IRegistryKey CreateSubKey(string subkey);

	IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck);

	IRegistryKey CreateSubKey(string subkey, RegistryKeyPermissionCheck permissionCheck, RegistryOptions options);

	void DeleteSubKey(string subkey, bool throwOnMissingSubKey);

	void DeleteSubKey(string subkey);

	void DeleteSubKeyTree(string subkey);

	void DeleteSubKeyTree(string subkey, bool throwOnMissingSubKey);

	void DeleteValue(string name);

	void DeleteValue(string name, bool throwOnMissingValue);

	void Flush();

	RegistrySecurity GetAccessControl(AccessControlSections includeSections);

	RegistrySecurity GetAccessControl();

	string[] GetSubKeyNames();

	object GetValue(string name, object defaultValue);

	object GetValue(string name, object defaultValue, RegistryValueOptions options);

	object GetValue(string name);

	RegistryValueKind GetValueKind(string name);

	string[] GetValueNames();

	IRegistryKey OpenSubKey(string name);

	IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck, RegistryRights rights);

	IRegistryKey OpenSubKey(string name, RegistryKeyPermissionCheck permissionCheck);

	IRegistryKey OpenSubKey(string name, bool writable);

	IRegistryKey OpenSubKey(string name, RegistryRights rights);

	void SetAccessControl(RegistrySecurity registrySecurity);

	void SetValue(string name, object value);

	void SetValue(string name, object value, RegistryValueKind valueKind);

	new string ToString();
}
