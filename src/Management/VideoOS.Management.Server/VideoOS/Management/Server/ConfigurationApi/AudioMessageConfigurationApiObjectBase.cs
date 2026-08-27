using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;

namespace VideoOS.Management.Server.ConfigurationApi;

public abstract class AudioMessageConfigurationApiObjectBase : ConfigurationApiBase, IConfigurationApiSupport
{
	private class DefaultRepository : IAudioMessageRepository
	{
		public static IAudioMessageRepository Instance { get; } = new DefaultRepository();

		private DefaultRepository()
		{
		}

		public AudioMessage QueryAudioMessageById(IGeminiRequestContext requestContext, string id)
		{
			return AudioMessage.QueryAudioMessageById(requestContext, id);
		}

		public AudioMessage[] QueryAudioMessages(IGeminiRequestContext requestContext)
		{
			return AudioMessage.QueryAudioMessages(requestContext);
		}

		public AudioMessage UpdateAudioMessage(IGeminiRequestContext requestContext, string id, string name, string description)
		{
			return AudioMessage.UpdateAudioMessage(requestContext, id, name, description);
		}

		public void UpdateAudioMessageValidation(IGeminiRequestContext requestContext, string id, string name, string description)
		{
			AudioMessage.UpdateAudioMessageValidation(requestContext, id, name, description);
		}

		public AudioMessage CreateAudioMessage(IGeminiRequestContext requestContext, string id, string name, string description, string originalFileName, string base64Content)
		{
			return AudioMessage.CreateAudioMessage(requestContext, id, name, description, originalFileName, base64Content);
		}

		public void DeleteAudioMessage(IGeminiRequestContext requestContext, string id)
		{
			AudioMessage.DeleteAudioMessage(requestContext, id);
		}

		public string GetMessageContent(IGeminiRequestContext requestContext, string id)
		{
			return AudioMessage.GetMessageContent(requestContext, id);
		}

		public void SetMessageContent(IGeminiRequestContext requestContext, string id, string originalFileName, string base64Content)
		{
			AudioMessage.SetMessageContent(requestContext, id, originalFileName, base64Content);
		}
	}

	protected const string AudioMessageParentPath = "/AudioMessageFolder";

	protected IAudioMessageRepository Repository { get; }

	protected AudioMessageConfigurationApiObjectBase(IAudioMessageRepository repository)
	{
		Repository = repository ?? DefaultRepository.Instance;
	}

	public abstract ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath);

	public abstract ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public abstract ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path);

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		ParamNullCheck(requestContext, "requestContext");
		ParamNullCheck(item, "item");
		ParamNullCheck(path, "path");
		ParamNullCheck(parentPath, "parentPath");
		ParamNullCheck(methodId, "methodId");
		ThrowIfPathInvalid(path);
		ThrowIfParentPathInvalid(parentPath);
		if (!item.MethodIds.Contains(methodId))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid MethodId {0} on ItemType: {1}", methodId, item.ItemType), "methodId");
		}
		if (!string.Equals(item.ItemType, "InvokeInfo", StringComparison.Ordinal))
		{
			return GetMethodInputParameters(requestContext, item, path, methodId);
		}
		return ExecuteMethod(requestContext, item, path, methodId);
	}

	protected abstract ConfigurationItem GetMethodInputParameters(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId);

	protected abstract ConfigurationItem ExecuteMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, string methodId);

	protected static void ParamNullCheck(object param, string name)
	{
		if (param != null)
		{
			return;
		}
		throw new ArgumentNullException(name ?? string.Empty);
	}

	protected static ConfigurationItem ConvertToAudioMessageConfigurationItem(AudioMessage message, bool includeContent)
	{
		ConfigurationItem configurationItem = new ConfigurationItem
		{
			ItemType = "AudioMessage",
			UIHint = "DirectionHorizontal",
			ItemCategory = ItemCategories.Item,
			DisplayName = message.Name,
			ParentPath = "/AudioMessageFolder"
		};
		List<Property> list = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(message.Id),
			GeneralPropertyDefinitions.GetNameProperty(message.Name),
			GeneralPropertyDefinitions.GetDescriptionProperty(message.Description),
			GeneralPropertyDefinitions.GetLastModifiedProperty(message.LastModified),
			AudioMessagePropertyDefinition.OriginalFileNameProperty(message.OriginalFileName)
		};
		if (includeContent)
		{
			list.Add(AudioMessagePropertyDefinition.ContentProperty(message.ContentBase64));
		}
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, message.Id).Text;
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[2] { "GetAudioMessageContent", "SetAudioMessageContent" };
		return configurationItem;
	}

	protected abstract void ThrowIfPathInvalid(ConfigurationItemPath path);

	protected abstract void ThrowIfParentPathInvalid(ConfigurationItemPath path);
}
