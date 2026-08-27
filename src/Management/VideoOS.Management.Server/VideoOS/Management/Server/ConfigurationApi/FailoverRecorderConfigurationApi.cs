using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIFailoverRecorder")]
[EntityName("ConfigAPIFailoverRecorderFolder")]
public class FailoverRecorderConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		string publicWebserverHostName = null;
		int publicWebserverPort = 0;
		string multicastServerAddress = null;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath, out var name, out var description, out var publicAccessEnabled, out publicWebserverHostName, out publicWebserverPort, out var udpPort, out multicastServerAddress, out var databasePath);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		FailoverServer failoverServer = FailoverServer.QueryFailoverServer(requestContext, path.Id);
		if (failoverServer == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		failoverServer.Enabled = item.EnableProperty.Enabled;
		failoverServer.Name = name;
		failoverServer.Description = description;
		failoverServer.PublicAccessEnabled = publicAccessEnabled;
		failoverServer.PublicWebServerHostName = publicWebserverHostName;
		failoverServer.PublicWebServerPort = publicWebserverPort;
		failoverServer.UdpPort = udpPort;
		failoverServer.MulticastServerAddress = multicastServerAddress;
		failoverServer.DatabasePath = databasePath;
		FailoverServer.UpdateFailoverServer(requestContext, failoverServer);
		ConfigurationItem resultItem = ConvertToFailoverRecorderConfigurationItem(parentPath, failoverServer);
		validateResult = new ValidateResult();
		validateResult.ValidatedOk = true;
		validateResult.ResultItem = resultItem;
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		if (FailoverServer.QueryFailoverServer(requestContext, path.Id) == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		string publicWebserverHostName = null;
		int publicWebserverPort = 0;
		string multicastServerAddress = null;
		string name;
		string description;
		bool publicAccessEnabled;
		int udpPort;
		string databasePath;
		return ValidateItemInternal(requestContext, item, path, parentPath, out name, out description, out publicAccessEnabled, out publicWebserverHostName, out publicWebserverPort, out udpPort, out multicastServerAddress, out databasePath);
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, out string name, out string description, out bool publicAccessEnabled, out string publicWebserverHostName, out int publicWebserverPort, out int udpPort, out string multicastServerAddress, out string databasePath)
	{
		name = null;
		description = null;
		publicAccessEnabled = false;
		publicWebserverHostName = null;
		publicWebserverPort = 0;
		multicastServerAddress = null;
		udpPort = 0;
		databasePath = "";
		string value = null;
		string value2 = null;
		string value3 = null;
		try
		{
			if (item.Properties == null)
			{
				throw new ArgumentException("Properties are missing");
			}
			if (item.EnableProperty == null)
			{
				throw new ArgumentException("EnableProperty is null");
			}
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PublicAccessEnabled", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PublicWebserverHostName", out publicWebserverHostName);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("PublicWebserverPort", out value3);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("MulticastServerAddress", out multicastServerAddress);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("UdpPort", out value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DatabasePath", out databasePath);
			publicAccessEnabled = Validation.CheckBooleanStringValue(value, "publicAccessEnabled", allowNull: false).Value;
			if (!int.TryParse(value2, out udpPort))
			{
				udpPort = 0;
			}
			if (!int.TryParse(value3, out publicWebserverPort))
			{
				publicWebserverPort = 0;
			}
			FailoverServer.UpdateFailoverServerValidation(requestContext, path.Id, name, description, publicWebserverHostName, publicWebserverPort, udpPort, multicastServerAddress, databasePath);
			ThrowIfDuplicateName(requestContext, name, path.Id, parentPath.Id);
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			return validateResult;
		}
		catch (ArgumentException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			string serverGroupId = (string.Equals(path.ParentItemType, "System", StringComparison.Ordinal) ? "6A85FFC3-C134-4338-A072-024817CB5820" : path.Id);
			FailoverServer[] array = FailoverServer.QueryFailoverServers(requestContext, serverGroupId);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			FailoverServer[] array2 = array;
			foreach (FailoverServer failoverRecorder in array2)
			{
				list.Add(ConvertToFailoverRecorderConfigurationItem(path, failoverRecorder));
			}
			return list.ToArray();
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			if (string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				return GroupConfigurationItemFactory.CreateFolderItem("FailoverRecorderFolder", ConfigurationItemPath.CreateSystemPath());
			}
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, parentPath);
		}
		FailoverServer failoverServer = FailoverServer.QueryFailoverServer(requestContext, path.Id);
		if (failoverServer == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToFailoverRecorderConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "FailoverRecorderFolder").CreatePath(), failoverServer);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (!path.IsFolder && !path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static void ThrowIfDuplicateName(IGeminiRequestContext requestContext, string name, string objectId, string parentId)
	{
		Validation.CheckName(name, "Name", allowNull: false);
		if (string.IsNullOrEmpty(parentId))
		{
			parentId = "6A85FFC3-C134-4338-A072-024817CB5820";
		}
		FailoverServer[] array = FailoverServer.QueryFailoverServers(requestContext, parentId);
		foreach (FailoverServer failoverServer in array)
		{
			if (!string.Equals(failoverServer.Id, objectId, StringComparison.OrdinalIgnoreCase) && failoverServer.Name == name)
			{
				throw new ArgumentException("Duplicate name");
			}
		}
	}

	public static ConfigurationItem ConvertToFailoverRecorderConfigurationItem(ConfigurationItemPath parentPath, FailoverServer failoverRecorder)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "FailoverRecorder",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = failoverRecorder.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, failoverRecorder.Id).Text;
		obj.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(failoverRecorder.Enabled, uiFollowsEnabled: false);
		obj.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(failoverRecorder.Id),
			GeneralPropertyDefinitions.GetNameProperty(failoverRecorder.Name, isSettable: true),
			GeneralPropertyDefinitions.GetDescriptionProperty(failoverRecorder.Description, isSettable: true),
			RecordingServerPropertyDefinition.HostNameProperty(failoverRecorder.WebServerHostName),
			RecordingServerPropertyDefinition.PortNumberProperty(failoverRecorder.WebServerPort),
			RecordingServerPropertyDefinition.WebServerUriProperty(failoverRecorder.WebServerUri),
			RecordingServerPropertyDefinition.ActiveWebServerUriProperty(failoverRecorder.WebServerUriConfiguration),
			RecordingServerPropertyDefinition.PublicAccessEnabledProperty(failoverRecorder.PublicAccessEnabled),
			RecordingServerPropertyDefinition.PublicWebServerHostnameProperty(failoverRecorder.PublicWebServerHostName),
			RecordingServerPropertyDefinition.PublicWebServerPortProperty(failoverRecorder.PublicWebServerPort),
			RecordingServerPropertyDefinition.MulticastServerAddressProperty(failoverRecorder.MulticastServerAddress),
			FailoverRecorderPropertyDefinitions.UdpPortNumberProperty(failoverRecorder.UdpPort),
			FailoverRecorderPropertyDefinitions.GroupPositionProperty(failoverRecorder.Position, isSettable: false),
			FailoverRecorderPropertyDefinitions.DatabasePathProperty(failoverRecorder.DatabasePath),
			GeneralPropertyDefinitions.GetCreatedDateProperty(failoverRecorder.CreatedDate),
			GeneralPropertyDefinitions.GetLastModifiedProperty(failoverRecorder.LastModified)
		}.ToArray();
		List<string> list = new List<string>();
		obj.MethodIds = list.ToArray();
		obj.Children = null;
		obj.ChildrenFilled = true;
		return obj;
	}
}
