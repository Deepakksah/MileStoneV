using System;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class HostedHardware
{
	public int Id { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public string Type { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public string HostingInfo { get; set; }

	public string ManufacturerId { get; set; }

	public string CustomData { get; set; }

	public int HostingServerId { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public static HostedHardware QueryHostedHardwareById(IGeminiRequestContext requestContext, int hostedHardwareId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		HostedHardware result = null;
		if (requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			using IHostedHardwareComponent hostedHardwareComponent = requestContext.CreateHostedHardwareComponent();
			result = hostedHardwareComponent.QueryHostedHardware(hostedHardwareId);
		}
		return result;
	}

	public static HostedHardware[] QueryHostedHardwareByIds(IGeminiRequestContext requestContext, int[] hostedHardwareIds)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		HostedHardware[] result = new HostedHardware[0];
		if (requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			using IHostedHardwareComponent hostedHardwareComponent = requestContext.CreateHostedHardwareComponent();
			result = hostedHardwareComponent.QueryHostedHardwaresByIds(hostedHardwareIds);
		}
		return result;
	}

	public static string[] QueryHostedHardwareIds(IGeminiRequestContext requestContext, int hostingServerId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		string[] result = new string[0];
		if (requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			using IHostedHardwareComponent hostedHardwareComponent = requestContext.CreateHostedHardwareComponent();
			result = hostedHardwareComponent.QueryHostedHardwareIds(hostingServerId);
		}
		return result;
	}

	public static HostedHardware CreateHostedHardware(IGeminiRequestContext requestContext, HostedHardware newHostedHardware, string hostedHardwareType)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckName(newHostedHardware.Name, "Name");
		using (IHostedHardwareComponent hostedHardwareComponent = requestContext.CreateHostedHardwareComponent())
		{
			newHostedHardware.Id = hostedHardwareComponent.CreateHostedHardware(newHostedHardware, sessionId, hostedHardwareType);
			newHostedHardware = FindHostedHardware(requestContext, hostedHardwareComponent, newHostedHardware.Id);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return newHostedHardware;
	}

	public static HostedHardware UpdateHostedHardware(IGeminiRequestContext requestContext, HostedHardware hostedHardware)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		Validation.CheckName(hostedHardware.Name, "Name");
		using (IHostedHardwareComponent hostedHardwareComponent = requestContext.CreateHostedHardwareComponent())
		{
			using IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent();
			hostedHardwareComponent.UpdateHostedHardware(hostedHardware, sessionId);
			hostedHardware = FindHostedHardware(requestContext, hostedHardwareComponent, hostedHardware.Id);
			Guid? idFromhostedHardwareId = recordingServerComponent.GetIdFromhostedHardwareId(hostedHardware.Id);
			if (idFromhostedHardwareId.HasValue)
			{
				Integration.TryNotifyRecordingServerAsync(idFromhostedHardwareId.Value);
			}
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return hostedHardware;
	}

	public static void DeleteHostedHardware(IGeminiRequestContext requestContext, int hostedHardwareId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RemoteConnectedServices");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Guid sessionId = Guid.NewGuid();
		using (IHostedHardwareComponent hostedHardwareComponent = requestContext.CreateHostedHardwareComponent())
		{
			hostedHardwareComponent.DeleteHostedHardware(hostedHardwareId, sessionId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	private static HostedHardware FindHostedHardware(IGeminiRequestContext requestContext, IHostedHardwareComponent hostedHardwareComponent, int hostedHardwareId)
	{
		bool flag = false;
		try
		{
			if (hostedHardwareComponent == null)
			{
				hostedHardwareComponent = requestContext.CreateHostedHardwareComponent();
				flag = true;
			}
			return hostedHardwareComponent.QueryHostedHardware(hostedHardwareId);
		}
		finally
		{
			if (flag)
			{
				hostedHardwareComponent.Dispose();
			}
		}
	}
}
