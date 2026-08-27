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
public class InstallerSettingsType
{
	private bool enableExtendedMSILoggingField;

	private bool disableChangingTargetDirField;

	private bool disableInstallSqlServerField;

	private bool disableWarningField;

	private bool useSharedInstallFolderField;

	private TargetServerType targetManagementServerField;

	private InstallerSettingsTypeServiceAccountSetup serviceAccountSetupField;

	private InstallerSettingsTypeWelcomePageSetup welcomePageSetupField;

	private InstallerSettingsTypeCustomerExperienceProgram customerExperienceProgramField;

	private string[] eventsSettingsField;

	private string[] flowSettingsField;

	private CustomPageSetupType[] customPagesSetupField;

	private InstallerSettingsTypeFlowSettings20 flowSettings20Field;

	private PageConditionType[] pageConditionsField;

	[DefaultValue(false)]
	public bool EnableExtendedMSILogging
	{
		get
		{
			return enableExtendedMSILoggingField;
		}
		set
		{
			enableExtendedMSILoggingField = value;
		}
	}

	[DefaultValue(false)]
	public bool DisableChangingTargetDir
	{
		get
		{
			return disableChangingTargetDirField;
		}
		set
		{
			disableChangingTargetDirField = value;
		}
	}

	[DefaultValue(false)]
	public bool DisableInstallSqlServer
	{
		get
		{
			return disableInstallSqlServerField;
		}
		set
		{
			disableInstallSqlServerField = value;
		}
	}

	[DefaultValue(true)]
	public bool DisableWarning
	{
		get
		{
			return disableWarningField;
		}
		set
		{
			disableWarningField = value;
		}
	}

	[DefaultValue(false)]
	public bool UseSharedInstallFolder
	{
		get
		{
			return useSharedInstallFolderField;
		}
		set
		{
			useSharedInstallFolderField = value;
		}
	}

	[DefaultValue(TargetServerType.Both)]
	public TargetServerType TargetManagementServer
	{
		get
		{
			return targetManagementServerField;
		}
		set
		{
			targetManagementServerField = value;
		}
	}

	public InstallerSettingsTypeServiceAccountSetup ServiceAccountSetup
	{
		get
		{
			return serviceAccountSetupField;
		}
		set
		{
			serviceAccountSetupField = value;
		}
	}

	public InstallerSettingsTypeWelcomePageSetup WelcomePageSetup
	{
		get
		{
			return welcomePageSetupField;
		}
		set
		{
			welcomePageSetupField = value;
		}
	}

	public InstallerSettingsTypeCustomerExperienceProgram CustomerExperienceProgram
	{
		get
		{
			return customerExperienceProgramField;
		}
		set
		{
			customerExperienceProgramField = value;
		}
	}

	[XmlArrayItem("SuppressEvent", IsNullable = false)]
	public string[] EventsSettings
	{
		get
		{
			return eventsSettingsField;
		}
		set
		{
			eventsSettingsField = value;
		}
	}

	[XmlArrayItem("SuppressPage", IsNullable = false)]
	public string[] FlowSettings
	{
		get
		{
			return flowSettingsField;
		}
		set
		{
			flowSettingsField = value;
		}
	}

	[XmlArrayItem("CustomPageSetup", IsNullable = false)]
	public CustomPageSetupType[] CustomPagesSetup
	{
		get
		{
			return customPagesSetupField;
		}
		set
		{
			customPagesSetupField = value;
		}
	}

	public InstallerSettingsTypeFlowSettings20 FlowSettings20
	{
		get
		{
			return flowSettings20Field;
		}
		set
		{
			flowSettings20Field = value;
		}
	}

	[XmlArrayItem("PageCondition", IsNullable = false)]
	public PageConditionType[] PageConditions
	{
		get
		{
			return pageConditionsField;
		}
		set
		{
			pageConditionsField = value;
		}
	}

	public InstallerSettingsType()
	{
		enableExtendedMSILoggingField = false;
		disableChangingTargetDirField = false;
		disableInstallSqlServerField = false;
		disableWarningField = true;
		useSharedInstallFolderField = false;
		targetManagementServerField = TargetServerType.Both;
	}
}
