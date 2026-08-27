using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Logging;
using VideoOS.Common.Integration.Properties;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Common.Integration;

public class LogEntryAuditLogger : IAuditLogger
{
	public enum LoginFrom
	{
		Administration,
		CommandApi,
		ExternalSource,
		HttpRequest,
		AnySource
	}

	private ILogAuditLogger _logManager;

	private IInfoCollector _dataAccessor;

	private readonly ILocalizedLogger _auditAndSystemLogger;

	public LogEntryAuditLogger(ILocalizedLogger auditAndSystemLogger, ILogAuditLogger logManager, IInfoCollector dataAccessor)
	{
		_auditAndSystemLogger = auditAndSystemLogger;
		_logManager = logManager;
		_dataAccessor = dataAccessor;
	}

	public void LogEntryAuditLog(LogEntryAudit log)
	{
		_logManager.AddAuditLogEntry(log);
	}

	public void AuditLoginSuccessfully(LoginFrom loginFrom, string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		string text2 = OemResources.Instance.Manager.ApplicationNameShort.ToLower(CultureInfo.InvariantCulture);
		switch (loginFrom)
		{
		case LoginFrom.Administration:
			LogEntryAuditLog(LogEntryAuditFactory.AuditLoginSuccessFromAdministration(resourceId, text, text2, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditLoginSuccessFromAdministration, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Granted, () => LogDefinitions.AuditLoginSuccessFromAdministration, text2);
			break;
		case LoginFrom.CommandApi:
			LogEntryAuditLog(LogEntryAuditFactory.AuditLoginSuccessFromCommandApi(resourceId, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditLoginSuccessFromCommandApi, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Granted, () => LogDefinitions.AuditLoginSuccessFromCommandApi);
			break;
		case LoginFrom.ExternalSource:
			LogEntryAuditLog(LogEntryAuditFactory.AuditLoginSuccessFromExternalSource(resourceId, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditLoginSuccessFromExternalSource, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Granted, () => LogDefinitions.AuditLoginSuccessFromExternalSource);
			break;
		case LoginFrom.HttpRequest:
			LogEntryAuditLog(LogEntryAuditFactory.AuditLoginSuccessFromHttpRequest(resourceId, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditLoginSuccessFromHttpRequest, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Granted, () => LogDefinitions.AuditLoginSuccessFromHttpRequest);
			break;
		case LoginFrom.AnySource:
			LogEntryAuditLog(LogEntryAuditFactory.AuditLoginSuccess(resourceId, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditLoginSuccess, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Granted, () => LogDefinitions.AuditLoginSuccess);
			break;
		default:
			throw new ArgumentException("The loginFrom parameter is not set to a valid value.", "loginFrom");
		}
	}

	public void AuditLoginUnsuccessfullyInsufficientRights(LoginFrom loginFrom, string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		switch (loginFrom)
		{
		case LoginFrom.Administration:
			LogEntryAuditLog(LogEntryAuditFactory.AuditLoginUserNotAdminPriviligesOnTheSystem(resourceId, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditLoginUserNotAdminPriviligesOnTheSystem, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Denied, () => LogDefinitions.AuditLoginUserNotAdminPriviligesOnTheSystem);
			break;
		case LoginFrom.CommandApi:
			LogEntryAuditLog(LogEntryAuditFactory.AuditLoginUserNotAllowedCommandApiAccess(resourceId, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditLoginUserNotAllowedCommandApiAccess, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Denied, () => LogDefinitions.AuditLoginUserNotAllowedCommandApiAccess);
			break;
		case LoginFrom.ExternalSource:
			LogEntryAuditLog(LogEntryAuditFactory.AuditLoginUserNotAllowedExternalAccess(resourceId, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditLoginUserNotAllowedExternalAccess, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Denied, () => LogDefinitions.AuditLoginUserNotAllowedExternalAccess);
			break;
		case LoginFrom.HttpRequest:
			LogEntryAuditLog(LogEntryAuditFactory.AuditLoginUserNotAllowedHttpRequestAccess(resourceId, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditLoginUserNotAllowedHttpRequestAccess, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Denied, () => LogDefinitions.AuditLoginUserNotAllowedHttpRequestAccess);
			break;
		case LoginFrom.AnySource:
			LogEntryAuditLog(LogEntryAuditFactory.AuditLoginUserNotAllowed(resourceId, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditLoginUserNotAllowed, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Denied, () => LogDefinitions.AuditLoginUserNotAllowed);
			break;
		default:
			throw new ArgumentException("The loginFrom parameter is not set to a valid value.", "loginFrom");
		}
	}

	public void AuditLoginUnSuccessfulUnknownUser(string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditLoginUnSuccessfulUnknownUser(resourceId, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditLoginUnSuccessfulUnknownUser, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Denied, () => LogDefinitions.AuditLoginUnSuccessfulUnknownUser);
	}

	public void AuditLoginUnSuccessfulIncorrectPassword(string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditLoginUnSuccessfulIncorrectPassword(resourceId, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditLoginUnSuccessfulIncorrectPassword, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Denied, () => LogDefinitions.AuditLoginUnSuccessfulIncorrectPassword);
	}

	public void AuditLogout(string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditLogout(resourceId, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditLogout, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Granted, () => LogDefinitions.AuditLogout);
	}

	public void AuditLogoutManagementClient(string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		string text2 = OemResources.Instance.Manager.ApplicationNameShort.ToLower(CultureInfo.InvariantCulture);
		LogEntryAuditLog(LogEntryAuditFactory.AuditLogoutSuccessFromAdministration(resourceId, text, text2, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditLogoutSuccessFromAdministration, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Granted, () => LogDefinitions.AuditLogoutSuccessFromAdministration, text2);
	}

	public void AuditLoginLogoutTimeout(string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditLoginLogoutTimeout(resourceId, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditLoginLogoutTimeout, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Granted, () => LogDefinitions.AuditLoginLogoutTimeout);
	}

	public void AuditLoginTimeBased(string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditLoginTimeBased(resourceId, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditLoginTimeBased, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Denied, () => LogDefinitions.AuditLoginTimeBased);
	}

	public void AuditDualAuthorizationUserAuthorized(string userCredentials, string authorizedUser, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditAuthorizationSucessful(resourceId, text, authorizedUser, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditAuthorizationSucessful, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Granted, () => LogDefinitions.AuditAuthorizationSucessful, authorizedUser);
	}

	public void AuditDualAuthorizationUserNotAllowedToAuthorize(string userCredentials, string authorizedUser, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditAuthorizationFailedInsufficientRights(resourceId, text, authorizedUser, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditAuthorizationFailedInsufficientRights, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Denied, () => LogDefinitions.AuditAuthorizationFailedInsufficientRights, authorizedUser);
	}

	public void AuditDualAuthorizationSelfAuthorization(string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditAuthorizationFailedSelfAuthorization(resourceId, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditAuthorizationFailedSelfAuthorization, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Denied, () => LogDefinitions.AuditAuthorizationFailedSelfAuthorization);
	}

	public void AuditHardwareAdded(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwareAdded(hardwareId, hardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwareAdded, hardwareId, hardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHardwareAdded, hardwareName);
	}

	public void AuditHardwareRemoved(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwareRemoved(hardwareId, hardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwareRemoved, hardwareId, hardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHardwareRemoved, hardwareName);
	}

	public void AuditHardwareEnabled(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwareEnabled(hardwareId, hardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwareEnabled, hardwareId, hardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHardwareEnabled, hardwareName);
	}

	public void AuditHardwareDisabled(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwareDisabled(hardwareId, hardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwareDisabled, hardwareId, hardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHardwareDisabled, hardwareName);
	}

	public void AuditHardwareRenamed(Guid hardwareId, string newHardwareName, string oldHardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwareRenamed(hardwareId, oldHardwareName, newHardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwareRenamed, hardwareId, newHardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHardwareRenamed, oldHardwareName, newHardwareName);
	}

	public void AuditHardwareMoved(Guid hardwareId, string hardwareName, string fromRecordingServerName, string toRecordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwareMoved(hardwareId, hardwareName, fromRecordingServerName, toRecordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwareMoved, hardwareId, hardwareName, fromRecordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHardwareMoved, hardwareName, toRecordingServerName);
	}

	public void AuditHardwarePasswordChanged(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwarePasswordChanged(hardwareId, hardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwarePasswordChanged, hardwareId, hardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHardwarePasswordChanged, hardwareName, recordingServerName);
	}

	public void AuditHardwareReplaced(Guid hardwareId, string newHardwareName, string oldHardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwareReplaced(hardwareId, oldHardwareName, newHardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwareReplaced, hardwareId, newHardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHardwareReplaced, oldHardwareName, newHardwareName);
	}

	public void AuditHardwareSettingsChanged(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwareSettingsChanged(hardwareId, hardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwareSettingsChanged, hardwareId, hardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHardwareSettingsChanged, hardwareName);
	}

	public void AuditHardwarePasswordRead(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwarePasswordRead(hardwareId, hardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwarePasswordRead, hardwareId, hardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Granted, () => LogDefinitions.AuditHardwarePasswordRead, hardwareName);
	}

	public void AuditHardwarePasswordReadFailedInsufficientRights(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwarePasswordReadFailedInsufficientRights(hardwareId, hardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwarePasswordReadFailedInsufficientRights, hardwareId, hardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Security, AccessGranted.Denied, () => LogDefinitions.AuditHardwarePasswordReadFailedInsufficientRights, hardwareName);
	}

	public void AuditHardwarePreConfigurationStarted(string hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwarePreConfigurationStarted(hardwareId, hardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwarePreConfigurationStarted, hardwareId, hardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHardwarePreConfigurationStarted, hardwareName, recordingServerName);
	}

	public void AuditHardwareFirmwareUpdateStarted(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditHardwareFirmwareUpdateStarted(hardwareId.ToString(), hardwareName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHardwareFirmwareUpdateStarted, hardwareId, hardwareName, recordingServerName, ResourceType.Hardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHardwareFirmwareUpdateStarted, hardwareName, recordingServerName);
	}

	public void AuditFileUploadStarted(string targetFileName, string size, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditFileUploadStarted(targetFileName, size, OemResources.Instance.Manager.ApplicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditFileUploadStarted, OemResources.Instance.Manager.ApplicationNameShort, OemResources.Instance.Manager.ApplicationNameShort, OemResources.Instance.Manager.ApplicationNameShort, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditFileUploadStarted, targetFileName, size);
	}

	public void AuditDeviceAdded(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceAdded(deviceID, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceAdded, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceAdded, deviceName);
	}

	public void AuditDeviceRemoved(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRemoved(deviceID, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRemoved, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRemoved, deviceName);
	}

	public void AuditDeviceEnabled(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceEnabled(deviceID, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceEnabled, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceEnabled, deviceName);
	}

	public void AuditDeviceDisabled(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceDisabled(deviceID, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceDisabled, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceDisabled, deviceName);
	}

	public void AuditDeviceRenamed(Guid deviceID, string oldDeviceName, string newDeviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRenamed(deviceID, oldDeviceName, newDeviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRenamed, deviceID, oldDeviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRenamed, oldDeviceName, newDeviceName);
	}

	public void AuditDeviceGisPointChanged(Guid deviceID, string deviceName, string gisPoint, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceGisPointChanged(deviceID, deviceName, gisPoint, LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType), recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceGisPointChanged, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceGisPointChanged, deviceName, gisPoint);
	}

	public void AuditDeviceShortNameChanged(Guid deviceID, string deviceName, string shortName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceShortNameChanged(deviceID, deviceName, shortName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceShortNameChanged, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceShortNameChanged, deviceName, shortName);
	}

	public void AuditDeviceShortcutChanged(Guid deviceID, string deviceName, string shortName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceShortcutChanged(deviceID, deviceName, shortName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceShortcutChanged, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceShortcutChanged, deviceName, shortName);
	}

	public void AuditDeviceIconChanged(Guid deviceID, string deviceName, string icon, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceIconChanged(deviceID, deviceName, icon, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceIconChanged, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceIconChanged, deviceName, icon);
	}

	public void AuditDeviceCoverageDirectionChanged(Guid deviceID, string deviceName, string coverageDirection, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceCoverageDirectionChanged(deviceID, deviceName, coverageDirection, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceCoverageDirectionChanged, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceCoverageDirectionChanged, deviceName, coverageDirection);
	}

	public void AuditDeviceCoverageDepthChanged(Guid deviceID, string deviceName, string coverageDepth, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceCoverageDepthChanged(deviceID, deviceName, coverageDepth, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceCoverageDepthChanged, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceCoverageDepthChanged, deviceName, coverageDepth);
	}

	public void AuditDeviceCoverageFieldOfViewChanged(Guid deviceID, string deviceName, string coverageFieldOfView, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceCoverageFieldOfViewChanged(deviceID, deviceName, coverageFieldOfView, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceCoverageFieldOfViewChanged, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceCoverageFieldOfViewChanged, deviceName, coverageFieldOfView);
	}

	public void AuditDeviceFailoverSettingChanged(Guid deviceID, string deviceName, string failoverSetting, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceFailoverSettingChanged(deviceID, deviceName, failoverSetting, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceFailoverSettingChanged, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceFailoverSettingChanged, deviceName, failoverSetting);
	}

	public void AuditDeviceReplaced(Guid deviceID, string oldDeviceName, string newDeviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		if (RegisteredDeviceType.IsRecordingDeviceType(deviceType))
		{
			LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceReplaced(deviceID, oldDeviceName, newDeviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditDeviceReplaced, deviceID, oldDeviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceReplaced, oldDeviceName, newDeviceName);
		}
		else
		{
			LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceReplacedNoDatabase(deviceID, oldDeviceName, newDeviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditDeviceReplacedNoDatabase, deviceID, oldDeviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceReplacedNoDatabase, oldDeviceName, newDeviceName);
		}
	}

	public void AuditDeviceSettingsChanged(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceSettingsChanged(deviceID, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceSettingsChanged, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceSettingsChanged, deviceName);
	}

	public void AuditDeviceRecordingSettingsChanged(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRecordSettingsChanged(deviceID, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRecordSettingsChanged, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRecordSettingsChanged, deviceName);
	}

	public void AuditDeviceRecordingStorageSelected(Guid deviceId, string deviceName, string deviceType, string storageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRecordingStorageSelected(deviceId, deviceName, resourceType, storageName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRecordingStorageSelected, deviceId, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRecordingStorageSelected, storageName, deviceName);
	}

	public void AuditDeviceRecordingEnabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRecordingEnabled(deviceId, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRecordingEnabled, deviceId, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRecordingEnabled, deviceName);
	}

	public void AuditDeviceRecordingDisabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRecordingDisabled(deviceId, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRecordingDisabled, deviceId, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRecordingDisabled, deviceName);
	}

	public void AuditDeviceRecordingPrebufferEnabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRecordingPrebufferEnabled(deviceId, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRecordingPrebufferEnabled, deviceId, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRecordingPrebufferEnabled, deviceName);
	}

	public void AuditDeviceRecordingPrebufferDisabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRecordingPrebufferDisabled(deviceId, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRecordingPrebufferDisabled, deviceId, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRecordingPrebufferDisabled, deviceName);
	}

	public void AuditDeviceRecordingEdgeStorageEnabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRecordingEdgeStorageEnabled(deviceId, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRecordingEdgeStorageEnabled, deviceId, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRecordingEdgeStorageEnabled, deviceName);
	}

	public void AuditDeviceRecordingEdgeStorageDisabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRecordingEdgeStorageDisabled(deviceId, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRecordingEdgeStorageDisabled, deviceId, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRecordingEdgeStorageDisabled, deviceName);
	}

	public void AuditDeviceRecordingEdgeStoragePlaybackEnabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRecordingEdgeStoragePlaybackEnabled(deviceId, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRecordingEdgeStoragePlaybackEnabled, deviceId, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRecordingEdgeStoragePlaybackEnabled, deviceName);
	}

	public void AuditDeviceRecordingEdgeStoragePlaybackDisabled(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRecordingEdgeStoragePlaybackDisabled(deviceId, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRecordingEdgeStoragePlaybackDisabled, deviceId, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRecordingEdgeStoragePlaybackDisabled, deviceName);
	}

	public void AuditDeviceRequestEdgeStorageRetrieval(Guid deviceId, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceRequestEdgeStorageRetrieval(deviceId, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceRequestEdgeStorageRetrieval, deviceId, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceRequestEdgeStorageRetrieval, deviceName);
	}

	public void AuditDeviceStreamSettingsChanged(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceStreamSettingsChanged(deviceID, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceStreamSettingsChanged, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceStreamSettingsChanged, deviceName);
	}

	public void AuditDeviceMotionDetectionSettingsChanged(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		if (deviceType != "Camera")
		{
			throw new ArgumentException("The deviceType argument must be 'Camera'.", "deviceType");
		}
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceMotionDetectionSettingsChanged(deviceID, deviceName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceMotionDetectionSettingsChanged, deviceID, deviceName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceMotionDetectionSettingsChanged, deviceName);
	}

	public void AuditDeviceMulticastEnabledChanged(Guid deviceID, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress, string multicastEnabled)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceMulticastEnabledChanged(deviceID, deviceName, multicastEnabled, ResourceType.Camera, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceMulticastEnabledChanged, deviceID, deviceName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceMulticastEnabledChanged, deviceName, multicastEnabled);
	}

	public void AuditDevicePtzManuelTimeoutChanged(Guid deviceID, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress, string timeout)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditDevicePtzManuelSessionTimeoutChanged(deviceID, deviceName, timeout, ResourceType.Camera, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDevicePtzManuelSessionTimeoutChanged, deviceID, deviceName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDevicePtzManuelSessionTimeoutChanged, deviceName, timeout);
	}

	public void AuditDevicePtzExtendedManuelTimeoutChanged(Guid deviceID, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress, string timeout)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditDevicePtzExtendedManualSessionTimeoutChanged(deviceID, deviceName, timeout, ResourceType.Camera, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDevicePtzExtendedManualSessionTimeoutChanged, deviceID, deviceName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDevicePtzExtendedManualSessionTimeoutChanged, deviceName, timeout);
	}

