using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[FunctionHandler("QueryRecordingServerMarkedData")]
[FunctionHandler("QueryRecorderHasMediaForMovedDevicesWithEvidenceLocks")]
public class MarkedDataGeneric : IGenericFunction
{
	public static GenericObject2 QueryMarkedDataById(IGeminiRequestContext requestContext, string id)
	{
		return ConvertToMarkedDataGenericObject2(MarkedData.QueryMarkedDataItem(requestContext, id));
	}

	public GenericObject2[] Execute(IGeminiRequestContext requestContext, string functionName, GenericObject2[] parameters, int clientVersion)
	{
		if (!(functionName == "QueryRecordingServerMarkedData"))
		{
			if (functionName == "QueryRecorderHasMediaForMovedDevicesWithEvidenceLocks")
			{
				return RecorderHasLockedDeviceDataDistributions(requestContext, parameters);
			}
			throw VmoExceptionFactory.VMO60311_VmoFunctionNotSupportedException(functionName ?? "<Null>");
		}
		return QueryRecordingServerMarkedData(requestContext, parameters);
	}

	private static GenericObject2[] QueryRecordingServerMarkedData(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		string property = GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "Id");
		return ConvertToMarkedDataGenericObject2s(MarkedData.QueryRecordingServerMarkedData(requestContext, property));
	}

	private static GenericObject2[] RecorderHasLockedDeviceDataDistributions(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		List<GenericObject2> list = new List<GenericObject2>();
		string property = GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "Id");
		if (MarkedData.RecorderHasLockedDeviceDataDistributions(requestContext, property))
		{
			list.Add(new GenericObject2(property, null, new ObjectProperty2[0], null));
		}
		return list.ToArray();
	}

	public static GenericObject2[] QueryBookmarksSearch(IGeminiRequestContext requestContext, DateTime centerTime, DateTime timeLimit, int countLimit, string[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, string optFromBookmarkId)
	{
		DeviceType[] deviceTypes2 = deviceTypes.Select((string f) => (DeviceType)Enum.Parse(typeof(DeviceType), f)).ToArray();
		TimeSpan timeLimit2 = timeLimit - centerTime;
		return ConvertToMarkedDataGenericObject2s(MarkedData.QueryBookmarksSearch(requestContext, centerTime, timeLimit2, countLimit, deviceTypes2, optDeviceIds, optUsers, optSearchStr, searchHeader: true, searchDescription: true, searchReference: true, searchUser: true, null, null, optFromBookmarkId));
	}

	public static GenericObject2[] QueryBookmarksSearch2(IGeminiRequestContext requestContext, DateTime centerTime, DateTime timeLimit, int countLimit, string[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string optFromBookmarkId)
	{
		DeviceType[] deviceTypes2 = deviceTypes.Select((string f) => (DeviceType)Enum.Parse(typeof(DeviceType), f)).ToArray();
		TimeSpan timeLimit2 = timeLimit - centerTime;
		return ConvertToMarkedDataGenericObject2s(MarkedData.QueryBookmarksSearch(requestContext, centerTime, timeLimit2, countLimit, deviceTypes2, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, optFromBookmarkId));
	}

	public static GenericObject2[] QueryMarkedDataSearch(GeminiRequestContext requestContext, string[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		return ConvertToMarkedDataGenericObject2s(MarkedData.QueryMarkedDataSearch(requestContext, deviceIds, searchText, searchHeader: true, searchDescription: true, searchReference: true, searchUser: true, users, null, null, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFromTime, retentionExpireToTime, pageIndex, pageSize, sortOrder, sortAscending));
	}

	public static GenericObject2[] QueryMarkedDataSearch2(GeminiRequestContext requestContext, string[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		return ConvertToMarkedDataGenericObject2s(MarkedData.QueryMarkedDataSearch(requestContext, deviceIds, searchText, searchHeader, searchDescription, searchReference, searchUser, users, headerFilter, descriptionFilter, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFromTime, retentionExpireToTime, pageIndex, pageSize, sortOrder, sortAscending));
	}

	public static GenericObject2 CreateMarkedData(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		MarkedDataDataContract markedDataDataContract = ConvertToMarkedData(properties);
		markedDataDataContract = MarkedData.CreateMarkedData(requestContext, markedDataDataContract.Id, markedDataDataContract.DeviceIds, markedDataDataContract.Reference, markedDataDataContract.Header, markedDataDataContract.Description, markedDataDataContract.MarkedDataType, markedDataDataContract.StartTime, markedDataDataContract.TagTime, markedDataDataContract.EndTime, markedDataDataContract.UseRetention, markedDataDataContract.RetentionType, markedDataDataContract.RetentionUnits, markedDataDataContract.RetentionExpire);
		return ConvertToMarkedDataGenericObject2(markedDataDataContract);
	}

	public static GenericObject2 UpdateMarkedData(IGeminiRequestContext requestContext, ObjectProperty2[] properties)
	{
		MarkedDataDataContract markedDataDataContract = ConvertToMarkedData(properties);
		markedDataDataContract = MarkedData.UpdateMarkedData(requestContext, markedDataDataContract.Id, markedDataDataContract.MarkedDataType, markedDataDataContract.DeviceIds, markedDataDataContract.Reference, markedDataDataContract.Header, markedDataDataContract.Description, markedDataDataContract.StartTime, markedDataDataContract.TagTime, markedDataDataContract.EndTime, markedDataDataContract.UseRetention, markedDataDataContract.RetentionType, markedDataDataContract.RetentionUnits, markedDataDataContract.RetentionExpire);
		return ConvertToMarkedDataGenericObject2(markedDataDataContract);
	}

	public static void DeleteMarkedData(IGeminiRequestContext requestContext, string markedDataId, MarkedDataDefinitions.MarkedDataTypes markedDataType)
	{
		MarkedData.DeleteMarkedData(requestContext, markedDataId, markedDataType);
	}

	public static void DeleteBookmarksByDeviceIdBeforeTime(IGeminiRequestContext requestContext, MediaStatusEntry mediaStatus)
	{
		MarkedData.DeleteBookmarksByDeviceIdBeforeTime(requestContext, mediaStatus);
	}

	public static MarkedDataReference2 QueryMarkedDataReference(IGeminiRequestContext requestContext, string[] deviceIds, bool realTime, MarkedDataDefinitions.MarkedDataTypes markedDataTypes)
	{
		MarkedDataReference markedDataReference = MarkedData.QueryMarkedDataReference(requestContext, deviceIds, realTime, markedDataTypes);
		return new MarkedDataReference2
		{
			Reference = markedDataReference.Reference,
			Time = markedDataReference.Time,
			Devices = deviceIds
		};
	}

	private static GenericObject2[] ConvertToMarkedDataGenericObject2s(MarkedDataDataContract[] markedData)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (MarkedDataDataContract markedData2 in markedData)
		{
			list.Add(ConvertToMarkedDataGenericObject2(markedData2));
		}
		return list.ToArray();
	}

	private static GenericObject2 ConvertToMarkedDataGenericObject2(MarkedDataDataContract markedData)
	{
		if (markedData == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", markedData.Id));
		list.Add(new ObjectProperty2("DeviceIds", string.Join(",", markedData.DeviceIds)));
		list.Add(new ObjectProperty2("Description", markedData.Description));
		list.Add(new ObjectProperty2("Reference", markedData.Reference));
		list.Add(new ObjectProperty2("UserName", markedData.UserName));
		list.Add(new ObjectProperty2("Header", markedData.Header));
		list.Add(new ObjectProperty2("MarkedDataType", markedData.MarkedDataType));
		list.Add(new ObjectProperty2("StartTime", markedData.StartTime));
		list.Add(new ObjectProperty2("TagTime", markedData.TagTime));
		list.Add(new ObjectProperty2("EndTime", markedData.EndTime));
		list.Add(new ObjectProperty2("UseRetention", markedData.MarkedDataType == 2));
		list.Add(new ObjectProperty2("RetentionType", markedData.RetentionType));
		list.Add(new ObjectProperty2("RetentionUnits", markedData.RetentionUnits));
		list.Add(new ObjectProperty2("RetentionExpire", markedData.RetentionExpire));
		list.Add(new ObjectProperty2("RetentionSize", markedData.RetentionSize));
		ObjectProperty2[] propertyValue = markedData.StickerSizes.Select((StickerSize2 ss) => new ObjectProperty2(ss.DeviceId.ToString(), ss.Size.ToString(CultureInfo.InvariantCulture))).ToArray();
		list.Add(new ObjectProperty2("StickerSizes", propertyValue));
		return new GenericObject2(markedData.Id, "MarkedData", list.ToArray(), null);
	}

	private static MarkedDataDataContract ConvertToMarkedData(ObjectProperty2[] properties)
	{
		MarkedDataDataContract markedDataDataContract = new MarkedDataDataContract();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			markedDataDataContract.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "DeviceIds"))
		{
			markedDataDataContract.DeviceIds = GenericObjectHelper2.GetProperty<string>(properties, "DeviceIds").Split(',');
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			markedDataDataContract.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "Reference"))
		{
			markedDataDataContract.Reference = GenericObjectHelper2.GetProperty<string>(properties, "Reference");
		}
		if (GenericObjectHelper2.Contains(properties, "UserName"))
		{
			markedDataDataContract.UserName = GenericObjectHelper2.GetProperty<string>(properties, "UserName");
		}
		if (GenericObjectHelper2.Contains(properties, "Header"))
		{
			markedDataDataContract.Header = GenericObjectHelper2.GetProperty<string>(properties, "Header");
		}
		if (GenericObjectHelper2.Contains(properties, "MarkedDataType"))
		{
			markedDataDataContract.MarkedDataType = GenericObjectHelper2.GetProperty<int>(properties, "MarkedDataType");
		}
		if (GenericObjectHelper2.Contains(properties, "StartTime"))
		{
			markedDataDataContract.StartTime = GenericObjectHelper2.GetProperty<DateTime>(properties, "StartTime");
		}
		if (GenericObjectHelper2.Contains(properties, "TagTime"))
		{
			markedDataDataContract.TagTime = GenericObjectHelper2.GetProperty<DateTime>(properties, "TagTime");
		}
		if (GenericObjectHelper2.Contains(properties, "EndTime"))
		{
			markedDataDataContract.EndTime = GenericObjectHelper2.GetProperty<DateTime>(properties, "EndTime");
		}
		if (GenericObjectHelper2.Contains(properties, "UseRetention"))
		{
			markedDataDataContract.UseRetention = GenericObjectHelper2.GetProperty<bool>(properties, "UseRetention");
		}
		if (GenericObjectHelper2.Contains(properties, "RetentionType"))
		{
			markedDataDataContract.RetentionType = GenericObjectHelper2.GetProperty<int>(properties, "RetentionType");
		}
		if (GenericObjectHelper2.Contains(properties, "RetentionUnits"))
		{
			markedDataDataContract.RetentionUnits = GenericObjectHelper2.GetProperty<int>(properties, "RetentionUnits");
		}
		if (GenericObjectHelper2.Contains(properties, "RetentionExpire"))
		{
			markedDataDataContract.RetentionExpire = GenericObjectHelper2.GetProperty<DateTime>(properties, "RetentionExpire");
		}
		if (GenericObjectHelper2.Contains(properties, "RetentionSize"))
		{
			markedDataDataContract.RetentionSize = GenericObjectHelper2.GetProperty<long>(properties, "RetentionSize");
		}
		if (GenericObjectHelper2.Contains(properties, "StickerSizes"))
		{
			ObjectProperty2[] nestedProperties = GenericObjectHelper2.GetNestedProperties(properties, "StickerSizes");
			markedDataDataContract.StickerSizes = nestedProperties.Select((ObjectProperty2 op) => new StickerSize2(new Guid(op.Name), long.Parse((string)op.Value, CultureInfo.InvariantCulture)));
		}
		return markedDataDataContract;
	}
}
