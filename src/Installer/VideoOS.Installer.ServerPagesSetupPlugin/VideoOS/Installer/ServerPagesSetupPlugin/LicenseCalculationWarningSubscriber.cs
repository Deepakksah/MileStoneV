using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Common.Interfaces.ServiceManager;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Logging;
using VideoOS.Installer.ServerPagesSetupPlugin.Properties;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class LicenseCalculationWarningSubscriber : IWizardCommonPageSubscription<WarningPageContent>
{
	private const string GlobalDataLicensePath = "LicensePath";

	private readonly ISystemConfiguration _systemConfiguration;

	private readonly ILicenseCalculator _licenseCalculator;

	private readonly IGlobalData _globalData;

	private ILicenseReportGenerator _reportGenerator;

	private ILicenseDocument _licenseDocument;

	private List<ILicenseCalculationResult> _licenseCalculations;

	private IVersionChecker _versionChecker;

	private readonly Version ChannelLicensing = new Version(21, 1);

	private const int VideoLossWarningRank = 7;

	private const int ChargingMoreWarningRank = 14;

	private bool _showVideoLossWarning;

	private bool _showChargingMoreWarning;

	public Guid Id => new Guid("D50AC9FC-C29B-479B-AA5A-B4DF071D8F55");

	protected bool ShowVideoLossWarning
	{
		get
		{
			return _showVideoLossWarning;
		}
		set
		{
			_showVideoLossWarning = value;
		}
	}

	protected bool ShowChargingMoreWarning
	{
		get
		{
			return _showChargingMoreWarning;
		}
		set
		{
			_showChargingMoreWarning = value;
		}
	}

	public LicenseCalculationWarningSubscriber()
	{
		_systemConfiguration = new SystemConfigurationApiServiceManager();
		_licenseCalculator = LicenseFactory.CreateLicenseCalculator();
		_globalData = new GlobalDataWrapper();
		_versionChecker = new VersionChecker();
	}

	public LicenseCalculationWarningSubscriber(ISystemConfiguration systemConfiguration, ILicenseCalculator licenseCalculator, IGlobalData globalData, ILicenseDocument licenseDocument, ILicenseReportGenerator reportGenerator, IVersionChecker versionChecker)
	{
		if (systemConfiguration == null)
		{
			throw new ArgumentNullException("systemConfiguration");
		}
		if (licenseCalculator == null)
		{
			throw new ArgumentNullException("licenseCalculator");
		}
		if (globalData == null)
		{
			throw new ArgumentNullException("globalData");
		}
		if (reportGenerator == null)
		{
			throw new ArgumentNullException("reportGenerator");
		}
		_systemConfiguration = systemConfiguration;
		_licenseCalculator = licenseCalculator;
		_globalData = globalData;
		_licenseDocument = licenseDocument;
		_reportGenerator = reportGenerator;
		_versionChecker = versionChecker;
	}

	[SuppressMessage("Microsoft.Naming", "CA2204:Literals should be spelled correctly", MessageId = "LicensePath", Justification = "That is the name of the item to retrieve")]
	public bool BeforeActivate(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		if (!_globalData.GetValueAsBoolean("IS_UPGRADE"))
		{
			return false;
		}
		Version version = _versionChecker.FromVersion();
		if (version != null && version >= ChannelLicensing)
		{
			return false;
		}
		if (_licenseDocument == null)
		{
			string value = string.Empty;
			if (!_globalData.TryGetValue("LicensePath", out value))
			{
				throw new InvalidOperationException("LicensePath could not be found");
			}
			if (!File.Exists(value))
			{
				throw new FileNotFoundException("License file could not be found!");
			}
			ILicenseLoader licenseLoader = LicenseFactory.CreateLicenseLoader();
			_licenseDocument = licenseLoader.ImportLicenseFile(value);
		}
		if (parameters.TryGetValue("WebSitePort", out var value2) && int.TryParse(value2, out var result))
		{
			_systemConfiguration.WebsitePort = result;
		}
		bool flag = CalculateLicenseDemands();
		bool flag2 = AnyHardwareWithMultipleChannelLicenses();
		ShowVideoLossWarning = flag2 && flag;
		if (!ShowVideoLossWarning)
		{
			ShowChargingMoreWarning = flag2;
		}
		if (ShowVideoLossWarning || ShowChargingMoreWarning)
		{
			_reportGenerator = LicenseFactory.CreateLicenseReportGenerator((!ShowVideoLossWarning) ? LicenseReportType.Information : LicenseReportType.Warning);
		}
		if (!ShowVideoLossWarning)
		{
			return ShowChargingMoreWarning;
		}
		return true;
	}

	public Dictionary<string, string> CollectParameters(WarningPageContent content)
	{
		return new Dictionary<string, string>();
	}

	public WarningPageContent GetContent()
	{
		if (ShowVideoLossWarning)
		{
			return GetVideoLossWarningContent();
		}
		if (ShowChargingMoreWarning)
		{
			return GetChargingMoreWarningContent();
		}
		LogManager.Write("GetContent() called, but neither VideoLossWarning or ChargingMoreWarning should be shown!");
		return null;
	}

	[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Installer can not fail")]
	private bool CalculateLicenseDemands()
	{
		try
		{
			IEnumerable<IHardwareInfo> enumerable = _systemConfiguration.FetchHardware();
			if (enumerable == null)
			{
				throw new InvalidOperationException("The System Configuration failed to return valid hardware list");
			}
			_licenseCalculator.Initialize(_licenseDocument);
			return !_licenseCalculator.CalculateLicenseRequirements(enumerable, out _licenseCalculations);
		}
		catch (Exception exception)
		{
			LogManager.Write("Failed to calculate license requirements.");
			LogManager.Write(exception);
			return false;
		}
	}

	private bool AnyHardwareWithMultipleChannelLicenses()
	{
		if (_licenseCalculations != null && _licenseCalculations.Any((ILicenseCalculationResult c) => c.ChannelLicenses > 1))
		{
			return true;
		}
		return false;
	}

	private WarningPageContent GetVideoLossWarningContent()
	{
		string supportedDeviceListUrl = OemResources.Instance.App.SupportedDeviceListUrl;
		List<string> list = new List<string>();
		List<Uri> list2 = new List<Uri>();
		ContentLinkText contentLinkText = null;
		Uri uri = GenerateAndSaveReport(supportedDeviceListUrl);
		if (uri != null)
		{
			list2.Add(uri);
			list.Add(Resources.LicenseWarning_BodyLinkText);
			contentLinkText = new ContentLinkText(Resources.VideoLoss_Body, list.ToArray(), list2.ToArray());
		}
		else
		{
			LogManager.Write("Failed to generate license report.", TraceEventType.Error);
			contentLinkText = new ContentLinkText(string.Format(CultureInfo.InvariantCulture, Resources.VideoLoss_Body, string.Empty));
		}
		ContentLinkText subSection = GetSubSection(supportedDeviceListUrl);
		if (subSection != null)
		{
			return new WarningPageContent(Resources.VideoLoss_Heading, contentLinkText, Resources.WhatToDo_Subheading, subSection, 7);
		}
		return new WarningPageContent(Resources.VideoLoss_Heading, contentLinkText, string.Empty, null, 7);
	}

	private WarningPageContent GetChargingMoreWarningContent()
	{
		string supportedDeviceListUrl = OemResources.Instance.App.SupportedDeviceListUrl;
		ContentLinkText details = null;
		ContentLinkText subSection = GetSubSection(supportedDeviceListUrl);
		Uri uri = GenerateAndSaveReport(supportedDeviceListUrl);
		if (uri != null)
		{
			List<string> list = new List<string>();
			List<Uri> list2 = new List<Uri>();
			list.Add(Resources.LicenseWarning_BodyLinkText);
			list2.Add(uri);
			details = new ContentLinkText("{0}", list.ToArray(), list2.ToArray());
		}
		else
		{
			LogManager.Write("Failed to generate license report.", TraceEventType.Error);
		}
		if (subSection != null)
		{
			return new WarningPageContent(Resources.LicenseUseageHeading, details, Resources.WhatToDo_Subheading, subSection, 14);
		}
		return new WarningPageContent(Resources.LicenseUseageHeading, details, string.Empty, null, 14);
	}

	private static ContentLinkText GetSubSection(string supportedDevicesString)
	{
		if (!string.IsNullOrWhiteSpace(supportedDevicesString) && Uri.TryCreate(supportedDevicesString, UriKind.Absolute, out var result))
		{
			List<string> list = new List<string>();
			list.Add(Resources.LicenseWarning_DetailLinkText);
			List<Uri> list2 = new List<Uri>();
			list2.Add(result);
			return new ContentLinkText(string.Format(CultureInfo.CurrentCulture, Resources.LicenseWarning_Detail, "{0}", OemResources.Instance.App.CompanyName), list.ToArray(), list2.ToArray());
		}
		return null;
	}

	[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Installer can not fail")]
	private Uri GenerateAndSaveReport(string oemSupportedDevicesLink)
	{
		if (_reportGenerator == null)
		{
			return null;
		}
		Dictionary<string, string> reportProperties = new Dictionary<string, string> { { "OemLink", oemSupportedDevicesLink } };
		byte[] array = _reportGenerator.GenerateReport(_licenseCalculations, _licenseDocument, reportProperties);
		string text = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "LicenseReport.html");
		if (array != null && array.Length != 0)
		{
			try
			{
				File.WriteAllBytes(text, array);
				return new Uri(text);
			}
			catch (Exception exception)
			{
				LogManager.Write("Failed to write license report to HTML file.", TraceEventType.Error);
				LogManager.Write(Encoding.UTF8.GetString(array));
				LogManager.Write(exception);
				return null;
			}
		}
		return null;
	}
}
