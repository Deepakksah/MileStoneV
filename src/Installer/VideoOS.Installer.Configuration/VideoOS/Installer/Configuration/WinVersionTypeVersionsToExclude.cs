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
public class WinVersionTypeVersionsToExclude
{
	private WinVersionTypeVersionsToExcludeProductType productTypeField;

	private string valueField;

	[XmlAttribute]
	[DefaultValue(WinVersionTypeVersionsToExcludeProductType.All)]
	public WinVersionTypeVersionsToExcludeProductType ProductType
	{
		get
		{
			return productTypeField;
		}
		set
		{
			productTypeField = value;
		}
	}

	[XmlText]
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

	public WinVersionTypeVersionsToExclude()
	{
		productTypeField = WinVersionTypeVersionsToExcludeProductType.All;
	}
}
