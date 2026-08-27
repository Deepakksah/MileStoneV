using System;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class VideoWallPreset
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public string VideoWallId { get; set; }

	public static VideoWallPreset QueryVideoWallPreset(IGeminiRequestContext requestContext, string videoWallPresetId)
	{
		VideoWallPreset result = null;
		Validation.CheckObjectId(ref videoWallPresetId, "videoWallPresetId", allowNullOrEmpty: false);
		using (IVideoWallPresetComponent videoWallPresetComponent = requestContext.CreateVideoWallPresetComponent())
		{
			string videoWallId = videoWallPresetComponent.GetVideoWallId(videoWallPresetId);
			if (videoWallId == null)
			{
				return null;
			}
			if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", videoWallId))
			{
				result = videoWallPresetComponent.QueryVideoWallPreset(videoWallPresetId);
			}
		}
		return result;
	}

	public static VideoWallPreset[] QueryVideoWallPresetsByIds(IGeminiRequestContext requestContext, string[] videoWallPresetIds)
	{
		using IVideoWallPresetComponent videoWallPresetComponent = requestContext.CreateVideoWallPresetComponent();
		return FilterVideoWallPresetsBasedOnPermissions(requestContext, videoWallPresetComponent.QueryVideoWallPresetsByIds(videoWallPresetIds));
	}

	public static string[] QueryVideoWallPresetIds(IGeminiRequestContext requestContext, string videoWallId)
	{
		string[] result = new string[0];
		if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", videoWallId))
		{
			using IVideoWallPresetComponent videoWallPresetComponent = requestContext.CreateVideoWallPresetComponent();
			result = videoWallPresetComponent.QueryVideoWallPresetIds(videoWallId);
		}
		return result;
	}

	public static VideoWallPreset CreateVideoWallPreset(IGeminiRequestContext requestContext, string videoWallId, VideoWallPreset newVideoWallPreset)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref videoWallId, "videoWallId", allowNullOrEmpty: false);
		Validation.CheckName(newVideoWallPreset.Name, "name");
		string description = newVideoWallPreset.Description;
		Validation.CheckDescription(ref description, "description");
		newVideoWallPreset.Description = description;
		newVideoWallPreset.Id = GeminiResourceComponent.GetGuid(newVideoWallPreset.Id);
		try
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_WRITE", videoWallId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new VideoWallNotFoundException();
		}
		using (VideoWallPresetComponent videoWallPresetComponent = requestContext.CreateComponent<VideoWallPresetComponent>())
		{
			videoWallPresetComponent.CreateVideoWallPreset(videoWallId, newVideoWallPreset, sessionId);
			newVideoWallPreset = FindVideoWallPreset(requestContext, newVideoWallPreset.Id);
			Integration.FireVideoWallPresetChangedEvent(requestContext, newVideoWallPreset.Id, videoWallId, EventAction.Added);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return newVideoWallPreset;
	}

	public static VideoWallPreset UpdateVideoWallPreset(IGeminiRequestContext requestContext, VideoWallPreset videoWallPreset)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		Guid sessionId = Guid.NewGuid();
		string objectId = videoWallPreset.Id;
		Validation.CheckObjectId(ref objectId, "videoWallPresetId", allowNullOrEmpty: false);
		Validation.CheckName(videoWallPreset.Name, "name");
		string description = videoWallPreset.Description;
		Validation.CheckDescription(ref description, "description");
		videoWallPreset.Description = description;
		using (VideoWallPresetComponent videoWallPresetComponent = requestContext.CreateComponent<VideoWallPresetComponent>())
		{
			string videoWallId = videoWallPresetComponent.GetVideoWallId(objectId);
			try
			{
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_WRITE", videoWallId);
			}
			catch (SecurityObjectDoesNotExistException)
			{
				throw new VideoWallPresetNotFoundException();
			}
			videoWallPresetComponent.UpdateVideoWallPreset(videoWallPreset, sessionId);
			videoWallPreset = FindVideoWallPreset(requestContext, videoWallPreset.Id);
			Integration.FireVideoWallPresetChangedEvent(requestContext, videoWallPreset.Id, videoWallId, EventAction.Updated);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return videoWallPreset;
	}

	public static void DeleteVideoWallPreset(IGeminiRequestContext requestContext, string videoWallPresetId)
	{
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref videoWallPresetId, "videoWallPresetId", allowNullOrEmpty: false);
		using (VideoWallPresetComponent videoWallPresetComponent = requestContext.CreateComponent<VideoWallPresetComponent>())
		{
			string videoWallId = videoWallPresetComponent.GetVideoWallId(videoWallPresetId);
			try
			{
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_WRITE", videoWallId);
			}
			catch (SecurityObjectDoesNotExistException)
			{
				throw new VideoWallPresetNotFoundException();
			}
			videoWallPresetComponent.DeleteVideoWallPreset(videoWallPresetId, sessionId);
			Integration.FireVideoWallPresetChangedEvent(requestContext, videoWallPresetId, videoWallId, EventAction.Deleted);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	public static void ActivateVideoWallPreset(IGeminiRequestContext requestContext, string videoWallPresetId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref videoWallPresetId, "videoWallPresetId", allowNullOrEmpty: false);
		using (VideoWallPresetComponent videoWallPresetComponent = requestContext.CreateComponent<VideoWallPresetComponent>())
		{
			string videoWallId = videoWallPresetComponent.GetVideoWallId(videoWallPresetId);
			try
			{
				requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
				requestContext.GetService<SecurityManagerService>().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "OPERATE", videoWallId);
			}
			catch (SecurityObjectDoesNotExistException)
			{
				throw new VideoWallPresetNotFoundException();
			}
		}
		using (VideoWallFunctionalityComponent videoWallFunctionalityComponent = requestContext.CreateComponent<VideoWallFunctionalityComponent>())
		{
			Tuple<string, string>[] array = videoWallFunctionalityComponent.ActivateVideoWallPreset(videoWallPresetId, sessionId);
			string videoWallId2 = videoWallFunctionalityComponent.GetVideoWallId(videoWallPresetId);
			Tuple<string, string>[] array2 = array;
			foreach (Tuple<string, string> tuple in array2)
			{
				Integration.FireMonitorViewChanged(requestContext, tuple.Item1, tuple.Item2, videoWallId2, EventAction.Updated);
			}
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	private static VideoWallPreset FindVideoWallPreset(IGeminiRequestContext requestContext, string videoWallPresetId)
	{
		using IVideoWallPresetComponent videoWallPresetComponent = requestContext.CreateVideoWallPresetComponent();
		return videoWallPresetComponent.QueryVideoWallPreset(videoWallPresetId);
	}

	private static VideoWallPreset[] FilterVideoWallPresetsBasedOnPermissions(IGeminiRequestContext requestContext, VideoWallPreset[] videoWallPresetArray)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", videoWallPresetArray.Select((VideoWallPreset p) => (p: p, Id: p.Id, VideoWallId: p.VideoWallId))).ToArray();
	}
}
