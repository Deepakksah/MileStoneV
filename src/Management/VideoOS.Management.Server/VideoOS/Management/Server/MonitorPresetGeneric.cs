using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("MonitorPreset")]
public class MonitorPresetGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(MonitorPreset.QueryMonitorPreset(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(MonitorPreset.QueryMonitorPresetsByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "Monitor" && entityName == "MonitorPreset")
		{
			return MonitorPreset.QueryMonitorPresetIdsByMonitorId(requestContext, parentId);
		}
		if (parentEntityName == "VideoWallPreset" && entityName == "MonitorPreset")
		{
			return MonitorPreset.QueryMonitorPresetIdsByVideoWallPresetId(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		MonitorPreset monitorPreset = ConvertToMonitorPreset(genericObject);
		return ConvertToGenericObject2(MonitorPreset.UpdateMonitorPreset(requestContext, monitorPreset.Id, monitorPreset.DefinitionXml, customProperties));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}

	private static GenericObject2 ConvertToGenericObject2(MonitorPreset monitorPreset)
	{
		if (monitorPreset == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", monitorPreset.Id));
		list.Add(new ObjectProperty2("DefinitionXml", monitorPreset.DefinitionXml));
		list.Add(new ObjectProperty2("LastModified", monitorPreset.LastModified));
		list.Add(new ObjectProperty2("MonitorId", monitorPreset.MonitorId));
		list.Add(new ObjectProperty2("VideoWallPresetId", monitorPreset.VideoWallPresetId));
		list.Add(new ObjectProperty2("VideoWallId", monitorPreset.VideoWallId));
		List<RelationProperty> list2 = new List<RelationProperty>();
		list2.Add(new RelationProperty("Monitor", monitorPreset.MonitorId));
		list2.Add(new RelationProperty("VideoWallPreset", monitorPreset.VideoWallPresetId));
		list2.Add(new RelationProperty("VideoWall", monitorPreset.VideoWallId));
		return new GenericObject2(monitorPreset.Id, "MonitorPreset", list.ToArray(), list2.ToArray());
	}

	private static GenericObject2[] ConvertToGenericObject2s(MonitorPreset[] monitorPresets)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (MonitorPreset monitorPreset in monitorPresets)
		{
			list.Add(ConvertToGenericObject2(monitorPreset));
		}
		return list.ToArray();
	}

	private static MonitorPreset ConvertToMonitorPreset(GenericObject2 genericObject)
	{
		return ConvertToMonitorPreset(genericObject.Properties);
	}

	private static MonitorPreset ConvertToMonitorPreset(ObjectProperty2[] properties)
	{
		MonitorPreset monitorPreset = new MonitorPreset();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			monitorPreset.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "DefinitionXml"))
		{
			monitorPreset.DefinitionXml = GenericObjectHelper2.GetProperty<string>(properties, "DefinitionXml");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			monitorPreset.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "MonitorId"))
		{
			monitorPreset.MonitorId = GenericObjectHelper2.GetProperty<string>(properties, "MonitorId");
		}
		if (GenericObjectHelper2.Contains(properties, "VideoWallPresetId"))
		{
			monitorPreset.VideoWallPresetId = GenericObjectHelper2.GetProperty<string>(properties, "VideoWallPresetId");
		}
		if (GenericObjectHelper2.Contains(properties, "VideoWallId"))
		{
			monitorPreset.VideoWallId = GenericObjectHelper2.GetProperty<string>(properties, "VideoWallId");
		}
		return monitorPreset;
	}
}
