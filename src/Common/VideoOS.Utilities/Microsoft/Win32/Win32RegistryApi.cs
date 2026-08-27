using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Microsoft.Win32;

public class Win32RegistryApi : IRegistryApi
{
	public struct SECURITY_ATTRIBUTES
	{
		public int nLength;

		public IntPtr lpSecurityDescriptor;

		public bool bInheritHandle;
	}

	private const int OpenRegKeyRead = 131097;

	private const int OpenRegKeyWrite = 131078;

	private const int Open64BitKey = 256;

	private const int Open32BitKey = 512;

	private const int Int32ByteSize = 4;

	private readonly int NativeBytesPerCharacter = Marshal.SystemDefaultCharSize;

	private const int BufferMaxLength = 1024;

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegCreateKey(IntPtr keyBase, string keyName, out IntPtr keyHandle);

	[DllImport("advapi32.dll", SetLastError = true)]
	private static extern int RegCreateKeyEx(IntPtr keyBase, string keyName, int reserved, string keyclass, int options, int access, int lpSecurityAttributes, out IntPtr keyHandle, out int lpdwDisposition);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegCloseKey(IntPtr keyHandle);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegFlushKey(IntPtr keyHandle);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegOpenKeyEx(IntPtr keyBase, string keyName, int reserved, int access, out IntPtr keyHandle);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegDeleteKey(IntPtr keyHandle, string valueName);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegDeleteValue(IntPtr keyHandle, string valueName);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegEnumKey(IntPtr keyBase, int index, StringBuilder nameBuffer, int bufferLength);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegEnumValue(IntPtr keyBase, int index, StringBuilder nameBuffer, ref int nameLength, IntPtr reserved, ref RegistryValueKind type, IntPtr data, IntPtr dataLength);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegSetValueEx(IntPtr keyBase, string valueName, int reserved, RegistryValueKind type, StringBuilder data, int rawDataLength);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegSetValueEx(IntPtr keyBase, string valueName, int reserved, RegistryValueKind type, string data, int rawDataLength);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegSetValueEx(IntPtr keyBase, string valueName, int reserved, RegistryValueKind type, byte[] rawData, int rawDataLength);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegSetValueEx(IntPtr keyBase, string valueName, int reserved, RegistryValueKind type, ref int data, int rawDataLength);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegQueryValueEx(IntPtr keyBase, string valueName, IntPtr reserved, ref RegistryValueKind type, IntPtr zero, ref int dataSize);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegQueryValueEx(IntPtr keyBase, string valueName, IntPtr reserved, ref RegistryValueKind type, [Out] byte[] data, ref int dataSize);

	[DllImport("advapi32.dll", CharSet = CharSet.Unicode)]
	private static extern int RegQueryValueEx(IntPtr keyBase, string valueName, IntPtr reserved, ref RegistryValueKind type, ref int data, ref int dataSize);

	private static IntPtr GetHandle(RegistryKeyEx key)
	{
		if (!key.IsRoot)
		{
			return (IntPtr)key.Data;
		}
		return new IntPtr((int)key.Data);
	}

	private static bool IsHandleValid(RegistryKeyEx key)
	{
		return key.Data != null;
	}

