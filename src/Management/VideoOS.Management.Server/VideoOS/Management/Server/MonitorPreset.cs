using System;
using System.Collections.Generic;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class MonitorPreset
{
	public string Id { get; set; }

	public string Name { get; set; }

	public DateTime LastModified { get; set; }

	public string DefinitionXml { get; set; }

	public string VideoWallPresetId { get; set; }

	public string VideoWallId { get; set; }

	public string MonitorId { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public static MonitorPreset QueryMonitorPreset(IGeminiRequestContext requestContext, string presetId)
	{
		MonitorPreset monitorPreset;
		using (IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent())
		{
			monitorPreset = monitorComponent.QueryMonitorPreset(presetId);
		}
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", monitorPreset.VideoWallId))
		{
			throw new MonitorNotFoundException();
		}
		return monitorPreset;
	}

	public static MonitorPreset[] QueryMonitorPresetsByIds(IGeminiRequestContext requestContext, string[] monitorPresetIds)
	{
		using IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent();
		return FilterMonitorPresetsBasedOnPermission(requestContext, monitorComponent.QueryMonitorPresetsByIds(monitorPresetIds));
	}

	public static string[] QueryMonitorPresetIdsByMonitorId(IGeminiRequestContext requestContext, string monitorId)
	{
		using IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent();
		string objectId = monitorComponent.QueryVideoWallId(monitorId);
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", objectId))
		{
			return new string[0];
		}
		return monitorComponent.QueryMonitorPresetIdsByMonitorId(monitorId);
	}

	public static string[] QueryMonitorPresetIdsByVideoWallPresetId(IGeminiRequestContext requestContext, string videoWallPresetId)
	{
		string[] result = new string[0];
		string videoWallId;
		using (IVideoWallPresetComponent videoWallPresetComponent = requestContext.CreateVideoWallPresetComponent())
		{
			videoWallId = videoWallPresetComponent.GetVideoWallId(videoWallPresetId);
		}
		if (!string.IsNullOrEmpty(videoWallId) && requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", videoWallId))
		{
			using IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent();
			result = monitorComponent.QueryMonitorPresetIdsByVideoWallPresetId(videoWallPresetId);
		}
		return result;
	}

	public static MonitorPreset UpdateMonitorPreset(IGeminiRequestContext requestContext, string monitorPresetId, string definitionXml, CustomProperty[] customProperties)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		MonitorPreset result = null;
		Validation.CheckName(monitorPresetId, "monitorPresetId");
		string idFromPreset = Monitor.GetIdFromPreset(requestContext, monitorPresetId);
		Guid sessionId = Guid.NewGuid();
		using (MonitorComponent monitorComponent = requestContext.CreateComponent<MonitorComponent>())
		{
			string text = monitorComponent.QueryVideoWallId(idFromPreset);
			try
			{
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_WRITE", text);
			}
			catch (SecurityObjectDoesNotExistException)
			{
				throw new VideoWallNotFoundException();
			}
			monitorComponent.UpdateMonitorPreset(monitorPresetId, definitionXml, sessionId, customProperties);
			result = FindMonitorPreset(requestContext, monitorPresetId);
			Integration.FireMonitorPresetChanged(requestContext, monitorPresetId, text, EventAction.Updated);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return result;
	}

	private static MonitorPreset FindMonitorPreset(IGeminiRequestContext requestContext, string monitorPresetId)
	{
		using IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent();
		return monitorComponent.QueryMonitorPreset(monitorPresetId);
	}

	private static MonitorPreset[] FilterMonitorPresetsBasedOnPermission(IGeminiRequestContext requestContext, MonitorPreset[] monitorPresetArray)
	{
		List<MonitorPreset> list = new List<MonitorPreset>();
		foreach (MonitorPreset monitorPreset in monitorPresetArray)
		{
			if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", monitorPreset.VideoWallId))
			{
				list.Add(monitorPreset);
			}
		}
		return list.ToArray();
	}
}
