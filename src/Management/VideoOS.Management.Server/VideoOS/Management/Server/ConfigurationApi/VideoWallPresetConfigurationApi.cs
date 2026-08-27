using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIVideoWallPreset")]
[EntityName("ConfigAPIVideoWallPresetFolder")]
public class VideoWallPresetConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			string[] videoWallPresetIds = VideoWallPreset.QueryVideoWallPresetIds(requestContext, path.Id);
			VideoWallPreset[] array = VideoWallPreset.QueryVideoWallPresetsByIds(requestContext, videoWallPresetIds);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			VideoWallPreset[] array2 = array;
			foreach (VideoWallPreset preset in array2)
			{
				list.Add(ConvertToVideoWallPresetConfigurationItem(path, preset));
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
			return GroupConfigurationItemFactory.CreateFolderItem("VideoWallPresetFolder", ConfigurationItemPath.CreateItemPath("VideoWall", path.Id));
		}
		VideoWallPreset videoWallPreset = VideoWallPreset.QueryVideoWallPreset(requestContext, path.Id);
		if (videoWallPreset == null)
		{
			throw new PathNotFoundException(path.Text);
		}
		return ConvertToVideoWallPresetConfigurationItem(ConfigurationItemPath.CreateFolderPath("VideoWall", videoWallPreset.VideoWallId, "VideoWallPresetFolder"), videoWallPreset);
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!item.MethodIds.Contains(method))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", method, item.ItemType), "method");
		}
		bool flag = string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal);
		switch (method)
		{
		case "AddVideoWallPreset":
			if (!flag)
			{
				return CreateInvokeInfoAddVideoWallPreset(item, method);
			}
			return AddVideoWallPreset(requestContext, item, path);
		case "RemoveVideoWallPreset":
			if (!flag)
			{
				return CreateInvokeInfoRemoveVideoWallPreset(item, GetChildItems(requestContext, path), method);
			}
			return RemoveVideoWallPreset(requestContext, item, path);
		case "Activate":
			return Activate(requestContext, item, path);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
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
		ValidateResult validateResult = ValidateItemInternal(item, path, out var name, out var description);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		try
		{
			VideoWallPreset videoWallPreset = VideoWallPreset.QueryVideoWallPreset(requestContext, path.Id);
			if (name != null)
			{
				videoWallPreset.Name = name;
			}
			if (description != null)
			{
				videoWallPreset.Description = description;
			}
			videoWallPreset = VideoWallPreset.UpdateVideoWallPreset(requestContext, videoWallPreset);
			ConfigurationItem resultItem = ConvertToVideoWallPresetConfigurationItem(parentPath, videoWallPreset);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem;
			return validateResult;
		}
		catch (VideoWallPresetUpdateException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (!path.IsFolder)
		{
			if (VideoWallPreset.QueryVideoWallPreset(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string name;
			string description;
			return ValidateItemInternal(item, path, out name, out description);
		}
		throw new NotSupportedException();
	}

	private static ConfigurationItem CreateInvokeInfoAddVideoWallPreset(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddVideoWallPreset;
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

	private static ConfigurationItem CreateInvokeInfoRemoveVideoWallPreset(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveVideoWallPreset, "PropertyRemoveVideoWallPreset", ConfigurationApiResources.PropertyRemoveVideoWallPreset, item, members);
	}

	private static ConfigurationItem AddVideoWallPreset(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "VideoWall", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string name;
		string description;
		ValidateResult validateResult = ValidateItemInternal(item, path, out name, out description, allowFolder: true);
		if (!validateResult.ValidatedOk)
		{
			throw new PropertyInvalidException(validateResult.ErrorResults.First().ErrorText);
		}
		try
		{
			VideoWallPreset preset = VideoWallPreset.CreateVideoWallPreset(requestContext, path.Id, new VideoWallPreset
			{
				Name = name,
				Description = description
			});
			ConfigurationItem configurationItem = ConvertToVideoWallPresetConfigurationItem(path, preset);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (VideoWallPresetCreationException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
	}

	private static ConfigurationItem RemoveVideoWallPreset(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "VideoWall", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			VideoWallPreset.DeleteVideoWallPreset(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (RemoveNonExistingVideoWallPresetException)
		{
			throw new PathNotFoundException(invokeMemberItemSelectionPath.Text);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(item);
	}

	private static ConfigurationItem Activate(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ItemType, "VideoWallPreset", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		try
		{
			VideoWallPreset.ActivateVideoWallPreset(requestContext, path.Id);
			return MethodTypesHelper.CreateInvokeResult(item, null, "Result", "Success", true);
		}
		catch (VideoWallPresetNotFoundException exception)
		{
			throw new PathInvalidException("Preset not found.", exception);
		}
	}

	private static ValidateResult ValidateItemInternal(ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, bool allowFolder = false)
	{
		name = null;
		description = null;
		try
		{
			if (!path.IsFolder || allowFolder)
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
				}
				Validation.CheckName(name, "Name", allowNull: true);
				Validation.CheckDescription(ref description, "Description", allowNull: true);
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

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "VideoWall", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static ConfigurationItem ConvertToVideoWallPresetConfigurationItem(ConfigurationItemPath parentPath, VideoWallPreset preset)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "VideoWallPreset",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = preset.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, preset.Id).Text;
		obj.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(preset.Id),
			GeneralPropertyDefinitions.GetNameProperty(preset.Name),
			GeneralPropertyDefinitions.GetDescriptionProperty(preset.Description),
			GeneralPropertyDefinitions.GetLastModifiedProperty(preset.LastModified),
			GeneralPropertyDefinitions.GetCreatedDateProperty(preset.CreatedDate)
		}.ToArray();
		obj.MethodIds = new List<string> { "Activate" }.ToArray();
		return obj;
	}
}
