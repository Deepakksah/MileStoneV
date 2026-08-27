using System;
using System.CodeDom.Compiler;
using System.Globalization;

namespace VideoOS.Common.Integration;

[GeneratedCode("CreateLogEntryAuditFactory build task", "0.0.0.0")]
public static class LogEntryAuditFactory
{
	public static LogEntryAudit AuditHardwarePasswordRead(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwarePasswordRead, locationIPAddress, SourceType.Server, hardwareName, userCredentials, ResourceType.Hardware, hardwareName, recordingServerName, hardwareId.ToString(), AuditType.Security, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Hardware password read for '{0}'", hardwareName);
		logEntryAudit.Arguments = new string[1] { hardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwarePasswordReadFailedInsufficientRights(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwarePasswordReadFailedInsufficientRights, locationIPAddress, SourceType.Server, hardwareName, userCredentials, ResourceType.Hardware, hardwareName, recordingServerName, hardwareId.ToString(), AuditType.Security, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Cannot read hardware password for '{0}'. Insufficient user permissions", hardwareName);
		logEntryAudit.Arguments = new string[1] { hardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditLoginCallingApplictionUnknown(string applicationName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditLoginCallingApplictionUnknown, locationIPAddress, SourceType.Server, applicationName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, applicationName, AuditType.Security, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User not allowed logging on from '{0}'. Access denied.", applicationName);
		logEntryAudit.Arguments = new string[1] { applicationName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditLoginUserNotRegisteredOnTheSystem(string applicationName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginUserNotRegisteredOnTheSystem, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, applicationName, AuditType.Security, AccessGranted.Denied)
		{
			Description = "User not allowed logging in to this system. Access denied."
		};
	}

	public static LogEntryAudit AuditLoginUserNotAdminPriviligesOnTheSystem(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginUserNotAdminPriviligesOnTheSystem, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Denied)
		{
			Description = "Login failed for user. Insufficient user permissions"
		};
	}

	public static LogEntryAudit AuditLoginUserNotAllowed(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginUserNotAllowed, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Denied)
		{
			Description = "Login failed for user. Insufficient user permissions"
		};
	}

	public static LogEntryAudit AuditLoginUserNotAllowedCommandApiAccess(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginUserNotAllowedCommandApiAccess, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Denied)
		{
			Description = "Login failed for user. Insufficient user permissions"
		};
	}

	public static LogEntryAudit AuditLoginUserNotAllowedHttpRequestAccess(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginUserNotAllowedHttpRequestAccess, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Denied)
		{
			Description = "Login failed for user. Insufficient user permissions"
		};
	}

	public static LogEntryAudit AuditLoginUserNotAllowedExternalAccess(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginUserNotAllowedExternalAccess, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Denied)
		{
			Description = "Login failed for user. Insufficient user permissions"
		};
	}

