using System;
using System.Windows.Forms;
using Microsoft.Win32;

namespace VideoOS.Mobile.Helpers;

public interface IRegistryHelper
{
	string SubKeyName { get; set; }

	ModifyRegistry Registry { get; }

	bool DeleteKey(string key);

	bool DeleteSubKeyTree(string key);

	ModifyRegistry GetExplicitRegistryEntry(RegistryHive hive, string subKey, RegistryView regView = RegistryView.Default);

	ModifyRegistry GetExplicitRegistryEntry(string subKey);

	ModifyRegistry GetRelativeRegistryEntry(string relativeSubKey);

	bool HasValue<T>(string entryName, RegistryHelperBase.CheckValue<T> checker);

	Guid InitServerId(string name, Guid initialValue);

	string InitTextBox(TextBox textBox, string entryName);

	string InitValue(string value, string entryName);

	string Read(string name);

	bool SetAccountRights(string userName);

	bool Write(string entryName, object entryValue);
}
