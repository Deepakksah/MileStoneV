using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class DevicePatrollingProfile
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string DefinitionXml { get; set; }

	public DateTime LastModified { get; set; }

	public string DeviceId { get; set; }

	public string HardwareId { get; set; }

	public string RecordingServerId { get; set; }

	public static DevicePatrollingProfile[] QueryPatrollingProfilesByPreset(IGeminiRequestContext requestContext, string deviceId, string presetId)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref presetId, "presetId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_READ", deviceId);
		using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
		return deviceComponent.QueryPatrollingProfilesByPreset(deviceId, presetId);
	}
}
