using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("VideoWallPreset")]
public class VideoWallPresetGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(VideoWallPreset.QueryVideoWallPreset(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(VideoWallPreset.QueryVideoWallPresetsByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "VideoWall" && entityName == "VideoWallPreset")
		{
			return VideoWallPreset.QueryVideoWallPresetIds(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		VideoWallPreset videoWallPreset = ConvertToVideoWallPreset(properties);
		videoWallPreset.CustomProperties = customProperties;
		string property = GenericObjectHelper2.GetProperty<string>(properties, "VideoWallId");
		return ConvertToGenericObject2(VideoWallPreset.CreateVideoWallPreset(requestContext, property, videoWallPreset));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		VideoWallPreset videoWallPreset = ConvertToVideoWallPreset(genericObject);
		videoWallPreset.CustomProperties = customProperties;
		return ConvertToGenericObject2(VideoWallPreset.UpdateVideoWallPreset(requestContext, videoWallPreset));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		VideoWallPreset.DeleteVideoWallPreset(requestContext, id);
	}

	private static GenericObject2 ConvertToGenericObject2(VideoWallPreset videoWallPreset)
	{
		if (videoWallPreset == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", videoWallPreset.Id));
		list.Add(new ObjectProperty2("Name", videoWallPreset.Name));
		list.Add(new ObjectProperty2("Description", videoWallPreset.Description));
		list.Add(new ObjectProperty2("CreatedDate", videoWallPreset.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", videoWallPreset.LastModified));
		list.Add(new ObjectProperty2("VideoWallId", videoWallPreset.VideoWallId));
		List<RelationProperty> list2 = new List<RelationProperty>();
		list2.Add(new RelationProperty("VideoWall", videoWallPreset.VideoWallId));
		return new GenericObject2(videoWallPreset.Id, "VideoWallPreset", list.ToArray(), list2.ToArray());
	}

	private static GenericObject2[] ConvertToGenericObject2s(VideoWallPreset[] videoWallPresets)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (VideoWallPreset videoWallPreset in videoWallPresets)
		{
			list.Add(ConvertToGenericObject2(videoWallPreset));
		}
		return list.ToArray();
	}

	private static VideoWallPreset ConvertToVideoWallPreset(GenericObject2 genericObject)
	{
		return ConvertToVideoWallPreset(genericObject.Properties);
	}

	private static VideoWallPreset ConvertToVideoWallPreset(ObjectProperty2[] properties)
	{
		VideoWallPreset videoWallPreset = new VideoWallPreset();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			videoWallPreset.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			videoWallPreset.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			videoWallPreset.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			videoWallPreset.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			videoWallPreset.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "VideoWallId"))
		{
			videoWallPreset.VideoWallId = GenericObjectHelper2.GetProperty<string>(properties, "VideoWallId");
		}
		return videoWallPreset;
	}
}
