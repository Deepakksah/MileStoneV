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
public class hardwareptzsettingsType : ptzsettingsType
{
	private ptzcomportType[] comportsField;

	private capabilitiesType capabilitiesField;

	private bool enabledField;

	private bool transparentsupportField;

	[XmlArrayItem("comport", IsNullable = false)]
	public ptzcomportType[] comports
	{
		get
		{
			return comportsField;
		}
		set
		{
			comportsField = value;
		}
	}

	public capabilitiesType capabilities
	{
		get
		{
			return capabilitiesField;
		}
		set
		{
			capabilitiesField = value;
		}
	}

	[XmlAttribute]
	public bool enabled
	{
		get
		{
			return enabledField;
		}
		set
		{
			enabledField = value;
		}
	}

	[XmlAttribute]
	public bool transparentsupport
	{
		get
		{
			return transparentsupportField;
		}
		set
		{
			transparentsupportField = value;
		}
	}
}
