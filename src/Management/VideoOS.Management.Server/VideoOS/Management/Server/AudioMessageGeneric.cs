using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("AudioMessage")]
[FunctionHandler("SetAudioMessageContent")]
[FunctionHandler("GetAudioMessageContent")]
public class AudioMessageGeneric : IGenericObjectSupport2, IGenericFunction
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(AudioMessage.QueryAudioMessageById(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToObject2s(AudioMessage.QueryAudioMessagesByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "AudioMessage")
		{
			return AudioMessage.QueryAudioMessageIds(requestContext);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		AudioMessage audioMessage = ConvertToAudioMessage(properties);
		audioMessage = AudioMessage.CreateAudioMessage(requestContext, audioMessage.Id, audioMessage.Name, audioMessage.Description, audioMessage.OriginalFileName, audioMessage.ContentBase64);
		return ConvertToGenericObject2(audioMessage);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		AudioMessage audioMessage = ConvertToAudioMessage(genericObject);
		audioMessage = AudioMessage.UpdateAudioMessage(requestContext, audioMessage.Id, audioMessage.Name, audioMessage.Description);
		return ConvertToGenericObject2(audioMessage);
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		AudioMessage.DeleteAudioMessage(requestContext, id);
	}

	public GenericObject2[] Execute(IGeminiRequestContext requestContext, string functionName, GenericObject2[] parameters, int clientVersion)
	{
		if (!(functionName == "SetAudioMessageContent"))
		{
			if (functionName == "GetAudioMessageContent")
			{
				return GetAudioMessageContent(requestContext, parameters);
			}
			throw VmoExceptionFactory.VMO60311_VmoFunctionNotSupportedException(functionName ?? "<Null>");
		}
		return SetAudioMessageContent(requestContext, parameters);
	}

	private static GenericObject2[] SetAudioMessageContent(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		string propertyOrDefault = genericObject.Properties.GetPropertyOrDefault<string>("Id");
		string propertyOrDefault2 = genericObject.Properties.GetPropertyOrDefault<string>("OriginalFileName");
		string propertyOrDefault3 = genericObject.Properties.GetPropertyOrDefault<string>("Content");
		AudioMessage.SetMessageContent(requestContext, propertyOrDefault, propertyOrDefault2, propertyOrDefault3);
		return null;
	}

	private static GenericObject2[] GetAudioMessageContent(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		if (!genericObject.Properties.TryGetProperty<string>("Id", out var value))
		{
			value = null;
		}
		string messageContent = AudioMessage.GetMessageContent(requestContext, value);
		ObjectProperty2[] properties = new ObjectProperty2[1]
		{
			new ObjectProperty2("Content", messageContent)
		};
		GenericObject2 genericObject2 = new GenericObject2(value, null, properties, null);
		return new GenericObject2[1] { genericObject2 };
	}

	private static GenericObject2 ConvertToGenericObject2(AudioMessage item)
	{
		if (item == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", item.Id));
		list.Add(new ObjectProperty2("Name", item.Name));
		list.Add(new ObjectProperty2("Description", item.Description));
		list.Add(new ObjectProperty2("LastModified", item.LastModified));
		list.Add(new ObjectProperty2("OriginalFileName", item.OriginalFileName));
		return new GenericObject2(item.Id, "AudioMessage", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToObject2s(AudioMessage[] items)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (AudioMessage item in items)
		{
			list.Add(ConvertToGenericObject2(item));
		}
		return list.ToArray();
	}

	private static AudioMessage ConvertToAudioMessage(GenericObject2 genericObject)
	{
		return ConvertToAudioMessage(genericObject.Properties);
	}

	private static AudioMessage ConvertToAudioMessage(ObjectProperty2[] properties)
	{
		AudioMessage audioMessage = new AudioMessage();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			audioMessage.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			audioMessage.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			audioMessage.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			audioMessage.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "OriginalFileName"))
		{
			audioMessage.OriginalFileName = GenericObjectHelper2.GetProperty<string>(properties, "OriginalFileName");
		}
		if (GenericObjectHelper2.Contains(properties, "Content"))
		{
			audioMessage.ContentBase64 = GenericObjectHelper2.GetProperty<string>(properties, "Content");
		}
		return audioMessage;
	}
}
