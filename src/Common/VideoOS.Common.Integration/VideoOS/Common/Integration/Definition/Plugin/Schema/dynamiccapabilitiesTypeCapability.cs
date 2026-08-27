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
public class dynamiccapabilitiesTypeCapability : capabilityType
{
	private dynamiccapabilitiesTypeCapabilityControl controlField;

	public dynamiccapabilitiesTypeCapabilityControl control
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
}
