using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPILayout")]
[EntityName("ConfigAPILayoutFolder")]
public class ViewLayoutConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	private const string DefaultDefinitionXml = "";

	private const string DefaultLayoutType = "VideoOS.RemoteClient.Application.Data.ViewLayouts.ViewLayoutCustom, VideoOS.RemoteClient.Application";

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			string[] ids = ViewLayout.QueryViewLayoutIdsByViewGroupId(requestContext, path.Id);
			ViewLayout[] array = ViewLayout.QueryViewLayoutByIds(requestContext, ids);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			ViewLayout[] array2 = array;
			foreach (ViewLayout viewLayout in array2)
			{
				list.Add(ConvertToLayoutConfigurationItem(path, viewLayout, !ViewLayout.IsBuiltinLayout(viewLayout.Id)));
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
			return GroupConfigurationItemFactory.CreateFolderItem("LayoutFolder", ConfigurationItemPath.CreateItemPath("LayoutGroup", path.Id));
		}
		ViewLayout viewLayout = null;
		try
		{
			viewLayout = ViewLayout.QueryViewLayoutById(requestContext, path.Id);
			if (viewLayout == null)
			{
				throw new PathNotFoundException(path.Text);
			}
		}
		catch (ViewLayoutNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToLayoutConfigurationItem(ConfigurationItemPath.CreateFolderPath("LayoutGroup", viewLayout.ViewLayoutGroupId, "LayoutFolder"), viewLayout, !ViewLayout.IsBuiltinLayout(path.Id));
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		if (!(method == "AddLayout"))
		{
			if (method == "RemoveLayout")
			{
				if (!flag)
				{
					return CreateInvokeInfoRemoveLayout(item, CustomChildItems(requestContext, path), method);
				}
				return RemoveLayout(requestContext, item, path);
			}
			throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, "Unknown method: {0}", method));
		}
		if (!flag)
		{
			return CreateInvokeInfoAddLayout(item, method);
		}
		return AddLayout(requestContext, item, path, parentPath);
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
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath, isFolder: false, parentPath.Id, path.Id, out var name, out var description, out var definitionXml);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		try
		{
			ViewLayout.UpdateViewLayout(requestContext, path.Id, name, description, definitionXml);
			ViewLayout viewLayout = ViewLayout.QueryViewLayoutById(requestContext, path.Id);
			ConfigurationItem resultItem = ConvertToLayoutConfigurationItem(parentPath, viewLayout, isSettable: true);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem;
			return validateResult;
		}
		catch (DuplicateNameException)
		{
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = false;
			validateResult.ErrorResults = new ErrorResult[1]
			{
				new ErrorResult
				{
					ErrorText = "Duplicate name",
					ErrorProperty = "Name"
				}
			};
			return validateResult;
		}
		catch (ViewLayoutNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
		catch (BuiltInViewLayoutCannotBeChangedException ex3)
		{
			throw new ArgumentException(ex3.Message, "path");
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		try
		{
			ViewLayout.CheckLayoutIdForChange(path.Id);
		}
		catch (BuiltInViewLayoutCannotBeChangedException ex)
		{
			ValidateResult validateResult = new ValidateResult();
			validateResult.ValidatedOk = false;
			validateResult.ErrorResults = new ErrorResult[1]
			{
				new ErrorResult
				{
					ErrorText = ex.Message,
					ErrorProperty = "Path"
				}
			};
			return validateResult;
		}
		string name = "";
		string description = "";
		string definitionXml = "";
		return ValidateItemInternal(requestContext, item, path, parentPath, isFolder: false, parentPath.Id, path.Id, out name, out description, out definitionXml);
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, bool isFolder, string viewGroupId, string id, out string name, out string description, out string definitionXml)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		name = "";
		description = "";
		definitionXml = "";
		if (path.IsFolder == isFolder)
		{
			if (!isFolder)
			{
				try
				{
					if (ViewLayout.QueryViewLayoutById(requestContext, path.Id) == null)
					{
						throw new PathNotFoundException(path.Text);
					}
				}
				catch (ViewLayoutNotFoundException)
				{
					throw new PathNotFoundException(path.Text);
				}
			}
			if (item.Properties != null)
			{
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
				((IEnumerable<Property>)item.Properties).TryGetPropertyValue("DefinitionXml", out definitionXml);
			}
			ValidateResult validateResult;
			try
			{
				ViewLayout.UpdateLayoutValidate(name, ref description, ref definitionXml, viewGroupId, id);
			}
			catch (DuplicateNameException exception)
			{
				validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
				if (validateResult != null)
				{
					return validateResult;
				}
				throw;
			}
			catch (ArgumentNullException exception2)
			{
				validateResult = ErrorHelper.CreateValidateResult(exception2, item.Properties);
				if (validateResult != null)
				{
					return validateResult;
				}
				throw;
			}
			catch (ArgumentException exception3)
			{
				validateResult = ErrorHelper.CreateValidateResult(exception3, item.Properties);
				if (validateResult != null)
				{
					return validateResult;
				}
				throw;
			}
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = item;
			return validateResult;
		}
		throw new NotSupportedException();
	}

	private static ConfigurationItem CreateInvokeInfoAddLayout(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddLayout;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[3]
		{
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			ViewLayoutPropertyDefinition.DefinitionXmlProperty("", isSettable: true)
		};
		return configurationItem;
	}

	private static ConfigurationItem AddLayout(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "LayoutFolder", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid Path: " + path.Text);
		}
		string id = Guid.NewGuid().ToString();
		string name;
		string description;
		string definitionXml;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath, isFolder: true, path.Id, id, out name, out description, out definitionXml);
		if (!validateResult.ValidatedOk)
		{
			throw new PropertyInvalidException(validateResult.ErrorResults.First().ErrorText);
		}
		try
		{
			ViewLayout.CreateViewLayout(requestContext, id, parentPath.Id, name, description, definitionXml);
		}
		catch (DuplicateNameException)
		{
			throw new PropertyInvalidException("Duplicate name not allowed", "Name");
		}
		catch (ServerGroupNotFoundException)
		{
			throw new PathInvalidException("Built-in group not found");
		}
		ViewLayout viewLayout = ViewLayout.QueryViewLayoutById(requestContext, id);
		ConfigurationItem configurationItem = ConvertToLayoutConfigurationItem(path, viewLayout, isSettable: true);
		return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
	}

	private static ConfigurationItem CreateInvokeInfoRemoveLayout(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveLayout, "PropertyRemoveLayout", ConfigurationApiResources.PropertyRemoveLayout, item, members);
	}

	private static ConfigurationItem RemoveLayout(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "LayoutFolder", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			if (ViewLayout.QueryViewLayoutById(requestContext, invokeMemberItemSelectionPath.Id) == null)
			{
				throw new PathNotFoundException();
			}
			ViewLayout.DeleteViewLayout(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (ViewLayoutNotFoundException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResult(item, path.Text, "Result", "Success", false);
	}

	private static ConfigurationItem[] CustomChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		string[] ids = ViewLayout.QueryViewLayoutIdsByViewGroupId(requestContext, path.Id);
		ViewLayout[] array = ViewLayout.QueryViewLayoutByIds(requestContext, ids);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		ViewLayout[] array2 = array;
		foreach (ViewLayout viewLayout in array2)
		{
			if (!ViewLayout.IsBuiltinLayout(viewLayout.Id))
			{
				list.Add(ConvertToLayoutConfigurationItem(path, viewLayout, isSettable: true));
			}
		}
		return list.ToArray();
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "LayoutGroup", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ConfigurationItem ConvertToLayoutConfigurationItem(ConfigurationItemPath parentPath, ViewLayout viewLayout, bool isSettable)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "Layout",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = viewLayout.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, viewLayout.Id).Text;
		obj.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(viewLayout.Id),
			GeneralPropertyDefinitions.GetNameProperty(viewLayout.Name, isSettable),
			GeneralPropertyDefinitions.GetDescriptionProperty(viewLayout.Description, isSettable),
			GeneralPropertyDefinitions.GetLastModifiedProperty(viewLayout.LastModified),
			GeneralPropertyDefinitions.GetCreatedDateProperty(viewLayout.CreatedDate),
			ViewLayoutPropertyDefinition.DefinitionXmlProperty(viewLayout.DefinitionXml, isSettable)
		}.ToArray();
		List<string> list = new List<string>();
		obj.MethodIds = list.ToArray();
		return obj;
	}
}
