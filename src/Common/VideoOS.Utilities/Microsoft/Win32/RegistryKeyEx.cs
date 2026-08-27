using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.Win32;

public sealed class RegistryKeyEx : MarshalByRefObject, IDisposable
{
	public object Data;

	private readonly string qname;

	private readonly bool isRoot;

	private readonly bool isWritable;

	private static readonly IRegistryApi RegistryApi;

	public bool IsRoot => isRoot;

	public bool IsWritable => isWritable;

	public string Name => qname;

	public int SubKeyCount
	{
		get
		{
			AssertKeyStillValid();
			return RegistryApi.SubKeyCount(this);
		}
	}

	public int ValueCount
	{
		get
		{
			AssertKeyStillValid();
			return RegistryApi.ValueCount(this);
		}
	}

	static RegistryKeyEx()
	{
		RegistryApi = new Win32RegistryApi();
	}

	public RegistryKeyEx(RegistryHive hiveId, string keyName)
	{
		Data = hiveId;
		qname = keyName;
		isRoot = true;
		isWritable = true;
	}

	public RegistryKeyEx(object data, string keyName, bool writable)
	{
		Data = data;
		qname = keyName;
		isRoot = false;
		isWritable = writable;
	}

	void IDisposable.Dispose()
	{
		GC.SuppressFinalize(this);
		Close();
	}

	~RegistryKeyEx()
	{
		Close();
	}

	public void Flush()
	{
		RegistryApi.Flush(this);
	}

	public void Close()
	{
		Flush();
		if (!isRoot)
		{
			RegistryApi.Close(this);
			Data = null;
		}
	}

	public void SetValue(string name, object value)
	{
		AssertKeyStillValid();
		if (value == null)
		{
			throw new ArgumentNullException("value");
		}
		if (!IsWritable)
		{
			throw new UnauthorizedAccessException("Cannot write to the registry key.");
		}
		RegistryApi.SetValue(this, name, value);
	}

	[ComVisible(false)]
	public void SetValue(string name, object value, RegistryValueKind valueKind)
	{
		AssertKeyStillValid();
		if (value == null)
		{
			throw new ArgumentNullException();
		}
		if (!IsWritable)
		{
			throw new UnauthorizedAccessException("Cannot write to the registry key.");
		}
		RegistryApi.SetValue(this, name, value, valueKind);
	}

	public RegistryKeyEx OpenSubKey(string keyName)
	{
		return OpenSubKey(keyName, writable: false);
	}

	public RegistryKeyEx OpenSubKey(string keyName, bool writable)
	{
		AssertKeyStillValid();
		AssertKeyNameNotNull(keyName);
		return RegistryApi.OpenSubKey(this, keyName, writable, access64bitKey: false);
	}

	public RegistryKeyEx OpenSubKey(string keyName, bool writable, bool access64bitKey)
	{
		AssertKeyStillValid();
		AssertKeyNameNotNull(keyName);
		return RegistryApi.OpenSubKey(this, keyName, writable, access64bitKey);
	}

	public object GetValue(string name)
	{
		return GetValue(name, null);
	}

	public object GetValue(string name, object defaultValue)
	{
		AssertKeyStillValid();
		return RegistryApi.GetValue(this, name, defaultValue, RegistryValueOptions.None);
	}

	[ComVisible(false)]
	public object GetValue(string name, object defaultValue, RegistryValueOptions options)
	{
		AssertKeyStillValid();
		return RegistryApi.GetValue(this, name, defaultValue, options);
	}

	public RegistryKeyEx CreateSubKey(string subkey, bool create64BitKey)
	{
		AssertKeyStillValid();
		AssertKeyNameNotNull(subkey);
		if (subkey.Length > 255)
		{
			throw new ArgumentException("keyName length is larger than 255 characters", subkey);
		}
		if (!IsWritable)
		{
			throw new UnauthorizedAccessException("Cannot write to the registry key.");
		}
		return RegistryApi.CreateSubKey(this, subkey, create64BitKey);
	}

	public void DeleteSubKey(string subkey)
	{
		DeleteSubKey(subkey, throwOnMissingSubKey: true);
	}

	public void DeleteSubKey(string subkey, bool throwOnMissingSubKey)
	{
		AssertKeyStillValid();
		AssertKeyNameNotNull(subkey);
		if (!IsWritable)
		{
			throw new UnauthorizedAccessException("Cannot write to the registry key.");
		}
		RegistryKeyEx registryKeyEx = OpenSubKey(subkey);
		if (registryKeyEx == null)
		{
			if (throwOnMissingSubKey)
			{
				throw new ArgumentException("Cannot delete a subkey tree because the subkey does not exist.");
			}
			return;
		}
		if (registryKeyEx.SubKeyCount > 0)
		{
			throw new InvalidOperationException("Registry key has subkeys and recursive removes are not supported by this method.");
		}
		registryKeyEx.Close();
		RegistryApi.DeleteKey(this, subkey, throwOnMissingSubKey);
	}

	public void DeleteSubKeyTree(string keyName)
	{
		AssertKeyStillValid();
		AssertKeyNameNotNull(keyName);
		RegistryKeyEx obj = OpenSubKey(keyName, writable: true) ?? throw new ArgumentException("Cannot delete a subkey tree because the subkey does not exist.");
		obj.DeleteChildKeysAndValues();
		obj.Close();
		DeleteSubKey(keyName, throwOnMissingSubKey: false);
	}

	public void DeleteValue(string value)
	{
		DeleteValue(value, shouldThrowWhenKeyMissing: true);
	}

	public void DeleteValue(string value, bool shouldThrowWhenKeyMissing)
	{
		AssertKeyStillValid();
		AssertKeyNameNotNull(value);
		if (!IsWritable)
		{
			throw new UnauthorizedAccessException("Cannot write to the registry key.");
		}
		RegistryApi.DeleteValue(this, value, shouldThrowWhenKeyMissing);
	}

	public string[] GetSubKeyNames()
	{
		AssertKeyStillValid();
		return RegistryApi.GetSubKeyNames(this);
	}

	public string[] GetValueNames()
	{
		AssertKeyStillValid();
		return RegistryApi.GetValueNames(this);
	}

	public override string ToString()
	{
		return RegistryApi.ToString(this);
	}

	private void AssertKeyStillValid()
	{
		if (Data == null)
		{
			throw new ObjectDisposedException("Microsoft.Win32.RegistryKeyEx");
		}
	}

	private void AssertKeyNameNotNull(string subKeyName)
	{
		if (subKeyName == null)
		{
			throw new ArgumentNullException();
		}
	}

	private void DeleteChildKeysAndValues()
	{
		if (!isRoot)
		{
			string[] subKeyNames = GetSubKeyNames();
			foreach (string text in subKeyNames)
			{
				RegistryKeyEx registryKeyEx = OpenSubKey(text, writable: true);
				registryKeyEx.DeleteChildKeysAndValues();
				registryKeyEx.Close();
				DeleteSubKey(text, throwOnMissingSubKey: false);
			}
			subKeyNames = GetValueNames();
			foreach (string value in subKeyNames)
			{
				DeleteValue(value, shouldThrowWhenKeyMissing: false);
			}
		}
	}

	public static string DecodeString(byte[] data)
	{
		string text = Encoding.Unicode.GetString(data);
		if (text.IndexOf('\0') != -1)
		{
			text = text.TrimEnd(default(char));
		}
		return text;
	}

	public static IOException CreateMarkedForDeletionException()
	{
		throw new IOException("Illegal operation attempted on a registry key that has been marked for deletion.");
	}
}
