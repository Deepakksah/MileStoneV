#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIGenericEvent")]
[EntityName("ConfigAPIGenericEventFolder")]
public class GenericEventConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public static Guid GenericPluginId = new Guid("ff9de88e-45f2-4b1e-a551-30ad84e25102");

	public static Guid GenericKind = new Guid("d4d19c01-03f2-4ac6-9d2b-5356b5de62f1");

	private Guid _serverId = Guid.Empty;

	public static string ShadowIdKey = "ShadowId";

	public static string DataSourceKey = "DataSource";

	public static string ExpressionKey = "Expression";

	public static string ExpressionTypeKey = "ExpressionType";

	public static string RegularExpressionKey = "RegularExpression";

	public static string PriorityKey = "Priority";

	private Guid ManagementServerId(IGeminiRequestContext requestContext)
	{
		if (_serverId == Guid.Empty && !Guid.TryParse(ServerInformation.GetServerInformation(requestContext).Id, out _serverId))
		{
			Trace.WriteLine("GenericEventConfiguration: No Mgt Server Id ?");
		}
		return _serverId;
	}

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		if (item.ItemType == "GenericEvent")
		{
			ValidateResult validateResult = ValidateItemInternal(requestContext, item, new Guid(path.Id), out var enabled, out var name, out var dataSourceId, out var expressionType, out var expression, out var priority, out var _);
			if (!validateResult.ValidatedOk)
			{
				return validateResult;
			}
			Item item2 = MIPItem.GetItem(requestContext, new Guid(path.Id));
			item2.Enabled = enabled;
			item2.Name = name;
			item2.Properties[DataSourceKey] = dataSourceId.ToString();
			item2.Properties[ExpressionTypeKey] = expressionType.ToString(CultureInfo.InvariantCulture);
			item2.Properties[ExpressionKey] = expression;
			item2.Properties[PriorityKey] = priority.ToString(CultureInfo.InvariantCulture);
			string g = item2.Properties[ShadowIdKey];
			UpdateShadow(requestContext, new Guid(g), name);
			MIPItem.SaveItem(requestContext, item2);
			ConfigurationItem resultItem = ConvertToGenericEventConfigurationItem(requestContext, parentPath, item2);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem;
			return validateResult;
		}
		throw new ArgumentException("Unknown ItemType", "item");
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder && item.ItemType == "GenericEvent")
		{
			if (MIPItem.GetItem(requestContext, new Guid(path.Id)) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			bool enabled;
			string name;
			Guid dataSourceId;
			int expressionType;
			string expression;
			int priority;
			string regularExpression;
			return ValidateItemInternal(requestContext, item, new Guid(path.Id), out enabled, out name, out dataSourceId, out expressionType, out expression, out priority, out regularExpression);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, Guid id, out bool enabled, out string name, out Guid dataSourceId, out int expressionType, out string expression, out int priority, out string regularExpression)
	{
		enabled = false;
		name = null;
		dataSourceId = Guid.Empty;
		expressionType = 0;
		expression = "";
		priority = 99;
		regularExpression = "";
		try
		{
			if (item.ItemType == "GenericEvent" || item.ItemType == "InvokeInfo")
			{
				enabled = item.EnableProperty.Enabled;
				if (item.Properties != null)
				{
					string stringValue = GetStringValue(item.Properties, DataSourceKey, "");
					expression = GetStringValue(item.Properties, ExpressionKey, "");
					expressionType = GetIntValue(item.Properties, ExpressionTypeKey, 0);
					priority = GetIntValue(item.Properties, PriorityKey, 1);
					ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(stringValue);
					if (configurationItemPath.ItemType != "GenericEventDataSource")
					{
						throw new ArgumentException("DataSourceId not correct", "dataSourceId");
					}
					if (!Guid.TryParse(configurationItemPath.Id, out dataSourceId))
					{
						throw new ArgumentException("DataSourceId not found", "dataSourceId");
					}
					if (GenericEventDataSource.QueryDataSource(requestContext, dataSourceId) == null)
					{
						throw new ArgumentException("Data source not found");
					}
					name = GetStringValue(item.Properties, "Name", "");
					regularExpression = ConvertToRegularExpression(expressionType, expression);
				}
				ThrowIfDuplicateName(requestContext, name, id);
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
			List<Item> items = MIPItem.GetItems(requestContext, GenericKind);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			foreach (Item item in items)
			{
				list.Add(ConvertToGenericEventConfigurationItem(requestContext, path, item));
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
			return GroupConfigurationItemFactory.CreateFolderItem("GenericEventFolder", ConfigurationItemPath.CreateSystemPath());
		}
		if (path.ItemType == "GenericEvent")
		{
			Item item = MIPItem.GetItem(requestContext, new Guid(path.Id));
			if (item == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			ConfigurationItemPath parentPath = new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "GenericEventFolder").CreatePath();
			return ConvertToGenericEventConfigurationItem(requestContext, parentPath, item);
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
		if (!(method == "AddGenericEvent"))
		{
			if (method == "RemoveGenericEvent")
			{
				if (!flag)
				{
					return CreateInvokeInfoRemoveGenericEvent(item, GetChildItems(requestContext, path), method);
				}
				return RemoveGenericEvent(requestContext, item, path);
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
		if (!flag)
		{
			return CreateInvokeInfoAddGenericEvent(requestContext, item, method);
		}
		return AddGenericEvent(requestContext, item, path);
	}

	private static ConfigurationItem CreateInvokeInfoAddGenericEvent(IGeminiRequestContext requestContext, ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddGenericEvent;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(enabled: true, uiFollowsEnabled: true);
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[5]
		{
			GeneralPropertyDefinitions.GetNameProperty(""),
			GenericEventProperties.DataSourceProperty("", isSettable: true, GenericEventDataSource.QueryDataSources(requestContext)),
			GenericEventProperties.ExpressionProperty("", isSettable: true),
			GenericEventProperties.ExpressionTypeProperty("0", isSettable: true),
			GenericEventProperties.PriorityProperty("1", isSettable: true)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveGenericEvent(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveGenericEvent, "PropertyRemoveGenericEvent", ConfigurationApiResources.PropertyRemoveGenericEvent, item, members);
	}

	private ConfigurationItem AddGenericEvent(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		bool enabled;
		string name;
		Guid dataSourceId;
		int expressionType;
		string expression;
		int priority;
		string regularExpression;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, Guid.NewGuid(), out enabled, out name, out dataSourceId, out expressionType, out expression, out priority, out regularExpression);
		if (!validateResult.ValidatedOk)
		{
			string message = "Incorrect properties";
			if (validateResult.ErrorResults != null && validateResult.ErrorResults.Length != 0)
			{
				message = validateResult.ErrorResults[0].ErrorText;
			}
			throw new PropertyInvalidException(message, "item");
		}
		try
		{
			ThrowIfDuplicateName(requestContext, name, Guid.Empty);
		}
		catch (ArgumentException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
		try
		{
			Item item2 = new Item(new FQID(new ServerId("XPCO", "localhost", 80, ManagementServerId(requestContext)), Guid.Empty, Guid.NewGuid(), FolderType.No, GenericKind), name);
			item2.Enabled = enabled;
			item2.Properties[DataSourceKey] = dataSourceId.ToString();
			item2.Properties[ExpressionKey] = expression;
			item2.Properties[ExpressionTypeKey] = expressionType.ToString(CultureInfo.InvariantCulture);
			item2.Properties[RegularExpressionKey] = regularExpression;
			item2.Properties[PriorityKey] = priority.ToString(CultureInfo.InvariantCulture);
			Guid guid = CreateShadow(requestContext, name);
			item2.Properties[ShadowIdKey] = guid.ToString();
			MIPItem.SaveItem(requestContext, item2);
			ConfigurationItem configurationItem = ConvertToGenericEventConfigurationItem(requestContext, path, item2);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (Exception ex2)
		{
			throw new PropertyInvalidException(ex2.Message);
		}
	}

	private ConfigurationItem RemoveGenericEvent(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			Guid objectId = new Guid(invokeMemberItemSelectionPath.Id);
			Item item2 = MIPItem.GetItem(requestContext, objectId);
			if (item2 == null)
			{
				throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
			}
			if (item2.Properties.ContainsKey(ShadowIdKey))
			{
				DeleteShadow(requestContext, item2.Properties[ShadowIdKey]);
			}
			MIPItem.Delete(requestContext, item2);
		}
		catch (RemoveNonExistingMIPItemException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static void DeleteShadow(IGeminiRequestContext requestContext, string shadowId)
	{
		try
		{
			UserDefinedEvent.DeleteUserDefinedEvent(requestContext, shadowId);
		}
		catch (UserDefinedEventNotFoundException)
		{
		}
	}

	private static void UpdateShadow(IGeminiRequestContext requestContext, Guid shadowId, string name)
	{
		UserDefinedEvent userDefinedEvent = UserDefinedEvent.QueryUserDefinedEvent(requestContext, shadowId.ToString());
		if (userDefinedEvent != null)
		{
			userDefinedEvent.Name = name;
			UserDefinedEvent.UpdateUserDefinedEvent(requestContext, userDefinedEvent);
		}
	}

	private static Guid CreateShadow(IGeminiRequestContext requestContext, string name)
	{
		Guid result = Guid.NewGuid();
		UserDefinedEvent userDefinedEvent = new UserDefinedEvent
		{
			Name = name,
			Id = result.ToString(),
			Subtype = "Generic"
		};
		UserDefinedEvent.CreateUserDefinedEvent(requestContext, userDefinedEvent);
		return result;
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

	private static void ThrowIfDuplicateName(IGeminiRequestContext requestContext, string name, Guid objectId)
	{
		Validation.CheckName(name, "Name", allowNull: false);
		foreach (Item item in MIPItem.GetItems(requestContext, GenericKind))
		{
			if (item.FQID.ObjectId != objectId && item.Name == name)
			{
				throw new ArgumentException("Duplicate name");
			}
		}
	}

	private static string ConvertToRegularExpression(int expressionType, string expression)
	{
		switch (expressionType)
		{
		case 0:
		case 1:
		{
			string empty = string.Empty;
			try
			{
				if (string.IsNullOrEmpty(expression))
				{
					throw new ArgumentNullException("expression");
				}
				bool flag = expressionType == 1;
				empty = GenericEventRules.BuildGenericEventRuleTree(expression).GetExpression(flag);
				if (flag)
				{
					empty = GenericEventRules.FinishMatchExpression(empty);
				}
				return empty;
			}
			catch (Exception)
			{
				throw new ArgumentException("Invalid expression", "expression");
			}
		}
		case 2:
			return expression;
		default:
			throw new ArgumentException("ExpressionType must be between 0..2", "expressionType");
		}
	}

	private static ConfigurationItem ConvertToGenericEventConfigurationItem(IGeminiRequestContext requestContext, ConfigurationItemPath parentPath, Item genericItem)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "GenericEvent",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = genericItem.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, genericItem.FQID.ObjectId.ToString()).Text;
		obj.EnableProperty = GeneralPropertyDefinitions.GetEnabledProperty(genericItem.Enabled, uiFollowsEnabled: true);
		List<Property> list = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(genericItem.FQID.ObjectId.ToString()),
			GeneralPropertyDefinitions.GetNameProperty(genericItem.Name, isSettable: true)
		};
		string text = ConfigurationItemPath.CreateItemPath("GenericEventDataSource", GetValue(genericItem.Properties, DataSourceKey, "")).Text;
		list.Add(GenericEventProperties.DataSourceProperty(text, isSettable: true, GenericEventDataSource.QueryDataSources(requestContext)));
		list.Add(GenericEventProperties.ExpressionProperty(GetValue(genericItem.Properties, ExpressionKey, ""), isSettable: true));
		list.Add(GenericEventProperties.ExpressionTypeProperty(GetValue(genericItem.Properties, ExpressionTypeKey, "0"), isSettable: true));
		list.Add(GenericEventProperties.PriorityProperty(GetValue(genericItem.Properties, PriorityKey, "1"), isSettable: true));
		list.Add(GenericEventProperties.ShadowIdProperty(GetValue(genericItem.Properties, ShadowIdKey, "")));
		string value = "";
		DateTime result = DateTime.UtcNow;
		if (genericItem.Properties.TryGetValue("LastModified", out value) && !DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out result))
		{
			Trace.WriteLine("GenericEventConfigurationApi: Unable to parse LastModified:" + value);
		}
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(result));
		obj.Properties = list.ToArray();
		List<string> list2 = new List<string>();
		obj.MethodIds = list2.ToArray();
		return obj;
	}

	private static string GetValue(Dictionary<string, string> properties, string key, string defaultValue)
	{
		if (properties.ContainsKey(key))
		{
			return properties[key];
		}
		return defaultValue;
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
}
