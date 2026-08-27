using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Schema;

[Serializable]
[GeneratedCode("xsd", "4.7.2558.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
public class settingsType
{
	private settingType[] settingField;

	private string hwidField;

	[XmlElement("setting")]
	public settingType[] setting
	{
		get
		{
			return settingField;
		}
		set
		{
			settingField = value;
		}
	}

	[XmlAttribute]
	public string hwid
	{
		get
		{
			return hwidField;
		}
		set
		{
			hwidField = value;
		}
	}

	public settingType FindSettingByName(string name)
	{
		if (setting == null)
		{
			return null;
		}
		settingType[] array = setting;
		foreach (settingType settingType2 in array)
		{
			if (string.Compare(settingType2.name, name, ignoreCase: true) == 0)
			{
				return settingType2;
			}
		}
		return null;
	}
}
