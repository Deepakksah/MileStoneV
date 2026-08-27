using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Security;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.Properties;
using VideoOS.Server.RuleActions;

namespace VideoOS.Server.Licensing;

public sealed class LicenseFeatureChecker
{
	private ILicenseManager _licenseManager;

	private string _snmpRuleActionName;

	private string _bookmarkRuleActionName;

	private string SnmpRuleActionName
	{
		get
		{
			if (_snmpRuleActionName == null)
			{
				_snmpRuleActionName = LookupRuleActionName(typeof(SendSNMPTrapRuleActionFactory));
			}
			return _snmpRuleActionName;
		}
	}

	private string BookmarkRuleActionName
	{
		get
		{
			if (_bookmarkRuleActionName == null)
			{
				_bookmarkRuleActionName = LookupRuleActionName(typeof(SetBookmarkRuleActionFactory));
			}
			return _bookmarkRuleActionName;
		}
	}

	public LicenseFeatureChecker(ILicenseManager licenseManager)
	{
		_licenseManager = licenseManager;
	}

	public void CheckSecurityDataSet(SecurityDataSet securityDataSet)
	{
		if (_licenseManager.IsFeatureLicensed("TimeControlledAccessRights"))
		{
			return;
		}
		if (securityDataSet.TimeProfileDefaultRoleRelation.Count > 0)
		{
			foreach (SecurityDataSet.TimeProfileDefaultRoleRelationRow row in securityDataSet.TimeProfileDefaultRoleRelation.Rows)
			{
				if (!row.RowState.HasFlag(DataRowState.Deleted))
				{
					throw new ServerServiceException(Resources.IServerFeatureNotLicensed);
				}
			}
		}
		if (securityDataSet.SecurityPermissions.Count > 0)
		{
			foreach (SecurityDataSet.SecurityPermissionsRow row2 in securityDataSet.SecurityPermissions.Rows)
			{
				if (!row2.RowState.HasFlag(DataRowState.Deleted) && !row2.IsDefaultTimeProfile && !row2.IsIDTimeProfileNull())
				{
					throw new ServerServiceException(Resources.IServerFeatureNotLicensed);
				}
			}
		}
		if (securityDataSet.SecurityPermissionsDevice.Count <= 0)
		{
			return;
		}
		foreach (SecurityDataSet.SecurityPermissionsDeviceRow row3 in securityDataSet.SecurityPermissionsDevice.Rows)
		{
			if (!row3.RowState.HasFlag(DataRowState.Deleted) && !row3.IsDefaultTimeProfile && !row3.IsIDTimeProfileNull())
			{
				throw new ServerServiceException(Resources.IServerFeatureNotLicensed);
			}
		}
	}

	public void CheckConfigurationDataSet(ConfigurationDataSet configDataSet)
	{
		bool snmpLicensed = _licenseManager.IsFeatureLicensed("SNMP");
		bool bookmarkLicensed = _licenseManager.IsFeatureLicensed("RuleBasedBookmarking");
		bool liveMultistreamingLicensed = _licenseManager.IsFeatureLicensed("LiveMultiStreaming");
		bool multistreamRecordingLicensed = _licenseManager.IsFeatureLicensed("MultistreamRecording");
		bool recorderFailoverLicensed = _licenseManager.IsFeatureLicensed("RecordingServerFailover");
		bool multicastLicensed = _licenseManager.IsFeatureLicensed("Multicast");
		bool aacLicensed = _licenseManager.IsFeatureLicensed("AACAudioSupport");
		bool vpsLicensed = _licenseManager.IsFeatureLicensed("VPS");
		CheckConfigurationDataSet_Rules(configDataSet, snmpLicensed, bookmarkLicensed);
		CheckConfigurationDataSet_Aac(configDataSet, aacLicensed);
		CheckConfigurationDataSet_StreamsAndMulticast(configDataSet, liveMultistreamingLicensed, multistreamRecordingLicensed, multicastLicensed);
		CheckConfigurationDataSet_FailoverAndMulticast(configDataSet, recorderFailoverLicensed, multicastLicensed);
		CheckConfigurationDataSet_Vps(configDataSet, vpsLicensed);
	}

