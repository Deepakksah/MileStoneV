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
public class InstallChecksTypeRegistryFileCheck
{
	private string propertyField;

	private string keyField;

	private string valueField;

	private string fileNameField;

	private string searchDepthField;

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
	public string Key
	{
		get
		{
			return keyField;
		}
		set
		{
			keyField = value;
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
	public string FileName
	{
		get
		{
			return fileNameField;
		}
		set
		{
			fileNameField = value;
		}
	}

	[XmlAttribute(DataType = "nonNegativeInteger")]
	public string SearchDepth
	{
		get
		{
			return searchDepthField;
		}
		set
		{
			searchDepthField = value;
		}
	}
}
