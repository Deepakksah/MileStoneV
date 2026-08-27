using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

namespace VideoOS.Mobile.Helpers;

public class RegistryHelperBase : IRegistryHelper, IDisposable
{
	public delegate string Convertor(string value);

	public delegate bool CheckValue<in T>(T data);

	private ModifyRegistry _registry;

	public static readonly IEnumerable<Guid> ClonnedServerIdsByHusky;

	public string SubKeyName { get; set; }

	[ExcludeFromCodeCoverage]
	public ModifyRegistry Registry
	{
		get
		{
			if (_registry == null)
			{
				_registry = new ModifyRegistry(RegistryHive.LocalMachine, RegistryView.Registry32, SubKeyName);
			}
			return _registry;
		}
	}

	[ExcludeFromCodeCoverage]
	static RegistryHelperBase()
	{
		ClonnedServerIdsByHusky = new List<Guid>
		{
			new Guid("904292d0-5809-417e-8aa2-f9d20ead518c"),
			new Guid("806A069D-D7CC-45EC-8932-1EA951B33D2E")
		};
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_registry.Dispose();
		}
	}

	[ExcludeFromCodeCoverage]
	public string Read(string name)
	{
		return Registry.Read(name);
	}

	[ExcludeFromCodeCoverage]
	public bool Write(string entryName, object entryValue)
	{
		return Registry.Write(entryName, entryValue);
	}

	[ExcludeFromCodeCoverage]
	public bool DeleteKey(string key)
	{
		return Registry.DeleteKey(key);
	}

	[ExcludeFromCodeCoverage]
	public bool DeleteSubKeyTree(string key)
	{
		return Registry.DeleteSubKeyTree(key);
	}

	[ExcludeFromCodeCoverage]
	public bool SetAccountRights(string userName)
	{
		return Registry.SetAccountRight(userName);
	}

	[ExcludeFromCodeCoverage]
	public ModifyRegistry GetExplicitRegistryEntry(RegistryHive hive, string subKey, RegistryView regView = RegistryView.Default)
	{
		return new ModifyRegistry(hive, regView, subKey);
	}

	[ExcludeFromCodeCoverage]
	public ModifyRegistry GetExplicitRegistryEntry(string subKey)
	{
		return new ModifyRegistry(RegistryHive.LocalMachine, RegistryView.Registry32, subKey);
	}

	[ExcludeFromCodeCoverage]
	public ModifyRegistry GetRelativeRegistryEntry(string relativeSubKey)
	{
		relativeSubKey = relativeSubKey ?? throw new ArgumentNullException("relativeSubKey");
		return new ModifyRegistry(RegistryHive.LocalMachine, RegistryView.Registry32, MakeRelativePath(relativeSubKey));
	}

	public string InitValue(string value, string entryName)
	{
		string result = value;
		try
		{
			string text = Registry.Read(entryName);
			if (!string.IsNullOrEmpty(text))
			{
				result = text;
			}
		}
		catch (Exception)
		{
		}
		return result;
	}

	public string InitTextBox(TextBox textBox, string entryName)
	{
		string text = Registry.Read(entryName);
		if (!string.IsNullOrWhiteSpace(text) && textBox != null)
		{
			textBox.Text = text;
		}
		return text;
	}

	public bool HasValue<T>(string entryName, CheckValue<T> checker)
	{
		bool result = false;
		try
		{
			checker = checker ?? throw new ArgumentNullException("checker");
			T data = Registry.ReadT<T>(entryName);
			result = checker(data);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public Guid InitServerId(string name, Guid initialValue)
	{
		Guid guid = initialValue;
		string g = Read(name);
		try
		{
			guid = new Guid(g);
		}
		catch (Exception)
		{
		}
		if (guid == Guid.Empty || ClonnedServerIdsByHusky.Contains(guid))
		{
			guid = Guid.NewGuid();
		}
		Write(name, guid);
		return guid;
	}

	public string MakeRelativePath(string relativeSubKey)
	{
		return SubKeyName + (relativeSubKey.StartsWith("\\", StringComparison.Ordinal) ? relativeSubKey : ("\\" + relativeSubKey));
	}
}
