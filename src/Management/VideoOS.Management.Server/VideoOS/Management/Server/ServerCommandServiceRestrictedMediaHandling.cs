using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.BusinessObjects;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public static class ServerCommandServiceRestrictedMediaHandling
{
	public static RestrictedMediaResult RestrictedMediaCreate(IGeminiRequestContext requestContext, Guid id, Guid[] deviceIds, string header, string description, DateTime startTime, DateTime endTime)
	{
		return CallWithExceptionHandling(id, deviceIds, (List<VideoOS.Management.Server.BusinessObjects.FaultDevice> faultDevices, List<VideoOS.Management.Server.BusinessObjects.WarningDevice> warningDevices) => CreatePlaybackResult(RestrictedMedia.CreatePlayback(requestContext, id.ToString(), deviceIds.Select((Guid d) => d.ToString()).ToArray(), header, description, startTime, endTime, faultDevices), faultDevices, warningDevices, deviceIds));
	}

	public static RestrictedMediaResult RestrictedMediaUpdate(IGeminiRequestContext requestContext, Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime startTime, DateTime endTime)
	{
		return CallWithExceptionHandling(id, null, delegate(List<VideoOS.Management.Server.BusinessObjects.FaultDevice> faultDevices, List<VideoOS.Management.Server.BusinessObjects.WarningDevice> warningDevices)
		{
			RestrictedMediaDataContract restrictedMedia = RestrictedMedia.UpdatePlayback(requestContext, id.ToString(), deviceIdsToAdd.Select((Guid d) => d.ToString()).ToArray(), deviceIdsToRemove.Select((Guid d) => d.ToString()).ToArray(), header, description, startTime, endTime, faultDevices);
			return new RestrictedMediaResult
			{
				FaultDevices = FaultOrWarningDeviceConverter.ToScsFaultDevice(faultDevices),
				WarningDevices = FaultOrWarningDeviceConverter.ToScsWarningDevice(warningDevices),
				RestrictedMedia = ConvertVmo2ScsRestrictedMedia(restrictedMedia),
				Status = RestrictedMediaResultStatus.Success
			};
		});
	}

	public static RestrictedMediaResult RestrictedMediaDelete(IGeminiRequestContext requestContext, Guid id)
	{
		return CallWithExceptionHandling(id, null, delegate
		{
			RestrictedMedia.DeletePlayback(requestContext, id.ToString());
			return new RestrictedMediaResult
			{
				FaultDevices = Array.Empty<VideoOS.WebService.Common.ServerCommandService.FaultDevice>(),
				WarningDevices = Array.Empty<VideoOS.WebService.Common.ServerCommandService.WarningDevice>(),
				RestrictedMedia = new VideoOS.WebService.Common.ServerCommandService.RestrictedMedia
				{
					Id = id
				},
				Status = RestrictedMediaResultStatus.Success
			};
		});
	}

	public static VideoOS.WebService.Common.ServerCommandService.RestrictedMedia[] QueryAll(IGeminiRequestContext requestContext)
	{
		try
		{
			return RestrictedMedia.QueryAllPlayback(requestContext).Select(ConvertVmo2ScsRestrictedMedia).ToArray();
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(IGeminiRequestContext requestContext, Guid[] deviceIds, DateTime startTime)
	{
		return CallWithExceptionHandling(deviceIds, (List<VideoOS.Management.Server.BusinessObjects.FaultDevice> faultDevices, List<VideoOS.Management.Server.BusinessObjects.WarningDevice> warningDevices) => CreateLiveResult(RestrictedMedia.EnterLive(requestContext, deviceIds.Select((Guid d) => d.ToString()).ToArray(), startTime, faultDevices), faultDevices, warningDevices, deviceIds));
	}

	public static RestrictedMediaResult RestrictedMediaLiveModeExit(IGeminiRequestContext requestContext, Guid id, Guid[] deviceIds, string header, string description, DateTime startTime, DateTime endTime)
	{
		return CallWithExceptionHandling(id, deviceIds, (List<VideoOS.Management.Server.BusinessObjects.FaultDevice> faultDevices, List<VideoOS.Management.Server.BusinessObjects.WarningDevice> warningDevices) => CreatePlaybackResult(RestrictedMedia.ExitLive(requestContext, id.ToString(), deviceIds.Select((Guid d) => d.ToString()).ToArray(), header, description, startTime, endTime, faultDevices), faultDevices, warningDevices, deviceIds));
	}

	public static RestrictedMediaLiveResult RestrictedMediaLiveUpdate(IGeminiRequestContext requestContext, Guid[] deviceIds, DateTime startTime)
	{
		return CallWithExceptionHandling(deviceIds, (List<VideoOS.Management.Server.BusinessObjects.FaultDevice> faultDevices, List<VideoOS.Management.Server.BusinessObjects.WarningDevice> warningDevices) => CreateLiveResult(RestrictedMedia.UpdateLive(requestContext, deviceIds.Select((Guid d) => d.ToString()).ToArray(), startTime, faultDevices), faultDevices, warningDevices, deviceIds));
	}

	public static RestrictedMediaLiveResult RestrictedMediaLiveDelete(IGeminiRequestContext requestContext, Guid[] deviceIds)
	{
		return CallWithExceptionHandling(deviceIds, delegate(List<VideoOS.Management.Server.BusinessObjects.FaultDevice> faultDevices, List<VideoOS.Management.Server.BusinessObjects.WarningDevice> warningDevices)
		{
			IEnumerable<Guid> deviceIds2 = RestrictedMedia.DeleteLive(requestContext, deviceIds.Select((Guid d) => d.ToString()).ToArray(), faultDevices);
			return new RestrictedMediaLiveResult
			{
				FaultDevices = FaultOrWarningDeviceConverter.ToScsFaultDevice(faultDevices),
				WarningDevices = FaultOrWarningDeviceConverter.ToScsWarningDevice(warningDevices),
				RestrictedMedia = ConvertDeviceIdsToRestrictedMediaLive(deviceIds2),
				Status = CalculateResultStatus(faultDevices, deviceIds)
			};
		});
	}

	public static RestrictedMediaLive[] QueryAllLive(IGeminiRequestContext requestContext)
	{
		try
		{
			return ConvertVmo2ScsRestrictedMediaLive(RestrictedMedia.QueryAllLive(requestContext));
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	private static RestrictedMediaResult CallWithExceptionHandling(Guid id, Guid[] deviceIds, Func<List<VideoOS.Management.Server.BusinessObjects.FaultDevice>, List<VideoOS.Management.Server.BusinessObjects.WarningDevice>, RestrictedMediaResult> callback)
	{
		List<VideoOS.Management.Server.BusinessObjects.FaultDevice> list = new List<VideoOS.Management.Server.BusinessObjects.FaultDevice>();
		List<VideoOS.Management.Server.BusinessObjects.WarningDevice> list2 = new List<VideoOS.Management.Server.BusinessObjects.WarningDevice>();
		try
		{
			return callback(list, list2);
		}
		catch (ObjectNotFoundException)
		{
			return new RestrictedMediaResult
			{
				FaultDevices = null,
				WarningDevices = null,
				RestrictedMedia = new VideoOS.WebService.Common.ServerCommandService.RestrictedMedia
				{
					Id = id
				},
				Status = RestrictedMediaResultStatus.RestrictedMediaNotFound
			};
		}
		catch (RestrictedMediaDevicesFailedException)
		{
			return new RestrictedMediaResult
			{
				FaultDevices = FaultOrWarningDeviceConverter.ToScsFaultDevice(list),
				WarningDevices = FaultOrWarningDeviceConverter.ToScsWarningDevice(list2),
				RestrictedMedia = new VideoOS.WebService.Common.ServerCommandService.RestrictedMedia
				{
					Id = id
				},
				Status = CalculateResultStatus(list, deviceIds)
			};
		}
		catch (RestrictedMediaFailedException)
		{
			return new RestrictedMediaResult
			{
				FaultDevices = FaultOrWarningDeviceConverter.ToScsFaultDevice(list),
				WarningDevices = FaultOrWarningDeviceConverter.ToScsWarningDevice(list2),
				RestrictedMedia = new VideoOS.WebService.Common.ServerCommandService.RestrictedMedia
				{
					Id = id
				},
				Status = RestrictedMediaResultStatus.Failed
			};
		}
		catch (FeatureNotLicensedException)
		{
			return new RestrictedMediaResult
			{
				FaultDevices = null,
				WarningDevices = null,
				RestrictedMedia = new VideoOS.WebService.Common.ServerCommandService.RestrictedMedia
				{
					Id = id
				},
				Status = RestrictedMediaResultStatus.FeatureNotLicensed
			};
		}
		catch (UnauthorizedAccessException)
		{
			return new RestrictedMediaResult
			{
				FaultDevices = ((list.Count != 0) ? FaultOrWarningDeviceConverter.ToScsFaultDevice(list) : null),
				WarningDevices = null,
				RestrictedMedia = new VideoOS.WebService.Common.ServerCommandService.RestrictedMedia
				{
					Id = id
				},
				Status = RestrictedMediaResultStatus.Unauthorized
			};
		}
		catch (Exception ex6)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex6, null);
			throw;
		}
	}

	private static RestrictedMediaResult CreatePlaybackResult(RestrictedMediaDataContract restrictedMedia, List<VideoOS.Management.Server.BusinessObjects.FaultDevice> faultDevices, List<VideoOS.Management.Server.BusinessObjects.WarningDevice> warningDevices, Guid[] deviceIds)
	{
		return new RestrictedMediaResult
		{
			FaultDevices = FaultOrWarningDeviceConverter.ToScsFaultDevice(faultDevices),
			WarningDevices = FaultOrWarningDeviceConverter.ToScsWarningDevice(warningDevices),
			RestrictedMedia = ConvertVmo2ScsRestrictedMedia(restrictedMedia),
			Status = CalculateResultStatus(faultDevices, deviceIds)
		};
	}

	private static VideoOS.WebService.Common.ServerCommandService.RestrictedMedia ConvertVmo2ScsRestrictedMedia(RestrictedMediaDataContract restrictedMedia)
	{
		return new VideoOS.WebService.Common.ServerCommandService.RestrictedMedia
		{
			Id = new Guid(restrictedMedia.Id),
			DeviceIds = restrictedMedia.DeviceIds.Select((string d) => new Guid(d)).ToArray(),
			LastModified = restrictedMedia.LastModified,
			UserName = restrictedMedia.UserName,
			Header = restrictedMedia.Header,
			Description = restrictedMedia.Description,
			StartTime = restrictedMedia.StartTime,
			EndTime = restrictedMedia.EndTime
		};
	}

	private static RestrictedMediaLiveResult CallWithExceptionHandling(Guid[] deviceIds, Func<List<VideoOS.Management.Server.BusinessObjects.FaultDevice>, List<VideoOS.Management.Server.BusinessObjects.WarningDevice>, RestrictedMediaLiveResult> callback)
	{
		List<VideoOS.Management.Server.BusinessObjects.FaultDevice> list = new List<VideoOS.Management.Server.BusinessObjects.FaultDevice>();
		List<VideoOS.Management.Server.BusinessObjects.WarningDevice> arg = new List<VideoOS.Management.Server.BusinessObjects.WarningDevice>();
		try
		{
			return callback(list, arg);
		}
		catch (ObjectNotFoundException)
		{
			return new RestrictedMediaLiveResult
			{
				FaultDevices = null,
				WarningDevices = null,
				RestrictedMedia = ConvertDeviceIdsToRestrictedMediaLive(deviceIds),
				Status = RestrictedMediaResultStatus.RestrictedMediaNotFound
			};
		}
		catch (RestrictedMediaDevicesFailedException)
		{
			return new RestrictedMediaLiveResult
			{
				FaultDevices = FaultOrWarningDeviceConverter.ToScsFaultDevice(list),
				RestrictedMedia = ConvertDeviceIdsToRestrictedMediaLive(deviceIds),
				Status = CalculateResultStatus(list, deviceIds)
			};
		}
		catch (FeatureNotLicensedException)
		{
			return new RestrictedMediaLiveResult
			{
				FaultDevices = null,
				WarningDevices = null,
				RestrictedMedia = ConvertDeviceIdsToRestrictedMediaLive(deviceIds),
				Status = RestrictedMediaResultStatus.FeatureNotLicensed
			};
		}
		catch (UnauthorizedAccessException)
		{
			return new RestrictedMediaLiveResult
			{
				FaultDevices = null,
				WarningDevices = null,
				RestrictedMedia = ConvertDeviceIdsToRestrictedMediaLive(deviceIds),
				Status = RestrictedMediaResultStatus.Unauthorized
			};
		}
		catch (Exception ex5)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex5, null);
			throw;
		}
	}

	private static RestrictedMediaLiveResult CreateLiveResult(RestrictedMediaLiveDataContract[] restrictedMedia, List<VideoOS.Management.Server.BusinessObjects.FaultDevice> faultDevices, List<VideoOS.Management.Server.BusinessObjects.WarningDevice> warningDevices, Guid[] deviceIds)
	{
		return new RestrictedMediaLiveResult
		{
			FaultDevices = FaultOrWarningDeviceConverter.ToScsFaultDevice(faultDevices),
			WarningDevices = FaultOrWarningDeviceConverter.ToScsWarningDevice(warningDevices),
			RestrictedMedia = ConvertVmo2ScsRestrictedMediaLive(restrictedMedia),
			Status = CalculateResultStatus(faultDevices, deviceIds)
		};
	}

	private static RestrictedMediaResultStatus CalculateResultStatus(List<VideoOS.Management.Server.BusinessObjects.FaultDevice> faultDevices, IEnumerable<Guid> deviceIds)
	{
		if (faultDevices.Count != 0)
		{
			if (deviceIds == null || faultDevices.Count != deviceIds.Distinct().Count())
			{
				return RestrictedMediaResultStatus.PartlySuccess;
			}
			return RestrictedMediaResultStatus.Failed;
		}
		return RestrictedMediaResultStatus.Success;
	}

	private static RestrictedMediaLive[] ConvertVmo2ScsRestrictedMediaLive(RestrictedMediaLiveDataContract[] restrictedMediaLiveDataContractCollection)
	{
		List<RestrictedMediaLive> restrictedMediaLiveCollection = new List<RestrictedMediaLive>();
		restrictedMediaLiveDataContractCollection.ToList().ForEach(delegate(RestrictedMediaLiveDataContract rm)
		{
			restrictedMediaLiveCollection.Add(new RestrictedMediaLive
			{
				DeviceId = new Guid(rm.DeviceId),
				LastModified = rm.LastModified,
				UserName = rm.UserName,
				StartTime = rm.StartTime
			});
		});
		return restrictedMediaLiveCollection.ToArray();
	}

	private static RestrictedMediaLive[] ConvertDeviceIdsToRestrictedMediaLive(IEnumerable<Guid> deviceIds)
	{
		return deviceIds?.Select((Guid id) => new RestrictedMediaLive
		{
			DeviceId = id
		}).ToArray();
	}
}
