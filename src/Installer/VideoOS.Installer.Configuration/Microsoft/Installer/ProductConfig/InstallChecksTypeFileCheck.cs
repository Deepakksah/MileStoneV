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
public class InstallChecksTypeFileCheck
{
	private string fileNameField;

	private string searchPathField;

	private string specialFolderField;

	private string searchDepthField;

	private string propertyField;

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

	[XmlAttribute]
	public string SearchPath
	{
		get
		{
			return searchPathField;
		}
		set
		{
			searchPathField = value;
		}
	}

	[XmlAttribute]
	public string SpecialFolder
	{
		get
		{
			return specialFolderField;
		}
		set
		{
			specialFolderField = value;
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
}
