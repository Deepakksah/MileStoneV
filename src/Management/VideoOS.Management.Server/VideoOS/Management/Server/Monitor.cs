using System;
using System.Collections.Generic;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class Monitor
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string ViewId { get; set; }

	public float MonitorSize { get; set; }

	public double LocationX { get; set; }

	public double LocationY { get; set; }

	public AspectRatio AspectRatio { get; set; }

	public ElementInsertionMethod InsertionMethod { get; set; }

	public PresetLayoutMethod NoLayout { get; set; }

	public PresetLayoutMethod EmptyViewItems { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public string VideoWallId { get; set; }

	public string MonitorState { get; set; }

	public static Monitor[] QueryMonitorsByIds(IGeminiRequestContext requestContext, string[] monitorIds)
	{
		using IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent();
		return FilterMonitorsBasedOnPermissions(requestContext, monitorComponent.QueryMonitorsByIds(monitorIds));
	}

	public static string[] QueryMonitorIds(IGeminiRequestContext requestContext, string videoWallId)
	{
		string[] result = new string[0];
		if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", videoWallId))
		{
			using IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent();
			result = monitorComponent.QueryMonitorIds(videoWallId);
		}
		return result;
	}

	public static Monitor QueryMonitor(IGeminiRequestContext requestContext, string monitorId)
	{
		Validation.CheckObjectId(ref monitorId, "monitorId", allowNullOrEmpty: false);
		using IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent();
		Monitor monitor = monitorComponent.QueryMonitor(monitorId);
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", monitor.VideoWallId))
		{
			throw new MonitorNotFoundException();
		}
		return monitor;
	}

	public static Monitor CreateMonitor(IGeminiRequestContext requestContext, string videoWallId, Monitor newMonitor)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		newMonitor.Id = GeminiResourceComponent.GetGuid(newMonitor.Id);
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref videoWallId, "videoWallId", allowNullOrEmpty: false);
		Validation.CheckName(newMonitor.Name, "name");
		string description = newMonitor.Description;
		Validation.CheckDescription(ref description, "description");
		newMonitor.Description = description;
		Validation.CheckAspectRatio(newMonitor.AspectRatio, "aspectRatio");
		Validation.CheckElementInsertionMethod(newMonitor.InsertionMethod, "insertionMethod");
		Validation.CheckPresetLayoutMethod(newMonitor.NoLayout, "noLayout");
		Validation.CheckPresetLayoutMethod(newMonitor.EmptyViewItems, "emptyViewItems");
		try
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_WRITE", videoWallId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new VideoWallNotFoundException();
		}
		using (MonitorComponent monitorComponent = requestContext.CreateComponent<MonitorComponent>())
		{
			monitorComponent.CreateMonitor(videoWallId, newMonitor, sessionId);
			newMonitor = FindMonitor(requestContext, newMonitor.Id);
			Integration.FireMonitorChanged(requestContext, newMonitor.Id, videoWallId, EventAction.Added);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return newMonitor;
	}

	public static Monitor UpdateMonitor(IGeminiRequestContext requestContext, Monitor monitor)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		Guid sessionId = Guid.NewGuid();
		string objectId = monitor.Id;
		Validation.CheckObjectId(ref objectId, "monitorId", allowNullOrEmpty: false);
		Validation.CheckName(monitor.Name, "name");
		string description = monitor.Description;
		Validation.CheckDescription(ref description, "description");
		monitor.Description = description;
		string objectId2 = monitor.ViewId;
		Validation.CheckObjectId(ref objectId2, "viewId", allowNullOrEmpty: false);
		Validation.CheckAspectRatio(monitor.AspectRatio, "aspectRatio");
		Validation.CheckElementInsertionMethod(monitor.InsertionMethod, "insertionMethod");
		Validation.CheckPresetLayoutMethod(monitor.NoLayout, "noLayout");
		Validation.CheckPresetLayoutMethod(monitor.EmptyViewItems, "emptyViewItems");
		Validation.CheckMonitorState(monitor.MonitorState, "MonitorState");
		using (MonitorComponent monitorComponent = requestContext.CreateComponent<MonitorComponent>())
		{
			string text = monitorComponent.QueryVideoWallId(monitor.Id);
			if (string.IsNullOrEmpty(text))
			{
				throw new MonitorNotFoundException();
			}
			try
			{
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_WRITE", text);
			}
			catch (SecurityObjectDoesNotExistException)
			{
				throw new MonitorNotFoundException();
			}
			monitorComponent.UpdateMonitor(monitor, sessionId);
			monitor = FindMonitor(requestContext, monitor.Id);
			Integration.FireMonitorChanged(requestContext, monitor.Id, text, EventAction.Updated);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return monitor;
	}

	public static void DeleteMonitor(IGeminiRequestContext requestContext, string monitorId)
	{
		Guid sessionId = Guid.NewGuid();
		using (MonitorComponent monitorComponent = requestContext.CreateComponent<MonitorComponent>())
		{
			string text = monitorComponent.QueryVideoWallId(monitorId);
			if (string.IsNullOrEmpty(text))
			{
				throw new MonitorNotFoundException();
			}
			try
			{
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_WRITE", text);
			}
			catch (SecurityObjectDoesNotExistException)
			{
				throw new MonitorNotFoundException();
			}
			monitorComponent.DeleteMonitor(monitorId, sessionId.ToString());
			Integration.FireMonitorChanged(requestContext, monitorId, text, EventAction.Deleted);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	public static Monitor ApplyLayout(IGeminiRequestContext requestContext, string monitorId, string layoutId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref monitorId, "monitorId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref layoutId, "layoutId", allowNullOrEmpty: false);
		Monitor monitor;
		string videoWallId;
		using (requestContext.CreateComponent<MonitorComponent>())
		{
			monitor = FindMonitor(requestContext, monitorId);
			if (monitor == null)
			{
				throw new MonitorNotFoundException();
			}
			videoWallId = monitor.VideoWallId;
		}
		try
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
			requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new MonitorNotFoundException();
		}
		using (VideoWallFunctionalityComponent videoWallFunctionalityComponent = requestContext.CreateComponent<VideoWallFunctionalityComponent>())
		{
			videoWallFunctionalityComponent.ApplyLayoutToMonitorView(monitor, layoutId, sessionId);
			Integration.FireMonitorViewChanged(requestContext, monitor.ViewId, monitor.Id, videoWallId, EventAction.Updated);
		}
		using (requestContext.CreateComponent<MonitorComponent>())
		{
			monitor = FindMonitor(requestContext, monitorId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return monitor;
	}

	public static Monitor ApplyCamerasToMonitorView(IGeminiRequestContext requestContext, string monitorId, string[] cameras, string[] cameraGroups, int fromPosition)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref monitorId, "monitorId", allowNullOrEmpty: false);
		Monitor monitor;
		string videoWallId;
		using (requestContext.CreateComponent<MonitorComponent>())
		{
			monitor = FindMonitor(requestContext, monitorId);
			if (monitor == null)
			{
				throw new MonitorNotFoundException();
			}
			videoWallId = monitor.VideoWallId;
		}
		try
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
			requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new MonitorNotFoundException();
		}
		using (VideoWallFunctionalityComponent videoWallFunctionalityComponent = requestContext.CreateComponent<VideoWallFunctionalityComponent>())
		{
			videoWallFunctionalityComponent.ApplyCamerasToMonitorView(monitor, cameras, cameraGroups, fromPosition, sessionId);
			Integration.FireMonitorViewChanged(requestContext, monitor.ViewId, monitor.Id, videoWallId, EventAction.Updated);
		}
		using (requestContext.CreateComponent<MonitorComponent>())
		{
			monitor = FindMonitor(requestContext, monitor.Id);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return monitor;
	}

	public static Monitor ApplyCamerasAndLayoutToMonitorView(IGeminiRequestContext requestContext, string monitorId, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref monitorId, "monitorId", allowNullOrEmpty: false);
		Monitor monitor;
		string videoWallId;
		using (requestContext.CreateComponent<MonitorComponent>())
		{
			monitor = FindMonitor(requestContext, monitorId);
			if (monitor == null)
			{
				throw new MonitorNotFoundException();
			}
			videoWallId = monitor.VideoWallId;
		}
		try
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
			requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new MonitorNotFoundException();
		}
		using (VideoWallFunctionalityComponent videoWallFunctionalityComponent = requestContext.CreateComponent<VideoWallFunctionalityComponent>())
		{
			videoWallFunctionalityComponent.ApplyCamerasAndLayoutToMonitorView(monitor, cameras, cameraGroups, fromPosition, layoutId, sessionId);
			Integration.FireMonitorViewChanged(requestContext, monitor.ViewId, monitor.Id, videoWallId, EventAction.Updated);
		}
		using (requestContext.CreateComponent<MonitorComponent>())
		{
			monitor = FindMonitor(requestContext, monitor.Id);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return monitor;
	}

	public static Monitor ApplyViewItemXmlsToMonitorView(IGeminiRequestContext requestContext, string monitorId, string[] viewItemXmls, int fromPosition)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref monitorId, "monitorId", allowNullOrEmpty: false);
		Monitor monitor;
		string videoWallId;
		using (requestContext.CreateComponent<MonitorComponent>())
		{
			monitor = FindMonitor(requestContext, monitorId);
			if (monitor == null)
			{
				throw new MonitorNotFoundException();
			}
			videoWallId = monitor.VideoWallId;
		}
		try
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
			requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new MonitorNotFoundException();
		}
		using (VideoWallFunctionalityComponent videoWallFunctionalityComponent = requestContext.CreateComponent<VideoWallFunctionalityComponent>())
		{
			videoWallFunctionalityComponent.ApplyViewItemsToMonitorView(monitor, viewItemXmls, fromPosition, sessionId);
			Integration.FireMonitorViewChanged(requestContext, monitor.ViewId, monitor.Id, videoWallId, EventAction.Updated);
		}
		using (requestContext.CreateComponent<MonitorComponent>())
		{
			monitor = FindMonitor(requestContext, monitor.Id);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return monitor;
	}

	public static void ApplyMonitorStateToMonitor(IGeminiRequestContext requestContext, string monitorId, string xmlState)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		Validation.CheckObjectId(ref monitorId, "monitorId", allowNullOrEmpty: false);
		Monitor monitor;
		string videoWallId;
		using (MonitorComponent monitorComponent = requestContext.CreateComponent<MonitorComponent>())
		{
			monitor = FindMonitor(requestContext, monitorId);
			if (monitor == null)
			{
				throw new MonitorNotFoundException();
			}
			videoWallId = monitor.VideoWallId;
			try
			{
				requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "PLAYBACK", videoWallId);
			}
			catch (SecurityObjectDoesNotExistException)
			{
				throw new MonitorNotFoundException();
			}
			monitorComponent.UpdateMonitorState(monitorId, xmlState);
		}
		Integration.FireMonitorChanged(requestContext, monitor.Id, videoWallId, EventAction.Updated);
	}

	public static Monitor ApplyTextAndLayoutToMonitorView(IGeminiRequestContext requestContext, string monitorId, string messageToDisplay, int fromPosition, string layoutId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref monitorId, "monitorId", allowNullOrEmpty: false);
		Monitor monitor = FindMonitor(requestContext, monitorId);
		if (monitor == null)
		{
			throw new MonitorNotFoundException();
		}
		string videoWallId = monitor.VideoWallId;
		try
		{
			SecurityManagerService service = requestContext.GetService<SecurityManagerService>();
			service.CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
			service.CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new MonitorNotFoundException();
		}
		using (VideoWallFunctionalityComponent videoWallFunctionalityComponent = requestContext.CreateComponent<VideoWallFunctionalityComponent>())
		{
			videoWallFunctionalityComponent.ApplyTextAndLayoutToMonitorView(monitor, messageToDisplay, fromPosition, layoutId, sessionId);
			Integration.FireMonitorViewChanged(requestContext, monitor.ViewId, monitor.Id, videoWallId, EventAction.Updated);
		}
		monitor = FindMonitor(requestContext, monitor.Id);
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return monitor;
	}

	public static void RemoveCamerasFromMonitorView(IGeminiRequestContext requestContext, string monitorId, string[] cameras, string[] cameraGroups)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref monitorId, "monitorId", allowNullOrEmpty: false);
		Monitor monitor;
		string videoWallId;
		using (requestContext.CreateComponent<MonitorComponent>())
		{
			monitor = FindMonitor(requestContext, monitorId);
			if (monitor == null)
			{
				throw new MonitorNotFoundException();
			}
			videoWallId = monitor.VideoWallId;
		}
		try
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
			requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new MonitorNotFoundException();
		}
		using (VideoWallFunctionalityComponent videoWallFunctionalityComponent = requestContext.CreateComponent<VideoWallFunctionalityComponent>())
		{
			videoWallFunctionalityComponent.RemoveCamerasFromMonitorView(monitor, cameras, cameraGroups, sessionId);
			Integration.FireMonitorViewChanged(requestContext, monitor.ViewId, monitor.Id, videoWallId, EventAction.Updated);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	public static string GetIdFromPreset(IGeminiRequestContext requestContext, string monitorPresetId)
	{
		using IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent();
		return monitorComponent.QueryMonitorIdFromPreset(monitorPresetId);
	}

	private static Monitor FindMonitor(IGeminiRequestContext requestContext, string monitorId)
	{
		using IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent();
		return monitorComponent.QueryMonitor(monitorId);
	}

	private static Monitor[] FilterMonitorsBasedOnPermissions(IGeminiRequestContext requestContext, Monitor[] monitorArray)
	{
		List<Monitor> list = new List<Monitor>();
		foreach (Monitor monitor in monitorArray)
		{
			if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", monitor.VideoWallId))
			{
				list.Add(monitor);
			}
		}
		return list.ToArray();
	}
}
