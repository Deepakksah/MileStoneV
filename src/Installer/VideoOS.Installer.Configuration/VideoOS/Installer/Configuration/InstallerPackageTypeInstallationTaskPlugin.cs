using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
public class InstallerPackageTypeInstallationTaskPlugin
{
	private XmlElement taskField;

	private string filenameField;

	private FormatType formatField;

	private bool formatFieldSpecified;

	private string versionField;

	public XmlElement Task
	{
		get
		{
			return taskField;
		}
		set
		{
			taskField = value;
		}
	}

	[XmlAttribute]
	public string Filename
	{
		get
		{
			return filenameField;
		}
		set
		{
			filenameField = value;
		}
	}

	[XmlAttribute]
	public FormatType Format
	{
		get
		{
			return formatField;
		}
		set
		{
			formatField = value;
		}
	}

	[XmlIgnore]
	public bool FormatSpecified
	{
		get
		{
			return formatFieldSpecified;
		}
		set
		{
			formatFieldSpecified = value;
		}
	}

	[XmlAttribute]
	public string Version
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

	public bool IsInstalled { get; set; }

	public object InstallationTask { get; set; }

	public object UninstallationTask { get; set; }
}