	public static LogEntryAudit AuditLoginSuccess(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginSuccess, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Granted)
		{
			Description = "User successfully logged in"
		};
	}

	public static LogEntryAudit AuditLoginSuccessFromAdministration(Guid resourceId, string managementServerName, string oemManagementClientName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditLoginSuccessFromAdministration, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User successfully logged in to the system from the {0}.", oemManagementClientName);
		logEntryAudit.Arguments = new string[1] { oemManagementClientName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditLoginSuccessFromCommandApi(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginSuccessFromCommandApi, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Granted)
		{
			Description = "User successfully logged in"
		};
	}

	public static LogEntryAudit AuditLoginSuccessFromExternalSource(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginSuccessFromExternalSource, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Granted)
		{
			Description = "User successfully logged in to the system from an external source."
		};
	}

	public static LogEntryAudit AuditLoginSuccessFromHttpRequest(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginSuccessFromHttpRequest, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Granted)
		{
			Description = "User successfully logged in to the system from HTTP request."
		};
	}

	public static LogEntryAudit AuditLogoutSuccessFromAdministration(Guid resourceId, string managementServerName, string oemManagementClientName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditLogoutSuccessFromAdministration, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Logout, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User logged out of the system from the {0}.", oemManagementClientName);
		logEntryAudit.Arguments = new string[1] { oemManagementClientName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditLoginUnSuccessfulUnknownUser(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginUnSuccessfulUnknownUser, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Denied)
		{
			Description = "Unsuccessful login - Unknown user"
		};
	}

	public static LogEntryAudit AuditLoginUnSuccessfulIncorrectPassword(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginUnSuccessfulIncorrectPassword, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Denied)
		{
			Description = "Unsuccessful login - Incorrect password"
		};
	}

	public static LogEntryAudit AuditLogout(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLogout, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Granted)
		{
			Description = "Logout"
		};
	}

	public static LogEntryAudit AuditLoginLogoutTimeout(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginLogoutTimeout, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Granted)
		{
			Description = "User connection timed out"
		};
	}

	public static LogEntryAudit AuditLoginTimeBased(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLoginTimeBased, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Denied)
		{
			Description = "Client access denied due to time based security restrictions"
		};
	}

	public static LogEntryAudit AuditLogoutTimeBased(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLogoutTimeBased, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Granted)
		{
			Description = "Client logged out due to time based security restrictions"
		};
	}

	public static LogEntryAudit AuditVideoWallPresetActivateTimeBased(Guid videoWallPresetId, string videoWallPresetName, string videoWallName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallPresetActivateTimeBased, locationIPAddress, SourceType.Server, videoWallPresetName, userCredentials, ResourceType.SmartWallPreset, videoWallPresetName, videoWallName, videoWallPresetId.ToString(), AuditType.Security, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} preset activation denied due to time based security restrictions", oemSmartWallName);
		logEntryAudit.Arguments = new string[1] { oemSmartWallName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorLayoutMonitorAppliedTimeBased(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorLayoutMonitorAppliedTimeBased, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.Security, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Apply {0} layout denied due to time based security restrictions", oemSmartWallName);
		logEntryAudit.Arguments = new string[1] { oemSmartWallName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditAuthorizationSucessful(Guid resourceId, string managementServerName, string authorizedUser, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditAuthorizationSucessful, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User authorized '{0}'", authorizedUser);
		logEntryAudit.Arguments = new string[1] { authorizedUser };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditAuthorizationFailedSelfAuthorization(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditAuthorizationFailedSelfAuthorization, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Denied)
		{
			Description = "Authorization failed: User can not authorize himself."
		};
	}

	public static LogEntryAudit AuditAuthorizationFailedInsufficientRights(Guid resourceId, string managementServerName, string authorizedUser, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditAuthorizationFailedInsufficientRights, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Security, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User failed to authorize {0}: Insufficient user permissions.", authorizedUser);
		logEntryAudit.Arguments = new string[1] { authorizedUser };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceAdded(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceAdded, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Device '{0}' added", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRemoved(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRemoved, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Device '{0}' removed", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceEnabled(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceEnabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Device '{0}' enabled", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceDisabled(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceDisabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Device '{0}' disabled", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRenamed(Guid deviceId, string deviceName, string newDeviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRenamed, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Device '{0}' renamed to '{1}'", deviceName, newDeviceName);
		logEntryAudit.Arguments = new string[2] { deviceName, newDeviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRecordSettingsChanged(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRecordSettingsChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Recording settings changed on '{0}'", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceStreamSettingsChanged(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceStreamSettingsChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Stream settings changed on '{0}'", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceMotionDetectionSettingsChanged(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceMotionDetectionSettingsChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Motion detection settings changed on '{0}'", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceDatabaseDelete(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceDatabaseDelete, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Emptying storage for '{0}' requested on the recording server '{1}'.", deviceName, recordingServerName);
		logEntryAudit.Arguments = new string[2] { deviceName, recordingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceDatabaseMove(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string databaseBankName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceDatabaseMove, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Database for '{0}' moved to use '{1}'", deviceName, databaseBankName);
		logEntryAudit.Arguments = new string[2] { deviceName, databaseBankName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceSettingsChanged(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceSettingsChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Settings changed on '{0}'", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceReplaced(Guid deviceId, string deviceName, string newDeviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceReplaced, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The device '{0}' was replaced with '{1}'. It inherits the database.", deviceName, newDeviceName);
		logEntryAudit.Arguments = new string[2] { deviceName, newDeviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceReplacedNoDatabase(Guid deviceId, string deviceName, string newDeviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceReplacedNoDatabase, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The device '{0}' was replaced with '{1}'.", deviceName, newDeviceName);
		logEntryAudit.Arguments = new string[2] { deviceName, newDeviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwareAdded(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwareAdded, locationIPAddress, SourceType.Server, hardwareName, userCredentials, ResourceType.Hardware, hardwareName, recordingServerName, hardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Hardware '{0}' added", hardwareName);
		logEntryAudit.Arguments = new string[1] { hardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwareRemoved(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwareRemoved, locationIPAddress, SourceType.Server, hardwareName, userCredentials, ResourceType.Hardware, hardwareName, recordingServerName, hardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Hardware '{0}' removed", hardwareName);
		logEntryAudit.Arguments = new string[1] { hardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwareEnabled(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwareEnabled, locationIPAddress, SourceType.Server, hardwareName, userCredentials, ResourceType.Hardware, hardwareName, recordingServerName, hardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Hardware '{0}' enabled", hardwareName);
		logEntryAudit.Arguments = new string[1] { hardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwareDisabled(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwareDisabled, locationIPAddress, SourceType.Server, hardwareName, userCredentials, ResourceType.Hardware, hardwareName, recordingServerName, hardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Hardware '{0}' disabled", hardwareName);
		logEntryAudit.Arguments = new string[1] { hardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwareRenamed(Guid hardwareId, string hardwareName, string newHardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwareRenamed, locationIPAddress, SourceType.Server, newHardwareName, userCredentials, ResourceType.Hardware, newHardwareName, recordingServerName, hardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Hardware '{0}' renamed to '{1}'", hardwareName, newHardwareName);
		logEntryAudit.Arguments = new string[2] { hardwareName, newHardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwareSettingsChanged(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwareSettingsChanged, locationIPAddress, SourceType.Server, hardwareName, userCredentials, ResourceType.Hardware, hardwareName, recordingServerName, hardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Settings changed on '{0}'", hardwareName);
		logEntryAudit.Arguments = new string[1] { hardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwareReplaced(Guid hardwareId, string hardwareName, string newHardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwareReplaced, locationIPAddress, SourceType.Server, newHardwareName, userCredentials, ResourceType.Hardware, newHardwareName, recordingServerName, hardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Hardware '{0}' replaced by '{1}'", hardwareName, newHardwareName);
		logEntryAudit.Arguments = new string[2] { hardwareName, newHardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDevicePrivacyMaskSettingsChanged(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDevicePrivacyMaskSettingsChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Privacy mask settings changed on '{0}'", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRuleCreated(Guid ruleId, string ruleName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRuleCreated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Rules, ruleName, managementServerName, ruleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Rule '{0}' created", ruleName);
		logEntryAudit.Arguments = new string[1] { ruleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRuleDeleted(Guid ruleId, string ruleName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRuleDeleted, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Rules, ruleName, managementServerName, ruleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Rule '{0}' deleted", ruleName);
		logEntryAudit.Arguments = new string[1] { ruleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRuleEdited(Guid ruleId, string ruleName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRuleEdited, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Rules, ruleName, managementServerName, ruleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Rule '{0}' edited", ruleName);
		logEntryAudit.Arguments = new string[1] { ruleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRuleDisabled(Guid ruleId, string ruleName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRuleDisabled, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Rules, ruleName, managementServerName, ruleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Rule '{0}' disabled", ruleName);
		logEntryAudit.Arguments = new string[1] { ruleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRuleEnabled(Guid ruleId, string ruleName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRuleEnabled, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Rules, ruleName, managementServerName, ruleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Rule '{0}' enabled", ruleName);
		logEntryAudit.Arguments = new string[1] { ruleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRuleRenamed(Guid ruleId, string ruleName, string newRuleName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRuleRenamed, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Rules, ruleName, managementServerName, ruleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Rule '{0}' renamed to '{1}'", ruleName, newRuleName);
		logEntryAudit.Arguments = new string[2] { ruleName, newRuleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMediaRestrictionCreated(Guid mediaRestrictionId, string mediaRestrictionName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMediaRestrictionCreated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.MediaRestriction, mediaRestrictionName, managementServerName, mediaRestrictionId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Created playback restriction '{0}'.", mediaRestrictionName);
		logEntryAudit.Arguments = new string[1] { mediaRestrictionName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMediaRestrictionUpdated(Guid mediaRestrictionId, string mediaRestrictionName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMediaRestrictionUpdated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.MediaRestriction, mediaRestrictionName, managementServerName, mediaRestrictionId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Updated playback restriction '{0}'.", mediaRestrictionName);
		logEntryAudit.Arguments = new string[1] { mediaRestrictionName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMediaRestrictionDeleted(Guid mediaRestrictionId, string mediaRestrictionName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMediaRestrictionDeleted, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.MediaRestriction, mediaRestrictionName, managementServerName, mediaRestrictionId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Deleted playback restriction '{0}'.", mediaRestrictionName);
		logEntryAudit.Arguments = new string[1] { mediaRestrictionName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMediaRestrictionLiveModeEntered(Guid deviceId, string deviceName, ResourceType deviceType, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMediaRestrictionLiveModeEntered, locationIPAddress, SourceType.Server, managementServerName, userCredentials, deviceType, deviceName, managementServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "'{0}' has entered restricted live mode.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMediaRestrictionLiveModeExited(Guid deviceId, string deviceName, ResourceType deviceType, string mediaRestrictionName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMediaRestrictionLiveModeExited, locationIPAddress, SourceType.Server, managementServerName, userCredentials, deviceType, deviceName, managementServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "'{0}' has exited restricted live mode and has been transformed into playback restriction '{1}'", deviceName, mediaRestrictionName);
		logEntryAudit.Arguments = new string[2] { deviceName, mediaRestrictionName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMediaRestrictionLiveUpdated(Guid deviceId, string deviceName, ResourceType deviceType, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMediaRestrictionLiveUpdated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, deviceType, deviceName, managementServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Updated live restriction for '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMediaRestrictionLiveDeleted(Guid deviceId, string deviceName, ResourceType deviceType, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMediaRestrictionLiveDeleted, locationIPAddress, SourceType.Server, managementServerName, userCredentials, deviceType, deviceName, managementServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Deleted live restriction '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditTimeProfileCreated(Guid timeProfileId, string timeProfileName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditTimeProfileCreated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.TimeProfiles, timeProfileName, managementServerName, timeProfileId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Time profile '{0}' created", timeProfileName);
		logEntryAudit.Arguments = new string[1] { timeProfileName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditTimeProfileDeleted(Guid timeProfileId, string timeProfileName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditTimeProfileDeleted, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.TimeProfiles, timeProfileName, managementServerName, timeProfileId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Time profile '{0}' deleted", timeProfileName);
		logEntryAudit.Arguments = new string[1] { timeProfileName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditTimeProfileEdited(Guid timeProfileId, string timeProfileName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditTimeProfileEdited, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.TimeProfiles, timeProfileName, managementServerName, timeProfileId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Time profile '{0}' edited", timeProfileName);
		logEntryAudit.Arguments = new string[1] { timeProfileName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditTimeProfileRenamed(Guid timeProfileId, string timeProfileName, string newTimeProfileName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditTimeProfileRenamed, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.TimeProfiles, timeProfileName, managementServerName, timeProfileId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Time profile '{0}' renamed to '{1}'", timeProfileName, newTimeProfileName);
		logEntryAudit.Arguments = new string[2] { timeProfileName, newTimeProfileName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleCreated(Guid roleId, string roleName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleCreated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Role '{0}' created", roleName);
		logEntryAudit.Arguments = new string[1] { roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleDeleted(Guid roleId, string roleName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleDeleted, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Role '{0}' deleted", roleName);
		logEntryAudit.Arguments = new string[1] { roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleEdited(Guid roleId, string roleName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleEdited, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Role '{0}' access permissions changed", roleName);
		logEntryAudit.Arguments = new string[1] { roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleRenamed(Guid roleId, string roleName, string newRoleName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleRenamed, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Role '{0}' renamed to '{1}'", roleName, newRoleName);
		logEntryAudit.Arguments = new string[2] { roleName, newRoleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleUserAdded(Guid roleId, string roleName, string userName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleUserAdded, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User '{0}' added to the '{1}' role", userName, roleName);
		logEntryAudit.Arguments = new string[2] { userName, roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleUserRemoved(Guid roleId, string roleName, string userName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleUserRemoved, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User '{0}' removed from the '{1}' role", userName, roleName);
		logEntryAudit.Arguments = new string[2] { userName, roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleGroupAdded(Guid roleId, string roleName, string groupName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleGroupAdded, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User group '{0}' added to the '{1}' role", groupName, roleName);
		logEntryAudit.Arguments = new string[2] { groupName, roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleGroupRemoved(Guid roleId, string roleName, string groupName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleGroupRemoved, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User group '{0}' removed from the '{1}' role", groupName, roleName);
		logEntryAudit.Arguments = new string[2] { groupName, roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleOverallSecurityChanged(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string accessStatus, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleOverallSecurityChanged, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The access permission '{0}' - '{1}' on role '{2}' has been changed to: {3}", accessRight, securityGroup, roleName, accessStatus);
		logEntryAudit.Arguments = new string[4] { accessRight, securityGroup, roleName, accessStatus };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleOverallSecurityGranted(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleOverallSecurityGranted, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The state Allow for the access permission '{0}'-'{1}' is granted", accessRight, securityGroup);
		logEntryAudit.Arguments = new string[2] { accessRight, securityGroup };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleOverallSecurityRevoked(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleOverallSecurityRevoked, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The state Allow for the access permission '{0}'-'{1}' is revoked", accessRight, securityGroup);
		logEntryAudit.Arguments = new string[2] { accessRight, securityGroup };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleOverallSecurityDenied(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleOverallSecurityDenied, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The state Deny for the access permission '{0}'-'{1}' is granted", accessRight, securityGroup);
		logEntryAudit.Arguments = new string[2] { accessRight, securityGroup };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleOverallSecurityRevokedDenied(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleOverallSecurityRevokedDenied, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The state Deny for the access permission '{0}'-'{1}' is revoked", accessRight, securityGroup);
		logEntryAudit.Arguments = new string[2] { accessRight, securityGroup };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityGranted(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityGranted, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The access permission '{0}'-'{1}' is granted on '{2}'", accessRight, securityGroup, objectName);
		logEntryAudit.Arguments = new string[3] { accessRight, securityGroup, objectName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityRevoked(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityRevoked, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The access permission '{0}'-'{1}' is revoked on '{2}'", accessRight, securityGroup, objectName);
		logEntryAudit.Arguments = new string[3] { accessRight, securityGroup, objectName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityTimeProfileToProfileChange(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, string oldTimeProfileName, string newTimeProfileName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityTimeProfileToProfileChange, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The time profile is changed for access permission '{0}'-'{1}' from time profile '{2}' to time profile '{3}' on '{4}'", accessRight, securityGroup, oldTimeProfileName, newTimeProfileName, objectName);
		logEntryAudit.Arguments = new string[5] { accessRight, securityGroup, oldTimeProfileName, newTimeProfileName, objectName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityTimeProfileToSystemProfileChange(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, string oldTimeProfileName, string newSystemTimeProfileName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityTimeProfileToSystemProfileChange, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The time profile is changed for access permission '{0}'-'{1}' from time profile '{2}' to system time profile '{3}' on '{4}'", accessRight, securityGroup, oldTimeProfileName, newSystemTimeProfileName, objectName);
		logEntryAudit.Arguments = new string[5] { accessRight, securityGroup, oldTimeProfileName, newSystemTimeProfileName, objectName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecuritySystemTimeProfileToProfileChange(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, string oldSystemTimeProfileName, string newTimeProfileName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToProfileChange, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The time profile is changed for access permission '{0}'-'{1}' from system time profile '{2}' to time profile '{3}' on '{4}'", accessRight, securityGroup, oldSystemTimeProfileName, newTimeProfileName, objectName);
		logEntryAudit.Arguments = new string[5] { accessRight, securityGroup, oldSystemTimeProfileName, newTimeProfileName, objectName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, string oldSystemTimeProfileName, string newSystemTimeProfileName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The time profile is changed for access permission '{0}'-'{1}' from system time profile '{2}' to system time profile '{3}' on '{4}'", accessRight, securityGroup, oldSystemTimeProfileName, newSystemTimeProfileName, objectName);
		logEntryAudit.Arguments = new string[5] { accessRight, securityGroup, oldSystemTimeProfileName, newSystemTimeProfileName, objectName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, int newRestrictedDays, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The restriction is changed for access permission '{0}'-'{1}' from no restriction to be restricted '{2}' days on '{3}'", accessRight, securityGroup, newRestrictedDays.ToString(CultureInfo.InvariantCulture), objectName);
		logEntryAudit.Arguments = new string[4]
		{
			accessRight,
			securityGroup,
			newRestrictedDays.ToString(CultureInfo.InvariantCulture),
			objectName
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, int newRestrictedMins, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The restriction is changed for access permission '{0}'-'{1}' from no restriction to be restricted '{2}' minutes on '{3}'", accessRight, securityGroup, newRestrictedMins.ToString(CultureInfo.InvariantCulture), objectName);
		logEntryAudit.Arguments = new string[4]
		{
			accessRight,
			securityGroup,
			newRestrictedMins.ToString(CultureInfo.InvariantCulture),
			objectName
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, int oldRestrictedMins, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' minutes to be without any restriction on '{3}'", accessRight, securityGroup, oldRestrictedMins.ToString(CultureInfo.InvariantCulture), objectName);
		logEntryAudit.Arguments = new string[4]
		{
			accessRight,
			securityGroup,
			oldRestrictedMins.ToString(CultureInfo.InvariantCulture),
			objectName
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, int oldRestrictedMins, int newRestrictedMins, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' minutes to restricted '{3}' minutes on '{4}'", accessRight, securityGroup, oldRestrictedMins.ToString(CultureInfo.InvariantCulture), newRestrictedMins.ToString(CultureInfo.InvariantCulture), objectName);
		logEntryAudit.Arguments = new string[5]
		{
			accessRight,
			securityGroup,
			oldRestrictedMins.ToString(CultureInfo.InvariantCulture),
			newRestrictedMins.ToString(CultureInfo.InvariantCulture),
			objectName
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, int oldRestrictedMins, int newRestrictedDays, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' minutes to restricted '{3}' days on '{4}'", accessRight, securityGroup, oldRestrictedMins.ToString(CultureInfo.InvariantCulture), newRestrictedDays.ToString(CultureInfo.InvariantCulture), objectName);
		logEntryAudit.Arguments = new string[5]
		{
			accessRight,
			securityGroup,
			oldRestrictedMins.ToString(CultureInfo.InvariantCulture),
			newRestrictedDays.ToString(CultureInfo.InvariantCulture),
			objectName
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, int oldRestrictedDays, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' days to be without any restriction on '{3}'", accessRight, securityGroup, oldRestrictedDays.ToString(CultureInfo.InvariantCulture), objectName);
		logEntryAudit.Arguments = new string[4]
		{
			accessRight,
			securityGroup,
			oldRestrictedDays.ToString(CultureInfo.InvariantCulture),
			objectName
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, int oldRestrictedDays, int newRestrictedMins, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' days to restricted '{3}' minutes on '{4}'", accessRight, securityGroup, oldRestrictedDays.ToString(CultureInfo.InvariantCulture), newRestrictedMins.ToString(CultureInfo.InvariantCulture), objectName);
		logEntryAudit.Arguments = new string[5]
		{
			accessRight,
			securityGroup,
			oldRestrictedDays.ToString(CultureInfo.InvariantCulture),
			newRestrictedMins.ToString(CultureInfo.InvariantCulture),
			objectName
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string objectName, int oldRestrictedDays, int newRestrictedDays, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The restriction is changed for access permission '{0}'-'{1}' from restricted '{2}' days to restricted '{3}' days on '{4}'", accessRight, securityGroup, oldRestrictedDays.ToString(CultureInfo.InvariantCulture), newRestrictedDays.ToString(CultureInfo.InvariantCulture), objectName);
		logEntryAudit.Arguments = new string[5]
		{
			accessRight,
			securityGroup,
			oldRestrictedDays.ToString(CultureInfo.InvariantCulture),
			newRestrictedDays.ToString(CultureInfo.InvariantCulture),
			objectName
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityPriorityChanged(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string oldPriority, string newPriority, string deviceName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityPriorityChanged, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The priority is changed for access permission '{0}'-'{1}' from '{2}' to '{3}' on '{4}'", accessRight, securityGroup, oldPriority, newPriority, deviceName);
		logEntryAudit.Arguments = new string[5] { accessRight, securityGroup, oldPriority, newPriority, deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityPriorityGranted(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string deviceName, string defaultPriority, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityPriorityGranted, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The access permission '{0}'-'{1}' is granted on '{2}' with default priority '{3}'", accessRight, securityGroup, deviceName, defaultPriority);
		logEntryAudit.Arguments = new string[4] { accessRight, securityGroup, deviceName, defaultPriority };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityPriorityRevoked(Guid roleId, string roleName, string managementServerName, string accessRight, string securityGroup, string deviceName, string defaultPriority, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityPriorityRevoked, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The access permission '{0}'-'{1}' is revoked on '{2}' with default priority '{3}'", accessRight, securityGroup, deviceName, defaultPriority);
		logEntryAudit.Arguments = new string[4] { accessRight, securityGroup, deviceName, defaultPriority };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleClientProfileChanged(string managementServerName, string oemClientName, string oldClientProfileName, string newClientProfileName, string roleName, string roleId, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleClientProfileChanged, locationIPAddress, SourceType.Server, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The {0} profile option is changed from '{1}' to '{2}' for the role '{3}'", oemClientName, oldClientProfileName, newClientProfileName, roleName);
		logEntryAudit.Arguments = new string[4] { oemClientName, oldClientProfileName, newClientProfileName, roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleDefaultTimeProfileChanged(string managementServerName, string oldTimeProfileName, string newTimeProfileName, string roleName, string roleId, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleDefaultTimeProfileChanged, locationIPAddress, SourceType.Server, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The Default time profile option is changed from '{0}' to '{1}' for the role '{2}'", oldTimeProfileName, newTimeProfileName, roleName);
		logEntryAudit.Arguments = new string[3] { oldTimeProfileName, newTimeProfileName, roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleOnlyAllowLoginWithinTimeProfileChanged(string managementServerName, string oldTimeProfileName, string newTimeProfileName, string roleName, string roleId, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleOnlyAllowLoginWithinTimeProfileChanged, locationIPAddress, SourceType.Server, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The Only allow login within time profile option is changed from '{0}' to '{1}' for the role '{2}'", oldTimeProfileName, newTimeProfileName, roleName);
		logEntryAudit.Arguments = new string[3] { oldTimeProfileName, newTimeProfileName, roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleClientLoginGranted(string managementServerName, string oemClientName, string roleName, string roleId, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleClientLoginGranted, locationIPAddress, SourceType.Server, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The Allow {0} login option is granted for the role '{1}'", oemClientName, roleName);
		logEntryAudit.Arguments = new string[2] { oemClientName, roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleClientLoginRevoked(string managementServerName, string oemClientName, string roleName, string roleId, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleClientLoginRevoked, locationIPAddress, SourceType.Server, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The Allow {0} login option is revoked for the role '{1}'", oemClientName, roleName);
		logEntryAudit.Arguments = new string[2] { oemClientName, roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleLoginAuthorizationRequiredGranted(string managementServerName, string roleName, string roleId, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleLoginAuthorizationRequiredGranted, locationIPAddress, SourceType.Server, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The Login authorization required option is granted for the role '{0}'", roleName);
		logEntryAudit.Arguments = new string[1] { roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleLoginAuthorizationRequiredRevoked(string managementServerName, string roleName, string roleId, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleLoginAuthorizationRequiredRevoked, locationIPAddress, SourceType.Server, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The Login authorization required option is revoked for the role '{0}'", roleName);
		logEntryAudit.Arguments = new string[1] { roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleAnonymityDuringPtzSessionsGranted(string managementServerName, string roleName, string roleId, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleAnonymityDuringPtzSessionsGranted, locationIPAddress, SourceType.Server, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The Make users anonymous during PTZ sessions option is granted for the role '{0}'", roleName);
		logEntryAudit.Arguments = new string[1] { roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleAnonymityDuringPtzSessionsRevoked(string managementServerName, string roleName, string roleId, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleAnonymityDuringPtzSessionsRevoked, locationIPAddress, SourceType.Server, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The Make users anonymous during PTZ sessions option is revoked for the role '{0}'", roleName);
		logEntryAudit.Arguments = new string[1] { roleName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleOverallSecurityChangeAccessDenied(Guid roleId, string roleName, string managementServerName, string securityGroup, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleOverallSecurityChangeAccessDenied, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The '{0}' access permissions could not be changed due to insufficient user permissions.", securityGroup);
		logEntryAudit.Arguments = new string[1] { securityGroup };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleObjectSecurityChangeAccessDenied(Guid roleId, string roleName, string managementServerName, string securityGroup, string objectName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleObjectSecurityChangeAccessDenied, locationIPAddress, SourceType.Role, roleName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The '{0}' access permissions on '{1}' could not be changed due to insufficient user permissions.", securityGroup, objectName);
		logEntryAudit.Arguments = new string[2] { securityGroup, objectName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleClaimAdded(Guid roleId, string roleName, string claimProvider, string claimName, string claimValue, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleClaimAdded, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Claim '{2}' with value '{3}' from external IDP '{1}' was added to the role '{0}'", roleName, claimProvider, claimName, claimValue);
		logEntryAudit.Arguments = new string[4] { roleName, claimProvider, claimName, claimValue };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRoleClaimRemoved(Guid roleId, string roleName, string claimProvider, string claimName, string claimValue, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRoleClaimRemoved, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.Roles, roleName, managementServerName, roleId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Claim '{2}' with value '{3}' from external IDP '{1}' was removed from the role '{0}'", roleName, claimProvider, claimName, claimValue);
		logEntryAudit.Arguments = new string[4] { roleName, claimProvider, claimName, claimValue };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditLoginProviderCreated(Guid loginProviderId, string loginProviderName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditLoginProviderCreated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, loginProviderName, managementServerName, loginProviderId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "External IDP '{0}' was created.", loginProviderName);
		logEntryAudit.Arguments = new string[1] { loginProviderName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditLoginProviderUpdated(Guid loginProviderId, string loginProviderName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditLoginProviderUpdated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, loginProviderName, managementServerName, loginProviderId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Settings for the external IDP '{0}' were changed.", loginProviderName);
		logEntryAudit.Arguments = new string[1] { loginProviderName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditLoginProviderDeleted(Guid loginProviderId, string loginProviderName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditLoginProviderDeleted, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, loginProviderName, managementServerName, loginProviderId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "External IDP '{0}' was deleted.", loginProviderName);
		logEntryAudit.Arguments = new string[1] { loginProviderName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditClaimCreated(int claimId, string claimName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditClaimCreated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, claimName, managementServerName, claimId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Claim '{0}' was created.", claimName);
		logEntryAudit.Arguments = new string[1] { claimName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditClaimUpdated(int claimId, string claimName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditClaimUpdated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, claimName, managementServerName, claimId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Claim '{0}' was changed.", claimName);
		logEntryAudit.Arguments = new string[1] { claimName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditClaimDeleted(int claimId, string claimName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditClaimDeleted, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, claimName, managementServerName, claimId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Claim '{0}' was deleted.", claimName);
		logEntryAudit.Arguments = new string[1] { claimName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditClientRedirectUrisSet(string clientId, string uriValues, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditClientRedirectUrisSet, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, uriValues, managementServerName, clientId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "'{0}' URIs were set to '{1}'.", clientId, uriValues);
		logEntryAudit.Arguments = new string[2] { clientId, uriValues };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditTrustedIssuerCreated(int id, string issuer, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditTrustedIssuerCreated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, issuer, managementServerName, id.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The trusted issuer {0} is created.", issuer);
		logEntryAudit.Arguments = new string[1] { issuer };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditTrustedIssuerUpdated(int id, string issuer, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditTrustedIssuerUpdated, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, issuer, managementServerName, id.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The trusted issuer {0} is updated.", issuer);
		logEntryAudit.Arguments = new string[1] { issuer };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditTrustedIssuerDeleted(int id, string issuer, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditTrustedIssuerDeleted, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, issuer, managementServerName, id.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The trusted issuer {0} is deleted.", issuer);
		logEntryAudit.Arguments = new string[1] { issuer };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditAuthorizeRecorder(Guid recordingServerId, string recordingServerName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditAuthorizeRecorder, locationIPAddress, SourceType.Server, recordingServerName, userCredentials, ResourceType.RecordingServers, recordingServerName, managementServerName, recordingServerId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "A user has authorized the recording server: {0}.", recordingServerName);
		logEntryAudit.Arguments = new string[1] { recordingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditCreateRecordingServer(Guid recordingServerId, string recordingServerName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditCreateRecordingServer, locationIPAddress, SourceType.Server, recordingServerName, userCredentials, ResourceType.RecordingServers, recordingServerName, managementServerName, recordingServerId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User has created the recording server: {0}.", recordingServerName);
		logEntryAudit.Arguments = new string[1] { recordingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceGisPointChanged(Guid deviceId, string deviceName, string newGisPoint, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceGisPointChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "GPS coordinates of device '{0}' changed to '{1}'", deviceName, newGisPoint);
		logEntryAudit.Arguments = new string[2] { deviceName, newGisPoint };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceShortNameChanged(Guid deviceId, string deviceName, string shortName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceShortNameChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Short name of device '{0}' changed to '{1}'", deviceName, shortName);
		logEntryAudit.Arguments = new string[2] { deviceName, shortName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceShortcutChanged(Guid deviceId, string deviceName, string shortcut, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceShortcutChanged, locationIPAddress, SourceType.Server, shortcut, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Shortcut of device '{0}' changed to '{1}'", deviceName, shortcut);
		logEntryAudit.Arguments = new string[2] { deviceName, shortcut };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceFailoverSettingChanged(Guid deviceId, string deviceName, string failoverSetting, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceFailoverSettingChanged, locationIPAddress, SourceType.Server, failoverSetting, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Failover settings of device '{0}' changed to '{1}'", deviceName, failoverSetting);
		logEntryAudit.Arguments = new string[2] { deviceName, failoverSetting };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceIconChanged(Guid deviceId, string deviceName, string icon, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceIconChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Icon identifier for device '{0}' changed to '{1}'", deviceName, icon);
		logEntryAudit.Arguments = new string[2] { deviceName, icon };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceCoverageDirectionChanged(Guid deviceId, string deviceName, string coverageDirection, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceCoverageDirectionChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Direction of device '{0}' changed to '{1}' degrees", deviceName, coverageDirection);
		logEntryAudit.Arguments = new string[2] { deviceName, coverageDirection };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceCoverageDepthChanged(Guid deviceId, string deviceName, string coverageDepth, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceCoverageDepthChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Depth of device '{0}' changed to '{1}' meters", deviceName, coverageDepth);
		logEntryAudit.Arguments = new string[2] { deviceName, coverageDepth };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceCoverageFieldOfViewChanged(Guid deviceId, string deviceName, string coverageFieldOfView, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceCoverageFieldOfViewChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Field of view of device '{0}' changed to '{1}' degrees", deviceName, coverageFieldOfView);
		logEntryAudit.Arguments = new string[2] { deviceName, coverageFieldOfView };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceMulticastEnabledChanged(Guid deviceId, string deviceName, string multicastEnabled, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceMulticastEnabledChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Device multicast changed on '{0}' to '{1}'", deviceName, multicastEnabled);
		logEntryAudit.Arguments = new string[2] { deviceName, multicastEnabled };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDevicePtzManuelSessionTimeoutChanged(Guid deviceId, string deviceName, string ptzManuelSessionTimeout, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDevicePtzManuelSessionTimeoutChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Timeout for manual PTZ session changed on '{0}' to '{1}'", deviceName, ptzManuelSessionTimeout);
		logEntryAudit.Arguments = new string[2] { deviceName, ptzManuelSessionTimeout };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDevicePtzReservedSessionTimeoutChanged(Guid deviceId, string deviceName, string ptzReservedSessionTimeout, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDevicePtzReservedSessionTimeoutChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Timeout for reserved PTZ session changed on '{0}' to '{1}'", deviceName, ptzReservedSessionTimeout);
		logEntryAudit.Arguments = new string[2] { deviceName, ptzReservedSessionTimeout };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDevicePtzExtendedManualSessionTimeoutChanged(Guid deviceId, string deviceName, string ptzExtendedManualSessionTimeout, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDevicePtzExtendedManualSessionTimeoutChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Timeout for pause patrolling session changed on '{0}' to '{1}'", deviceName, ptzExtendedManualSessionTimeout);
		logEntryAudit.Arguments = new string[2] { deviceName, ptzExtendedManualSessionTimeout };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecorderRemoved(Guid recordingServerId, string recordingServerName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecorderRemoved, locationIPAddress, SourceType.Server, recordingServerName, userCredentials, ResourceType.RecordingServers, recordingServerName, managementServerName, recordingServerId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Recording server '{0}' removed", recordingServerName);
		logEntryAudit.Arguments = new string[1] { recordingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecorderRenamed(Guid recordingServerId, string recordingServerName, string newRecordingServerName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecorderRenamed, locationIPAddress, SourceType.Server, recordingServerName, userCredentials, ResourceType.RecordingServers, recordingServerName, managementServerName, recordingServerId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Recording server '{0}' renamed to '{1}'", recordingServerName, newRecordingServerName);
		logEntryAudit.Arguments = new string[2] { recordingServerName, newRecordingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDirectoryCreatedOnRecorder(Guid recordingServerId, string recordingServerName, string fullDirectoryName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDirectoryCreatedOnRecorder, locationIPAddress, SourceType.Server, recordingServerName, userCredentials, ResourceType.RecordingServers, recordingServerName, managementServerName, recordingServerId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "New directory created on recording server '{0}': '{1}'", recordingServerName, fullDirectoryName);
		logEntryAudit.Arguments = new string[2] { recordingServerName, fullDirectoryName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallCreated(Guid videoWallId, string videoWallName, string managementServerName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallCreated, locationIPAddress, SourceType.Server, videoWallName, userCredentials, ResourceType.SmartWall, videoWallName, managementServerName, videoWallId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' created", oemSmartWallName, videoWallName);
		logEntryAudit.Arguments = new string[2] { oemSmartWallName, videoWallName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallDeleted(Guid videoWallId, string videoWallName, string managementServerName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallDeleted, locationIPAddress, SourceType.Server, videoWallName, userCredentials, ResourceType.SmartWall, videoWallName, managementServerName, videoWallId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' deleted", oemSmartWallName, videoWallName);
		logEntryAudit.Arguments = new string[2] { oemSmartWallName, videoWallName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallRenamed(Guid videoWallId, string videoWallName, string newVideoWallName, string managementServerName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallRenamed, locationIPAddress, SourceType.Server, videoWallName, userCredentials, ResourceType.SmartWall, videoWallName, managementServerName, videoWallId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' renamed to '{2}'", oemSmartWallName, videoWallName, newVideoWallName);
		logEntryAudit.Arguments = new string[3] { oemSmartWallName, videoWallName, newVideoWallName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallPresetAdded(Guid videoWallPresetId, string videoWallPresetName, string videoWallName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallPresetAdded, locationIPAddress, SourceType.Server, videoWallPresetName, userCredentials, ResourceType.SmartWallPreset, videoWallPresetName, videoWallName, videoWallPresetId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} preset '{1}' added to the '{2}' {0}", oemSmartWallName, videoWallPresetName, videoWallName);
		logEntryAudit.Arguments = new string[3] { oemSmartWallName, videoWallPresetName, videoWallName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallPresetDeleted(Guid videoWallPresetId, string videoWallPresetName, string videoWallName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallPresetDeleted, locationIPAddress, SourceType.Server, videoWallPresetName, userCredentials, ResourceType.SmartWallPreset, videoWallPresetName, videoWallName, videoWallPresetId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} preset '{1}' for the '{2}' {0} deleted", oemSmartWallName, videoWallPresetName, videoWallName);
		logEntryAudit.Arguments = new string[3] { oemSmartWallName, videoWallPresetName, videoWallName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorPresetEdited(Guid videoWallPresetId, string videoWallPresetName, string videoWallMonitorName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorPresetEdited, locationIPAddress, SourceType.Server, videoWallPresetName, userCredentials, ResourceType.SmartWallPreset, videoWallPresetName, videoWallMonitorName, videoWallPresetId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} preset '{1}' for the '{2}' {0} monitor edited", oemSmartWallName, videoWallPresetName, videoWallMonitorName);
		logEntryAudit.Arguments = new string[3] { oemSmartWallName, videoWallPresetName, videoWallMonitorName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallPresetRenamed(Guid videoWallPresetId, string videoWallPresetName, string newVideoWallPresetName, string videoWallName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallPresetRenamed, locationIPAddress, SourceType.Server, videoWallPresetName, userCredentials, ResourceType.SmartWallPreset, videoWallPresetName, videoWallName, videoWallPresetId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} preset '{1}' renamed to '{2}'", oemSmartWallName, videoWallPresetName, newVideoWallPresetName);
		logEntryAudit.Arguments = new string[3] { oemSmartWallName, videoWallPresetName, newVideoWallPresetName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallPresetActivate(Guid videoWallPresetId, string videoWallPresetName, string videoWallName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallPresetActivate, locationIPAddress, SourceType.Server, videoWallPresetName, userCredentials, ResourceType.SmartWallPreset, videoWallPresetName, videoWallName, videoWallPresetId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} preset '{1}' selected", oemSmartWallName, videoWallPresetName);
		logEntryAudit.Arguments = new string[2] { oemSmartWallName, videoWallPresetName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorAdded(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorAdded, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} monitor '{1}' created", oemSmartWallName, videoWallMonitorName);
		logEntryAudit.Arguments = new string[2] { oemSmartWallName, videoWallMonitorName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorDeleted(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorDeleted, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} monitor '{1}' deleted", oemSmartWallName, videoWallMonitorName);
		logEntryAudit.Arguments = new string[2] { oemSmartWallName, videoWallMonitorName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorRenamed(Guid videoWallMonitorId, string videoWallMonitorName, string newVideoWallMonitorName, string videoWallName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorRenamed, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} monitor '{1}' renamed to '{2}'", oemSmartWallName, videoWallMonitorName, newVideoWallMonitorName);
		logEntryAudit.Arguments = new string[3] { oemSmartWallName, videoWallMonitorName, newVideoWallMonitorName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorEdited(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorEdited, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} monitor '{1}' edited", oemSmartWallName, videoWallMonitorName);
		logEntryAudit.Arguments = new string[2] { oemSmartWallName, videoWallMonitorName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallEdited(Guid videoWallId, string videoWallName, string managementServerName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallEdited, locationIPAddress, SourceType.Server, videoWallName, userCredentials, ResourceType.SmartWall, videoWallName, managementServerName, videoWallId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} '{1}' edited", oemSmartWallName, videoWallName);
		logEntryAudit.Arguments = new string[2] { oemSmartWallName, videoWallName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMapCreated(Guid mapId, string mapName, string imageFileName, string mapServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMapCreated, locationIPAddress, SourceType.Server, mapServerName, userCredentials, ResourceType.Map, mapServerName, mapServerName, mapId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Map '{0}' created with this image '{1}'", mapName, imageFileName);
		logEntryAudit.Arguments = new string[2] { mapName, imageFileName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMapDeleted(Guid mapId, string mapName, string mapServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMapDeleted, locationIPAddress, SourceType.Server, mapServerName, userCredentials, ResourceType.Map, mapServerName, mapServerName, mapId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Map '{0}' deleted", mapName);
		logEntryAudit.Arguments = new string[1] { mapName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMapImageReplaced(Guid mapId, string mapName, string imageFileName, string mapServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMapImageReplaced, locationIPAddress, SourceType.Server, mapServerName, userCredentials, ResourceType.Map, mapServerName, mapServerName, mapId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Map image for map '{0}' replaced with this image '{1}'", mapName, imageFileName);
		logEntryAudit.Arguments = new string[2] { mapName, imageFileName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMapRenamed(Guid mapId, string mapName, string newMapName, string mapServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMapRenamed, locationIPAddress, SourceType.Server, mapServerName, userCredentials, ResourceType.Map, mapServerName, mapServerName, mapId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Map '{0}' renamed to '{1}'", mapName, newMapName);
		logEntryAudit.Arguments = new string[2] { mapName, newMapName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMapHotZoneLinkCreated(Guid mapId, string hotZoneMapName, string hostMapName, string mapServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMapHotZoneLinkCreated, locationIPAddress, SourceType.Server, mapServerName, userCredentials, ResourceType.Map, mapServerName, mapServerName, mapId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Hot zone link to map '{0}' created on '{1}'", hotZoneMapName, hostMapName);
		logEntryAudit.Arguments = new string[2] { hotZoneMapName, hostMapName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMapHotZoneLinkRemoved(Guid mapId, string hotZoneMapName, string hostMapName, string mapServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMapHotZoneLinkRemoved, locationIPAddress, SourceType.Server, mapServerName, userCredentials, ResourceType.Map, mapServerName, mapServerName, mapId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Hot zone link to map '{0}' removed from '{1}'", hotZoneMapName, hostMapName);
		logEntryAudit.Arguments = new string[2] { hotZoneMapName, hostMapName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMapDeviceServerPlaced(Guid mapId, string mapName, string serverDeviceName, string mapServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMapDeviceServerPlaced, locationIPAddress, SourceType.Server, mapServerName, userCredentials, ResourceType.Map, mapServerName, mapServerName, mapId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "'{0}' placed on map '{1}'", serverDeviceName, mapName);
		logEntryAudit.Arguments = new string[2] { serverDeviceName, mapName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMapDeviceServerRemoved(Guid mapId, string mapName, string serverDeviceName, string mapServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMapDeviceServerRemoved, locationIPAddress, SourceType.Server, mapServerName, userCredentials, ResourceType.Map, mapServerName, mapServerName, mapId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "'{0}' removed from map '{1}'", serverDeviceName, mapName);
		logEntryAudit.Arguments = new string[2] { serverDeviceName, mapName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditClientProfileCreated(Guid clientProfileId, string oemClientName, string clientProfileName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditClientProfileCreated, locationIPAddress, SourceType.Server, clientProfileName, userCredentials, ResourceType.ClientProfiles, clientProfileName, managementServerName, clientProfileId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The {0} profile '{1}' created.", oemClientName, clientProfileName);
		logEntryAudit.Arguments = new string[2] { oemClientName, clientProfileName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditClientProfileEdited(Guid clientProfileId, string oemClientName, string clientProfileName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditClientProfileEdited, locationIPAddress, SourceType.Server, clientProfileName, userCredentials, ResourceType.ClientProfiles, clientProfileName, managementServerName, clientProfileId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Settings for the {0} profile '{1}' changed.", oemClientName, clientProfileName);
		logEntryAudit.Arguments = new string[2] { oemClientName, clientProfileName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditClientProfileDeleted(Guid clientProfileId, string oemClientName, string clientProfileName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditClientProfileDeleted, locationIPAddress, SourceType.Server, clientProfileName, userCredentials, ResourceType.ClientProfiles, clientProfileName, managementServerName, clientProfileId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The {0} profile '{1}' deleted.", oemClientName, clientProfileName);
		logEntryAudit.Arguments = new string[2] { oemClientName, clientProfileName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditClientProfileRenamed(Guid clientProfileId, string oemClientName, string clientProfileName, string newClientProfileName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditClientProfileRenamed, locationIPAddress, SourceType.Server, clientProfileName, userCredentials, ResourceType.ClientProfiles, clientProfileName, managementServerName, clientProfileId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The {0} profile '{1}' renamed to '{2}'.", oemClientName, clientProfileName, newClientProfileName);
		logEntryAudit.Arguments = new string[3] { oemClientName, clientProfileName, newClientProfileName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditSiteSlaveCreated(Guid slaveSiteId, string slaveSiteName, string slaveSiteAddress, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSiteSlaveCreated, locationIPAddress, SourceType.Server, slaveSiteName, userCredentials, ResourceType.FederatedArchitecture, slaveSiteName, managementServerName, slaveSiteId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The site '{0}' on '{1}' was added to the hierarchy.", slaveSiteName, slaveSiteAddress);
		logEntryAudit.Arguments = new string[2] { slaveSiteName, slaveSiteAddress };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditSiteSlaveDeleted(Guid slaveSiteId, string slaveSiteName, string slaveSiteAddress, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSiteSlaveDeleted, locationIPAddress, SourceType.Server, slaveSiteName, userCredentials, ResourceType.FederatedArchitecture, slaveSiteName, managementServerName, slaveSiteId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The site '{0}' on '{1}' was detached from hierarchy.", slaveSiteName, slaveSiteAddress);
		logEntryAudit.Arguments = new string[2] { slaveSiteName, slaveSiteAddress };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditSiteMasterDeleted(Guid masterSiteId, string masterSiteName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditSiteMasterDeleted, locationIPAddress, SourceType.Server, masterSiteName, userCredentials, ResourceType.FederatedArchitecture, masterSiteName, managementServerName, masterSiteId.ToString(), AuditType.Management, AccessGranted.Granted)
		{
			Description = "Site detached from hierarchy."
		};
	}

	public static LogEntryAudit AuditSiteMasterAuthorized(Guid masterSiteId, string masterSiteName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSiteMasterAuthorized, locationIPAddress, SourceType.Server, masterSiteName, userCredentials, ResourceType.FederatedArchitecture, masterSiteName, managementServerName, masterSiteId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The site administrator accepted the inclusion of the site {0} in the hierarchy under {1}.", managementServerName, masterSiteName);
		logEntryAudit.Arguments = new string[2] { managementServerName, masterSiteName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditSiteMasterAuthorizedDenied(Guid masterSiteId, string masterSiteName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSiteMasterAuthorizedDenied, locationIPAddress, SourceType.Server, masterSiteName, userCredentials, ResourceType.FederatedArchitecture, masterSiteName, managementServerName, masterSiteId.ToString(), AuditType.Management, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The site administrator denied the inclusion of the site {0} in the hierarchy under {1}.", managementServerName, masterSiteName);
		logEntryAudit.Arguments = new string[2] { managementServerName, masterSiteName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditSiteRenamed(Guid siteId, string managementServerName, string newManagementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSiteRenamed, locationIPAddress, SourceType.Server, newManagementServerName, userCredentials, ResourceType.ManagementServer, newManagementServerName, newManagementServerName, siteId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Surveillance system '{0}' renamed to '{1}'.", managementServerName, newManagementServerName);
		logEntryAudit.Arguments = new string[2] { managementServerName, newManagementServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecordingStorageCreated(Guid storageId, string recordingStorageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecordingStorageCreated, locationIPAddress, SourceType.Server, recordingStorageName, userCredentials, ResourceType.Storage, recordingStorageName, recordingServerName, storageId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Storage '{0}' added.", recordingStorageName);
		logEntryAudit.Arguments = new string[1] { recordingStorageName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecordingStorageEdited(Guid storageId, string recordingStorageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecordingStorageEdited, locationIPAddress, SourceType.Server, recordingStorageName, userCredentials, ResourceType.Storage, recordingStorageName, recordingServerName, storageId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Storage settings in '{0}' edited.", recordingStorageName);
		logEntryAudit.Arguments = new string[1] { recordingStorageName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecordingStorageDeleted(Guid storageId, string recordingStorageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecordingStorageDeleted, locationIPAddress, SourceType.Server, recordingStorageName, userCredentials, ResourceType.Storage, recordingStorageName, recordingServerName, storageId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Storage '{0}' deleted.", recordingStorageName);
		logEntryAudit.Arguments = new string[1] { recordingStorageName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecordingStorageMoved(Guid storageId, string recordingStorageName, string oldPath, string newPath, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecordingStorageMoved, locationIPAddress, SourceType.Server, recordingStorageName, userCredentials, ResourceType.Storage, recordingStorageName, recordingServerName, storageId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Storage '{0}' moved from '{1}' to '{2}'.", recordingStorageName, oldPath, newPath);
		logEntryAudit.Arguments = new string[3] { recordingStorageName, oldPath, newPath };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecordingStorageAddArchive(Guid storageId, string archiveStorageName, string recordingStorageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecordingStorageAddArchive, locationIPAddress, SourceType.Server, recordingStorageName, userCredentials, ResourceType.Storage, recordingStorageName, recordingServerName, storageId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Archive '{0}' added to '{1}'.", archiveStorageName, recordingStorageName);
		logEntryAudit.Arguments = new string[2] { archiveStorageName, recordingStorageName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecordingStorageEditArchive(Guid storageId, string archiveStorageName, string recordingStorageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecordingStorageEditArchive, locationIPAddress, SourceType.Server, recordingStorageName, userCredentials, ResourceType.Storage, recordingStorageName, recordingServerName, storageId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Archive '{0}' in '{1}' edited.", archiveStorageName, recordingStorageName);
		logEntryAudit.Arguments = new string[2] { archiveStorageName, recordingStorageName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecordingStorageArchiveMoved(Guid storageId, string archiveStorageName, string recordingStorageName, string oldPath, string newPath, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecordingStorageArchiveMoved, locationIPAddress, SourceType.Server, recordingStorageName, userCredentials, ResourceType.Storage, recordingStorageName, recordingServerName, storageId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Archive '{0}' in '{1}' moved from '{2}' to '{3}'.", archiveStorageName, recordingStorageName, oldPath, newPath);
		logEntryAudit.Arguments = new string[4] { archiveStorageName, recordingStorageName, oldPath, newPath };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecordingStorageDeleteArchive(Guid storageId, string archiveStorageName, string recordingStorageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecordingStorageDeleteArchive, locationIPAddress, SourceType.Server, recordingStorageName, userCredentials, ResourceType.Storage, recordingStorageName, recordingServerName, storageId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Archive '{0}' deleted from '{1}'.", archiveStorageName, recordingStorageName);
		logEntryAudit.Arguments = new string[2] { archiveStorageName, recordingStorageName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecordingStorageArchiveMounted(Guid storageId, string archiveStorageName, string recordingStorageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecordingStorageArchiveMounted, locationIPAddress, SourceType.Server, recordingStorageName, userCredentials, ResourceType.Storage, recordingStorageName, recordingServerName, storageId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Archive '{0}' in '{1}' has been mounted.", archiveStorageName, recordingStorageName);
		logEntryAudit.Arguments = new string[2] { archiveStorageName, recordingStorageName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRecordingStorageArchiveUnmounted(Guid storageId, string archiveStorageName, string recordingStorageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecordingStorageArchiveUnmounted, locationIPAddress, SourceType.Server, recordingStorageName, userCredentials, ResourceType.Storage, recordingStorageName, recordingServerName, storageId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Archive '{0}' in '{1}' has been unmounted.", archiveStorageName, recordingStorageName);
		logEntryAudit.Arguments = new string[2] { archiveStorageName, recordingStorageName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRecordingStorageSelected(Guid deviceId, string deviceName, ResourceType deviceType, string recordingStorageName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRecordingStorageSelected, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Storage '{0}' selected for recording on '{1}'.", recordingStorageName, deviceName);
		logEntryAudit.Arguments = new string[2] { recordingStorageName, deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRecordingEnabled(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRecordingEnabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Recording enabled on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRecordingDisabled(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRecordingDisabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Recording disabled on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRecordingPrebufferEnabled(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRecordingPrebufferEnabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Pre-buffer enabled on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRecordingPrebufferDisabled(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRecordingPrebufferDisabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Pre-buffer disabled on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRecordingEdgeStorageEnabled(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRecordingEdgeStorageEnabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Remote recording enabled on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRecordingEdgeStorageDisabled(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRecordingEdgeStorageDisabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Remote recording disabled on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRecordingEdgeStoragePlaybackEnabled(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRecordingEdgeStoragePlaybackEnabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Playback of remote recording enabled on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRecordingEdgeStoragePlaybackDisabled(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRecordingEdgeStoragePlaybackDisabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Playback of remote recording disabled on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDeviceRequestEdgeStorageRetrieval(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDeviceRequestEdgeStorageRetrieval, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Retrieval of remote recordings requested on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDevice360LensSettingsEnabled(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDevice360LensSettingsEnabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Fisheye lens settings enabled on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDevice360LensSettingsDisabled(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDevice360LensSettingsDisabled, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Fisheye lens settings disabled on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditDevice360LensSettingsChanged(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditDevice360LensSettingsChanged, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Fisheye lens settings changed on '{0}'.", deviceName);
		logEntryAudit.Arguments = new string[1] { deviceName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditConfigurationExported(string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditConfigurationExported, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, Guid.Empty.ToString(), AuditType.Management, AccessGranted.Granted)
		{
			Description = "System configuration was exported."
		};
	}

	public static LogEntryAudit AuditConfigurationRestored(string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditConfigurationRestored, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, Guid.Empty.ToString(), AuditType.Management, AccessGranted.Granted)
		{
			Description = "System configuration was restored."
		};
	}

	public static LogEntryAudit AuditHostingServerAdded(int hostingServerId, string hostingServerName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHostingServerAdded, locationIPAddress, SourceType.Server, hostingServerName, userCredentials, ResourceType.RemoteConnectServer, hostingServerName, managementServerName, hostingServerId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Remote connect server '{0}' added.", hostingServerName);
		logEntryAudit.Arguments = new string[1] { hostingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHostingServerModified(int hostingServerId, string hostingServerName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHostingServerModified, locationIPAddress, SourceType.Server, hostingServerName, userCredentials, ResourceType.RemoteConnectServer, hostingServerName, managementServerName, hostingServerId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Remote connect server '{0}' modified.", hostingServerName);
		logEntryAudit.Arguments = new string[1] { hostingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHostingServerDeleted(int hostingServerId, string hostingServerName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHostingServerDeleted, locationIPAddress, SourceType.Server, hostingServerName, userCredentials, ResourceType.RemoteConnectServer, hostingServerName, managementServerName, hostingServerId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Remote connect server '{0}' removed.", hostingServerName);
		logEntryAudit.Arguments = new string[1] { hostingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHostedHardwareAdded(int hostedHardwareId, string hostedHardwareName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHostedHardwareAdded, locationIPAddress, SourceType.Server, hostedHardwareName, userCredentials, ResourceType.RemoteConnectHardware, hostedHardwareName, managementServerName, hostedHardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Remote connect hardware '{0}' added.", hostedHardwareName);
		logEntryAudit.Arguments = new string[1] { hostedHardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHostedHardwareModified(int hostedHardwareId, string hostedHardwareName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHostedHardwareModified, locationIPAddress, SourceType.Server, hostedHardwareName, userCredentials, ResourceType.RemoteConnectHardware, hostedHardwareName, managementServerName, hostedHardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Remote connect hardware '{0}' modified.", hostedHardwareName);
		logEntryAudit.Arguments = new string[1] { hostedHardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHostedHardwareRenamed(int hostedHardwareId, string hostedHardwareName, string newHostedHardwareName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHostedHardwareRenamed, locationIPAddress, SourceType.Server, hostedHardwareName, userCredentials, ResourceType.RemoteConnectHardware, hostedHardwareName, managementServerName, hostedHardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Remote connect hardware '{0}' renamed to '{1}'.", hostedHardwareName, newHostedHardwareName);
		logEntryAudit.Arguments = new string[2] { hostedHardwareName, newHostedHardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHostedHardwareDeleted(int hostedHardwareId, string hostedHardwareName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHostedHardwareDeleted, locationIPAddress, SourceType.Server, hostedHardwareName, userCredentials, ResourceType.RemoteConnectHardware, hostedHardwareName, managementServerName, hostedHardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Remote connect hardware '{0}' removed.", hostedHardwareName);
		logEntryAudit.Arguments = new string[1] { hostedHardwareName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHostingServerRenamed(int hostingServerId, string hostingServerName, string newHostingServerName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHostingServerRenamed, locationIPAddress, SourceType.Server, hostingServerName, userCredentials, ResourceType.RemoteConnectServer, hostingServerName, managementServerName, hostingServerId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Remote connect server '{0}' renamed to '{1}'.", hostingServerName, newHostingServerName);
		logEntryAudit.Arguments = new string[2] { hostingServerName, newHostingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditConfigReportExported(string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditConfigReportExported, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, Guid.Empty.ToString(), AuditType.Management, AccessGranted.Granted)
		{
			Description = "Configuration report exported."
		};
	}

	public static LogEntryAudit AuditBasicUserCreated(string sid, string userName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserCreated, locationIPAddress, SourceType.Server, userName, userCredentials, ResourceType.BasicUser, userName, managementServerName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' created.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserDeleted(string sid, string userName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserDeleted, locationIPAddress, SourceType.Server, userName, userCredentials, ResourceType.BasicUser, userName, managementServerName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' deleted.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserEdited(string sid, string userName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserEdited, locationIPAddress, SourceType.Server, userName, userCredentials, ResourceType.BasicUser, userName, managementServerName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The settings for basic user '{0}' are changed.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserRenamed(string sid, string userName, string newUserName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserRenamed, locationIPAddress, SourceType.Server, userName, userCredentials, ResourceType.BasicUser, userName, managementServerName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' renamed to '{1}'.", userName, newUserName);
		logEntryAudit.Arguments = new string[2] { userName, newUserName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserPasswordChanged(string sid, string userName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserPasswordChanged, locationIPAddress, SourceType.Server, userName, userCredentials, ResourceType.BasicUser, userName, managementServerName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The password for basic user '{0}' is changed.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicInfoCreated(string id, string name, string details, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicInfoCreated, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.SiteInfo, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The site information '{0}' is created with value '{1}'", name, details);
		logEntryAudit.Arguments = new string[2] { name, details };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicInfoDeleted(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicInfoDeleted, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.SiteInfo, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The site information '{0}' is deleted.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicInfoEdited(string id, string name, string details, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicInfoEdited, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.SiteInfo, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The settings for the site information '{0}' is changed to '{1}'", name, details);
		logEntryAudit.Arguments = new string[2] { name, details };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserLockedOut(string sid, string userName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserLockedOut, locationIPAddress, SourceType.Server, userName, userCredentials, ResourceType.BasicUser, userName, managementServerName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' is locked out.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditRetentionOptionRoleRelationUpdated(string RoleName, string RetentionName, string managementServerName, string roleId, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRetentionOptionRoleRelationUpdated, locationIPAddress, SourceType.Server, RoleName, userCredentials, ResourceType.RetentionOptionRoleRelation, RoleName, managementServerName, roleId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The default evidence lock time option for the role '{0}' was changed to '{1}'", RoleName, RetentionName);
		logEntryAudit.Arguments = new string[2] { RoleName, RetentionName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserEnabled(string sid, string userName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserEnabled, locationIPAddress, SourceType.Server, userName, userCredentials, ResourceType.BasicUser, userName, managementServerName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' is now enabled.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserForcePasswordChange(string sid, string userName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserForcePasswordChange, locationIPAddress, SourceType.Server, userName, userCredentials, ResourceType.BasicUser, userName, managementServerName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' must change password.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMarkedDataUpdate(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMarkedDataUpdate, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.MarkedData, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The marked data (with evidence locks or bookmarks) '{0}' was updated.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMarkedDataDeleted(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMarkedDataDeleted, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.MarkedData, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Marked data (with evidence locks or bookmarks) '{0}' was deleted.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMarkedDataCreated(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMarkedDataCreated, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.MarkedData, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Marked data (with evidence locks or bookmarks) '{0}' was created.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPerfCountStateRuleCreated(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPerfCountStateRuleCreated, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.PerformanceCounterRule, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The system dashboard threshold '{0}' was created.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPerfCountStateRuleUpdated(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPerfCountStateRuleUpdated, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.PerformanceCounterRule, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The system dashboard threshold '{0}' was updated.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPerfCountStateRuleDeleted(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPerfCountStateRuleDeleted, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.PerformanceCounterRule, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The system dashboard threshold '{0}' was deleted.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditSystemDashboardConfigurationUpdated(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSystemDashboardConfigurationUpdated, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.SystemDashboardConfiguration, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The system dashboard configuration '{0}' was updated.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditSystemDashboardConfigurationDeleted(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSystemDashboardConfigurationDeleted, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.SystemDashboardConfiguration, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The system dashboard configuration '{0}' was deleted.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditSystemDashboardConfigurationCreated(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSystemDashboardConfigurationCreated, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.SystemDashboardConfiguration, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The system dashboard configuration '{0}' was created.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwareMoved(Guid hardwareId, string hardwareName, string fromRecordingServerName, string toRecordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwareMoved, locationIPAddress, SourceType.Server, hardwareName, userCredentials, ResourceType.Hardware, hardwareName, fromRecordingServerName, hardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User initiated a move of the hardware '{0}' to the recording server '{1}'.", hardwareName, toRecordingServerName);
		logEntryAudit.Arguments = new string[2] { hardwareName, toRecordingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwarePasswordChanged(Guid hardwareId, string hardwareName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwarePasswordChanged, locationIPAddress, SourceType.Server, hardwareName, userCredentials, ResourceType.Hardware, hardwareName, recordingServerName, hardwareId.ToString(), AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User initiated a password change of the hardware '{0}' on recording server '{1}'.", hardwareName, recordingServerName);
		logEntryAudit.Arguments = new string[2] { hardwareName, recordingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditAutomaticactivationEnabled(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditAutomaticactivationEnabled, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Management, AccessGranted.Granted)
		{
			Description = "Automatic activation is enabled."
		};
	}

	public static LogEntryAudit AuditAutomaticactivationDisabled(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditAutomaticactivationDisabled, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Management, AccessGranted.Granted)
		{
			Description = "Automatic activation is disabled."
		};
	}

	public static LogEntryAudit AuditAutomaticactivationCredentialsChanged(Guid resourceId, string managementServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditAutomaticactivationCredentialsChanged, locationIPAddress, SourceType.Server, managementServerName, userCredentials, ResourceType.ManagementServer, managementServerName, managementServerName, resourceId.ToString(), AuditType.Management, AccessGranted.Granted)
		{
			Description = "The credentials for online activation have been changed."
		};
	}

	public static LogEntryAudit AuditAudioMessageCreated(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditAudioMessageCreated, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.AudioMessage, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Audio message '{0}' created.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditAudioMessageContentChanged(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditAudioMessageContentChanged, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.AudioMessage, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The content of the audio message '{0}' is changed.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditAudioMessageDeleted(string id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditAudioMessageDeleted, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.AudioMessage, name, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Audio message '{0}' deleted.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwarePreConfigurationStarted(string hardwareId, string hardwareAddress, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwarePreConfigurationStarted, locationIPAddress, SourceType.Server, recordingServerName, userCredentials, ResourceType.Hardware, hardwareAddress, recordingServerName, hardwareId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User initiated pre-configuration of credentials for hardware '{0}' on recording server '{1}'.", hardwareAddress, recordingServerName);
		logEntryAudit.Arguments = new string[2] { hardwareAddress, recordingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditHardwareFirmwareUpdateStarted(string hardwareId, string hardwareAddress, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditHardwareFirmwareUpdateStarted, locationIPAddress, SourceType.Server, recordingServerName, userCredentials, ResourceType.Hardware, hardwareAddress, recordingServerName, hardwareId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User initiated firmware update for hardware '{0}' on recording server '{1}'.", hardwareAddress, recordingServerName);
		logEntryAudit.Arguments = new string[2] { hardwareAddress, recordingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditFileTransferToRecorderStarted(string fileId, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditFileTransferToRecorderStarted, locationIPAddress, SourceType.Server, recordingServerName, userCredentials, ResourceType.Storage, fileId, recordingServerName, fileId, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User initiated transfer of file '{0}' to recording server '{1}'.", fileId, recordingServerName);
		logEntryAudit.Arguments = new string[2] { fileId, recordingServerName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditFileUploadStarted(string targetFileName, string size, string managementClientName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditFileUploadStarted, locationIPAddress, SourceType.Server, managementClientName, userCredentials, ResourceType.Storage, managementClientName, managementClientName, managementClientName, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User initiated a file upload to the server. Target file name '{0}', size {1}.", targetFileName, size);
		logEntryAudit.Arguments = new string[2] { targetFileName, size };
		return logEntryAudit;
	}

	public static LogEntryAudit MatrixCreated(string id, string matrixName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.MatrixCreated, locationIPAddress, SourceType.Server, matrixName, userCredentials, ResourceType.Matrix, matrixName, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Matrix created: '{0}' ", matrixName);
		logEntryAudit.Arguments = new string[1] { matrixName };
		return logEntryAudit;
	}

	public static LogEntryAudit MatrixUpdated(string id, string matrixName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.MatrixUpdated, locationIPAddress, SourceType.Server, matrixName, userCredentials, ResourceType.Matrix, matrixName, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Matrix updated: '{0}' ", matrixName);
		logEntryAudit.Arguments = new string[1] { matrixName };
		return logEntryAudit;
	}

	public static LogEntryAudit MatrixDeleted(string id, string matrixName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.MatrixDeleted, locationIPAddress, SourceType.Server, matrixName, userCredentials, ResourceType.Matrix, matrixName, managementServerName, id, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Matrix deleted: '{0}' ", matrixName);
		logEntryAudit.Arguments = new string[1] { matrixName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditClientLiveFeedRequested(Guid deviceId, string deviceName, ResourceType deviceType, AuditType auditType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditClientLiveFeedRequested, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), auditType, AccessGranted.Granted)
		{
			Description = "Client live feed requested."
		};
	}

	public static LogEntryAudit AuditClientLiveFeedTerminated(Guid deviceId, string deviceName, ResourceType deviceType, AuditType auditType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditClientLiveFeedTerminated, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), auditType, AccessGranted.Granted)
		{
			Description = "Client live feed terminated."
		};
	}

	public static LogEntryAudit AuditLiveCameraStarted(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveCameraStarted, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveVideo, AccessGranted.Granted)
		{
			Description = "Client live feed requested"
		};
	}

	public static LogEntryAudit AuditLiveCameraEnded(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveCameraEnded, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveVideo, AccessGranted.Granted)
		{
			Description = "Client live feed terminated"
		};
	}

	public static LogEntryAudit AuditLiveCameraDenied(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveCameraDenied, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveVideo, AccessGranted.Denied)
		{
			Description = "Client live feed denied"
		};
	}

	public static LogEntryAudit AuditLiveCameraDeniedTimeBased(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveCameraDeniedTimeBased, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveVideo, AccessGranted.Denied)
		{
			Description = "Client live feed denied due to time based security restrictions"
		};
	}

	public static LogEntryAudit AuditLiveMicrophoneStarted(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveMicrophoneStarted, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Granted)
		{
			Description = "Client live feed requested"
		};
	}

	public static LogEntryAudit AuditLiveMicrophoneEnded(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveMicrophoneEnded, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Granted)
		{
			Description = "Client live feed terminated"
		};
	}

	public static LogEntryAudit AuditLiveMicrophoneDenied(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveMicrophoneDenied, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Denied)
		{
			Description = "Client live feed denied"
		};
	}

	public static LogEntryAudit AuditLiveSpeakToSpeakerStarted(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveSpeakToSpeakerStarted, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Granted)
		{
			Description = "Start speaking to speakers"
		};
	}

	public static LogEntryAudit AuditLiveSpeakToSpeakerEnded(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveSpeakToSpeakerEnded, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Granted)
		{
			Description = "Stop speaking to speakers"
		};
	}

	public static LogEntryAudit AuditLiveSpeakToSpeakerDenied(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveSpeakToSpeakerDenied, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Denied)
		{
			Description = "Speaking to speaker denied"
		};
	}

	public static LogEntryAudit AuditLiveSpeakerStarted(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveSpeakerStarted, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Granted)
		{
			Description = "Client live feed requested"
		};
	}

	public static LogEntryAudit AuditLiveSpeakerEnded(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveSpeakerEnded, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Granted)
		{
			Description = "Client live feed terminated"
		};
	}

	public static LogEntryAudit AuditLiveSpeakerDenied(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveSpeakerDenied, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Denied)
		{
			Description = "Client live feed denied"
		};
	}

	public static LogEntryAudit AuditLiveMicrophoneDeniedTimeBased(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveMicrophoneDeniedTimeBased, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Denied)
		{
			Description = "Client live feed denied due to time based security restrictions"
		};
	}

	public static LogEntryAudit AuditLiveSpeakToSpeakerDeniedTimeBased(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveSpeakToSpeakerDeniedTimeBased, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Denied)
		{
			Description = "Speech denied due to time based security restriction"
		};
	}

	public static LogEntryAudit AuditLiveSpeakerDeniedTimeBased(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveSpeakerDeniedTimeBased, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveAudio, AccessGranted.Denied)
		{
			Description = "Client live feed denied due to time based security restrictions"
		};
	}

	public static LogEntryAudit AuditLiveMetadataStarted(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveMetadataStarted, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveMetadata, AccessGranted.Granted)
		{
			Description = "Client live feed requested"
		};
	}

	public static LogEntryAudit AuditLiveMetadataEnded(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveMetadataEnded, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveMetadata, AccessGranted.Granted)
		{
			Description = "Client live feed terminated"
		};
	}

	public static LogEntryAudit AuditLiveMetadataDenied(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveMetadataDenied, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveMetadata, AccessGranted.Denied)
		{
			Description = "Client live feed denied"
		};
	}

	public static LogEntryAudit AuditLiveMetadataDeniedTimeBased(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditLiveMetadataDeniedTimeBased, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.LiveMetadata, AccessGranted.Denied)
		{
			Description = "Client live feed denied due to time based security restrictions"
		};
	}

	public static LogEntryAudit AuditRecordingSequencesRequested(Guid deviceId, string cameraName, int count, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditRecordingSequencesRequested, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.RecordingSequences, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Recording sequence list retrieved. {0} instances from {1} to {2} (UTC time)", count.ToString(CultureInfo.InvariantCulture), startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[3]
		{
			count.ToString(CultureInfo.InvariantCulture),
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditClientBrowseSequence(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, ResourceType deviceType, AuditType auditType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditClientBrowseSequence, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), auditType, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Playback of recorded sequence from '{0}' to '{1}' started.", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackCameraRecordings(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackCameraRecordings, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackVideo, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Playing back recorded images '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackCameraRecordingsDenied(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPlaybackCameraRecordingsDenied, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackVideo, AccessGranted.Denied)
		{
			Description = "The client was not allowed to play back video."
		};
	}

	public static LogEntryAudit AuditPlaybackCameraRecordingsDeniedTimeBased(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPlaybackCameraRecordingsDeniedTimeBased, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackVideo, AccessGranted.Denied)
		{
			Description = "The client was not allowed to play back video (time based)."
		};
	}

	public static LogEntryAudit AuditPlaybackCameraRecordingsOneLogOnly(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackCameraRecordingsOneLogOnly, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackVideo, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Retrieving recorded video in main track from '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackCameraEdgeRecordingsOneLogOnly(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackCameraEdgeRecordingsOneLogOnly, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackVideo, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Retrieving recorded video in edge track from '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackMicrophoneRecordings(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackMicrophoneRecordings, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackAudio, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Playing back recorded audio from microphones '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackSpeakerRecordings(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackSpeakerRecordings, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackAudio, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Playing back recorded audio from speakers '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackMicrophoneRecordingsDenied(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPlaybackMicrophoneRecordingsDenied, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackAudio, AccessGranted.Denied)
		{
			Description = "The client was not allowed to play back audio."
		};
	}

	public static LogEntryAudit AuditPlaybackSpeakerRecordingsDenied(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPlaybackSpeakerRecordingsDenied, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackAudio, AccessGranted.Denied)
		{
			Description = "The client was not allowed to play back audio."
		};
	}

	public static LogEntryAudit AuditPlaybackMicrophoneRecordingsDeniedTimeBased(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPlaybackMicrophoneRecordingsDeniedTimeBased, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackAudio, AccessGranted.Denied)
		{
			Description = "The client was not allowed to play back audio (time based)."
		};
	}

	public static LogEntryAudit AuditPlaybackSpeakerRecordingsDeniedTimeBased(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPlaybackSpeakerRecordingsDeniedTimeBased, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackAudio, AccessGranted.Denied)
		{
			Description = "The client was not allowed to play back audio"
		};
	}

	public static LogEntryAudit AuditPlaybackMicrophoneRecordingsOneLogOnly(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackMicrophoneRecordingsOneLogOnly, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackAudio, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Retrieving recorded audio in main track from '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackSpeakerRecordingsOneLogOnly(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackSpeakerRecordingsOneLogOnly, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackAudio, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Retrieving recorded audio in main track from '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackMicrophoneEdgeRecordingsOneLogOnly(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackMicrophoneEdgeRecordingsOneLogOnly, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackAudio, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Retrieving recorded audio in edge track from '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackSpeakerEdgeRecordingsOneLogOnly(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackSpeakerEdgeRecordingsOneLogOnly, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackAudio, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Retrieving recorded audio in edge track from '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackMetadataRecordings(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackMetadataRecordings, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackMetadata, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Playing back recorded metadata '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackMetadataRecordingsOneLogOnly(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackMetadataRecordingsOneLogOnly, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackMetadata, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Retrieving recorded metadata in main track from '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPlaybackMetadataRecordingsDenied(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPlaybackMetadataRecordingsDenied, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackMetadata, AccessGranted.Denied)
		{
			Description = "The client was not allowed to play back metadata."
		};
	}

	public static LogEntryAudit AuditPlaybackMetadataRecordingsDeniedTimeBased(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPlaybackMetadataRecordingsDeniedTimeBased, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackMetadata, AccessGranted.Denied)
		{
			Description = "The client was not allowed to play back metadata (time based)."
		};
	}

	public static LogEntryAudit AuditPlaybackMetadataEdgeRecordingsOneLogOnly(Guid deviceId, string deviceName, DateTime startTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPlaybackMetadataEdgeRecordingsOneLogOnly, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.PlaybackMetadata, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Retrieving recorded metadata in edge track from '{0}' to '{1}' (UTC time).", startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			startTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzMoveToPresetPosition(Guid deviceId, string cameraName, string presetName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzMoveToPresetPosition, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "PTZ preset position '{0}' activated", presetName);
		logEntryAudit.Arguments = new string[1] { presetName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzMoveCenterZoomAbsolutePositioned(Guid deviceId, string cameraName, string width, string height, string centerX, string centerY, string zoom, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzMoveCenterZoomAbsolutePositioned, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "PTZ center activated. Width '{0}', Height '{1}', Center-X '{2}', Center-Y '{3}', Zoom '{4}'", width, height, centerX, centerY, zoom);
		logEntryAudit.Arguments = new string[5] { width, height, centerX, centerY, zoom };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzMoveCenterZoomRelativePositioned(Guid deviceId, string cameraName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPtzMoveCenterZoomRelativePositioned, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted)
		{
			Description = "PTZ center activated. Relative location"
		};
	}

	public static LogEntryAudit AuditPtzRectangleZoom(Guid deviceId, string cameraName, string width, string height, string left, string right, string top, string bottom, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzRectangleZoom, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "PTZ rectangle activated. Width '{0}', Height '{1}', Left '{2}', Top '{3}', Right '{4}', Bottom '{5}'.", width, height, left, top, right, bottom);
		logEntryAudit.Arguments = new string[6] { width, height, left, top, right, bottom };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzMoveZoomButtonsActivated(Guid deviceId, string cameraName, string direction, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzMoveZoomButtonsActivated, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "PTZ move '{0}'", direction);
		logEntryAudit.Arguments = new string[1] { direction };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzMoveRelative(Guid deviceId, string cameraName, string direction, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzMoveRelative, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "PTZ move '{0}'", direction);
		logEntryAudit.Arguments = new string[1] { direction };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzMoveHome(Guid deviceId, string cameraName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPtzMoveHome, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted)
		{
			Description = "PTZ move 'Home'"
		};
	}

	public static LogEntryAudit AuditPtzMoveStart(Guid deviceId, string cameraName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPtzMoveStart, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted)
		{
			Description = "PTZ move 'Started'"
		};
	}

	public static LogEntryAudit AuditPtzMoveStop(Guid deviceId, string cameraName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPtzMoveStop, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted)
		{
			Description = "PTZ move 'Stopped'"
		};
	}

	public static LogEntryAudit AuditPtzMoveAbsolute(Guid deviceId, string cameraName, string panValue, string tiltValue, string zoomValue, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzMoveAbsolute, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "PTZ move absolute. Pan '{0}', tilt '{1}', zoom '{2}'.", panValue, tiltValue, zoomValue);
		logEntryAudit.Arguments = new string[3] { panValue, tiltValue, zoomValue };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzControlTimeBased(Guid deviceId, string cameraName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPtzControlTimeBased, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Denied)
		{
			Description = "Request for a PTZ session denied due to time based security restrictions."
		};
	}

	public static LogEntryAudit AuditPtzReserveGranted(Guid deviceId, string cameraName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPtzReserveGranted, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted)
		{
			Description = "Reserve PTZ session"
		};
	}

	public static LogEntryAudit AuditPtzReserveDenied(Guid deviceId, string cameraName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPtzReserveDenied, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Denied)
		{
			Description = "Reserve PTZ session"
		};
	}

	public static LogEntryAudit AuditPtzReleaseGranted(Guid deviceId, string cameraName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPtzReleaseGranted, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted)
		{
			Description = "PTZ session was released"
		};
	}

	public static LogEntryAudit AuditPtzReleaseDenied(Guid deviceId, string cameraName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditPtzReleaseDenied, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Denied)
		{
			Description = "PTZ session release was denied"
		};
	}

	public static LogEntryAudit AuditPtzActivatePatrollingProfileGranted(Guid deviceId, string cameraName, string recordingServerName, string patrollingprofilename, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzActivatePatrollingProfileGranted, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "PTZ patrolling profile '{0}' was started", patrollingprofilename);
		logEntryAudit.Arguments = new string[1] { patrollingprofilename };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzActivatePatrollingProfileDenied(Guid deviceId, string cameraName, string recordingServerName, string patrollingprofilename, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzActivatePatrollingProfileDenied, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Start of PTZ patrolling profile '{0}' was denied", patrollingprofilename);
		logEntryAudit.Arguments = new string[1] { patrollingprofilename };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzNewSessionGranted(Guid deviceId, string cameraName, string recordingServerName, string userName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzNewSessionGranted, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "New PTZ session was started by user {0}", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzNewSessionDenied(Guid deviceId, string cameraName, string recordingServerName, string userName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzNewSessionDenied, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "New PTZ session was denied user {0}", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzPresetCreated(Guid deviceId, string cameraName, string presetName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzPresetCreated, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "PTZ preset position '{0}' was created", presetName);
		logEntryAudit.Arguments = new string[1] { presetName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzPresetChanged(Guid deviceId, string cameraName, string presetName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzPresetChanged, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "PTZ preset position '{0}' was changed", presetName);
		logEntryAudit.Arguments = new string[1] { presetName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzPresetDeleted(Guid deviceId, string cameraName, string presetName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzPresetDeleted, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "PTZ preset position '{0}' was removed", presetName);
		logEntryAudit.Arguments = new string[1] { presetName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzPatrollingProfileCreated(Guid deviceId, string cameraName, string patrollingprofilename, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzPatrollingProfileCreated, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Patrolling profile '{0}' was created", patrollingprofilename);
		logEntryAudit.Arguments = new string[1] { patrollingprofilename };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzPatrollingProfileChanged(Guid deviceId, string cameraName, string patrollingprofilename, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzPatrollingProfileChanged, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Patrolling profile '{0}' was changed", patrollingprofilename);
		logEntryAudit.Arguments = new string[1] { patrollingprofilename };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditPtzPatrollingProfileDeleted(Guid deviceId, string cameraName, string patrollingprofilename, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditPtzPatrollingProfileDeleted, locationIPAddress, SourceType.Server, cameraName, userCredentials, ResourceType.Camera, cameraName, recordingServerName, deviceId.ToString(), AuditType.Ptz, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Patrolling profile '{0}' was deleted", patrollingprofilename);
		logEntryAudit.Arguments = new string[1] { patrollingprofilename };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditOutputActivated(Guid deviceId, string outputName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditOutputActivated, locationIPAddress, SourceType.Server, outputName, userCredentials, ResourceType.Output, outputName, recordingServerName, deviceId.ToString(), AuditType.IOAndEvents, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Output '{0}' activated", outputName);
		logEntryAudit.Arguments = new string[1] { outputName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditOutputActivatedTimeBased(Guid deviceId, string outputName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditOutputActivatedTimeBased, locationIPAddress, SourceType.Server, outputName, userCredentials, ResourceType.Output, outputName, recordingServerName, deviceId.ToString(), AuditType.IOAndEvents, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Activating output '{0}' denied due to time based security restrictions", outputName);
		logEntryAudit.Arguments = new string[1] { outputName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditUserDefinedEventActivated(Guid eventId, string userDefinedEventName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditUserDefinedEventActivated, locationIPAddress, SourceType.Server, userDefinedEventName, userCredentials, ResourceType.UserDefinedEvents, userDefinedEventName, managementServerName, eventId.ToString(), AuditType.IOAndEvents, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "User-defined event '{0}' activated", userDefinedEventName);
		logEntryAudit.Arguments = new string[1] { userDefinedEventName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditUserDefinedEventActivatedTimeBased(Guid eventId, string userDefinedEventName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditUserDefinedEventActivatedTimeBased, locationIPAddress, SourceType.Server, userDefinedEventName, userCredentials, ResourceType.UserDefinedEvents, userDefinedEventName, managementServerName, eventId.ToString(), AuditType.IOAndEvents, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Activation user-defined event '{0}' denied due to time based security restrictions", userDefinedEventName);
		logEntryAudit.Arguments = new string[1] { userDefinedEventName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditGenericEventsActivated(string dataSourceNames, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditGenericEventsActivated, locationIPAddress, SourceType.Server, dataSourceNames, userCredentials, ResourceType.GenericEvents, dataSourceNames, string.Empty, Guid.Empty.ToString(), AuditType.IOAndEvents, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Generic events activated. Data sources enabled: {0}", dataSourceNames);
		logEntryAudit.Arguments = new string[1] { dataSourceNames };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditGenericEventsDeactivated(string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditGenericEventsDeactivated, locationIPAddress, SourceType.Server, string.Empty, userCredentials, ResourceType.GenericEvents, string.Empty, string.Empty, Guid.Empty.ToString(), AuditType.IOAndEvents, AccessGranted.Granted)
		{
			Description = "Generic events deactivated. All data sources are disabled."
		};
	}

	public static LogEntryAudit AuditUserTriggeredEvent(Guid eventId, string eventName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditUserTriggeredEvent, locationIPAddress, SourceType.Server, eventName, userCredentials, ResourceType.UserDefinedEvents, eventName, managementServerName, eventId.ToString(), AuditType.IOAndEvents, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The user-defined event '{0}' was triggered.", eventName);
		logEntryAudit.Arguments = new string[1] { eventName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportCameraToAvi(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportCameraToAvi, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "AVI export started. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportCameraToDBNoEncryption(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportCameraToDBNoEncryption, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Database export started. No encryption. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportCameraToDBWithEncryption(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportCameraToDBWithEncryption, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Database export started. With encryption. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportCameraCancelled(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditExportCameraCancelled, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted)
		{
			Description = "Export cancelled."
		};
	}

	public static LogEntryAudit AuditExportCameraCompleted(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportCameraCompleted, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Camera, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Export completed. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportMicrophoneToAvi(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportMicrophoneToAvi, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "AVI export started. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportMicrophoneToDBNoEncryption(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportMicrophoneToDBNoEncryption, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Database export started. No encryption. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportMicrophoneToDBWithEncryption(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportMicrophoneToDBWithEncryption, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Database export started. With encryption. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportMicrophoneCancelled(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditExportMicrophoneCancelled, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted)
		{
			Description = "Export cancelled."
		};
	}

	public static LogEntryAudit AuditExportMicrophoneCompleted(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportMicrophoneCompleted, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Microphone, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Export completed. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportSpeakerToAvi(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportSpeakerToAvi, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "AVI export started. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportSpeakerToDBNoEncryption(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportSpeakerToDBNoEncryption, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Database export started. No encryption. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportSpeakerToDBWithEncryption(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportSpeakerToDBWithEncryption, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Database export started. With encryption. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportSpeakerCancelled(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditExportSpeakerCancelled, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted)
		{
			Description = "Export cancelled."
		};
	}

	public static LogEntryAudit AuditExportSpeakerCompleted(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportSpeakerCompleted, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Speaker, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Export completed. Sequence '{0}' to '{1}'.", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportMetadataToDBNoEncryption(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportMetadataToDBNoEncryption, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Database export started. No encryption. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportMetadataToDBWithEncryption(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportMetadataToDBWithEncryption, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Database export started. With encryption. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportMetadataCompleted(Guid deviceId, string deviceName, DateTime sequenceStart, DateTime sequenceEnd, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditExportMetadataCompleted, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Export completed. Sequence '{0}' to '{1}'", sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[2]
		{
			sequenceStart.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			sequenceEnd.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditExportMetadataCancelled(Guid deviceId, string deviceName, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditExportMetadataCancelled, locationIPAddress, SourceType.Server, deviceName, userCredentials, ResourceType.Metadata, deviceName, recordingServerName, deviceId.ToString(), AuditType.Export, AccessGranted.Granted)
		{
			Description = "Export cancelled."
		};
	}

	public static LogEntryAudit AuditVideoWallMonitorCameraRemoved(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string cameraName, string position, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorCameraRemoved, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.SmartWallControl, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Camera '{0}' removed from position '{1}'", cameraName, position);
		logEntryAudit.Arguments = new string[2] { cameraName, position };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorLayoutChanged(Guid videoWallMonitorId, string videoWallMonitorName, string layoutName, string videoWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorLayoutChanged, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.SmartWallControl, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Layout changed to '{0}'", layoutName);
		logEntryAudit.Arguments = new string[1] { layoutName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorLayoutCameraChanged(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string cameraName, string position, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorLayoutCameraChanged, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.SmartWallControl, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Camera '{0}' placed at position '{1}'", cameraName, position);
		logEntryAudit.Arguments = new string[2] { cameraName, position };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorLayoutPositionCleared(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string position, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorLayoutPositionCleared, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.SmartWallControl, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Position '{0}' cleared", position);
		logEntryAudit.Arguments = new string[1] { position };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorLayoutPresetActivated(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string presetName, string oemSmartWallName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorLayoutPresetActivated, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.SmartWallControl, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "{0} preset '{1}' selected", oemSmartWallName, presetName);
		logEntryAudit.Arguments = new string[2] { oemSmartWallName, presetName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorLayoutCleared(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditVideoWallMonitorLayoutCleared, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.SmartWallControl, AccessGranted.Granted)
		{
			Description = "Layout cleared"
		};
	}

	public static LogEntryAudit AuditVideoWallMonitorLayoutMonitorApplied(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string sourceMonitorName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorLayoutMonitorApplied, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.SmartWallControl, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "View '{0}' applied to layout", sourceMonitorName);
		logEntryAudit.Arguments = new string[1] { sourceMonitorName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallMonitorLayoutViewApplied(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string sourceViewName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallMonitorLayoutViewApplied, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.SmartWallControl, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "View '{0}' applied to layout", sourceViewName);
		logEntryAudit.Arguments = new string[1] { sourceViewName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditVideoWallTextViewItemCreated(Guid videoWallMonitorId, string videoWallMonitorName, string videoWallName, string message, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditVideoWallTextViewItemCreated, locationIPAddress, SourceType.Server, videoWallMonitorName, userCredentials, ResourceType.SmartWallMonitor, videoWallMonitorName, videoWallName, videoWallMonitorId.ToString(), AuditType.SmartWallControl, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Text message ('{0}') displayed", message);
		logEntryAudit.Arguments = new string[1] { message };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMapDisplayed(Guid mapId, string mapName, string mapServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMapDisplayed, locationIPAddress, SourceType.Server, mapServerName, userCredentials, ResourceType.Map, mapServerName, mapServerName, mapId.ToString(), AuditType.MapViewing, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Map '{0}' displayed", mapName);
		logEntryAudit.Arguments = new string[1] { mapName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBookmarkCreated(Guid deviceId, string deviceName, ResourceType deviceType, string refId, string headline, DateTime beginTime, DateTime triggerTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBookmarkCreated, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Bookmark, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Bookmark '{0}' added. Headline: '{1}', clip start time: '{2}' (utc), bookmark time: '{3}' (utc), clip end time: '{4}' (utc).", refId, headline, beginTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), triggerTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[5]
		{
			refId,
			headline,
			beginTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			triggerTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBookmarkEdited(Guid deviceId, string deviceName, ResourceType deviceType, string refId, string headline, DateTime beginTime, DateTime triggerTime, DateTime endTime, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBookmarkEdited, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Bookmark, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Bookmark '{0}' changed. Headline: '{1}', clip start time: '{2}' (utc), bookmark time: '{3}' (utc), clip end time: '{4}' (utc).", refId, headline, beginTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), triggerTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
		logEntryAudit.Arguments = new string[5]
		{
			refId,
			headline,
			beginTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			triggerTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture),
			endTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)
		};
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBookmarkDeleted(Guid deviceId, string deviceName, ResourceType deviceType, string refId, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBookmarkDeleted, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Bookmark, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Bookmark '{0}' deleted.", refId);
		logEntryAudit.Arguments = new string[1] { refId };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBookmarkViewed(Guid deviceId, string deviceName, ResourceType deviceType, string refId, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBookmarkViewed, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.Bookmark, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Bookmark(s) viewed: '{0}'.", refId);
		logEntryAudit.Arguments = new string[1] { refId };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditManualRecordingStarted(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditManualRecordingStarted, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.ManualRecording, AccessGranted.Granted)
		{
			Description = "Manual recording started."
		};
	}

	public static LogEntryAudit AuditManualRecordingStopped(Guid deviceId, string deviceName, ResourceType deviceType, string recordingServerName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditManualRecordingStopped, locationIPAddress, SourceType.Server, deviceName, userCredentials, deviceType, deviceName, recordingServerName, deviceId.ToString(), AuditType.ManualRecording, AccessGranted.Granted)
		{
			Description = "Manual recording stopped."
		};
	}

	public static LogEntryAudit AuditSearchCreated(Guid id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSearchCreated, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.SaveSearches, name, managementServerName, id.ToString(), AuditType.SaveSearches, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Search '{0}' saved.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditSearchEdited(Guid id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSearchEdited, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.SaveSearches, name, managementServerName, id.ToString(), AuditType.SaveSearches, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Search '{0}' edited.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditSearchDeleted(Guid id, string name, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSearchDeleted, locationIPAddress, SourceType.Server, name, userCredentials, ResourceType.SaveSearches, name, managementServerName, id.ToString(), AuditType.SaveSearches, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Search '{0}' deleted.", name);
		logEntryAudit.Arguments = new string[1] { name };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditSearchRenamed(Guid id, string newName, string oldName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditSearchRenamed, locationIPAddress, SourceType.Server, newName, userCredentials, ResourceType.SaveSearches, newName, managementServerName, id.ToString(), AuditType.SaveSearches, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Search '{0}' renamed to '{1}'.", oldName, newName);
		logEntryAudit.Arguments = new string[2] { oldName, newName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMetadataSearchCategoryIsEnabled(string id, string categoryName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMetadataSearchCategoryIsEnabled, locationIPAddress, SourceType.Server, categoryName, userCredentials, ResourceType.MetadataSearch, categoryName, managementServerName, id, AuditType.MetadataSearch, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Visibility of search category '{0}' enabled", categoryName);
		logEntryAudit.Arguments = new string[1] { categoryName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMetadataSearchCategoryIsDisabled(string id, string categoryName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMetadataSearchCategoryIsDisabled, locationIPAddress, SourceType.Server, categoryName, userCredentials, ResourceType.MetadataSearch, categoryName, managementServerName, id, AuditType.MetadataSearch, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Visibility of search category '{0}' disabled", categoryName);
		logEntryAudit.Arguments = new string[1] { categoryName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMetadataSearchFilterIsEnabled(string id, string filterName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMetadataSearchFilterIsEnabled, locationIPAddress, SourceType.Server, filterName, userCredentials, ResourceType.MetadataSearch, filterName, managementServerName, id, AuditType.MetadataSearch, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Visibility of search filter '{0}' enabled", filterName);
		logEntryAudit.Arguments = new string[1] { filterName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditMetadataSearchFilterIsDisabled(string id, string filterName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditMetadataSearchFilterIsDisabled, locationIPAddress, SourceType.Server, filterName, userCredentials, ResourceType.MetadataSearch, filterName, managementServerName, id, AuditType.MetadataSearch, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Visibility of search filter '{0}' disabled", filterName);
		logEntryAudit.Arguments = new string[1] { filterName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditIdentityProviderAdministratorLoginFailed(Guid resourceId, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditIdentityProviderAdministratorLoginFailed, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.IdentityProvider, identityProviderName, identityProviderName, resourceId.ToString(), AuditType.Security, AccessGranted.Denied)
		{
			Description = "Administrator login was rejected by the Identity Provider. Access denied."
		};
	}

	public static LogEntryAudit AuditUserLoggedIn(Guid resourceId, string identityProviderName, string userName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditUserLoggedIn, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, resourceId.ToString(), AuditType.Login, AccessGranted.Granted)
		{
			Description = "Login by the user succeeded."
		};
	}

	public static LogEntryAudit AuditUserLoginFailedPasswordChange(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditUserLoginFailedPasswordChange, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Login, AccessGranted.Denied)
		{
			Description = "Login by the user failed. The password must be changed."
		};
	}

	public static LogEntryAudit AuditUserLoginFailed(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditUserLoginFailed, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Login, AccessGranted.Denied)
		{
			Description = "Login by the user failed. The password is invalid."
		};
	}

	public static LogEntryAudit AuditBasicUserSystemLockedOut(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserSystemLockedOut, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Security, AccessGranted.Denied);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' has been locked out by the system due to too many failed login attempts.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserLockedOutCleared(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserLockedOutCleared, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' locked out time has been cleared.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserDescriptionChanged(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserDescriptionChanged, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' description has changed.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserCreatedIdentityProvider(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserCreatedIdentityProvider, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' created.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserDeletedIdentityProvider(string sid, string userName, string managementServerName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserDeletedIdentityProvider, locationIPAddress, SourceType.IdentityProvider, userName, userCredentials, ResourceType.BasicUser, userName, managementServerName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' deleted.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserForcePasswordChangeIdentityProvider(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserForcePasswordChangeIdentityProvider, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' must change password.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserEnabledIdentityProvider(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserEnabledIdentityProvider, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' is now enabled.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserDisabled(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserDisabled, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' is disabled.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditBasicUserForcePasswordChangeCleared(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserForcePasswordChangeCleared, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "Basic user '{0}' not forced to change password.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}

	public static LogEntryAudit AuditUserLoginFailedLockedOutByAdmin(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditUserLoginFailedLockedOutByAdmin, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Login, AccessGranted.Denied)
		{
			Description = "Login by the user failed. The user is locked out by the administrator."
		};
	}

	public static LogEntryAudit AuditUserLoginFailedUnknownUser(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditUserLoginFailedUnknownUser, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Login, AccessGranted.Denied)
		{
			Description = "Login by the user failed. The user is unknown."
		};
	}

	public static LogEntryAudit AuditUserLoggedOut(Guid resourceId, string identityProviderName, string userName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditUserLoggedOut, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, resourceId.ToString(), AuditType.Login, AccessGranted.Granted)
		{
			Description = "Logout by the user succeeded."
		};
	}

	public static LogEntryAudit AuditIdentityProviderRegistrationSuccess(string identityProviderName, string clientId, string clientName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditIdentityProviderRegistrationSuccess, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.ServiceRegistration, clientName, identityProviderName, clientId, AuditType.Management, AccessGranted.Granted)
		{
			Description = "Service registered successfully."
		};
	}

	public static LogEntryAudit AuditIdentityProviderRegistrationChanged(string identityProviderName, string clientId, string clientName, string userCredentials, string locationIPAddress)
	{
		return new LogEntryAudit(LogEntryId.AuditIdentityProviderRegistrationChanged, locationIPAddress, SourceType.IdentityProvider, identityProviderName, userCredentials, ResourceType.ServiceRegistration, clientName, identityProviderName, clientId, AuditType.Management, AccessGranted.Granted)
		{
			Description = "Service registration changed successfully."
		};
	}

	public static LogEntryAudit AuditBasicUserChangeOwnPassword(string sid, string userName, string identityProviderName, string userCredentials, string locationIPAddress)
	{
		LogEntryAudit logEntryAudit = new LogEntryAudit(LogEntryId.AuditBasicUserChangeOwnPassword, locationIPAddress, SourceType.IdentityProvider, userName, userCredentials, ResourceType.BasicUser, userName, identityProviderName, sid, AuditType.Management, AccessGranted.Granted);
		logEntryAudit.Description = string.Format(CultureInfo.CurrentCulture, "The password for basic user '{0}' is changed.", userName);
		logEntryAudit.Arguments = new string[1] { userName };
		return logEntryAudit;
	}
}
