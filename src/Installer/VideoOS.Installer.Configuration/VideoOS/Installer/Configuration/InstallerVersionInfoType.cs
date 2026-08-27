using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:milestone-systems")]
public class InstallerVersionInfoType
{
	private string majorField;

	private string minorField;

	private string revisionField;

	private string buildField;

	private bool visibleField;

	[XmlElement(DataType = "integer")]
	public string Major
	{
		get
		{
			return majorField;
		}
		set
		{
			majorField = value;
		}
	}

	[XmlElement(DataType = "integer")]
	public string Minor
	{
		get
		{
			return minorField;
		}
		set
		{
			minorField = value;
		}
	}

	public string Revision
	{
		get
		{
			return revisionField;
		}
		set
		{
			revisionField = value;
		}
	}

	[XmlElement(DataType = "integer")]
	public string Build
	{
		get
		{
			return buildField;
		}
		set
		{
			buildField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(true)]
	public bool Visible
	{
		get
		{
			return visibleField;
		}
		set
		{
			visibleField = value;
		}
	}

	public InstallerVersionInfoType()
	{
		visibleField = true;
	}
}
