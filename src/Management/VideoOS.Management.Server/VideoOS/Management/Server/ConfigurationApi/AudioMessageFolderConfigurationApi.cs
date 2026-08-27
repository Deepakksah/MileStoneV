using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIAudioMessageFolder")]
public class AudioMessageFolderConfigurationApi : AudioMessageConfigurationApiObjectBase
{
	public AudioMessageFolderConfigurationApi()
		: this(null)
	{
	}

	public AudioMessageFolderConfigurationApi(IAudioMessageRepository repository)
		: base(repository)
	{
	}

	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(requestContext, "requestContext");
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(path, "path");
		ThrowIfPathInvalid(path);
		AudioMessage[] array = base.Repository.QueryAudioMessages(requestContext);
		ConfigurationItem[] array2 = new ConfigurationItem[array.Length];
		for (int i = 0; i < array2.Length; i++)
		{
			array2[i] = AudioMessageConfigurationApiObjectBase.ConvertToAudioMessageConfigurationItem(array[i], includeContent: false);
		}
		return array2;
	}

	public override ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(requestContext, "requestContext");
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(path, "path");
		ThrowIfPathInvalid(path);
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemCategory = ItemCategories.Group;
		configurationItem.ItemType = "AudioMessageFolder";
		configurationItem.Path = "/AudioMessageFolder";
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeAudioMessageFolder;
		configurationItem.ParentPath = "/";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.MethodIds = new string[2] { "AddAudioMessage", "RemoveAudioMessage" };
		return configurationItem;
	}

	protected override ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ConfigurationItem configurationItem;
		if (!(methodId == "AddAudioMessage"))
		{
			if (methodId == "RemoveAudioMessage")
			{
				configurationItem = new ConfigurationItem();
				configurationItem.ItemType = "InvokeInfo";
				configurationItem.UIHint = "DirectionVertical";
				configurationItem.ItemCategory = ItemCategories.Item;
				configurationItem.DisplayName = ConfigurationApiResources.MethodRemoveAudioMessage;
				configurationItem.ParentPath = "/";
				configurationItem.Path = "/AudioMessageFolder";
				configurationItem.MethodIds = new string[1] { "RemoveAudioMessage" };
				configurationItem.Properties = new Property[1] { GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.Empty) };
				return configurationItem;
			}
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
		configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.UIHint = "DirectionVertical";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodAddAudioMessage;
		configurationItem.ParentPath = "/";
		configurationItem.Path = "/AudioMessageFolder";
		configurationItem.MethodIds = new string[1] { "AddAudioMessage" };
		configurationItem.Properties = new Property[5]
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(Guid.Empty),
			GeneralPropertyDefinitions.GetNameProperty(""),
			GeneralPropertyDefinitions.GetDescriptionProperty(""),
			AudioMessagePropertyDefinition.OriginalFileNameProperty(""),
			AudioMessagePropertyDefinition.ContentProperty("")
		};
		return configurationItem;
	}

	protected override ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ThrowIfPathInvalid(path);
		if (!(methodId == "AddAudioMessage"))
		{
			if (methodId == "RemoveAudioMessage")
			{
				return DeleteAudioFile(requestContext, item);
			}
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
		return AddAudioFile(requestContext, item);
	}

	protected override void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		ThrowIfFolderPathInvalid(path);
	}

	protected override void ThrowIfParentPathInvalid(ConfigurationItemPath path)
	{
		if (path != null && path.ParentItemType != null && path.ParentItemType.Length == 0 && path.Id != null && path.Id.Length == 0 && path.ItemType == "System")
		{
			return;
		}
		throw new PathInvalidException("Invalid audio folder path: " + path?.Text);
	}

	public static void ThrowIfFolderPathInvalid(ConfigurationItemPath path)
	{
		if (path != null && path.IsFolder && string.Equals(path.ParentItemType, "System", StringComparison.Ordinal))
		{
			return;
		}
		throw new PathInvalidException("Invalid audio folder path: " + path?.Text);
	}

	private ConfigurationItem AddAudioFile(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		string value = null;
		string value2 = null;
		string value3 = null;
		string value4 = null;
		string value5 = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Id", out value);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out value2);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out value3);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("OriginalFileName", out value4);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Content", out value5);
		}
		if (value != null && value.IsGuid() && value.Equals(Guid.Empty.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			value = null;
		}
		try
		{
			ConfigurationItem configurationItem = AudioMessageConfigurationApiObjectBase.ConvertToAudioMessageConfigurationItem(base.Repository.CreateAudioMessage(requestContext, value, value2, value3, value4, value5), includeContent: false);
			return MethodTypesHelper.CreateInvokeResult(configurationItem, configurationItem.Path, "Result", "Success", true);
		}
		catch (VmoServerException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
	}

	private ConfigurationItem DeleteAudioFile(IGeminiRequestContext requestContext, ConfigurationItem item)
	{
		string value = null;
		item.Properties?.TryGetPropertyValue("Id", out value);
		try
		{
			base.Repository.DeleteAudioMessage(requestContext, value);
		}
		catch (VmoServerException ex)
		{
			throw new PropertyInvalidException(ex.Message);
		}
		return MethodTypesHelper.CreateInvokeResultWithoutItemPropertiesAndChildren(new ConfigurationItem
		{
			Path = ConfigurationItemPath.CreateItemPath("AudioMessage", value).Text,
			ParentPath = "/AudioMessageFolder"
		});
	}
}
