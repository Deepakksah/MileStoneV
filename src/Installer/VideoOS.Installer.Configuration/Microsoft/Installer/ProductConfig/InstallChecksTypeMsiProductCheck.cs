using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Microsoft.Installer.ProductConfig;

[Serializable]
[GeneratedCode("xsd", "4.7.2053.0")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/developer/2004/01/bootstrapper")]
public class InstallChecksTypeMsiProductCheck
{
	private string propertyField;

	private string productField;

	private string featureField;

	[XmlAttribute]
	public string Property
	{
		get
		{
			return propertyField;
		}
		set
		{
			propertyField = value;
		}
	}

	[XmlAttribute]
	public string Product
	{
		get
		{
			return productField;
		}
		set
		{
			productField = value;
		}
	}

	[XmlAttribute]
	public string Feature
	{
		get
		{
			return featureField;
		}
		set
		{
			featureField = value;
		}
	}
}
