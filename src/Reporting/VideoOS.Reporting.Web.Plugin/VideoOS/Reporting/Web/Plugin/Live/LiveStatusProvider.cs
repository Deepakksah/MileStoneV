using System;
using VideoOS.Reporting.Web.Plugin.Base;
using VideoOS.Reporting.Web.Plugin.Business;
using VideoOS.Reporting.Web.Plugin.Business.Model;

namespace VideoOS.Reporting.Web.Plugin.Live;

public static class LiveStatusProvider
{
	public static RecorderStatus GetRecorderStatus(Guid recorderId)
	{
		return ServerStatusProvider.GetRecorderStatus(recorderId, ServiceLogger.Instance);
	}

	public static FailoverStatus GetFailoverStatus(Guid serverId, Uri uri)
	{
		return ServerStatusProvider.GetFailoverStatus(serverId, uri, ServiceLogger.Instance);
	}

	public static DeviceStatusResult GetDeviceStatusByRecorder(Guid recorderId)
	{
		return DeviceStatusProvider.GetDeviceStatusByRecorderId(recorderId, ServiceLogger.Instance);
	}

	public static DeviceStatusResult GetDeviceStatusByCameraGroup(Guid cameraGroupId)
	{
		return DeviceStatusProvider.GetDeviceStatusByCameraGroupId(cameraGroupId, ServiceLogger.Instance);
	}

	public static DeviceStatusResult GetDeviceStatusByCamera(Guid cameraId)
	{
		return DeviceStatusProvider.GetDeviceStatusByCameraId(cameraId, ServiceLogger.Instance);
	}
}
