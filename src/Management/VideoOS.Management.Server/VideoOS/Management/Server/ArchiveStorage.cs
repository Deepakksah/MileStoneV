using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public static class ArchiveStorage
{
	public static ArchiveStorageDataContract[] QueryArchiveStorages(IGeminiRequestContext requestContext, string recordingStorageId)
	{
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		ArchiveStorageDataContract[] archiveStorageArray = Array.Empty<ArchiveStorageDataContract>();
		using (IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent())
		{
			archiveStorageArray = archiveStorageComponent.QueryArchiveStorages(recordingStorageId);
		}
		return FilterArchiveStoragesBasedOnPermissions(requestContext, archiveStorageArray);
	}

	public static ArchiveStorageDataContract QueryArchiveStorage(IGeminiRequestContext requestContext, string archiveStorageId)
	{
		ArchiveStorageDataContract archiveStorageDataContract = null;
		using (IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent())
		{
			archiveStorageDataContract = archiveStorageComponent.QueryArchiveStorage(archiveStorageId);
		}
		if (archiveStorageDataContract != null && requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", archiveStorageDataContract.RecordingServerId))
		{
			return archiveStorageDataContract;
		}
		return null;
	}

	public static ArchiveStorageDataContract CreateArchiveStorage(IGeminiRequestContext requestContext, ArchiveStorageDataContract archiveStorage)
	{
		if (archiveStorage.FramerateReductionEnabled)
		{
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "ArchiveReducedFrameRate");
		}
		string objectId = archiveStorage.RecordingStorageId;
		Validation.CheckObjectId(ref objectId, "recordingStorageId", allowNullOrEmpty: false);
		Validation.CheckName(archiveStorage.Name, "Name");
		string description = archiveStorage.Description;
		Validation.CheckDescription(ref description, "Description");
		archiveStorage.Description = description;
		Validation.CheckPath(archiveStorage.Path, "Path");
		Validation.CheckRetainMinutes(archiveStorage.RetainMinutes, "RetainMinutes");
		Validation.CheckMaxSize(archiveStorage.MaxSize, "MaxSize");
		Validation.CheckTargetFramerate(archiveStorage.TargetFramerate, "TargetFramerate");
		Validation.CheckSchedule(archiveStorage.ArchiveSchedule, "ArchiveSchedule");
		archiveStorage.Id = GeminiResourceComponent.GetGuid();
		Guid sessionId = Guid.NewGuid();
		string recordingServerId;
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			using IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent();
			recordingServerId = recordingStorageComponent.GetRecordingServerId(archiveStorage.RecordingStorageId);
			if (recordingServerId == null)
			{
				throw new RecordingStorageUpdateException();
			}
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
			if (!Integration.RecordingServer.VerifyArchiveStoragePath(recordingServerId, archiveStorage.Id, archiveStorage.Path, newPath: true))
			{
				throw new CreateArchiveStoragePathInvalidException();
			}
			string featureParameterValue = GeminiApplicationCore.FeatureCacheManager.GetFeatureParameterValue(requestContext, "MultiStageVideoStorage", "MultiStageVideoStorageLimit");
			int result = 0;
			if (!int.TryParse(featureParameterValue, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
			{
				result = int.MaxValue;
			}
			archiveStorageComponent.CreateArchiveStorage(archiveStorage.RecordingStorageId, archiveStorage, sessionId, result);
			archiveStorage = FindArchiveStorage(requestContext, archiveStorageComponent, archiveStorage.Id);
			Integration.TryNotifyRecordingServerAsync(recordingServerId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "ArchiveStorage", archiveStorage.Id, EventAction.Added, archiveStorage.Name);
		try
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(5.0);
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			while (!Integration.RecordingServer.VerifyStorageAndArchiveOnline(recordingServerId, null, null, archiveStorage.Id) && !(stopwatch.Elapsed >= timeSpan))
			{
				Thread.Sleep(500);
			}
		}
		catch (VmoServerException)
		{
		}
		return archiveStorage;
	}

	public static ArchiveStorageDataContract UpdateArchiveStorage(IGeminiRequestContext requestContext, ArchiveStorageDataContract archiveStorage)
	{
		UpdateArchiveStorageValidation(requestContext, archiveStorage.Id, archiveStorage.Name, archiveStorage.Description, archiveStorage.RetainMinutes, archiveStorage.MaxSize, archiveStorage.FramerateReductionEnabled, archiveStorage.TargetFramerate, archiveStorage.ArchiveSchedule);
		Guid sessionId = Guid.NewGuid();
		using (IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent())
		{
			string recordingServerId = archiveStorageComponent.GetRecordingServerId(archiveStorage.Id);
			if (recordingServerId == null)
			{
				throw new ArchiveStorageUpdateException();
			}
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
			archiveStorageComponent.UpdateArchiveStorage(archiveStorage, sessionId);
			archiveStorage = FindArchiveStorage(requestContext, archiveStorageComponent, archiveStorage.Id);
			Integration.TryNotifyRecordingServerAsync(archiveStorage.RecordingServerId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "ArchiveStorage", archiveStorage.Id, EventAction.Updated, archiveStorage.Name);
		return archiveStorage;
	}

	public static void UpdateArchiveStorageValidation(IGeminiRequestContext requestContext, string archiveStorageId, string name, string description, int retainMinutes, long maxSize, bool framerateReductionEnabled, double targetFramerate, string archiveSchedule)
	{
		if (framerateReductionEnabled)
		{
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "ArchiveReducedFrameRate");
		}
		Validation.CheckRetainMinutes(retainMinutes, "RetainMinutes");
		Validation.CheckMaxSize(maxSize, "MaxSize");
		Validation.CheckTargetFramerate(targetFramerate, "TargetFramerate");
		Validation.CheckSchedule(archiveSchedule, "ArchiveSchedule");
		Validation.CheckObjectId(ref archiveStorageId, "archiveStorageId", allowNullOrEmpty: false);
		Validation.CheckName(name, "name");
		Validation.CheckDescription(ref description, "Description");
	}

	public static ArchiveStorageDataContract[] QueryArchiveStoragesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		ArchiveStorageDataContract[] archiveStorageArray = Array.Empty<ArchiveStorageDataContract>();
		using (IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent())
		{
			archiveStorageArray = archiveStorageComponent.QueryArchiveStorages(ids);
		}
		return FilterArchiveStoragesBasedOnPermissions(requestContext, archiveStorageArray);
	}

	public static string[] QueryArchiveStorageIdsByRecordingServerId(IGeminiRequestContext requestContext, string recordingServerId)
	{
		string[] result = new string[0];
		if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId))
		{
			using IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent();
			result = archiveStorageComponent.QueryArchiveStorageIdsByRecordingServerId(recordingServerId);
		}
		return result;
	}

	public static string[] QueryArchiveStorageIdsByRecordingStorageId(IGeminiRequestContext requestContext, string recordingStorageId)
	{
		string[] result = new string[0];
		string recordingServerId;
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			recordingServerId = recordingStorageComponent.GetRecordingServerId(recordingStorageId);
		}
		if (recordingServerId != null && requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId))
		{
			using IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent();
			result = archiveStorageComponent.QueryArchiveStorageIdsByRecordingStorageId(recordingStorageId);
		}
		return result;
	}

	public static string[] QueryArchiveStorageIdsByDeviceId(IGeminiRequestContext requestContext, string deviceId)
	{
		string[] result = new string[0];
		string idFromDeviceId;
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			idFromDeviceId = recordingServerComponent.GetIdFromDeviceId(deviceId);
		}
		if (idFromDeviceId != null && requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", idFromDeviceId))
		{
			using IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent();
			result = archiveStorageComponent.QueryArchiveStorageIdsByDeviceId(deviceId);
		}
		return result;
	}

	public static void EnableFramerateReduction(IGeminiRequestContext requestContext, string archiveStorageId, bool enable)
	{
		if (enable)
		{
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "ArchiveReducedFrameRate");
		}
		Validation.CheckObjectId(ref archiveStorageId, "archiveStorageId", allowNullOrEmpty: false);
		Guid sessionId = Guid.NewGuid();
		using (IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent())
		{
			string recordingServerId = archiveStorageComponent.GetRecordingServerId(archiveStorageId);
			if (recordingServerId == null)
			{
				throw new ArchiveStorageUpdateException();
			}
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
			archiveStorageComponent.EnableFramerateReduction(archiveStorageId, enable, sessionId);
			Integration.TryNotifyRecordingServerAsync(recordingServerId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "ArchiveStorage", archiveStorageId, EventAction.Updated, string.Empty);
	}

	private static ArchiveStorageDataContract FindArchiveStorage(IGeminiRequestContext requestContext, IArchiveStorageComponent archiveStorageComponent, string archiveStorageId)
	{
		bool flag = false;
		try
		{
			if (archiveStorageComponent == null)
			{
				archiveStorageComponent = requestContext.CreateArchiveStorageComponent();
				flag = true;
			}
			return archiveStorageComponent.QueryArchiveStorage(archiveStorageId);
		}
		finally
		{
			if (flag)
			{
				archiveStorageComponent.Dispose();
			}
		}
	}

	private static ArchiveStorageDataContract[] FilterArchiveStoragesBasedOnPermissions(IGeminiRequestContext requestContext, ArchiveStorageDataContract[] archiveStorageArray)
	{
		if (archiveStorageArray == null)
		{
			throw new ArgumentNullException("archiveStorageArray");
		}
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", archiveStorageArray.Select((ArchiveStorageDataContract a) => (a: a, Id: a.Id, RecordingServerId: a.RecordingServerId))).ToArray();
	}
}
