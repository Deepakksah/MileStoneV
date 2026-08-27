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

[EntityName("ConfigAPIAnalyticsEvent")]
[EntityName("ConfigAPIAnalyticsEventFolder")]
public class AnalyticsEventConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public static Guid AnalyticsPluginId = new Guid("9F6E7E9D-3FD2-49B2-9CA0-F7E7F8F8C3A7");

	public static Guid AnalyticsKind = new Guid("2E319F3E-0350-4DF1-90C6-633EAD31C117");

	private Guid _serverId = Guid.Empty;

	private static string[] AnalyticsSources = new string[2] { "Camera", "CameraGroup" };

	private Guid ManagementServerId(IGeminiRequestContext requestContext)
	{
		if (_serverId == Guid.Empty && !Guid.TryParse(ServerInformation.GetServerInformation(requestContext).Id, out _serverId))
		{
			Trace.WriteLine("AnalyticsEventConfiguration: No Mgt Server Id ?");
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
		Item item2 = MIPItem.GetItem(requestContext, new Guid(path.Id));
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out var name, out var description);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		if (name != null)
		{
			item2.Name = name;
		}
		if (description != null)
		{
			item2.Properties["Description"] = description;
		}
		MIPItem.SaveItem(requestContext, item2);
		ConfigurationItem resultItem = ConvertToAnalyticsEventConfigurationItem(parentPath, item2);
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
		if (!path.IsFolder)
		{
			if (MIPItem.GetItem(requestContext, new Guid(path.Id)) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string name;
			string description;
			return ValidateItemInternal(requestContext, item, path, out name, out description);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out string name, out string description)
	{
		name = null;
		description = null;
		try
		{
			if (!path.IsFolder)
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
				}
				ThrowIfDuplicateName(requestContext, name, new Guid(path.Id));
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
			List<Item> items = MIPItem.GetItems(requestContext, AnalyticsKind);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			foreach (Item item in items)
			{
				list.Add(ConvertToAnalyticsEventConfigurationItem(path, item));
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
			return GroupConfigurationItemFactory.CreateFolderItem("AnalyticsEventFolder", ConfigurationItemPath.CreateSystemPath());
		}
		Item item = MIPItem.GetItem(requestContext, new Guid(path.Id));
		if (item == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToAnalyticsEventConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "AnalyticsEventFolder").CreatePath(), item);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (!(method == "AddAnalyticsEvent"))
		{
			if (method == "RemoveAnalyticsEvent")
			{
				if (!flag)
				{
					return CreateInvokeInfoRemoveAnalyticsEvent(item, GetChildItems(requestContext, path), method);
				}
				return RemoveAnalyticsEvent(requestContext, item, path);
			}
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
		if (!flag)
		{
			return CreateInvokeInfoAddAnalyticsEvent(item, method);
		}
		return AddAnalyticsEvent(requestContext, item, path);
	}

	private static ConfigurationItem CreateInvokeInfoAddAnalyticsEvent(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddAnalyticsEvent;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[2]
		{
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty("")
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveAnalyticsEvent(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveAnalyticsEvent, "PropertyRemoveAnalyticsEvent", ConfigurationApiResources.PropertyRemoveAnalyticsEvent, item, members);
	}

	private ConfigurationItem AddAnalyticsEvent(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = "New Analytics Event";
		string value2 = "";
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out value2);
		}
		try
		{
			ThrowIfDuplicateName(requestContext, value, Guid.Empty);
		}
		catch (ArgumentException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
		try
		{
			Item item2 = new Item(new FQID(new ServerId("XPCO", "localhost", 80, ManagementServerId(requestContext)), Guid.Empty, Guid.NewGuid(), FolderType.No, AnalyticsKind), value);
			item2.Properties["Description"] = value2;
			MIPItem.SaveItem(requestContext, item2);
			ConfigurationItem configurationItem = ConvertToAnalyticsEventConfigurationItem(path, item2);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (UnauthorizedAccessException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			throw new PropertyInvalidException(ex3.Message);
		}
	}

	private static ConfigurationItem RemoveAnalyticsEvent(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		Item item2 = MIPItem.GetItem(requestContext, new Guid(invokeMemberItemSelectionPath.Id));
		if (item2 == null)
		{
			throw new PathNotFoundException();
		}
		try
		{
			MIPItem.Delete(requestContext, item2);
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

	private static void ThrowIfDuplicateName(IGeminiRequestContext requestContext, string name, Guid objectId)
	{
		Validation.CheckName(name, "Name", allowNull: false);
		foreach (Item item in MIPItem.GetItems(requestContext, AnalyticsKind))
		{
			if (item.FQID.ObjectId != objectId && string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException("Duplicate name");
			}
		}
	}

	private static ConfigurationItem ConvertToAnalyticsEventConfigurationItem(ConfigurationItemPath parentPath, Item analyticsItem)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "AnalyticsEvent",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = analyticsItem.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, analyticsItem.FQID.ObjectId.ToString()).Text;
		List<Property> list = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(analyticsItem.FQID.ObjectId.ToString()),
			GeneralPropertyDefinitions.GetNameProperty(analyticsItem.Name, isSettable: true)
		};
		string value = "";
		DateTime result = DateTime.UtcNow;
		if (analyticsItem.Properties.TryGetValue("LastModified", out value) && !DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out result))
		{
			Trace.WriteLine("AnalyticsEventConfigurationApi: Unable to parse LastModified:" + value);
		}
		list.Add(GeneralPropertyDefinitions.GetLastModifiedProperty(result));
		string value2 = "";
		analyticsItem.Properties.TryGetValue("Description", out value2);
		list.Add(GeneralPropertyDefinitions.GetDescriptionProperty(value2, isSettable: true));
		list.Add(EventTypeProperties.GetSourceArrayProperty(AnalyticsSources));
		obj.Properties = list.ToArray();
		List<string> list2 = new List<string>();
		obj.MethodIds = list2.ToArray();
		return obj;
	}
}
