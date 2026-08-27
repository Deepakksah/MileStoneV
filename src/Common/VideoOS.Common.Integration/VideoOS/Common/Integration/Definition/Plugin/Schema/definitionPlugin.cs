using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Common.Integration.Definition.Plugin.Schema;

[Serializable]
[GeneratedCode("xsd", "4.6.1055.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://VideoOS.net/Surveillance/XmlSchema")]
public class definitionPlugin : InformationType
{
	private definitionPluginCapabilities capabilitiesField;

	private definitionPluginEvents eventsField;

	public definitionPluginCapabilities capabilities
	{
		get
		{
			return capabilitiesField;
		}
		set
		{
			capabilitiesField = value;
		}
	}

	public definitionPluginEvents events
	{
		get
		{
			return eventsField;
		}
		set
		{
			eventsField = value;
		}
	}
}
