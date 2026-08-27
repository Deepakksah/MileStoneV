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
public class InformationTypeUsedbyproductids
{
	private string[] productidField;

	[XmlElement("productid")]
	public string[] productid
	{
		get
		{
			return productidField;
		}
		set
		{
			productidField = value;
		}
	}
}
