using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("TimeProfile")]
[EntityName("SunClockTimeProfile")]
[EntityName("CalendarTimeProfile")]
public class TimeProfileGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(TimeProfile.QueryTimeProfile(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(TimeProfile.QueryTimeProfiles(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer")
		{
			switch (entityName)
			{
			case "SunClockTimeProfile":
				return TimeProfile.QueryTimeProfileIds(requestContext, TimeProfileType.Sunclock);
			case "CalendarTimeProfile":
				return TimeProfile.QueryTimeProfileIds(requestContext, TimeProfileType.Calendar);
			case "TimeProfile":
				return TimeProfile.QueryTimeProfileIds(requestContext);
			}
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		TimeProfile timeProfile = ConvertToTimeProfile(properties);
		if (entityName == "CalendarTimeProfile")
		{
			timeProfile.Type = TimeProfileType.Calendar;
		}
		else
		{
			if (!(entityName == "SunClockTimeProfile"))
			{
				throw new ArgumentException("Time Profile Create: EntityName needs to be SunClockTimeProfile or CalendarTimeProfile");
			}
			timeProfile.Type = TimeProfileType.Sunclock;
		}
		timeProfile.CustomProperties = customProperties;
		return ConvertToGenericObject2(TimeProfile.CreateTimeProfile(requestContext, timeProfile));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		TimeProfile timeProfile = ConvertToTimeProfile(genericObject);
		if (genericObject.EntityName == "CalendarTimeProfile")
		{
			timeProfile.Type = TimeProfileType.Calendar;
		}
		else
		{
			if (!(genericObject.EntityName == "SunClockTimeProfile"))
			{
				throw new ArgumentException("Time Profile Create: EntityName needs to be SunClockTimeProfile or CalendarTimeProfile");
			}
			timeProfile.Type = TimeProfileType.Sunclock;
		}
		timeProfile.CustomProperties = customProperties;
		return ConvertToGenericObject2(TimeProfile.UpdateTimeProfile(requestContext, timeProfile));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		TimeProfile.DeleteTimeProfile(requestContext, id);
	}

	private static GenericObject2 ConvertToGenericObject2(TimeProfile timeProfile)
	{
		if (timeProfile != null)
		{
			List<ObjectProperty2> list = new List<ObjectProperty2>();
			list.Add(new ObjectProperty2("Id", timeProfile.Id));
			list.Add(new ObjectProperty2("Name", timeProfile.Name));
			list.Add(new ObjectProperty2("Description", timeProfile.Description));
			list.Add(new ObjectProperty2("CreatedDate", timeProfile.CreatedDate));
			list.Add(new ObjectProperty2("LastModified", timeProfile.LastModified));
			list.Add(new ObjectProperty2("DefinitionXml", timeProfile.DefinitionXml));
			if (timeProfile.Type == TimeProfileType.Calendar)
			{
				return new GenericObject2(timeProfile.Id, "CalendarTimeProfile", list.ToArray(), null);
			}
			if (timeProfile.Type == TimeProfileType.Sunclock)
			{
				return new GenericObject2(timeProfile.Id, "SunClockTimeProfile", list.ToArray(), null);
			}
		}
		return null;
	}

	private static GenericObject2[] ConvertToGenericObject2s(TimeProfile[] timeProfiles)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (TimeProfile timeProfile in timeProfiles)
		{
			list.Add(ConvertToGenericObject2(timeProfile));
		}
		return list.ToArray();
	}

	private static TimeProfile ConvertToTimeProfile(GenericObject2 genericObject)
	{
		return ConvertToTimeProfile(genericObject.Properties);
	}

	private static TimeProfile ConvertToTimeProfile(ObjectProperty2[] properties)
	{
		TimeProfile timeProfile = new TimeProfile();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			timeProfile.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			timeProfile.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			timeProfile.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			timeProfile.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			timeProfile.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "DefinitionXml"))
		{
			timeProfile.DefinitionXml = GenericObjectHelper2.GetProperty<string>(properties, "DefinitionXml");
		}
		return timeProfile;
	}
}
