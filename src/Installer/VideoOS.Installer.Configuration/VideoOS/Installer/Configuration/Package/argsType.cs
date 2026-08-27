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
public class argsType
{
	private string groupField;

	private string productField;

	private string downloadpageField;

	private string languageField;

	private string versionField;

	private string referencedfileField;

	private string sourcefilesField;

	private string showwithalllanguagesField;

	private string platformField;

	private string displaynameField;

	public string group
	{
		get
		{
			return groupField;
		}
		set
		{
			groupField = value;
		}
	}

	public string product
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

	public string downloadpage
	{
		get
		{
			return downloadpageField;
		}
		set
		{
			downloadpageField = value;
		}
	}

	public string language
	{
		get
		{
			return languageField;
		}
		set
		{
			languageField = value;
		}
	}

	public string version
	{
		get
		{
			return versionField;
		}
		set
		{
			versionField = value;
		}
	}

	public string referencedfile
	{
		get
		{
			return referencedfileField;
		}
		set
		{
			referencedfileField = value;
		}
	}

	public string sourcefiles
	{
		get
		{
			return sourcefilesField;
		}
		set
		{
			sourcefilesField = value;
		}
	}

	public string showwithalllanguages
	{
		get
		{
			return showwithalllanguagesField;
		}
		set
		{
			showwithalllanguagesField = value;
		}
	}

	public string platform
	{
		get
		{
			return platformField;
		}
		set
		{
			platformField = value;
		}
	}

	public string displayname
	{
		get
		{
			return displaynameField;
		}
		set
		{
			displaynameField = value;
		}
	}
}