	public object GetValue(RegistryKeyEx rkey, string name, object defaultValue, RegistryValueOptions options)
	{
		RegistryValueKind type = RegistryValueKind.Unknown;
		int dataSize = 0;
		object obj = null;
		IntPtr handle = GetHandle(rkey);
		int num = RegQueryValueEx(handle, name, IntPtr.Zero, ref type, IntPtr.Zero, ref dataSize);
		switch (num)
		{
		case 2:
		case 1018:
			return defaultValue;
		default:
			GenerateException(num);
			break;
		case 0:
		case 234:
			break;
		}
		switch (type)
		{
		case RegistryValueKind.String:
		{
			num = GetBinaryValue(rkey, name, type, out var data2, dataSize);
			obj = RegistryKeyEx.DecodeString(data2);
			break;
		}
		case RegistryValueKind.ExpandString:
		{
			num = GetBinaryValue(rkey, name, type, out var data5, dataSize);
			obj = RegistryKeyEx.DecodeString(data5);
			if ((options & RegistryValueOptions.DoNotExpandEnvironmentNames) == 0)
			{
				obj = Environment.ExpandEnvironmentVariables((string)obj);
			}
			break;
		}
		case RegistryValueKind.DWord:
		{
			int data4 = 0;
			num = RegQueryValueEx(handle, name, IntPtr.Zero, ref type, ref data4, ref dataSize);
			obj = data4;
			break;
		}
		case RegistryValueKind.Binary:
		{
			num = GetBinaryValue(rkey, name, type, out var data3, dataSize);
			obj = data3;
			break;
		}
		case RegistryValueKind.MultiString:
		{
			obj = null;
			num = GetBinaryValue(rkey, name, type, out var data, dataSize);
			if (num == 0)
			{
				obj = RegistryKeyEx.DecodeString(data).Split(default(char));
			}
			break;
		}
		default:
			throw new SystemException();
		}
		if (num != 0)
		{
			GenerateException(num);
		}
		return obj;
	}

	public void SetValue(RegistryKeyEx rkey, string name, object value, RegistryValueKind valueKind)
	{
		Type type = value.GetType();
		IntPtr handle = GetHandle(rkey);
		int num;
		if (valueKind == RegistryValueKind.DWord && type == typeof(int))
		{
			int data = (int)value;
			num = RegSetValueEx(handle, name, 0, RegistryValueKind.DWord, ref data, 4);
		}
		else if (valueKind == RegistryValueKind.Binary && type == typeof(byte[]))
		{
			byte[] array = (byte[])value;
			num = RegSetValueEx(handle, name, 0, RegistryValueKind.Binary, array, array.Length);
		}
		else if (valueKind == RegistryValueKind.MultiString && type == typeof(string[]))
		{
			string[] obj = (string[])value;
			StringBuilder stringBuilder = new StringBuilder();
			string[] array2 = obj;
			foreach (string value2 in array2)
			{
				stringBuilder.Append(value2);
				stringBuilder.Append('\0');
			}
			stringBuilder.Append('\0');
			byte[] bytes = Encoding.Unicode.GetBytes(stringBuilder.ToString());
			num = RegSetValueEx(handle, name, 0, RegistryValueKind.MultiString, bytes, bytes.Length);
		}
		else
		{
			if ((valueKind != RegistryValueKind.String && valueKind != RegistryValueKind.ExpandString) || !(type == typeof(string)))
			{
				if (type.IsArray)
				{
					throw new ArgumentException("Only string and byte arrays can written as registry values");
				}
				throw new ArgumentException("Type does not match the valueKind");
			}
			string text = string.Format(CultureInfo.InvariantCulture, "{0}{1}", value, '\0');
			num = RegSetValueEx(handle, name, 0, valueKind, text, text.Length * NativeBytesPerCharacter);
		}
		if (num != 0)
		{
			GenerateException(num);
		}
	}

