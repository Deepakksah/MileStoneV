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
[XmlRoot(Namespace = "http://schemas.microsoft.com/developer/2004/01/bootstrapper", IsNullable = false)]
public class Package
{
	private StringsTypeString[] stringsField;

	private PackageFilesType packageFilesField;

	private object[] installChecksField;

	private CommandsType commandsField;

	private SchedulesTypeSchedule[] schedulesField;

	private string nameField;

	private string cultureField;

	private string licenseAgreementField;

	[XmlArrayItem("String")]
	public StringsTypeString[] Strings
	{
		get
		{
			return stringsField;
		}
		set
		{
			stringsField = value;
		}
	}

	public PackageFilesType PackageFiles
	{
		get
		{
			return packageFilesField;
		}
		set
		{
			packageFilesField = value;
		}
	}

	[XmlArrayItem("AssemblyCheck", typeof(InstallChecksTypeAssemblyCheck), IsNullable = false)]
	[XmlArrayItem("ExternalCheck", typeof(InstallChecksTypeExternalCheck), IsNullable = false)]
	[XmlArrayItem("FileCheck", typeof(InstallChecksTypeFileCheck), IsNullable = false)]
	[XmlArrayItem("MsiProductCheck", typeof(InstallChecksTypeMsiProductCheck), IsNullable = false)]
	[XmlArrayItem("RegistryCheck", typeof(InstallChecksTypeRegistryCheck), IsNullable = false)]
	[XmlArrayItem("RegistryFileCheck", typeof(InstallChecksTypeRegistryFileCheck), IsNullable = false)]
	public object[] InstallChecks
	{
		get
		{
			return installChecksField;
		}
		set
		{
			installChecksField = value;
		}
	}

	public CommandsType Commands
	{
		get
		{
			return commandsField;
		}
		set
		{
			commandsField = value;
		}
	}

	[XmlArrayItem("Schedule")]
	public SchedulesTypeSchedule[] Schedules
	{
		get
		{
			return schedulesField;
		}
		set
		{
			schedulesField = value;
		}
	}

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
	public string Culture
	{
		get
		{
			return cultureField;
		}
		set
		{
			cultureField = value;
		}
	}

	[XmlAttribute]
	public string LicenseAgreement
	{
		get
		{
			return licenseAgreementField;
		}
		set
		{
			licenseAgreementField = value;
		}
	}
}
