using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.DeviceDiscovery;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;
using VideoOS.Management.Server.TaskSecurityProviders;

namespace VideoOS.Management.Server;

public sealed class Task
{
	public enum TaskType
	{
		Base,
		AddHardware,
		DeleteHardware,
		MoveHardware,
		HardwareScan,
		HardwareScanExpress,
		ChangeHardwarePassword,
		ActivateLicense,
		TruncateDeviceStorage,
		MoveRecordingStorage,
		MoveArchive,
		MountArchive,
		UnmountArchive,
		DeleteArchive,
		DeleteStorage,
		StorageRetrieval,
		MoveDeviceDb,
		SetEncryptionMethod,
		SetEncryptionPassword,
		Archiving,
		Repairing,
		UpdateHardware,
		FirmwareUpgrade,
		AddIdpAdmin,
		DeleteIdpAdmin,
		DeleteIdpUser,
		GetDevicePresets,
		ReplaceHardware,
		DeleteRecorder
	}

	public enum RetentionPolicy
	{
		Default,
		KeepForever
	}

	public string Id { get; set; }

	public string TypeDb { get; set; }

	public TaskType Type { get; set; }

	public string Target { get; set; }

	public string Information { get; set; }

	public AsyncStatus AsyncStatus { get; set; }

	public string AsyncOperation { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }

	public int PercentCompleted { get; set; }

	public TaskPropertyCollection TaskProperties { get; set; }

