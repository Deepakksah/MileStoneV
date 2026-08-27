using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIGenericEventDataSource")]
[EntityName("ConfigAPIGenericEventDataSourceFolder")]
public class GenericEventDataSourceConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public static string DataSourcePortKey = "DataSourcePort";

	public static string DataSourceSeparatorKey = "DataSourceSeparator";

	public static string DataSourceEncodingKey = "DataSourceEncoding";

	public static string DataSourceLogKey = "DataSourceLog";

	public static string DataSourceEchoKey = "DataSourceEcho";

	public static string DataSourceAllowedKey = "DataSourceAllowed";

	public static string DataSourceAllowed6Key = "DataSourceAllowed6";

	public static string DataSourceProtocolKey = "DataSourceProtocol";

	public static string DataSourceAddressFamilyKey = "DataSourceAddressFamily";

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		if (item.ItemType == "GenericEventDataSource")
		{
			Guid guid = new Guid(path.Id);
			ValidateResult validateResult = ValidateDataSourceItemInternal(requestContext, item, guid, out var name, out var protocol, out var family, out var port, out var separator, out var encoding, out var log, out var echo, out var allowed, out var allowed2, out var enabled);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			GenericEventDataSource dataSource = new GenericEventDataSource(name, guid, protocol, family, port, separator, encoding, log, echo, allowed, allowed2, enabled);
			GenericEventDataSource.SaveDataSource(requestContext, dataSource);
			validateResult = new ValidateResult();
			validateResult.ResultItem = ConvertToGenericEventDataSourceConfigurationItem(parentPath, GenericEventDataSource.QueryDataSource(requestContext, guid));
			validateResult.ValidatedOk = true;
			return validateResult;
		}
		throw new ArgumentException("Unknown ItemType", "item");
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder && item.ItemType == "GenericEventDataSource")
		{
			Guid guid = new Guid(path.Id);
			if (GenericEventDataSource.QueryDataSource(requestContext, guid) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string name;
			string protocol;
			string family;
			int port;
			byte[] separator;
			int encoding;
			bool log;
			string echo;
			string allowed;
			string allowed2;
			bool enabled;
			return ValidateDataSourceItemInternal(requestContext, item, guid, out name, out protocol, out family, out port, out separator, out encoding, out log, out echo, out allowed, out allowed2, out enabled);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateDataSourceItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, Guid guid, out string name, out string protocol, out string family, out int port, out byte[] separator, out int encoding, out bool log, out string echo, out string allowed, out string allowed6, out bool enabled)
	{
		name = null;
		protocol = "Both";
		family = "Both";
		port = 1240;
		separator = new byte[0];
		encoding = 1252;
		log = false;
		echo = "Full";
		allowed = "";
		allowed6 = "";
		enabled = true;
		try
		{
			if (item.ItemType == "GenericEventDataSource" || item.ItemType == "InvokeInfo")
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				}
				protocol = GetStringValue(item.Properties, DataSourceProtocolKey, "Both");
				family = GetStringValue(item.Properties, DataSourceAddressFamilyKey, "Both");
				port = GetIntValue(item.Properties, DataSourcePortKey, 1240);
				separator = GetSeparator(item.Properties, DataSourceSeparatorKey);
				encoding = GetIntValue(item.Properties, DataSourceEncodingKey, 1252);
				log = GetBoolValue(item.Properties, DataSourceLogKey, defaultValue: false);
				echo = GetStringValue(item.Properties, DataSourceEchoKey, "Full");
				allowed = GetStringValue(item.Properties, DataSourceAllowedKey, "");
				allowed6 = GetStringValue(item.Properties, DataSourceAllowed6Key, "");
				enabled = item.EnableProperty.Enabled;
				ThrowIfDuplicateDataSourceName(requestContext, name, guid);
				ValidateIPAddress(allowed, ipv4: true);
				ValidateIPAddress(allowed6, ipv4: false);
				ValidateResult validateResult = new ValidateResult();
				validateResult.ValidatedOk = true;
				validateResult.ResultItem = item;
				return validateResult;
			}
			throw new NotSupportedException();
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
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			foreach (GenericEventDataSource item in GenericEventDataSource.QueryDataSources(requestContext))
			{
				list.Add(ConvertToGenericEventDataSourceConfigurationItem(path, item));
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
			return GroupConfigurationItemFactory.CreateFolderItem("GenericEventDataSourceFolder", ConfigurationItemPath.CreateSystemPath());
		}
		if (path.ItemType == "GenericEventDataSource")
		{
			GenericEventDataSource genericEventDataSource = GenericEventDataSource.QueryDataSource(requestContext, new Guid(path.Id));
			if (genericEventDataSource == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			return ConvertToGenericEventDataSourceConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "GenericEventDataSourceFolder").CreatePath(), genericEventDataSource);
		}
		throw new PathNotFoundException();
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (!(method == "AddGenericEventDataSource"))
		{
			if (method == "RemoveGenericEventDataSource")
			{
				if (!flag)
				{
					return CreateInvokeInfoRemoveGenericEventDataSource(item, GetChildItems(requestContext, path), method);
				}
				return RemoveGenericEventDataSource(requestContext, item, path);
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
		if (!flag)
		{
			return CreateInvokeInfoAddGenericEventDataSource(item, method);
		}
		return AddGenericEventDataSource(requestContext, item, path);
	}

	private static ConfigurationItem CreateInvokeInfoAddGenericEventDataSource(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddGenericEventDataSource;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(enabled: true, uiFollowsEnabled: true);
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[10]
		{
			GeneralPropertyDefinitions.GetNameProperty(""),
			GenericEventDataSourceProperties.PortProperty(1240),
			GenericEventDataSourceProperties.SeparatorProperty(new byte[0]),
			GenericEventDataSourceProperties.EncodingProperty(1252),
			GenericEventDataSourceProperties.LogProperty(val: false),
			GenericEventDataSourceProperties.EchoProperty("None"),
			GenericEventDataSourceProperties.AllowedProperty(""),
			GenericEventDataSourceProperties.Allowed6Property(""),
			GenericEventDataSourceProperties.ProtocolProperty("Both"),
			GenericEventDataSourceProperties.AddressFamilyProperty("Both")
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveGenericEventDataSource(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveGenericEventDataSource, "PropertyRemoveGenericEventDataSource", ConfigurationApiResources.PropertyRemoveGenericEventDataSource, item, members);
	}

	private static ConfigurationItem AddGenericEventDataSource(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		Guid guid = Guid.NewGuid();
		string name;
		string protocol;
		string family;
		int port;
		byte[] separator;
		int encoding;
		bool log;
		string echo;
		string allowed;
		string allowed2;
		bool enabled;
		ValidateResult validateResult = ValidateDataSourceItemInternal(requestContext, item, guid, out name, out protocol, out family, out port, out separator, out encoding, out log, out echo, out allowed, out allowed2, out enabled);
		if (!validateResult.ValidatedOk)
		{
			string message = "Unable to validate";
			if (validateResult.ErrorResults != null && validateResult.ErrorResults.Length != 0)
			{
				message = validateResult.ErrorResults[0].ErrorText;
			}
			throw new ArgumentException(message, "item");
		}
		GenericEventDataSource dataSource = new GenericEventDataSource(name, guid, protocol, family, port, separator, encoding, log, echo, allowed, allowed2, enabled);
		GenericEventDataSource.SaveDataSource(requestContext, dataSource);
		ConfigurationItem configurationItem = ConvertToGenericEventDataSourceConfigurationItem(path, dataSource);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static ConfigurationItem RemoveGenericEventDataSource(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			GenericEventDataSource.DeleteDataSource(requestContext, new Guid(invokeMemberItemSelectionPath.Id));
		}
		catch (RemoveNonExistingMIPItemException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static void ThrowIfDuplicateDataSourceName(IGeminiRequestContext requestContext, string name, Guid objectId)
	{
		Validation.CheckName(name, "Name", allowNull: false);
		foreach (GenericEventDataSource item in GenericEventDataSource.QueryDataSources(requestContext))
		{
			if (item.Id != objectId && item.Name == name)
			{
				throw new ArgumentException("Duplicate name");
			}
		}
	}

	private static ConfigurationItem ConvertToGenericEventDataSourceConfigurationItem(ConfigurationItemPath parentPath, GenericEventDataSource dataSource)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "GenericEventDataSource",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = dataSource.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, dataSource.Id.ToString()).Text;
		obj.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(dataSource.Enabled, uiFollowsEnabled: true);
		obj.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(dataSource.Id.ToString()),
			GeneralPropertyDefinitions.GetNameProperty(dataSource.Name, isSettable: true),
			GenericEventDataSourceProperties.PortProperty(dataSource.Port),
			GenericEventDataSourceProperties.SeparatorProperty(dataSource.Separator),
			GenericEventDataSourceProperties.EncodingProperty(dataSource.Encoding),
			GenericEventDataSourceProperties.LogProperty(dataSource.Log),
			GenericEventDataSourceProperties.EchoProperty(dataSource.Echo),
			GenericEventDataSourceProperties.AllowedProperty(dataSource.Allowed),
			GenericEventDataSourceProperties.Allowed6Property(dataSource.Allowed6),
			GenericEventDataSourceProperties.ProtocolProperty(dataSource.Protocol),
			GenericEventDataSourceProperties.AddressFamilyProperty(dataSource.AddressFamily)
		}.ToArray();
		List<string> list = new List<string>();
		obj.MethodIds = list.ToArray();
		return obj;
	}

	private static string GetStringValue(Property[] properties, string key, string defaultValue)
	{
		Property property = properties.FirstOrDefault((Property p) => p.Key == key);
		if (property != null)
		{
			return property.Value;
		}
		return defaultValue;
	}

	private static bool GetBoolValue(Property[] properties, string key, bool defaultValue)
	{
		Property property = properties.FirstOrDefault((Property p) => p.Key == key);
		bool result = defaultValue;
		if (property != null)
		{
			bool.TryParse(property.Value, out result);
			return result;
		}
		return result;
	}

	private static int GetIntValue(Property[] properties, string key, int defaultValue)
	{
		Property property = properties.FirstOrDefault((Property p) => p.Key == key);
		int result = defaultValue;
		if (property != null && int.TryParse(property.Value, out result) && property.ValueTypeInfos != null)
		{
			if (property.ValueType == "Int")
			{
				ValueTypeInfo[] valueTypeInfos = property.ValueTypeInfos;
				foreach (ValueTypeInfo obj in valueTypeInfos)
				{
					int num2 = int.Parse(obj.Value, CultureInfo.InvariantCulture);
					if (obj.Name == "MinValue" && result < num2)
					{
						throw new ArgumentException("Value too small in " + key);
					}
					if (obj.Name == "MaxValue" && result > num2)
					{
						throw new ArgumentException("Value too big in " + key);
					}
				}
			}
			if (property.ValueType == "Enum")
			{
				bool flag = false;
				ValueTypeInfo[] valueTypeInfos = property.ValueTypeInfos;
				for (int num = 0; num < valueTypeInfos.Length; num++)
				{
					if (valueTypeInfos[num].Value == property.Value)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					throw new ArgumentException("Value not in valid list " + key);
				}
			}
		}
		return result;
	}

	private static byte[] GetSeparator(Property[] properties, string key)
	{
		Property property = properties.FirstOrDefault((Property p) => p.Key == key);
		if (property != null && !string.IsNullOrEmpty(property.Value))
		{
			string[] array = property.Value.Split(',');
			byte[] array2 = new byte[array.Length];
			int num = 0;
			byte result = 0;
			string[] array3 = array;
			for (int num2 = 0; num2 < array3.Length; num2++)
			{
				if (byte.TryParse(array3[num2], out result))
				{
					array2[num] = result;
				}
				else
				{
					array2[num] = 0;
				}
				num++;
			}
			return array2;
		}
		return new byte[0];
	}

	private static void ValidateIPAddress(string list, bool ipv4)
	{
		if (string.IsNullOrEmpty(list))
		{
			return;
		}
		string[] array = list.Split(';');
		for (int i = 0; i < array.Length; i++)
		{
			if (!IPAddress.TryParse(array[i], out var address))
			{
				throw new ArgumentException("Invalid Address format");
			}
			if (ipv4)
			{
				if (address.AddressFamily != AddressFamily.InterNetwork)
				{
					throw new ArgumentException("Invalid IPv4 Address");
				}
			}
			else if (address.AddressFamily != AddressFamily.InterNetworkV6)
			{
				throw new ArgumentException("Invalid IPv6 Address");
			}
		}
	}
}
