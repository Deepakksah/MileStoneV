using System;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class HostingServer
{
	public int Id { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public string Type { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public string InternalAddress { get; set; }

	public string ExternalAddress { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public static HostingServer QueryHostingServerById(IGeminiRequestContext requestContext, int hostingServerId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		HostingServer result = null;
		if (requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			using IHostingServerComponent hostingServerComponent = requestContext.CreateHostingServerComponent();
			result = hostingServerComponent.QueryHostingServer(hostingServerId);
		}
		return result;
	}

	public static HostingServer[] QueryHostingServerByIds(IGeminiRequestContext requestContext, int[] hostingServerIds)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		HostingServer[] result = new HostingServer[0];
		if (requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			using IHostingServerComponent hostingServerComponent = requestContext.CreateHostingServerComponent();
			result = hostingServerComponent.QueryHostingServersByIds(hostingServerIds);
		}
		return result;
	}

	public static string[] QueryHostingServerIds(IGeminiRequestContext requestContext)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		string[] result = new string[0];
		if (requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			using IHostingServerComponent hostingServerComponent = requestContext.CreateHostingServerComponent();
			result = hostingServerComponent.QueryHostingServerIds();
		}
		return result;
	}

	public static HostingServer CreateHostingServer(IGeminiRequestContext requestContext, HostingServer newHostingServer, string hostingServerType)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckName(newHostingServer.Name, "Name");
		Validation.CheckWebAddress(newHostingServer.InternalAddress, allowHttps: false, "InternalAddress");
		Validation.CheckWebAddress(newHostingServer.ExternalAddress, allowHttps: false, "ExternalAddress");
		using (IHostingServerComponent hostingServerComponent = requestContext.CreateHostingServerComponent())
		{
			newHostingServer.Id = hostingServerComponent.CreateHostingServer(newHostingServer, sessionId, hostingServerType);
			newHostingServer = FindHostingServer(requestContext, hostingServerComponent, newHostingServer.Id);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return newHostingServer;
	}

	public static HostingServer UpdateHostingServer(IGeminiRequestContext requestContext, HostingServer hostingServer)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckName(hostingServer.Name, "Name");
		Validation.CheckWebAddress(hostingServer.InternalAddress, allowHttps: false, "InternalAddress");
		Validation.CheckWebAddress(hostingServer.ExternalAddress, allowHttps: false, "ExternalAddress");
		using (IHostingServerComponent hostingServerComponent = requestContext.CreateHostingServerComponent())
		{
			using IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent();
			hostingServerComponent.UpdateHostingServer(hostingServer, sessionId);
			hostingServer = FindHostingServer(requestContext, hostingServerComponent, hostingServer.Id);
			Guid[] idsFromHostingServerId = recordingServerComponent.GetIdsFromHostingServerId(hostingServer.Id);
			for (int i = 0; i < idsFromHostingServerId.Length; i++)
			{
				Integration.TryNotifyRecordingServerAsync(idsFromHostingServerId[i]);
			}
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return hostingServer;
	}

	public static void DeleteHostingServer(IGeminiRequestContext requestContext, int hostingServerId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		using (IHostingServerComponent hostingServerComponent = requestContext.CreateHostingServerComponent())
		{
			hostingServerComponent.DeleteHostingServer(hostingServerId, sessionId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	private static HostingServer FindHostingServer(IGeminiRequestContext requestContext, IHostingServerComponent hostingServerComponent, int hostingServerId)
	{
		bool flag = false;
		try
		{
			if (hostingServerComponent == null)
			{
				hostingServerComponent = requestContext.CreateHostingServerComponent();
				flag = true;
			}
			return hostingServerComponent.QueryHostingServer(hostingServerId);
		}
		finally
		{
			if (flag)
			{
				hostingServerComponent.Dispose();
			}
		}
	}
}
