using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration.TreckDescriptions;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:milestone-systems")]
public class TreckDescriptionType
{
	private string buttonNameField;

	private string sideTextField;

	private string idField;

	public string ButtonName
	{
		get
		{
			return buttonNameField;
		}
		set
		{
			buttonNameField = value;
		}
	}

	public string SideText
	{
		get
		{
			return sideTextField;
		}
		set
		{
			sideTextField = value;
		}
	}

	[XmlAttribute]
	public string Id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}
}
