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
public class InstallChecksTypeExternalCheck
{
	private string propertyField;

	private string argumentsField;

	private string packageFileField;

	private string logField;

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
	public string Arguments
	{
		get
		{
			return argumentsField;
		}
		set
		{
			argumentsField = value;
		}
	}

	[XmlAttribute]
	public string PackageFile
	{
		get
		{
			return packageFileField;
		}
		set
		{
			packageFileField = value;
		}
	}

	[XmlAttribute]
	public string Log
	{
		get
		{
			return logField;
		}
		set
		{
			logField = value;
		}
	}
}
