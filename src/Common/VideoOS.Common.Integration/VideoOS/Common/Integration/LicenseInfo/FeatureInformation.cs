using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo;

[DebuggerDisplay("FeatureInformation: {Name}")]
public class FeatureInformation
{
	private static readonly string[] AllDefinedFeaturesWithoutParameters = new string[49]
	{
		"Audio", "DeviceChannels", "DLKQuota", "Metadata", "HardwareLicensing", "ChannelLicensing", "ArchiveReducedFrameRate", "AxisOneClick", "FederatedSites", "AddToFederatedHierarchy",
		"EvidenceLock", "DevicePasswordManagement", "FirmwareUpdate", "LiveMultiStreaming", "MultistreamRecording", "Multicast", "RecordingServerFailover", "RecordingStorageEncryption", "RecordingStorageSigning", "RemoteConnectedServices",
		"Bookmarking", "RuleBasedBookmarking", "MIPUseInternally", "MIPUseExternally", "ManageAlarms", "SNMP", "TimeControlledAccessRights", "ManagementClientProfiles", "DifferentiatedAdministratorSecurity", "DualAuthorization",
		"ConfigurationReports", "SystemMonitor", "EdgeStorage", "SmartClientLogin", "MobileLogin", "ExtendedPtz", "CustomerDashboard", "SmartMap", "SmartMapOtherProviders", "HardwareAcceleratedVMD",
		"ExtendedPrivacyMasking", "SaveSearches", "TwoStepVerification", "NvidiaVMD", "VPS", "AdaptiveStreaming", "ManagementServerFailover", "MultipleSimultaneousSearches", "RestrictedMedia"
	};

	public string Name { get; private set; }

	public string DisplayName { get; private set; }

	public bool Licensed { get; private set; }

	public bool Expires => ExpiryDate != DateTime.MaxValue;

	public bool Visible { get; private set; }

	public DateTime ExpiryDate { get; private set; }

	public Collection<FeatureParameterInformation> Parameters { get; private set; }

	public FeatureInformation(string name, string displayName, bool licensed, DateTime expiryDate, bool visible)
	{
		Name = name;
		DisplayName = displayName;
		Licensed = licensed;
		ExpiryDate = expiryDate;
		Visible = visible;
		Parameters = new Collection<FeatureParameterInformation>();
	}

	protected static int AllFeaturesCount()
	{
		return AllDefinedFeaturesWithoutParameters.Length + 8;
	}

	public static IEnumerable<IFeature> QueryAllLicensedFeatures(ILicenseDocument licenseDocument)
	{
		if (licenseDocument == null)
		{
			return null;
		}
		Dictionary<string, IFeature> dictionary = new Dictionary<string, IFeature>();
		string[] allDefinedFeaturesWithoutParameters = AllDefinedFeaturesWithoutParameters;
		for (int i = 0; i < allDefinedFeaturesWithoutParameters.Length; i++)
		{
			AddToFeatureCollection(allDefinedFeaturesWithoutParameters[i], dictionary, licenseDocument);
		}
		AddToFeatureCollection("MultiStageVideoStorage", new List<string> { "MultiStageVideoStorageLimit" }, dictionary, visible: false, licenseDocument);
		AddToFeatureCollection("SmartClientProfiles", new List<string> { "SmartClientProfilesLimit" }, dictionary, visible: false, licenseDocument);
		AddToFeatureCollection("SmartWall", new List<string> { "SLC", "Expires" }, dictionary, visible: true, licenseDocument);
		AddToFeatureCollection("PTZPriority", new List<string> { "PTZPriorityLimit" }, dictionary, visible: false, licenseDocument);
		AddToFeatureCollection("InterconnectedProducts", new List<string> { "30002" }, dictionary, visible: false, licenseDocument);
		AddToFeatureCollection("AACAudioSupport", new List<string> { "EncodeCount", "DecodeCount", "AACLicenseCount" }, dictionary, visible: false, licenseDocument);
		AddToFeatureCollection("SearchCategories", new List<string> { "50F8A616-C795-49C4-A8EE-CAAB5C782CB8", "0BE9585D-ECFE-4142-BE82-A962157F7090", "8CB984FE-7A5E-4BF2-BDAA-E1E27EA48CFE", "A33FB5B5-5F9E-49DD-BD1C-1ABB813AF961", "DB8CB491-1713-4B4A-9E31-2FD30C748970", "72443CEB-5A71-41B8-95AB-160A11F7E8F6", "F1802236-DA43-4222-A056-501209379802", "7C4765C6-BD67-45A1-9E0B-84313207379C", "DE799EAA-7564-4882-8E86-B0FA85919559", "4E22B171-B878-4751-8184-DD87B2DD65F1" }, dictionary, visible: false, licenseDocument);
		AddToFeatureCollection("CustomLicenseType", new List<string> { "VPS Driver", "Arcules" }, dictionary, visible: false, licenseDocument);
		return new List<IFeature>(dictionary.Values);
	}

	private static void AddToFeatureCollection(string featureName, Dictionary<string, IFeature> featureCollection, ILicenseDocument licenseDocument)
	{
		AddToFeatureCollection(featureName, null, featureCollection, visible: false, licenseDocument);
	}

	private static void AddToFeatureCollection(string featureName, IEnumerable<string> parameters, Dictionary<string, IFeature> featureCollection, bool visible, ILicenseDocument licenseDocument)
	{
		if (string.IsNullOrEmpty(featureName))
		{
			throw new ArgumentException("featureName is null or empty", "featureName");
		}
		if (!licenseDocument.IsFeatureLicensed(featureName))
		{
			return;
		}
		IFeature feature = LicenseFactory.CreateNewFeature(featureName);
		feature.Visible = visible;
		if (parameters != null)
		{
			foreach (string parameter in parameters)
			{
				try
				{
					feature.Parameters[parameter] = GetFeatureParameterValue(featureName, parameter, licenseDocument);
				}
				catch (ArgumentException)
				{
				}
			}
		}
		featureCollection[featureName] = feature;
	}

	private static string GetFeatureParameterValue(string featureName, string featureParameterName, ILicenseDocument licenseDocument)
	{
		return licenseDocument.GetFeatureParameterValue(featureName, featureParameterName);
	}
}
