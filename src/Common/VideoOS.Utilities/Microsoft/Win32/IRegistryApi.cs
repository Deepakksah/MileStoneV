namespace Microsoft.Win32;

public interface IRegistryApi
{
	RegistryKeyEx CreateSubKey(RegistryKeyEx rkey, string keyname, bool create64BitKey);

	RegistryKeyEx OpenSubKey(RegistryKeyEx rkey, string keyname, bool writable, bool access64bitKey);

	void Flush(RegistryKeyEx rkey);

	void Close(RegistryKeyEx rkey);

	object GetValue(RegistryKeyEx rkey, string name, object default_value, RegistryValueOptions options);

	void SetValue(RegistryKeyEx rkey, string name, object value);

	int SubKeyCount(RegistryKeyEx rkey);

	int ValueCount(RegistryKeyEx rkey);

	void DeleteValue(RegistryKeyEx rkey, string value, bool throw_if_missing);

	void DeleteKey(RegistryKeyEx rkey, string keyName, bool throw_if_missing);

	string[] GetSubKeyNames(RegistryKeyEx rkey);

	string[] GetValueNames(RegistryKeyEx rkey);

	string ToString(RegistryKeyEx rkey);

	void SetValue(RegistryKeyEx rkey, string name, object value, RegistryValueKind valueKind);
}
