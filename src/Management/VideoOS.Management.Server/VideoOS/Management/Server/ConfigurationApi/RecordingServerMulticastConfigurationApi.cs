using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Common.DefaultValues;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Network;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIRecordingServerMulticast")]
[EntityName("ConfigAPIRecordingServerMulticastFolder")]
public class RecordingServerMulticastConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 26);
		path.NullCheck("", "GetChildItems", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 27);
		ThrowIfPathInvalid(path);
		string itemType = path.ItemType;
		if (string.Equals(itemType, "RecordingServerMulticastFolder", StringComparison.OrdinalIgnoreCase))
		{
			string id = path.Id;
			RecordingServer recordingServer = RecordingServer.QueryRecordingServer(requestContext, id);
			if (recordingServer == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			return new ConfigurationItem[1] { ConvertToRecordingServerMulticastConfigurationItem(path, recordingServer) };
		}
		if (string.Equals(itemType, "RecordingServerMulticast", StringComparison.OrdinalIgnoreCase))
		{
			return new ConfigurationItem[0];
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		requestContext.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 56);
		path.NullCheck("", "GetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 57);
		ThrowIfPathInvalid(path);
		if (string.Equals(path.ItemType, "RecordingServerMulticastFolder", StringComparison.OrdinalIgnoreCase))
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem("RecordingServerMulticastFolder", parentPath);
		}
		string id = path.Id;
		RecordingServer recordingServer = RecordingServer.QueryRecordingServer(requestContext, id);
		if (recordingServer == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToRecordingServerMulticastConfigurationItem(ConfigurationItemPath.CreateFolderPath("RecordingServer", id, "RecordingServerMulticastFolder"), recordingServer);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		throw new NotSupportedException();
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 90);
		item.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 91);
		path.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 92);
		parentPath.NullCheck("", "SetItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 93);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!string.Equals(path.ItemType, "RecordingServerMulticast", StringComparison.Ordinal))
		{
			throw new NotSupportedException();
		}
		RecordingServer recordingServer = RecordingServer.QueryRecordingServer(requestContext, path.Id);
		if (recordingServer == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		string ipAddressStartString = null;
		string ipAddressEndString = null;
		bool enabled;
		int portStart;
		int portEnd;
		int mtu;
		int ttl;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out enabled, out ipAddressStartString, out ipAddressEndString, out portStart, out portEnd, out mtu, out ttl);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		RecorderMulticastSetting recorderMulticastSetting = new RecorderMulticastSetting(recordingServer.MulticastSettings);
		recorderMulticastSetting.Enabled = enabled;
		recorderMulticastSetting.IPAddressStart = ipAddressStartString;
		recorderMulticastSetting.IPAddressEnd = ipAddressEndString;
		recorderMulticastSetting.PortRangeStart = portStart.ToString(CultureInfo.InvariantCulture);
		recorderMulticastSetting.PortRangeEnd = portEnd.ToString(CultureInfo.InvariantCulture);
		recorderMulticastSetting.MTU = mtu.ToString(CultureInfo.InvariantCulture);
		recorderMulticastSetting.TTL = ttl.ToString(CultureInfo.InvariantCulture);
		recordingServer.MulticastSettings = recorderMulticastSetting.SaveAsXml();
		RecordingServer.UpdateRecordingServerMulticastSettings(requestContext, recordingServer.Id, recordingServer.MulticastSettings);
		validateResult.ResultItem = ConvertToRecordingServerMulticastConfigurationItem(parentPath, recordingServer);
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		requestContext.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 144);
		item.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 145);
		path.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 146);
		parentPath.NullCheck("", "ValidateItem", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\RecordingServerMultiCast\\RecordingServerMulticastConfigurationApi.cs", 147);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (string.Equals(path.ItemType, "RecordingServerMulticast", StringComparison.Ordinal))
		{
			if (RecordingServer.QueryRecordingServer(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string ipAddressStartString = null;
			string ipAddressEndString = null;
			bool enabled;
			int portStart;
			int portEnd;
			int mtu;
			int ttl;
			return ValidateItemInternal(requestContext, item, path, out enabled, out ipAddressStartString, out ipAddressEndString, out portStart, out portEnd, out mtu, out ttl);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out bool enabled, out string ipAddressStartString, out string ipAddressEndString, out int portStart, out int portEnd, out int mtu, out int ttl)
	{
		ipAddressStartString = null;
		ipAddressEndString = null;
		portStart = 0;
		portEnd = 0;
		mtu = 0;
		ttl = 0;
		if (item.Properties == null)
		{
			throw new ArgumentNullException("item.Properties", "No properties provided");
		}
		enabled = item.EnableProperty != null && item.EnableProperty.Enabled;
		Property property = item.Properties.GetProperty("IPAddressStart");
		ipAddressStartString = property?.Value;
		Property property2 = item.Properties.GetProperty("IPAddressEnd");
		ipAddressEndString = property2?.Value;
		Property property3 = item.Properties.GetProperty("PortRangeStart");
		if (!int.TryParse(property3.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out portStart))
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property3, string.Empty);
		}
		Property property4 = item.Properties.GetProperty("PortRangeEnd");
		if (!int.TryParse(property4.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out portEnd))
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property4, string.Empty);
		}
		Property property5 = item.Properties.GetProperty("MTU");
		if (!int.TryParse(property5.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out mtu))
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property5, string.Empty);
		}
		Property property6 = item.Properties.GetProperty("TTL");
		if (!int.TryParse(property6.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out ttl))
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property6, string.Empty);
		}
		if (!ValidateMulticastIP(ipAddressStartString))
		{
			return ErrorHelper.ErrorResultPropertyOutOfRange(property);
		}
		if (!ValidateMulticastIP(ipAddressEndString))
		{
			return ErrorHelper.ErrorResultPropertyOutOfRange(property2);
		}
		IPAddress iPAddress = IPAddress.Parse(ipAddressStartString);
		IPAddress iPAddress2 = IPAddress.Parse(ipAddressEndString);
		if (iPAddress.AddressFamily != iPAddress2.AddressFamily)
		{
			return ErrorHelper.ErrorResultPropertyOutOfRange(property2);
		}
		if (IPHelper.IPAddressCompare(iPAddress, iPAddress2) > 0)
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property2, ConfigurationApiResources.PropertyIPAddressOverlap);
		}
		if (enabled)
		{
			IPEndPointRange range = new IPEndPointRange(iPAddress, iPAddress2);
			if (IsRangeUsed(requestContext, range, path.Id, out var occupyingRecorder))
			{
				return ErrorHelper.ErrorResultPropertyInvalid(property2, string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.PropertyIPAddressOverlapWithAnotherRecorder, occupyingRecorder));
			}
		}
		if (!ValidateIntValue(portStart, 1, 65535))
		{
			return ErrorHelper.ErrorResultPropertyOutOfRange(property3);
		}
		if (!ValidateIntValue(portEnd, 1, 65535))
		{
			return ErrorHelper.ErrorResultPropertyOutOfRange(property4);
		}
		if (portStart > portEnd)
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property4, ConfigurationApiResources.PropertyPortOverlap);
		}
		if (!IPAddress.TryParse(ipAddressStartString, out var address))
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property, ConfigurationApiResources.PropertyInvalidIPAddress);
		}
		if (address.AddressFamily != IPAddress.Parse(ipAddressStartString).AddressFamily)
		{
			return ErrorHelper.ErrorResultPropertyInvalid(property, ConfigurationApiResources.PropertyIPAddressNotSame);
		}
		if (!ValidateIntValue(mtu, 100, 65535))
		{
			return ErrorHelper.ErrorResultPropertyOutOfRange(property5);
		}
		if (!ValidateIntValue(ttl, 0, 255))
		{
			return ErrorHelper.ErrorResultPropertyOutOfRange(property6);
		}
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	private static bool IsRangeUsed(IGeminiRequestContext requestContext, IPEndPointRange range, string recorderId, out string occupyingRecorder)
	{
		occupyingRecorder = string.Empty;
		bool result = false;
		Dictionary<IPEndPointRange, string> dictionary = new Dictionary<IPEndPointRange, string>();
		string[] recordingServerIds = RecordingServer.QueryRecordingServerIds(requestContext);
		RecordingServer[] array = RecordingServer.QueryRecordingServersByIds(requestContext, recordingServerIds);
		foreach (RecordingServer recordingServer in array)
		{
			if (recordingServer.Id == recorderId)
			{
				continue;
			}
			try
			{
				if (!string.IsNullOrEmpty(recordingServer.MulticastSettings))
				{
					RecorderMulticastSetting recorderMulticastSetting = new RecorderMulticastSetting(recordingServer.MulticastSettings);
					if (recorderMulticastSetting.Enabled)
					{
						IPEndPointRange key = new IPEndPointRange(IPAddress.Parse(recorderMulticastSetting.IPAddressStart), IPAddress.Parse(recorderMulticastSetting.IPAddressEnd));
						dictionary.Add(key, recordingServer.Name);
					}
				}
			}
			catch
			{
			}
		}
		foreach (IPEndPointRange key2 in dictionary.Keys)
		{
			if (key2.IsRangeOverlapping(range))
			{
				result = true;
				occupyingRecorder = dictionary[key2];
				break;
			}
		}
		return result;
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

	private static bool ValidateMulticastIP(string text)
	{
		if (IPAddress.TryParse(text, out var address))
		{
			return IsAddressWithinRange(address);
		}
		return false;
	}

	private static bool IsAddressWithinRange(IPAddress address)
	{
		if (address.AddressFamily == AddressFamily.InterNetwork)
		{
			if (IPHelper.IPAddressCompare(address, RecordingServerMulticast.IPv4MulticastAllowedRangeStart) >= 0)
			{
				return IPHelper.IPAddressCompare(address, RecordingServerMulticast.IPv4MulticastAllowedRangeEnd) <= 0;
			}
			return false;
		}
		if (IPHelper.IPAddressCompare(address, RecordingServerMulticast.IPv6MulticastAllowedRangeStart) >= 0)
		{
			return IPHelper.IPAddressCompare(address, RecordingServerMulticast.IPv6MulticastAllowedRangeEnd) <= 0;
		}
		return false;
	}

	private static ConfigurationItem ConvertToRecordingServerMulticastConfigurationItem(ConfigurationItemPath parentPath, RecordingServer recordingServer)
	{
		RecorderMulticastSetting recorderMulticastSetting = new RecorderMulticastSetting(recordingServer.MulticastSettings);
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "RecordingServerMulticast";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeRecordingServerMulticast;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, recordingServer.Id).Text;
		configurationItem.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(recorderMulticastSetting.Enabled, uiFollowsEnabled: true);
		List<Property> list = new List<Property>();
		list.Add(GeneralPropertyDefinitions.GetIdPropertyGuid(recordingServer.Id));
		list.Add(RecordingServerMulticastPropertyDefinition.IPAddressStartProperty(recorderMulticastSetting.IPAddressStart));
		list.Add(RecordingServerMulticastPropertyDefinition.IPAddressEndProperty(recorderMulticastSetting.IPAddressEnd));
		list.Add(RecordingServerMulticastPropertyDefinition.PortRangeStartProperty(recorderMulticastSetting.PortRangeStart));
		list.Add(RecordingServerMulticastPropertyDefinition.PortRangeEndProperty(recorderMulticastSetting.PortRangeEnd));
		list.Add(RecordingServerMulticastPropertyDefinition.MTUProperty(recorderMulticastSetting.MTU));
		list.Add(RecordingServerMulticastPropertyDefinition.TTLProperty(recorderMulticastSetting.TTL));
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
