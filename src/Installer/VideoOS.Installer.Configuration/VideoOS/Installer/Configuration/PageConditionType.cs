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
public class PageConditionType
{
	private PageConditionTypeByPassIf[] byPassIfField;

	private string pageNameField;

	private string nextPageField;

	private string eventField;

	[XmlElement("ByPassIf")]
	public PageConditionTypeByPassIf[] ByPassIf
	{
		get
		{
			return byPassIfField;
		}
		set
		{
			byPassIfField = value;
		}
	}

	[XmlAttribute]
	public string PageName
	{
		get
		{
			return pageNameField;
		}
		set
		{
			pageNameField = value;
		}
	}

	[XmlAttribute]
	public string NextPage
	{
		get
		{
			return nextPageField;
		}
		set
		{
			nextPageField = value;
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
}
