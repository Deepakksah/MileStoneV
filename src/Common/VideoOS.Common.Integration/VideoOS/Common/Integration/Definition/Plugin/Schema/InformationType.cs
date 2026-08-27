using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Plugin.Schema;

[Serializable]
[XmlInclude(typeof(capabilityType))]
[XmlInclude(typeof(ItemInformationType))]
[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://VideoOS.net/Surveillance/XmlSchema")]
public class InformationType
{
	private string nameField;

	private string referenceidField;

	private string tagField;

	private displayname[] displaynameField;

	private InformationTypeUsedbyproductids usedbyproductidsField;

	private InformationTypeNotusedbyproductids notusedbyproductidsField;

	public string name
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

	public string referenceid
	{
		get
		{
			return referenceidField;
		}
		set
		{
			referenceidField = value;
		}
	}

	public string tag
	{
		get
		{
			return tagField;
		}
		set
		{
			tagField = value;
		}
	}

	[XmlElement("displayname")]
	public displayname[] displayname
	{
		get
		{
			return displaynameField;
		}
		set
		{
			displaynameField = value;
		}
	}

	public InformationTypeUsedbyproductids usedbyproductids
	{
		get
		{
			return usedbyproductidsField;
		}
		set
		{
			usedbyproductidsField = value;
		}
	}

	public InformationTypeNotusedbyproductids notusedbyproductids
	{
		get
		{
			return notusedbyproductidsField;
		}
		set
		{
			notusedbyproductidsField = value;
		}
	}
}
