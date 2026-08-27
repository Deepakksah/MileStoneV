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
[XmlType(AnonymousType = true, Namespace = "urn:milestone-systems")]
public class InstallerPluginTypeFlowStep
{
	private string pageField;

	private string controlField;

	private string eventField;

	private string valueField;

	[XmlAttribute]
	public string Page
	{
		get
		{
			return pageField;
		}
		set
		{
			pageField = value;
		}
	}

	[XmlAttribute]
	public string Control
	{
		get
		{
			return controlField;
		}
		set
		{
			controlField = value;
		}
	}

	[XmlAttribute]
	public string Event
	{
		get
		{
			return eventField;
		}
		set
		{
			eventField = value;
		}
	}

	[XmlAttribute]
	public string Value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
		}
	}
}
