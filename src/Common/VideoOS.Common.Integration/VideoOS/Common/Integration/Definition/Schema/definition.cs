using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Schema;

[Serializable]
[GeneratedCode("xsd", "4.7.2558.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true)]
[XmlRoot(Namespace = "", IsNullable = false)]
public class definition
{
	private driverType driverField;

	private settingsType[] propertiesField;

	private typeType[] typesField;

	private hardwareType hardwareField;

	private currentSchemaId schemaidField;

	public driverType driver
	{
		get
		{
			return driverField;
		}
		set
		{
			driverField = value;
		}
	}

	[XmlArrayItem("settings", IsNullable = false)]
	public settingsType[] properties
	{
		get
		{
			return propertiesField;
		}
		set
		{
			propertiesField = value;
		}
	}

	[XmlArrayItem("type", IsNullable = false)]
	public typeType[] types
	{
		get
		{
			return typesField;
		}
		set
		{
			typesField = value;
		}
	}

	public hardwareType hardware
	{
		get
		{
			return hardwareField;
		}
		set
		{
			hardwareField = value;
		}
	}

	[XmlAttribute]
	public currentSchemaId schemaid
	{
		get
		{
			return schemaidField;
		}
		set
		{
			schemaidField = value;
		}
	}

	public settingsType FindSettingsByHwid(string hwid)
	{
		if (properties == null)
		{
			return null;
		}
		settingsType[] array = properties;
		foreach (settingsType settingsType2 in array)
		{
			if (string.Compare(settingsType2.hwid, hwid, ignoreCase: true) == 0)
			{
				return settingsType2;
			}
		}
		return null;
	}

	public settingType FindHardwareSettingByName(string name)
	{
		if (properties == null)
		{
			return null;
		}
		settingsType[] array = properties;
		foreach (settingsType settingsType2 in array)
		{
			if (string.Compare(settingsType2.hwid, "hardware", ignoreCase: true) != 0)
			{
				continue;
			}
			settingType[] setting = settingsType2.setting;
			foreach (settingType settingType2 in setting)
			{
				if (string.Compare(settingType2.name, name, ignoreCase: true) == 0)
				{
					return settingType2;
				}
			}
		}
		return null;
	}

	public settingType FindHardwareSettingByTypeReferenceId(Guid referenceId)
	{
		string typeName = FindTypeNameByReferenceId(referenceId);
		capabilityType capabilityType2 = hardware.capabilities.FindCapabilityByTypeName(typeName);
		if (capabilityType2 == null)
		{
			return null;
		}
		return FindHardwareSettingByName(capabilityType2.name);
	}

	public typeType FindTypeByName(string name)
	{
		typeType[] array = types;
		foreach (typeType typeType2 in array)
		{
			if (typeType2.name == name)
			{
				return typeType2;
			}
		}
		return null;
	}

	public typeType FindTypeByReferenceId(Guid referenceId)
	{
		string a = referenceId.ToString();
		typeType[] array = types;
		foreach (typeType typeType2 in array)
		{
			if (string.Equals(a, typeType2.referenceid, StringComparison.OrdinalIgnoreCase))
			{
				return typeType2;
			}
		}
		return null;
	}

	public string FindTypeNameByReferenceId(Guid referenceId)
	{
		string a = referenceId.ToString();
		typeType[] array = types;
		foreach (typeType typeType2 in array)
		{
			if (string.Equals(a, typeType2.referenceid, StringComparison.OrdinalIgnoreCase))
			{
				return typeType2.name;
			}
		}
		return null;
	}

	public enumValueType[] GetSupportedProductIds()
	{
		List<enumValueType> list = new List<enumValueType>();
		typeType typeType2 = FindTypeByReferenceId(TypeReferenceIds.ProductId);
		if (typeType2 == null || typeType2.typedef == null || typeType2.typedef.Length < 1 || typeType2.typedef[0].Items == null || typeType2.typedef[0].Items.Length < 1)
		{
			return list.ToArray();
		}
		if (!(typeType2.typedef[0].Items[0] is enumerationType { value: var value }))
		{
			return list.ToArray();
		}
		foreach (enumValueType item in value)
		{
			list.Add(item);
		}
		return list.ToArray();
	}
}
