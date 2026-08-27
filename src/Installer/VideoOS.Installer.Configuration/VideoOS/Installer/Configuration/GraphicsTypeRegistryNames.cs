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
public class GraphicsTypeRegistryNames
{
	private string publisherKeyField;

	[XmlAttribute]
	public string PublisherKey
	{
		get
		{
			return publisherKeyField;
		}
		set
		{
			publisherKeyField = value;
		}
	}
}
