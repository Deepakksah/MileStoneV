using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIVideoWall")]
[EntityName("ConfigAPIVideoWallFolder")]
public class VideoWallConfigurationApi : NewSecurityConfigurationApi, IConfigurationApiSupport
{
	protected override Dictionary<string, List<string>> ItemTypeActionsToExclude => new Dictionary<string, List<string>> { 
	{
		"VideoWall",
		new List<string> { "CREATE_SMARTWALL", "ADMIN_SECURITY" }
	} };

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		ValidateResult validateResult = ValidateItemInternal(item, path, out var name, out var description, out var statusText, out var titleBar, allowNameIsNull: true);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		try
		{
			VideoWall videoWall = VideoWall.QueryVideoWall(requestContext, path.Id);
			if (name != null)
			{
				videoWall.Name = name;
			}
			if (description != null)
			{
				videoWall.Description = description;
			}
			if (statusText.HasValue)
			{
				videoWall.StatusText = statusText.Value;
			}
			if (titleBar.HasValue)
			{
				videoWall.TitleBar = titleBar.Value;
			}
			videoWall = VideoWall.UpdateVideoWall(requestContext, videoWall);
			ConfigurationItem resultItem = ConvertToVideoWallConfigurationItem(parentPath, videoWall);
			validateResult = new ValidateResult();
			validateResult.ValidatedOk = true;
			validateResult.ResultItem = resultItem;
			return validateResult;
		}
		catch (VideoWallUpdateException)
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
			if (VideoWall.QueryVideoWall(requestContext, path.Id) == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			string name;
			string description;
			bool? statusText;
			TitleBar? titleBar;
			return ValidateItemInternal(item, path, out name, out description, out statusText, out titleBar, allowNameIsNull: false);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(ConfigurationItem item, ConfigurationItemPath path, out string name, out string description, out bool? statusText, out TitleBar? titleBar, bool allowNameIsNull)
	{
		name = null;
		description = null;
		statusText = null;
		titleBar = null;
		try
		{
			if (!path.IsFolder)
			{
				if (item.Properties != null)
				{
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("StatusText", out statusText);
					((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TitleBar", out titleBar);
				}
				Validation.CheckName(name, "Name", allowNameIsNull);
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
			string[] ids = VideoWall.QueryVideoWallIds(requestContext);
			VideoWall[] array = VideoWall.QueryVideoWalls(requestContext, ids);
			List<ConfigurationItem> list = new List<ConfigurationItem>();
			VideoWall[] array2 = array;
			foreach (VideoWall videoWall in array2)
			{
				list.Add(ConvertToVideoWallConfigurationItem(path, videoWall));
			}
			return list.ToArray();
		}
		ConfigurationItem configurationItem = GroupConfigurationItemFactory.CreateFolderItem("MonitorFolder", path);
		ConfigurationItem configurationItem2 = GroupConfigurationItemFactory.CreateFolderItem("VideoWallPresetFolder", path);
		return new ConfigurationItem[2] { configurationItem, configurationItem2 };
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			return GroupConfigurationItemFactory.CreateFolderItem("VideoWallFolder", ConfigurationItemPath.CreateSystemPath());
		}
		try
		{
			VideoWall videoWall = VideoWall.QueryVideoWall(requestContext, path.Id);
			if (videoWall == null)
			{
				throw new PathNotFoundException(path.Text);
			}
			return ConvertToVideoWallConfigurationItem(new ConfigurationItemPathBuilder(ConfigurationItemPath.CreateSystemPath(), "VideoWallFolder").CreatePath(), videoWall);
		}
		catch (VideoWallNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
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
		case "AddVideoWall":
			if (!flag)
			{
				return CreateInvokeInfoAddVideoWall(item, method);
			}
			return AddVideoWall(requestContext, item, path);
		case "RemoveVideoWall":
			if (!flag)
			{
				return CreateInvokeInfoRemoveVideoWall(item, GetChildItems(requestContext, path), method);
			}
			return RemoveVideoWall(requestContext, item, path);
		case "ChangeSecurityPermissions":
			return InvokeSecurityMethod(requestContext, item, path, method);
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Unknown methodId: {0}", method), method);
		}
	}

	public override bool SetSecurityOnRoleOnly()
	{
		return true;
	}

	private static ConfigurationItem CreateInvokeInfoAddVideoWall(ConfigurationItem item, string methodId)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.WizardAddVideoWall;
		configurationItem.ParentPath = item.ParentPath;
		configurationItem.Path = item.Path;
		configurationItem.MethodIds = new string[1] { methodId };
		configurationItem.Properties = new Property[4]
		{
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			VideoWallPropertyDefinition.StatusTextProperty(statusText: false),
			VideoWallPropertyDefinition.TitleBarProperty(TitleBar.None)
		};
		return configurationItem;
	}

	private static ConfigurationItem CreateInvokeInfoRemoveVideoWall(ConfigurationItem item, ConfigurationItem[] members, string methodId)
	{
		return MethodTypesHelper.CreateInvokeMemberSelectionInfo(methodId, ConfigurationApiResources.WizardRemoveVideoWall, "PropertyRemoveVideoWall", ConfigurationApiResources.PropertyRemoveVideoWall, item, members);
	}

	private static ConfigurationItem AddVideoWall(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		string value = null;
		string value2 = null;
		bool? value3 = null;
		TitleBar? value4 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("StatusText", out value3);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("TitleBar", out value4);
		}
		try
		{
			Validation.CheckName(value, "Name", allowNull: false);
		}
		catch (ArgumentException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
		try
		{
			VideoWall videoWall = VideoWall.CreateVideoWall(requestContext, new VideoWall
			{
				Name = value,
				Description = value2,
				Height = 200.0,
				Width = 200.0,
				StatusText = (value3 == true),
				TitleBar = value4.GetValueOrDefault()
			});
			ConfigurationItem configurationItem = ConvertToVideoWallConfigurationItem(path, videoWall);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (VideoWallCreationException ex2)
		{
			throw new PropertyInvalidException(ex2.Message);
		}
	}

	private static ConfigurationItem RemoveVideoWall(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, item, path);
		if (!string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			throw new PathInvalidException("Invalid parent. Path: " + path.Text);
		}
		ConfigurationItemPath invokeMemberItemSelectionPath = MethodTypesHelper.GetInvokeMemberItemSelectionPath(item);
		try
		{
			VideoWall.DeleteVideoWall(requestContext, invokeMemberItemSelectionPath.Id);
		}
		catch (RemoveNonExistingVideoWallException)
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

	private static ConfigurationItem ConvertToVideoWallConfigurationItem(ConfigurationItemPath parentPath, VideoWall videoWall)
	{
		ConfigurationItem obj = new ConfigurationItem
		{
			ItemType = "VideoWall",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = videoWall.Name,
			ParentPath = parentPath.Text
		};
		obj.Path = ConfigurationItemPath.CreateItemPath(obj.ItemType, videoWall.Id).Text;
		obj.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(videoWall.Id),
			GeneralPropertyDefinitions.GetNameProperty(videoWall.Name),
			GeneralPropertyDefinitions.GetDescriptionProperty(videoWall.Description),
			GeneralPropertyDefinitions.GetLastModifiedProperty(videoWall.LastModified),
			GeneralPropertyDefinitions.GetCreatedDateProperty(videoWall.CreatedDate),
			VideoWallPropertyDefinition.HeightProperty(videoWall.Height),
			VideoWallPropertyDefinition.StatusTextProperty(videoWall.StatusText),
			VideoWallPropertyDefinition.TitleBarProperty(videoWall.TitleBar),
			VideoWallPropertyDefinition.WidthProperty(videoWall.Width)
		}.ToArray();
		List<string> list = new List<string>();
		obj.MethodIds = list.ToArray();
		obj.MethodIds = SecurityConfigurationApiBase.AddSecurityMethods(obj.MethodIds);
		return obj;
	}
}
