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
[DebuggerDisplay("{Name} - {ID}")]
[DebuggerDisplay("{Name} - {ID}")]
public class InstallerPackageType
{
	private string descriptionField;

	private InstallerPackageTypePreInstallActions preInstallActionsField;

	private PostCommandsCollectionType postCommandsField;

	private InstallerPackageTypePostUninstallActions postUninstallActionsField;

	private InstallerPackageTypePostInstallActions postInstallActionsField;

	private InstallConditionTypeByPassIf[] installConditionsField;

	private UninstallConditionType uninstallConditionsField;

	private InstallerPackageTypeProductManifest productManifestField;

	private InstallerPackageTypeProductManifest64 productManifest64Field;

	private RegistryCountType[] referenceCountsField;

	private InstallerPluginType installerPluginField;

	private InstallerPackageTypeInstallationTaskPlugin installationTaskPluginField;

	private string nameField;

	private string idField;

	private PackageTypeType packageTypeField;

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

	public InstallerPackageTypePreInstallActions PreInstallActions
	{
		get
		{
			return preInstallActionsField;
		}
		set
		{
			preInstallActionsField = value;
		}
	}

	public PostCommandsCollectionType PostCommands
	{
		get
		{
			return postCommandsField;
		}
		set
		{
			postCommandsField = value;
		}
	}

	public InstallerPackageTypePostUninstallActions PostUninstallActions
	{
		get
		{
			return postUninstallActionsField;
		}
		set
		{
			postUninstallActionsField = value;
		}
	}

	public InstallerPackageTypePostInstallActions PostInstallActions
	{
		get
		{
			return postInstallActionsField;
		}
		set
		{
			postInstallActionsField = value;
		}
	}

	[XmlArrayItem("ByPassIf", IsNullable = false)]
	public InstallConditionTypeByPassIf[] InstallConditions
	{
		get
		{
			return installConditionsField;
		}
		set
		{
			installConditionsField = value;
		}
	}

	public UninstallConditionType UninstallConditions
	{
		get
		{
			return uninstallConditionsField;
		}
		set
		{
			uninstallConditionsField = value;
		}
	}

	public InstallerPackageTypeProductManifest ProductManifest
	{
		get
		{
			return productManifestField;
		}
		set
		{
			productManifestField = value;
		}
	}

	public InstallerPackageTypeProductManifest64 ProductManifest64
	{
		get
		{
			return productManifest64Field;
		}
		set
		{
			productManifest64Field = value;
		}
	}

	[XmlArrayItem("RegistryCount", IsNullable = false)]
	public RegistryCountType[] ReferenceCounts
	{
		get
		{
			return referenceCountsField;
		}
		set
		{
			referenceCountsField = value;
		}
	}

	public InstallerPluginType InstallerPlugin
	{
		get
		{
			return installerPluginField;
		}
		set
		{
			installerPluginField = value;
		}
	}

	public InstallerPackageTypeInstallationTaskPlugin InstallationTaskPlugin
	{
		get
		{
			return installationTaskPluginField;
		}
		set
		{
			installationTaskPluginField = value;
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

	[XmlAttribute]
	[DefaultValue(PackageTypeType.InstallerPackage)]
	public PackageTypeType PackageType
	{
		get
		{
			return packageTypeField;
		}
		set
		{
			packageTypeField = value;
		}
	}

	public InstallerPackageType()
	{
		packageTypeField = PackageTypeType.InstallerPackage;
	}
}