	public void SetValue(RegistryKeyEx rkey, string name, object value)
	{
		Type type = value.GetType();
		IntPtr handle = GetHandle(rkey);
		int num;
		if (type == typeof(int))
		{
			int data = (int)value;
			num = RegSetValueEx(handle, name, 0, RegistryValueKind.DWord, ref data, 4);
		}
		else if (type == typeof(byte[]))
		{
			byte[] array = (byte[])value;
			num = RegSetValueEx(handle, name, 0, RegistryValueKind.Binary, array, array.Length);
		}
		else if (type == typeof(string[]))
		{
			string[] obj = (string[])value;
			StringBuilder stringBuilder = new StringBuilder();
			string[] array2 = obj;
			foreach (string value2 in array2)
			{
				stringBuilder.Append(value2);
				stringBuilder.Append('\0');
			}
			stringBuilder.Append('\0');
			byte[] bytes = Encoding.Unicode.GetBytes(stringBuilder.ToString());
			num = RegSetValueEx(handle, name, 0, RegistryValueKind.MultiString, bytes, bytes.Length);
		}
		else
		{
			if (type.IsArray)
			{
				throw new ArgumentException("Only string and byte arrays can written as registry values");
			}
			string text = string.Format(CultureInfo.InvariantCulture, "{0}{1}", value, '\0');
			num = RegSetValueEx(handle, name, 0, RegistryValueKind.String, text, text.Length * NativeBytesPerCharacter);
		}
		switch (num)
		{
		case 1018:
			throw RegistryKeyEx.CreateMarkedForDeletionException();
		case 0:
			return;
		}
		GenerateException(num);
	}

	private int GetBinaryValue(RegistryKeyEx rkey, string name, RegistryValueKind type, out byte[] data, int size)
	{
		byte[] array = new byte[size];
		int result = RegQueryValueEx(GetHandle(rkey), name, IntPtr.Zero, ref type, array, ref size);
		data = array;
		return result;
	}

