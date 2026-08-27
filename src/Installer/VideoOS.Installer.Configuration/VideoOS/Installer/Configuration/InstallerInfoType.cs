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
[XmlType(Namespace = "urn:milestone-systems")]
public class InstallerInfoType
{
	private string publisherField;

	private string helpLinkField;

	private string customerExpirienceImprovementProgramField;

	private bool customerExpirienceImprovementProgramDefaultStateField;

	private bool customerExpirienceImprovementProgramDefaultStateFieldSpecified;

	private string idField;

	private string displayNameField;

	private string descriptionField;

	private string primaryLanguageField;

	private InstallerVersionInfoType versionField;

	private string targetPlatformField;

	private bool groupInstallerField;

	public string Publisher
	{
		get
		{
			return publisherField;
		}
		set
		{
			publisherField = value;
		}
	}

	public string HelpLink
	{
		get
		{
			return helpLinkField;
		}
		set
		{
			helpLinkField = value;
		}
	}

	public string CustomerExpirienceImprovementProgram
	{
		get
		{
			return customerExpirienceImprovementProgramField;
		}
		set
		{
			customerExpirienceImprovementProgramField = value;
		}
	}

	public bool CustomerExpirienceImprovementProgramDefaultState
	{
		get
		{
			return customerExpirienceImprovementProgramDefaultStateField;
		}
		set
		{
			customerExpirienceImprovementProgramDefaultStateField = value;
		}
	}

	[XmlIgnore]
	public bool CustomerExpirienceImprovementProgramDefaultStateSpecified
	{
		get
		{
			return customerExpirienceImprovementProgramDefaultStateFieldSpecified;
		}
		set
		{
			customerExpirienceImprovementProgramDefaultStateFieldSpecified = value;
		}
	}

	public string ID
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

	public string DisplayName
	{
		get
		{
			return displayNameField;
		}
		set
		{
			displayNameField = value;
		}
	}

	public string Description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}

	public string PrimaryLanguage
	{
		get
		{
			return primaryLanguageField;
		}
		set
		{
			primaryLanguageField = value;
		}
	}

	public InstallerVersionInfoType Version
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

	[DefaultValue("Any")]
	public string TargetPlatform
	{
		get
		{
			return targetPlatformField;
		}
		set
		{
			targetPlatformField = value;
		}
	}

	[DefaultValue(false)]
	public bool GroupInstaller
	{
		get
		{
			return groupInstallerField;
		}
		set
		{
			groupInstallerField = value;
		}
	}

	public InstallerInfoType()
	{
		targetPlatformField = "Any";
		groupInstallerField = false;
	}
}
