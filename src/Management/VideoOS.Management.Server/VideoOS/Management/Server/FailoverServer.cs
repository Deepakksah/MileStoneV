using System;
using System.Globalization;
using VideoOS.Common.Integration.Communication;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public sealed class FailoverServer
{
	public string Id { get; set; }

	public string HotStandbyRecorderId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public int Position { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public bool Enabled { get; set; }

	public string HostName { get; set; }

	public int UdpPort { get; set; }

	public string DatabasePath { get; set; }

	public bool ClientEncryptionEnabled { get; set; }

	public string WebServerHostName { get; set; }

	public int WebServerPort { get; set; }

	public string WebServerUri
	{
		get
		{
			if (!ClientEncryptionEnabled)
			{
				return UriHandling.CreateHttpUri(WebServerHostName, WebServerPort).AbsoluteUri;
			}
			return UriHandling.CreateHttpsUri(WebServerHostName, WebServerPort).AbsoluteUri;
		}
	}

	public bool PublicAccessEnabled { get; set; }

	public string PublicWebServerHostName { get; set; }

	public int PublicWebServerPort { get; set; }

	public string WebServerUriConfiguration
	{
		get
		{
			if (!PublicAccessEnabled || string.IsNullOrEmpty(PublicWebServerHostName))
			{
				return string.Empty;
			}
			if (!ClientEncryptionEnabled)
			{
				return UriHandling.CreateHttpUri(PublicWebServerHostName, PublicWebServerPort).AbsoluteUri;
			}
			return UriHandling.CreateHttpsUri(PublicWebServerHostName, PublicWebServerPort).AbsoluteUri;
		}
	}

	public string ServiceId { get; set; }

	public string RelationIds { get; set; }

	public string ParentGroupId { get; set; }

	public string MulticastServerAddress { get; set; }

	public static FailoverServer[] QueryFailoverServers(IGeminiRequestContext requestContext, string serverGroupId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingServerFailover");
		Validation.CheckObjectId(ref serverGroupId, "serverGroupId", allowNullOrEmpty: false);
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_READ", "/"))
		{
			return new FailoverServer[0];
		}
		using IFailoverServerComponent failoverServerComponent = requestContext.CreateFailoverServerComponent();
		return failoverServerComponent.QueryFailoverServers(serverGroupId, serverGroupId != "D7BF5260-0B76-45F3-B920-9A4CC56D6CD5");
	}

	public static FailoverServer QueryFailoverServer(IGeminiRequestContext requestContext, string failoverId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingServerFailover");
		Validation.CheckObjectId(ref failoverId, "failoverId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_READ", "/");
		using IFailoverServerComponent failoverServerComponent = requestContext.CreateFailoverServerComponent();
		return failoverServerComponent.QueryFailoverServer(failoverId);
	}

	public static FailoverServer[] QueryFailoverServersByIds(IGeminiRequestContext requestContext, string[] failoverServerIds)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingServerFailover");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_READ", "/");
		using IFailoverServerComponent failoverServerComponent = requestContext.CreateFailoverServerComponent();
		return failoverServerComponent.QueryFailoverServersByIds(failoverServerIds);
	}

	public static FailoverServer CreateFailoverServer(IGeminiRequestContext requestContext, string parentId, FailoverServer newFailoverServer)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingServerFailover");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_WRITE", "/");
		Validation.CheckName(newFailoverServer.Name, "Name");
		Validation.CheckHostName(newFailoverServer.HostName, "HostName");
		Validation.CheckIsPositive(newFailoverServer.UdpPort, "UdpPort", allowNull: false);
		Validation.CheckHostName(newFailoverServer.WebServerHostName, "WebServerHostName");
		Validation.CheckIsPositive(newFailoverServer.WebServerPort, "WebServerPort", allowNull: false);
		string description = newFailoverServer.Description;
		Validation.CheckDescription(ref description, "description");
		newFailoverServer.Description = description;
		newFailoverServer.Id = GeminiResourceComponent.GetGuid(newFailoverServer.Id);
		using (IFailoverServerComponent failoverServerComponent = requestContext.CreateFailoverServerComponent())
		{
			failoverServerComponent.CreateFailoverServer(parentId, newFailoverServer);
			newFailoverServer = FindFailoverServer(requestContext, failoverServerComponent, newFailoverServer.Id);
		}
		Integration.FireConfigurationChanged(requestContext, "FailoverRecorder", newFailoverServer.Id, EventAction.Added, newFailoverServer.Name);
		return newFailoverServer;
	}

	public static void UpdateFailoverServerValidation(IGeminiRequestContext requestContext, string recordingServerId, string name, string description, string publicWebServerHostName, int publicWebServerPort, int udpPort, string multicastServerAddress, string databasePath)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingServerFailover");
		Validation.CheckObjectId(recordingServerId, "Id", allowNullOrEmpty: false);
		Validation.CheckName(name, "Name", allowNull: false);
		Validation.CheckDescription(ref description, "Description", allowNull: true);
		if (!string.IsNullOrEmpty(publicWebServerHostName))
		{
			Validation.CheckWebAddress(string.Format(CultureInfo.InvariantCulture, "http://{0}:{1}", publicWebServerHostName, publicWebServerPort), allowHttps: true, "WebServerUriConfiguration");
		}
		Validation.ValidateMulticastIP(multicastServerAddress, "MulticastServerAddress", allowNull: true);
		Validation.ValidateInt32(udpPort, "UdpPort", 1, 65535, allowNull: false);
		Validation.CheckPath(databasePath, "DatabasePath");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_WRITE", "/");
	}

	public static FailoverServer UpdateFailoverServer(IGeminiRequestContext requestContext, FailoverServer failoverServer)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingServerFailover");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_WRITE", "/");
		string objectId = failoverServer.Id;
		Validation.CheckObjectId(ref objectId, "failoverServerId", allowNullOrEmpty: false);
		failoverServer.Id = objectId;
		Validation.CheckName(failoverServer.Name, "name");
		string description = failoverServer.Description;
		Validation.CheckDescription(ref description, "description");
		failoverServer.Description = description;
		string objectId2 = failoverServer.HotStandbyRecorderId;
		Validation.CheckObjectId(ref objectId2, "hotStandbyRecorderId", allowNullOrEmpty: false);
		failoverServer.HotStandbyRecorderId = objectId2;
		if (!string.IsNullOrEmpty(failoverServer.PublicWebServerHostName))
		{
			Validation.CheckWebAddress(string.Format(CultureInfo.InvariantCulture, "http://{0}:{1}", failoverServer.PublicWebServerHostName, failoverServer.PublicWebServerPort), allowHttps: true, "WebServerUriConfiguration");
		}
		Guid guid = QueryAssociatedRecorderId(requestContext, failoverServer.Id);
		using (IFailoverServerComponent failoverServerComponent = requestContext.CreateFailoverServerComponent())
		{
			failoverServerComponent.UpdateFailoverServer(failoverServer);
			failoverServer = FindFailoverServer(requestContext, failoverServerComponent, failoverServer.Id);
		}
		if (guid != Guid.Empty)
		{
			Integration.TryNotifyRecordingServerAsync(guid);
		}
		Integration.FireConfigurationChanged(requestContext, "FailoverRecorder", failoverServer.Id, EventAction.Updated, failoverServer.Name);
		return failoverServer;
	}

	public static void DeleteFailoverServer(IGeminiRequestContext requestContext, string failoverServerId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingServerFailover");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_WRITE", "/");
		Validation.CheckObjectId(ref failoverServerId, "failoverServerId", allowNullOrEmpty: false);
		Guid guid = QueryAssociatedRecorderId(requestContext, failoverServerId);
		using (IFailoverServerComponent failoverServerComponent = requestContext.CreateFailoverServerComponent())
		{
			failoverServerComponent.DeleteFailoverServer(failoverServerId);
		}
		if (guid != Guid.Empty)
		{
			Integration.TryNotifyRecordingServerAsync(guid);
		}
		Integration.FireConfigurationChanged(requestContext, "FailoverRecorder", failoverServerId, EventAction.Deleted, string.Empty);
	}

	public static void SetEnabled(IGeminiRequestContext requestContext, string failoverServerId, bool enable)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingServerFailover");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_WRITE", "/");
		Validation.CheckObjectId(ref failoverServerId, "failoverServerId", allowNullOrEmpty: false);
		using (IFailoverServerComponent failoverServerComponent = requestContext.CreateFailoverServerComponent())
		{
			failoverServerComponent.SetEnabled(failoverServerId, enable);
		}
		Integration.FireConfigurationChanged(requestContext, "FailoverRecorder", failoverServerId, EventAction.Updated, string.Empty);
	}

	public static void Move(IGeminiRequestContext requestContext, string failoverServerId, string newParentId, int insertAtPosition)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingServerFailover");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_WRITE", "/");
		Validation.CheckObjectId(ref failoverServerId, "failoverServerId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref newParentId, "newParentId", allowNullOrEmpty: false);
		using IFailoverServerComponent failoverServerComponent = requestContext.CreateFailoverServerComponent();
		failoverServerComponent.MoveToGroup(failoverServerId, newParentId, insertAtPosition);
	}

	private static Guid QueryAssociatedRecorderId(IGeminiRequestContext requestContext, string failoverId)
	{
		using IFailoverServerComponent failoverServerComponent = requestContext.CreateFailoverServerComponent();
		return failoverServerComponent.QueryAssociatedRecorderId(failoverId);
	}

	private static FailoverServer FindFailoverServer(IGeminiRequestContext requestContext, IFailoverServerComponent failoverComponent, string failoverId)
	{
		bool flag = false;
		try
		{
			if (failoverComponent == null)
			{
				failoverComponent = requestContext.CreateFailoverServerComponent();
				flag = true;
			}
			return failoverComponent.QueryFailoverServer(failoverId);
		}
		finally
		{
			if (flag)
			{
				failoverComponent.Dispose();
			}
		}
	}

	public static void SetRecorderInformation(IGeminiRequestContext requestContext, string id, bool? clientEncryptionEnabled, string webserverHostName, int? webserverPort, string version, string timeZone)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingServerFailover");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_WRITE", "/");
		Validation.CheckObjectId(id, "Id", allowNullOrEmpty: false);
		Validation.CheckBooleanValue(clientEncryptionEnabled, "ClientEncryptionEnabled", allowNull: false);
		Validation.CheckHostName(webserverHostName, "WebServerHostName");
		Validation.CheckIsPositive(webserverPort, "WebServerPort", allowNull: false);
		Validation.CheckVersionString(version, "version");
		Validation.CheckStringNotNullOrWhiteSpace(timeZone, "timeZone");
		using IFailoverServerComponent failoverServerComponent = requestContext.CreateFailoverServerComponent();
		failoverServerComponent.SetRecorderInformation(id, clientEncryptionEnabled.Value, webserverHostName, webserverPort.Value, version, timeZone);
	}
}
