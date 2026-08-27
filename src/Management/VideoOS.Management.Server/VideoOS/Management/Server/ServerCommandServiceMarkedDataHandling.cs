using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.BusinessObjects;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public static class ServerCommandServiceMarkedDataHandling
{
	public static VideoOS.WebService.Common.ServerCommandService.MarkedDataReference MarkedDataGetNewReference(IGeminiRequestContext requestContext, Guid[] deviceIds, bool live)
	{
		try
		{
			MarkedDataReference markedDataReference = MarkedData.QueryMarkedDataReference(requestContext, deviceIds.Select((Guid d) => d.ToString()).ToArray(), live, MarkedDataDefinitions.MarkedDataTypes.MarkedData);
			return new VideoOS.WebService.Common.ServerCommandService.MarkedDataReference
			{
				Reference = markedDataReference.Reference,
				Time = markedDataReference.Time
			};
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static MarkedDataResult MarkedDataCreate(IGeminiRequestContext requestContext, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, bool useRetention, string reference, string header, string description, int markedDataType, DateTime retentionExpire, RetentionOption retentionOption)
	{
		if (retentionOption == null)
		{
			throw new ArgumentNullException("retentionOption");
		}
		if (markedDataType != 2)
		{
			throw new ArgumentException("Invalid value.", "markedDataType");
		}
		List<VideoOS.Management.Server.BusinessObjects.FaultDevice> faultDevices = new List<VideoOS.Management.Server.BusinessObjects.FaultDevice>();
		List<VideoOS.Management.Server.BusinessObjects.WarningDevice> warningDevices = new List<VideoOS.Management.Server.BusinessObjects.WarningDevice>();
		try
		{
			MarkedDataDataContract markedData = MarkedData.CreateMarkedData(requestContext, markedDataId.ToString(), deviceIds.Select((Guid d) => d.ToString()).ToArray(), reference, header, description, 2, timeStart, timeTag, timeEnd, useRetention, (int)retentionOption.RetentionOptionType, retentionOption.RetentionUnits, retentionExpire, out faultDevices, out warningDevices);
			VideoOS.WebService.Common.ServerCommandService.FaultDevice[] array = FaultOrWarningDeviceConverter.ToScsFaultDevice(faultDevices);
			VideoOS.WebService.Common.ServerCommandService.WarningDevice[] warningDevices2 = FaultOrWarningDeviceConverter.ToScsWarningDevice(warningDevices);
			VideoOS.WebService.Common.ServerCommandService.MarkedData markedData2 = ConvertVmo2ScsMarkedData(markedData);
			return new MarkedDataResult
			{
				FaultDevices = array,
				WarningDevices = warningDevices2,
				MarkedData = markedData2,
				Status = ((array.Length == 0) ? ResultStatus.Success : ((deviceIds != null && array.Length == deviceIds.Length) ? ResultStatus.Failed : ResultStatus.PartlySuccess))
			};
		}
		catch (MarkedDataDevicesFailedException)
		{
			VideoOS.WebService.Common.ServerCommandService.FaultDevice[] array2 = FaultOrWarningDeviceConverter.ToScsFaultDevice(faultDevices);
			VideoOS.WebService.Common.ServerCommandService.MarkedData markedData3 = new VideoOS.WebService.Common.ServerCommandService.MarkedData
			{
				Id = markedDataId
			};
			return new MarkedDataResult
			{
				FaultDevices = array2,
				MarkedData = markedData3,
				Status = ((array2.Length == 0) ? ResultStatus.Success : ((deviceIds != null && array2.Length == deviceIds.Length) ? ResultStatus.Failed : ResultStatus.PartlySuccess))
			};
		}
		catch (FeatureNotLicensedException)
		{
			if (deviceIds == null)
			{
				new WcfWebServiceManagerExceptionHandler().HandleException(new ArgumentNullException("deviceIds"), null);
				throw;
			}
			VideoOS.WebService.Common.ServerCommandService.MarkedData markedData4 = new VideoOS.WebService.Common.ServerCommandService.MarkedData
			{
				Id = markedDataId
			};
			return new MarkedDataResult
			{
				FaultDevices = null,
				MarkedData = markedData4,
				Status = ResultStatus.FeatureNotLicensed
			};
		}
		catch (UnauthorizedAccessException)
		{
			VideoOS.WebService.Common.ServerCommandService.FaultDevice[] faultDevices2 = ((faultDevices != null && faultDevices.Count != 0) ? FaultOrWarningDeviceConverter.ToScsFaultDevice(faultDevices) : null);
			return new MarkedDataResult
			{
				FaultDevices = faultDevices2,
				MarkedData = new VideoOS.WebService.Common.ServerCommandService.MarkedData
				{
					Id = markedDataId
				},
				Status = ResultStatus.Unauthorized
			};
		}
		catch (Exception ex4)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex4, null);
			throw;
		}
	}

	public static MarkedDataResult MarkedDataUpdate(IGeminiRequestContext requestContext, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, bool useRetention, string reference, string header, string description, int markedDataType, DateTime retentionExpire, RetentionOption retentionOption)
	{
		if (retentionOption == null)
		{
			throw new ArgumentNullException("retentionOption");
		}
		if (markedDataType != 2)
		{
			throw new ArgumentException("Invalid value.", "markedDataType");
		}
		List<VideoOS.Management.Server.BusinessObjects.FaultDevice> faultDevices = new List<VideoOS.Management.Server.BusinessObjects.FaultDevice>();
		List<VideoOS.Management.Server.BusinessObjects.WarningDevice> warningDevices = new List<VideoOS.Management.Server.BusinessObjects.WarningDevice>();
		try
		{
			MarkedDataDataContract markedData = MarkedData.UpdateMarkedData(requestContext, markedDataId.ToString(), 2, deviceIds.Select((Guid d) => d.ToString()).ToArray(), reference, header, description, timeStart, timeTag, timeEnd, useRetention, (int)retentionOption.RetentionOptionType, retentionOption.RetentionUnits, retentionExpire, out faultDevices, out warningDevices);
			VideoOS.WebService.Common.ServerCommandService.FaultDevice[] array = FaultOrWarningDeviceConverter.ToScsFaultDevice(faultDevices);
			VideoOS.WebService.Common.ServerCommandService.WarningDevice[] warningDevices2 = FaultOrWarningDeviceConverter.ToScsWarningDevice(warningDevices);
			VideoOS.WebService.Common.ServerCommandService.MarkedData markedData2 = ConvertVmo2ScsMarkedData(markedData);
			return new MarkedDataResult
			{
				FaultDevices = array,
				WarningDevices = warningDevices2,
				MarkedData = markedData2,
				Status = ((array.Length == 0) ? ResultStatus.Success : ((deviceIds != null && array.Length == deviceIds.Length) ? ResultStatus.Failed : ResultStatus.PartlySuccess))
			};
		}
		catch (MarkedDataDevicesFailedException)
		{
			VideoOS.WebService.Common.ServerCommandService.FaultDevice[] array2 = FaultOrWarningDeviceConverter.ToScsFaultDevice(faultDevices);
			VideoOS.WebService.Common.ServerCommandService.MarkedData markedData3 = new VideoOS.WebService.Common.ServerCommandService.MarkedData
			{
				Id = markedDataId
			};
			return new MarkedDataResult
			{
				FaultDevices = array2,
				MarkedData = markedData3,
				Status = ((array2.Length == 0) ? ResultStatus.Success : ((deviceIds != null && array2.Length == deviceIds.Length) ? ResultStatus.Failed : ResultStatus.PartlySuccess))
			};
		}
		catch (MarkedDataNotFoundException)
		{
			VideoOS.WebService.Common.ServerCommandService.MarkedData markedData4 = new VideoOS.WebService.Common.ServerCommandService.MarkedData
			{
				Id = markedDataId
			};
			return new MarkedDataResult
			{
				FaultDevices = null,
				MarkedData = markedData4,
				Status = ResultStatus.MarkedDataNotFound
			};
		}
		catch (FeatureNotLicensedException)
		{
			if (deviceIds == null)
			{
				new WcfWebServiceManagerExceptionHandler().HandleException(new ArgumentNullException("deviceIds"), null);
				throw;
			}
			VideoOS.WebService.Common.ServerCommandService.MarkedData markedData5 = new VideoOS.WebService.Common.ServerCommandService.MarkedData
			{
				Id = markedDataId
			};
			return new MarkedDataResult
			{
				FaultDevices = null,
				MarkedData = markedData5,
				Status = ResultStatus.FeatureNotLicensed
			};
		}
		catch (UnauthorizedAccessException)
		{
			return new MarkedDataResult
			{
				FaultDevices = null,
				MarkedData = new VideoOS.WebService.Common.ServerCommandService.MarkedData
				{
					Id = markedDataId
				},
				Status = ResultStatus.Unauthorized
			};
		}
		catch (Exception ex5)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex5, null);
			throw;
		}
	}

	public static MarkedDataResult MarkedDataDelete(IGeminiRequestContext requestContext, Guid markedDataId)
	{
		try
		{
			MarkedData.DeleteMarkedData(requestContext, markedDataId.ToString(), MarkedDataDefinitions.MarkedDataTypes.MarkedData);
			return new MarkedDataResult
			{
				FaultDevices = Array.Empty<VideoOS.WebService.Common.ServerCommandService.FaultDevice>(),
				WarningDevices = new VideoOS.WebService.Common.ServerCommandService.WarningDevice[0],
				MarkedData = new VideoOS.WebService.Common.ServerCommandService.MarkedData
				{
					Id = markedDataId
				},
				Status = ResultStatus.Success
			};
		}
		catch (MarkedDataNotFoundException)
		{
			return new MarkedDataResult
			{
				FaultDevices = null,
				MarkedData = new VideoOS.WebService.Common.ServerCommandService.MarkedData
				{
					Id = markedDataId
				},
				Status = ResultStatus.MarkedDataNotFound
			};
		}
		catch (FeatureNotLicensedException)
		{
			return new MarkedDataResult
			{
				FaultDevices = null,
				MarkedData = new VideoOS.WebService.Common.ServerCommandService.MarkedData
				{
					Id = markedDataId
				},
				Status = ResultStatus.FeatureNotLicensed
			};
		}
		catch (UnauthorizedAccessException)
		{
			return new MarkedDataResult
			{
				FaultDevices = Array.Empty<VideoOS.WebService.Common.ServerCommandService.FaultDevice>(),
				MarkedData = new VideoOS.WebService.Common.ServerCommandService.MarkedData
				{
					Id = markedDataId
				},
				Status = ResultStatus.Unauthorized
			};
		}
		catch (Exception ex4)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex4, null);
			throw;
		}
	}

	public static VideoOS.WebService.Common.ServerCommandService.MarkedData MarkedDataGet(IGeminiRequestContext requestContext, Guid markedDataId)
	{
		try
		{
			return ConvertVmo2ScsMarkedData(MarkedData.QueryMarkedDataItem(requestContext, markedDataId.ToString(), 2));
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static VideoOS.WebService.Common.ServerCommandService.MarkedData[] MarkedDataSearch(IGeminiRequestContext requestContext, Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		try
		{
			string[] deviceIds2 = deviceIds?.Select((Guid id) => id.ToString()).ToArray();
			MarkedDataDataContract[] array = MarkedData.QueryMarkedDataSearch(requestContext, deviceIds2, searchText, searchHeader, searchDescription, searchReference, searchUser, users, headerFilter, descriptionFilter, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder.ToString(), sortAscending);
			List<VideoOS.WebService.Common.ServerCommandService.MarkedData> list = new List<VideoOS.WebService.Common.ServerCommandService.MarkedData>();
			MarkedDataDataContract[] array2 = array;
			foreach (MarkedDataDataContract markedData in array2)
			{
				list.Add(ConvertVmo2ScsMarkedData(markedData));
			}
			return list.ToArray();
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static bool RecordingServerContainMarkedData(IGeminiRequestContext requestContext, string recordingServerId)
	{
		try
		{
			return MarkedData.RecordingServerContainMarkedData(requestContext, recordingServerId);
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	private static VideoOS.WebService.Common.ServerCommandService.MarkedData ConvertVmo2ScsMarkedData(MarkedDataDataContract markedData)
	{
		return new VideoOS.WebService.Common.ServerCommandService.MarkedData
		{
			Id = new Guid(markedData.Id),
			DeviceIds = markedData.DeviceIds.Select((string d) => new Guid(d)).ToArray(),
			Created = markedData.Created,
			LastModified = markedData.LastModified,
			User = markedData.UserName,
			Reference = markedData.Reference,
			Header = markedData.Header,
			Description = markedData.Description,
			StartTime = markedData.StartTime,
			TagTime = markedData.TagTime,
			EndTime = markedData.EndTime,
			UseRetention = (markedData.MarkedDataType == 2),
			RetentionOption = 
			{
				RetentionOptionType = (RetentionOptionType)markedData.RetentionType,
				RetentionUnits = markedData.RetentionUnits
			},
			RetentionExpire = markedData.RetentionExpire,
			RetentionSize = markedData.RetentionSize
		};
	}
}
