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
[XmlRoot(Namespace = "urn:milestone-systems", IsNullable = false)]
public class InstallerConfig
{
	private InstallerInfoType installerInfoField;

	private string installerCaptionField;

	private PackageRefType[] installExecuteSequenceField;

	private InstallerPrerequisitesType installerPrerequisitesField;

	private InstallerPluginType[] installerPluginsField;

	private SubscriptionPluginType[] installerSubscriptionsField;

	private LicenseProviderType licenseProviderField;

	private EulaNoteProviderType eulaNoteProviderField;

	private InstallerSettingsType installerSettingsField;

	private InstallerPackageType[] installerPackagesField;

	private string[] installerLanguagesField;

	private GraphicsType graphicsField;

	private UninstallerInfoType uninstallerInfoField;

	public InstallerInfoType InstallerInfo
	{
		get
		{
			return installerInfoField;
		}
		set
		{
			installerInfoField = value;
		}
	}

	public string InstallerCaption
	{
		get
		{
			return installerCaptionField;
		}
		set
		{
			installerCaptionField = value;
		}
	}

	[XmlArrayItem("PackageRef", IsNullable = false)]
	public PackageRefType[] InstallExecuteSequence
	{
		get
		{
			return installExecuteSequenceField;
		}
		set
		{
			installExecuteSequenceField = value;
		}
	}

	public InstallerPrerequisitesType InstallerPrerequisites
	{
		get
		{
			return installerPrerequisitesField;
		}
		set
		{
			installerPrerequisitesField = value;
		}
	}

	[XmlArrayItem("InstallerPlugin", IsNullable = false)]
	public InstallerPluginType[] InstallerPlugins
	{
		get
		{
			return installerPluginsField;
		}
		set
		{
			installerPluginsField = value;
		}
	}

	[XmlArrayItem("SubscriptionPlugin", IsNullable = false)]
	public SubscriptionPluginType[] InstallerSubscriptions
	{
		get
		{
			return installerSubscriptionsField;
		}
		set
		{
			installerSubscriptionsField = value;
		}
	}

	public LicenseProviderType LicenseProvider
	{
		get
		{
			return licenseProviderField;
		}
		set
		{
			licenseProviderField = value;
		}
	}

	public EulaNoteProviderType EulaNoteProvider
	{
		get
		{
			return eulaNoteProviderField;
		}
		set
		{
			eulaNoteProviderField = value;
		}
	}

	public InstallerSettingsType InstallerSettings
	{
		get
		{
			return installerSettingsField;
		}
		set
		{
			installerSettingsField = value;
		}
	}

	[XmlArrayItem("InstallerPackage", IsNullable = false)]
	public InstallerPackageType[] InstallerPackages
	{
		get
		{
			return installerPackagesField;
		}
		set
		{
			installerPackagesField = value;
		}
	}

	[XmlArrayItem("Language", DataType = "language", IsNullable = false)]
	public string[] InstallerLanguages
	{
		get
		{
			return installerLanguagesField;
		}
		set
		{
			installerLanguagesField = value;
		}
	}

	public GraphicsType Graphics
	{
		get
		{
			return graphicsField;
		}
		set
		{
			graphicsField = value;
		}
	}

	public UninstallerInfoType UninstallerInfo
	{
		get
		{
			return uninstallerInfoField;
		}
		set
		{
			uninstallerInfoField = value;
		}
	}
}
