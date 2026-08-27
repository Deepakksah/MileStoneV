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
public class TreckTypeFinishSetupOpenLink
{
	private LinkTitleType linkTitleField;

	private string linkField;

	private bool checkedField;

	[XmlAttribute]
	[DefaultValue(LinkTitleType.GettingStarted)]
	public LinkTitleType LinkTitle
	{
		get
		{
			return linkTitleField;
		}
		set
		{
			linkTitleField = value;
		}
	}

	[XmlAttribute]
	public string Link
	{
		get
		{
			return linkField;
		}
		set
		{
			linkField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(true)]
	public bool Checked
	{
		get
		{
			return checkedField;
		}
		set
		{
			checkedField = value;
		}
	}

	public TreckTypeFinishSetupOpenLink()
	{
		linkTitleField = LinkTitleType.GettingStarted;
		checkedField = true;
	}
}
