using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.Data;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class SDKRestrictedMediaService : RestrictedMediaService
{
	public override RestrictedMediaResult RestrictedMediaCreate(ServerId serverId, IEnumerable<FQID> devices, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		if (serverId == null)
		{
			throw new ArgumentNullMIPException("serverId");
		}
		if (devices == null)
		{
			throw new ArgumentNullMIPException("devices");
		}
		if (!devices.Any() || devices.Any((FQID d) => d == null))
		{
			throw new ArgumentMIPException("Invalid or no devices specified. (devices)");
		}
		ILoginServer loginServer = GetLoginServer(serverId);
		Guid restrictedMediaId = Guid.NewGuid();
		Guid[] deviceIds = devices.Select((FQID d) => d.ObjectId).ToArray();
		return ConvertToPlatformRestrictedMediaResult(loginServer.RestrictedMediaCreate(restrictedMediaId, deviceIds, header, description, timeStart, timeEnd), serverId);
	}

	public override RestrictedMedia[] RestrictedMediaQueryAll(ServerId serverId)
	{
		if (serverId == null)
		{
			throw new ArgumentNullMIPException("serverId");
		}
		return (from restrictedMedia in GetLoginServer(serverId).RestrictedMediaQueryAll()
			select ConvertToPlatformRestrictedMedia(restrictedMedia, serverId)).ToArray();
	}

	public override RestrictedMediaResult RestrictedMediaUpdate(FQID restrictedMediaFqid, IEnumerable<FQID> devicesToAdd, IEnumerable<FQID> devicesToRemove, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		if (restrictedMediaFqid == null)
		{
			throw new ArgumentNullMIPException("restrictedMediaFqid");
		}
		if (devicesToAdd == null)
		{
			throw new ArgumentNullMIPException("devicesToAdd");
		}
		if (devicesToAdd.Any((FQID d) => d == null))
		{
			throw new ArgumentMIPException("Invalid devices specified. (devicesToAdd)");
		}
		if (devicesToRemove == null)
		{
			throw new ArgumentNullMIPException("devicesToRemove");
		}
		if (devicesToRemove.Any((FQID d) => d == null))
		{
			throw new ArgumentMIPException("Invalid devices to remove specified. (devicesToRemove)");
		}
		return ConvertToPlatformRestrictedMediaResult(GetLoginServer(restrictedMediaFqid.ServerId).RestrictedMediaUpdate(deviceIdsToAdd: devicesToAdd.Select((FQID d) => d.ObjectId).ToArray(), deviceIdsToRemove: devicesToRemove.Select((FQID d) => d.ObjectId).ToArray(), restrictedMediaId: restrictedMediaFqid.ObjectId, header: header, description: description, timeStart: timeStart, timeEnd: timeEnd), restrictedMediaFqid.ServerId);
	}

	public override RestrictedMediaResult RestrictedMediaDelete(FQID restrictedMediaFqid)
	{
		if (restrictedMediaFqid == null)
		{
			throw new ArgumentNullMIPException("restrictedMediaFqid");
		}
		return ConvertToPlatformRestrictedMediaResult(GetLoginServer(restrictedMediaFqid.ServerId).RestrictedMediaDelete(restrictedMediaFqid.ObjectId), restrictedMediaFqid.ServerId);
	}

	public override RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(ServerId serverId, IEnumerable<FQID> devices, DateTime timeStart)
	{
		if (serverId == null)
		{
			throw new ArgumentNullMIPException("serverId");
		}
		if (devices == null)
		{
			throw new ArgumentNullMIPException("devices");
		}
		if (!devices.Any() || devices.Any((FQID d) => d == null))
		{
			throw new ArgumentMIPException("Invalid or no devices specified. (devices)");
		}
		ILoginServer loginServer = GetLoginServer(serverId);
		Guid[] deviceIds = devices.Select((FQID d) => d.ObjectId).ToArray();
		return ConvertToPlatformRestrictedMediaLiveResult(loginServer.RestrictedMediaLiveModeEnter(deviceIds, timeStart), serverId);
	}

	public override RestrictedMediaResult RestrictedMediaLiveModeExit(ServerId serverId, IEnumerable<FQID> devices, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		if (serverId == null)
		{
			throw new ArgumentNullMIPException("serverId");
		}
		if (devices == null)
		{
			throw new ArgumentNullMIPException("devices");
		}
		if (!devices.Any() || devices.Any((FQID d) => d == null))
		{
			throw new ArgumentMIPException("Invalid or no devices specified. (devices)");
		}
		ILoginServer loginServer = GetLoginServer(serverId);
		Guid restrictedMediaId = Guid.NewGuid();
		Guid[] deviceIds = devices.Select((FQID d) => d.ObjectId).ToArray();
		return ConvertToPlatformRestrictedMediaResult(loginServer.RestrictedMediaLiveModeExit(restrictedMediaId, deviceIds, header, description, timeStart, timeEnd), serverId);
	}

	public override RestrictedMediaLive[] RestrictedMediaLiveQueryAll(ServerId serverId)
	{
		if (serverId == null)
		{
			throw new ArgumentNullMIPException("serverId");
		}
		return (from rml in GetLoginServer(serverId).RestrictedMediaLiveQueryAll()
			select ConvertToPlatformRestrictedMediaLive(rml, serverId)).ToArray();
	}

	public override RestrictedMediaLiveResult RestrictedMediaLiveUpdate(ServerId serverId, IEnumerable<FQID> devices, DateTime timeStart)
	{
		if (serverId == null)
		{
			throw new ArgumentNullMIPException("serverId");
		}
		if (devices == null)
		{
			throw new ArgumentNullMIPException("devices");
		}
		if (!devices.Any() || devices.Any((FQID d) => d == null))
		{
			throw new ArgumentMIPException("Invalid or no devices specified. (devices)");
		}
		ILoginServer loginServer = GetLoginServer(serverId);
		IEnumerable<Guid> source = devices.Select((FQID d) => d.ObjectId);
		return ConvertToPlatformRestrictedMediaLiveResult(loginServer.RestrictedMediaLiveUpdate(source.ToArray(), timeStart), serverId);
	}

	public override RestrictedMediaLiveResult RestrictedMediaLiveDelete(ServerId serverId, IEnumerable<FQID> devices)
	{
		if (serverId == null)
		{
			throw new ArgumentNullMIPException("serverId");
		}
		if (devices == null)
		{
			throw new ArgumentNullMIPException("devices");
		}
		if (!devices.Any() || devices.Any((FQID d) => d == null))
		{
			throw new ArgumentMIPException("Invalid or no devices specified. (devices)");
		}
		ILoginServer loginServer = GetLoginServer(serverId);
		Guid[] deviceIds = devices.Select((FQID d) => d.ObjectId).ToArray();
		return ConvertToPlatformRestrictedMediaLiveResult(loginServer.RestrictedMediaLiveDelete(deviceIds), serverId);
	}

	private static RestrictedMediaResult ConvertToPlatformRestrictedMediaResult(IRestrictedMediaResult restrictedMediaResult, ServerId serverId)
	{
		IRestrictedMedia restrictedMedia = restrictedMediaResult.RestrictedMedia;
		return new RestrictedMediaResult
		{
			Status = ConvertToPlatformResultStatus(restrictedMediaResult.Status),
			RestrictedMedia = ((restrictedMedia != null) ? ConvertToPlatformRestrictedMedia(restrictedMedia, serverId) : null),
			FaultDevices = (restrictedMediaResult.FaultDevices?.Select((IFaultDevice fd) => ConvertToPlatformFaultDevice(fd, serverId)).ToArray() ?? Array.Empty<FaultDevice>()),
			WarningDevices = (restrictedMediaResult.WarningDevices?.Select((IWarningDevice wd) => ConvertToPlatformWarningDevice(wd, serverId)).ToArray() ?? Array.Empty<WarningDevice>())
		};
	}

	private static RestrictedMediaLiveResult ConvertToPlatformRestrictedMediaLiveResult(IRestrictedMediaLiveResult restrictedMediaLiveResult, ServerId serverId)
	{
		return new RestrictedMediaLiveResult
		{
			Status = ConvertToPlatformResultStatus(restrictedMediaLiveResult.Status),
			RestrictedMedia = (restrictedMediaLiveResult.RestrictedMedia?.Select((IRestrictedMediaLive rm) => ConvertToPlatformRestrictedMediaLive(rm, serverId)).ToArray() ?? Array.Empty<RestrictedMediaLive>()),
			FaultDevices = (restrictedMediaLiveResult.FaultDevices?.Select((IFaultDevice fd) => ConvertToPlatformFaultDevice(fd, serverId)).ToArray() ?? Array.Empty<FaultDevice>()),
			WarningDevices = (restrictedMediaLiveResult.WarningDevices?.Select((IWarningDevice wd) => ConvertToPlatformWarningDevice(wd, serverId)).ToArray() ?? Array.Empty<WarningDevice>())
		};
	}

	private static VideoOS.Platform.Data.RestrictedMediaResultStatus ConvertToPlatformResultStatus(VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus status)
	{
		return status switch
		{
			VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus.Undefined => VideoOS.Platform.Data.RestrictedMediaResultStatus.Undefined, 
			VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus.Success => VideoOS.Platform.Data.RestrictedMediaResultStatus.Success, 
			VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus.Failed => VideoOS.Platform.Data.RestrictedMediaResultStatus.Failed, 
			VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus.PartlySuccess => VideoOS.Platform.Data.RestrictedMediaResultStatus.PartlySuccess, 
			VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus.RestrictedMediaNotFound => VideoOS.Platform.Data.RestrictedMediaResultStatus.RestrictedMediaNotFound, 
			VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus.FeatureNotLicensed => VideoOS.Platform.Data.RestrictedMediaResultStatus.FeatureNotLicensed, 
			VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus.RecorderNotFound => VideoOS.Platform.Data.RestrictedMediaResultStatus.RecorderNotFound, 
			VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus.ManagementServerUnavailable => VideoOS.Platform.Data.RestrictedMediaResultStatus.ManagementServerUnavailable, 
			VideoOS.Platform.SDK.Interface.RestrictedMediaResultStatus.Unauthorized => VideoOS.Platform.Data.RestrictedMediaResultStatus.Unauthorized, 
			_ => VideoOS.Platform.Data.RestrictedMediaResultStatus.Undefined, 
		};
	}

	private static RestrictedMedia ConvertToPlatformRestrictedMedia(IRestrictedMedia restrictedMedia, ServerId serverId)
	{
		if (restrictedMedia == null)
		{
			throw new ArgumentNullMIPException("restrictedMedia");
		}
		if (serverId == null)
		{
			throw new ArgumentNullMIPException("serverId");
		}
		FQID restrictedMediaFQID = RestrictedMedia.MakeFQID(serverId, restrictedMedia.Id);
		IEnumerable<Guid> enumerable = restrictedMedia?.DeviceIds;
		return new RestrictedMedia(restrictedMediaFQID, GetDeviceFQIDs(enumerable ?? Enumerable.Empty<Guid>(), serverId), restrictedMedia.Header, restrictedMedia.Description, restrictedMedia.UserName, restrictedMedia.LastModified, restrictedMedia.StartTime, restrictedMedia.EndTime);
	}

	private static RestrictedMediaLive ConvertToPlatformRestrictedMediaLive(IRestrictedMediaLive restrictedMediaLive, ServerId serverId)
	{
		if (restrictedMediaLive == null)
		{
			throw new ArgumentNullMIPException("restrictedMediaLive");
		}
		return new RestrictedMediaLive
		{
			Device = GetDeviceFQID(restrictedMediaLive.DeviceId, serverId),
			LastModified = restrictedMediaLive.LastModified,
			StartTime = restrictedMediaLive.StartTime,
			UserName = restrictedMediaLive.UserName
		};
	}

	private static WarningDevice ConvertToPlatformWarningDevice(IWarningDevice warningDevice, ServerId serverId)
	{
		if (warningDevice == null)
		{
			throw new ArgumentNullMIPException("warningDevice");
		}
		return new WarningDevice
		{
			FQID = GetDeviceFQID(warningDevice.DeviceId, serverId),
			ErrorCode = warningDevice.ErrorCode,
			Message = warningDevice.Message
		};
	}

	private static FaultDevice ConvertToPlatformFaultDevice(IFaultDevice faultDevice, ServerId serverId)
	{
		if (faultDevice == null)
		{
			throw new ArgumentNullMIPException("faultDevice");
		}
		return new FaultDevice
		{
			FQID = GetDeviceFQID(faultDevice.DeviceId, serverId),
			ErrorCode = faultDevice.ErrorCode,
			Message = faultDevice.Message
		};
	}

	private static ServerId GetServerId(ServerId serverId)
	{
		if (serverId.ServerType == "XPCO")
		{
			return serverId;
		}
		SDKConfiguration sDKConfiguration = ((serverId.UserContext != null) ? ((SDKConfiguration)serverId.UserContext.Configuration) : (Configuration.Instance as SDKConfiguration));
		if (sDKConfiguration == null)
		{
			return null;
		}
		Item item = sDKConfiguration.GetItem(serverId.Id, Kind.Server);
		int num = 5;
		while (item != null && item.FQID.ServerId.ServerType != "XPCO" && num-- > 0)
		{
			item = item.GetParent();
		}
		return item?.FQID.ServerId;
	}

	private static ILoginServer GetLoginServer(ServerId serverId)
	{
		ServerId serverId2 = GetServerId(serverId);
		foreach (ISystemInfo value in (((SDKConfiguration)serverId.UserContext.Configuration) ?? throw new MIPException("Server configuration not found.")).SystemInfoList.Values)
		{
			if (value.Populate() && value.Id == serverId2.Id)
			{
				return value.LoginServer;
			}
		}
		throw new ServerNotFoundMIPException(serverId.Uri.AbsoluteUri);
	}

	private static IEnumerable<FQID> GetDeviceFQIDs(IEnumerable<Guid> deviceIds, ServerId serverId)
	{
		return deviceIds.Select((Guid d) => GetDeviceFQID(d, serverId));
	}

	private static FQID GetDeviceFQID(Guid deviceId, ServerId serverId)
	{
		Item item = Configuration.Instance.GetItem(serverId, deviceId, Kind.Camera) ?? Configuration.Instance.GetItem(serverId, deviceId, Kind.Microphone) ?? Configuration.Instance.GetItem(serverId, deviceId, Kind.Speaker) ?? Configuration.Instance.GetItem(serverId, deviceId, Kind.Metadata);
		if (item?.FQID == null)
		{
			EnvironmentManager.Instance.Log(typeof(SDKRestrictedMediaService).Name, error: true, "SDKRestrictedMediaService", "A device item with the specified id: {0} was not found in the configuration.", new object[1] { deviceId });
		}
		return item?.FQID;
	}
}
