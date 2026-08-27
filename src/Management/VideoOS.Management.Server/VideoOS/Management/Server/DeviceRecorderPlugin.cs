using System;
using System.Collections.Generic;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class DeviceRecorderPlugin
{
	public Guid DeviceId { get; set; }

	public Guid RecorderPluginId { get; set; }

	public string Settings { get; set; }

	public static IEnumerable<DeviceRecorderPlugin> QueryEnabledPluginsByType(IGeminiRequestContext requestContext, Guid pluginTypeId)
	{
		using IDeviceRecorderPluginDataComponent deviceRecorderPluginDataComponent = requestContext.CreateDeviceRecorderPluginDataComponent();
		return deviceRecorderPluginDataComponent.QueryEnabledPluginsByType(pluginTypeId);
	}

	public static DeviceRecorderPlugin QueryEnabledPluginsByTypeAndId(IGeminiRequestContext requestContext, Guid pluginTypeId, Guid deviceId)
	{
		using IDeviceRecorderPluginDataComponent deviceRecorderPluginDataComponent = requestContext.CreateDeviceRecorderPluginDataComponent();
		return deviceRecorderPluginDataComponent.QueryEnabledPluginsByTypeAndId(pluginTypeId, deviceId);
	}

	public static void UpdateDeviceFisheyeSettings(IGeminiRequestContext requestContext, Guid deviceId, string settings, bool enabled)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_WRITE", deviceId.ToString());
		Guid sessionId = Guid.NewGuid();
		using (IDeviceRecorderPluginDataComponent deviceRecorderPluginDataComponent = requestContext.CreateDeviceRecorderPluginDataComponent())
		{
			deviceRecorderPluginDataComponent.UpdateDeviceFisheyeSettings(deviceId, settings, enabled, sessionId);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}
}
