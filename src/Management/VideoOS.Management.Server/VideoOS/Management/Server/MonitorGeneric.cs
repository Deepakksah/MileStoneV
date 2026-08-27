using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("Monitor")]
public class MonitorGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(Monitor.QueryMonitor(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(Monitor.QueryMonitorsByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "VideoWall" && entityName == "Monitor")
		{
			return Monitor.QueryMonitorIds(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		Monitor monitor = ConvertToMonitor(properties);
		monitor.CustomProperties = customProperties;
		string property = GenericObjectHelper2.GetProperty<string>(properties, "VideoWallId");
		return ConvertToGenericObject2(Monitor.CreateMonitor(requestContext, property, monitor));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		Monitor monitor = ConvertToMonitor(genericObject);
		monitor.CustomProperties = customProperties;
		return ConvertToGenericObject2(Monitor.UpdateMonitor(requestContext, monitor));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		Monitor.DeleteMonitor(requestContext, id);
	}

	public static GenericObject2 ApplyLayout(IGeminiRequestContext requestContext, string monitorId, string layoutId)
	{
		return ConvertToGenericObject2(Monitor.ApplyLayout(requestContext, monitorId, layoutId));
	}

	public static GenericObject2 ApplyCamerasToMonitorView(IGeminiRequestContext requestContext, string monitorId, string[] cameras, string[] cameraGroups, int fromPosition)
	{
		return ConvertToGenericObject2(Monitor.ApplyCamerasToMonitorView(requestContext, monitorId, cameras, cameraGroups, fromPosition));
	}

	public static GenericObject2 ApplyCamerasAndLayoutToMonitorView(IGeminiRequestContext requestContext, string monitorId, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId)
	{
		return ConvertToGenericObject2(Monitor.ApplyCamerasAndLayoutToMonitorView(requestContext, monitorId, cameras, cameraGroups, fromPosition, layoutId));
	}

	public static GenericObject2 ApplyViewItemXmlsToMonitorView(IGeminiRequestContext requestContext, string monitorId, string[] viewItemXmls, int fromPosition)
	{
		return ConvertToGenericObject2(Monitor.ApplyViewItemXmlsToMonitorView(requestContext, monitorId, viewItemXmls, fromPosition));
	}

	public static void ApplyMonitorStateToMonitor(IGeminiRequestContext requestContext, string monitorId, string xmlState)
	{
		Monitor.ApplyMonitorStateToMonitor(requestContext, monitorId, xmlState);
	}

	public static GenericObject2 ApplyTextAndLayoutToMonitorView(IGeminiRequestContext requestContext, string monitorId, string messageToDisplay, int fromPosition, string layoutId)
	{
		return ConvertToGenericObject2(Monitor.ApplyTextAndLayoutToMonitorView(requestContext, monitorId, messageToDisplay, fromPosition, layoutId));
	}

	public static void RemoveCamerasFromMonitorView(IGeminiRequestContext requestContext, string monitorId, string[] cameras, string[] cameraGroups)
	{
		Monitor.RemoveCamerasFromMonitorView(requestContext, monitorId, cameras, cameraGroups);
	}

	private static GenericObject2 ConvertToGenericObject2(Monitor monitor)
	{
		if (monitor == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", monitor.Id));
		list.Add(new ObjectProperty2("Name", monitor.Name));
		list.Add(new ObjectProperty2("Description", monitor.Description));
		list.Add(new ObjectProperty2("CreatedDate", monitor.CreatedDate));
		list.Add(new ObjectProperty2("LastModified", monitor.LastModified));
		list.Add(new ObjectProperty2("ViewId", monitor.ViewId));
		list.Add(new ObjectProperty2("MonitorSize", monitor.MonitorSize));
		list.Add(new ObjectProperty2("LocationX", monitor.LocationX));
		list.Add(new ObjectProperty2("LocationY", monitor.LocationY));
		list.Add(new ObjectProperty2("AspectRatio", monitor.AspectRatio.ToString()));
		list.Add(new ObjectProperty2("InsertionMethod", monitor.InsertionMethod.ToString()));
		list.Add(new ObjectProperty2("NoLayout", monitor.NoLayout.ToString()));
		list.Add(new ObjectProperty2("EmptyViewItems", monitor.EmptyViewItems.ToString()));
		list.Add(new ObjectProperty2("VideoWallId", monitor.VideoWallId));
		list.Add(new ObjectProperty2("MonitorState", monitor.MonitorState));
		List<RelationProperty> list2 = new List<RelationProperty>();
		list2.Add(new RelationProperty("VideoWall", monitor.VideoWallId));
		return new GenericObject2(monitor.Id, "Monitor", list.ToArray(), list2.ToArray());
	}

	private static GenericObject2[] ConvertToGenericObject2s(Monitor[] monitors)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (Monitor monitor in monitors)
		{
			list.Add(ConvertToGenericObject2(monitor));
		}
		return list.ToArray();
	}

	private static Monitor ConvertToMonitor(GenericObject2 genericObject)
	{
		return ConvertToMonitor(genericObject.Properties);
	}

	private static Monitor ConvertToMonitor(ObjectProperty2[] properties)
	{
		Monitor monitor = new Monitor();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			monitor.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			monitor.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			monitor.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			monitor.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			monitor.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "AspectRatio"))
		{
			monitor.AspectRatio = GenericObjectHelper2.GetProperty<AspectRatio>(properties, "AspectRatio");
		}
		if (GenericObjectHelper2.Contains(properties, "ViewId"))
		{
			monitor.ViewId = GenericObjectHelper2.GetProperty<string>(properties, "ViewId");
		}
		if (GenericObjectHelper2.Contains(properties, "MonitorSize"))
		{
			monitor.MonitorSize = GenericObjectHelper2.GetProperty<float>(properties, "MonitorSize");
		}
		if (GenericObjectHelper2.Contains(properties, "LocationX"))
		{
			monitor.LocationX = GenericObjectHelper2.GetProperty<double>(properties, "LocationX");
		}
		if (GenericObjectHelper2.Contains(properties, "LocationY"))
		{
			monitor.LocationY = GenericObjectHelper2.GetProperty<double>(properties, "LocationY");
		}
		if (GenericObjectHelper2.Contains(properties, "InsertionMethod"))
		{
			monitor.InsertionMethod = GenericObjectHelper2.GetProperty<ElementInsertionMethod>(properties, "InsertionMethod");
		}
		if (GenericObjectHelper2.Contains(properties, "NoLayout"))
		{
			monitor.NoLayout = GenericObjectHelper2.GetProperty<PresetLayoutMethod>(properties, "NoLayout");
		}
		if (GenericObjectHelper2.Contains(properties, "EmptyViewItems"))
		{
			monitor.EmptyViewItems = GenericObjectHelper2.GetProperty<PresetLayoutMethod>(properties, "EmptyViewItems");
		}
		if (GenericObjectHelper2.Contains(properties, "VideoWallId"))
		{
			monitor.VideoWallId = GenericObjectHelper2.GetProperty<string>(properties, "VideoWallId");
		}
		if (GenericObjectHelper2.Contains(properties, "MonitorState"))
		{
			monitor.MonitorState = GenericObjectHelper2.GetProperty<string>(properties, "MonitorState");
		}
		return monitor;
	}
}
