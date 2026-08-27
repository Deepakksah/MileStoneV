using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Management.Common;
using VideoOS.Management.Server.BusinessObjects;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Management.Server;

public static class MarkedData
{
	private class MarkedDataTypeInfo
	{
		public MarkedDataDefinitions.MarkedDataTypes MarkedDataType { get; }

		public string FeatureFlag { get; }

		public string SecurityActionRead { get; }

		public string SecurityActionCreate { get; }

		public string SecurityActionDelete { get; }

		public IEnumerable<DeviceType> SupportedDeviceTypes { get; }

		public MarkedDataTypeInfo(MarkedDataDefinitions.MarkedDataTypes markedDataType, string featureFlag, DeviceType[] supportedDeviceTypes, string securityActionRead, string securityActionCreate, string securityActionDelete)
		{
			MarkedDataType = markedDataType;
			FeatureFlag = featureFlag;
			SupportedDeviceTypes = supportedDeviceTypes;
			SecurityActionRead = securityActionRead;
			SecurityActionCreate = securityActionCreate;
			SecurityActionDelete = securityActionDelete;
		}
	}

	private enum EvidenceLockResponseCode
	{
		UnknownError,
		None,
		Warning,
		RecorderNotFound,
		LegacyNotSupported,
		MediaDBError,
		Unauthorized,
		EdgeStoragePlaybackError,
		DeviceNotFound,
		ErrorFailoverMode,
		MarkedDataNotSupported
	}

	private enum EvidenceLockWarningCode
	{
		UnknownError,
		RecorderOffline
	}

	private static Dictionary<MarkedDataDefinitions.MarkedDataTypes, MarkedDataTypeInfo> markedDataTypeMap = new List<MarkedDataTypeInfo>(new MarkedDataTypeInfo[2]
	{
		new MarkedDataTypeInfo(MarkedDataDefinitions.MarkedDataTypes.Bookmark, "Bookmarking", new DeviceType[3]
		{
			DeviceType.Camera,
			DeviceType.Microphone,
			DeviceType.Speaker
		}, "READ_BOOKMARKS", "CREATE_BOOKMARKS", "DELETE_BOOKMARKS"),
		new MarkedDataTypeInfo(MarkedDataDefinitions.MarkedDataTypes.MarkedData, "EvidenceLock", new DeviceType[4]
		{
			DeviceType.Camera,
			DeviceType.Microphone,
			DeviceType.Speaker,
			DeviceType.Metadata
		}, "READ_EVIDENCE_LOCK", "CREATE_EVIDENCE_LOCK", "DELETE_EVIDENCE_LOCK")
	}).ToDictionary((MarkedDataTypeInfo m) => m.MarkedDataType);

	private const string BookmarkSequenceCounterName = "BookmarkId";

	private const string BookmarkReferencePrefixName = "BookmarkIdPrefix";

	private const string GeneralSequenceCounterName = "MarkedDataId";

	private const string GeneralReferencePrefixName = "MarkedDataIdPrefix";

	private static volatile bool checkForOldShemaRecorders = true;

