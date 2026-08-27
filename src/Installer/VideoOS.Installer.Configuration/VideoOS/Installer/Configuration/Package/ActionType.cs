using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Installer.Configuration.Package;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "urn:milestone-systems")]
public class ActionType
{
	private object itemField;

	[XmlElement("RestartService", typeof(ActionRestartServiceType))]
	[XmlElement("StartService", typeof(ActionStartServiceType))]
	[XmlElement("StopService", typeof(ActionStopServiceType))]
	public object Item
	{
		get
		{
			return itemField;
		}
		set
		{
			itemField = value;
		}
	}
}
