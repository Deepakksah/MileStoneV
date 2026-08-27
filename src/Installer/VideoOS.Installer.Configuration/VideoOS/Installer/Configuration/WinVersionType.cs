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
public class WinVersionType
{
	private WinVersionTypeVersionsToExclude[] versionsToExcludeField;

	private bool excludeBelow10_0Field;

	[XmlElement("VersionsToExclude")]
	public WinVersionTypeVersionsToExclude[] VersionsToExclude
	{
		get
		{
			return versionsToExcludeField;
		}
		set
		{
			versionsToExcludeField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(true)]
	public bool ExcludeBelow10_0
	{
		get
		{
			return excludeBelow10_0Field;
		}
		set
		{
			excludeBelow10_0Field = value;
		}
	}

	public WinVersionType()
	{
		excludeBelow10_0Field = true;
	}
}
