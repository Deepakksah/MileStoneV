using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;

namespace VideoOS.Management.Server;

public sealed class RecordingStorage
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Path { get; set; }

	public bool IsDefault { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public int RetainMinutes { get; set; }

	public long MaxSize { get; set; }

	public int SpaceUsed { get; set; }

	public int SpaceAvailable { get; set; }

	public bool Signing { get; set; }

	public EncryptionMethod Encryption { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public string RecordingServerId { get; set; }

	public static RecordingStorage[] QueryRecordingStorages(IGeminiRequestContext requestContext, string recordingServerId)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		RecordingStorage[] array = Array.Empty<RecordingStorage>();
		using IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent();
		return FilterRecordingStoragesBasedOnPermissions(requestContext, recordingStorageComponent.QueryRecordingStorages(recordingServerId));
	}

	public static RecordingStorage QueryRecordingStorage(IGeminiRequestContext requestContext, string recordingStorageId)
	{
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		RecordingStorage recordingStorage = null;
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			recordingStorage = recordingStorageComponent.QueryRecordingStorage(recordingStorageId);
		}
		if (recordingStorage != null && requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingStorage.RecordingServerId))
		{
			return recordingStorage;
		}
		return null;
	}

	public static RecordingStorage CreateRecordingStorage(IGeminiRequestContext requestContext, RecordingStorage recordingStorage)
	{
		if (recordingStorage.Signing)
		{
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingStorageSigning");
		}
		if (recordingStorage.Encryption != EncryptionMethod.None)
		{
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingStorageEncryption");
		}
		Validation.CheckObjectReference(recordingStorage, "recordingStorage");
		string objectId = recordingStorage.RecordingServerId;
		Validation.CheckObjectId(ref objectId, "recordingServerId", allowNullOrEmpty: false);
		Validation.CheckName(recordingStorage.Name, "name");
		string description = recordingStorage.Description;
		Validation.CheckDescription(ref description, "description");
		recordingStorage.Description = description;
		Validation.CheckPath(recordingStorage.Path, "path");
		Validation.CheckRetainMinutes(recordingStorage.RetainMinutes, "retainMinutes");
		Validation.CheckMaxSize(recordingStorage.MaxSize, "maxSize");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingStorage.RecordingServerId);
		Guid sessionId = Guid.NewGuid();
		recordingStorage.Id = GeminiResourceComponent.GetGuid();
		if (!Integration.RecordingServer.VerifyRecordingStoragePath(recordingStorage.RecordingServerId, recordingStorage.Id, recordingStorage.Path, newPath: true))
		{
			throw new CreateRecordingStoragePathInvalidException();
		}
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			recordingStorageComponent.CreateRecordingStorage(recordingStorage, sessionId);
			recordingStorage = FindRecordingStorage(requestContext, recordingStorageComponent, recordingStorage.Id);
			Integration.TryNotifyRecordingServerAsync(recordingStorage.RecordingServerId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Storage", recordingStorage.Id, EventAction.Added, recordingStorage.Name);
		try
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(5.0);
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			while (!Integration.RecordingServer.VerifyStorageAndArchiveOnline(recordingStorage.RecordingServerId, null, recordingStorage.Id, null) && !(stopwatch.Elapsed >= timeSpan))
			{
				Thread.Sleep(500);
			}
		}
		catch (VmoServerException)
		{
		}
		return recordingStorage;
	}

	public static RecordingStorage UpdateRecordingStorage(IGeminiRequestContext requestContext, RecordingStorage recordingStorage)
	{
		UpdateRecordingStorageValidation(requestContext, recordingStorage.Id, recordingStorage.Name, recordingStorage.RetainMinutes, recordingStorage.MaxSize, recordingStorage.Signing, recordingStorage.Encryption);
		Validation.CheckObjectReference(recordingStorage, "recordingStorage");
		Guid sessionId = Guid.NewGuid();
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			string recordingServerId = recordingStorageComponent.GetRecordingServerId(recordingStorage.Id);
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
			recordingStorageComponent.UpdateRecordingStorage(recordingStorage, sessionId);
			recordingStorage = FindRecordingStorage(requestContext, recordingStorageComponent, recordingStorage.Id);
			Integration.TryNotifyRecordingServerAsync(recordingServerId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Storage", recordingStorage.Id, EventAction.Updated, recordingStorage.Name);
		return recordingStorage;
	}

	public static void UpdateRecordingStorageValidation(IGeminiRequestContext requestContext, string recordingStorageId, string name, int retainMinutes, long maxSize, bool signing, EncryptionMethod encryptionMethod)
	{
		if (signing)
		{
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingStorageSigning");
		}
		if (encryptionMethod != EncryptionMethod.None)
		{
			GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingStorageEncryption");
		}
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		Validation.CheckName(name, "name");
		Validation.CheckRetainMinutes(retainMinutes, "retainMinutes");
		Validation.CheckMaxSize(maxSize, "maxSize");
	}

	public static RecordingStorage[] QueryRecordingStoragesByIds(IGeminiRequestContext requestContext, string[] ids)
	{
		Validation.CheckObjectIdArray(ids, "ids", allowNullOrEmptyIds: true);
		RecordingStorage[] array = Array.Empty<RecordingStorage>();
		using IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent();
		return FilterRecordingStoragesBasedOnPermissions(requestContext, recordingStorageComponent.QueryRecordingStorages(ids));
	}

	public static string[] QueryRecordingStorageIds(IGeminiRequestContext requestContext, string recordingServerId)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		string[] result = new string[0];
		if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId))
		{
			using IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent();
			result = recordingStorageComponent.QueryRecordingStorageIds(recordingServerId);
		}
		return result;
	}

	public static string[] QueryRecordingStorageIdsByDeviceId(IGeminiRequestContext requestContext, string deviceId)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		string[] result = new string[0];
		string idFromDeviceId;
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			idFromDeviceId = recordingServerComponent.GetIdFromDeviceId(deviceId);
		}
		if (idFromDeviceId != null && requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", idFromDeviceId))
		{
			using IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent();
			result = recordingStorageComponent.QueryRecordingStorageIdsByDeviceId(deviceId);
		}
		return result;
	}

	public static void SetRecordingStorageAsDefault(IGeminiRequestContext requestContext, string recordingStorageId)
	{
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		using IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent();
		string recordingServerId = recordingStorageComponent.GetRecordingServerId(recordingStorageId);
		if (string.IsNullOrEmpty(recordingServerId))
		{
			throw new SetNonExistingRecordingStorageAsDefaultException();
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
		recordingStorageComponent.SetRecordingStorageAsDefault(recordingStorageId);
	}

	public static string GetIdFromArchive(IGeminiRequestContext requestContext, string archiveStorageId)
	{
		Validation.CheckObjectId(ref archiveStorageId, "archiveStorageId", allowNullOrEmpty: false);
		using IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent();
		return recordingStorageComponent.GetIdFromArchive(archiveStorageId);
	}

	public static bool ValidateEncryptionPassword(IGeminiRequestContext requestContext, string recordingStorageId, string password)
	{
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		Validation.CheckDescription(ref password, "password");
		using IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent();
		string recordingServerId = recordingStorageComponent.GetRecordingServerId(recordingStorageId);
		if (recordingServerId == null)
		{
			throw new RecordingStorageUpdateException();
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
		return recordingStorageComponent.QueryEncryptionPassword(recordingStorageId).Equals(password);
	}

	private static RecordingStorage FindRecordingStorage(IGeminiRequestContext requestContext, IRecordingStorageComponent recordingStorageComponent, string recordingStorageId)
	{
		bool flag = false;
		try
		{
			if (recordingStorageComponent == null)
			{
				recordingStorageComponent = requestContext.CreateRecordingStorageComponent();
				flag = true;
			}
			return recordingStorageComponent.QueryRecordingStorage(recordingStorageId);
		}
		finally
		{
			if (flag)
			{
				recordingStorageComponent.Dispose();
			}
		}
	}

	private static RecordingStorage[] FilterRecordingStoragesBasedOnPermissions(IGeminiRequestContext requestContext, RecordingStorage[] recordingStorageArray)
	{
		if (recordingStorageArray == null)
		{
			throw new ArgumentNullException("recordingStorageArray");
		}
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingStorageArray.Select((RecordingStorage r) => (r: r, Id: r.Id, RecordingServerId: r.RecordingServerId))).ToArray();
	}
}