	public static Task[] QueryTasksByRecordingServer(IGeminiRequestContext requestContext, string recordingServerId)
	{
		Task[] tasksArray = null;
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			tasksArray = taskComponent.QueryTasksByRecordingServer(recordingServerId);
		}
		return FilterTasksBasedOnPermissions(requestContext, tasksArray);
	}

	public static Task[] QueryTasksByParentRecordingServer(IGeminiRequestContext requestContext, string recordingServerId)
	{
		Task[] tasksArray = null;
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			tasksArray = taskComponent.QueryTasksByParentRecordingServer(recordingServerId);
		}
		return FilterTasksBasedOnPermissions(requestContext, tasksArray);
	}

	public static Task[] QueryTasksForManagementServer(IGeminiRequestContext requestContext)
	{
		Task[] tasksArray = null;
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			tasksArray = taskComponent.QueryTasksForManagementServer();
		}
		return FilterTasksBasedOnPermissions(requestContext, tasksArray);
	}

	public static Task QueryTask(IGeminiRequestContext requestContext, string taskId)
	{
		Validation.CheckObjectIdAsInt32(ref taskId, "taskId", allowNullOrEmpty: false);
		Task task = null;
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			task = taskComponent.QueryTask(taskId);
		}
		if (task == null)
		{
			return null;
		}
		TaskSecurityProviderFactory.GetSecurityProvider(task.Type).CheckRead(requestContext, task);
		return task;
	}

	public static Task[] QueryTasks(IGeminiRequestContext requestContext, string[] taskIds)
	{
		Task[] tasksArray = null;
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			tasksArray = taskComponent.QueryTasksByIds(taskIds);
		}
		return FilterTasksBasedOnPermissions(requestContext, tasksArray);
	}

	private static Task QueryTaskWithoutSecurityExceptions(IGeminiRequestContext requestContext, string taskId)
	{
		try
		{
			return QueryTask(requestContext, taskId);
		}
		catch (UnauthorizedAccessException)
		{
			return null;
		}
	}

	public static string[] QueryTaskIds(IGeminiRequestContext requestContext, string recordingServerId)
	{
		return QueryTaskIds(requestContext, TaskType.Base, recordingServerId);
	}

	public static string[] QueryTaskIds(IGeminiRequestContext requestContext, TaskType type, string recordingServerId)
	{
		string[] array = null;
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		using ITaskComponent taskComponent = requestContext.CreateTaskComponent();
		array = ((type != TaskType.Base) ? (from id in taskComponent.QueryTasksIds(type, recordingServerId)
			select id.ToString(CultureInfo.InvariantCulture)).ToArray() : (from id in taskComponent.QueryTasksIds(recordingServerId)
			select id.ToString(CultureInfo.InvariantCulture)).ToArray());
		Task[] tasksArray = taskComponent.QueryTasksByIds(array);
		tasksArray = FilterTasksBasedOnPermissions(requestContext, tasksArray);
		return tasksArray.Select((Task f) => f.Id.ToString(CultureInfo.InvariantCulture)).ToArray();
	}

	public static string[] QueryAllTaskIds(IGeminiRequestContext requestContext)
	{
		string[] array = null;
		using ITaskComponent taskComponent = requestContext.CreateTaskComponent();
		array = (from id in taskComponent.QueryAllTasksIds()
			select id.ToString(CultureInfo.InvariantCulture)).ToArray();
		Task[] tasksArray = taskComponent.QueryTasksByIds(array);
		tasksArray = FilterTasksBasedOnPermissions(requestContext, tasksArray);
		return tasksArray.Select((Task f) => f.Id.ToString(CultureInfo.InvariantCulture)).ToArray();
	}

	public static void RequestStopTaskStopEdgeStorageRetrieval(IGeminiRequestContext requestContext, string taskId)
	{
		Validation.CheckObjectIdAsInt32(ref taskId, "taskId", allowNullOrEmpty: false);
		Task task = QueryTaskWithoutSecurityExceptions(requestContext, taskId);
		if (task != null)
		{
			RequestStopTaskStopEdgeStorageRetrieval(requestContext, task);
		}
	}

	public static void RequestStopTaskStopEdgeStorageRetrieval(IGeminiRequestContext requestContext, Task task)
	{
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EdgeStorage"))
		{
			throw new FeatureNotLicensedException();
		}
		KeyValue value = null;
		if (task.Type == TaskType.StorageRetrieval && task.TaskProperties.ToDictionary((KeyValue p) => p.Key).TryGetValue("idRecorder", out value))
		{
			RequestStopTask(requestContext, task);
			Integration.TryNotifyRecordingServerAsync(new Guid(value.Value));
			Integration.FireEdgeJobUpdatedEvent(requestContext, task.Id, EventAction.Updated);
		}
	}

	public static void RequestStopTask(IGeminiRequestContext requestContext, Task task)
	{
		TaskSecurityProviderFactory.GetSecurityProvider(task.Type).CheckStop(requestContext, task);
		using ITaskComponent taskComponent = requestContext.CreateTaskComponent();
		taskComponent.RequestStopTask(task.Id);
	}

	public static void CleanupExpiredTasks(IGeminiRequestContext requestContext, TaskType taskType, int retainMinutes)
	{
		using ITaskComponent taskComponent = requestContext.CreateTaskComponent();
		taskComponent.CleanupExpiredTasks(taskType, retainMinutes);
	}

	public static Task AddHardware(IGeminiRequestContext requestContext, string recordingServerId, string address, string userName, string password, int driverNumber, string definitionXml, string customData)
	{
		Task result = null;
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		Validation.CheckWebAddress(address, allowHttps: true, "address");
		Validation.EnsureStringNotNull(ref userName, "userName");
		Validation.EnsureStringNotNull(ref password, "password");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", recordingServerId);
		if (definitionXml == null)
		{
			definitionXml = string.Empty;
		}
		if (customData == null)
		{
			customData = string.Empty;
		}
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.AddHardware(recordingServerId, address, userName, password, driverNumber, definitionXml, customData).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		return result;
	}

	public static Task HardwareScan(IGeminiRequestContext requestContext, string recordingServerId, string address, string userName, string password, bool useDefaultCredentials)
	{
		return HardwareScan(requestContext, recordingServerId, address, userName, password, useDefaultCredentials, null);
	}

	public static Task HardwareScan(IGeminiRequestContext requestContext, string recordingServerId, string address, string userName, string password, bool useDefaultCredentials, int[] selectedHardwareDriverIds)
	{
		return HardwareScan(requestContext, recordingServerId, address, userName, password, useDefaultCredentials, selectedHardwareDriverIds, new AddressDriverPairs());
	}

	public static Task HardwareScan(IGeminiRequestContext requestContext, string recordingServerId, string address, string userName, string password, bool useDefaultCredentials, int[] selectedHardwareDriverIds, AddressDriverPairs addressDriverPairs)
	{
		Task result = null;
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		Validation.CheckWebAddress(address, allowHttps: true, "address");
		Validation.CheckUserName(userName, "userName", useDefaultCredentials);
		Validation.CheckUserName(password, "password", useDefaultCredentials);
		if (!string.IsNullOrEmpty(userName) && password == null)
		{
			password = string.Empty;
		}
		HardwareDriver[] driverOptions = GetDriverOptions(requestContext, recordingServerId, selectedHardwareDriverIds);
		ScanPriorities selectedDrivers = GetSelectedDrivers(driverOptions);
		ScanPriorities prioritizedDrivers = GetPrioritizedDrivers(driverOptions);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", recordingServerId);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.HardwareScan(recordingServerId, address, userName, password, useDefaultCredentials, selectedDrivers.ToXml(), prioritizedDrivers.ToXml(), addressDriverPairs.ToXml()).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		return result;
	}

	private static ScanPriorities GetPrioritizedDrivers(HardwareDriver[] selectedDrivers)
	{
		ScanPriorities scanPriorities = new ScanPriorities();
		foreach (HardwareDriver hardwareDriver in selectedDrivers)
		{
			if (hardwareDriver.UseCount > 0)
			{
				scanPriorities.Items.Add(new ScanPriority(hardwareDriver.Number, hardwareDriver.UseCount));
			}
		}
		return scanPriorities;
	}

	private static ScanPriorities GetSelectedDrivers(HardwareDriver[] selectedDrivers)
	{
		ScanPriorities scanPriorities = new ScanPriorities();
		foreach (HardwareDriver hardwareDriver in selectedDrivers)
		{
			scanPriorities.Items.Add(new ScanPriority(hardwareDriver.Number, hardwareDriver.UseCount));
		}
		return scanPriorities;
	}

	private static HardwareDriver[] GetDriverOptions(IGeminiRequestContext requestContext, string recordingServerId)
	{
		return GetDriverOptions(requestContext, recordingServerId, null);
	}

	private static HardwareDriver[] GetDriverOptions(IGeminiRequestContext requestContext, string recordingServerId, int[] selectedHardwareDriverIds)
	{
		HardwareDriver[] source = HardwareDriver.QueryActiveHardwareDrivers(requestContext, recordingServerId);
		HardwareDriver[] array = null;
		if (selectedHardwareDriverIds == null)
		{
			return source.Where((HardwareDriver f) => !DriverNumbering.IsIcpDriver(f.Number) && !DriverNumbering.IsMipDriver(f.Number)).ToArray();
		}
		return source.Where((HardwareDriver driver) => selectedHardwareDriverIds.Contains(driver.Number)).ToArray();
	}

	public static Task HardwareScanExpress(IGeminiRequestContext requestContext, string recordingServerId, Credentials credentials, bool useDefaultCredentials, bool useSecurePort)
	{
		Task result = null;
		ValidateHardwareScanExpressParameters(ref recordingServerId, credentials, useDefaultCredentials);
		ScanPriorities prioritizedDrivers = GetPrioritizedDrivers(GetDriverOptions(requestContext, recordingServerId));
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", recordingServerId);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.HardwareScanExpress(recordingServerId, credentials.ToXml(), useDefaultCredentials, useSecurePort, prioritizedDrivers.ToXml()).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		return result;
	}

	public static Task GetDevicePresets(IGeminiRequestContext requestContext, string recordingServerId, string deviceId, long sessionDataId)
	{
		Task result = null;
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_WRITE", deviceId);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.GetDevicePresets(recordingServerId, deviceId, sessionDataId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		return result;
	}

	private static void ValidateHardwareScanExpressParameters(ref string recordingServerId, Credentials credentials, bool useDefaultCredentials)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		Validation.CheckCredentialList(credentials, "credentials", useDefaultCredentials);
	}

	public static Task DeleteHardware(IGeminiRequestContext requestContext, string hardwareId)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(ref hardwareId, "hardwareId", allowNullOrEmpty: false);
		Task result = null;
		string recordingServerId;
		using (IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent())
		{
			recordingServerId = (hardwareComponent.QueryHardwareById(hardwareId) ?? throw new HardwareNotFoundException()).RecordingServerId;
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", recordingServerId);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.DeleteHardware(hardwareId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		return result;
	}

	public static Task DeleteRecordingServer(IGeminiRequestContext requestContext, string recorderId, bool userAcceptedToDeleteMarkedData)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(ref recorderId, "idRecorder", allowNullOrEmpty: false);
		Task result = null;
		using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
		{
			if (recordingServerComponent.QueryRecordingServer(recorderId) == null)
			{
				throw new RecordingServerNotFoundException();
			}
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "DELETE", recorderId);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.DeleteRecordingServer(recorderId, userAcceptedToDeleteMarkedData).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		return result;
	}

	public static Task ChangeHardwarePassword(IGeminiRequestContext requestContext, string hardwareId, string userName, string password)
	{
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref hardwareId, "hardwareId", allowNullOrEmpty: false);
		Validation.EnsureStringNotNull(ref userName, "userName");
		Validation.EnsureStringNotNull(ref password, "password");
		ValidatePassword(requestContext, hardwareId, password);
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "DevicePasswordManagement"))
		{
			throw new FeatureNotLicensedException();
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", hardwareId);
		Task result;
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.ChangeHardwarePassword(hardwareId, userName, password, sessionId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return result;
	}

	private static void ValidatePassword(IGeminiRequestContext requestContext, string hardwareId, string password)
	{
		DriverSettingsBusiness.QueryHardwareSettings(requestContext, hardwareId, out var _, out var hardwareSettings);
		GetMinMaxPasswordLength(hardwareSettings, out var minPasswordLength, out var maxPasswordLength);
		ValidatePasswordLength(minPasswordLength, maxPasswordLength, password);
		PasswordValidationResult passwordValidationResult = new PasswordRequirements(new List<CharacterSet>(), new CharacterSet(), needsUppercaseLetters: true, needsLowercaseLetters: true, needsDigits: true).ValidatePassword(password);
		if (passwordValidationResult != PasswordValidationResult.Success)
		{
			throw new ArgumentException(GetExceptionInformationFromResult(passwordValidationResult), "Password");
		}
	}

	private static void GetMinMaxPasswordLength(DriverSettings driverSettings, out int minPasswordLength, out int maxPasswordLength)
	{
		int num = 1;
		int num2 = 64;
		string valueByTypeId = driverSettings.GetValueByTypeId("hardware", TypeReferenceIds.PasswordChangeMinLength.ToString());
		string valueByTypeId2 = driverSettings.GetValueByTypeId("hardware", TypeReferenceIds.PasswordChangeMaxLength.ToString());
		if (!int.TryParse(valueByTypeId, out minPasswordLength))
		{
			minPasswordLength = num;
		}
		if (!int.TryParse(valueByTypeId2, out maxPasswordLength))
		{
			maxPasswordLength = num2;
		}
		else if (maxPasswordLength > num2)
		{
			maxPasswordLength = num2;
		}
	}

	private static void ValidatePasswordLength(int minPasswordLenght, int maxPasswordLength, string password)
	{
		int length = password.Length;
		if (length < minPasswordLenght || length > maxPasswordLength)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66035, minPasswordLenght, maxPasswordLength), "Password");
		}
	}

	private static string GetExceptionInformationFromResult(PasswordValidationResult result)
	{
		return result switch
		{
			PasswordValidationResult.UsesForbiddenCharacter => VmoResources.VMO66030, 
			PasswordValidationResult.NeedsUppercase => VmoResources.VMO66031, 
			PasswordValidationResult.NeedsLowercase => VmoResources.VMO66032, 
			PasswordValidationResult.NeedsDigits => VmoResources.VMO66033, 
			_ => VmoResources.VMO66034, 
		};
	}

	public static Task FirmwareUpgrade(IGeminiRequestContext requestContext, string hardwareId, string fileId)
	{
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref hardwareId, "hardwareId", allowNullOrEmpty: false);
		Validation.EnsureStringNotNull(ref fileId, "fileId");
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", hardwareId);
		Task result;
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.FirmwareUpgrade(hardwareId, fileId, sessionId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return result;
	}

	public static Task MoveHardware(IGeminiRequestContext requestContext, string hardwareId, string destinationRecorderId, string destinationStorageId, bool ignoreSourceRecordingServer)
	{
		Task result = null;
		Guid sessionId = Guid.NewGuid();
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(ref hardwareId, "hardwareId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref destinationRecorderId, "destinationRecorderId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref destinationStorageId, "destinationStorageId", allowNullOrEmpty: false);
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		securityManagerService.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", destinationRecorderId);
		string recordingServerId;
		using (IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent())
		{
			recordingServerId = (hardwareComponent.QueryHardwareById(hardwareId) ?? throw new HardwareNotFoundException()).RecordingServerId;
		}
		securityManagerService.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", recordingServerId);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.MoveHardware(hardwareId, destinationRecorderId, destinationStorageId, ignoreSourceRecordingServer, sessionId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return result;
	}

	public static Task UpdateHardware(IGeminiRequestContext requestContext, string hardwareId, long sessionDataId)
	{
		Task result = null;
		Guid sessionId = Guid.NewGuid();
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(ref hardwareId, "hardwareId", allowNullOrEmpty: false);
		string recordingServerId;
		using (IHardwareComponent hardwareComponent = requestContext.CreateHardwareComponent())
		{
			recordingServerId = (hardwareComponent.QueryHardwareById(hardwareId) ?? throw new HardwareNotFoundException()).RecordingServerId;
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", recordingServerId);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.UpdateHardware(hardwareId, sessionId, sessionDataId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return result;
	}

	public static Task ReplaceHardware(IGeminiRequestContext requestContext, string hardwareId, string address, int port, bool useHttps, int httpsPort, string userName, string password, int driverNumber, string customDriverData, bool allowDeletingDisabledDevices)
	{
		Task result = null;
		Validation.CheckRequestContext(requestContext);
		Validation.CheckObjectId(hardwareId, "hardwareId", allowNullOrEmpty: false);
		Validation.CheckHostName(address, "Address");
		Validation.CheckNetworkPort(port, "Port", allowNull: false);
		Validation.CheckNetworkPort(httpsPort, "HttpsPort", allowNull: false);
		Validation.CheckHardwareUserName(userName, "UserName", allowNull: false);
		Validation.CheckStringNotNull(password, "Password");
		customDriverData = customDriverData ?? string.Empty;
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", hardwareId);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.ReplaceHardware(hardwareId, address, port, useHttps, httpsPort, userName, password, driverNumber, customDriverData, allowDeletingDisabledDevices, requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		return result;
	}

	public static Task ActivateLicense(IGeminiRequestContext requestContext, string userName, string password)
	{
		Validation.CheckRequestContext(requestContext);
		Validation.CheckStringNotNullOrWhiteSpace(userName, "userName");
		Validation.CheckStringNotNullOrWhiteSpace(password, "password");
		Task result;
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.ActivateLicense(userName, password).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		return result;
	}

	public static Task AddIdpAdmin(IGeminiRequestContext requestContext, string id)
	{
		Task result = null;
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.AddIdpAdmin(id).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		return result;
	}

	public static Task DeleteIdpAdmin(IGeminiRequestContext requestContext, string id)
	{
		Validation.CheckRequestContext(requestContext);
		Task result = null;
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.DeleteIdpAdmin(id).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		return result;
	}

	public static Task DeleteIdpUser(IGeminiRequestContext requestContext, string id)
	{
		Validation.CheckRequestContext(requestContext);
		Task result = null;
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.DeleteIdpUser(id).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyServerAsync();
		return result;
	}

	public static Task TruncateDeviceStorage(IGeminiRequestContext requestContext, string deviceId, string recordingServerId)
	{
		Task task = null;
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: true);
		Guid namespaceId = DeviceTypeLookup.Instance.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, namespaceId, "DELETE_RECORDINGS", deviceId);
		Device device = null;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			device = deviceComponent.QueryDeviceById(deviceId);
			if (device == null)
			{
				throw new DeviceUpdateException();
			}
		}
		if (string.IsNullOrEmpty(recordingServerId))
		{
			recordingServerId = device.RecordingServerId;
		}
		if (string.Equals(recordingServerId, device.RecordingServerId, StringComparison.OrdinalIgnoreCase) && !Integration.RecordingServer.VerifyStorageAndArchiveOnline(recordingServerId, deviceId, string.Empty, string.Empty))
		{
			throw VmoExceptionFactory.VMO61109_VmoServerServiceException();
		}
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			task = taskComponent.QueryTask(taskComponent.TruncateDeviceStorage(deviceId, recordingServerId, sessionId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyRecordingServerAsync(new Guid(task.Target));
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return task;
	}

	public static Task MoveRecordingStorage(IGeminiRequestContext requestContext, string recordingStorageId, string newPath)
	{
		Task task = null;
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			string recordingServerId = recordingStorageComponent.GetRecordingServerId(recordingStorageId);
			if (recordingServerId == null)
			{
				throw new RecordingStorageUpdateException();
			}
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
			if (!Integration.RecordingServer.VerifyStorageAndArchiveOnline(recordingServerId, string.Empty, recordingStorageId, string.Empty))
			{
				throw VmoExceptionFactory.VMO61109_VmoServerServiceException();
			}
			if (!Integration.RecordingServer.VerifyRecordingStoragePath(recordingServerId, recordingStorageId, newPath, newPath: true))
			{
				throw new RecordingStorageMovePathInvalidException();
			}
		}
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			task = taskComponent.QueryTask(taskComponent.MoveRecordingStorage(recordingStorageId, newPath, sessionId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyRecordingServerAsync(new Guid(task.Target));
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Storage", recordingStorageId, EventAction.Updated, string.Empty);
		return task;
	}

	public static Task MoveArchiveStorage(IGeminiRequestContext requestContext, string archiveStorageId, string newPath)
	{
		Task task = null;
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref archiveStorageId, "archiveStorageId", allowNullOrEmpty: false);
		using (IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent())
		{
			string recordingServerId = archiveStorageComponent.GetRecordingServerId(archiveStorageId);
			if (recordingServerId == null)
			{
				throw new ArchiveStorageUpdateException();
			}
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
			if (!Integration.RecordingServer.VerifyStorageAndArchiveOnline(recordingServerId, string.Empty, string.Empty, archiveStorageId))
			{
				throw VmoExceptionFactory.VMO61110_VmoServerServiceException();
			}
			if (!Integration.RecordingServer.VerifyArchiveStoragePath(recordingServerId, archiveStorageId, newPath, newPath: true))
			{
				throw new ArchiveStorageMovePathInvalidException();
			}
		}
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			task = taskComponent.QueryTask(taskComponent.MoveArchiveStorage(archiveStorageId, newPath, sessionId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyRecordingServerAsync(new Guid(task.Target));
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "ArchiveStorage", archiveStorageId, EventAction.Updated, string.Empty);
		return task;
	}

	public static Task MountArchiveStorage(IGeminiRequestContext requestContext, string archiveStorageId)
	{
		Task task = null;
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref archiveStorageId, "archiveStorageId", allowNullOrEmpty: false);
		string recordingServerId;
		using (IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent())
		{
			recordingServerId = archiveStorageComponent.GetRecordingServerId(archiveStorageId);
			if (recordingServerId == null)
			{
				throw VmoExceptionFactory.VMO60408_VmoServerServiceException();
			}
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
		Integration.RecordingServer.VerifyStorageAndArchiveOnline(recordingServerId, string.Empty, string.Empty, archiveStorageId);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			task = taskComponent.QueryTask(taskComponent.MountArchiveStorage(archiveStorageId, sessionId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyRecordingServerAsync(new Guid(task.Target));
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return task;
	}

	public static Task UnmountArchiveStorage(IGeminiRequestContext requestContext, string archiveStorageId)
	{
		Task task = null;
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref archiveStorageId, "archiveStorageId", allowNullOrEmpty: false);
		string recordingServerId;
		using (IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent())
		{
			recordingServerId = archiveStorageComponent.GetRecordingServerId(archiveStorageId);
			if (recordingServerId == null)
			{
				throw VmoExceptionFactory.VMO60409_VmoServerServiceException();
			}
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
		Integration.RecordingServer.VerifyStorageAndArchiveOnline(recordingServerId, string.Empty, string.Empty, archiveStorageId);
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			task = taskComponent.QueryTask(taskComponent.UnmountArchiveStorage(archiveStorageId, sessionId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyRecordingServerAsync(new Guid(task.Target));
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return task;
	}

	public static Task ChangeDeviceRecordingStorage(IGeminiRequestContext requestContext, string deviceId, string targetStorageId)
	{
		Task task = null;
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref targetStorageId, "newStorageId", allowNullOrEmpty: false);
		Device device = null;
		using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
		{
			device = deviceComponent.QueryDeviceById(deviceId);
		}
		if (device != null)
		{
			Guid securityNamespace = DeviceTypeInformation.DeviceTypeDictionary[device.DeviceType].SecurityNamespace;
			if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, securityNamespace, "GENERIC_WRITE", deviceId) && !requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", device.RecordingServerId))
			{
				throw new UnauthorizedAccessException(VmoResources.VMO61008);
			}
			if (!Integration.RecordingServer.VerifyStorageAndArchiveOnline(device.RecordingServerId, deviceId, targetStorageId, string.Empty))
			{
				throw VmoExceptionFactory.VMO61109_VmoServerServiceException();
			}
			using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
			{
				task = taskComponent.QueryTask(taskComponent.ChangeDeviceRecordingStorage(deviceId, targetStorageId, sessionId).ToString(CultureInfo.InvariantCulture));
			}
			Integration.TryNotifyRecordingServerAsync(new Guid(task.Target));
			using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
			{
				vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
			}
			Integration.FireConfigurationChanged(requestContext, "Storage", targetStorageId, EventAction.Updated, string.Empty);
			return task;
		}
		throw new DeviceUpdateException();
	}

	public static Task DeleteRecordingStorage(IGeminiRequestContext requestContext, string recordingStorageId, bool deleteIfOffline)
	{
		Task task = null;
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		string recordingServerId;
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			recordingServerId = recordingStorageComponent.GetRecordingServerId(recordingStorageId);
		}
		if (recordingServerId == null)
		{
			throw new RemoveNonExistingRecordingStorageException();
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
		if (!deleteIfOffline && !Integration.RecordingServer.VerifyStorageAndArchiveOnline(recordingServerId, string.Empty, recordingStorageId, string.Empty))
		{
			throw VmoExceptionFactory.VMO61109_VmoServerServiceException();
		}
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			task = taskComponent.QueryTask(taskComponent.DeleteRecordingStorage(recordingStorageId, sessionId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyRecordingServerAsync(new Guid(task.Target));
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Storage", recordingStorageId, EventAction.Deleted, string.Empty);
		return task;
	}

	public static Task DeleteArchiveStorage(IGeminiRequestContext requestContext, string archiveStorageId, bool deleteIfOffline)
	{
		Task task = null;
		Guid sessionId = Guid.NewGuid();
		Validation.CheckObjectId(ref archiveStorageId, "archiveStorageId", allowNullOrEmpty: false);
		string recordingServerId;
		using (IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent())
		{
			recordingServerId = archiveStorageComponent.GetRecordingServerId(archiveStorageId);
		}
		if (recordingServerId == null)
		{
			throw new RemoveNonExistingArchiveStorageException();
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
		if (!deleteIfOffline && !Integration.RecordingServer.VerifyStorageAndArchiveOnline(recordingServerId, string.Empty, string.Empty, archiveStorageId))
		{
			throw VmoExceptionFactory.VMO61110_VmoServerServiceException();
		}
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			task = taskComponent.QueryTask(taskComponent.DeleteArchiveStorage(archiveStorageId, sessionId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyRecordingServerAsync(new Guid(task.Target));
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "ArchiveStorage", archiveStorageId, EventAction.Deleted, string.Empty);
		return task;
	}

	public static Task[] EnableRecordingStorageEncryption(IGeminiRequestContext requestContext, string recordingStorageId, EncryptionMethod encryptionMethod, string password)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingStorageEncryption");
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		Validation.CheckDescription(ref password, "password");
		string recordingServerId;
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			recordingServerId = recordingStorageComponent.GetRecordingServerId(recordingStorageId);
		}
		if (recordingServerId == null)
		{
			throw new RecordingStorageUpdateException();
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
		if (encryptionMethod == EncryptionMethod.None)
		{
			throw new RecordingStorageInvalidEncryptionMethodException();
		}
		VerifyStorageAreaAvailabilityEncryption(requestContext, recordingStorageId);
		Guid sessionId = Guid.NewGuid();
		List<Task> list = new List<Task>();
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			long[] array = taskComponent.EnableRecordingStorageEncryption(recordingStorageId, encryptionMethod, password, sessionId);
			foreach (long num in array)
			{
				Task item = taskComponent.QueryTask(num.ToString(CultureInfo.InvariantCulture));
				list.Add(item);
			}
			foreach (Guid item2 in list.Select((Task t) => new Guid(t.Target)).Distinct())
			{
				Integration.TryNotifyRecordingServerAsync(item2);
			}
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Storage", recordingStorageId, EventAction.Updated, string.Empty);
		return list.ToArray();
	}

	public static Task[] DisableRecordingStorageEncryption(IGeminiRequestContext requestContext, string recordingStorageId, string password)
	{
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingStorageEncryption");
		string recordingServerId;
		string text;
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			recordingServerId = recordingStorageComponent.GetRecordingServerId(recordingStorageId);
			text = recordingStorageComponent.QueryEncryptionPassword(recordingStorageId);
		}
		if (recordingServerId == null)
		{
			throw new RecordingStorageUpdateException();
		}
		if (!text.Equals(password))
		{
			throw new RecordingStorageEncryptionConfigurationIncorrectPasswordException();
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
		VerifyStorageAreaAvailabilityEncryption(requestContext, recordingStorageId);
		List<Task> list = new List<Task>();
		Guid sessionId = Guid.NewGuid();
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			long[] array = taskComponent.DisableRecordingStorageEncryption(recordingStorageId, sessionId);
			foreach (long num in array)
			{
				Task item = taskComponent.QueryTask(num.ToString(CultureInfo.InvariantCulture));
				list.Add(item);
			}
			foreach (Guid item2 in list.Select((Task t) => new Guid(t.Target)).Distinct())
			{
				Integration.TryNotifyRecordingServerAsync(item2);
			}
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Storage", recordingStorageId, EventAction.Updated, string.Empty);
		return list.ToArray();
	}

	public static Task ChangeRecordingStorageEncryptionMethod(IGeminiRequestContext requestContext, string recordingStorageId, EncryptionMethod encryptionMethod, string password)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingStorageEncryption");
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		Validation.CheckDescription(ref password, "password");
		string recordingServerId;
		string text;
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			recordingServerId = recordingStorageComponent.GetRecordingServerId(recordingStorageId);
			text = recordingStorageComponent.QueryEncryptionPassword(recordingStorageId);
		}
		if (recordingServerId == null)
		{
			throw new RecordingStorageUpdateException();
		}
		if (!text.Equals(password))
		{
			throw new RecordingStorageEncryptionConfigurationIncorrectPasswordException();
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
		if (encryptionMethod == EncryptionMethod.None)
		{
			throw new RecordingStorageInvalidEncryptionMethodException();
		}
		VerifyStorageAreaAvailabilityEncryption(requestContext, recordingStorageId);
		Guid sessionId = Guid.NewGuid();
		Task result = null;
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.ChangeRecordingStorageEncryptionMethod(recordingStorageId, encryptionMethod, sessionId).ToString(CultureInfo.InvariantCulture));
			Integration.TryNotifyRecordingServerAsync(recordingServerId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Storage", recordingStorageId, EventAction.Updated, string.Empty);
		return result;
	}

	public static Task ChangeRecordingStorageEncryptionPassword(IGeminiRequestContext requestContext, string recordingStorageId, string oldPassword, string newPassword)
	{
		GeminiApplicationCore.FeatureCacheManager.CheckFeatureLicense(requestContext, "RecordingStorageEncryption");
		Validation.CheckObjectId(ref recordingStorageId, "recordingStorageId", allowNullOrEmpty: false);
		Validation.CheckDescription(ref oldPassword, "oldPassword");
		Validation.CheckDescription(ref newPassword, "newPassword");
		string recordingServerId;
		string text;
		using (IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent())
		{
			recordingServerId = recordingStorageComponent.GetRecordingServerId(recordingStorageId);
			if (recordingServerId == null)
			{
				throw new RecordingStorageUpdateException();
			}
			text = recordingStorageComponent.QueryEncryptionPassword(recordingStorageId);
		}
		if (!text.Equals(oldPassword))
		{
			throw new RecordingStorageEncryptionConfigurationIncorrectPasswordException();
		}
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_STORAGE", recordingServerId);
		VerifyStorageAreaAvailabilityEncryption(requestContext, recordingStorageId);
		Guid sessionId = Guid.NewGuid();
		Task result = null;
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			result = taskComponent.QueryTask(taskComponent.ChangeRecordingStorageEncryptionPassword(recordingStorageId, newPassword, sessionId).ToString(CultureInfo.InvariantCulture));
			Integration.TryNotifyRecordingServerAsync(recordingServerId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		Integration.FireConfigurationChanged(requestContext, "Storage", recordingStorageId, EventAction.Updated, string.Empty);
		return result;
	}

	public static Task RequestEdgeStorageRetrieval(IGeminiRequestContext requestContext, string deviceId, DateTime startTime, DateTime endTime, string metadata)
	{
		Guid sessionId = Guid.NewGuid();
		Task task = null;
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EdgeStorage"))
		{
			throw new FeatureNotLicensedException();
		}
		Device device = Device.QueryDeviceById(requestContext, deviceId);
		if (device == null)
		{
			throw new ArgumentException(VmoResources.VMO62003, "deviceId");
		}
		if (!device.SettingUtilities.IsEdgeStorageSupported)
		{
			throw new ArgumentException(VmoResources.VMO62007, "deviceId");
		}
		if (device.DeviceType == DeviceType.Camera || device.DeviceType == DeviceType.Microphone || device.DeviceType == DeviceType.Speaker || device.DeviceType == DeviceType.Metadata)
		{
			Guid securityNamespace = DeviceTypeInformation.DeviceTypeDictionary[device.DeviceType].SecurityNamespace;
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, securityNamespace, "GENERIC_READ", deviceId);
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, securityNamespace, "READ_REMOTE_RECORDINGS", deviceId);
		}
		using (ITaskComponent taskComponent = requestContext.CreateTaskComponent())
		{
			task = taskComponent.QueryTask(taskComponent.RequestEdgeStorageRetrieval(deviceId, startTime, endTime, metadata, sessionId).ToString(CultureInfo.InvariantCulture));
		}
		Integration.TryNotifyRecordingServerAsync(new Guid(task.Target));
		using VmoLogManager vmoLogManager = new VmoLogManager(requestContext);
		vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return task;
	}

	private static void VerifyStorageAreaAvailabilityEncryption(IGeminiRequestContext requestContext, string recordingStorageId)
	{
		using IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent();
		using IArchiveStorageComponent archiveStorageComponent = requestContext.CreateArchiveStorageComponent();
		if (recordingStorageComponent.QueryRecordingStorage(recordingStorageId) == null)
		{
			throw new RecordingStorageUpdateException();
		}
		string recordingServerId = recordingStorageComponent.GetRecordingServerId(recordingStorageId);
		if (!Integration.RecordingServer.VerifyStorageAndArchiveOnline(recordingServerId, string.Empty, recordingStorageId, string.Empty))
		{
			throw new RecordingStorageEncryptionConfigurationStorageUnavailableException();
		}
		ArchiveStorageDataContract[] array = archiveStorageComponent.QueryArchiveStorages(recordingStorageId);
		foreach (ArchiveStorageDataContract archiveStorageDataContract in array)
		{
			if (!Integration.RecordingServer.VerifyStorageAndArchiveOnline(recordingServerId, string.Empty, string.Empty, archiveStorageDataContract.Id))
			{
				throw new RecordingStorageEncryptionConfigurationArchiveUnavailableException(archiveStorageDataContract.Name);
			}
		}
	}

	public static void DeleteTasksByDeviceBeforeDt(IGeminiRequestContext requestContext, string deviceId, DateTime dt)
	{
		Validation.CheckObjectId(ref deviceId, "deviceId", allowNullOrEmpty: false);
		Validation.CheckDateTime(ref dt, "time");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		using ITaskComponent taskComponent = requestContext.CreateTaskComponent();
		taskComponent.DeleteCompletedTasksByDeviceIdBeforeDt(deviceId, dt);
	}

	public static void CleanupTask(IGeminiRequestContext requestContext, string taskId)
	{
		Validation.CheckObjectIdAsInt32(ref taskId, "taskId", allowNullOrEmpty: false);
		using ITaskComponent taskComponent = requestContext.CreateTaskComponent();
		Task task = taskComponent.QueryTask(taskId);
		if (task != null)
		{
			TaskSecurityProviderFactory.GetSecurityProvider(task.Type).CanRead(requestContext, task);
			taskComponent.CleanupTask(taskId);
		}
	}

	public static Task[] FilterTasksBasedOnPermissions(IGeminiRequestContext requestContext, Task[] tasksArray)
	{
		List<Task> list = new List<Task>();
		foreach (IGrouping<TaskType, Task> item in from t in tasksArray
			group t by t.Type)
		{
			Task[] collection = TaskSecurityProviderFactory.GetSecurityProvider(item.Key).CanRead(requestContext, item.ToArray()).ToArray();
			list.AddRange(collection);
		}
		return list.ToArray();
	}
}
