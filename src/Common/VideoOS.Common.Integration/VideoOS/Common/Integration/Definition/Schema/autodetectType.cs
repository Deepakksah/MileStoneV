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
public class autodetectType
{
	private string defaultloginnameField;

	private string defaultloginpasswordField;

	private string[] macaddressidentifiersField;

	private implementationType implementationField;

	public string defaultloginname
	{
		get
		{
			return defaultloginnameField;
		}
		set
		{
			defaultloginnameField = value;
		}
	}

	public string defaultloginpassword
	{
		get
		{
			return defaultloginpasswordField;
		}
		set
		{
			defaultloginpasswordField = value;
		}
	}

	[XmlArrayItem("macaddressidentifier", IsNullable = false)]
	public string[] macaddressidentifiers
	{
		get
		{
			return macaddressidentifiersField;
		}
		set
		{
			macaddressidentifiersField = value;
		}
	}

	public implementationType implementation
	{
		get
		{
			return implementationField;
		}
		set
		{
			implementationField = value;
		}
	}
}
