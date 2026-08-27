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
[XmlRoot("Package", Namespace = "urn:milestone-systems", IsNullable = false)]
public class PackageType
{
	private ProductInfoType productInfoField;

	private string publisherField;

	private ProductVersionInfoType versionField;

	private string[] languagesField;

	private string displayNameField;

	private DescriptionType descriptionField;

	private string identificationNameField;

	private string[] primaryFileField;

	private string[][] secondaryFilesField;

	private PackageCollectionType[] prerequisitesField;

	private PackageSeverityType severityField;

	private bool severityFieldSpecified;

	private InstallerFrameworkType installerFrameworkField;

	private bool installerFrameworkFieldSpecified;

	private InstallUninstall installField;

	private InstallUninstall uninstallField;

	private SSRegistrationArgsType sSRegistrationArgsField;

	public ProductInfoType ProductInfo
	{
		get
		{
			return productInfoField;
		}
		set
		{
			productInfoField = value;
		}
	}

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

	public ProductVersionInfoType Version
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

	[XmlArrayItem("Language", DataType = "language", IsNullable = false)]
	public string[] Languages
	{
		get
		{
			return languagesField;
		}
		set
		{
			languagesField = value;
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

	public DescriptionType Description
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

	public string IdentificationName
	{
		get
		{
			return identificationNameField;
		}
		set
		{
			identificationNameField = value;
		}
	}

	[XmlArrayItem("Name", IsNullable = false)]
	public string[] PrimaryFile
	{
		get
		{
			return primaryFileField;
		}
		set
		{
			primaryFileField = value;
		}
	}

	[XmlArrayItem("File", IsNullable = false)]
	[XmlArrayItem("Name", IsNullable = false, NestingLevel = 1)]
	public string[][] SecondaryFiles
	{
		get
		{
			return secondaryFilesField;
		}
		set
		{
			secondaryFilesField = value;
		}
	}

	[XmlArrayItem("Prerequisite", IsNullable = false)]
	public PackageCollectionType[] Prerequisites
	{
		get
		{
			return prerequisitesField;
		}
		set
		{
			prerequisitesField = value;
		}
	}

	public PackageSeverityType Severity
	{
		get
		{
			return severityField;
		}
		set
		{
			severityField = value;
		}
	}

	[XmlIgnore]
	public bool SeveritySpecified
	{
		get
		{
			return severityFieldSpecified;
		}
		set
		{
			severityFieldSpecified = value;
		}
	}

	public InstallerFrameworkType InstallerFramework
	{
		get
		{
			return installerFrameworkField;
		}
		set
		{
			installerFrameworkField = value;
		}
	}

	[XmlIgnore]
	public bool InstallerFrameworkSpecified
	{
		get
		{
			return installerFrameworkFieldSpecified;
		}
		set
		{
			installerFrameworkFieldSpecified = value;
		}
	}

	public InstallUninstall Install
	{
		get
		{
			return installField;
		}
		set
		{
			installField = value;
		}
	}

	public InstallUninstall Uninstall
	{
		get
		{
			return uninstallField;
		}
		set
		{
			uninstallField = value;
		}
	}

	public SSRegistrationArgsType SSRegistrationArgs
	{
		get
		{
			return sSRegistrationArgsField;
		}
		set
		{
			sSRegistrationArgsField = value;
		}
	}
}
