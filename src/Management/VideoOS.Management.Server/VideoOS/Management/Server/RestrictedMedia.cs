using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.BusinessObjects;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public static class RestrictedMedia
{
	public enum RestrictedMediaResponseCode
	{
		UnknownError,
		None,
		Warning,
		Unauthorized,
		DeviceNotFound,
		RestrictedMediasNotSupported,
		AlreadyInRestrictedLiveMode,
		NotInRestrictedLiveMode
	}

	private static RestrictedMediaDataContract QueryPlaybackWithoutAnyChecks(IGeminiRequestContext requestContext, string id)
	{
		using IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent();
		return restrictedMediaComponent.QueryPlayback(new Guid(id));
	}

	public static RestrictedMediaDataContract[] QueryAllPlayback(IGeminiRequestContext requestContext)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RestrictedMedia");
		RestrictedMediaDataContract[] source = null;
		using (IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent())
		{
			source = restrictedMediaComponent.QueryAllPlayback();
		}
		source = source.Where((RestrictedMediaDataContract r) => r.DeviceIds.Any((string id) => HasDevicePermission(requestContext, id, "READ_RESTRICTIONS"))).ToArray();
		RestrictedMediaDataContract[] array = source;
		foreach (RestrictedMediaDataContract restrictedMediaDataContract in array)
		{
			restrictedMediaDataContract.DeviceIds = GetDeviceIdsWithReadPermissions(requestContext, restrictedMediaDataContract);
		}
		return source;
	}

	private static string[] GetDeviceIdsWithReadPermissions(IGeminiRequestContext requestContext, RestrictedMediaDataContract playbackItem)
	{
		return playbackItem.DeviceIds.Select((string id) => (!HasDevicePermission(requestContext, id, "READ_RESTRICTIONS")) ? Guid.Empty.ToString() : id).ToArray();
	}

	public static RestrictedMediaLiveDataContract[] QueryAllLive(IGeminiRequestContext requestContext)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RestrictedMedia");
		RestrictedMediaLiveDataContract[] source = null;
		using (IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent())
		{
			source = restrictedMediaComponent.QueryAllLive();
		}
		return source.Where((RestrictedMediaLiveDataContract r) => HasDevicePermission(requestContext, r.DeviceId, "READ_RESTRICTIONS")).ToArray();
	}

	public static RestrictedMediaDataContract CreatePlayback(IGeminiRequestContext requestContext, string id, string[] deviceIds, string header, string description, DateTime startTime, DateTime endTime, List<FaultDevice> faultDevices)
	{
		CreatePlayback_Validation(requestContext, id, deviceIds, header, description, startTime, endTime, out var restrictedMedia, faultDevices);
		Guid sessionId = Guid.NewGuid();
		Guid[] array;
		using (IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent())
		{
			array = restrictedMediaComponent.CreatePlayback(new Guid(restrictedMedia.Id), restrictedMedia.DeviceIds, restrictedMedia.Header, restrictedMedia.Description, restrictedMedia.UserName, restrictedMedia.StartTime, restrictedMedia.EndTime, restrictedMedia.LastModified, sessionId);
		}
		Guid[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Integration.TryNotifyRecordingServerAsync(array2[i]);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return restrictedMedia;
	}

	public static void CreatePlayback_Validation(IGeminiRequestContext requestContext, string id, string[] deviceIds, string header, string description, DateTime startTime, DateTime endTime, out RestrictedMediaDataContract restrictedMedia, List<FaultDevice> faultDevices)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RestrictedMedia");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		deviceIds = ValidateStandardParameters(deviceIds, header, description, ref startTime, ref endTime);
		deviceIds = CreateOrExtendRestrictedMedia_FilterDeviceIds(requestContext, deviceIds, faultDevices);
		if (deviceIds.Length == 0)
		{
			throw new RestrictedMediaDevicesFailedException("Could not create media restriction. Failed for all devices. (see fault device list for details).");
		}
		restrictedMedia = new RestrictedMediaDataContract
		{
			Id = id,
			DeviceIds = deviceIds,
			Header = header,
			Description = description,
			UserName = requestContext.UserDomainAccountName,
			StartTime = startTime,
			EndTime = endTime,
			LastModified = DateTime.UtcNow
		};
	}

	private static string[] CreateOrExtendRestrictedMedia_FilterDeviceIds(IGeminiRequestContext requestContext, IEnumerable<string> deviceIds, List<FaultDevice> faultDevices)
	{
		List<Device> list = FilterDeviceIds(requestContext, deviceIds, faultDevices);
		FilterCreatePermissionDevices(list, requestContext, faultDevices);
		return list.Select((Device d) => d.Id).ToArray();
	}

	public static RestrictedMediaLiveDataContract[] EnterLive(IGeminiRequestContext requestContext, string[] deviceIds, DateTime startTime, List<FaultDevice> faultDevices)
	{
		Guid sessionId = Guid.NewGuid();
		HashSet<Guid> hashSet = new HashSet<Guid>();
		List<RestrictedMediaLiveDataContract> liveRestrictions;
		List<RestrictedMediaLiveDataContract> existingLiveRestrictions;
		string[] filteredDeviceIds = EnterLive_Validation(requestContext, deviceIds, ref startTime, out liveRestrictions, out existingLiveRestrictions, faultDevices);
		liveRestrictions.AddRange(existingLiveRestrictions.Where((RestrictedMediaLiveDataContract lr) => startTime >= lr.StartTime));
		existingLiveRestrictions = existingLiveRestrictions.Where((RestrictedMediaLiveDataContract lr) => startTime < lr.StartTime).ToList();
		if (filteredDeviceIds.Any() || existingLiveRestrictions.Any())
		{
			DateTime lastModified = DateTime.UtcNow;
			using IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent();
			if (filteredDeviceIds.Any())
			{
				RestrictedMediaLiveDataContract restrictedMediaLiveDataContract = liveRestrictions.First((RestrictedMediaLiveDataContract r) => filteredDeviceIds.Contains(r.DeviceId, StringComparer.OrdinalIgnoreCase));
				lastModified = restrictedMediaLiveDataContract.LastModified;
				Guid[] array = restrictedMediaComponent.EnterLive(filteredDeviceIds, restrictedMediaLiveDataContract.UserName, startTime, lastModified, sessionId);
				foreach (Guid item in array)
				{
					hashSet.Add(item);
				}
			}
			if (existingLiveRestrictions.Any())
			{
				try
				{
					Guid[] array = restrictedMediaComponent.UpdateLive(existingLiveRestrictions.Select((RestrictedMediaLiveDataContract lr) => lr.DeviceId).ToArray(), startTime, lastModified, sessionId);
					foreach (Guid item2 in array)
					{
						hashSet.Add(item2);
					}
					foreach (RestrictedMediaLiveDataContract item3 in existingLiveRestrictions)
					{
						item3.StartTime = startTime;
						item3.LastModified = lastModified;
					}
					liveRestrictions.AddRange(existingLiveRestrictions);
				}
				catch (Exception ex) when (ex is DatabaseException || ex is VmoServerServiceException)
				{
					GeminiTrace.Warning("RestrictedMedia EnterLive failed to update: " + ex.ToString());
					if (!filteredDeviceIds.Any())
					{
						throw;
					}
					faultDevices.AddRange(existingLiveRestrictions.Select((RestrictedMediaLiveDataContract d) => CreateFaultDevice(requestContext, d.DeviceId, DeviceType.Unknown, VmoResources.VMO60000, 0)));
				}
			}
		}
		foreach (Guid item4 in hashSet)
		{
			Integration.TryNotifyRecordingServerAsync(item4);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		return liveRestrictions.ToArray();
	}

	private static string[] EnterLive_Validation(IGeminiRequestContext requestContext, string[] deviceIds, ref DateTime startTime, out List<RestrictedMediaLiveDataContract> liveRestrictions, out List<RestrictedMediaLiveDataContract> existingLiveRestrictions, List<FaultDevice> faultDevices)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RestrictedMedia");
		Validation.CheckObjectIdArray(deviceIds, "deviceIds", allowNullOrEmptyIds: false);
		Validation.CheckDateTimeHigherThan1900(ref startTime, "startTime", allowDateMinValue: false);
		DateTime updatedStartTime = startTime;
		if (startTime > DateTime.UtcNow)
		{
			throw new ArgumentException(VmoResources.VMO62001, "startTime");
		}
		liveRestrictions = new List<RestrictedMediaLiveDataContract>();
		existingLiveRestrictions = new List<RestrictedMediaLiveDataContract>();
		deviceIds = RemoveDuplicates(deviceIds);
		deviceIds = CreateOrExtendRestrictedMedia_FilterDeviceIds(requestContext, deviceIds, faultDevices);
		if (deviceIds.Length == 0)
		{
			throw new RestrictedMediaDevicesFailedException("Could not enter restricted live mode. Failed for all devices. (see fault device list for details).");
		}
		using (IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent())
		{
			RestrictedMediaLiveDataContract[] existingLiveRestrictionsLocal = restrictedMediaComponent.QueryLiveByIds(deviceIds);
			deviceIds = deviceIds.Where((string id) => !existingLiveRestrictionsLocal.Any((RestrictedMediaLiveDataContract r) => string.Equals(r.DeviceId, id, StringComparison.OrdinalIgnoreCase))).ToArray();
			existingLiveRestrictions.AddRange(existingLiveRestrictionsLocal);
		}
		DateTime lastModified = DateTime.UtcNow;
		string userName = requestContext.UserDomainAccountName;
		liveRestrictions.AddRange(deviceIds.Select((string deviceId) => new RestrictedMediaLiveDataContract
		{
			DeviceId = deviceId,
			UserName = userName,
			StartTime = updatedStartTime,
			LastModified = lastModified
		}));
		return deviceIds;
	}

	public static RestrictedMediaDataContract ExitLive(IGeminiRequestContext requestContext, string id, string[] deviceIds, string header, string description, DateTime startTime, DateTime endTime, List<FaultDevice> faultDevices)
	{
		Guid sessionId = Guid.NewGuid();
		ExitLive_Validation(requestContext, id, deviceIds, header, description, startTime, endTime, out var restrictedMedia, faultDevices);
		Guid[] array;
		using (IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent())
		{
			array = restrictedMediaComponent.ExitLive(new Guid(id), restrictedMedia.DeviceIds, restrictedMedia.Header, restrictedMedia.Description, restrictedMedia.UserName, restrictedMedia.StartTime, restrictedMedia.EndTime, restrictedMedia.LastModified, sessionId);
		}
		Guid[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Integration.TryNotifyRecordingServerAsync(array2[i]);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return restrictedMedia;
	}

	private static void ExitLive_Validation(IGeminiRequestContext requestContext, string id, string[] deviceIds, string header, string description, DateTime startTime, DateTime endTime, out RestrictedMediaDataContract restrictedMedia, List<FaultDevice> faultDevices)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RestrictedMedia");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		deviceIds = ValidateStandardParameters(deviceIds, header, description, ref startTime, ref endTime);
		List<Device> list = FilterDeviceIds(requestContext, deviceIds, faultDevices);
		FilterCreatePermissionDevices(list, requestContext, faultDevices);
		if (!list.Any())
		{
			throw new RestrictedMediaDevicesFailedException("Could exit restricted live mode. Failed for all devices. (see fault device list for details).");
		}
		using (IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent())
		{
			RestrictedMediaLiveDataContract[] array = restrictedMediaComponent.QueryLiveByIds(list.Select((Device d) => d.Id));
			FilterDevicesNotInRestrictedLiveMode(requestContext, list, array, faultDevices);
			if (!array.Where((RestrictedMediaLiveDataContract lr) => lr.StartTime < startTime).All((RestrictedMediaLiveDataContract d) => HasDevicePermission(requestContext, d.DeviceId, "DELETE_RESTRICTIONS")))
			{
				faultDevices.AddRange(list.Select((Device d) => CreateFaultDevice(requestContext, d.Id, DeviceType.Unknown, VmoResources.VMO61008, 3)));
				list.Clear();
			}
		}
		if (!list.Any())
		{
			throw new RestrictedMediaDevicesFailedException("Could exit restricted live mode. Failed for all devices. (see fault device list for details).");
		}
		restrictedMedia = new RestrictedMediaDataContract
		{
			Id = id,
			DeviceIds = list.Select((Device d) => d.Id).ToArray(),
			Header = header,
			Description = description,
			UserName = requestContext.UserDomainAccountName,
			StartTime = startTime,
			EndTime = endTime,
			LastModified = DateTime.UtcNow
		};
	}

	public static RestrictedMediaDataContract UpdatePlayback(IGeminiRequestContext requestContext, string id, string[] deviceIdsToAdd, string[] deviceIdsToRemove, string header, string description, DateTime startTime, DateTime endTime, List<FaultDevice> faultDevices)
	{
		UpdatePlayback_Validation(requestContext, id, deviceIdsToAdd, deviceIdsToRemove, header, description, startTime, endTime, out var _, out var restrictedMediaAfter, faultDevices);
		Guid sessionId = Guid.NewGuid();
		Guid[] array;
		using (IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent())
		{
			array = restrictedMediaComponent.UpdatePlayback(new Guid(restrictedMediaAfter.Id), restrictedMediaAfter.DeviceIds, restrictedMediaAfter.Header, restrictedMediaAfter.Description, restrictedMediaAfter.StartTime, restrictedMediaAfter.EndTime, restrictedMediaAfter.LastModified, sessionId);
		}
		Guid[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Integration.TryNotifyRecordingServerAsync(array2[i]);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		restrictedMediaAfter.DeviceIds = GetDeviceIdsWithReadPermissions(requestContext, restrictedMediaAfter);
		return restrictedMediaAfter;
	}

	private static void UpdatePlayback_Validation(IGeminiRequestContext requestContext, string id, string[] deviceIdsToAdd, string[] deviceIdsToRemove, string header, string description, DateTime startTime, DateTime endTime, out RestrictedMediaDataContract restrictedMediaBefore, out RestrictedMediaDataContract restrictedMediaAfter, List<FaultDevice> faultDevices)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RestrictedMedia");
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		Validation.CheckObjectIdArray(deviceIdsToAdd, "deviceIdsToAdd", allowNullOrEmptyIds: false, allowEmptyArray: true);
		Validation.CheckObjectIdArray(deviceIdsToRemove, "deviceIdsToRemove", allowNullOrEmptyIds: false, allowEmptyArray: true);
		Validation.CheckName(header, "header");
		Validation.CheckDescription(ref description, "description");
		Validation.CheckDateTimeHigherThan1900(ref startTime, "startTime", allowDateMinValue: false);
		Validation.CheckDateTimeHigherThan1900(ref endTime, "endTime", allowDateMinValue: false);
		deviceIdsToAdd = deviceIdsToAdd ?? Array.Empty<string>();
		deviceIdsToRemove = deviceIdsToRemove ?? Array.Empty<string>();
		restrictedMediaBefore = QueryPlaybackWithoutAnyChecks(requestContext, id);
		if (restrictedMediaBefore == null)
		{
			throw new ObjectNotFoundException();
		}
		deviceIdsToAdd = RemoveDuplicates(deviceIdsToAdd);
		deviceIdsToRemove = RemoveDuplicates(deviceIdsToRemove);
		if (deviceIdsToAdd.Intersect(deviceIdsToRemove, StringComparer.OrdinalIgnoreCase).Any())
		{
			throw new ArgumentException(VmoResources.VMO62001, "deviceIdsToAdd");
		}
		if (deviceIdsToAdd.Any((string d) => d.Equals(Guid.Empty.ToString(), StringComparison.OrdinalIgnoreCase)))
		{
			throw new ArgumentException(VmoResources.VMO62001, "deviceIdsToAdd");
		}
		if (startTime > endTime)
		{
			throw new ArgumentException(VmoResources.VMO62001, "startTime");
		}
		if (endTime > DateTime.UtcNow)
		{
			throw new ArgumentException(VmoResources.VMO62001, "endTime");
		}
		if (restrictedMediaBefore.DeviceIds.Intersect(deviceIdsToAdd, StringComparer.OrdinalIgnoreCase).Any())
		{
			throw new ArgumentException(VmoResources.VMO62001, "deviceIdsToAdd");
		}
		string[] existingDeviceIds = restrictedMediaBefore.DeviceIds;
		if (deviceIdsToRemove.Any((string db) => !existingDeviceIds.Contains(db, StringComparer.OrdinalIgnoreCase)))
		{
			throw new ArgumentException(VmoResources.VMO62001, "deviceIdsToRemove");
		}
		IEnumerable<string> source = restrictedMediaBefore.DeviceIds.Union(deviceIdsToAdd, StringComparer.OrdinalIgnoreCase).Except(deviceIdsToRemove, StringComparer.OrdinalIgnoreCase);
		restrictedMediaAfter = new RestrictedMediaDataContract
		{
			Id = id,
			DeviceIds = source.ToArray(),
			Header = header,
			Description = description,
			UserName = restrictedMediaBefore.UserName,
			StartTime = startTime,
			EndTime = endTime,
			LastModified = DateTime.UtcNow
		};
		UpdatePlayback_CheckSecurity(requestContext, restrictedMediaBefore, restrictedMediaAfter, faultDevices);
	}

	private static FaultDevice CreateFaultDevice(IGeminiRequestContext requestContext, string deviceId, DeviceType type, string message, int code)
	{
		if (type == DeviceType.Unknown)
		{
			type = DeviceTypeLookup.Instance.QueryDeviceTypeFromDeviceId(requestContext, deviceId);
		}
		if (DeviceTypeInformation.DeviceTypeDictionary.ContainsKey(type) && !HasDevicePermission(requestContext, deviceId, type, "GENERIC_READ"))
		{
			deviceId = Guid.Empty.ToString();
		}
		return new FaultDevice(deviceId, message, code, FaultSource.ManagementServer);
	}

	private static void UpdatePlayback_CheckSecurity(IGeminiRequestContext requestContext, RestrictedMediaDataContract restrictedMediaBefore, RestrictedMediaDataContract restrictedMediaAfter, List<FaultDevice> faultDevices)
	{
		IEnumerable<string> enumerable = restrictedMediaAfter.DeviceIds.Except(restrictedMediaBefore.DeviceIds, StringComparer.OrdinalIgnoreCase);
		IEnumerable<string> enumerable2 = restrictedMediaBefore.DeviceIds.Except(restrictedMediaAfter.DeviceIds, StringComparer.OrdinalIgnoreCase);
		HashSet<string> deviceIdsToCheckForCreatePermission = new HashSet<string>(enumerable, StringComparer.OrdinalIgnoreCase);
		HashSet<string> deviceIdsToCheckForDeletePermission = new HashSet<string>(enumerable2, StringComparer.OrdinalIgnoreCase);
		FilterDeviceIds(requestContext, enumerable.Union(enumerable2, StringComparer.OrdinalIgnoreCase), faultDevices);
		if (faultDevices.Any())
		{
			throw new RestrictedMediaFailedException("Could not update media restriction. Failed for one or more devices. (see fault device list for details).");
		}
		if (!restrictedMediaBefore.Header.Equals(restrictedMediaAfter.Header) || !restrictedMediaBefore.Description.Equals(restrictedMediaAfter.Description) || restrictedMediaBefore.StartTime > restrictedMediaAfter.StartTime || restrictedMediaBefore.EndTime < restrictedMediaAfter.EndTime)
		{
			restrictedMediaAfter.DeviceIds.ToList().ForEach(delegate(string id)
			{
				deviceIdsToCheckForCreatePermission.Add(id);
			});
		}
		if (restrictedMediaBefore.StartTime < restrictedMediaAfter.StartTime || restrictedMediaBefore.EndTime > restrictedMediaAfter.EndTime)
		{
			restrictedMediaAfter.DeviceIds.ToList().ForEach(delegate(string id)
			{
				deviceIdsToCheckForDeletePermission.Add(id);
			});
		}
		if (deviceIdsToCheckForCreatePermission.Any((string id) => !HasDevicePermission(requestContext, id, "CREATE_RESTRICTIONS")) || deviceIdsToCheckForDeletePermission.Any((string id) => !HasDevicePermission(requestContext, id, "DELETE_RESTRICTIONS")))
		{
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
	}

	public static RestrictedMediaLiveDataContract[] UpdateLive(IGeminiRequestContext requestContext, string[] deviceIds, DateTime startTime, List<FaultDevice> faultDevices)
	{
		deviceIds = UpdateLive_Validation(requestContext, deviceIds, startTime, out var liveRestrictions, faultDevices);
		Guid sessionId = Guid.NewGuid();
		Guid[] array;
		using (IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent())
		{
			RestrictedMediaLiveDataContract restrictedMediaLiveDataContract = liveRestrictions.First();
			array = restrictedMediaComponent.UpdateLive(deviceIds, restrictedMediaLiveDataContract.StartTime, restrictedMediaLiveDataContract.LastModified, sessionId);
		}
		Guid[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Integration.TryNotifyRecordingServerAsync(array2[i]);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return liveRestrictions;
	}

	private static string[] UpdateLive_Validation(IGeminiRequestContext requestContext, string[] deviceIds, DateTime startTime, out RestrictedMediaLiveDataContract[] liveRestrictions, List<FaultDevice> faultDevices)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RestrictedMedia");
		Validation.CheckObjectIdArray(deviceIds, "deviceIds", allowNullOrEmptyIds: false);
		Validation.CheckDateTimeHigherThan1900(ref startTime, "startTime", allowDateMinValue: false);
		if (startTime > DateTime.UtcNow)
		{
			throw new ArgumentException(VmoResources.VMO62001, "startTime");
		}
		deviceIds = RemoveDuplicates(deviceIds);
		List<Device> list = FilterDeviceIds(requestContext, deviceIds, faultDevices);
		FilterCreatePermissionDevices(list, requestContext, faultDevices);
		if (!list.Any())
		{
			throw new RestrictedMediaDevicesFailedException("Could not update restricted live. Failed for all devices. (see fault device list for details).");
		}
		RestrictedMediaLiveDataContract[] existingLiveRestrictions;
		using (IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent())
		{
			existingLiveRestrictions = restrictedMediaComponent.QueryLiveByIds(list.Select((Device d) => d.Id));
			FilterDevicesNotInRestrictedLiveMode(requestContext, list, existingLiveRestrictions, faultDevices);
			IEnumerable<RestrictedMediaLiveDataContract> source = existingLiveRestrictions.Where((RestrictedMediaLiveDataContract lr) => lr.StartTime < startTime);
			FilterDeletePermissionDevices(list, source.Select((RestrictedMediaLiveDataContract d) => d.DeviceId), requestContext, faultDevices);
		}
		if (!list.Any())
		{
			throw new RestrictedMediaDevicesFailedException("Could not update restricted live. Failed for all devices. (see fault device list for details).");
		}
		DateTime lastModified = DateTime.UtcNow;
		liveRestrictions = list.Select((Device device) => new RestrictedMediaLiveDataContract
		{
			DeviceId = device.Id,
			UserName = existingLiveRestrictions.Single((RestrictedMediaLiveDataContract r) => r.DeviceId.Equals(device.Id, StringComparison.OrdinalIgnoreCase)).UserName,
			StartTime = startTime,
			LastModified = lastModified
		}).ToArray();
		return liveRestrictions.Select((RestrictedMediaLiveDataContract r) => r.DeviceId).ToArray();
	}

	public static void DeletePlayback(IGeminiRequestContext requestContext, string id)
	{
		DeletePlayback_Validation(requestContext, id);
		Guid sessionId = Guid.NewGuid();
		Guid[] array;
		using (IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent())
		{
			array = restrictedMediaComponent.DeletePlayback(new Guid(id), sessionId);
		}
		Guid[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Integration.TryNotifyRecordingServerAsync(array2[i]);
		}
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
	}

	private static void DeletePlayback_Validation(IGeminiRequestContext requestContext, string id)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RestrictedMedia");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		RestrictedMediaDataContract restrictedMediaDataContract = QueryPlaybackWithoutAnyChecks(requestContext, id);
		if (restrictedMediaDataContract == null)
		{
			throw new ObjectNotFoundException();
		}
		CheckDevicePermissions(requestContext, restrictedMediaDataContract.DeviceIds, "DELETE_RESTRICTIONS", throwOnPermissionDenied: true, throwOnUnknowSecurityAction: true);
	}

	public static IEnumerable<Guid> DeleteLive(IGeminiRequestContext requestContext, string[] deviceIds, List<FaultDevice> faultDevices)
	{
		deviceIds = DeleteLive_Validation(requestContext, deviceIds, faultDevices);
		Guid sessionId = Guid.NewGuid();
		Guid[] array;
		using (IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent())
		{
			array = restrictedMediaComponent.DeleteLive(deviceIds, sessionId);
		}
		Guid[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Integration.TryNotifyRecordingServerAsync(array2[i]);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		return deviceIds.Select((string d) => d.ToGuid());
	}

	private static string[] DeleteLive_Validation(IGeminiRequestContext requestContext, string[] deviceIds, List<FaultDevice> faultDevices)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RestrictedMedia");
		Validation.CheckObjectIdArray(deviceIds, "deviceIds", allowNullOrEmptyIds: false);
		deviceIds = RemoveDuplicates(deviceIds);
		List<Device> list = FilterDeviceIds(requestContext, deviceIds, faultDevices);
		if (!deviceIds.Any())
		{
			throw new RestrictedMediaDevicesFailedException("Could not delete restricted live. Failed for all devices. (see fault device list for details).");
		}
		using IRestrictedMediaComponent restrictedMediaComponent = requestContext.CreateRestrictedMediaComponent();
		RestrictedMediaLiveDataContract[] existingLiveRestrictions = restrictedMediaComponent.QueryLiveByIds(list.Select((Device d) => d.Id));
		FilterDevicesNotInRestrictedLiveMode(requestContext, list, existingLiveRestrictions, faultDevices);
		if (!list.All((Device d) => HasDevicePermission(requestContext, d.Id, "DELETE_RESTRICTIONS")))
		{
			faultDevices.AddRange(list.Select((Device d) => CreateFaultDevice(requestContext, d.Id, DeviceType.Unknown, VmoResources.VMO61008, 3)));
			list.Clear();
		}
		deviceIds = list.Select((Device d) => d.Id).ToArray();
		if (!deviceIds.Any())
		{
			throw new RestrictedMediaDevicesFailedException("Could not delete restricted live. Failed for all devices. (see fault device list for details).");
		}
		return deviceIds;
	}

	private static List<Device> FilterDeviceIds(IGeminiRequestContext requestContext, IEnumerable<string> deviceIds, List<FaultDevice> faultDevices)
	{
		List<Device> list;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			list = deviceComponent.QueryDevicesByIds(deviceIds).ToList();
		}
		FilterNotFoundDevices(requestContext, list, deviceIds, faultDevices);
		FilterReadPermissionDevices(list, requestContext, faultDevices);
		FilterNonSupportedDevices(requestContext, list, faultDevices);
		return list;
	}

	private static void FilterDevicesNotInRestrictedLiveMode(IGeminiRequestContext requestContext, List<Device> devices, IEnumerable<RestrictedMediaLiveDataContract> existingLiveRestrictions, List<FaultDevice> faultDevices)
	{
		List<Device> devicesWithoutLiveRestriction = devices.Where((Device d) => !existingLiveRestrictions.Any((RestrictedMediaLiveDataContract e) => e.DeviceId.Equals(d.Id, StringComparison.OrdinalIgnoreCase))).ToList();
		if (!devicesWithoutLiveRestriction.Any())
		{
			return;
		}
		faultDevices.AddRange(devicesWithoutLiveRestriction.Select((Device d) => CreateFaultDevice(requestContext, d.Id, d.DeviceType, VmoResources.VMO60403, 7)));
		devices.RemoveAll((Device d) => devicesWithoutLiveRestriction.Any((Device dlr) => d.Id.Equals(dlr.Id, StringComparison.OrdinalIgnoreCase)));
	}

	private static void FilterNotFoundDevices(IGeminiRequestContext requestContext, List<Device> devices, IEnumerable<string> deviceIds, List<FaultDevice> faultDevices)
	{
		List<string> notFoundDeviceIds = deviceIds.Where((string id) => !devices.Any((Device d) => string.Equals(d.Id, id, StringComparison.OrdinalIgnoreCase))).ToList();
		if (!notFoundDeviceIds.Any())
		{
			return;
		}
		faultDevices.AddRange(notFoundDeviceIds.Select((string id) => CreateFaultDevice(requestContext, id, DeviceType.Unknown, VmoResources.VMO60071, 4)));
		devices.RemoveAll((Device d) => notFoundDeviceIds.Any((string id) => string.Equals(id, d.Id, StringComparison.OrdinalIgnoreCase)));
	}

	private static void FilterReadPermissionDevices(List<Device> devices, IGeminiRequestContext requestContext, List<FaultDevice> faultDevices)
	{
		List<Device> noReadPermissionDevices = devices.Where((Device d) => !HasDevicePermission(requestContext, d.Id, d.DeviceType, "GENERIC_READ")).ToList();
		if (!noReadPermissionDevices.Any())
		{
			return;
		}
		faultDevices.AddRange(noReadPermissionDevices.Select((Device d) => CreateFaultDevice(requestContext, d.Id, d.DeviceType, VmoResources.VMO60071, 4)));
		devices.RemoveAll((Device d) => noReadPermissionDevices.Any((Device n) => string.Equals(n.Id, d.Id, StringComparison.OrdinalIgnoreCase)));
	}

	private static void FilterNonSupportedDevices(IGeminiRequestContext requestContext, List<Device> devices, List<FaultDevice> faultDevices)
	{
		List<Device> notSupportedDevices = devices.Where((Device d) => !DeviceTypeInfo.IsRecordingDevice(d.DeviceType)).ToList();
		if (!notSupportedDevices.Any())
		{
			return;
		}
		faultDevices.AddRange(notSupportedDevices.Select((Device d) => CreateFaultDevice(requestContext, d.Id, d.DeviceType, VmoResources.VMO60374, 5)));
		devices.RemoveAll((Device d) => notSupportedDevices.Any((Device n) => string.Equals(n.Id, d.Id, StringComparison.OrdinalIgnoreCase)));
	}

	private static void FilterCreatePermissionDevices(List<Device> devices, IGeminiRequestContext requestContext, List<FaultDevice> faultDevices)
	{
		List<Device> deniedDevices = devices.Where((Device d) => !HasDevicePermission(requestContext, d.Id, "CREATE_RESTRICTIONS")).ToList();
		if (!deniedDevices.Any())
		{
			return;
		}
		faultDevices.AddRange(deniedDevices.Select((Device d) => CreateFaultDevice(requestContext, d.Id, d.DeviceType, VmoResources.VMO61008, 3)));
		devices.RemoveAll((Device d) => deniedDevices.Any((Device n) => string.Equals(n.Id, d.Id, StringComparison.OrdinalIgnoreCase)));
	}

	private static void FilterDeletePermissionDevices(List<Device> devices, IEnumerable<string> deviceIds, IGeminiRequestContext requestContext, List<FaultDevice> faultDevices)
	{
		List<Device> deniedDevices = devices.Where((Device d) => deviceIds.Any((string i) => string.Equals(i, d.Id, StringComparison.OrdinalIgnoreCase) && !HasDevicePermission(requestContext, d.Id, "DELETE_RESTRICTIONS"))).ToList();
		if (!deniedDevices.Any())
		{
			return;
		}
		faultDevices.AddRange(deniedDevices.Select((Device d) => CreateFaultDevice(requestContext, d.Id, d.DeviceType, VmoResources.VMO61008, 3)));
		devices.RemoveAll((Device d) => deniedDevices.Any((Device n) => string.Equals(n.Id, d.Id, StringComparison.OrdinalIgnoreCase)));
	}

	private static List<FaultDevice> CheckDevicePermissions(IGeminiRequestContext requestContext, IEnumerable<string> deviceIds, string securityActionId, bool throwOnPermissionDenied, bool throwOnUnknowSecurityAction)
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
		FaultDevice faultDevice;
		return CheckDevicePermission(requestContext, deviceId, securityActionId, throwOnPermissionDenied: false, throwOnUnknowSecurityAction: true, out faultDevice);
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
			faultDevice = CreateFaultDevice(requestContext, deviceId, DeviceType.Unknown, VmoResources.VMO60071, 4);
			return false;
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		return CheckDevicePermissionCreateFaultDevice(requestContext, deviceId, securityNamespaceId, securityActionId, throwOnPermissionDenied, throwOnUnknowSecurityAction, out faultDevice);
	}

	private static bool HasDevicePermission(IGeminiRequestContext requestContext, string deviceId, DeviceType deviceType, string securityActionId)
	{
		Guid securityNamespace = DeviceTypeInformation.DeviceTypeDictionary[deviceType].SecurityNamespace;
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, securityNamespace, securityActionId, deviceId))
		{
			return false;
		}
		return true;
	}

	private static bool CheckDevicePermissionCreateFaultDevice(IGeminiRequestContext requestContext, string deviceId, Guid securityNamespaceId, string securityActionId, bool throwOnPermissionDenied, bool throwOnUnknowSecurityAction, out FaultDevice faultDevice)
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
				faultDevice = CreateFaultDevice(requestContext, deviceId, DeviceType.Unknown, VmoResources.VMO61008, 3);
				return false;
			}
		}
		catch (SecurityActionDoesNotExistException)
		{
			if (throwOnUnknowSecurityAction)
			{
				throw;
			}
			faultDevice = CreateFaultDevice(requestContext, deviceId, DeviceType.Unknown, VmoResources.VMO60013, 3);
			return false;
		}
		return true;
	}

	private static string[] ValidateStandardParameters(string[] deviceIds, string header, string description, ref DateTime startTime, ref DateTime endTime)
	{
		Validation.CheckObjectIdArray(deviceIds, "deviceIds", allowNullOrEmptyIds: false);
		Validation.CheckName(header, "header");
		Validation.CheckDescription(ref description, "description");
		Validation.CheckDateTimeHigherThan1900(ref startTime, "startTime", allowDateMinValue: false);
		Validation.CheckDateTimeHigherThan1900(ref endTime, "endTime", allowDateMinValue: false);
		deviceIds = RemoveDuplicates(deviceIds);
		if (startTime > endTime)
		{
			throw new ArgumentException(VmoResources.VMO62001, "startTime");
		}
		if (endTime > DateTime.UtcNow + TimeSpan.FromSeconds(1.0))
		{
			throw new ArgumentException(VmoResources.VMO62001, "endTime");
		}
		return deviceIds;
	}

	private static string[] RemoveDuplicates(string[] deviceIds)
	{
		return new HashSet<string>(deviceIds, StringComparer.OrdinalIgnoreCase).ToArray();
	}
}
