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
public class ARPEntryType
{
	private ArgumentType[] argumentsField;

	private string displayVersionField;

	private string upgradeCodeField;

	private string contactField;

	private ArpEntryDeleteByType deleteByField;

	[XmlArrayItem("Argument", IsNullable = false)]
	public ArgumentType[] Arguments
	{
		get
		{
			return argumentsField;
		}
		set
		{
			argumentsField = value;
		}
	}

	public string DisplayVersion
	{
		get
		{
			return displayVersionField;
		}
		set
		{
			displayVersionField = value;
		}
	}

	public string UpgradeCode
	{
		get
		{
			return upgradeCodeField;
		}
		set
		{
			upgradeCodeField = value;
		}
	}

	public string Contact
	{
		get
		{
			return contactField;
		}
		set
		{
			contactField = value;
		}
	}

	[XmlAttribute]
	[DefaultValue(ArpEntryDeleteByType.Everyone)]
	public ArpEntryDeleteByType DeleteBy
	{
		get
		{
			return deleteByField;
		}
		set
		{
			deleteByField = value;
		}
	}

	public ARPEntryType()
	{
		deleteByField = ArpEntryDeleteByType.Everyone;
	}
}
