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
[XmlType(Namespace = "http://schemas.microsoft.com/developer/2004/01/bootstrapper")]
public class FileType
{
	private string nameField;

	private string homeSiteField;

	private string publicKeyField;

	private string hashField;

	private string copyOnBuildField;

	[XmlAttribute]
	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	[XmlAttribute]
	public string HomeSite
	{
		get
		{
			return homeSiteField;
		}
		set
		{
			homeSiteField = value;
		}
	}

	[XmlAttribute]
	public string PublicKey
	{
		get
		{
			return publicKeyField;
		}
		set
		{
			publicKeyField = value;
		}
	}

	[XmlAttribute]
	public string Hash
	{
		get
		{
			return hashField;
		}
		set
		{
			hashField = value;
		}
	}

	[XmlAttribute]
	public string CopyOnBuild
	{
		get
		{
			return copyOnBuildField;
		}
		set
		{
			copyOnBuildField = value;
		}
	}
}