	public int SubKeyCount(RegistryKeyEx rkey)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		IntPtr handle = GetHandle(rkey);
		int num = 0;
		while (true)
		{
			int num2 = RegEnumKey(handle, num, stringBuilder, stringBuilder.Capacity);
			switch (num2)
			{
			case 1018:
				throw RegistryKeyEx.CreateMarkedForDeletionException();
			default:
				GenerateException(num2);
				break;
			case 0:
				break;
			case 259:
				return num;
			}
			num++;
		}
	}

	public int ValueCount(RegistryKeyEx rkey)
	{
		StringBuilder stringBuilder = new StringBuilder(1024);
		IntPtr handle = GetHandle(rkey);
		int num = 0;
		while (true)
		{
			RegistryValueKind type = RegistryValueKind.Unknown;
			int nameLength = stringBuilder.Capacity;
			int num2 = RegEnumValue(handle, num, stringBuilder, ref nameLength, IntPtr.Zero, ref type, IntPtr.Zero, IntPtr.Zero);
			switch (num2)
			{
			case 1018:
				throw RegistryKeyEx.CreateMarkedForDeletionException();
			default:
				GenerateException(num2);
				break;
			case 0:
			case 234:
				break;
			case 259:
				return num;
			}
			num++;
		}
	}

	public RegistryKeyEx OpenSubKey(RegistryKeyEx rkey, string keyName, bool writable, bool access64bitKey)
	{
		int num = 131097;
		if (writable)
		{
			num |= 0x20006;
		}
		num = ((!access64bitKey) ? (num | 0x200) : (num | 0x100));
		IntPtr keyHandle;
		int num2 = RegOpenKeyEx(GetHandle(rkey), keyName, 0, num, out keyHandle);
		switch (num2)
		{
		case 2:
		case 1018:
			return null;
		default:
			GenerateException(num2);
			break;
		case 0:
			break;
		}
		return new RegistryKeyEx(keyHandle, CombineName(rkey, keyName), writable);
	}

	public void Flush(RegistryKeyEx rkey)
	{
		if (IsHandleValid(rkey))
		{
			RegFlushKey(GetHandle(rkey));
		}
	}

	public void Close(RegistryKeyEx rkey)
	{
		if (IsHandleValid(rkey))
		{
			RegCloseKey(GetHandle(rkey));
		}
	}

	public RegistryKeyEx CreateSubKey(RegistryKeyEx rkey, string keyName, bool create64BitKey)
	{
		IntPtr handle = GetHandle(rkey);
		int num = 131103;
		num = ((!create64BitKey) ? (num | 0x200) : (num | 0x100));
		SECURITY_ATTRIBUTES structure = default(SECURITY_ATTRIBUTES);
		structure.nLength = Marshal.SizeOf(structure);
		structure.lpSecurityDescriptor = new IntPtr(0);
		structure.bInheritHandle = true;
		IntPtr keyHandle;
		int lpdwDisposition;
		int num2 = RegCreateKeyEx(handle, keyName, 0, null, 0, num, 0, out keyHandle, out lpdwDisposition);
		switch (num2)
		{
		case 1018:
			throw RegistryKeyEx.CreateMarkedForDeletionException();
		default:
			GenerateException(num2);
			break;
		case 0:
			break;
		}
		return new RegistryKeyEx(keyHandle, CombineName(rkey, keyName), writable: true);
	}

	public void DeleteKey(RegistryKeyEx rkey, string keyName, bool shouldThrowWhenKeyMissing)
	{
		int num = RegDeleteKey(GetHandle(rkey), keyName);
		switch (num)
		{
		case 2:
			if (shouldThrowWhenKeyMissing)
			{
				throw new ArgumentException("key " + keyName);
			}
			break;
		default:
			GenerateException(num);
			break;
		case 0:
			break;
		}
	}

	public void DeleteValue(RegistryKeyEx rkey, string value, bool shouldThrowWhenKeyMissing)
	{
		int num = RegDeleteValue(GetHandle(rkey), value);
		switch (num)
		{
		case 0:
		case 1018:
			break;
		case 2:
			if (shouldThrowWhenKeyMissing)
			{
				throw new ArgumentException("value " + value);
			}
			break;
		default:
			GenerateException(num);
			break;
		}
	}

	public string[] GetSubKeyNames(RegistryKeyEx rkey)
	{
		IntPtr handle = GetHandle(rkey);
		StringBuilder stringBuilder = new StringBuilder(1024);
		ArrayList arrayList = new ArrayList();
		int num = 0;
		while (true)
		{
			int num2 = RegEnumKey(handle, num, stringBuilder, stringBuilder.Capacity);
			switch (num2)
			{
			case 0:
				arrayList.Add(stringBuilder.ToString());
				stringBuilder.Length = 0;
				break;
			default:
				GenerateException(num2);
				break;
			case 259:
				return (string[])arrayList.ToArray(typeof(string));
			}
			num++;
		}
	}

	public string[] GetValueNames(RegistryKeyEx rkey)
	{
		IntPtr handle = GetHandle(rkey);
		ArrayList arrayList = new ArrayList();
		int num = 0;
		while (true)
		{
			StringBuilder stringBuilder = new StringBuilder(1024);
			int nameLength = stringBuilder.Capacity;
			RegistryValueKind type = RegistryValueKind.Unknown;
			int num2 = RegEnumValue(handle, num, stringBuilder, ref nameLength, IntPtr.Zero, ref type, IntPtr.Zero, IntPtr.Zero);
			switch (num2)
			{
			case 0:
			case 234:
				arrayList.Add(stringBuilder.ToString());
				break;
			case 1018:
				throw RegistryKeyEx.CreateMarkedForDeletionException();
			default:
				GenerateException(num2);
				break;
			case 259:
				return (string[])arrayList.ToArray(typeof(string));
			}
			num++;
		}
	}

	private void GenerateException(int errorCode)
	{
		switch (errorCode)
		{
		case 2:
		case 87:
			throw new ArgumentException();
		case 5:
			throw new SecurityException();
		default:
			throw new SystemException();
		}
	}

	public string ToString(RegistryKeyEx rkey)
	{
		IntPtr handle = GetHandle(rkey);
		return string.Format(CultureInfo.InvariantCulture, "{0} [0x{1:X}]", rkey.Name, handle.ToInt32());
	}

	public static string CombineName(RegistryKeyEx rkey, string localName)
	{
		return rkey.Name + "\\" + localName;
	}
}
