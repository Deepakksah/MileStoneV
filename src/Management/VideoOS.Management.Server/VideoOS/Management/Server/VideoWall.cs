using System;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class VideoWall
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public double Width { get; set; }

	public double Height { get; set; }

	public bool StatusText { get; set; }

	public TitleBar TitleBar { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public static VideoWall QueryVideoWall(IGeminiRequestContext requestContext, string videoWallId)
	{
		Validation.CheckObjectId(ref videoWallId, "videoWallId", allowNullOrEmpty: false);
		try
		{
			if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", videoWallId))
			{
				throw new VideoWallNotFoundException();
			}
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new VideoWallNotFoundException();
		}
		using IVideoWallComponent videoWallComponent = requestContext.CreateVideoWallComponent();
		return videoWallComponent.QueryVideoWall(videoWallId);
	}

	public static VideoWall[] QueryVideoWalls(IGeminiRequestContext requestContext, string[] ids)
	{
		for (int i = 0; i < ids.Length; i++)
		{
			string objectId = ids[i];
			Validation.CheckObjectId(ref objectId, "videoWallId", allowNullOrEmpty: false);
		}
		using IVideoWallComponent videoWallComponent = requestContext.CreateVideoWallComponent();
		return FilterVideoWallsBasedOnPermissions(requestContext, videoWallComponent.QueryVideoWallsByIds(ids));
	}

	public static string[] QueryVideoWallIds(IGeminiRequestContext requestContext)
	{
		using IVideoWallComponent videoWallComponent = requestContext.CreateVideoWallComponent();
		return FilterVideoWallIdsBasedOnPermissions(requestContext, videoWallComponent.QueryVideoWallIds());
	}

	public static VideoWall CreateVideoWall(IGeminiRequestContext requestContext, VideoWall newVideoWall)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "CREATE_SMARTWALL");
		Validation.CheckName(newVideoWall.Name, "name");
		string description = newVideoWall.Description;
		Validation.CheckDescription(ref description, "description");
		newVideoWall.Description = description;
		newVideoWall.Id = GeminiResourceComponent.GetGuid(newVideoWall.Id);
		using (VideoWallComponent videoWallComponent = requestContext.CreateComponent<VideoWallComponent>())
		{
			videoWallComponent.CreateVideoWall(newVideoWall, requestContext.AuthenticatedUserSid);
			newVideoWall = FindVideoWall(requestContext, newVideoWall.Id);
			Integration.FireVideoWallChangedEvent(requestContext, newVideoWall.Id, EventAction.Added);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogEntryAuditLogger.AuditVideoWallCreated(new Guid(newVideoWall.Id), newVideoWall.Name, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return newVideoWall;
	}

	public static VideoWall UpdateVideoWall(IGeminiRequestContext requestContext, VideoWall videoWall)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "SmartWall");
		string objectId = videoWall.Id;
		Validation.CheckObjectId(ref objectId, "videoWallId", allowNullOrEmpty: false);
		Validation.CheckName(videoWall.Name, "name");
		string description = videoWall.Description;
		Validation.CheckDescription(ref description, "description");
		videoWall.Description = description;
		try
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_WRITE", objectId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new VideoWallNotFoundException();
		}
		Guid sessionId = Guid.NewGuid();
		using (VideoWallComponent videoWallComponent = requestContext.CreateComponent<VideoWallComponent>())
		{
			videoWallComponent.UpdateVideoWall(videoWall, sessionId);
			videoWall = FindVideoWall(requestContext, videoWall.Id);
			Integration.FireVideoWallChangedEvent(requestContext, videoWall.Id, EventAction.Updated);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return videoWall;
	}

	public static void DeleteVideoWall(IGeminiRequestContext requestContext, string videoWallId)
	{
		Validation.CheckObjectId(ref videoWallId, "videoWallId", allowNullOrEmpty: false);
		try
		{
			requestContext.GetService<SecurityManagerService>().CheckPermission(requestContext, SecurityNamespaceIdentifier.SmartWall, "DELETE", videoWallId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new RemoveNonExistingVideoWallException();
		}
		Guid sessionId = Guid.NewGuid();
		using (VideoWallComponent videoWallComponent = requestContext.CreateComponent<VideoWallComponent>())
		{
			string[] monitors = Monitor.QueryMonitorIds(requestContext, videoWallId);
			videoWallComponent.DeleteVideoWall(videoWallId, sessionId);
			Integration.FireVideoWallDeletedEvent(requestContext, videoWallId, monitors);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	public static string GetIdFromMonitorId(IGeminiRequestContext requestContext, string monitorId)
	{
		Validation.CheckRequestContext(requestContext);
		using IMonitorComponent monitorComponent = requestContext.CreateMonitorComponent();
		return monitorComponent.QueryVideoWallId(monitorId);
	}

	private static VideoWall[] FilterVideoWallsBasedOnPermissions(IGeminiRequestContext requestContext, VideoWall[] videoWallArray)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", videoWallArray.Select((VideoWall v) => (v: v, Id: v.Id))).ToArray();
	}

	private static string[] FilterVideoWallIdsBasedOnPermissions(IGeminiRequestContext requestContext, string[] videoWallIdArray)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.SmartWall, "GENERIC_READ", videoWallIdArray.Select((string id) => (id, id))).ToArray();
	}

	private static VideoWall FindVideoWall(IGeminiRequestContext requestContext, string videoWallId)
	{
		return requestContext.CreateVideoWallComponent().QueryVideoWall(videoWallId);
	}
}
