using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIAudioMessage")]
public class AudioMessageConfigurationApi : AudioMessageConfigurationApiObjectBase
{
	public AudioMessageConfigurationApi(IAudioMessageRepository repository)
		: base(repository)
	{
	}

	public AudioMessageConfigurationApi()
		: this(null)
	{
	}

	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		string name;
		string description;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, parentPath, out name, out description);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		AudioMessage audioFileWithPathCheck = GetAudioFileWithPathCheck(requestContext, path);
		audioFileWithPathCheck = base.Repository.UpdateAudioMessage(requestContext, audioFileWithPathCheck.Id, name, description);
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = AudioMessageConfigurationApiObjectBase.ConvertToAudioMessageConfigurationItem(audioFileWithPathCheck, includeContent: false)
		};
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		string name;
		string description;
		return ValidateItemInternal(requestContext, item, path, parentPath, out name, out description);
	}

	private ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, out string name, out string description)
	{
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(requestContext, "requestContext");
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(item, "item");
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(path, "path");
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(parentPath, "parentPath");
		ThrowIfPathInvalid(path);
		ThrowIfParentPathInvalid(parentPath);
		GetAudioFileWithPathCheck(requestContext, path);
		string id = path.Id;
		name = null;
		description = null;
		if (item.Properties != null)
		{
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Name", out name);
			((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Description", out description);
		}
		try
		{
			base.Repository.UpdateAudioMessageValidation(requestContext, id, name, description);
			return new ValidateResult
			{
				ValidatedOk = true,
				ResultItem = item
			};
		}
		catch (ArgumentNullException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
		catch (ArgumentException exception2)
		{
			ValidateResult validateResult2 = ErrorHelper.CreateValidateResult(exception2, item.Properties);
			if (validateResult2 != null)
			{
				return validateResult2;
			}
			throw;
		}
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(requestContext, "requestContext");
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(path, "path");
		ThrowIfPathInvalid(path);
		return new ConfigurationItem[0];
	}

	public override ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(requestContext, "requestContext");
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(path, "path");
		ThrowIfPathInvalid(path);
		return AudioMessageConfigurationApiObjectBase.ConvertToAudioMessageConfigurationItem(GetAudioFileWithPathCheck(requestContext, path), includeContent: false);
	}

	protected override ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ThrowIfPathInvalid(path);
		if (!(methodId == "GetAudioMessageContent"))
		{
			if (methodId == "SetAudioMessageContent")
			{
				return SetAudioFileContent(requestContext, item, path);
			}
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
		return GetAudioFileContent(requestContext, path);
	}

	private ConfigurationItem SetAudioFileContent(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path)
	{
		AudioMessageConfigurationApiObjectBase.ParamNullCheck(item.Properties, "Properties");
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("OriginalFileName", out string value);
		((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Content", out string value2);
		try
		{
			base.Repository.SetMessageContent(requestContext, path.Id, value, value2);
			return MethodTypesHelper.CreateInvokeResult(AudioMessageConfigurationApiObjectBase.ConvertToAudioMessageConfigurationItem(base.Repository.QueryAudioMessageById(requestContext, path.Id), includeContent: false), null, "Result", "Success", true);
		}
		catch (ObjectNotFoundException exception)
		{
			throw new PathNotFoundException("Item path not found : " + path.Text, exception);
		}
	}

	private ConfigurationItem GetAudioFileContent(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		AudioMessage audioFileWithPathCheck = GetAudioFileWithPathCheck(requestContext, path);
		audioFileWithPathCheck.ContentBase64 = base.Repository.GetMessageContent(requestContext, path.Id);
		return MethodTypesHelper.CreateInvokeResult(AudioMessageConfigurationApiObjectBase.ConvertToAudioMessageConfigurationItem(audioFileWithPathCheck, includeContent: true), null, "Result", "Success", true);
	}

	protected override void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (!path.IsFolder && path.Id.IsGuid())
		{
			return;
		}
		throw new PathInvalidException("Invalid id. Path: " + path.Text);
	}

	protected override void ThrowIfParentPathInvalid(ConfigurationItemPath path)
	{
		AudioMessageFolderConfigurationApi.ThrowIfFolderPathInvalid(path);
	}

	protected override ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId)
	{
		ThrowIfPathInvalid(path);
		ConfigurationItem configurationItem;
		if (!(methodId == "GetAudioMessageContent"))
		{
			if (methodId == "SetAudioMessageContent")
			{
				configurationItem = new ConfigurationItem();
				configurationItem.ItemType = "InvokeInfo";
				configurationItem.ItemCategory = ItemCategories.Item;
				configurationItem.DisplayName = ConfigurationApiResources.MethodSetContentAudioMessage;
				configurationItem.ParentPath = "/AudioMessageFolder";
				configurationItem.Path = path.Text;
				configurationItem.MethodIds = new string[1] { "SetAudioMessageContent" };
				configurationItem.Properties = new Property[2]
				{
					AudioMessagePropertyDefinition.OriginalFileNameProperty(""),
					AudioMessagePropertyDefinition.ContentProperty("")
				};
				return configurationItem;
			}
			throw new ArgumentException("Unknown methodId:" + methodId, "methodId");
		}
		configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "InvokeInfo";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.MethodGetContentAudioMessage;
		configurationItem.ParentPath = "/AudioMessageFolder";
		configurationItem.Path = path.Text;
		configurationItem.MethodIds = new string[1] { "GetAudioMessageContent" };
		configurationItem.Properties = new Property[0];
		return configurationItem;
	}

	private AudioMessage GetAudioFileWithPathCheck(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		try
		{
			return base.Repository.QueryAudioMessageById(requestContext, path.Id);
		}
		catch (ObjectNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}
}
