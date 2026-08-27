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
public class SubscriptionPluginTypeByPassSubscriptionIf
{
	private string subscriptionIDField;

	private string subscriptionNameField;

	private string packageIDField;

	private string packageNameField;

	private ByPassConditionType conditionField;

	[XmlAttribute]
	public string SubscriptionID
	{
		get
		{
			return subscriptionIDField;
		}
		set
		{
			subscriptionIDField = value;
		}
	}

	[XmlAttribute]
	public string SubscriptionName
	{
		get
		{
			return subscriptionNameField;
		}
		set
		{
			subscriptionNameField = value;
		}
	}

	[XmlAttribute]
	public string PackageID
	{
		get
		{
			return packageIDField;
		}
		set
		{
			packageIDField = value;
		}
	}

	[XmlAttribute]
	public string PackageName
	{
		get
		{
			return packageNameField;
		}
		set
		{
			packageNameField = value;
		}
	}

	[XmlAttribute]
	public ByPassConditionType Condition
	{
		get
		{
			return conditionField;
		}
		set
		{
			conditionField = value;
		}
	}
}
