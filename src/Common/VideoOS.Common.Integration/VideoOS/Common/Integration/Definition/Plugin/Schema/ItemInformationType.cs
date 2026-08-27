using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Plugin.Schema;

[Serializable]
[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://VideoOS.net/Surveillance/XmlSchema")]
public class ItemInformationType : InformationType
{
	private double versionField;

	private bool versionFieldSpecified;

	private string fullversionField;

	private string serialnumberField;

	public double version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}

	[XmlIgnore]
	public bool versionSpecified
	{
		get
		{
			return versionFieldSpecified;
		}
		set
		{
			versionFieldSpecified = value;
		}
	}

	public string fullversion
	{
		get
		{
			return fullversionField;
		}
		set
		{
			fullversionField = value;
		}
	}

	public string serialnumber
	{
		get
		{
			return serialnumberField;
		}
		set
		{
			serialnumberField = value;
		}
	}
}
