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
public class RegistryCountType
{
	private string keyNameField;

	private string valueField;

	private bool onlyRegistryActionsField;

	[XmlAttribute]
	public string KeyName
	{
		get
		{
			return keyNameField;
		}
		set
		{
			keyNameField = value;
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

	[XmlAttribute]
	[DefaultValue(false)]
	public bool OnlyRegistryActions
	{
		get
		{
			return onlyRegistryActionsField;
		}
		set
		{
			onlyRegistryActionsField = value;
		}
	}

	public RegistryCountType()
	{
		onlyRegistryActionsField = false;
	}
}