	public void AuditDevicePtzReservedSessionTimeoutChanged(Guid deviceID, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress, string timeout)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditDevicePtzReservedSessionTimeoutChanged(deviceID, deviceName, timeout, ResourceType.Camera, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDevicePtzReservedSessionTimeoutChanged, deviceID, deviceName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDevicePtzReservedSessionTimeoutChanged, deviceName, timeout);
	}

	public void AuditDeviceDatabaseDelete(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceDatabaseDelete(deviceID, deviceName, resourceType, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceDatabaseDelete, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceDatabaseDelete, deviceName, recordingServerName);
	}

	public void AuditDeviceDatabaseMove(Guid deviceID, string deviceName, string deviceType, string databaseBankName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditDeviceDatabaseMove(deviceID, deviceName, resourceType, recordingServerName, databaseBankName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDeviceDatabaseMove, deviceID, deviceName, recordingServerName, resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDeviceDatabaseMove, deviceName, databaseBankName);
	}

	public void AuditDevice360LensSettingsEnabled(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		if (deviceType != "Camera")
		{
			throw new ArgumentException("Wrong device type specified", "deviceType");
		}
		LogEntryAuditLog(LogEntryAuditFactory.AuditDevice360LensSettingsEnabled(deviceID, deviceName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDevice360LensSettingsEnabled, deviceID, deviceName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDevice360LensSettingsEnabled);
	}

	public void AuditDevice360LensSettingsChanged(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		if (deviceType != "Camera")
		{
			throw new ArgumentException("Wrong device type specified", "deviceType");
		}
		LogEntryAuditLog(LogEntryAuditFactory.AuditDevice360LensSettingsChanged(deviceID, deviceName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDevice360LensSettingsChanged, deviceID, deviceName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDevice360LensSettingsChanged, deviceName);
	}

	public void AuditDevice360LensSettingsDisabled(Guid deviceID, string deviceName, string deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		if (deviceType != "Camera")
		{
			throw new ArgumentException("Wrong device type specified", "deviceType");
		}
		LogEntryAuditLog(LogEntryAuditFactory.AuditDevice360LensSettingsDisabled(deviceID, deviceName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDevice360LensSettingsDisabled, deviceID, deviceName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDevice360LensSettingsDisabled, deviceName);
	}

	public void AuditDevicePrivacyMaskSettingsChanged(Guid deviceID, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditDevicePrivacyMaskSettingsChanged(deviceID, deviceName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDevicePrivacyMaskSettingsChanged, deviceID, deviceName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDevicePrivacyMaskSettingsChanged, deviceName);
	}

	public void AuditRuleCreated(Guid ruleID, string ruleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRuleCreated(ruleID, ruleName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRuleCreated, ruleID, ruleName, text, ResourceType.Rules, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRuleCreated, ruleName);
	}

	public void AuditRuleDelete(Guid ruleID, string ruleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRuleDeleted(ruleID, ruleName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRuleDeleted, ruleID, ruleName, text, ResourceType.Rules, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRuleDeleted, ruleName);
	}

	public void AuditRuleEnabled(Guid ruleID, string ruleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRuleEnabled(ruleID, ruleName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRuleEnabled, ruleID, ruleName, text, ResourceType.Rules, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRuleEnabled, ruleName);
	}

	public void AuditRuleDisabled(Guid ruleID, string ruleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRuleDisabled(ruleID, ruleName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRuleDisabled, ruleID, ruleName, text, ResourceType.Rules, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRuleDisabled, ruleName);
	}

	public void AuditRuleRenamed(string oldruleName, string newruleName, Guid ruleID, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRuleRenamed(ruleID, oldruleName, newruleName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRuleRenamed, ruleID, oldruleName, text, ResourceType.Rules, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRuleRenamed, oldruleName, newruleName);
	}

	public void AuditRuleEdited(Guid ruleID, string ruleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRuleEdited(ruleID, ruleName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRuleEdited, ruleID, ruleName, text, ResourceType.Rules, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRuleEdited, ruleName);
	}

	public void AuditTimeProfileCreated(Guid timeProfileID, string timeProfileName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditTimeProfileCreated(timeProfileID, timeProfileName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditTimeProfileCreated, timeProfileID, timeProfileName, text, ResourceType.TimeProfiles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditTimeProfileCreated, timeProfileName);
	}

	public void AuditTimeProfileEdited(Guid timeProfileID, string timeProfileName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditTimeProfileEdited(timeProfileID, timeProfileName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditTimeProfileEdited, timeProfileID, timeProfileName, text, ResourceType.TimeProfiles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditTimeProfileEdited, timeProfileName);
	}

	public void AuditTimeProfileDelete(Guid timeProfileID, string timeProfileName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditTimeProfileDeleted(timeProfileID, timeProfileName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditTimeProfileDeleted, timeProfileID, timeProfileName, text, ResourceType.TimeProfiles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditTimeProfileDeleted, timeProfileName);
	}

	public void AuditTimeProfileRenamed(string oldtimeProfileName, string newtimeProfileName, Guid timeProfileID, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditTimeProfileRenamed(timeProfileID, oldtimeProfileName, newtimeProfileName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditTimeProfileRenamed, timeProfileID, oldtimeProfileName, text, ResourceType.TimeProfiles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditTimeProfileRenamed, oldtimeProfileName, newtimeProfileName);
	}

	public void AuditLoginProviderCreated(Guid loginProviderId, string loginProviderName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditLoginProviderCreated(loginProviderId, loginProviderName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditLoginProviderCreated, loginProviderId, loginProviderName, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditLoginProviderCreated, loginProviderName);
	}

	public void AuditLoginProviderUpdated(Guid loginProviderId, string loginProviderName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditLoginProviderUpdated(loginProviderId, loginProviderName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditLoginProviderUpdated, loginProviderId, loginProviderName, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditLoginProviderUpdated, loginProviderName);
	}

	public void AuditLoginProviderDeleted(Guid loginProviderId, string loginProviderName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditLoginProviderDeleted(loginProviderId, loginProviderName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditLoginProviderDeleted, loginProviderId, loginProviderName, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditLoginProviderDeleted, loginProviderName);
	}

	public void AuditClaimCreated(int claimId, string claimName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditClaimCreated(claimId, claimName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditClaimCreated, claimId.ToString(CultureInfo.InvariantCulture), claimName, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditClaimCreated, claimName);
	}

	public void AuditClaimUpdated(int claimId, string claimName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditClaimUpdated(claimId, claimName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditClaimUpdated, claimId.ToString(CultureInfo.InvariantCulture), claimName, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditClaimUpdated, claimName);
	}

	public void AuditClaimDeleted(int claimId, string claimName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditClaimDeleted(claimId, claimName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditClaimDeleted, claimId.ToString(CultureInfo.InvariantCulture), claimName, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditClaimDeleted, claimName);
	}

	public void AuditClientRedirectUrisSet(string clientId, IEnumerable<string> uris, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		string text2 = string.Join(", ", uris);
		LogEntryAuditLog(LogEntryAuditFactory.AuditClientRedirectUrisSet(clientId, text2, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditClientRedirectUrisSet, clientId, text2, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditClientRedirectUrisSet, string.Join(", ", uris));
	}

	public void AuditTrustedIssuerCreated(int id, string issuer, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditTrustedIssuerCreated(id, issuer, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditTrustedIssuerCreated, id.ToString(CultureInfo.InvariantCulture), issuer, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditTrustedIssuerCreated, issuer);
	}

	public void AuditTrustedIssuerUpdated(int id, string issuer, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditTrustedIssuerUpdated(id, issuer, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditTrustedIssuerUpdated, id.ToString(CultureInfo.InvariantCulture), issuer, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditTrustedIssuerUpdated, issuer);
	}

	public void AuditTrustedIssuerDeleted(int id, string issuer, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditTrustedIssuerDeleted(id, issuer, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditTrustedIssuerDeleted, id.ToString(CultureInfo.InvariantCulture), issuer, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditTrustedIssuerDeleted, issuer);
	}

	public void AuditRoleCreated(Guid roleID, string roleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleCreated(roleID, roleName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleCreated, roleID, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleCreated, roleName);
	}

	public void AuditRoleEdited(Guid roleID, string roleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleEdited(roleID, roleName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleEdited, roleID, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleEdited, roleName);
	}

	public void AuditRoleClientProfileChanged(string roleId, string roleName, string clientName, string oldClientProfileName, string newClientProfileName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleClientProfileChanged(text, clientName, oldClientProfileName, newClientProfileName, roleName, roleId, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleClientProfileChanged, roleId, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleClientProfileChanged, clientName, oldClientProfileName, newClientProfileName, roleName);
	}

	public void AuditRoleDefaultTimeProfileChanged(string roleId, string roleName, string oldTimeProfileName, string newTimeProfileName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleDefaultTimeProfileChanged(text, oldTimeProfileName, newTimeProfileName, roleName, roleId, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleDefaultTimeProfileChanged, roleId, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleDefaultTimeProfileChanged, oldTimeProfileName, newTimeProfileName, roleName);
	}

	public void AuditRoleOnlyAllowLogOnWithinTimeProfileChanged(string roleId, string roleName, string oldTimeProfileName, string newTimeProfileName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleOnlyAllowLoginWithinTimeProfileChanged(text, oldTimeProfileName, newTimeProfileName, roleName, roleId, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleOnlyAllowLoginWithinTimeProfileChanged, roleId, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleOnlyAllowLoginWithinTimeProfileChanged, oldTimeProfileName, newTimeProfileName, roleName);
	}

	public void AuditRoleClientLogOnGranted(string roleId, string roleName, string clientName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleClientLoginGranted(text, clientName, roleName, roleId, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleClientLoginGranted, roleId, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleClientLoginGranted, clientName, roleName);
	}

	public void AuditRoleClientLogOnRevoked(string roleId, string roleName, string clientName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleClientLoginRevoked(text, clientName, roleName, roleId, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleClientLoginRevoked, roleId, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleClientLoginRevoked, clientName, roleName);
	}

	public void AuditRoleAnonymityDuringPtzSessionsGranted(string roleId, string roleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleAnonymityDuringPtzSessionsGranted(text, roleName, roleId, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleAnonymityDuringPtzSessionsGranted, roleId, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleAnonymityDuringPtzSessionsGranted, roleName);
	}

	public void AuditRoleAnonymityDuringPtzSessionsRevoked(string roleId, string roleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleAnonymityDuringPtzSessionsRevoked(text, roleName, roleId, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleAnonymityDuringPtzSessionsRevoked, roleId, text, roleName, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleAnonymityDuringPtzSessionsRevoked, roleName);
	}

	public void AuditRoleLogOnAuthorizationRequiredGranted(string roleId, string roleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleLoginAuthorizationRequiredGranted(text, roleName, roleId, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleLoginAuthorizationRequiredGranted, roleId, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleLoginAuthorizationRequiredGranted, roleName);
	}

	public void AuditRoleLogOnAuthorizationRequiredRevoked(string roleId, string roleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleLoginAuthorizationRequiredRevoked(text, roleName, roleId, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleLoginAuthorizationRequiredRevoked, roleId, text, roleName, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleLoginAuthorizationRequiredRevoked, roleName);
	}

	public void AuditRoleOverallSecurityChanged(Guid roleID, string roleName, string accessRight, string securityGroup, string accessRightStatus, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleOverallSecurityChanged(roleID, roleName, text, accessRight, securityGroup, accessRightStatus, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleOverallSecurityChanged, roleID, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleOverallSecurityChanged, accessRight, securityGroup, roleName, accessRightStatus);
	}

	public void AuditRoleDelete(Guid roleID, string roleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleDeleted(roleID, roleName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleDeleted, roleID, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleDeleted, roleName);
	}

	public void AuditRoleRenamed(string oldroleName, string newroleName, Guid roleID, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleRenamed(roleID, oldroleName, newroleName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleRenamed, roleID, oldroleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleRenamed, oldroleName, newroleName);
	}

	public void AuditRoleUserAdded(string userName, Guid roleID, string roleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleUserAdded(roleID, roleName, userName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleUserAdded, roleID, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleUserAdded, userName, roleName);
	}

	public void AuditRoleUserRemoved(string userName, Guid roleID, string roleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleUserRemoved(roleID, roleName, userName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleUserRemoved, roleID, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleUserRemoved, userName, roleName);
	}

	public void AuditRoleGroupAdded(string userGroupName, Guid roleID, string roleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleGroupAdded(roleID, roleName, userGroupName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleGroupAdded, roleID, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleGroupAdded, userGroupName, roleName);
	}

	public void AuditRoleGroupRemoved(string userGroupName, Guid roleID, string roleName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleGroupRemoved(roleID, roleName, userGroupName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleGroupRemoved, roleID, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleGroupRemoved, userGroupName, roleName);
	}

	public void AuditRoleClaimAdded(Guid roleId, string roleName, string claimProvider, string claimName, string claimValue, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleClaimAdded(roleId, roleName, claimProvider, claimName, claimValue, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleClaimAdded, roleId, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleClaimAdded, roleName, claimProvider, claimName, claimValue);
	}

	public void AuditRoleClaimRemoved(Guid roleId, string roleName, string claimProvider, string claimName, string claimValue, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleClaimRemoved(roleId, roleName, claimProvider, claimName, claimValue, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleClaimRemoved, roleId, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleClaimRemoved, roleName, claimProvider, claimName, claimValue);
	}

	public void AuditRoleOverallSecurityGranted(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleOverallSecurityGranted(roleID, roleName, text, actionName, namespaceName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleOverallSecurityGranted, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleOverallSecurityGranted, actionName, namespaceName);
	}

	public void AuditRoleOverallSecurityRevoked(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleOverallSecurityRevoked(roleID, roleName, text, actionName, namespaceName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleOverallSecurityRevoked, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleOverallSecurityRevoked, actionName, namespaceName);
	}

	public void AuditRoleOverallSecurityDenied(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleOverallSecurityDenied(roleID, roleName, text, actionName, namespaceName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleOverallSecurityDenied, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleOverallSecurityDenied, actionName, namespaceName);
	}

	public void AuditRoleOverallSecurityRevokedDenied(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleOverallSecurityRevokedDenied(roleID, roleName, text, actionName, namespaceName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleOverallSecurityRevokedDenied, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleOverallSecurityRevokedDenied, actionName, namespaceName);
	}

	public void AuditRoleObjectSecurityGranted(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string objectName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityGranted(roleID, roleName, text, actionName, namespaceName, objectName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityGranted, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityGranted, actionName, namespaceName, objectName);
	}

	public void AuditRoleObjectSecurityRevoked(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string objectName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityRevoked(roleID, roleName, text, actionName, namespaceName, objectName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRevoked, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRevoked, actionName, namespaceName, objectName);
	}

	public void AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string deviceName, int existingRestrictionDays, int newRestrictionDays)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays(roleID, roleName, text, actionName, namespaceName, deviceName, existingRestrictionDays, newRestrictionDays, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays, actionName, namespaceName, existingRestrictionDays.ToString(CultureInfo.InvariantCulture), newRestrictionDays.ToString(CultureInfo.InvariantCulture), deviceName);
	}

	public void AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string deviceName, int existingRestrictionDays, int newRestrictionMinutes)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes(roleID, roleName, text, actionName, namespaceName, deviceName, existingRestrictionDays, newRestrictionMinutes, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes, actionName, namespaceName, existingRestrictionDays.ToString(CultureInfo.InvariantCulture), newRestrictionMinutes.ToString(CultureInfo.InvariantCulture), deviceName);
	}

	public void AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string deviceName, int existingRestrictionMinutes, int newRestrictionDays)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays(roleID, roleName, text, actionName, namespaceName, deviceName, existingRestrictionMinutes, newRestrictionDays, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays, actionName, namespaceName, existingRestrictionMinutes.ToString(CultureInfo.InvariantCulture), newRestrictionDays.ToString(CultureInfo.InvariantCulture), deviceName);
	}

	public void AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string deviceName, int existingRestrictionMinutes, int newRestrictionMinutes)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes(roleID, roleName, text, actionName, namespaceName, deviceName, existingRestrictionMinutes, newRestrictionMinutes, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes, actionName, namespaceName, existingRestrictionMinutes.ToString(CultureInfo.InvariantCulture), newRestrictionMinutes.ToString(CultureInfo.InvariantCulture), deviceName);
	}

	public void AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string deviceName, int existingRestrictionDays)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction(roleID, roleName, text, actionName, namespaceName, deviceName, existingRestrictionDays, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction, actionName, namespaceName, existingRestrictionDays.ToString(CultureInfo.InvariantCulture), deviceName);
	}

	public void AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string deviceName, int existingRestrictionMinutes)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction(roleID, roleName, text, actionName, namespaceName, deviceName, existingRestrictionMinutes, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction, actionName, namespaceName, existingRestrictionMinutes.ToString(CultureInfo.InvariantCulture), deviceName);
	}

	public void AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string deviceName, int newRestrictionDays)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays(roleID, roleName, text, actionName, namespaceName, deviceName, newRestrictionDays, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays, actionName, namespaceName, newRestrictionDays.ToString(CultureInfo.InvariantCulture), deviceName);
	}

	public void AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string deviceName, int newRestrictionMinutes)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes(roleID, roleName, text, actionName, namespaceName, deviceName, newRestrictionMinutes, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes, actionName, namespaceName, newRestrictionMinutes.ToString(CultureInfo.InvariantCulture), deviceName);
	}

	public void AuditRoleObjectSecurityPriorityGranted(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string deviceName, string defaultPriority)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityPriorityGranted(roleID, roleName, text, actionName, namespaceName, deviceName, defaultPriority, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityGranted, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityGranted, actionName, namespaceName, deviceName, defaultPriority);
	}

	public void AuditRoleObjectSecurityPriorityRevoked(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string deviceName, string defaultPriority)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityPriorityRevoked(roleID, roleName, text, actionName, namespaceName, deviceName, defaultPriority, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityRevoked, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityRevoked, actionName, namespaceName, deviceName, defaultPriority);
	}

	public void AuditRoleObjectSecurityPriorityChanged(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string deviceName, string existingPriority, string newPriority)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityPriorityChanged(roleID, roleName, text, actionName, namespaceName, existingPriority, newPriority, deviceName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityChanged, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityChanged, actionName, namespaceName, existingPriority, newPriority, deviceName);
	}

	public void AuditRoleObjectSecurityTimeProfileToProfileChange(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string objectName, string existingTimeProfileName, string newTimeProfileName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityTimeProfileToProfileChange(roleID, roleName, text, actionName, namespaceName, objectName, existingTimeProfileName, newTimeProfileName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityTimeProfileToProfileChange, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityTimeProfileToProfileChange, actionName, namespaceName, existingTimeProfileName, newTimeProfileName, objectName);
	}

	public void AuditRoleObjectSecurityTimeProfileToSystemProfileChange(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string objectName, string existingTimeProfileName, string newSystemTimeProfileName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityTimeProfileToSystemProfileChange(roleID, roleName, text, actionName, namespaceName, objectName, existingTimeProfileName, newSystemTimeProfileName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityTimeProfileToSystemProfileChange, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityTimeProfileToSystemProfileChange, actionName, namespaceName, existingTimeProfileName, newSystemTimeProfileName, objectName);
	}

	public void AuditRoleObjectSecuritySystemTimeProfileToProfileChange(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string objectName, string existingSystemTimeProfileName, string newTimeProfileName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecuritySystemTimeProfileToProfileChange(roleID, roleName, text, actionName, namespaceName, objectName, existingSystemTimeProfileName, newTimeProfileName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToProfileChange, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecuritySystemTimeProfileToProfileChange, actionName, namespaceName, existingSystemTimeProfileName, newTimeProfileName, objectName);
	}

	public void AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string actionName, string objectName, string existingSystemTimeProfileName, string newSystemTimeProfileName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange(roleID, roleName, text, actionName, namespaceName, objectName, existingSystemTimeProfileName, newSystemTimeProfileName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange, roleID, roleName, text, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange, actionName, namespaceName, existingSystemTimeProfileName, newSystemTimeProfileName, objectName);
	}

	public void AuditRoleOverallSecurityChangeAccessDenied(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleOverallSecurityChangeAccessDenied(roleID, roleName, text, namespaceName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleOverallSecurityChangeAccessDenied, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleOverallSecurityChangeAccessDenied, namespaceName);
	}

	public void AuditRoleObjectSecurityChangeAccessDenied(Guid roleID, string roleName, string userCredentials, string locationIPAddress, string namespaceName, string objectName)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRoleObjectSecurityChangeAccessDenied(roleID, roleName, text, namespaceName, objectName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityChangeAccessDenied, roleID, roleName, text, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Denied, () => LogDefinitions.AuditRoleObjectSecurityChangeAccessDenied, namespaceName, objectName);
	}

	public void AuditRecordingServerCreate(Guid recorderID, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditCreateRecordingServer(recorderID, recordingServerName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditCreateRecordingServer, recorderID, recordingServerName, text, ResourceType.RecordingServers, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditCreateRecordingServer, recordingServerName);
	}

	public void AuditRecordingServerAuthorize(Guid recorderID, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditAuthorizeRecorder(recorderID, recordingServerName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditAuthorizeRecorder, recorderID, recordingServerName, text, ResourceType.RecordingServers, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditAuthorizeRecorder, recordingServerName);
	}

	public void AuditRecordingServerRemove(Guid recorderID, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecorderRemoved(recorderID, recordingServerName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecorderRemoved, recorderID, recordingServerName, text, ResourceType.RecordingServers, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecorderRemoved, recordingServerName);
	}

	public void AuditRecordingServerRename(string oldRecordingServerName, string newRecordingServerName, Guid recorderID, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecorderRenamed(recorderID, oldRecordingServerName, newRecordingServerName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecorderRenamed, recorderID, oldRecordingServerName, text, ResourceType.RecordingServers, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecorderRenamed, oldRecordingServerName, newRecordingServerName);
	}

	public void AuditDirectoryCreatedOnRecorder(Guid recorderID, string recordingServerName, string fullDirectoryName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditDirectoryCreatedOnRecorder(recorderID, recordingServerName, fullDirectoryName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditDirectoryCreatedOnRecorder, recorderID, recordingServerName, text, ResourceType.RecordingServers, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditDirectoryCreatedOnRecorder, recordingServerName, fullDirectoryName);
	}

	public void AuditStorageCreated(Guid storageId, string storageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecordingStorageCreated(storageId, storageName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecordingStorageCreated, storageId, storageName, recordingServerName, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecordingStorageCreated, storageName);
	}

	public void AuditStorageDeleted(Guid storageId, string storageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecordingStorageDeleted(storageId, storageName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecordingStorageDeleted, storageId, storageName, recordingServerName, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecordingStorageDeleted, storageName);
	}

	public void AuditStorageEdited(Guid storageId, string storageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecordingStorageEdited(storageId, storageName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecordingStorageEdited, storageId, storageName, recordingServerName, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecordingStorageEdited, storageName);
	}

	public void AuditStorageMoved(Guid storageId, string storageName, string oldPath, string newPath, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecordingStorageMoved(storageId, storageName, oldPath, newPath, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecordingStorageMoved, storageId, storageName, recordingServerName, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecordingStorageMoved, storageName, oldPath, newPath);
	}

	public void AuditStorageArchiveCreated(Guid storageId, string storageName, string archiveName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecordingStorageAddArchive(storageId, archiveName, storageName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecordingStorageAddArchive, storageId, storageName, recordingServerName, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecordingStorageAddArchive, archiveName, storageName);
	}

	public void AuditStorageArchiveDeleted(Guid storageId, string storageName, string archiveName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecordingStorageDeleteArchive(storageId, archiveName, storageName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecordingStorageDeleteArchive, storageId, storageName, recordingServerName, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecordingStorageDeleteArchive, archiveName, storageName);
	}

	public void AuditStorageArchiveEdited(Guid storageId, string storageName, string archiveName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecordingStorageEditArchive(storageId, archiveName, storageName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecordingStorageEditArchive, storageId, storageName, recordingServerName, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecordingStorageEditArchive, archiveName, storageName);
	}

	public void AuditStorageArchiveMoved(Guid storageId, string storageName, string archiveName, string oldPath, string newPath, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecordingStorageArchiveMoved(storageId, archiveName, storageName, oldPath, newPath, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecordingStorageArchiveMoved, storageId, storageName, recordingServerName, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecordingStorageArchiveMoved, archiveName, storageName, oldPath, newPath);
	}

	public void AuditStorageArchiveMounted(Guid storageId, string storageName, string archiveName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecordingStorageArchiveMounted(storageId, archiveName, storageName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecordingStorageArchiveMounted, storageId, storageName, recordingServerName, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecordingStorageArchiveMounted, archiveName, storageName);
	}

	public void AuditStorageArchiveUnmounted(Guid storageId, string storageName, string archiveName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditRecordingStorageArchiveUnmounted(storageId, archiveName, storageName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRecordingStorageArchiveUnmounted, storageId, storageName, recordingServerName, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRecordingStorageArchiveUnmounted, archiveName, storageName);
	}

	public void AuditVideoWallCreated(Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallCreated(videoWallId, videoWallName, text, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallCreated, videoWallId, videoWallName, text, ResourceType.SmartWall, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallCreated, applicationNameShort, videoWallName);
	}

	public void AuditVideoWallDeleted(Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallDeleted(videoWallId, videoWallName, text, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallDeleted, videoWallId, videoWallName, text, ResourceType.SmartWall, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallDeleted, applicationNameShort, videoWallName);
	}

	public void AuditVideoWallRenamed(string newVideoWallName, string oldVideoWallName, Guid videoWallId, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallRenamed(videoWallId, oldVideoWallName, newVideoWallName, text, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallRenamed, videoWallId, oldVideoWallName, text, ResourceType.SmartWall, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallRenamed, applicationNameShort, oldVideoWallName, newVideoWallName);
	}

	public void AuditVideoWallEdited(Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallEdited(videoWallId, videoWallName, text, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallEdited, videoWallId, videoWallName, text, ResourceType.SmartWall, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallEdited, applicationNameShort, videoWallName);
	}

	public void AuditVideoWallPresetAdded(Guid videoWallPresetId, string videoWallPresetName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallPresetAdded(videoWallPresetId, videoWallPresetName, videoWallName, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallPresetAdded, videoWallPresetId, videoWallPresetName, videoWallName, ResourceType.SmartWallPreset, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallPresetAdded, applicationNameShort, videoWallPresetName, videoWallName);
	}

	public void AuditVideoWallPresetDeleted(Guid videoWallPresetId, string videoWallPresetName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallPresetDeleted(videoWallPresetId, videoWallPresetName, videoWallName, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallPresetDeleted, videoWallPresetId, videoWallPresetName, videoWallName, ResourceType.SmartWallPreset, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallPresetDeleted, applicationNameShort, videoWallPresetName, videoWallName);
	}

	public void AuditVideoWallPresetRenamed(Guid videoWallPresetId, string oldVideoWallPresetName, string newVideoWallPresetName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallPresetRenamed(videoWallPresetId, oldVideoWallPresetName, newVideoWallPresetName, videoWallName, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallPresetRenamed, videoWallPresetId, oldVideoWallPresetName, videoWallName, ResourceType.SmartWallPreset, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallPresetRenamed, applicationNameShort, oldVideoWallPresetName, newVideoWallPresetName);
	}

	public void AuditVideoWallMonitorPresetEdited(Guid videoWallPresetId, string videoWallPresetName, Guid videoWallMonitorId, string videoWallMonitorName, string userCredentials, string locationIPAddress)
	{
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorPresetEdited(videoWallPresetId, videoWallPresetName, videoWallMonitorName, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorPresetEdited, videoWallPresetId, videoWallPresetName, videoWallMonitorName, ResourceType.SmartWallPreset, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorPresetEdited, applicationNameShort, videoWallPresetName, videoWallMonitorName);
	}

	public void AuditVideoWallPresetActivated(Guid videoWallPresetId, string videoWallPresetName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallPresetActivate(videoWallPresetId, videoWallPresetName, videoWallName, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallPresetActivate, videoWallPresetId, videoWallPresetName, videoWallName, ResourceType.SmartWallPreset, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallPresetActivate, applicationNameShort, videoWallPresetName);
	}

	public void AuditVideoWallMonitorAdded(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorAdded(videoWallMonitorId, videoWallMonitorName, videoWallName, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorAdded, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorAdded, applicationNameShort, videoWallMonitorName);
	}

	public void AuditVideoWallMonitorDeleted(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorDeleted(videoWallMonitorId, videoWallMonitorName, videoWallName, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorDeleted, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorDeleted, applicationNameShort, videoWallMonitorName);
	}

	public void AuditVideoWallMonitorEdited(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorEdited(videoWallMonitorId, videoWallMonitorName, videoWallName, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorEdited, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorEdited, applicationNameShort, videoWallMonitorName);
	}

	public void AuditVideoWallMonitorRenamed(string newVideoWallMonitorName, string oldVideoWallMonitorName, Guid videoWallMonitorId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorRenamed(videoWallMonitorId, oldVideoWallMonitorName, newVideoWallMonitorName, videoWallName, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorRenamed, videoWallMonitorId, oldVideoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorRenamed, applicationNameShort, oldVideoWallMonitorName, newVideoWallMonitorName);
	}

	public void AuditVideoWallMonitorLayoutChanged(string layoutName, string videoWallMonitorName, Guid videoWallMonitorId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorLayoutChanged(videoWallMonitorId, videoWallMonitorName, layoutName, videoWallName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorLayoutChanged, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.SmartWallControl, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorLayoutChanged, layoutName);
	}

	public void AuditVideoWallMonitorLayoutCameraPlaced(string cameraName, string position, string videoWallMonitorName, Guid videoWallMonitorId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorLayoutCameraChanged(videoWallMonitorId, videoWallMonitorName, videoWallName, cameraName, position, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorLayoutCameraChanged, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.SmartWallControl, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorLayoutCameraChanged, cameraName, position);
	}

	public void AuditVideoWallMonitorLayoutPositionCleared(string position, string videoWallMonitorName, Guid videoWallMonitorId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorLayoutPositionCleared(videoWallMonitorId, videoWallMonitorName, videoWallName, position, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorLayoutPositionCleared, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.SmartWallControl, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorLayoutPositionCleared, position);
	}

	public void AuditVideoWallMonitorLayoutCleared(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorLayoutCleared(videoWallMonitorId, videoWallMonitorName, videoWallName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorLayoutCleared, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.SmartWallControl, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorLayoutCleared);
	}

	public void AuditVideoWallMonitorLayoutPresetSelected(string presetName, string videoWallMonitorName, Guid videoWallMonitorId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		string applicationNameShort = OemResources.Instance.VideoWall.ApplicationNameShort;
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorLayoutPresetActivated(videoWallMonitorId, videoWallMonitorName, videoWallName, presetName, applicationNameShort, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorLayoutPresetActivated, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.SmartWallControl, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorLayoutPresetActivated, applicationNameShort, presetName);
	}

	public void AuditVideoWallMonitorLayoutMonitorApplied(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string sourceVideoWallMonitorName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorLayoutMonitorApplied(videoWallMonitorId, videoWallMonitorName, videoWallName, sourceVideoWallMonitorName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorLayoutMonitorApplied, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.SmartWallControl, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorLayoutMonitorApplied, sourceVideoWallMonitorName);
	}

	public void AuditVideoWallMonitorLayoutViewApplied(Guid videoWallMonitorId, string videoWallMonitorName, Guid videoWallId, string videoWallName, string sourceVideoWallViewName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorLayoutMonitorApplied(videoWallMonitorId, videoWallMonitorName, videoWallName, sourceVideoWallViewName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorLayoutViewApplied, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.SmartWallControl, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorLayoutViewApplied, sourceVideoWallViewName);
	}

	public void AuditVideoWallMonitorCameraRemoved(string cameraName, string position, string videoWallMonitorName, Guid videoWallMonitorId, string videoWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallMonitorCameraRemoved(videoWallMonitorId, videoWallMonitorName, videoWallName, cameraName, position, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallMonitorCameraRemoved, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.SmartWallControl, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallMonitorCameraRemoved, cameraName, position);
	}

	public void AuditVideoWallTextViewItemCreated(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string message, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditVideoWallTextViewItemCreated(videoWallMonitorId, videoWallMonitorName, videoWallName, message, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditVideoWallTextViewItemCreated, videoWallMonitorId, videoWallMonitorName, videoWallName, ResourceType.SmartWallMonitor, SourceType.Server, locationIPAddress, userCredentials, AuditType.SmartWallControl, AccessGranted.Granted, () => LogDefinitions.AuditVideoWallTextViewItemCreated, message);
	}

	public void AuditPtzPatrollingProfileCreated(Guid deviceId, string cameraName, string patrollingProfileName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditPtzPatrollingProfileCreated(deviceId, cameraName, patrollingProfileName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditPtzPatrollingProfileCreated, deviceId, cameraName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Ptz, AccessGranted.Granted, () => LogDefinitions.AuditPtzPatrollingProfileCreated, patrollingProfileName);
	}

	public void AuditPtzPatrollingProfileDeleted(Guid deviceId, string cameraName, string patrollingProfileName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditPtzPatrollingProfileDeleted(deviceId, cameraName, patrollingProfileName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditPtzPatrollingProfileDeleted, deviceId, cameraName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Ptz, AccessGranted.Granted, () => LogDefinitions.AuditPtzPatrollingProfileDeleted, patrollingProfileName);
	}

	public void AuditPtzPatrollingProfileChanged(Guid deviceId, string cameraName, string patrollingProfileName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditPtzPatrollingProfileChanged(deviceId, cameraName, patrollingProfileName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditPtzPatrollingProfileChanged, deviceId, cameraName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Ptz, AccessGranted.Granted, () => LogDefinitions.AuditPtzPatrollingProfileChanged, patrollingProfileName);
	}

	public void AuditPtzPresetCreated(Guid deviceId, string cameraName, string presetName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditPtzPresetCreated(deviceId, cameraName, presetName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditPtzPresetCreated, deviceId, cameraName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Ptz, AccessGranted.Granted, () => LogDefinitions.AuditPtzPresetCreated, presetName);
	}

	public void AuditPtzPresetChanged(Guid deviceId, string cameraName, string presetName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditPtzPresetChanged(deviceId, cameraName, presetName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditPtzPresetChanged, deviceId, cameraName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Ptz, AccessGranted.Granted, () => LogDefinitions.AuditPtzPresetChanged, presetName);
	}

	public void AuditPtzPresetDeleted(Guid deviceId, string cameraName, string presetName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditPtzPresetDeleted(deviceId, cameraName, presetName, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditPtzPresetDeleted, deviceId, cameraName, recordingServerName, ResourceType.Camera, SourceType.Server, locationIPAddress, userCredentials, AuditType.Ptz, AccessGranted.Granted, () => LogDefinitions.AuditPtzPresetDeleted, presetName);
	}

	public void AuditUserDefinedEventActivated(Guid eventId, string userDefinedEventName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditUserDefinedEventActivated(eventId, userDefinedEventName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditUserDefinedEventActivated, eventId, userDefinedEventName, text, ResourceType.UserDefinedEvents, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditUserDefinedEventActivated, userDefinedEventName);
	}

	public void AuditBookmarkCreated(string refId, string headline, DateTime beginTime, DateTime triggerTime, DateTime endTime, Guid deviceId, string deviceType, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditBookmarkCreated(deviceId, deviceName, LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType), refId, headline, beginTime, triggerTime, endTime, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBookmarkCreated, deviceId, deviceName, recordingServerName, LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType), SourceType.Server, locationIPAddress, userCredentials, AuditType.Bookmark, AccessGranted.Granted, () => LogDefinitions.AuditBookmarkCreated, refId, headline, beginTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), triggerTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
	}

	public void AuditBookmarkEdited(string refId, string headline, DateTime beginTime, DateTime triggerTime, DateTime endTime, Guid deviceId, string deviceType, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditBookmarkEdited(deviceId, deviceName, LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType), refId, headline, beginTime, triggerTime, endTime, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBookmarkEdited, deviceId, deviceName, recordingServerName, LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType), SourceType.Server, locationIPAddress, userCredentials, AuditType.Bookmark, AccessGranted.Granted, () => LogDefinitions.AuditBookmarkEdited, refId, headline, beginTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), triggerTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
	}

	public void AuditBookmarkDeleted(string refId, Guid deviceId, string deviceType, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditBookmarkDeleted(deviceId, deviceName, LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType), refId, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBookmarkDeleted, deviceId, deviceName, recordingServerName, LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType), SourceType.Server, locationIPAddress, userCredentials, AuditType.Bookmark, AccessGranted.Granted, () => LogDefinitions.AuditBookmarkDeleted, refId);
	}

	public void AuditBookmarkViewed(string refIds, Guid deviceId, string deviceType, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditBookmarkViewed(deviceId, deviceName, LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType), refIds, recordingServerName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBookmarkViewed, deviceId, deviceName, recordingServerName, LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType), SourceType.Server, locationIPAddress, userCredentials, AuditType.Bookmark, AccessGranted.Granted, () => LogDefinitions.AuditBookmarkViewed, refIds);
	}

	public void AuditClientProfileCreated(Guid clientProfileId, string clientProfileName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		string clientName = OemResources.Instance.SmartClient.ClientName;
		LogEntryAuditLog(LogEntryAuditFactory.AuditClientProfileCreated(clientProfileId, clientName, clientProfileName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditClientProfileCreated, clientProfileId, clientProfileName, text, ResourceType.ClientProfiles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditClientProfileCreated, clientName, clientProfileName);
	}

	public void AuditClientProfileEdited(Guid clientProfileId, string clientProfileName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		string clientName = OemResources.Instance.SmartClient.ClientName;
		LogEntryAuditLog(LogEntryAuditFactory.AuditClientProfileEdited(clientProfileId, clientName, clientProfileName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditClientProfileEdited, clientProfileId, clientProfileName, text, ResourceType.ClientProfiles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditClientProfileEdited, clientName, clientProfileName);
	}

	public void AuditClientProfileDelete(Guid clientProfileId, string clientProfileName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		string clientName = OemResources.Instance.SmartClient.ClientName;
		LogEntryAuditLog(LogEntryAuditFactory.AuditClientProfileDeleted(clientProfileId, clientName, clientProfileName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditClientProfileDeleted, clientProfileId, clientProfileName, text, ResourceType.ClientProfiles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditClientProfileDeleted, clientName, clientProfileName);
	}

	public void AuditClientProfileRenamed(string oldClientProfileName, string newClientProfileName, Guid clientProfileId, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		string clientName = OemResources.Instance.SmartClient.ClientName;
		LogEntryAuditLog(LogEntryAuditFactory.AuditClientProfileRenamed(clientProfileId, clientName, oldClientProfileName, newClientProfileName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditClientProfileRenamed, clientProfileId, oldClientProfileName, text, ResourceType.ClientProfiles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditClientProfileRenamed, clientName, oldClientProfileName, newClientProfileName);
	}

	public void AuditSitesSlaveSiteCreated(Guid slaveSiteId, string slaveSiteName, string slaveSiteAddress, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSiteSlaveCreated(slaveSiteId, slaveSiteName, slaveSiteAddress, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSiteSlaveCreated, slaveSiteId, slaveSiteName, text, ResourceType.FederatedArchitecture, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditSiteSlaveCreated, slaveSiteName, slaveSiteAddress);
	}

	public void AuditSitesSlaveSiteDeleted(Guid slaveSiteId, string slaveSiteName, string slaveSiteAddress, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSiteSlaveDeleted(slaveSiteId, slaveSiteName, slaveSiteAddress, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSiteSlaveDeleted, slaveSiteId, slaveSiteName, text, ResourceType.FederatedArchitecture, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditSiteSlaveDeleted, slaveSiteName, slaveSiteAddress);
	}

	public void AuditSitesMasterSiteDeleted(Guid masterSiteId, string masterSiteName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSiteMasterDeleted(masterSiteId, masterSiteName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSiteMasterDeleted, masterSiteId, masterSiteName, text, ResourceType.FederatedArchitecture, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditSiteMasterDeleted);
	}

	public void AuditSitesMasterSiteAuthorized(Guid masterSiteId, string masterSiteName, string masterSiteAddress, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSiteMasterAuthorized(masterSiteId, masterSiteName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSiteMasterAuthorized, masterSiteId, masterSiteName, text, ResourceType.FederatedArchitecture, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditSiteMasterAuthorized, text, masterSiteName);
	}

	public void AuditSitesMasterSiteAuthorizeDenied(Guid masterSiteId, string masterSiteName, string masterSiteAddress, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSiteMasterAuthorizedDenied(masterSiteId, masterSiteName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSiteMasterAuthorizedDenied, masterSiteId, masterSiteName, text, ResourceType.FederatedArchitecture, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Denied, () => LogDefinitions.AuditSiteMasterAuthorizedDenied, text, masterSiteName);
	}

	public void AuditSitesRenamed(Guid siteId, string newSiteName, string oldSiteName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditSiteRenamed(siteId, oldSiteName, newSiteName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSiteRenamed, siteId, oldSiteName, OemResources.Instance.VideoWall.ApplicationNameShort, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditSiteRenamed, oldSiteName, newSiteName);
	}

	public void AuditConfigurationExported(string locationIPAddress, string userCredentials)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditConfigurationExported(text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditConfigurationExported, Guid.Empty, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditConfigurationExported);
	}

	public void AuditConfigurationRestored(string locationIPAddress, string userCredentials)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditConfigurationRestored(text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditConfigurationRestored, Guid.Empty, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditConfigurationRestored);
	}

	public void AuditHostingServerAdded(int hostingServerId, string hostingServerName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditHostingServerAdded(hostingServerId, hostingServerName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHostingServerAdded, hostingServerId.ToString(CultureInfo.InvariantCulture), hostingServerName, text, ResourceType.RemoteConnectServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHostingServerAdded, hostingServerName);
	}

	public void AuditHostingServerModified(int hostingServerId, string hostingServerName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditHostingServerModified(hostingServerId, hostingServerName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHostingServerModified, hostingServerId.ToString(CultureInfo.InvariantCulture), hostingServerName, text, ResourceType.RemoteConnectServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHostingServerModified, hostingServerName);
	}

	public void AuditHostingServerRenamed(int hostingServerId, string newHostingServerName, string oldHostingServerName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditHostingServerRenamed(hostingServerId, oldHostingServerName, newHostingServerName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHostingServerRenamed, hostingServerId.ToString(CultureInfo.InvariantCulture), oldHostingServerName, text, ResourceType.RemoteConnectServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHostingServerRenamed, oldHostingServerName, newHostingServerName);
	}

	public void AuditHostingServerDeleted(int hostingServerId, string hostingServerName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditHostingServerDeleted(hostingServerId, hostingServerName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHostingServerDeleted, hostingServerId.ToString(CultureInfo.InvariantCulture), hostingServerName, text, ResourceType.RemoteConnectServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHostingServerDeleted, hostingServerName);
	}

	public void AuditHostedHardwareAdded(int hostedHardwareId, string hostedHardwareName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditHostedHardwareAdded(hostedHardwareId, hostedHardwareName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHostedHardwareAdded, hostedHardwareId.ToString(CultureInfo.InvariantCulture), hostedHardwareName, text, ResourceType.RemoteConnectHardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHostedHardwareAdded, hostedHardwareName);
	}

	public void AuditHostedHardwareModified(int hostedHardwareId, string hostedHardwareName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditHostedHardwareModified(hostedHardwareId, hostedHardwareName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHostedHardwareModified, hostedHardwareId.ToString(CultureInfo.InvariantCulture), hostedHardwareName, text, ResourceType.RemoteConnectHardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHostedHardwareModified, hostedHardwareName);
	}

	public void AuditHostedHardwareRenamed(int hostedHardwareId, string newHostedHardwareName, string oldHostedHardwareName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditHostedHardwareRenamed(hostedHardwareId, oldHostedHardwareName, newHostedHardwareName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHostedHardwareRenamed, hostedHardwareId.ToString(CultureInfo.InvariantCulture), oldHostedHardwareName, text, ResourceType.RemoteConnectHardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHostedHardwareRenamed, oldHostedHardwareName, newHostedHardwareName);
	}

	public void AuditHostedHardwareDeleted(int hostedHardwareId, string hostedHardwareName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditHostedHardwareDeleted(hostedHardwareId, hostedHardwareName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditHostedHardwareDeleted, hostedHardwareId.ToString(CultureInfo.InvariantCulture), hostedHardwareName, text, ResourceType.RemoteConnectHardware, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditHostedHardwareDeleted, hostedHardwareName);
	}

	public void AuditBasicOwnerInfoCreated(string id, string name, string details, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicInfoCreated(id, name, details, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicInfoCreated, id, name, text, ResourceType.SiteInfo, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditBasicInfoCreated, name, details);
	}

	public void AuditBasicOwnerInfoEdited(string id, string name, string details, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicInfoEdited(id, name, details, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicInfoEdited, id, name, text, ResourceType.SiteInfo, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditBasicInfoEdited, name, details);
	}

	public void AuditBasicOwnerInfoDelete(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicInfoDeleted(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicInfoDeleted, id, name, text, ResourceType.SiteInfo, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditBasicInfoDeleted, name);
	}

	public void AuditMarkedDataCreated(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditMarkedDataCreated(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditMarkedDataCreated, id, name, text, ResourceType.MarkedData, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditMarkedDataCreated, name);
	}

	public void AuditMarkedDataUpdated(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditMarkedDataUpdate(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditMarkedDataUpdate, id, name, text, ResourceType.MarkedData, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditMarkedDataUpdate, name);
	}

	public void AuditMarkedDataDelete(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditMarkedDataDeleted(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditMarkedDataDeleted, id, name, text, ResourceType.MarkedData, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditMarkedDataDeleted, name);
	}

	public void AuditMediaRestrictionCreated(Guid id, string name, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditMediaRestrictionCreated(id, name, _dataAccessor.ManagementServerName(), userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditMediaRestrictionCreated, id, name, _dataAccessor.ManagementServerName(), ResourceType.MediaRestriction, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditMediaRestrictionCreated, name);
	}

	public void AuditMediaRestrictionUpdated(Guid id, string name, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditMediaRestrictionUpdated(id, name, _dataAccessor.ManagementServerName(), userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditMediaRestrictionUpdated, id, name, _dataAccessor.ManagementServerName(), ResourceType.MediaRestriction, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditMediaRestrictionUpdated, name);
	}

	public void AuditMediaRestrictionDeleted(Guid id, string name, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditMediaRestrictionDeleted(id, name, _dataAccessor.ManagementServerName(), userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditMediaRestrictionDeleted, id, name, _dataAccessor.ManagementServerName(), ResourceType.MediaRestriction, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditMediaRestrictionDeleted, name);
	}

	public void AuditMediaRestrictionLiveModeEntered(Guid deviceId, string deviceName, string deviceType, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditMediaRestrictionLiveModeEntered(deviceId, deviceName, resourceType, _dataAccessor.ManagementServerName(), userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditMediaRestrictionLiveModeEntered, deviceId, deviceName, _dataAccessor.ManagementServerName(), resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditMediaRestrictionLiveModeEntered, deviceName);
	}

	public void AuditMediaRestrictionLiveModeExited(Guid deviceId, string deviceName, string deviceType, string header, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditMediaRestrictionLiveModeExited(deviceId, deviceName, resourceType, header, _dataAccessor.ManagementServerName(), userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditMediaRestrictionLiveModeExited, deviceId, deviceName, _dataAccessor.ManagementServerName(), resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditMediaRestrictionLiveModeExited, deviceName, header);
	}

	public void AuditMediaRestrictionLiveUpdated(Guid deviceId, string deviceName, string deviceType, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditMediaRestrictionLiveUpdated(deviceId, deviceName, resourceType, _dataAccessor.ManagementServerName(), userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditMediaRestrictionLiveUpdated, deviceId, deviceName, _dataAccessor.ManagementServerName(), resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditMediaRestrictionLiveUpdated, deviceName);
	}

	public void AuditMediaRestrictionLiveDeleted(Guid deviceId, string deviceName, string deviceType, string userCredentials, string locationIPAddress)
	{
		ResourceType resourceType = LogManagerBase.RegisteredDeviceTypeToResourceType(deviceType);
		LogEntryAuditLog(LogEntryAuditFactory.AuditMediaRestrictionLiveDeleted(deviceId, deviceName, resourceType, _dataAccessor.ManagementServerName(), userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditMediaRestrictionLiveDeleted, deviceId, deviceName, _dataAccessor.ManagementServerName(), resourceType, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditMediaRestrictionLiveDeleted, deviceName);
	}

	public void AuditPerformanceStateRuleCreated(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditPerfCountStateRuleCreated(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditPerfCountStateRuleCreated, id, name, text, ResourceType.PerformanceCounterRule, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditPerfCountStateRuleCreated, name);
	}

	public void AuditPerformanceStateRuleUpdated(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditPerfCountStateRuleUpdated(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditPerfCountStateRuleUpdated, id, name, text, ResourceType.PerformanceCounterRule, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditPerfCountStateRuleUpdated, name);
	}

	public void AuditPerformanceStateRuleDelete(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditPerfCountStateRuleDeleted(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditPerfCountStateRuleDeleted, id, name, text, ResourceType.PerformanceCounterRule, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditPerfCountStateRuleDeleted, name);
	}

	public void AuditDashboardTileCreated(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSystemDashboardConfigurationCreated(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSystemDashboardConfigurationCreated, id, name, text, ResourceType.SystemDashboardConfiguration, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditSystemDashboardConfigurationCreated, name);
	}

	public void AuditDashboardTileUpdated(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSystemDashboardConfigurationUpdated(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSystemDashboardConfigurationUpdated, id, name, text, ResourceType.SystemDashboardConfiguration, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditSystemDashboardConfigurationUpdated, name);
	}

	public void AuditDashboardTileDelete(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSystemDashboardConfigurationDeleted(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSystemDashboardConfigurationDeleted, id, name, text, ResourceType.SystemDashboardConfiguration, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditSystemDashboardConfigurationDeleted, name);
	}

	public void AuditBasicUserCreated(string sid, string userName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicUserCreated(sid, userName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicUserCreated, sid, userName, text, ResourceType.BasicUser, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditBasicUserCreated, userName);
	}

	public void AuditBasicUserEdited(string sid, string userName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicUserEdited(sid, userName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicUserEdited, sid, userName, text, ResourceType.BasicUser, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditBasicUserEdited, userName);
	}

	public void AuditBasicUserDelete(string sid, string userName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicUserDeleted(sid, userName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicUserDeleted, sid, userName, text, ResourceType.BasicUser, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditBasicUserDeleted, userName);
	}

	public void AuditBasicUserRenamed(string oldUserName, string newUserName, string sid, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicUserRenamed(sid, oldUserName, newUserName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicUserRenamed, sid, oldUserName, text, ResourceType.BasicUser, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditBasicUserRenamed, oldUserName, newUserName);
	}

	public void AuditBasicUserPasswordChanged(string sid, string userName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicUserPasswordChanged(sid, userName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicUserPasswordChanged, sid, userName, text, ResourceType.BasicUser, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditBasicUserPasswordChanged, userName);
	}

	public void AuditBasicUserForcePasswordChange(string sid, string userName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicUserForcePasswordChange(sid, userName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicUserForcePasswordChange, sid, userName, text, ResourceType.BasicUser, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditBasicUserForcePasswordChange, userName);
	}

	public void AuditBasicUserLockedOut(string sid, string userName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicUserLockedOut(sid, userName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicUserLockedOut, sid, userName, text, ResourceType.BasicUser, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditBasicUserLockedOut, userName);
	}

	public void AuditBasicUserEnabled(string sid, string userName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicUserEnabled(sid, userName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicUserEnabled, sid, userName, text, ResourceType.BasicUser, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditBasicUserEnabled, userName);
	}

	public void AuditRetentionOptionRoleRelationUpdated(string roleId, string roleName, string retentionName, string userCredentials, string remoteIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditRetentionOptionRoleRelationUpdated(roleName, retentionName, text, roleId, userCredentials, remoteIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditRetentionOptionRoleRelationUpdated, roleId, roleName, text, ResourceType.RetentionOptionRoleRelation, SourceType.Server, remoteIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRetentionOptionRoleRelationUpdated, roleName, retentionName);
	}

	public void AuditAutomaticActivationCredentialsChanged(string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditAutomaticactivationCredentialsChanged(resourceId, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditAutomaticactivationCredentialsChanged, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditAutomaticactivationCredentialsChanged);
	}

	public void AuditAutomaticActivationEnabled(string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditAutomaticactivationEnabled(resourceId, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditAutomaticactivationEnabled, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditAutomaticactivationEnabled);
	}

	public void AuditAutomaticActivationDisabled(string userCredentials, string locationIPAddress)
	{
		Guid resourceId = _dataAccessor.ManagementServerId();
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditAutomaticactivationDisabled(resourceId, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditAutomaticactivationDisabled, resourceId, text, text, ResourceType.ManagementServer, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditAutomaticactivationDisabled);
	}

	public void AuditAudioMessageCreated(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditAudioMessageCreated(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditAudioMessageCreated, id, name, text, ResourceType.AudioMessage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditAudioMessageCreated, name);
	}

	public void AuditAudioMessageContentChanged(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditAudioMessageContentChanged(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditAudioMessageContentChanged, id, name, text, ResourceType.AudioMessage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditAudioMessageContentChanged, name);
	}

	public void AuditAudioMessageDeleted(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditAudioMessageDeleted(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditAudioMessageDeleted, id, name, text, ResourceType.AudioMessage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditAudioMessageDeleted, name);
	}

	public void AuditSearchCreated(Guid id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSearchCreated(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSearchCreated, id, name, text, ResourceType.SaveSearches, SourceType.Server, locationIPAddress, userCredentials, AuditType.SaveSearches, AccessGranted.Granted, () => LogDefinitions.AuditSearchCreated, name);
	}

	public void AuditSearchEdited(Guid id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSearchEdited(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSearchEdited, id, name, text, ResourceType.SaveSearches, SourceType.Server, locationIPAddress, userCredentials, AuditType.SaveSearches, AccessGranted.Granted, () => LogDefinitions.AuditSearchEdited, name);
	}

	public void AuditSearchDeleted(Guid id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSearchDeleted(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSearchDeleted, id, name, text, ResourceType.SaveSearches, SourceType.Server, locationIPAddress, userCredentials, AuditType.SaveSearches, AccessGranted.Granted, () => LogDefinitions.AuditSearchDeleted, name);
	}

	public void AuditSearchRenamed(Guid id, string name, string oldName, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditSearchRenamed(id, name, oldName, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditSearchRenamed, id, name, text, ResourceType.SaveSearches, SourceType.Server, locationIPAddress, userCredentials, AuditType.SaveSearches, AccessGranted.Granted, () => LogDefinitions.AuditSearchRenamed, oldName, name);
	}

	public void AuditMetadataSearchCategoryVisibilityChanged(string categoryId, string categoryName, bool updatedVisibility, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		if (updatedVisibility)
		{
			LogEntryAuditLog(LogEntryAuditFactory.AuditMetadataSearchCategoryIsEnabled(categoryId, categoryName, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditMetadataSearchCategoryIsEnabled, categoryId, categoryName, text, ResourceType.MetadataSearch, SourceType.Server, locationIPAddress, userCredentials, AuditType.MetadataSearch, AccessGranted.Granted, () => LogDefinitions.AuditMetadataSearchCategoryIsEnabled, categoryName);
		}
		else
		{
			LogEntryAuditLog(LogEntryAuditFactory.AuditMetadataSearchCategoryIsDisabled(categoryId, categoryName, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditMetadataSearchCategoryIsDisabled, categoryId, categoryName, text, ResourceType.MetadataSearch, SourceType.Server, locationIPAddress, userCredentials, AuditType.MetadataSearch, AccessGranted.Granted, () => LogDefinitions.AuditMetadataSearchCategoryIsDisabled, categoryName);
		}
	}

	public void AuditMetadataSearchFilterVisibilityChanged(string filterId, string filterName, bool updatedVisibility, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		if (updatedVisibility)
		{
			LogEntryAuditLog(LogEntryAuditFactory.AuditMetadataSearchFilterIsEnabled(filterId, filterName, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditMetadataSearchFilterIsEnabled, filterId, filterName, text, ResourceType.MetadataSearch, SourceType.Server, locationIPAddress, userCredentials, AuditType.MetadataSearch, AccessGranted.Granted, () => LogDefinitions.AuditMetadataSearchFilterIsEnabled, filterName);
		}
		else
		{
			LogEntryAuditLog(LogEntryAuditFactory.AuditMetadataSearchFilterIsDisabled(filterId, filterName, text, userCredentials, locationIPAddress));
			_auditAndSystemLogger.Log(LogEntryId.AuditMetadataSearchFilterIsDisabled, filterId, filterName, text, ResourceType.MetadataSearch, SourceType.Server, locationIPAddress, userCredentials, AuditType.MetadataSearch, AccessGranted.Granted, () => LogDefinitions.AuditMetadataSearchFilterIsDisabled, filterName);
		}
	}

	public void AuditFileUploadToRecorder(string fileId, string recorderName, string userCredentials, string locationIPAddress)
	{
		LogEntryAuditLog(LogEntryAuditFactory.AuditFileTransferToRecorderStarted(fileId, recorderName, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditFileTransferToRecorderStarted, fileId, recorderName, recorderName, ResourceType.Storage, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditFileTransferToRecorderStarted, fileId, recorderName);
	}

	public void AuditMatrixCreated(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.MatrixCreated(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.MatrixCreated, id, name, text, ResourceType.Matrix, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.MatrixCreated, name);
	}

	public void AuditMatrixUpdated(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.MatrixUpdated(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.MatrixUpdated, id, name, text, ResourceType.Matrix, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.MatrixUpdated, name);
	}

	public void AuditMatrixDelete(string id, string name, string userCredentials, string locationIPAddress)
	{
		string text = _dataAccessor.ManagementServerName();
		LogEntryAuditLog(LogEntryAuditFactory.AuditBasicInfoDeleted(id, name, text, userCredentials, locationIPAddress));
		_auditAndSystemLogger.Log(LogEntryId.AuditBasicInfoDeleted, id, name, text, ResourceType.Matrix, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.MatrixDeleted, name);
	}
}
