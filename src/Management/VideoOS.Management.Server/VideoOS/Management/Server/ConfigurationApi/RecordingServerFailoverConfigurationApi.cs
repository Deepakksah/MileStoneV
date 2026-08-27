using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIRecordingServerFailover")]
[EntityName("ConfigAPIRecordingServerFailoverFolder")]
public class RecordingServerFailoverConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 26);
		path.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 27);
		ThrowIfPathInvalid(path);
		string itemType = path.ItemType;
		if (string.Equals(itemType, "RecordingServerFailoverFolder", StringComparison.OrdinalIgnoreCase))
		{
			string id = path.Id;
			RecordingServer recordingServer = RecordingServer.QueryRecordingServer(requestContext, id);
			if (recordingServer == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			FailoverServerInformation failoverInfo = new FailoverServerInformation(requestContext, id);
			return new ConfigurationItem[1] { ConvertToRecordingServerFailoverConfigurationItem(path, recordingServer, failoverInfo) };
		}
		if (string.Equals(itemType, "RecordingServerFailover", StringComparison.OrdinalIgnoreCase))
		{
			return new ConfigurationItem[0];
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 56);
		path.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 57);
		ThrowIfPathInvalid(path);
		string itemType = path.ItemType;
		string id = path.Id;
		RecordingServer recordingServer = RecordingServer.QueryRecordingServer(requestContext, id);
		if (recordingServer == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		if (string.Equals(itemType, "RecordingServerFailoverFolder", StringComparison.OrdinalIgnoreCase))
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem("RecordingServerFailoverFolder", parentPath);
		}
		FailoverServerInformation failoverInfo = new FailoverServerInformation(requestContext, id);
		return ConvertToRecordingServerFailoverConfigurationItem(ConfigurationItemPath.CreateFolderPath("RecordingServer", id, "RecordingServerFailoverFolder"), recordingServer, failoverInfo);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		throw new NotSupportedException();
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 92);
		item.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 93);
		path.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 94);
		parentPath.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 95);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!string.Equals(path.ItemType, "RecordingServerFailover", StringComparison.Ordinal))
		{
			throw new NotSupportedException();
		}
		RecordingServer recordingServer = RecordingServer.QueryRecordingServer(requestContext, path.Id);
		if (recordingServer == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		FailoverServerInformation failoverServerInformation = new FailoverServerInformation(requestContext, path.Id);
		string primaryGroupId;
		string secondaryGroupId;
		string hotStandbyId;
		int failoverPort;
		ValidateResult validateResult = ValidateItemInternal(item, failoverServerInformation, out primaryGroupId, out secondaryGroupId, out hotStandbyId, out failoverPort);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		recordingServer.PrimaryFailoverGroup = primaryGroupId;
		recordingServer.SecondaryFailoverGroup = secondaryGroupId;
		recordingServer.FailoverPort = failoverPort;
		RecordingServer.UpdateRecordingServerFailoverSettings(requestContext, path.Id, primaryGroupId, secondaryGroupId, failoverPort);
		if (!string.IsNullOrEmpty(hotStandbyId))
		{
			if (failoverServerInformation.HotStandbyForCurrent == null || !string.Equals(hotStandbyId, failoverServerInformation.HotStandbyForCurrent.Id, StringComparison.OrdinalIgnoreCase))
			{
				if (failoverServerInformation.HotStandbyForCurrent != null)
				{
					failoverServerInformation.HotStandbyForCurrent.HotStandbyRecorderId = Guid.Empty.ToString();
					FailoverServer.UpdateFailoverServer(requestContext, failoverServerInformation.HotStandbyForCurrent);
				}
				FailoverServer failoverServer = failoverServerInformation.UnusedFailoverServers.First((FailoverServer s) => s.Id == hotStandbyId);
				failoverServer.HotStandbyRecorderId = path.Id;
				FailoverServer.UpdateFailoverServer(requestContext, failoverServer);
				failoverServerInformation.HotStandbyForCurrent = failoverServer;
			}
		}
		else if (failoverServerInformation.HotStandbyForCurrent != null && failoverServerInformation.HotStandbyForCurrent.HotStandbyRecorderId != Guid.Empty.ToString())
		{
			failoverServerInformation.HotStandbyForCurrent.HotStandbyRecorderId = Guid.Empty.ToString();
			FailoverServer.UpdateFailoverServer(requestContext, failoverServerInformation.HotStandbyForCurrent);
			failoverServerInformation.HotStandbyForCurrent = null;
		}
		validateResult.ResultItem = ConvertToRecordingServerFailoverConfigurationItem(parentPath, recordingServer, failoverServerInformation);
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 155);
		item.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 156);
		path.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 157);
		parentPath.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerFailover\\RecordingServerFailoverConfigurationApi.cs", 158);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (string.Equals(path.ItemType, "RecordingServerFailover", StringComparison.Ordinal))
		{
			if (RecordingServer.QueryRecordingServer(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			FailoverServerInformation info = new FailoverServerInformation(requestContext, path.Id);
			string primaryGroupId;
			string secondaryGroupId;
			string hotStandbyId;
			int failoverPort;
			return ValidateItemInternal(item, info, out primaryGroupId, out secondaryGroupId, out hotStandbyId, out failoverPort);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(ConfigurationItem item, FailoverServerInformation info, out string primaryGroupId, out string secondaryGroupId, out string hotStandbyId, out int failoverPort)
	{
		primaryGroupId = null;
		secondaryGroupId = null;
		hotStandbyId = null;
		failoverPort = 0;
		Property property = item.Properties.GetProperty("PrimaryFailoverGroup");
		Property property2 = item.Properties.GetProperty("SecondaryFailoverGroup");
		Property property3 = item.Properties.GetProperty("HotStandby");
		Property property4 = item.Properties.GetProperty("FailoverPort");
		if (property == null)
		{
			primaryGroupId = "";
		}
		else
		{
			ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(property.Value);
			if (configurationItemPath.ItemType != "FailoverGroup" || !info.GroupExist(configurationItemPath.Id))
			{
				return ErrorHelper.ErrorResultPropertyInvalid(property, string.Empty);
			}
			primaryGroupId = configurationItemPath.Id;
		}
		if (property2 == null)
		{
			secondaryGroupId = "";
		}
		else
		{
			ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreatePath(property2.Value);
			if (configurationItemPath2.ItemType != "FailoverGroup" || !info.GroupExist(configurationItemPath2.Id))
			{
				return ErrorHelper.ErrorResultPropertyInvalid(property2, string.Empty);
			}
			secondaryGroupId = configurationItemPath2.Id;
		}
		if (property3 == null)
		{
			hotStandbyId = "";
		}
		else if (string.IsNullOrEmpty(property3.Value))
		{
			hotStandbyId = "";
		}
		else
		{
			ConfigurationItemPath configurationItemPath3 = ConfigurationItemPath.CreatePath(property3.Value);
			if (configurationItemPath3.ItemType != "FailoverRecorder" || !info.UnusedServerExist(configurationItemPath3.Id))
			{
				return ErrorHelper.ErrorResultPropertyInvalid(property3, string.Empty);
			}
			hotStandbyId = configurationItemPath3.Id;
		}
		if (property4 == null)
		{
			return ErrorHelper.ErrorResultPropertyMissing("FailoverPort");
		}
		if (!int.TryParse(property4.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out failoverPort))
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property4, string.Empty);
		}
		if (!ValidateIntValue(failoverPort, 1, 65535))
		{
			return ErrorHelper.ErrorResultPropertyOutOfRange(property4);
		}
		bool flag = primaryGroupId != Guid.Empty.ToString();
		bool flag2 = secondaryGroupId != Guid.Empty.ToString();
		bool flag3 = !string.IsNullOrEmpty(hotStandbyId);
		if (!flag && flag2)
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property2, ConfigurationApiResources.PropertyErrorFailoverGroupSecondaryOnly);
		}
		if (flag && flag3)
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property3, ConfigurationApiResources.PropertyErrorFailoverGroupAndHotStandby);
		}
		if (flag && primaryGroupId == secondaryGroupId)
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property2, ConfigurationApiResources.PropertyErrorFailoverSameGroupTwice);
		}
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static bool ValidateIntValue(int? intValue, int minValue, int maxValue)
	{
		bool result = true;
		if (intValue.HasValue)
		{
			int value = intValue.Value;
			if (value < minValue || value > maxValue)
			{
				result = false;
			}
		}
		else
		{
			result = false;
		}
		return result;
	}

	private static ConfigurationItem ConvertToRecordingServerFailoverConfigurationItem(ConfigurationItemPath parentPath, RecordingServer recordingServer, FailoverServerInformation failoverInfo)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "RecordingServerFailover";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeRecordingServerFailover;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, recordingServer.Id).Text;
		configurationItem.EnableProperty = null;
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(recordingServer.Id));
		list.Add(RecordingServerFailoverPropertyDefinition.PrimaryFailoverGroupProperty(recordingServer.PrimaryFailoverGroup, failoverInfo));
		list.Add(RecordingServerFailoverPropertyDefinition.SecondaryFailoverGroupProperty(recordingServer.SecondaryFailoverGroup, failoverInfo));
		list.Add(RecordingServerFailoverPropertyDefinition.HotStandbyProperty(failoverInfo));
		list.Add(RecordingServerFailoverPropertyDefinition.FailoverPortProperty(recordingServer.FailoverPort));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[0];
		return configurationItem;
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "RecordingServer", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Invalid id. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}
}
