using System;
using System.Diagnostics;
using System.Globalization;
using VideoOS.Common.Integration.Drivers;
using VideoOS.Common.Integration.Properties;

namespace VideoOS.Common.Integration;

[DebuggerDisplay("PtzPreset: {Name}; {Key}; {Id}")]
public class PtzPreset
{
	private int number;

	public Guid Id { get; private set; }

	public string Key { get; private set; }

	public bool IsDevicePreset { get; private set; }

	public bool IsDefault { get; set; }

	public bool IsNumbered { get; private set; }

	public int Number
	{
		get
		{
			if (!IsNumbered)
			{
				throw new NotSupportedException();
			}
			return number;
		}
	}

	public string Name { get; set; }

	public string Description { get; set; }

	public bool Locked { get; set; }

	public PtzArgs PtzArgs { get; private set; }

	public static PtzPreset CreateAbsolutePreset(Guid id, string name, string description, PtzArgs ptzArgs, bool defaultPreset, bool locked)
	{
		string key = id.ToString();
		return new PtzPreset(id, key, name, description, ptzArgs, devicePreset: false, defaultPreset, locked);
	}

	public static PtzPreset CreateNamedDevicePreset(Guid id, string name, string description, bool defaultPreset, bool locked)
	{
		return new PtzPreset(id, name, name, description, new PtzArgs(), devicePreset: true, defaultPreset, locked);
	}

	public static PtzPreset CreateNumberedDevicePreset(Guid id, int number, string name, string description, bool defaultPreset, bool locked)
	{
		string key = number.ToString(CultureInfo.InvariantCulture);
		return new PtzPreset(id, key, number, name, description, new PtzArgs(), devicePreset: true, defaultPreset, locked);
	}

	public PtzPreset(Guid id, string key, int number, string name, string description, PtzArgs ptzArgs, bool devicePreset, bool defaultPreset, bool locked)
	{
		if (ptzArgs == null)
		{
			throw new ArgumentException(Resources.PtzArgsNull);
		}
		Id = id;
		Key = key;
		IsDevicePreset = devicePreset;
		IsDefault = defaultPreset;
		IsNumbered = true;
		this.number = number;
		Name = name;
		Description = description;
		PtzArgs = ptzArgs;
		Locked = locked;
	}

	public PtzPreset(Guid id, string key, string name, string description, PtzArgs ptzArgs, bool devicePreset, bool defaultPreset, bool locked)
	{
		if (ptzArgs == null)
		{
			throw new ArgumentException(Resources.PtzArgsNull);
		}
		Id = id;
		Key = key;
		IsDevicePreset = devicePreset;
		IsDefault = defaultPreset;
		IsNumbered = false;
		number = -1;
		Name = name;
		Description = description;
		PtzArgs = ptzArgs;
		Locked = locked;
	}

	public PtzPreset Clone()
	{
		PtzArgs ptzArgs = PtzArgs.Clone();
		return new PtzPreset(Id, Key, number, Name, Description, ptzArgs, IsDevicePreset, IsDefault, Locked)
		{
			IsNumbered = IsNumbered
		};
	}

	public bool EqualsTo(PtzPreset other)
	{
		if (other == null)
		{
			return false;
		}
		if (!Id.Equals(other.Id))
		{
			return false;
		}
		if (!AreEqual(Description, other.Description))
		{
			return false;
		}
		if (!IsDefault.Equals(other.IsDefault))
		{
			return false;
		}
		if (!AreEqual(Name, other.Name))
		{
			return false;
		}
		if (!Locked.Equals(other.Locked))
		{
			return false;
		}
		if (!IsDevicePreset.Equals(other.IsDevicePreset))
		{
			return false;
		}
		if (!IsNumbered.Equals(other.IsNumbered))
		{
			return false;
		}
		if (IsNumbered && !Number.Equals(other.Number))
		{
			return false;
		}
		if ((PtzArgs == null && other.PtzArgs != null) || (PtzArgs != null && other.PtzArgs == null))
		{
			return false;
		}
		if (PtzArgs != null || other.PtzArgs != null)
		{
			string text = PtzArgs.SaveAsXml();
			string value = other.PtzArgs.SaveAsXml();
			if (!text.Equals(value))
			{
				return false;
			}
		}
		return true;
	}

	private static bool AreEqual(string a, string b)
	{
		if (a == null && b == null)
		{
			return true;
		}
		if ((a == null && b != null) || (a != null && b == null))
		{
			return false;
		}
		return a.Equals(b);
	}
}
