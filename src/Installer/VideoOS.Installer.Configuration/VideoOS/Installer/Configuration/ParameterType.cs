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
public class ParameterType
{
	private string descriptionField;

	private PackageRefType[] targetPackagesField;

	private string nameField;

	private string defaultValueField;

	private bool suppressInArgumentsFileField;

	public string Description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}

	[XmlArrayItem("PackageRef", IsNullable = false)]
	public PackageRefType[] TargetPackages
	{
		get
		{
			return targetPackagesField;
		}
		set
		{
			targetPackagesField = value;
		}
	}

	[XmlAttribute]
	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	[XmlAttribute]
	public string DefaultValue
	{
		get
		{
			return defaultValueField;
		}
		set
		{
			defaultValueField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(false)]
	public bool SuppressInArgumentsFile
	{
		get
		{
			return suppressInArgumentsFileField;
		}
		set
		{
			suppressInArgumentsFileField = value;
		}
	}

	public ParameterType()
	{
		suppressInArgumentsFileField = false;
	}
}
