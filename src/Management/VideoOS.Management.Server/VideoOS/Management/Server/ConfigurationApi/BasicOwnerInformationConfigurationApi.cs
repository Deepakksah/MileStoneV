using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIBasicOwnerInformationFolder")]
[EntityName("ConfigAPIBasicOwnerInformation")]
public class BasicOwnerInformationConfigurationApi : BasicOwnerInformationConfigurationApiObjectBase
{
	private static List<string> _tagGroups = new List<string> { "address", "admin", "additional" };

	private static List<string> _tagNames = new List<string> { "Name", "Address", "Phone", "ZipCode", "Email", "State", "Country", "AdditionalInfo" };

	public BasicOwnerInformationConfigurationApi(IBasicOwnerInformationDataRepository repository)
		: base(repository)
	{
	}

	public BasicOwnerInformationConfigurationApi()
		: this(null)
	{
	}

	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		ValidateInternal(requestContext, item);
		BasicOwnerInfo[] source = base.Repository.QueryBasicUserInfo(requestContext);
		Guid sessionId = Guid.NewGuid();
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (BasicOwnerInformationPropertyDefinition.UnformatKey(property.Key, out var id, out var _))
			{
				string value = property.Value;
				BasicOwnerInfo basicOwnerInfo = source.FirstOrDefault((BasicOwnerInfo b) => b.Id == id);
				if (basicOwnerInfo != null && basicOwnerInfo.Property != value)
				{
					base.Repository.UpdateBasicOwnerInfo(requestContext, id, basicOwnerInfo.Name, value, basicOwnerInfo.Order, sessionId);
				}
			}
		}
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = CreateBasicOwnerInformation(requestContext, base.Repository.GetServerId(requestContext))
		};
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (parentPath == null)
		{
			throw new ArgumentNullException("parentPath");
		}
		ValidateInternal(requestContext, item);
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = item
		};
	}

	public override ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (item == null)
		{
			throw new ArgumentNullException("item");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (methodId == null)
		{
			throw new ArgumentNullException("methodId");
		}
		if (!(methodId == "AddBasicOwnerInfo"))
		{
			if (methodId == "RemoveBasicOwnerInfo")
			{
				if (string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
				{
					return RemoveBasicOwnerInfo(requestContext, item);
				}
				return CreateItemRemoveBasicOwnerInfo(requestContext, item, methodId);
			}
			throw new ArgumentException("Unknown methodId:" + methodId);
		}
		if (string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return AddBasicOwnerInfo(requestContext, item, path);
		}
		return CreateItemAddBasicOwnerInfo(item, methodId);
	}

	public override ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.IsFolder && path.ItemType == "BasicOwnerInformationFolder")
		{
			return GroupConfigurationItemFactory.CreateFolderItem("BasicOwnerInformationFolder", ConfigurationItemPath.CreateSystemPath());
		}
		if (!path.IsFolder && path.ItemType == "BasicOwnerInformation")
		{
			if (string.Compare(path.Id, base.Repository.GetServerId(requestContext), StringComparison.OrdinalIgnoreCase) != 0)
			{
				throw new PathNotFoundException(path.Text);
			}
			return CreateBasicOwnerInformation(requestContext, path.Id);
		}
		throw new PathNotFoundException(path.Text);
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		if (path.IsFolder && path.ItemType == "BasicOwnerInformationFolder")
		{
			ConfigurationItem configurationItem = CreateBasicOwnerInformation(requestContext, base.Repository.GetServerId(requestContext));
			return new ConfigurationItem[1] { configurationItem };
		}
		return new ConfigurationItem[0];
	}

	private ConfigurationItem CreateBasicOwnerInformation(IGeminiRequestContext requestContext, string serverId)
	{
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateSystemFolderPath("BasicOwnerInformationFolder");
		ConfigurationItemPath configurationItemPath2 = ConfigurationItemPath.CreateItemPath("BasicOwnerInformation", serverId);
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.ItemType = "BasicOwnerInformation";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.DisplayName = ConfigurationApiResources.BasicOwnerInformationDisplayName;
		configurationItem.Path = configurationItemPath2.Text;
		configurationItem.ParentPath = configurationItemPath.Text;
		BasicInfoHelper basicInfoHelper = new BasicInfoHelper(base.Repository.QueryBasicUserInfo(requestContext));
		List<Property> list = new List<Property>();
		foreach (KeyValuePair<string, TagValue> item in basicInfoHelper.Address)
		{
			list.Add(BasicOwnerInformationPropertyDefinition.BasicOwnerTagIdProperty(item.Key, "Address", item.Value.Key, item.Value.Tag, item.Value.Value, item.Value.Order));
		}
		foreach (KeyValuePair<string, TagValue> item2 in basicInfoHelper.Admin)
		{
			list.Add(BasicOwnerInformationPropertyDefinition.BasicOwnerTagIdProperty(item2.Key, "Admin", item2.Value.Key, item2.Value.Tag, item2.Value.Value, item2.Value.Order));
		}
		foreach (KeyValuePair<string, TagValue> item3 in basicInfoHelper.Additional)
		{
			list.Add(BasicOwnerInformationPropertyDefinition.BasicOwnerTagIdProperty(item3.Key, "Additional", item3.Value.Key, item3.Value.Tag, item3.Value.Value, item3.Value.Order));
		}
		configurationItem.Properties = list.ToArray();
		configurationItem.Children = new ConfigurationItem[0];
		configurationItem.ChildrenFilled = true;
		configurationItem.MethodIds = new string[2] { "AddBasicOwnerInfo", "RemoveBasicOwnerInfo" };
		return configurationItem;
	}

	public static ConfigurationItem CreateItemAddBasicOwnerInfo(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddBasicOwnerInfo;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(BasicOwnerInformationPropertyDefinition.BasicOwnerTagTypeProperty());
		list.Add(BasicOwnerInformationPropertyDefinition.BasicOwnerTagIdProperty("TagValue", "", 999));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private ConfigurationItem AddBasicOwnerInfo(IGeminiRequestContext requestContext, ConfigurationItem configurationItem, ConfigurationItemPath path)
	{
		string id = Guid.NewGuid().ToString();
		string text = null;
		((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("TagType", out string value);
		Property property = configurationItem.Properties.FirstOrDefault((Property p) => p.Key == "TagValue");
		if (property != null)
		{
			text = property.Value;
		}
		if (string.IsNullOrWhiteSpace(text))
		{
			throw new ArgumentNullException("configurationItem", "Value cannot be null or empty");
		}
		if (!CheckTagValid(value))
		{
			throw new PropertyInvalidException("Tag type is not valid: " + value);
		}
		try
		{
			int num = 1;
			BasicOwnerInfo[] array = base.Repository.QueryBasicUserInfo(requestContext);
			for (int num2 = 0; num2 < array.Length; num2++)
			{
				int num3 = int.Parse(array[num2].Order, CultureInfo.InvariantCulture);
				if (num3 > num)
				{
					num = num3;
				}
			}
			base.Repository.AddBasicUserInfo(requestContext, id, value, text, (num + 1).ToString(CultureInfo.InvariantCulture));
		}
		catch (ArgumentException)
		{
			throw;
		}
		return MethodTypesHelper.CreateInvokeResult(configurationItem, path.Text, "Result", "Success", true);
	}

	public ConfigurationItem CreateItemRemoveBasicOwnerInfo(IGeminiRequestContext requestContext, ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardRemoveBasicOwnerInfo;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[1] { methodId };
		list.Add(BasicOwnerInformationPropertyDefinition.SelectToRemoveProperty(base.Repository.QueryBasicUserInfo(requestContext)));
		configurationItem.Properties = list.ToArray();
		return configurationItem;
	}

	private ConfigurationItem RemoveBasicOwnerInfo(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		string value = null;
		if (!((IEnumerable<Property>)item.Properties).TryGetPropertyValue("ItemSelection", out value) && !((IEnumerable<Property>)item.Properties).TryGetPropertyValue("SelectToRemove", out value))
		{
			throw new PropertyInvalidException("No selection.", "ItemSelection");
		}
		if (value == null)
		{
			throw ExceptionFactory.CreateArgumentNullException("ItemSelection");
		}
		try
		{
			base.Repository.RemoveBasicUserInfo(requestContext, value);
		}
		catch (BasicOwnerInfoDeleteException inner)
		{
			throw new PropertyInvalidException("Invalid id " + value, "ItemSelection", inner);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static bool CheckTagValid(string tag)
	{
		string[] array = tag.Split('.');
		if (array.Length != 2)
		{
			return false;
		}
		if (!_tagGroups.Contains(array[0]))
		{
			return false;
		}
		if (!_tagNames.Contains(array[1]))
		{
			return false;
		}
		return true;
	}

	private void ValidateInternal(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		if (item.ItemType != "BasicOwnerInformation")
		{
			throw new NotSupportedException("ItemType not supported for this call");
		}
		if (item.Properties == null)
		{
			throw new ArgumentNullException("item", "The property 'item.Properties' cannot be null");
		}
		if (ConfigurationItemPath.CreatePath(item.Path).Id != base.Repository.GetServerId(requestContext))
		{
			throw new PathNotFoundException(item.Path);
		}
		Property[] properties = item.Properties;
		foreach (Property property in properties)
		{
			if (!BasicOwnerInformationPropertyDefinition.UnformatKey(property.Key, out var _, out var _))
			{
				throw new PropertyInvalidException("Unknown property key: " + property.Key);
			}
		}
	}
}
