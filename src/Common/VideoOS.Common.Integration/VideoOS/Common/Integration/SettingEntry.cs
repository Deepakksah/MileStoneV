using System;
using System.Diagnostics;

namespace VideoOS.Common.Integration;

[DebuggerDisplay("{DebuggerDisplay},nq")]
public class SettingEntry
{
	public string Section { get; set; }

	public string Name { get; set; }

	public string TypeId { get; set; }

	public string Value { get; set; }

	private string DebuggerDisplay => FormattableString.Invariant($"SettingEntry. {Section} - {Name} = {Value}");

	public SettingEntry(string section, string name, string value)
	{
		Section = section;
		Name = name;
		Value = value;
	}

	public SettingEntry(string section, string name, string value, string typeId)
	{
		Section = section;
		Name = name;
		Value = value;
		TypeId = typeId;
	}

	public SettingEntry Clone()
	{
		return new SettingEntry(Section, Name, Value, TypeId);
	}
}
