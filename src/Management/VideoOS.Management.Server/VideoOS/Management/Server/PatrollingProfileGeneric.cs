using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("PatrollingProfile")]
public class PatrollingProfileGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		PatrollingProfile[] array = PatrollingProfile.QueryPatrollingProfiles(requestContext, new string[1] { id });
		if (array != null && array.Length == 1)
		{
			return ConvertToGenericObject2(array[0]);
		}
		return null;
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(PatrollingProfile.QueryPatrollingProfiles(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		return (from f in PatrollingProfile.QueryPatrollingProfiles(requestContext, parentId)
			select f.Id).ToArray();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		PatrollingProfile patrollingProfile = ConvertToPatrollingProfile(properties);
		patrollingProfile.CustomProperties = customProperties;
		return ConvertToGenericObject2(PatrollingProfile.CreatePatrollingProfile(requestContext, patrollingProfile));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		PatrollingProfile.DeletePatrollingProfile(requestContext, id);
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		PatrollingProfile patrollingProfile = ConvertToPatrollingProfile(genericObject);
		patrollingProfile.CustomProperties = customProperties;
		return ConvertToGenericObject2(PatrollingProfile.UpdatePatrollingProfile(requestContext, patrollingProfile));
	}

	private static GenericObject2 ConvertToGenericObject2(PatrollingProfile patrollingProfile)
	{
		if (patrollingProfile != null)
		{
			List<ObjectProperty2> list = new List<ObjectProperty2>();
			list.Add(new ObjectProperty2("Id", patrollingProfile.Id));
			list.Add(new ObjectProperty2("Name", patrollingProfile.Name));
			list.Add(new ObjectProperty2("LastModified", patrollingProfile.LastModified));
			list.Add(new ObjectProperty2("Description", patrollingProfile.Description));
			list.Add(new ObjectProperty2("CameraDeviceId", patrollingProfile.CameraDeviceId));
			list.Add(new ObjectProperty2("DefinitionXml", patrollingProfile.DefinitionXml));
			List<RelationProperty> list2 = new List<RelationProperty>();
			list2.Add(new RelationProperty("CameraDevice", patrollingProfile.CameraDeviceId));
			return new GenericObject2(patrollingProfile.Id, "PatrollingProfile", list.ToArray(), list2.ToArray());
		}
		return null;
	}

	private static GenericObject2[] ConvertToGenericObject2s(PatrollingProfile[] patrollingProfiles)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (PatrollingProfile patrollingProfile in patrollingProfiles)
		{
			if (patrollingProfile != null)
			{
				list.Add(ConvertToGenericObject2(patrollingProfile));
			}
		}
		return list.ToArray();
	}

	private static PatrollingProfile ConvertToPatrollingProfile(GenericObject2 genericObject)
	{
		return ConvertToPatrollingProfile(genericObject.Properties);
	}

	private static PatrollingProfile ConvertToPatrollingProfile(ObjectProperty2[] properties)
	{
		PatrollingProfile patrollingProfile = new PatrollingProfile();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			patrollingProfile.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			patrollingProfile.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			patrollingProfile.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			patrollingProfile.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "CameraDeviceId"))
		{
			patrollingProfile.CameraDeviceId = GenericObjectHelper2.GetProperty<string>(properties, "CameraDeviceId");
		}
		if (GenericObjectHelper2.Contains(properties, "DefinitionXml"))
		{
			patrollingProfile.DefinitionXml = GenericObjectHelper2.GetProperty<string>(properties, "DefinitionXml");
		}
		return patrollingProfile;
	}
}