	private void CheckConfigurationDataSet_Rules(ConfigurationDataSet configDataSet, bool snmpLicensed, bool bookmarkLicensed)
	{
		if ((snmpLicensed && bookmarkLicensed) || configDataSet.Rules.Count <= 0)
		{
			return;
		}
		foreach (ConfigurationDataSet.RulesRow row in configDataSet.Rules.Rows)
		{
			if (row.RowState == DataRowState.Deleted)
			{
				continue;
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(row.Definition);
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes(string.Format(CultureInfo.InvariantCulture, "//{0}/{1}", "Actions", "Action"));
			foreach (XmlNode item in xmlNodeList)
			{
				XmlAttribute xmlAttribute = item.Attributes["name"];
				if (xmlAttribute != null && ((!snmpLicensed && xmlAttribute.Value.Equals(SnmpRuleActionName)) || (!bookmarkLicensed && xmlAttribute.Value.Equals(BookmarkRuleActionName))))
				{
					throw new ServerServiceException(Resources.IServerFeatureNotLicensed);
				}
			}
		}
	}

	private static void CheckConfigurationDataSet_Aac(ConfigurationDataSet configDataSet, bool aacLicensed)
	{
		if (aacLicensed)
		{
			return;
		}
		foreach (ConfigurationDataSet.HardwareRow item in configDataSet.Hardware)
		{
			if (item.RowState != DataRowState.Deleted)
			{
				CheckAacConfigured(item.Settings);
			}
		}
		foreach (ConfigurationDataSet.DevicesRow device in configDataSet.Devices)
		{
			if (device.RowState != DataRowState.Deleted && RegisteredDeviceType.IsAudioDevice(device.DeviceType))
			{
				CheckAacConfigured(device.Settings);
			}
		}
	}

	private static void CheckConfigurationDataSet_Vps(ConfigurationDataSet configDataSet, bool vpsLicensed)
	{
		if (vpsLicensed)
		{
			return;
		}
		foreach (ConfigurationDataSet.HardwareRow item in configDataSet.Hardware)
		{
			if (item.RowState != DataRowState.Deleted && DriverNumbering.IsVpsDriver(item.DriverNumber))
			{
				throw new ServerServiceException(Resources.IServerFeatureNotLicensed);
			}
		}
	}

	private static void CheckAacConfigured(string settingsXml)
	{
		DriverSettings driverSettings = new DriverSettings(settingsXml);
		IEnumerable<SettingEntry> settingsByTypeReferenceId = driverSettings.GetSettingsByTypeReferenceId(TypeReferenceIds.AudioCodec.ToString());
		if (settingsByTypeReferenceId.Count() == 0 || !settingsByTypeReferenceId.Any(DefinitionUtility.IsSettingAac))
		{
			return;
		}
		throw new ServerServiceException(Resources.IServerFeatureNotLicensed);
	}

	private static void CheckConfigurationDataSet_StreamsAndMulticast(ConfigurationDataSet configDataSet, bool liveMultistreamingLicensed, bool multistreamRecordingLicensed, bool multicastLicensed)
	{
		if (liveMultistreamingLicensed && multistreamRecordingLicensed && multicastLicensed)
		{
			return;
		}
		foreach (ConfigurationDataSet.DevicesRow device in configDataSet.Devices)
		{
			if (device.RowState == DataRowState.Deleted)
			{
				continue;
			}
			if (!string.IsNullOrEmpty(device.Streams) && (!liveMultistreamingLicensed || !multistreamRecordingLicensed))
			{
				StreamsConfiguration streamsConfiguration = new StreamsConfiguration();
				streamsConfiguration.LoadXml(device.Streams);
				if (!liveMultistreamingLicensed && (streamsConfiguration.Count > 2 || streamsConfiguration.Count((StreamInfo si) => si.LiveFeed) > 1))
				{
					throw new ServerServiceException(Resources.IServerFeatureNotLicensed);
				}
				if (!multistreamRecordingLicensed && streamsConfiguration.Count((StreamInfo si) => si.TrackDefinition.HasValue) > 1)
				{
					throw new ServerServiceException(Resources.IServerFeatureNotLicensed);
				}
			}
			if (!multicastLicensed && device.MulticastEnabled)
			{
				throw new ServerServiceException(Resources.IServerFeatureNotLicensed);
			}
		}
	}

	private static void CheckConfigurationDataSet_FailoverAndMulticast(ConfigurationDataSet configDataSet, bool recorderFailoverLicensed, bool multicastLicensed)
	{
		if ((recorderFailoverLicensed && multicastLicensed) || configDataSet.Recorders.Count <= 0)
		{
			return;
		}
		foreach (ConfigurationDataSet.RecordersRow row in configDataSet.Recorders.Rows)
		{
			if (row.RowState == DataRowState.Deleted)
			{
				continue;
			}
			if (!recorderFailoverLicensed && (row.IDFailoverPrimaryGroup != Guid.Empty.ToString() || row.IDFailoverSecondaryGroup != Guid.Empty.ToString()))
			{
				throw new ServerServiceException(Resources.IServerFeatureNotLicensed);
			}
			if (!multicastLicensed && !string.IsNullOrEmpty(row.MulticastSettings))
			{
				VideoOS.Common.Integration.Settings settings = new VideoOS.Common.Integration.Settings(row.MulticastSettings);
				if (Convert.ToBoolean(settings.Get("MulticastEnabled"), CultureInfo.InvariantCulture))
				{
					throw new ServerServiceException(Resources.IServerFeatureNotLicensed);
				}
			}
		}
	}

	private static string LookupRuleActionName(Type ruleActionFactory)
	{
		object[] customAttributes = ruleActionFactory.GetCustomAttributes(typeof(RuleActionFactoryAttribute), inherit: false);
		if (customAttributes.Length != 0 && customAttributes[0] is RuleActionFactoryAttribute ruleActionFactoryAttribute)
		{
			return ruleActionFactoryAttribute.RuleActionName;
		}
		return string.Empty;
	}
}
