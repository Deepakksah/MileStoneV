using System;
using System.Collections.Generic;
using System.Diagnostics;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;
using VideoOS.Installer.RecorderWarningPlugin.Properties;

namespace VideoOS.Installer.RecorderWarningPlugin;

public class EvidenceLocksUnsupportedWarningPageSubscriber : IWizardCommonPageSubscription<WarningPageContent>
{
	private IGlobalData globalData;

	private IComponentFactory componentFactory;

	private ILicenseUtility licenseUtility;

	private IPackageInfoProvider packageInfoProvider;

	private const int warningRank = 15;

	private const string ManServerPackageID = "58451C4D-72B8-4D83-885D-A74B82F6C997";

	public Guid Id => new Guid("C2924ED3-371A-4F35-A111-7660C953D0A6");

	public EvidenceLocksUnsupportedWarningPageSubscriber()
	{
		globalData = new GlobalDataWrapper();
		componentFactory = new ComponentFactory();
		licenseUtility = new LicenseUtility(globalData);
		packageInfoProvider = new PackageInfoProvider("58451C4D-72B8-4D83-885D-A74B82F6C997");
	}

	public EvidenceLocksUnsupportedWarningPageSubscriber(IGlobalData globalData, IComponentFactory componentFactory, ILicenseUtility licenseUtility, IPackageInfoProvider packageInfoProvider)
	{
		this.globalData = globalData;
		this.componentFactory = componentFactory;
		this.licenseUtility = licenseUtility;
		this.packageInfoProvider = packageInfoProvider;
	}

	public bool BeforeActivate(Dictionary<string, string> parameters)
	{
		if (IsUpgrade() && LicenseHasEvidenceLockFeatureFlag())
		{
			return ConfigurationContainsMoreThanOneLegacyRecordingServer();
		}
		return false;
	}

	private bool IsUpgrade()
	{
		return globalData.GetValueAsBoolean("IS_UPGRADE");
	}

	private bool LicenseHasEvidenceLockFeatureFlag()
	{
		return licenseUtility.HasEvidenceLockFeature(valueIfError: false);
	}

	private bool ConfigurationContainsMoreThanOneLegacyRecordingServer()
	{
		if (packageInfoProvider.IsInstalledInHWMM())
		{
			return false;
		}
		if (!globalData.TryGetValue("SQL_CONNECTION_STRING_MS", out var value))
		{
			LogManager.Write("Failed to look up connection string when checking if configuration contains more that one legacy recording server.", TraceEventType.Error);
			return false;
		}
		try
		{
			using IConfigurationDataComponent configurationDataComponent = componentFactory.CreateConfigurationComponent(value, null);
			return configurationDataComponent.QueryRecordingServerCountPre2020R1() > 1;
		}
		catch (ConfigurationDataException exception)
		{
			LogManager.Write("Failed to check for configuration containing more that one legacy recording server.");
			LogManager.Write(exception);
			return false;
		}
	}

	public Dictionary<string, string> CollectParameters(WarningPageContent content)
	{
		return null;
	}

	public WarningPageContent GetContent()
	{
		ContentLinkText details = new ContentLinkText(Resources.WarningPageBodyDetails);
		ContentLinkText subsection = new ContentLinkText(Resources.WarningPageBodySubsection);
		return new WarningPageContent(Resources.WarningPageHeadingDetails, details, Resources.WarningPageHeadingSubsection, subsection, 15);
	}
}