	public static MarkedDataDataContract QueryMarkedDataItem(IGeminiRequestContext requestContext, string markedDataId, int markedDataType)
	{
		MarkedDataDefinitions.MarkedDataTypes key = ValidateMarkedDataType(markedDataType, "markedDataType");
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, markedDataTypeMap[key].FeatureFlag);
		Validation.CheckObjectId(ref markedDataId, "markedDataId", allowNullOrEmpty: false);
		MarkedDataDataContract markedDataDataContract;
		using (IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent())
		{
			markedDataDataContract = markedDataComponent.QueryMarkedDataItemNoSecurity(markedDataId);
		}
		if (markedDataDataContract == null)
		{
			return null;
		}
		if (markedDataDataContract.MarkedDataType != markedDataType)
		{
			throw new ArgumentException(VmoResources.VMO62001, "markedDataType");
		}
		string permissionToCheck = markedDataTypeMap[key].SecurityActionRead;
		if (!markedDataDataContract.DeviceIds.Any((string id) => HasDevicePermission(requestContext, id, permissionToCheck)))
		{
			return null;
		}
		return markedDataDataContract;
	}

	public static MarkedDataDataContract QueryMarkedDataItem(IGeminiRequestContext requestContext, string markedDataId)
	{
		Validation.CheckObjectId(ref markedDataId, "markedDataId", allowNullOrEmpty: false);
		MarkedDataDataContract markedDataDataContract;
		using (IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent())
		{
			markedDataDataContract = markedDataComponent.QueryMarkedDataItemNoSecurity(markedDataId);
		}
		if (markedDataDataContract == null)
		{
			return null;
		}
		MarkedDataDefinitions.MarkedDataTypes markedDataType = (MarkedDataDefinitions.MarkedDataTypes)markedDataDataContract.MarkedDataType;
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, markedDataTypeMap[markedDataType].FeatureFlag);
		string permissionToCheck = markedDataTypeMap[markedDataType].SecurityActionRead;
		if (!markedDataDataContract.DeviceIds.Any((string id) => HasDevicePermission(requestContext, id, permissionToCheck)))
		{
			return null;
		}
		return markedDataDataContract;
	}

	public static MarkedDataDataContract[] QueryBookmarksSearch(IGeminiRequestContext requestContext, DateTime centerTime, TimeSpan timeLimit, int countLimit, DeviceType[] deviceTypes, string[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string optFromBookmarkId)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "Bookmarking");
		MarkedDataDataContract[] result = null;
		if (optDeviceIds == null || optDeviceIds.Length == 0)
		{
			string[] roleIds = IdentityCache.ReadIdentityFromCache(requestContext, requestContext.AuthenticatedUserSid, QueryMembership.Expanded).MemberOfRoles.Select((Guid f) => f.ToString()).ToArray();
			BookmarkSearchParametersNoDeviceFilter searchParameters = new BookmarkSearchParametersNoDeviceFilter
			{
				CenterTime = centerTime,
				TimeLimit = timeLimit,
				CountLimit = countLimit,
				DeviceTypes = deviceTypes,
				Users = optUsers,
				SearchString = optSearchStr,
				SearchHeader = searchHeader,
				SearchDescription = searchDescription,
				SearchReference = searchReference,
				HeaderFilter = headerFilter,
				DescriptionFilter = descriptionFilter,
				SearchUser = searchUser,
				FromBookmarkId = optFromBookmarkId
			};
			using IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent();
			result = markedDataComponent.QueryBookmarksSearchNoDeviceFilter(roleIds, searchParameters);
		}
		else
		{
			List<FaultDevice> source = CheckDevicePermissions(requestContext, optDeviceIds, "READ_BOOKMARKS", throwOnPermissionDenied: false, throwOnUnknowSecurityAction: false);
			optDeviceIds = optDeviceIds.Except(source.Select((FaultDevice x) => x.DeviceId)).ToArray();
			BookmarkSearchParametersDeviceFilter searchParameters2 = new BookmarkSearchParametersDeviceFilter
			{
				CenterTime = centerTime,
				TimeLimit = timeLimit,
				CountLimit = countLimit,
				DeviceIds = optDeviceIds,
				Users = optUsers,
				SearchString = optSearchStr,
				SearchHeader = searchHeader,
				SearchDescription = searchDescription,
				SearchReference = searchReference,
				HeaderFilter = headerFilter,
				DescriptionFilter = descriptionFilter,
				SearchUser = searchUser,
				FromBookmarkId = optFromBookmarkId
			};
			using IMarkedDataComponent markedDataComponent2 = requestContext.CreateMarkedDataComponent();
			result = markedDataComponent2.QueryBookmarksSearchDeviceFilter(searchParameters2);
		}
		return result;
	}

	public static MarkedDataDataContract[] QueryMarkedDataSearch(IGeminiRequestContext requestContext, string[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string[] users, string headerFilter, string descriptionFilter, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFromTime, DateTime retentionExpireToTime, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "EvidenceLock");
		MarkedDataSearchParams searchParams = new MarkedDataSearchParams
		{
			DeviceIds = deviceIds,
			SearchString = searchText,
			SearchHeader = searchHeader,
			SearchDescription = searchDescription,
			SearchReference = searchReference,
			SearchUser = searchUser,
			DescriptionFilter = descriptionFilter,
			HeaderFilter = headerFilter,
			Users = users,
			CreatedFromTime = createdFromTime,
			CreatedToTime = createdToTime,
			MarkedDataFromTime = markedDataFromTime,
			MarkedDataToTime = markedDataToTime,
			TagTimeFromTime = tagTimeFromTime,
			TagTimeToTime = tagTimeToTime,
			RetentionExpireFromTime = retentionExpireFromTime,
			RetentionExpireToTime = retentionExpireToTime,
			PageIndex = pageIndex,
			PageSize = pageSize,
			SortOrder = sortOrder,
			SortAscending = sortAscending,
			MarkedDataType = MarkedDataDefinitions.MarkedDataTypes.MarkedData
		};
		string[] roleIds = IdentityCache.ReadIdentityFromCache(requestContext, requestContext.AuthenticatedUserSid, QueryMembership.Expanded).MemberOfRoles.Select((Guid f) => f.ToString()).ToArray();
		using IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent();
		return markedDataComponent.QueryMarkedDataSearch(roleIds, searchParams);
	}

	public static MarkedDataDataContract CreateMarkedData(IGeminiRequestContext requestContext, string markedDataId, string[] deviceIds, string reference, string header, string description, int markedDataType, DateTime startTime, DateTime tagTime, DateTime endTime, bool useRetention, int retentionType, int retentionUnits, DateTime retentionExpire)
	{
		List<FaultDevice> faultDevices = null;
		MarkedDataDataContract markedData;
		try
		{
			CreateMarkeddata_Validation(requestContext, markedDataId, deviceIds, reference, header, description, markedDataType, startTime, tagTime, endTime, useRetention, retentionType, retentionUnits, retentionExpire, out markedData, out faultDevices);
		}
		catch (MarkedDataDevicesFailedException)
		{
			if (faultDevices != null && faultDevices.Any())
			{
				throw VmoServerServiceException.CreateFromVmoErrorMessage(faultDevices.First().Message);
			}
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
		if (faultDevices.Any())
		{
			throw VmoServerServiceException.CreateFromVmoErrorMessage(faultDevices.First().Message);
		}
		CreateMarkedDataInternal(requestContext, markedData, out var _);
		return markedData;
	}

	public static MarkedDataDataContract CreateMarkedData(IGeminiRequestContext requestContext, string markedDataId, string[] deviceIds, string reference, string header, string description, int markedDataType, DateTime startTime, DateTime tagTime, DateTime endTime, bool useRetention, int retentionType, int retentionUnits, DateTime retentionExpire, out List<FaultDevice> faultDevices, out List<WarningDevice> warningDevices)
	{
		warningDevices = new List<WarningDevice>();
		CreateMarkeddata_Validation(requestContext, markedDataId, deviceIds, reference, header, description, markedDataType, startTime, tagTime, endTime, useRetention, retentionType, retentionUnits, retentionExpire, out var markedData, out faultDevices);
		CreateMarkedDataInternal(requestContext, markedData, out var affectedRecorderIds);
		warningDevices.AddRange(CheckForWarnings(requestContext, markedData, affectedRecorderIds));
		return markedData;
	}

	private static IEnumerable<WarningDevice> CheckForWarnings(IGeminiRequestContext requestContext, MarkedDataDataContract markedData, Guid[] affectedRecorderIds)
	{
		List<WarningDevice> list = new List<WarningDevice>();
		if (markedData.MarkedDataType == 2)
		{
			for (int i = 0; i < affectedRecorderIds.Length; i++)
			{
				Guid guid = affectedRecorderIds[i];
				if (Integration.IsRecorderOnline(guid.ToString()))
				{
					continue;
				}
				if (affectedRecorderIds.Length == 1)
				{
					string[] deviceIds = markedData.DeviceIds;
					foreach (string deviceId in deviceIds)
					{
						list.Add(new WarningDevice(deviceId, VmoResources.VMO60337, 1));
					}
					continue;
				}
				using IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent();
				List<string> source = new List<string>(deviceComponent.QueryDeviceIdsWithMediaOnRecorder(guid.ToString()));
				string[] deviceIds = markedData.DeviceIds;
				foreach (string text in deviceIds)
				{
					if (source.Contains(text, StringComparer.OrdinalIgnoreCase))
					{
						list.Add(new WarningDevice(text, VmoResources.VMO60337, 1));
					}
				}
			}
		}
		return list;
	}

	private static void CreateMarkeddata_Validation(IGeminiRequestContext requestContext, string markedDataId, string[] deviceIds, string reference, string header, string description, int markedDataType, DateTime startTime, DateTime tagTime, DateTime endTime, bool useRetention, int retentionType, int retentionUnits, DateTime retentionExpire, out MarkedDataDataContract markedData, out List<FaultDevice> faultDevices)
	{
		MarkedDataDefinitions.MarkedDataTypes markedDataTypes = ValidateMarkedDataType(markedDataType, "markedDataType");
		MarkedDataTypeInfo markedDataTypeInfo = markedDataTypeMap[markedDataTypes];
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, markedDataTypeInfo.FeatureFlag);
		Validation.CheckObjectId(ref markedDataId, "markedDataId", allowNullOrEmpty: false);
		deviceIds = ValidateStandardParameters(deviceIds, reference, header, ref description, ref startTime, ref tagTime, ref endTime, useRetention, markedDataTypes, ref retentionExpire);
		if (markedDataTypes == MarkedDataDefinitions.MarkedDataTypes.MarkedData)
		{
			ValidateTimeOptionEvidenceLock(requestContext, retentionType, retentionUnits);
		}
		else
		{
			ValidateRetentionValuesBookmarks(retentionType, retentionUnits);
		}
		faultDevices = new List<FaultDevice>();
		deviceIds = MarkedData_FilterDeviceIds(requestContext, deviceIds, markedDataTypeInfo, faultDevices, checkCreatePermissions: true);
		if (deviceIds.Length == 0)
		{
			throw new MarkedDataDevicesFailedException("Could not create marked data (evidence lock or bookmark). Failed for all devices. (see fault device list for details).");
		}
		long noSizeValue = ((markedDataTypes == MarkedDataDefinitions.MarkedDataTypes.MarkedData) ? (-1) : 0);
		markedData = new MarkedDataDataContract
		{
			Id = markedDataId,
			UserName = requestContext.UserDomainAccountName,
			DeviceIds = deviceIds,
			Reference = reference,
			Header = header,
			Description = description,
			MarkedDataType = markedDataType,
			StartTime = startTime,
			TagTime = tagTime,
			EndTime = endTime,
			RetentionType = retentionType,
			RetentionUnits = retentionUnits,
			RetentionExpire = retentionExpire,
			RetentionSize = noSizeValue,
			StickerSizes = deviceIds.Select((string id) => new StickerSize2(new Guid(id), noSizeValue))
		};
	}

	private static void ValidateRetentionValuesBookmarks(int retentionType, int retentionUnits)
	{
		if (retentionType != 0)
		{
			throw new ArgumentException(VmoResources.VMO62001, "retentionType");
		}
		if (retentionUnits != 0)
		{
			throw new ArgumentException(VmoResources.VMO62001, "retentionUnits");
		}
	}

	private static string[] MarkedData_FilterDeviceIds(IGeminiRequestContext requestContext, IEnumerable<string> deviceIds, MarkedDataTypeInfo markedDataTypeInfo, List<FaultDevice> faultDevices, bool checkCreatePermissions)
	{
		List<Device> list;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			list = deviceComponent.QueryDevicesByIds(deviceIds).ToList();
		}
		FilterNotFoundDevices(list, deviceIds, faultDevices);
		FilterReadPermissionDevices(list, requestContext, faultDevices);
		FilterNonSupportedDevices(list, markedDataTypeInfo, faultDevices);
		FilterRemotePlaybackDevices(list, markedDataTypeInfo, faultDevices);
		if (checkCreatePermissions)
		{
			FilterCreatePermissionDevices(list, requestContext, markedDataTypeInfo, faultDevices);
		}
		FilterNonSupportedRecorderDevices(list, requestContext, markedDataTypeInfo, faultDevices);
		return list.Select((Device d) => d.Id).ToArray();
	}

	private static void FilterNotFoundDevices(List<Device> devices, IEnumerable<string> deviceIds, List<FaultDevice> faultDevices)
	{
		List<string> notFoundDeviceIds = deviceIds.Where((string id) => !devices.Any((Device d) => string.Equals(d.Id, id, StringComparison.OrdinalIgnoreCase))).ToList();
		if (!notFoundDeviceIds.Any())
		{
			return;
		}
		faultDevices.AddRange(notFoundDeviceIds.Select((string id) => new FaultDevice(id, VmoResources.VMO60071, 8, FaultSource.ManagementServer)));
		devices.RemoveAll((Device d) => notFoundDeviceIds.Any((string id) => string.Equals(id, d.Id, StringComparison.OrdinalIgnoreCase)));
	}

	private static void FilterReadPermissionDevices(List<Device> devices, IGeminiRequestContext requestContext, List<FaultDevice> faultDevices)
	{
		List<Device> noReadPermissionDevices = devices.Where((Device d) => !HasDevicePermission(requestContext, d.Id, d.DeviceType, "GENERIC_READ")).ToList();
		if (!noReadPermissionDevices.Any())
		{
			return;
		}
		faultDevices.AddRange(noReadPermissionDevices.Select((Device d) => new FaultDevice(Guid.Empty.ToString(), VmoResources.VMO60071, 8, FaultSource.ManagementServer)));
		devices.RemoveAll((Device d) => noReadPermissionDevices.Any((Device n) => string.Equals(n.Id, d.Id, StringComparison.OrdinalIgnoreCase)));
	}

	private static void FilterNonSupportedDevices(List<Device> devices, MarkedDataTypeInfo markedDataTypeInfo, List<FaultDevice> faultDevices)
	{
		List<Device> notSupportedDevices = devices.Where((Device d) => !markedDataTypeInfo.SupportedDeviceTypes.Contains(d.DeviceType)).ToList();
		if (!notSupportedDevices.Any())
		{
			return;
		}
		if (markedDataTypeInfo.MarkedDataType == MarkedDataDefinitions.MarkedDataTypes.MarkedData)
		{
			faultDevices.AddRange(notSupportedDevices.Select((Device d) => new FaultDevice(d.Id, VmoResources.VMO60331, 10, FaultSource.ManagementServer)));
		}
		else
		{
			faultDevices.AddRange(notSupportedDevices.Select((Device d) => new FaultDevice(d.Id, VmoResources.VMO60332, 10, FaultSource.ManagementServer)));
		}
		devices.RemoveAll((Device d) => notSupportedDevices.Any((Device n) => string.Equals(n.Id, d.Id, StringComparison.OrdinalIgnoreCase)));
	}

	private static void FilterRemotePlaybackDevices(List<Device> devices, MarkedDataTypeInfo markedDataTypeInfo, List<FaultDevice> faultDevices)
	{
		if (markedDataTypeInfo.MarkedDataType != MarkedDataDefinitions.MarkedDataTypes.MarkedData)
		{
			return;
		}
		List<Device> edgePlaybackDevices = devices.Where((Device d) => d.EdgeStoragePlayback).ToList();
		if (!edgePlaybackDevices.Any())
		{
			return;
		}
		faultDevices.AddRange(edgePlaybackDevices.Select((Device d) => new FaultDevice(d.Id, VmoResources.VMO60333, 7, FaultSource.ManagementServer)));
		devices.RemoveAll((Device d) => edgePlaybackDevices.Any((Device n) => string.Equals(n.Id, d.Id, StringComparison.OrdinalIgnoreCase)));
	}

	private static void FilterCreatePermissionDevices(List<Device> devices, IGeminiRequestContext requestContext, MarkedDataTypeInfo markedDataTypeInfo, List<FaultDevice> faultDevices)
	{
		List<Device> deniedDevices = devices.Where((Device d) => !HasDevicePermission(requestContext, d.Id, markedDataTypeInfo.SecurityActionCreate)).ToList();
		if (!deniedDevices.Any())
		{
			return;
		}
		faultDevices.AddRange(deniedDevices.Select((Device d) => new FaultDevice(d.Id, VmoResources.VMO61008, 6, FaultSource.ManagementServer)));
		devices.RemoveAll((Device d) => deniedDevices.Any((Device n) => string.Equals(n.Id, d.Id, StringComparison.OrdinalIgnoreCase)));
	}

	private static void FilterNonSupportedRecorderDevices(List<Device> devices, IGeminiRequestContext requestContext, MarkedDataTypeInfo markedDataTypeInfo, List<FaultDevice> faultDevices)
	{
		if (!checkForOldShemaRecorders || markedDataTypeInfo.MarkedDataType != MarkedDataDefinitions.MarkedDataTypes.MarkedData)
		{
			return;
		}
		using IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent();
		string[] array = markedDataComponent.QueryOldSchemeRecordingServers();
		if (array.Any())
		{
			using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
			{
				HashSet<string> deviceIdsOnOldRecordersSql = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
				string[] array2 = array;
				foreach (string recorderId in array2)
				{
					deviceIdsOnOldRecordersSql.UnionWith(deviceComponent.QueryDeviceIdsWithMediaOnRecorder(recorderId));
				}
				List<Device> devicesOnOldRecorders = devices.Where((Device d) => deviceIdsOnOldRecordersSql.Contains(d.Id)).ToList();
				if (devicesOnOldRecorders.Any())
				{
					faultDevices.AddRange(devicesOnOldRecorders.Select((Device d) => new FaultDevice(d.Id, VmoResources.VMO60336, 4, FaultSource.ManagementServer)));
					devices.RemoveAll((Device d) => devicesOnOldRecorders.Any((Device door) => string.Equals(door.Id, d.Id, StringComparison.OrdinalIgnoreCase)));
				}
				return;
			}
		}
		checkForOldShemaRecorders = false;
	}

	private static void CreateMarkedDataInternal(IGeminiRequestContext requestContext, MarkedDataDataContract markedData, out Guid[] affectedRecorderIds)
	{
		MarkedDataDefinitions.MarkedDataTypes markedDataType = (MarkedDataDefinitions.MarkedDataTypes)markedData.MarkedDataType;
		Guid sessionId = Guid.NewGuid();
		using (IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent())
		{
			affectedRecorderIds = markedDataComponent.CreateMarkedData(new Guid(markedData.Id), markedData.DeviceIds, markedData.Reference, markedData.UserName, markedData.Header, markedData.Description, (MarkedDataDefinitions.MarkedDataTypes)markedData.MarkedDataType, markedData.StartTime, markedData.TagTime, markedData.EndTime, (OptionType)markedData.RetentionType, markedData.RetentionUnits, markedData.RetentionExpire, markedData.RetentionSize, sessionId);
		}
		if (markedDataType == MarkedDataDefinitions.MarkedDataTypes.MarkedData)
		{
			Guid[] array = affectedRecorderIds;
			for (int i = 0; i < array.Length; i++)
			{
				Integration.TryNotifyRecordingServerAsync(array[i]);
			}
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		if (markedData.MarkedDataType == 2)
		{
			string eventId = BuiltInEventTypes.EventIDs.RecordingsEvidenceLocked.ToString();
			DateTime utcNow = DateTime.UtcNow;
			string[] deviceIds = markedData.DeviceIds;
			foreach (string deviceId in deviceIds)
			{
				UserDefinedEvent.TriggerBuiltInEventForDevice(requestContext, eventId, deviceId, utcNow);
			}
		}
	}

	public static MarkedDataDataContract UpdateMarkedData(IGeminiRequestContext requestContext, string markedDataId, int markedDataType, string[] deviceIds, string reference, string header, string description, DateTime startTime, DateTime tagTime, DateTime endTime, bool useRetention, int retentionType, int retentionUnits, DateTime retentionExpire)
	{
		List<FaultDevice> faultDevices = null;
		MarkedDataDataContract markedDataBefore;
		MarkedDataDataContract markedDataAfter;
		try
		{
			UpdateMarkedData_Validation(requestContext, markedDataId, markedDataType, deviceIds, reference, header, description, startTime, tagTime, endTime, useRetention, retentionType, retentionUnits, retentionExpire, out markedDataBefore, out markedDataAfter, out faultDevices);
		}
		catch (MarkedDataDevicesFailedException)
		{
			if (faultDevices != null && faultDevices.Any())
			{
				throw VmoServerServiceException.CreateFromVmoErrorMessage(faultDevices.First().Message);
			}
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
		if (faultDevices.Any())
		{
			throw VmoServerServiceException.CreateFromVmoErrorMessage(faultDevices.First().Message);
		}
		UpdateMarkedDataInternal(requestContext, markedDataBefore, markedDataAfter, out var _);
		return markedDataAfter;
	}

	public static MarkedDataDataContract UpdateMarkedData(IGeminiRequestContext requestContext, string markedDataId, int markedDataType, string[] deviceIds, string reference, string header, string description, DateTime startTime, DateTime tagTime, DateTime endTime, bool useRetention, int retentionType, int retentionUnits, DateTime retentionExpire, out List<FaultDevice> faultDevices, out List<WarningDevice> warningDevices)
	{
		warningDevices = new List<WarningDevice>();
		UpdateMarkedData_Validation(requestContext, markedDataId, markedDataType, deviceIds, reference, header, description, startTime, tagTime, endTime, useRetention, retentionType, retentionUnits, retentionExpire, out var markedDataBefore, out var markedDataAfter, out faultDevices);
		UpdateMarkedDataInternal(requestContext, markedDataBefore, markedDataAfter, out var affectedRecorderIds);
		warningDevices.AddRange(CheckForWarnings(requestContext, markedDataAfter, affectedRecorderIds));
		return markedDataAfter;
	}

	private static void UpdateMarkedData_Validation(IGeminiRequestContext requestContext, string markedDataId, int markedDataType, string[] deviceIds, string reference, string header, string description, DateTime startTime, DateTime tagTime, DateTime endTime, bool useRetention, int retentionType, int retentionUnits, DateTime retentionExpire, out MarkedDataDataContract markedDataBefore, out MarkedDataDataContract markedDataAfter, out List<FaultDevice> faultDevices)
	{
		MarkedDataDefinitions.MarkedDataTypes markedDataTypes = ValidateMarkedDataType(markedDataType, "markedDataType");
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, markedDataTypeMap[markedDataTypes].FeatureFlag);
		Validation.CheckObjectId(ref markedDataId, "markedDataId", allowNullOrEmpty: false);
		markedDataBefore = QueryMarkedDataItem(requestContext, markedDataId, markedDataType);
		if (markedDataBefore == null)
		{
			throw new MarkedDataNotFoundException();
		}
		if (markedDataTypes == MarkedDataDefinitions.MarkedDataTypes.Bookmark)
		{
			deviceIds = markedDataBefore.DeviceIds;
		}
		deviceIds = ValidateStandardParameters(deviceIds, reference, header, ref description, ref startTime, ref tagTime, ref endTime, useRetention, markedDataTypes, ref retentionExpire);
		if (markedDataTypes == MarkedDataDefinitions.MarkedDataTypes.MarkedData)
		{
			ValidateTimeOptionEvidenceLock(requestContext, retentionType, retentionUnits);
		}
		else
		{
			ValidateRetentionValuesBookmarks(retentionType, retentionUnits);
		}
		markedDataAfter = new MarkedDataDataContract
		{
			Id = markedDataBefore.Id,
			MarkedDataType = markedDataBefore.MarkedDataType,
			DeviceIds = deviceIds,
			Reference = reference,
			Header = header,
			Description = description,
			StartTime = startTime,
			TagTime = tagTime,
			EndTime = endTime,
			RetentionType = retentionType,
			RetentionUnits = retentionUnits,
			RetentionExpire = retentionExpire,
			UserName = markedDataBefore.UserName,
			RetentionSize = markedDataBefore.RetentionSize,
			StickerSizes = markedDataBefore.StickerSizes
		};
		faultDevices = new List<FaultDevice>();
		UpdateMarkedData_CheckSecurity(requestContext, markedDataTypes, markedDataBefore, markedDataAfter, faultDevices);
		if (markedDataBefore.StartTime != markedDataAfter.StartTime || markedDataBefore.EndTime != markedDataAfter.EndTime || !SequencesAreEquivalent(markedDataBefore.DeviceIds, markedDataAfter.DeviceIds, StringComparer.OrdinalIgnoreCase) || markedDataBefore.RetentionExpire != markedDataAfter.RetentionExpire)
		{
			long noSizeValue = ((markedDataTypes == MarkedDataDefinitions.MarkedDataTypes.MarkedData) ? (-1) : 0);
			markedDataAfter.RetentionSize = noSizeValue;
			markedDataAfter.StickerSizes = deviceIds.Select((string id) => new StickerSize2(new Guid(id), noSizeValue));
		}
	}

	private static void UpdateMarkedData_CheckSecurity(IGeminiRequestContext requestContext, MarkedDataDefinitions.MarkedDataTypes markedDataType, MarkedDataDataContract markedDataBefore, MarkedDataDataContract markedDataAfter, List<FaultDevice> faultDevices)
	{
		switch (markedDataType)
		{
		case MarkedDataDefinitions.MarkedDataTypes.MarkedData:
		{
			IEnumerable<string> enumerable = markedDataAfter.DeviceIds.Except(markedDataBefore.DeviceIds, StringComparer.OrdinalIgnoreCase);
			IEnumerable<string> enumerable2 = markedDataBefore.DeviceIds.Except(markedDataAfter.DeviceIds, StringComparer.OrdinalIgnoreCase);
			HashSet<string> deviceIdsToCheckForCreatePermission = new HashSet<string>(enumerable, StringComparer.OrdinalIgnoreCase);
			HashSet<string> deviceIdsToCheckForDeletePermission = new HashSet<string>(enumerable2, StringComparer.OrdinalIgnoreCase);
			MarkedData_FilterDeviceIds(requestContext, enumerable, markedDataTypeMap[markedDataType], faultDevices, checkCreatePermissions: true);
			MarkedData_FilterDeviceIds(requestContext, enumerable2, markedDataTypeMap[markedDataType], faultDevices, checkCreatePermissions: false);
			if (faultDevices.Any())
			{
				throw new MarkedDataDevicesFailedException("Could not update evidence lock. Failed for all devices. (see fault device list for details).");
			}
			if (!markedDataBefore.Header.Equals(markedDataAfter.Header) || markedDataBefore.StartTime > markedDataAfter.StartTime || markedDataBefore.EndTime < markedDataAfter.EndTime || markedDataBefore.RetentionExpire < markedDataAfter.RetentionExpire)
			{
				markedDataAfter.DeviceIds.ToList().ForEach(delegate(string id)
				{
					deviceIdsToCheckForCreatePermission.Add(id);
				});
			}
			if (markedDataBefore.StartTime < markedDataAfter.StartTime || markedDataBefore.EndTime > markedDataAfter.EndTime || markedDataBefore.RetentionExpire > markedDataAfter.RetentionExpire)
			{
				markedDataAfter.DeviceIds.ToList().ForEach(delegate(string id)
				{
					deviceIdsToCheckForDeletePermission.Add(id);
				});
			}
			if (deviceIdsToCheckForCreatePermission.Any((string id) => !HasDevicePermission(requestContext, id, "CREATE_EVIDENCE_LOCK")) || deviceIdsToCheckForDeletePermission.Any((string id) => !HasDevicePermission(requestContext, id, "DELETE_EVIDENCE_LOCK")))
			{
				throw new UnauthorizedAccessException(VmoResources.VMO61008);
			}
			break;
		}
		case MarkedDataDefinitions.MarkedDataTypes.Bookmark:
			if (markedDataAfter.DeviceIds.Any((string id) => !HasDevicePermission(requestContext, id, "EDIT_BOOKMARKS")))
			{
				throw new UnauthorizedAccessException(VmoResources.VMO61008);
			}
			break;
		}
	}

	private static bool SequencesAreEquivalent(IEnumerable<string> sequence1, IEnumerable<string> sequence2, IEqualityComparer<string> comparer)
	{
		if (sequence1 == sequence2)
		{
			return true;
		}
		if (sequence1 == null)
		{
			return false;
		}
		if (sequence2 == null)
		{
			return false;
		}
		if (sequence1.Count() != sequence2.Count())
		{
			return false;
		}
		foreach (string item in sequence1)
		{
			if (!sequence2.Contains(item, comparer))
			{
				return false;
			}
		}
		return true;
	}

	private static MarkedDataDataContract UpdateMarkedDataInternal(IGeminiRequestContext requestContext, MarkedDataDataContract markedDataBefore, MarkedDataDataContract markedDataAfter, out Guid[] affectedRecorderIds)
	{
		MarkedDataDefinitions.MarkedDataTypes markedDataType = (MarkedDataDefinitions.MarkedDataTypes)markedDataAfter.MarkedDataType;
		Guid sessionId = Guid.NewGuid();
		using (IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent())
		{
			affectedRecorderIds = markedDataComponent.UpdateMarkedData(new Guid(markedDataAfter.Id), markedDataAfter.DeviceIds, markedDataAfter.Reference, markedDataAfter.Header, markedDataAfter.Description, markedDataAfter.StartTime, markedDataAfter.TagTime, markedDataAfter.EndTime, (OptionType)markedDataAfter.RetentionType, markedDataAfter.RetentionUnits, markedDataAfter.RetentionExpire, markedDataAfter.RetentionSize, markedDataAfter.StickerSizes, sessionId);
		}
		if (markedDataType == MarkedDataDefinitions.MarkedDataTypes.MarkedData)
		{
			Guid[] array = affectedRecorderIds;
			for (int i = 0; i < array.Length; i++)
			{
				Integration.TryNotifyRecordingServerAsync(array[i]);
			}
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		if (markedDataAfter.MarkedDataType == 2)
		{
			string eventId = BuiltInEventTypes.EventIDs.EvidenceLockChanged.ToString();
			DateTime utcNow = DateTime.UtcNow;
			string[] deviceIds = markedDataBefore.DeviceIds;
			foreach (string deviceId in deviceIds)
			{
				UserDefinedEvent.TriggerBuiltInEventForDevice(requestContext, eventId, deviceId, utcNow);
			}
		}
		return markedDataAfter;
	}

	public static void DeleteMarkedData(IGeminiRequestContext requestContext, string markedDataId, MarkedDataDefinitions.MarkedDataTypes markedDataType)
	{
		ValidateMarkedDataType((int)markedDataType, "markedDataType");
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, markedDataTypeMap[markedDataType].FeatureFlag);
		Validation.CheckObjectId(ref markedDataId, "markedDataId", allowNullOrEmpty: false);
		MarkedDataDataContract markedDataDataContract = QueryMarkedDataItem(requestContext, markedDataId, (int)markedDataType);
		if (markedDataDataContract == null)
		{
			throw new MarkedDataNotFoundException();
		}
		if (markedDataDataContract.MarkedDataType != (int)markedDataType)
		{
			throw new ArgumentException(VmoResources.VMO62001, "markedDataType");
		}
		string securityActionDelete = markedDataTypeMap[markedDataType].SecurityActionDelete;
		CheckDevicePermissions(requestContext, markedDataDataContract.DeviceIds, securityActionDelete, throwOnPermissionDenied: true, throwOnUnknowSecurityAction: true);
		Guid[] array = Array.Empty<Guid>();
		Guid sessionId = Guid.NewGuid();
		using (IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent())
		{
			array = markedDataComponent.DeleteMarkedData(new Guid(markedDataId), sessionId);
		}
		if (markedDataType == MarkedDataDefinitions.MarkedDataTypes.MarkedData)
		{
			Guid[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Integration.TryNotifyRecordingServerAsync(array2[i]);
			}
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		if (markedDataDataContract.MarkedDataType == 2)
		{
			string eventId = BuiltInEventTypes.EventIDs.RecordingsEvidenceUnlocked.ToString();
			DateTime utcNow = DateTime.UtcNow;
			string[] deviceIds = markedDataDataContract.DeviceIds;
			foreach (string deviceId in deviceIds)
			{
				UserDefinedEvent.TriggerBuiltInEventForDevice(requestContext, eventId, deviceId, utcNow);
			}
		}
	}

	public static void DeleteBookmarksByDeviceIdBeforeTime(IGeminiRequestContext requestContext, MediaStatusEntry mediaStatus)
	{
		using IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent();
		markedDataComponent.DeleteBookmarksByDeviceIdBeforeTime(mediaStatus);
	}

	public static int NextSequenceCounterValue(IGeminiRequestContext requestContext, string counterName)
	{
		using IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent();
		return markedDataComponent.NextSequenceCounterValue(counterName);
	}

	public static MarkedDataReference QueryMarkedDataReference(IGeminiRequestContext requestContext, string[] deviceIds, bool realTime, MarkedDataDefinitions.MarkedDataTypes markedDataType)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, markedDataTypeMap[markedDataType].FeatureFlag);
		DateTime utcNow = DateTime.UtcNow;
		string counterName = "MarkedDataId";
		string setting = "MarkedDataIdPrefix";
		string eventId = BuiltInEventTypes.EventIDs.MarkedDataReferenceRequested.ToString();
		if (markedDataType.HasFlag(MarkedDataDefinitions.MarkedDataTypes.Bookmark))
		{
			counterName = "BookmarkId";
			setting = "BookmarkIdPrefix";
			eventId = BuiltInEventTypes.EventIDs.BookmarkReferenceRequested.ToString();
		}
		if (realTime)
		{
			string securityActionId = "GENERIC_READ";
			CheckDevicePermissions(requestContext, deviceIds, securityActionId, throwOnPermissionDenied: true, throwOnUnknowSecurityAction: true);
			foreach (string deviceId in deviceIds)
			{
				UserDefinedEvent.TriggerBuiltInEventForDevice(requestContext, eventId, deviceId, utcNow);
			}
		}
		int num = NextSequenceCounterValue(requestContext, counterName);
		string systemSetting = ServerInformation.GetSystemSetting(requestContext, setting);
		return new MarkedDataReference
		{
			Reference = string.Format(CultureInfo.InvariantCulture, "{0}{1:D6}", systemSetting, num),
			Time = utcNow,
			Devices = deviceIds
		};
	}

	public static void CleanExpiredMarkedData(IGeminiRequestContext requestContext)
	{
		using IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent();
		MarkedDataDataContract[] array = markedDataComponent.QueryExpiredMarkedData();
		foreach (MarkedDataDataContract markedDataDataContract in array)
		{
			try
			{
				DeleteMarkedData(requestContext, markedDataDataContract.Id, MarkedDataDefinitions.MarkedDataTypes.MarkedData);
			}
			catch (MarkedDataNotFoundException)
			{
			}
		}
	}

	public static bool RecordingServerContainMarkedData(IGeminiRequestContext requestContext, string recordingServerId)
	{
		using IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent();
		return markedDataComponent.QueryRecordingServerMarkedData(recordingServerId).Length != 0;
	}

	public static MarkedDataDataContract[] QueryRecordingServerMarkedData(IGeminiRequestContext requestContext, string recordingServerId)
	{
		using IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent();
		return markedDataComponent.QueryRecordingServerMarkedData(recordingServerId);
	}

	public static bool RecorderHasLockedDeviceDataDistributions(IGeminiRequestContext requestContext, string recorderId)
	{
		using IMarkedDataComponent markedDataComponent = requestContext.CreateMarkedDataComponent();
		return markedDataComponent.RecorderHasLockedDeviceDataDistributions(recorderId);
	}

	private static List<FaultDevice> CheckDevicePermissions(IGeminiRequestContext requestContext, string[] deviceIds, string securityActionId, bool throwOnPermissionDenied, bool throwOnUnknowSecurityAction)
	{
		List<FaultDevice> list = new List<FaultDevice>();
		foreach (string deviceId in deviceIds)
		{
			if (!CheckDevicePermission(requestContext, deviceId, securityActionId, throwOnPermissionDenied, throwOnUnknowSecurityAction, out var faultDevice))
			{
				list.Add(faultDevice);
			}
		}
		return list;
	}

	private static bool HasDevicePermission(IGeminiRequestContext requestContext, string deviceId, string securityActionId)
	{
		bool throwOnPermissionDenied = false;
		bool throwOnUnknowSecurityAction = true;
		FaultDevice faultDevice;
		return CheckDevicePermission(requestContext, deviceId, securityActionId, throwOnPermissionDenied, throwOnUnknowSecurityAction, out faultDevice);
	}

	private static bool CheckDevicePermission(IGeminiRequestContext requestContext, string deviceId, string securityActionId, bool throwOnPermissionDenied, bool throwOnUnknowSecurityAction, out FaultDevice faultDevice)
	{
		faultDevice = null;
		Guid securityNamespaceId;
		try
		{
			securityNamespaceId = DeviceTypeLookup.Instance.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
		}
		catch (UnknownDeviceException)
		{
			if (throwOnPermissionDenied)
			{
				throw new DeviceNotFoundException();
			}
			faultDevice = new FaultDevice(deviceId, VmoResources.VMO60071, 8, FaultSource.ManagementServer);
			return false;
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		return CheckDevicePermission(requestContext, deviceId, securityNamespaceId, securityActionId, throwOnPermissionDenied, throwOnUnknowSecurityAction, out faultDevice);
	}

	private static bool HasDevicePermission(IGeminiRequestContext requestContext, string deviceId, DeviceType deviceType, string securityActionId)
	{
		Guid securityNamespace = DeviceTypeInformation.DeviceTypeDictionary[deviceType].SecurityNamespace;
		bool throwOnPermissionDenied = false;
		bool throwOnUnknowSecurityAction = true;
		FaultDevice faultDevice;
		return CheckDevicePermission(requestContext, deviceId, securityNamespace, securityActionId, throwOnPermissionDenied, throwOnUnknowSecurityAction, out faultDevice);
	}

	private static bool CheckDevicePermission(IGeminiRequestContext requestContext, string deviceId, Guid securityNamespaceId, string securityActionId, bool throwOnPermissionDenied, bool throwOnUnknowSecurityAction, out FaultDevice faultDevice)
	{
		faultDevice = null;
		try
		{
			if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, securityNamespaceId, securityActionId, deviceId))
			{
				if (throwOnPermissionDenied)
				{
					throw new UnauthorizedAccessException(VmoResources.VMO61008);
				}
				faultDevice = new FaultDevice(deviceId, VmoResources.VMO61008, 6, FaultSource.ManagementServer);
				return false;
			}
		}
		catch (SecurityActionDoesNotExistException)
		{
			if (throwOnUnknowSecurityAction)
			{
				throw;
			}
			faultDevice = new FaultDevice(deviceId, VmoResources.VMO60013, 6, FaultSource.ManagementServer);
			return false;
		}
		return true;
	}

	private static string[] ValidateStandardParameters(string[] deviceIds, string reference, string header, ref string description, ref DateTime startTime, ref DateTime tagTime, ref DateTime endTime, bool useRetention, MarkedDataDefinitions.MarkedDataTypes markedDataType, ref DateTime retentionExpire)
	{
		Validation.CheckObjectIdArray(deviceIds, "deviceIds", allowNullOrEmptyIds: false);
		Validation.CheckName(reference, "reference");
		Validation.CheckName(header, "header");
		Validation.CheckDescription(ref description, "description");
		Validation.CheckDateTimeHigherThan1900(ref startTime, "startTime", allowDateMinValue: true);
		Validation.CheckDateTimeHigherThan1900(ref tagTime, "tagTime", allowDateMinValue: true);
		Validation.CheckDateTimeHigherThan1900(ref endTime, "endTime", allowDateMinValue: true);
		Validation.CheckDateTimeHigherThan1900(ref retentionExpire, "retentionExpire", allowDateMinValue: true);
		deviceIds = RemoveDuplicates(deviceIds);
		if ((markedDataType == MarkedDataDefinitions.MarkedDataTypes.MarkedData && !useRetention) || (markedDataType == MarkedDataDefinitions.MarkedDataTypes.Bookmark && useRetention))
		{
			throw new ArgumentException(VmoResources.VMO62001, "useRetention");
		}
		if (markedDataType == MarkedDataDefinitions.MarkedDataTypes.Bookmark && deviceIds.Length != 1)
		{
			throw new ArgumentException(VmoResources.VMO62001, "deviceIds");
		}
		if (startTime > endTime)
		{
			throw new ArgumentException(VmoResources.VMO62001, "startTime");
		}
		if (tagTime < startTime || tagTime > endTime)
		{
			throw new ArgumentException(VmoResources.VMO62001, "tagTime");
		}
		if (useRetention && DateTime.UtcNow < endTime)
		{
			throw new ArgumentException(VmoResources.VMO62027);
		}
		if (useRetention && DateTime.UtcNow > retentionExpire)
		{
			throw new ArgumentException(VmoResources.VMO62001, "retentionExpire");
		}
		return deviceIds;
	}

	private static MarkedDataDefinitions.MarkedDataTypes ValidateMarkedDataType(int value, string parameterName)
	{
		return value switch
		{
			8 => MarkedDataDefinitions.MarkedDataTypes.Bookmark, 
			2 => MarkedDataDefinitions.MarkedDataTypes.MarkedData, 
			_ => throw new ArgumentException(VmoResources.VMO62001, parameterName), 
		};
	}

	private static void ValidateTimeOptionEvidenceLock(IGeminiRequestContext requestContext, int retentionType, int retentionUnits)
	{
		RetentionOptionsXmlFormatter retentionOptionsXmlFormatter = RetentionOptionsXmlManager.Deserialize(ServerInformation.GetSystemSetting(requestContext, SettingList.RetentionOptions));
		Role[] array = Role.FindRolesByMember(requestContext, requestContext.AuthenticatedUserSid);
		foreach (Role role in array)
		{
			List<RetentionTime> retentionTimes;
			try
			{
				RetentionOptionRoleRelation relation = RetentionOptionRoleRelation.QueryRetentionOptionRoleRelationByRoleId(requestContext, role.Id);
				retentionTimes = retentionOptionsXmlFormatter.RetentionOptions.Single((RetentionTimeOption retentionTimeOption) => string.Equals(retentionTimeOption.Id.ToString(), relation.IdRetention, StringComparison.OrdinalIgnoreCase)).RetentionTimes;
			}
			catch (RetentionOptionRoleRelationNotExistException)
			{
				retentionTimes = retentionOptionsXmlFormatter.RetentionOptions.Single((RetentionTimeOption retentionTimeOption) => retentionTimeOption.Default).RetentionTimes;
			}
			foreach (RetentionTime item in retentionTimes)
			{
				if (item.Option == OptionType.UserDefined || (item.Option == OptionType.Indefinite && retentionType == 6) || (item.Option == (OptionType)retentionType && item.Unit == retentionUnits))
				{
					return;
				}
			}
		}
		throw new ArgumentException(VmoResources.VMO62001, "retentionType");
	}

	private static string[] RemoveDuplicates(string[] deviceIds)
	{
		return new HashSet<string>(deviceIds, StringComparer.OrdinalIgnoreCase).ToArray();
	}
}
