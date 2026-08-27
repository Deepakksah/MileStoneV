using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("UserDefinedEvent")]
public class UserDefinedEventGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToUserGenericObject2(UserDefinedEvent.QueryUserDefinedEvent(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToUserGenericObject2s(UserDefinedEvent.QueryUserDefinedEvents(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "UserDefinedEvent")
		{
			return UserDefinedEvent.QueryUserDefinedEventIds(requestContext);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		UserDefinedEvent userDefinedEvent = ConvertToUserDefinedEvent(properties);
		userDefinedEvent.CustomProperties = customProperties;
		return ConvertToUserGenericObject2(UserDefinedEvent.CreateUserDefinedEvent(requestContext, userDefinedEvent));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		UserDefinedEvent userDefinedEvent = ConvertToUserDefinedEvent(genericObject);
		userDefinedEvent.CustomProperties = customProperties;
		return ConvertToUserGenericObject2(UserDefinedEvent.UpdateUserDefinedEvent(requestContext, userDefinedEvent));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		UserDefinedEvent.DeleteUserDefinedEvent(requestContext, id);
	}

	private static GenericObject2 ConvertToUserGenericObject2(UserDefinedEvent userDefinedEvent)
	{
		if (userDefinedEvent == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", userDefinedEvent.Id));
		list.Add(new ObjectProperty2("Name", userDefinedEvent.Name));
		list.Add(new ObjectProperty2("CreatedDate", userDefinedEvent.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", userDefinedEvent.LastModified));
		list.Add(new ObjectProperty2("Subtype", userDefinedEvent.Subtype));
		return new GenericObject2(userDefinedEvent.Id, "UserDefinedEvent", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToUserGenericObject2s(UserDefinedEvent[] userDefinedEvents)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (UserDefinedEvent userDefinedEvent in userDefinedEvents)
		{
			list.Add(ConvertToUserGenericObject2(userDefinedEvent));
		}
		return list.ToArray();
	}

	private static UserDefinedEvent ConvertToUserDefinedEvent(GenericObject2 genericObject)
	{
		return ConvertToUserDefinedEvent(genericObject.Properties);
	}

	private static UserDefinedEvent ConvertToUserDefinedEvent(ObjectProperty2[] properties)
	{
		UserDefinedEvent userDefinedEvent = new UserDefinedEvent();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			userDefinedEvent.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			userDefinedEvent.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			userDefinedEvent.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			userDefinedEvent.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "Subtype"))
		{
			userDefinedEvent.Subtype = GenericObjectHelper2.GetProperty<string>(properties, "Subtype");
		}
		return userDefinedEvent;
	}
}
