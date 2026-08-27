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
public class SubscriptionPluginType
{
	private SubscriptionPluginTypePluginAssembly pluginAssemblyField;

	private SubscriptionPluginTypeByPassSubscriptionIf[] bypassConditionsField;

	private string nameField;

	private string filenameField;

	public SubscriptionPluginTypePluginAssembly PluginAssembly
	{
		get
		{
			return pluginAssemblyField;
		}
		set
		{
			pluginAssemblyField = value;
		}
	}

	[XmlArrayItem("ByPassSubscriptionIf", IsNullable = false)]
	public SubscriptionPluginTypeByPassSubscriptionIf[] BypassConditions
	{
		get
		{
			return bypassConditionsField;
		}
		set
		{
			bypassConditionsField = value;
		}
	}

	[XmlAttribute]
	public string Name
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
}
