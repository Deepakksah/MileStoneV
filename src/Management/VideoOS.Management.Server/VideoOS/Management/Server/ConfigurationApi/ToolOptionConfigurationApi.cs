using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Sockets;
using System.Xml;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.LogServer.Communication;
using VideoOS.LogServer.Communication.LogConfigurationServiceReference;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Network;
using VideoOS.Utilities.Validation;
using VideoOS.Vms.Utilities.SystemSettings;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIToolOptionFolder")]
[EntityName("ConfigAPIToolOption")]
public class ToolOptionConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	private const int MaxTimeValue = 999;

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 35);
		item.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 36);
		path.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 37);
		parentPath.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 38);
		ValidateInternal(requestContext, item, path);
		if (path.ItemType != "ToolOption")
		{
			throw new PathNotFoundException(path.Text);
		}
		bool flag = GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "Bookmarking");
		Guid guid = new Guid(path.Id);
		if (guid == ToolOptionIds.RecordingServerToolOptionId)
		{
			return UpdateRecordingServerOptions(requestContext, item, path);
		}
		if (guid == ToolOptionIds.ServerLogToolOptionId)
		{
			return UpdateServerLogOptions(requestContext, item);
		}
		if (guid == ToolOptionIds.BookmarkToolOptionId && flag)
		{
			return UpdateBookmarkOptions(requestContext, item, path);
		}
		if (guid == ToolOptionIds.PrivacySettingsToolOptionId)
		{
			return UpdatePrivacySettingsOptions(requestContext, item, path);
		}
		if (guid == ToolOptionIds.CustomerDashboardToolOptionId)
		{
			return UpdateCUDAOptions(requestContext, item, path);
		}
		if (guid == ToolOptionIds.MailServerToolOptionId)
		{
			return UpdateMailServerOptions(requestContext, item, path);
		}
		if (guid == ToolOptionIds.NetworkToolOptionId)
		{
			return UpdateNetworkOptions(requestContext, item, path);
		}
		if (guid == ToolOptionIds.AviGenerationToolOptionId)
		{
			return UpdateAviGenerationOptions(requestContext, item, path);
		}
		throw new PathNotFoundException(path.Text);
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 76);
		item.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 77);
		path.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 78);
		parentPath.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 79);
		try
		{
			return ValidateInternal(requestContext, item, path);
		}
		catch (ArgumentNullException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
		catch (ArgumentException exception2)
		{
			ValidateResult validateResult2 = ErrorHelper.CreateValidateResult(exception2, item.Properties);
			if (validateResult2 != null)
			{
				return validateResult2;
			}
			throw;
		}
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		throw new NotSupportedException();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 108);
		path.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 109);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("ToolOptionFolder", ConfigurationItemPath.CreateSystemPath());
		}
		if (path.ItemType != "ToolOption")
		{
			throw new PathNotFoundException(path.Text);
		}
		bool flag = GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "Bookmarking");
		Guid guid = new Guid(path.Id);
		if (guid == ToolOptionIds.RecordingServerToolOptionId)
		{
			return ConstructRecordingServerOptions(requestContext);
		}
		if (guid == ToolOptionIds.ServerLogToolOptionId)
		{
			return ConstructServerLogOptions(requestContext);
		}
		if (guid == ToolOptionIds.BookmarkToolOptionId && flag)
		{
			return ConstructBookmarkOptions(requestContext);
		}
		if (guid == ToolOptionIds.PrivacySettingsToolOptionId)
		{
			return ConstructPrivacySettingsOptions(requestContext);
		}
		if (guid == ToolOptionIds.CustomerDashboardToolOptionId)
		{
			return ConstructCUDAOptions(requestContext);
		}
		if (guid == ToolOptionIds.MailServerToolOptionId)
		{
			return ConstructMailServerOptions(requestContext);
		}
		if (guid == ToolOptionIds.NetworkToolOptionId)
		{
			return ConstructNetworkOptions(requestContext);
		}
		if (guid == ToolOptionIds.AviGenerationToolOptionId)
		{
			return ConstructAviGenerationOptions(requestContext);
		}
		throw new PathNotFoundException(path.Text);
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 152);
		path.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\ToolOptions\\ToolOptionConfigurationApi.cs", 153);
		if (path.IsFolder)
		{
			bool num = GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "Bookmarking");
			List<ConfigurationItem> list = new List<ConfigurationItem>
			{
				ConstructRecordingServerOptions(requestContext),
				ConstructServerLogOptions(requestContext),
				ConstructPrivacySettingsOptions(requestContext),
				ConstructCUDAOptions(requestContext),
				ConstructMailServerOptions(requestContext),
				ConstructNetworkOptions(requestContext),
				ConstructAviGenerationOptions(requestContext)
			};
			if (num)
			{
				list.Add(ConstructBookmarkOptions(requestContext));
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	private static ValidateResult ValidateInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (item.ItemType != "ToolOption")
		{
			throw new PathNotFoundException();
		}
		if (item.Properties == null)
		{
			item.Properties = new Property[0];
		}
		Guid guid = new Guid(path.Id);
		if (guid == ToolOptionIds.RecordingServerToolOptionId)
		{
			return ValidateInternalRecordingServer(requestContext, item);
		}
		if (guid == ToolOptionIds.ServerLogToolOptionId)
		{
			return ValidateInternalServerLog(item);
		}
		if (guid == ToolOptionIds.BookmarkToolOptionId)
		{
			return ValidateInternalBookmark(requestContext, item);
		}
		if (guid == ToolOptionIds.PrivacySettingsToolOptionId)
		{
			return ValidateInternalPrivacySettings(requestContext, item);
		}
		if (guid == ToolOptionIds.CustomerDashboardToolOptionId)
		{
			return ValidateInternalCUDA(requestContext, item);
		}
		if (guid == ToolOptionIds.MailServerToolOptionId)
		{
			return ValidateInternalMailServerOptions(requestContext, item);
		}
		LocalIPRangeXmlFormatter formatter;
		bool? ipv;
		if (guid == ToolOptionIds.NetworkToolOptionId)
		{
			return ValidateInternalNetworkOptions(requestContext, item, out formatter, out ipv);
		}
		Guid selectedGuid;
		if (guid == ToolOptionIds.AviGenerationToolOptionId)
		{
			return ValidateInternalAviGeneration(requestContext, item, out selectedGuid);
		}
		throw new PathNotFoundException("Option Id not recognized");
	}

	private static ValidateResult ValidateInternalRecordingServer(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PtzManualSessionTimeoutSec", out string value) || !int.TryParse(value, out var _))
		{
			throw new PropertyInvalidException("", "PtzManualSessionTimeoutSec");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PausePatrollingTimeoutSec", out string value2) || !int.TryParse(value2, out var _))
		{
			throw new PropertyInvalidException("", "PausePatrollingTimeoutSec");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PtzReservedSessionTimeoutSec", out string value3) || !int.TryParse(value3, out var _))
		{
			throw new PropertyInvalidException("", "PtzReservedSessionTimeoutSec");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PtzUseDefaultPresetAsPtzHome", out string value4) || !bool.TryParse(value4, out var _))
		{
			throw new PropertyInvalidException("", "PtzUseDefaultPresetAsPtzHome");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DeviceErrorTimeoutSec", out string value5) || !int.TryParse(value5, out var _))
		{
			throw new PropertyInvalidException("", "DeviceErrorTimeoutSec");
		}
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static ConfigurationItem ConstructRecordingServerOptions(IGeminiRequestContext requestContext)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "ToolOptionFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("ToolOption", ToolOptionIds.RecordingServerToolOptionId.ToString());
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "ToolOption",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = ConfigurationApiResources.ToolOptionRecordingServerDisplayName,
			ParentPath = configurationItemPath.Text,
			Path = configurationItemPath2.Text
		};
		List<Property> list = new List<Property>();
		obj.MethodIds = new string[0];
		string systemSetting = SystemSetting.GetSystemSetting(requestContext, "PtzManualSessionTimeoutSec");
		string systemSetting2 = SystemSetting.GetSystemSetting(requestContext, "PausePatrollingTimeoutSec");
		string systemSetting3 = SystemSetting.GetSystemSetting(requestContext, "PtzReservedSessionTimeoutSec");
		string systemSetting4 = SystemSetting.GetSystemSetting(requestContext, "PtzUseDefaultPresetAsPtzHome");
		string systemSetting5 = SystemSetting.GetSystemSetting(requestContext, "DeviceErrorTimeoutSec");
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("PtzManualSessionTimeoutSec", systemSetting, "ToolOptionTimeoutSeconds_ToolTip"));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("PausePatrollingTimeoutSec", systemSetting2, "ToolOptionTimeoutSeconds_ToolTip"));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("PtzReservedSessionTimeoutSec", systemSetting3, "ToolOptionTimeoutSeconds_ToolTip"));
		list.Add(ToolOptionPropertyDefinitions.GetTickProperty("PtzUseDefaultPresetAsPtzHome", systemSetting4, "", settable: true));
		list.Add(ToolOptionPropertyDefinitions.GetDeviceErrorTimeoutSecProperty(systemSetting5, "ToolOptionDeviceErrorSeconds_ToolTip"));
		obj.Properties = list.ToArray();
		return obj;
	}

	private static ValidateResult UpdateRecordingServerOptions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PtzManualSessionTimeoutSec", out string value) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PausePatrollingTimeoutSec", out string value2) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PtzReservedSessionTimeoutSec", out string value3) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PtzUseDefaultPresetAsPtzHome", out string value4) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DeviceErrorTimeoutSec", out string value5))
		{
			throw new PropertyInvalidException("Missing property");
		}
		AdjustSecondsToAllowedValues(ref value);
		AdjustSecondsToAllowedValues(ref value2);
		AdjustSecondsToAllowedValues(ref value3);
		if (!bool.TryParse(value4, out var result))
		{
			throw new PropertyInvalidException("", "PtzUseDefaultPresetAsPtzHome");
		}
		value5 = AdjustSecondsToAllowedValuesCommunicationError(value5).ToString(CultureInfo.InvariantCulture);
		SystemSetting.SetSystemSetting(requestContext, "PtzManualSessionTimeoutSec", value);
		SystemSetting.SetSystemSetting(requestContext, "PausePatrollingTimeoutSec", value2);
		SystemSetting.SetSystemSetting(requestContext, "PtzReservedSessionTimeoutSec", value3);
		SystemSetting.SetSystemSetting(requestContext, "PtzUseDefaultPresetAsPtzHome", result.ToString());
		SystemSetting.SetSystemSetting(requestContext, "DeviceErrorTimeoutSec", value5);
		return new ValidateResult(ConstructRecordingServerOptions(requestContext));
	}

	private static int AdjustSecondsToAllowedValuesCommunicationError(string value)
	{
		int num = int.Parse(value, CultureInfo.InvariantCulture);
		if (num < 0)
		{
			throw new InvalidProgramException("value cannot less than 0");
		}
		if (num == 0)
		{
			return 0;
		}
		if (num < 5)
		{
			return 1;
		}
		if (num < 10)
		{
			return 5;
		}
		if (num < 15)
		{
			return 10;
		}
		if (num < 30)
		{
			return 15;
		}
		if (num < 45)
		{
			return 30;
		}
		if (num < 60)
		{
			return 45;
		}
		if (num < 120)
		{
			return 60;
		}
		if (num < 300)
		{
			return 120;
		}
		return 300;
	}

	private static void AdjustSecondsToAllowedValues(ref string value)
	{
		int num = int.Parse(value, CultureInfo.InvariantCulture);
		if (num <= 999)
		{
			return;
		}
		int num2 = num / 60;
		if (num2 <= 999)
		{
			value = (num2 * 60).ToString(CultureInfo.InvariantCulture);
			return;
		}
		int num3 = num / 3600;
		if (num3 <= 999)
		{
			value = (num3 * 3600).ToString(CultureInfo.InvariantCulture);
			return;
		}
		throw new PropertyInvalidException();
	}

	private static ValidateResult ValidateInternalServerLog(ConfigurationItem item)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SystemLogEnabled", out string value) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SystemLogLevel", out string _) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SystemDaysToKeep", out string _) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AuditLogEnabled", out string value4) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AuditDaysToKeep", out string _) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AuditUserAccessLogging", out string value6) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AuditPlaybackSequenceLogLength", out string _) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AuditRecordsSeenBeforeLogging", out string _) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RuleLogEnabled", out string value9) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RuleDaysToKeep", out string _))
		{
			throw new PropertyInvalidException("Missing property");
		}
		if (!bool.TryParse(value, out var result))
		{
			throw new PropertyInvalidException("SystemLogEnabled");
		}
		if (!bool.TryParse(value4, out result))
		{
			throw new PropertyInvalidException("AuditLogEnabled");
		}
		if (!bool.TryParse(value6, out result))
		{
			throw new PropertyInvalidException("AuditUserAccessLogging");
		}
		if (!bool.TryParse(value9, out result))
		{
			throw new PropertyInvalidException("RuleLogEnabled");
		}
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static ConfigurationItem ConstructServerLogOptions(IGeminiRequestContext requestContext)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "ToolOptionFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("ToolOption", ToolOptionIds.ServerLogToolOptionId.ToString());
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "ToolOption";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.ToolOptionServerLogDisplayName;
		configurationItem.ParentPath = configurationItemPath.Text;
		configurationItem.Path = configurationItemPath2.Text;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[0];
		string systemSetting = SystemSetting.GetSystemSetting(requestContext, "LogSystemEnabled");
		string systemSetting2 = SystemSetting.GetSystemSetting(requestContext, "LogAuditEnabled");
		string systemSetting3 = SystemSetting.GetSystemSetting(requestContext, "LogRuleEnabled");
		ILogServerConfigurationComponent logServerConfigurationComponent = requestContext.CreateLogServerConfigurationComponent();
		string value;
		string value2;
		string value3;
		try
		{
			LogRetentions logRetentions = logServerConfigurationComponent.QueryLogServerConfiguration();
			if (logRetentions != null)
			{
				value = logRetentions.SystemLogRetention.ToString(CultureInfo.InvariantCulture);
				value2 = logRetentions.AuditLogRetention.ToString(CultureInfo.InvariantCulture);
				value3 = logRetentions.RuleLogRetention.ToString(CultureInfo.InvariantCulture);
			}
			else
			{
				value = string.Empty;
				value2 = string.Empty;
				value3 = string.Empty;
			}
		}
		catch (CommunicationServiceException)
		{
			value = string.Empty;
			value2 = string.Empty;
			value3 = string.Empty;
		}
		string systemSetting4 = SystemSetting.GetSystemSetting(requestContext, "LogSystemLevel");
		list.Add(ToolOptionPropertyDefinitions.GetTickProperty("SystemLogEnabled", (string.IsNullOrEmpty(systemSetting) || "true".Equals(systemSetting, StringComparison.OrdinalIgnoreCase)).ToString(), "", !string.IsNullOrEmpty(systemSetting)));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("SystemDaysToKeep", value, ""));
		list.Add(ToolOptionPropertyDefinitions.GetLogLevelProperty("SystemLogLevel", systemSetting4, ""));
		string systemSetting5 = SystemSetting.GetSystemSetting(requestContext, SettingList.LogAuditEnableUserAccessLogging.ToString());
		string systemSetting6 = SystemSetting.GetSystemSetting(requestContext, SettingList.LogAuditPlaybackWindowSizeInSec.ToString());
		string systemSetting7 = SystemSetting.GetSystemSetting(requestContext, SettingList.LogAuditPlaybackFramesBeforeLogging.ToString());
		list.Add(ToolOptionPropertyDefinitions.GetTickProperty("AuditLogEnabled", (string.IsNullOrEmpty(systemSetting2) || "true".Equals(systemSetting2, StringComparison.OrdinalIgnoreCase)).ToString(), "", !string.IsNullOrEmpty(systemSetting2)));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("AuditDaysToKeep", value2, ""));
		list.Add(ToolOptionPropertyDefinitions.GetTickProperty("AuditUserAccessLogging", systemSetting5, "", settable: true));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("AuditPlaybackSequenceLogLength", systemSetting6, ""));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("AuditRecordsSeenBeforeLogging", systemSetting7, ""));
		list.Add(ToolOptionPropertyDefinitions.GetTickProperty("RuleLogEnabled", systemSetting3, "", settable: true));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("RuleDaysToKeep", value3, ""));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private static ValidateResult UpdateServerLogOptions(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		string value = "";
		string value2 = "";
		string value3 = "";
		ILogServerConfigurationComponent logServerConfigurationComponent = requestContext.CreateLogServerConfigurationComponent();
		LogRetentions logRetentions = null;
		try
		{
			logRetentions = logServerConfigurationComponent.QueryLogServerConfiguration();
		}
		catch (CommunicationServiceException)
		{
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SystemLogEnabled", out string value4))
		{
			throw new PropertyInvalidException("", "SystemLogEnabled");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AuditLogEnabled", out string value5))
		{
			throw new PropertyInvalidException("", "AuditLogEnabled");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RuleLogEnabled", out string value6))
		{
			throw new PropertyInvalidException("", "RuleLogEnabled");
		}
		int result;
		if (logRetentions != null)
		{
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SystemDaysToKeep", out value) || !int.TryParse(value, out result))
			{
				throw new PropertyInvalidException("", "SystemDaysToKeep");
			}
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AuditDaysToKeep", out value2) || !int.TryParse(value2, out result))
			{
				throw new PropertyInvalidException("", "AuditDaysToKeep");
			}
			if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("RuleDaysToKeep", out value3) || !int.TryParse(value3, out result))
			{
				throw new PropertyInvalidException("", "RuleDaysToKeep");
			}
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SystemLogLevel", out string value7) || !int.TryParse(value7, out result) || result < 0 || result > 3)
		{
			throw new PropertyInvalidException("", "SystemLogLevel");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AuditUserAccessLogging", out string value8))
		{
			throw new PropertyInvalidException("", "AuditUserAccessLogging");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AuditPlaybackSequenceLogLength", out string value9) || !int.TryParse(value9, out result) || result < 30 || result > 3600)
		{
			throw new PropertyInvalidException("", "AuditPlaybackSequenceLogLength");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("AuditRecordsSeenBeforeLogging", out string value10) || !int.TryParse(value10, out result) || result < 0 || result > 20)
		{
			throw new PropertyInvalidException("", "AuditRecordsSeenBeforeLogging");
		}
		string systemSetting = SystemSetting.GetSystemSetting(requestContext, "LogSystemEnabled");
		string systemSetting2 = SystemSetting.GetSystemSetting(requestContext, "LogAuditEnabled");
		string systemSetting3 = SystemSetting.GetSystemSetting(requestContext, "LogRuleEnabled");
		if (!string.IsNullOrEmpty(systemSetting))
		{
			SystemSetting.SetSystemSetting(requestContext, "LogSystemEnabled", value4);
		}
		if (!string.IsNullOrEmpty(systemSetting2))
		{
			SystemSetting.SetSystemSetting(requestContext, "LogAuditEnabled", value5);
		}
		if (!string.IsNullOrEmpty(systemSetting3))
		{
			SystemSetting.SetSystemSetting(requestContext, "LogRuleEnabled", value6);
		}
		if (logRetentions != null)
		{
			logRetentions.SystemLogRetention = StringToInt(value);
			logRetentions.AuditLogRetention = StringToInt(value2);
			logRetentions.RuleLogRetention = StringToInt(value3);
			try
			{
				logServerConfigurationComponent.UpdateLogServerConfiguration(logRetentions);
			}
			catch (AggregateException)
			{
			}
		}
		SystemSetting.SetSystemSetting(requestContext, "LogSystemLevel", value7);
		SystemSetting.SetSystemSetting(requestContext, "LogAuditEnableUserAccessLogging", value8);
		SystemSetting.SetSystemSetting(requestContext, "LogAuditPlaybackWindowSizeInSec", value9);
		SystemSetting.SetSystemSetting(requestContext, "LogAuditPlaybackFramesBeforeLogging", value10);
		return new ValidateResult(ConstructServerLogOptions(requestContext));
	}

	private static int StringToInt(string days)
	{
		return int.Parse(days, CultureInfo.InvariantCulture);
	}

	private static ValidateResult ValidateInternalBookmark(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("BookmarkIdPrefix", out string _))
		{
			throw new PropertyInvalidException("", "BookmarkIdPrefix");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("BookmarkPreTime", out string value2) || !int.TryParse(value2, out var result) || result < 0 || result > 99)
		{
			throw new PropertyInvalidException("", "BookmarkPreTime");
		}
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("BookmarkPostTime", out string value3) || !int.TryParse(value3, out var result2) || result2 < 0 || result2 > 999)
		{
			throw new PropertyInvalidException("", "BookmarkPostTime");
		}
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static ConfigurationItem ConstructBookmarkOptions(IGeminiRequestContext requestContext)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "ToolOptionFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("ToolOption", ToolOptionIds.BookmarkToolOptionId.ToString());
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "ToolOption",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = ConfigurationApiResources.ToolOptionBookmarkDisplayName,
			ParentPath = configurationItemPath.Text,
			Path = configurationItemPath2.Text
		};
		List<Property> list = new List<Property>();
		obj.MethodIds = new string[0];
		string systemSetting = SystemSetting.GetSystemSetting(requestContext, "BookmarkIdPrefix");
		string systemSetting2 = SystemSetting.GetSystemSetting(requestContext, "BookmarkPreTime");
		string systemSetting3 = SystemSetting.GetSystemSetting(requestContext, "BookmarkPostTime");
		list.Add(ToolOptionPropertyDefinitions.GetStringProperty("BookmarkIdPrefix", systemSetting, "ToolOptionBookmarkIdPrefix_ToolTip"));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("BookmarkPreTime", systemSetting2, "ToolOptionBookmarkPreTime_ToolTip"));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("BookmarkPostTime", systemSetting3, "ToolOptionBookmarkPostTime_ToolTip"));
		obj.Properties = list.ToArray();
		return obj;
	}

	private static ValidateResult UpdateBookmarkOptions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("BookmarkIdPrefix", out string value) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("BookmarkPreTime", out string value2) || !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("BookmarkPostTime", out string value3))
		{
			throw new PropertyInvalidException("Missing property");
		}
		SystemSetting.SetSystemSetting(requestContext, "BookmarkIdPrefix", value);
		SystemSetting.SetSystemSetting(requestContext, "BookmarkPreTime", value2);
		SystemSetting.SetSystemSetting(requestContext, "BookmarkPostTime", value3);
		return new ValidateResult(ConstructBookmarkOptions(requestContext));
	}

	private static ValidateResult ValidateInternalPrivacySettings(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CollectUsageDataEnabled", out string value) || !bool.TryParse(value, out var result))
		{
			throw new PropertyInvalidException("", "CollectUsageDataEnabled");
		}
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CollectTelemetryEnabled", out string value2) && !bool.TryParse(value2, out result))
		{
			throw new PropertyInvalidException("", "CollectTelemetryEnabled");
		}
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static ConfigurationItem ConstructPrivacySettingsOptions(IGeminiRequestContext requestContext)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "ToolOptionFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("ToolOption", ToolOptionIds.PrivacySettingsToolOptionId.ToString());
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "ToolOption",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = ConfigurationApiResources.ToolOptionPrivacySettingsDisplayName,
			ParentPath = configurationItemPath.Text,
			Path = configurationItemPath2.Text
		};
		List<Property> list = new List<Property>();
		obj.MethodIds = new string[0];
		string systemSetting = SystemSetting.GetSystemSetting(requestContext, "CollectUsageDataEnabled");
		string systemSetting2 = SystemSetting.GetSystemSetting(requestContext, "CollectTelemetryEnabled");
		list.Add(ToolOptionPropertyDefinitions.GetTickProperty("CollectUsageDataEnabled", systemSetting, "", settable: true));
		list.Add(ToolOptionPropertyDefinitions.GetTickProperty("CollectTelemetryEnabled", systemSetting2, "ToolOptionCollectTelemetryEnabled_ToolTip", settable: true));
		obj.Properties = list.ToArray();
		return obj;
	}

	private static ValidateResult UpdatePrivacySettingsOptions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CollectUsageDataEnabled", out string value))
		{
			throw new PropertyInvalidException("Missing property");
		}
		SystemSetting.SetSystemSetting(requestContext, "CollectUsageDataEnabled", value);
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("CollectTelemetryEnabled", out string value2))
		{
			SystemSetting.SetSystemSetting(requestContext, "CollectTelemetryEnabled", value2);
		}
		return new ValidateResult(ConstructPrivacySettingsOptions(requestContext));
	}

	private static ValidateResult ValidateInternalCUDA(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static ConfigurationItem ConstructCUDAOptions(IGeminiRequestContext requestContext)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "ToolOptionFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("ToolOption", ToolOptionIds.CustomerDashboardToolOptionId.ToString());
		bool enabled = CUDAXmlToOptions(CustomSetting.GetCustomSettingDataGlobal(requestContext, ToolOptionIds.CustomerDashboardToolOptionId)?.CustomSettingDataXml);
		return new ConfigurationItem
		{
			ItemType = "ToolOption",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = ConfigurationApiResources.ToolOptionCUDADisplayName,
			ParentPath = configurationItemPath.Text,
			Path = configurationItemPath2.Text,
			MethodIds = new string[0],
			EnableProperty = new EnablePropertyInfo
			{
				DisplayName = "Enabled",
				Enabled = enabled
			},
			Properties = new Property[0]
		};
	}

	private static ValidateResult UpdateCUDAOptions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		bool flag = item.EnableProperty?.Enabled ?? false;
		MIPKind kind = MIPKind.GetKind(requestContext, ToolOptionIds.CustomerDashboardToolOptionId);
		if (kind == null)
		{
			throw new ArgumentException("No Kind found for CustomerDashboard");
		}
		int dataVersion = kind.DataVersion + 1;
		CustomSettingInternalData customSettingInternalData = new CustomSettingInternalData
		{
			CustomSettingDataXml = CUDAOptionsToXmlString(flag.ToString()),
			DataVersion = dataVersion
		};
		CustomSetting.SetCustomSettingDataGlobal(requestContext, ToolOptionIds.CustomerDashboardToolOptionId, customSettingInternalData, forceOverride: false);
		kind.DataVersion = dataVersion;
		kind.ModifiedUser = requestContext.UserDomainAccountName;
		MIPKind.SaveKind(requestContext, kind);
		return new ValidateResult(ConstructCUDAOptions(requestContext));
	}

	private static string CUDAOptionsToXmlString(string enabledString)
	{
		XmlElement xmlElement = new XmlDocument().CreateElement("Enabled");
		xmlElement.InnerText = enabledString;
		return xmlElement.OuterXml;
	}

	private static bool CUDAXmlToOptions(string xml)
	{
		bool result = false;
		if (!string.IsNullOrEmpty(xml))
		{
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.XmlResolver = null;
			XmlReader reader = XmlReader.Create(new StringReader(xml), new XmlReaderSettings
			{
				XmlResolver = null
			});
			xmlDocument.Load(reader);
			bool.TryParse(xmlDocument.FirstChild.InnerText, out result);
		}
		return result;
	}

	private static ValidateResult ValidateInternalMailServerOptions(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SmtpPort", out string value) || !int.TryParse(value, out var result))
		{
			throw new PropertyInvalidException("Invalid number", "SmtpPort");
		}
		if (result < 1 || result > 65535)
		{
			throw new PropertyInvalidException("Out of range", "SmtpPort");
		}
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SmtpSender", out string value2) && !ValidateEmailAddress(value2))
		{
			throw new PropertyInvalidException("", "SmtpSender");
		}
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static ConfigurationItem ConstructMailServerOptions(IGeminiRequestContext requestContext)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "ToolOptionFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("ToolOption", ToolOptionIds.MailServerToolOptionId.ToString());
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "ToolOption",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = ConfigurationApiResources.ToolOptionMailServerDisplayName,
			ParentPath = configurationItemPath.Text,
			Path = configurationItemPath2.Text
		};
		List<Property> list = new List<Property>();
		obj.MethodIds = new string[0];
		string systemSetting = SystemSetting.GetSystemSetting(requestContext, "SmtpSender");
		string systemSetting2 = SystemSetting.GetSystemSetting(requestContext, "SmtpServer");
		string systemSetting3 = SystemSetting.GetSystemSetting(requestContext, "SmtpPort");
		string systemSetting4 = SystemSetting.GetSystemSetting(requestContext, "SmtpUseSsl");
		string systemSetting5 = SystemSetting.GetSystemSetting(requestContext, "SmtpLogOn");
		string systemSetting6 = SystemSetting.GetSystemSetting(requestContext, "SmtpUserName");
		string systemSetting7 = SystemSetting.GetSystemSetting(requestContext, "SmtpPassword");
		list.Add(ToolOptionPropertyDefinitions.GetStringProperty("SmtpSender", systemSetting, ""));
		list.Add(ToolOptionPropertyDefinitions.GetStringProperty("SmtpServer", systemSetting2, ""));
		list.Add(ToolOptionPropertyDefinitions.GetIntProperty("SmtpPort", systemSetting3, ""));
		list.Add(ToolOptionPropertyDefinitions.GetTickProperty("SmtpUseSsl", systemSetting4, "", settable: true));
		list.Add(ToolOptionPropertyDefinitions.GetTickProperty("SmtpLogOn", systemSetting5, "", settable: true));
		list.Add(ToolOptionPropertyDefinitions.GetStringProperty("SmtpUsername", systemSetting6, ""));
		Property stringProperty = ToolOptionPropertyDefinitions.GetStringProperty("SmtpPassword", systemSetting7, "");
		stringProperty.UIImportance = UIImportance.Password;
		list.Add(stringProperty);
		obj.Properties = list.ToArray();
		return obj;
	}

	private static ValidateResult UpdateMailServerOptions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidateInternalMailServerOptions(requestContext, item);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SmtpPort", out string value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SmtpSender", out string value2);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SmtpServer", out string value3);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SmtpUseSsl", out string value4);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SmtpLogOn", out string value5);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SmtpUsername", out string value6);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SmtpPassword", out string value7);
		if (!bool.TryParse(value4, out var _))
		{
			throw new PropertyInvalidException("", "SmtpUseSsl");
		}
		if (!bool.TryParse(value5, out var result2))
		{
			throw new PropertyInvalidException("", "SmtpLogOn");
		}
		if (result2 && (string.IsNullOrEmpty(value6) || string.IsNullOrEmpty(value7)))
		{
			throw new PropertyInvalidException("Username or password missing");
		}
		SystemSetting.SetSystemSetting(requestContext, "SmtpSender", value2);
		SystemSetting.SetSystemSetting(requestContext, "SmtpServer", value3);
		SystemSetting.SetSystemSetting(requestContext, "SmtpPort", value);
		SystemSetting.SetSystemSetting(requestContext, "SmtpUseSsl", value4);
		SystemSetting.SetSystemSetting(requestContext, "SmtpLogOn", value5);
		if (result2)
		{
			SystemSetting.SetSystemSetting(requestContext, "SmtpUserName", value6);
			SystemSetting.SetSystemSetting(requestContext, "SmtpPassword", value7);
		}
		return new ValidateResult(ConstructMailServerOptions(requestContext));
	}

	private static ValidateResult ValidateInternalNetworkOptions(IGeminiRequestContext requestContext, ConfigurationItem item, out LocalIPRangeXmlFormatter formatter, out bool? ipv6)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		Property obj = item.Properties.FirstOrDefault((Property p) => p.Key == "NetworkRangeList") ?? throw new PropertyInvalidException("", "NetworkRangeList");
		ipv6 = null;
		Property property = item.Properties.FirstOrDefault((Property p) => p.Key == "ServerRecorderIPv6Comm");
		if (property != null && bool.TryParse(property.Value, out var result))
		{
			ipv6 = result;
		}
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ServerLanAddress", out string value) && !string.IsNullOrEmpty(value) && !ValidateUrl(value))
		{
			throw new PropertyInvalidException("Invalid IP address", "ServerLanAddress");
		}
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ServerWanAddress", out string value2) && !string.IsNullOrEmpty(value2) && !ValidateUrl(value2))
		{
			throw new PropertyInvalidException("Invalid IP address", "ServerWanAddress");
		}
		item.Properties.FirstOrDefault((Property p) => p.Key == "ServerWanAddress");
		formatter = new LocalIPRangeXmlFormatter();
		string[] valueArray = obj.ValueArray;
		foreach (string text in valueArray)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(';');
				if (array.Length != 2)
				{
					throw new PropertyInvalidException("Invalid range - need to have format like: 'start-ip;end-ip'", "NetworkRangeList");
				}
				if (!ValidateIpRange(array[0], array[1], formatter))
				{
					throw new PropertyInvalidException("Invalid IP address - need to have format like: 'start-ip;end-ip'", "NetworkRangeList");
				}
			}
		}
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static ConfigurationItem ConstructNetworkOptions(IGeminiRequestContext requestContext)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "ToolOptionFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("ToolOption", ToolOptionIds.NetworkToolOptionId.ToString());
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "ToolOption",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = ConfigurationApiResources.ToolOptionNetworkDisplayName,
			ParentPath = configurationItemPath.Text,
			Path = configurationItemPath2.Text
		};
		List<Property> list = new List<Property>();
		obj.MethodIds = new string[0];
		string systemSetting = SystemSetting.GetSystemSetting(requestContext, "LocalIPRanges");
		string systemSetting2 = SystemSetting.GetSystemSetting(requestContext, "ServerRecorderIPv6Comm");
		string systemSetting3 = SystemSetting.GetSystemSetting(requestContext, SupportedSystemSettings.ServerLanAddress);
		string systemSetting4 = SystemSetting.GetSystemSetting(requestContext, SupportedSystemSettings.ServerWanAddress);
		string[] valueArray = new string[0];
		if (!string.IsNullOrEmpty(systemSetting))
		{
			valueArray = ConvertRangeToList(LocalIPRangeXmlFormatter.FromXml(systemSetting));
		}
		list.Add(ToolOptionPropertyDefinitions.GetNetworkListRangeProperty("NetworkRangeList", valueArray, "NetworkLocalRangeTooltip"));
		list.Add(ToolOptionPropertyDefinitions.GetTickProperty("ServerRecorderIPv6Comm", systemSetting2, "", settable: true));
		list.Add(ToolOptionPropertyDefinitions.GetStringProperty("ServerLanAddress", systemSetting3, "ServerLanAddressTooltip"));
		list.Add(ToolOptionPropertyDefinitions.GetStringProperty("ServerWanAddress", systemSetting4, "ServerWanAddressTooltip"));
		obj.Properties = list.ToArray();
		return obj;
	}

	private static ValidateResult UpdateNetworkOptions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidateInternalNetworkOptions(requestContext, item, out var formatter, out var ipv);
		SystemSetting.SetSystemSetting(requestContext, "LocalIPRanges", formatter.ToXml());
		if (ipv.HasValue)
		{
			SystemSetting.SetSystemSetting(requestContext, "ServerRecorderIPv6Comm", ipv.Value.ToString());
		}
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ServerLanAddress", out string value))
		{
			SystemSetting.SetSystemSetting(requestContext, SupportedSystemSettings.ServerLanAddress, value);
		}
		if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ServerWanAddress", out string value2))
		{
			SystemSetting.SetSystemSetting(requestContext, SupportedSystemSettings.ServerWanAddress, value2);
		}
		return new ValidateResult(ConstructNetworkOptions(requestContext));
	}

	private static ValidateResult ValidateInternalAviGeneration(IGeminiRequestContext requestContext, ConfigurationItem item, out Guid selectedGuid)
	{
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Compressor", out string value))
		{
			throw new PropertyInvalidException("", "Compressor");
		}
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(value);
		if (configurationItemPath.ItemType != "Compressor")
		{
			throw new PropertyInvalidException("Invalid path");
		}
		selectedGuid = new Guid(configurationItemPath.Id);
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static ConfigurationItem ConstructAviGenerationOptions(IGeminiRequestContext requestContext)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateSystemPath(), "ToolOptionFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("ToolOption", ToolOptionIds.AviGenerationToolOptionId.ToString());
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "ToolOption",
			UIHint = "DirectionVertical",
			ItemCategory = ItemCategories.Item,
			DisplayName = ConfigurationApiResources.ToolOptionAviGenerationDisplayName,
			ParentPath = configurationItemPath.Text,
			Path = configurationItemPath2.Text
		};
		List<Property> list = new List<Property>();
		obj.MethodIds = new string[0];
		string systemSetting = SystemSetting.GetSystemSetting(requestContext, "AVICodec");
		Compressors[] source = Compressors.QueryCompressors(requestContext);
		Compressors compressors = null;
		long handler;
		if (string.IsNullOrEmpty(systemSetting))
		{
			compressors = source.First();
		}
		else if (systemSetting != null && long.TryParse(systemSetting, out handler))
		{
			compressors = source.FirstOrDefault((Compressors c) => c.Handler == handler);
		}
		if (compressors == null)
		{
			throw new InvalidProgramException("Compressor not found");
		}
		ConfigurationItemPath configurationItemPath3 = ConfigurationItemPath.CreateItemPath("Compressor", compressors.IdCodec.ToString());
		list.Add(ToolOptionPropertyDefinitions.GetAviCodecProperty("Compressor", configurationItemPath3.Text, ""));
		obj.Properties = list.ToArray();
		return obj;
	}

	private static ValidateResult UpdateAviGenerationOptions(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ValidateInternalAviGeneration(requestContext, item, out var idCodec);
		Compressors compressors = Compressors.QueryCompressors(requestContext).FirstOrDefault((Compressors c) => c.IdCodec == idCodec);
		if (compressors == null)
		{
			throw new InvalidProgramException("Unknown compressor");
		}
		SystemSetting.SetSystemSetting(requestContext, "AVICodec", compressors.Handler.ToString(CultureInfo.InvariantCulture));
		return new ValidateResult(ConstructAviGenerationOptions(requestContext));
	}

	private static string[] ConvertRangeToList(LocalIPRangeXmlFormatter formatter)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<int, LocalIPRangeXmlFormatter.IPRange> iPRange in formatter.IPRanges)
		{
			list.Add(iPRange.Value.RangeFrom.ToString() + ";" + iPRange.Value.RangeTo.ToString());
		}
		return list.ToArray();
	}

	private static bool ValidateEmailAddress(string email)
	{
		try
		{
			if (!string.IsNullOrEmpty(email))
			{
				new MailAddress(email);
				return true;
			}
			return false;
		}
		catch (FormatException)
		{
			return false;
		}
	}

	private static bool ValidateIpRange(string ip1, string ip2, LocalIPRangeXmlFormatter formatter)
	{
		if (!IPAddress.TryParse(ip1, out var address))
		{
			return false;
		}
		if (!IPAddress.TryParse(ip2, out var address2))
		{
			return false;
		}
		if (address.AddressFamily != address2.AddressFamily)
		{
			return false;
		}
		if (IPHelper.IPAddressCompare(address, address2) == 1)
		{
			return false;
		}
		if (address.AddressFamily == AddressFamily.InterNetwork || address.AddressFamily == AddressFamily.InterNetworkV6)
		{
			formatter.AddRange(address, address2);
			return true;
		}
		return false;
	}

	private static bool ValidateUrl(string value)
	{
		if (Uri.TryCreate(value, UriKind.Absolute, out var result))
		{
			return !result.IsFile;
		}
		return false;
	}
}
