using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("VideoWall")]
public class VideoWallGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(VideoWall.QueryVideoWall(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(VideoWall.QueryVideoWalls(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "VideoWall")
		{
			return VideoWall.QueryVideoWallIds(requestContext);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		VideoWall videoWall = ConvertToVideoWall(properties);
		videoWall.CustomProperties = customProperties;
		return ConvertToGenericObject2(VideoWall.CreateVideoWall(requestContext, videoWall));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		VideoWall videoWall = ConvertToVideoWall(genericObject);
		videoWall.CustomProperties = customProperties;
		return ConvertToGenericObject2(VideoWall.UpdateVideoWall(requestContext, videoWall));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		VideoWall.DeleteVideoWall(requestContext, id);
	}

	private static GenericObject2 ConvertToGenericObject2(VideoWall videoWall)
	{
		if (videoWall == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", videoWall.Id));
		list.Add(new ObjectProperty2("Name", videoWall.Name));
		list.Add(new ObjectProperty2("Description", videoWall.Description));
		list.Add(new ObjectProperty2("CreatedDate", videoWall.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", videoWall.LastModified));
		list.Add(new ObjectProperty2("Height", videoWall.Height));
		list.Add(new ObjectProperty2("StatusText", videoWall.StatusText));
		list.Add(new ObjectProperty2("TitleBar", videoWall.TitleBar.ToString()));
		list.Add(new ObjectProperty2("Width", videoWall.Width));
		return new GenericObject2(videoWall.Id, "VideoWall", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToGenericObject2s(VideoWall[] videoWalls)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (VideoWall videoWall in videoWalls)
		{
			list.Add(ConvertToGenericObject2(videoWall));
		}
		return list.ToArray();
	}

	private static VideoWall ConvertToVideoWall(GenericObject2 genericObject)
	{
		return ConvertToVideoWall(genericObject.Properties);
	}

	private static VideoWall ConvertToVideoWall(ObjectProperty2[] properties)
	{
		VideoWall videoWall = new VideoWall();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			videoWall.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			videoWall.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			videoWall.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			videoWall.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			videoWall.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "Height"))
		{
			videoWall.Height = GenericObjectHelper2.GetProperty<double>(properties, "Height");
		}
		if (GenericObjectHelper2.Contains(properties, "StatusText"))
		{
			videoWall.StatusText = GenericObjectHelper2.GetProperty<bool>(properties, "StatusText");
		}
		if (GenericObjectHelper2.Contains(properties, "TitleBar"))
		{
			videoWall.TitleBar = GenericObjectHelper2.GetProperty<TitleBar>(properties, "TitleBar");
		}
		if (GenericObjectHelper2.Contains(properties, "Width"))
		{
			videoWall.Width = GenericObjectHelper2.GetProperty<double>(properties, "Width");
		}
		return videoWall;
	}
}
