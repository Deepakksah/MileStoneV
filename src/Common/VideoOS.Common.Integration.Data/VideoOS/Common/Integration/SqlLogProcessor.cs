using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Common.Integration;

public class SqlLogProcessor : ISqlLogProcessor, IDisposable
{
	private SqlDataAccessor _vmoDataAccessor;

	private SqlDataFilter _vmoDataFilter;

	private SqlLogPendingAccessor _vmoLogPendingaccessor;

	private bool _isDisposed;

	private bool _disablePermissionAuditLog;

	public LogEntryAuditLogger LogEntryAuditLogger { get; private set; }

	public SqlLogProcessor(ISqlConnectionManager sqlConnectionManager, Guid managementServerId, string managementServerName, bool disablePermissionAuditLog, ILocalizedLogger auditAndSystemLogger)
	{
		_vmoDataAccessor = new SqlDataAccessor(sqlConnectionManager);
		_vmoDataFilter = new SqlDataFilter(sqlConnectionManager, _vmoDataAccessor);
		_vmoLogPendingaccessor = new SqlLogPendingAccessor(sqlConnectionManager);
		SqlInfoCollector dataAccessor = new SqlInfoCollector(managementServerId, managementServerName);
		LogEntryAuditLogger = new LogEntryAuditLogger(auditAndSystemLogger, _vmoDataFilter, dataAccessor);
		_disablePermissionAuditLog = disablePermissionAuditLog;
	}

	private static void GetResourceInfoFromParameters(string[] parameters, out Guid resourceId, out string resourceName, out Guid resourceHostId, out string resourceHostName)
	{
		resourceId = new Guid(parameters[0]);
		resourceName = parameters[1];
		resourceHostId = new Guid(parameters[2]);
		resourceHostName = parameters[3];
	}

	private static void GetResourceInfoFromParameters(string[] parameters, out Guid resourceId, out string resourceName, out Guid resourceHostId, out string resourceHostName, out string extra)
	{
		GetResourceInfoFromParameters(parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
		extra = parameters[4];
	}

	private static void GetResourceInfoFromParameters(string[] parameters, out Guid resourceId, out string resourceName, out Guid resourceHostId, out string resourceHostName, out string extra, out string extra2)
	{
		GetResourceInfoFromParameters(parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
		extra = parameters[4];
		extra2 = parameters[5];
	}

	private static void GetResourceInfoFromParameters(string[] parameters, out Guid resourceId, out string resourceName, out Guid resourceHostId, out string resourceHostName, out string extra, out string extra2, out string extra3)
	{
		GetResourceInfoFromParameters(parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
		extra = parameters[4];
		extra2 = parameters[5];
		extra3 = parameters[6];
	}

	private static void GetSimpleResourceInfoFromParameters(string[] parameters, out Guid resourceId, out string resourceName)
	{
		resourceId = new Guid(parameters[0]);
		resourceName = parameters[1];
	}

	private static void GetSimpleResourceInfoFromParameters(string[] parameters, out Guid resourceId, out string resourceName, out string extra)
	{
		GetSimpleResourceInfoFromParameters(parameters, out resourceId, out resourceName);
		extra = parameters[2];
	}

	private static void GetSimpleResourceInfoFromParameters(string[] parameters, out Guid resourceId, out string resourceName, out string extra, out string extra2)
	{
		GetSimpleResourceInfoFromParameters(parameters, out resourceId, out resourceName);
		extra = parameters[2];
		extra2 = parameters[3];
	}

	private static void GetSimpleResourceInfoFromParameters(string[] parameters, out Guid resourceId, out string resourceName, out string extra, out string extra2, out string extra3)
	{
		GetSimpleResourceInfoFromParameters(parameters, out resourceId, out resourceName);
		extra = parameters[2];
		extra2 = parameters[3];
		extra3 = parameters[4];
	}

	private static void GetSimpleResourceInfoFromParameters(string[] parameters, out int resourceId, out string resourceName)
	{
		resourceId = int.Parse(parameters[0], NumberStyles.Integer, CultureInfo.InvariantCulture);
		resourceName = parameters[1];
	}

	private static void GetSimpleResourceInfoFromParameters(string[] parameters, out int resourceId, out string resourceName, out string extra)
	{
		GetSimpleResourceInfoFromParameters(parameters, out resourceId, out resourceName);
		extra = parameters[2];
	}

	private static void GetResourceSidInfoFromParameters(string[] parameters, out string resourceId, out string resourceName)
	{
		resourceId = parameters[0];
		resourceName = parameters[1];
	}

	private static void GetResourceSidInfoFromParameters(string[] parameters, out string resourceId, out string resourceName, out string extra)
	{
		GetResourceSidInfoFromParameters(parameters, out resourceId, out resourceName);
		extra = parameters[2];
	}

	private static void GetResourceIdDetialsInfoFromParameters(string[] parameters, out Guid resourceId, out string resourceName, out string details)
	{
		GetResourceIdInfoFromParameters(parameters, out resourceId, out resourceName);
		details = parameters[2];
	}

	private static void GetResourceIdInfoFromParameters(string[] parameters, out Guid resourceId, out string resourceName)
	{
		resourceId = new Guid(parameters[0]);
		resourceName = parameters[1];
	}

	private static void GetResourceRoleRelationFromParameters(string[] parameters, out string roleName, out string retentionName, out Guid roleId)
	{
		roleName = parameters[0];
		retentionName = parameters[1];
		if (parameters.Length >= 3)
		{
			roleId = ConvertToGuid(parameters[2]);
		}
		else
		{
			roleId = Guid.Empty;
		}
	}

	private static void GetResourceTimeProfileInfoFromParameters(string[] parameters, out string roleName, out string existingTimeProfileName, out string newTimeProfileName, out Guid roleId)
	{
		roleName = parameters[0];
		existingTimeProfileName = parameters[1];
		newTimeProfileName = parameters[2];
		if (parameters.Length >= 4)
		{
			roleId = ConvertToGuid(parameters[3]);
		}
		else
		{
			roleId = Guid.Empty;
		}
	}

	private static void GetResourceClientProfileInfoFromParameters(string[] parameters, out string roleName, out string clientType, out string existingProfileName, out bool existingProfileIsDefault, out string newProfileName, out bool newProfileIsDefault, out Guid roleId)
	{
		roleName = parameters[0];
		clientType = parameters[1];
		existingProfileName = parameters[2];
		existingProfileIsDefault = Convert.ToBoolean(int.Parse(parameters[3], CultureInfo.InvariantCulture));
		newProfileName = parameters[4];
		newProfileIsDefault = Convert.ToBoolean(int.Parse(parameters[5], CultureInfo.InvariantCulture));
		if (parameters.Length >= 7)
		{
			roleId = ConvertToGuid(parameters[6]);
		}
		else
		{
			roleId = Guid.Empty;
		}
	}

	private static void GetResourceSecurityPermissionInfoFromParameters(string[] parameters, out string namespaceName, out string actionId, out string objectId, out Guid roleId, out string roleName)
	{
		namespaceName = parameters[0];
		namespaceName = namespaceName.Replace("{MatrixApplicationName}", OemResources.Instance.Manager.MatrixApplicationName);
		namespaceName = namespaceName.Replace("{SmartWall}", OemResources.Instance.VideoWall.ApplicationNameShort);
		actionId = parameters[1];
		objectId = parameters[2];
		roleId = ConvertToGuid(parameters[3]);
		roleName = parameters[4];
	}

	private static void GetResourceSecurityPermissionInfoFromParameters(string[] parameters, out string namespaceName, out string actionId, out string objectId, out string objectName, out Guid roleId, out string roleName)
	{
		namespaceName = parameters[0];
		namespaceName = namespaceName.Replace("{MatrixApplicationName}", OemResources.Instance.Manager.MatrixApplicationName);
		namespaceName = namespaceName.Replace("{SmartWall}", OemResources.Instance.VideoWall.ApplicationNameShort);
		actionId = parameters[1];
		objectId = parameters[2];
		objectName = parameters[3];
		if (!Guid.TryParse(parameters[4], out roleId))
		{
			roleId = Guid.Empty;
		}
		roleName = parameters[5];
	}

	private static void GetResourceSecurityPermissionInfoTimebasedFromParameters(string[] parameters, out string namespaceName, out string actionId, out string objectId, out string objectName, out Guid roleId, out string roleName, out string existingProfileName, out string newProfileName)
	{
		namespaceName = parameters[0];
		namespaceName = namespaceName.Replace("{MatrixApplicationName}", OemResources.Instance.Manager.MatrixApplicationName);
		namespaceName = namespaceName.Replace("{SmartWall}", OemResources.Instance.VideoWall.ApplicationNameShort);
		actionId = parameters[1];
		objectId = parameters[2];
		objectName = parameters[3];
		if (!Guid.TryParse(parameters[4], out roleId))
		{
			roleId = Guid.Empty;
		}
		roleName = parameters[5];
		existingProfileName = parameters[6];
		newProfileName = parameters[7];
	}

	private static void GetResourceSecurityPermissionPriorityInfoFromParameters(string[] parameters, out string namespaceName, out string actionId, out string deviceId, out string deviceName, out Guid roleId, out string roleName, out string existingPriority, out string newPriority)
	{
		namespaceName = parameters[0];
		actionId = parameters[1];
		deviceId = parameters[2];
		deviceName = parameters[3];
		if (!Guid.TryParse(parameters[4], out roleId))
		{
			roleId = Guid.Empty;
		}
		roleName = parameters[5];
		existingPriority = parameters[6];
		newPriority = parameters[7];
	}

	private static void GetResourceSecurityPermissionGrantRevokePriorityInfoFromParameters(string[] parameters, out string namespaceName, out string actionId, out string deviceId, out string deviceName, out Guid roleId, out string roleName, out string priority)
	{
		namespaceName = parameters[0];
		actionId = parameters[1];
		deviceId = parameters[2];
		deviceName = parameters[3];
		if (!Guid.TryParse(parameters[4], out roleId))
		{
			roleId = Guid.Empty;
		}
		roleName = parameters[5];
		priority = parameters[6];
	}

	private static void GetResourceSecurityPermissionRestrictionInfoFromParameters(string[] parameters, out string namespaceName, out string actionId, out string deviceId, out string deviceName, out Guid roleId, out string roleName, out int existingRestriction, out int newRestriction)
	{
		namespaceName = parameters[0];
		actionId = parameters[1];
		deviceId = parameters[2];
		deviceName = parameters[3];
		if (!Guid.TryParse(parameters[4], out roleId))
		{
			roleId = Guid.Empty;
		}
		roleName = parameters[5];
		if (!int.TryParse(parameters[6], out existingRestriction))
		{
			existingRestriction = 0;
		}
		if (!int.TryParse(parameters[7], out newRestriction))
		{
			newRestriction = 0;
		}
	}

	public void LogSession(Guid sessionId, string userCredentials, string remoteIPAddress)
	{
		try
		{
			List<SqlLogPendingAccessor.PendingLog> pendingLogs = _vmoLogPendingaccessor.QueryPendingLogs(sessionId);
			LogSession(userCredentials, remoteIPAddress, pendingLogs);
		}
		finally
		{
			_vmoLogPendingaccessor.DeletePendingLogs(sessionId);
		}
	}

	private void LogSession(string userCredentials, string remoteIPAddress, List<SqlLogPendingAccessor.PendingLog> pendingLogs)
	{
		foreach (SqlLogPendingAccessor.PendingLog pendingLog in pendingLogs)
		{
			string resourceName = string.Empty;
			string extra = string.Empty;
			string resourceHostName = string.Empty;
			string extra2 = string.Empty;
			string extra3 = string.Empty;
			string deviceName = string.Empty;
			string extra4 = string.Empty;
			string extra5 = string.Empty;
			string extra6 = string.Empty;
			string extra7 = string.Empty;
			string extra8 = string.Empty;
			string extra9 = string.Empty;
			string extra10 = string.Empty;
			string extra11 = string.Empty;
			string extra12 = string.Empty;
			string details = string.Empty;
			string extra13 = string.Empty;
			string extra14 = string.Empty;
			string extra15 = string.Empty;
			string extra16 = string.Empty;
			string extra17 = string.Empty;
			string extra18 = string.Empty;
			string extra19 = string.Empty;
			string extra20 = string.Empty;
			string extra21 = string.Empty;
			string namespaceName = string.Empty;
			string actionId = string.Empty;
			string deviceId = string.Empty;
			string objectName = string.Empty;
			string roleName = string.Empty;
			string extra22 = string.Empty;
			Guid roleId = Guid.Empty;
			string extra23 = string.Empty;
			Guid resourceId;
			Guid resourceHostId;
			int resourceId2;
			string resourceId3;
			switch (pendingLog.LogId)
			{
			case LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionRestrictionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out deviceName, out roleId, out roleName, out var existingRestriction8, out var newRestriction8);
					LogEntryAuditLogger.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, deviceName, existingRestriction8, newRestriction8);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionRestrictionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out deviceName, out roleId, out roleName, out var existingRestriction4, out var newRestriction4);
					LogEntryAuditLogger.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, deviceName, existingRestriction4, newRestriction4);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionRestrictionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out deviceName, out roleId, out roleName, out var existingRestriction6, out var _);
					LogEntryAuditLogger.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, deviceName, existingRestriction6);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionRestrictionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out deviceName, out roleId, out roleName, out var existingRestriction3, out var newRestriction3);
					LogEntryAuditLogger.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, deviceName, existingRestriction3, newRestriction3);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionRestrictionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out deviceName, out roleId, out roleName, out var existingRestriction7, out var newRestriction7);
					LogEntryAuditLogger.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, deviceName, existingRestriction7, newRestriction7);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionRestrictionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out deviceName, out roleId, out roleName, out var existingRestriction5, out var _);
					LogEntryAuditLogger.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, deviceName, existingRestriction5);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionRestrictionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out deviceName, out roleId, out roleName, out var _, out var newRestriction2);
					LogEntryAuditLogger.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, deviceName, newRestriction2);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionRestrictionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out deviceName, out roleId, out roleName, out var _, out var newRestriction);
					LogEntryAuditLogger.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, deviceName, newRestriction);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityPriorityChanged:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionPriorityInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out deviceName, out roleId, out roleName, out var existingPriority, out var newPriority);
					LogEntryAuditLogger.AuditRoleObjectSecurityPriorityChanged(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, deviceName, existingPriority, newPriority);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityPriorityGranted:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionGrantRevokePriorityInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out deviceName, out roleId, out roleName, out var priority2);
					LogEntryAuditLogger.AuditRoleObjectSecurityPriorityGranted(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, deviceName, priority2);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityPriorityRevoked:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionGrantRevokePriorityInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out deviceName, out roleId, out roleName, out var priority);
					LogEntryAuditLogger.AuditRoleObjectSecurityPriorityRevoked(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, deviceName, priority);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityTimeProfileToProfileChange:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionInfoTimebasedFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out objectName, out roleId, out roleName, out var existingProfileName5, out var newProfileName5);
					LogEntryAuditLogger.AuditRoleObjectSecurityTimeProfileToProfileChange(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, objectName, existingProfileName5, newProfileName5);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityTimeProfileToSystemProfileChange:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionInfoTimebasedFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out objectName, out roleId, out roleName, out var existingProfileName4, out var newProfileName4);
					LogEntryAuditLogger.AuditRoleObjectSecurityTimeProfileToSystemProfileChange(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, objectName, existingProfileName4, newProfileName4);
				}
				break;
			case LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToProfileChange:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionInfoTimebasedFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out objectName, out roleId, out roleName, out var existingProfileName3, out var newProfileName3);
					LogEntryAuditLogger.AuditRoleObjectSecuritySystemTimeProfileToProfileChange(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, objectName, existingProfileName3, newProfileName3);
				}
				break;
			case LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionInfoTimebasedFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out objectName, out roleId, out roleName, out var existingProfileName, out var newProfileName);
					LogEntryAuditLogger.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, objectName, existingProfileName, newProfileName);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityGranted:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out objectName, out roleId, out roleName);
					LogEntryAuditLogger.AuditRoleObjectSecurityGranted(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, objectName);
				}
				break;
			case LogEntryId.AuditRoleObjectSecurityRevoked:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out objectName, out roleId, out roleName);
					LogEntryAuditLogger.AuditRoleObjectSecurityRevoked(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId, objectName);
				}
				break;
			case LogEntryId.AuditRoleOverallSecurityGranted:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out roleId, out roleName);
					LogEntryAuditLogger.AuditRoleOverallSecurityGranted(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId);
				}
				break;
			case LogEntryId.AuditRoleOverallSecurityRevoked:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out roleId, out roleName);
					LogEntryAuditLogger.AuditRoleOverallSecurityRevoked(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId);
				}
				break;
			case LogEntryId.AuditRoleOverallSecurityDenied:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out roleId, out roleName);
					LogEntryAuditLogger.AuditRoleOverallSecurityDenied(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId);
				}
				break;
			case LogEntryId.AuditRoleOverallSecurityRevokedDenied:
				if (!_disablePermissionAuditLog)
				{
					GetResourceSecurityPermissionInfoFromParameters(pendingLog.Parameters, out namespaceName, out actionId, out deviceId, out roleId, out roleName);
					LogEntryAuditLogger.AuditRoleOverallSecurityRevokedDenied(roleId, roleName, userCredentials, remoteIPAddress, namespaceName, actionId);
				}
				break;
			case LogEntryId.AuditDeviceMulticastEnabledChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra20);
				LogEntryAuditLogger.AuditDeviceMulticastEnabledChanged(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress, extra20);
				break;
			case LogEntryId.AuditDevicePtzExtendedManualSessionTimeoutChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra21);
				LogEntryAuditLogger.AuditDevicePtzExtendedManuelTimeoutChanged(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress, extra21);
				break;
			case LogEntryId.AuditDevicePtzManuelSessionTimeoutChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra21);
				LogEntryAuditLogger.AuditDevicePtzManuelTimeoutChanged(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress, extra21);
				break;
			case LogEntryId.AuditDevicePtzReservedSessionTimeoutChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra21);
				LogEntryAuditLogger.AuditDevicePtzReservedSessionTimeoutChanged(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress, extra21);
				break;
			case LogEntryId.AuditVideoWallTextViewItemCreated:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra23);
				LogEntryAuditLogger.AuditVideoWallTextViewItemCreated(resourceId, resourceName, resourceHostName, extra23, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorAdded:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditVideoWallMonitorAdded(resourceId, resourceName, resourceHostId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorDeleted:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditVideoWallMonitorDeleted(resourceId, resourceName, resourceHostId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorEdited:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditVideoWallMonitorEdited(resourceId, resourceName, resourceHostId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorRenamed:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra);
				LogEntryAuditLogger.AuditVideoWallMonitorRenamed(resourceName, extra, resourceId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallDeleted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditVideoWallDeleted(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallRenamed:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra);
				LogEntryAuditLogger.AuditVideoWallRenamed(resourceName, extra, resourceId, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallEdited:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditVideoWallEdited(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallPresetAdded:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditVideoWallPresetAdded(resourceId, resourceName, resourceHostId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallPresetDeleted:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditVideoWallPresetDeleted(resourceId, resourceName, resourceHostId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallPresetRenamed:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra);
				LogEntryAuditLogger.AuditVideoWallPresetRenamed(resourceId, extra, resourceName, resourceHostId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallPresetActivate:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditVideoWallPresetActivated(resourceId, resourceName, resourceHostId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorPresetEdited:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditVideoWallMonitorPresetEdited(resourceId, resourceName, resourceHostId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorLayoutChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra3);
				LogEntryAuditLogger.AuditVideoWallMonitorLayoutChanged(extra3, resourceName, resourceId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorLayoutPositionCleared:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra2);
				LogEntryAuditLogger.AuditVideoWallMonitorLayoutPositionCleared(extra2, resourceName, resourceId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorLayoutCleared:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditVideoWallMonitorLayoutCleared(resourceId, resourceName, resourceHostId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorLayoutCameraChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra2, out deviceName);
				LogEntryAuditLogger.AuditVideoWallMonitorLayoutCameraPlaced(deviceName, extra2, resourceName, resourceId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorLayoutMonitorApplied:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra4);
				LogEntryAuditLogger.AuditVideoWallMonitorLayoutMonitorApplied(resourceId, resourceName, resourceHostId, resourceHostName, extra4, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorLayoutViewApplied:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra5);
				LogEntryAuditLogger.AuditVideoWallMonitorLayoutMonitorApplied(resourceId, resourceName, resourceHostId, resourceHostName, extra5, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditVideoWallMonitorCameraRemoved:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra2, out deviceName);
				LogEntryAuditLogger.AuditVideoWallMonitorCameraRemoved(deviceName, extra2, resourceName, resourceId, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditTimeProfileRenamed:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra);
				LogEntryAuditLogger.AuditTimeProfileRenamed(extra, resourceName, resourceId, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditTimeProfileEdited:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditTimeProfileEdited(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleDeleted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditRoleDelete(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleEdited:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditRoleEdited(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleRenamed:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra);
				LogEntryAuditLogger.AuditRoleRenamed(extra, resourceName, resourceId, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleUserAdded:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra6);
				LogEntryAuditLogger.AuditRoleUserAdded(extra6, resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleUserRemoved:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra6);
				LogEntryAuditLogger.AuditRoleUserRemoved(extra6, resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleGroupAdded:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra7);
				LogEntryAuditLogger.AuditRoleGroupAdded(extra7, resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleGroupRemoved:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra7);
				LogEntryAuditLogger.AuditRoleGroupRemoved(extra7, resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleClientProfileChanged:
			{
				GetResourceClientProfileInfoFromParameters(pendingLog.Parameters, out resourceName, out var clientType, out var existingProfileName2, out var existingProfileIsDefault, out var newProfileName2, out var newProfileIsDefault, out resourceId);
				string text = "";
				if (!(clientType == "SmartClientProfile"))
				{
					if (clientType == "ManagementClientProfile")
					{
						text = OemResources.Instance.Manager.ApplicationNameShort;
					}
				}
				else
				{
					text = OemResources.Instance.SmartClient.ClientName;
				}
				if (existingProfileIsDefault)
				{
					existingProfileName2 = "Default " + text + " Profile";
				}
				if (newProfileIsDefault)
				{
					newProfileName2 = "Default " + text + " Profile";
				}
				LogEntryAuditLogger.AuditRoleClientProfileChanged(resourceId.ToString(), resourceName, text, existingProfileName2, newProfileName2, userCredentials, remoteIPAddress);
				break;
			}
			case LogEntryId.AuditRoleClientLoginGranted:
			{
				string clientName2 = "";
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out string extra33);
				switch (extra33)
				{
				case "mobileclient":
					clientName2 = OemResources.Instance.MobileServer.MobileClientNameShort;
					break;
				case "webclient":
					clientName2 = OemResources.Instance.MobileServer.WebClientNameShort;
					break;
				case "smartclient":
					clientName2 = OemResources.Instance.SmartClient.ClientName;
					break;
				}
				LogEntryAuditLogger.AuditRoleClientLogOnGranted(resourceId.ToString(), resourceName, clientName2, userCredentials, remoteIPAddress);
				break;
			}
			case LogEntryId.AuditRoleClientLoginRevoked:
			{
				string clientName = "";
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out string extra32);
				switch (extra32)
				{
				case "mobileclient":
					clientName = OemResources.Instance.MobileServer.MobileClientNameShort;
					break;
				case "webclient":
					clientName = OemResources.Instance.MobileServer.WebClientNameShort;
					break;
				case "smartclient":
					clientName = OemResources.Instance.SmartClient.ClientName;
					break;
				}
				LogEntryAuditLogger.AuditRoleClientLogOnRevoked(resourceId.ToString(), resourceName, clientName, userCredentials, remoteIPAddress);
				break;
			}
			case LogEntryId.AuditRoleDefaultTimeProfileChanged:
			{
				GetResourceTimeProfileInfoFromParameters(pendingLog.Parameters, out resourceName, out var existingTimeProfileName2, out var newTimeProfileName2, out resourceId);
				LogEntryAuditLogger.AuditRoleDefaultTimeProfileChanged(resourceId.ToString(), resourceName, existingTimeProfileName2, newTimeProfileName2, userCredentials, remoteIPAddress);
				break;
			}
			case LogEntryId.AuditRoleOnlyAllowLoginWithinTimeProfileChanged:
			{
				GetResourceTimeProfileInfoFromParameters(pendingLog.Parameters, out resourceName, out var existingTimeProfileName, out var newTimeProfileName, out resourceId);
				LogEntryAuditLogger.AuditRoleOnlyAllowLogOnWithinTimeProfileChanged(resourceId.ToString(), resourceName, existingTimeProfileName, newTimeProfileName, userCredentials, remoteIPAddress);
				break;
			}
			case LogEntryId.AuditRoleAnonymityDuringPtzSessionsGranted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditRoleAnonymityDuringPtzSessionsGranted(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleAnonymityDuringPtzSessionsRevoked:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditRoleAnonymityDuringPtzSessionsRevoked(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleLoginAuthorizationRequiredGranted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditRoleLogOnAuthorizationRequiredGranted(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleLoginAuthorizationRequiredRevoked:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditRoleLogOnAuthorizationRequiredRevoked(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRoleClaimAdded:
			{
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out var extra29, out var extra30, out var extra31);
				LogEntryAuditLogger.AuditRoleClaimAdded(resourceId, resourceName, extra29, extra30, extra31, userCredentials, remoteIPAddress);
				break;
			}
			case LogEntryId.AuditRoleClaimRemoved:
			{
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out var extra26, out var extra27, out var extra28);
				LogEntryAuditLogger.AuditRoleClaimRemoved(resourceId, resourceName, extra26, extra27, extra28, userCredentials, remoteIPAddress);
				break;
			}
			case LogEntryId.AuditRecordingStorageCreated:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditStorageCreated(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRecordingStorageEdited:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditStorageEdited(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRecordingStorageMoved:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra10, out extra11);
				LogEntryAuditLogger.AuditStorageMoved(resourceId, resourceName, extra10, extra11, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRecordingStorageDeleted:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditStorageDeleted(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRecordingStorageAddArchive:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra9);
				LogEntryAuditLogger.AuditStorageArchiveCreated(resourceId, resourceName, extra9, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRecordingStorageArchiveMoved:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra9, out extra10, out extra11);
				LogEntryAuditLogger.AuditStorageArchiveMoved(resourceId, resourceName, extra9, extra10, extra11, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRecordingStorageArchiveMounted:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra9);
				LogEntryAuditLogger.AuditStorageArchiveMounted(resourceId, resourceName, extra9, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRecordingStorageArchiveUnmounted:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra9);
				LogEntryAuditLogger.AuditStorageArchiveUnmounted(resourceId, resourceName, extra9, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRecordingStorageEditArchive:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra9);
				LogEntryAuditLogger.AuditStorageArchiveEdited(resourceId, resourceName, extra9, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRecordingStorageDeleteArchive:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra9);
				LogEntryAuditLogger.AuditStorageArchiveDeleted(resourceId, resourceName, extra9, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRecorderRenamed:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra);
				LogEntryAuditLogger.AuditRecordingServerRename(resourceName, extra, resourceId, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHardwareRenamed:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra);
				LogEntryAuditLogger.AuditHardwareRenamed(resourceId, resourceName, extra, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHardwareEnabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditHardwareEnabled(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHardwareDisabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditHardwareDisabled(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHardwareMoved:
			{
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out var extra25);
				LogEntryAuditLogger.AuditHardwareMoved(resourceId, resourceName, resourceHostName, extra25, userCredentials, remoteIPAddress);
				break;
			}
			case LogEntryId.AuditHardwarePasswordChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditHardwarePasswordChanged(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHardwareFirmwareUpdateStarted:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditHardwareFirmwareUpdateStarted(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHardwareSettingsChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName);
				LogEntryAuditLogger.AuditHardwareSettingsChanged(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRenamed:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra);
				LogEntryAuditLogger.AuditDeviceRenamed(resourceId, extra, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceGisPointChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra15);
				LogEntryAuditLogger.AuditDeviceGisPointChanged(resourceId, resourceName, extra15, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceShortNameChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra16);
				LogEntryAuditLogger.AuditDeviceShortNameChanged(resourceId, resourceName, extra16, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceShortcutChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra17);
				LogEntryAuditLogger.AuditDeviceShortcutChanged(resourceId, resourceName, extra17, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceIconChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra18);
				LogEntryAuditLogger.AuditDeviceIconChanged(resourceId, resourceName, extra18, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceFailoverSettingChanged:
			{
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out var extra24);
				LogEntryAuditLogger.AuditDeviceFailoverSettingChanged(resourceId, resourceName, extra24, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			}
			case LogEntryId.AuditDeviceCoverageDirectionChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra19);
				LogEntryAuditLogger.AuditDeviceCoverageDirectionChanged(resourceId, resourceName, extra19, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceCoverageDepthChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra19);
				LogEntryAuditLogger.AuditDeviceCoverageDepthChanged(resourceId, resourceName, extra19, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceCoverageFieldOfViewChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12, out extra19);
				LogEntryAuditLogger.AuditDeviceCoverageFieldOfViewChanged(resourceId, resourceName, extra19, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceEnabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceEnabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceDisabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceDisabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRecordSettingsChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceRecordingSettingsChanged(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRecordingEnabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceRecordingEnabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRecordingDisabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceRecordingDisabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRecordingPrebufferEnabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceRecordingPrebufferEnabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRecordingPrebufferDisabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceRecordingPrebufferDisabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRecordingEdgeStorageEnabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceRecordingEdgeStorageEnabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRecordingEdgeStorageDisabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceRecordingEdgeStorageDisabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRecordingEdgeStoragePlaybackEnabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceRecordingEdgeStoragePlaybackEnabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRecordingEdgeStoragePlaybackDisabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceRecordingEdgeStoragePlaybackDisabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceStreamSettingsChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceStreamSettingsChanged(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceMotionDetectionSettingsChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceMotionDetectionSettingsChanged(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceSettingsChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceSettingsChanged(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDevice360LensSettingsChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDevice360LensSettingsChanged(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDevice360LensSettingsDisabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDevice360LensSettingsDisabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDevice360LensSettingsEnabled:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDevice360LensSettingsEnabled(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDevicePrivacyMaskSettingsChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDevicePrivacyMaskSettingsChanged(resourceId, resourceName, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRecordingStorageSelected:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra8, out extra12);
				LogEntryAuditLogger.AuditDeviceRecordingStorageSelected(resourceId, resourceName, extra12, extra8, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHostingServerAdded:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName);
				LogEntryAuditLogger.AuditHostingServerAdded(resourceId2, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHostingServerModified:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName);
				LogEntryAuditLogger.AuditHostingServerModified(resourceId2, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHostingServerRenamed:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName, out extra);
				LogEntryAuditLogger.AuditHostingServerRenamed(resourceId2, resourceName, extra, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHostingServerDeleted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName);
				LogEntryAuditLogger.AuditHostingServerDeleted(resourceId2, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHostedHardwareAdded:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName);
				LogEntryAuditLogger.AuditHostedHardwareAdded(resourceId2, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHostedHardwareModified:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName);
				LogEntryAuditLogger.AuditHostedHardwareModified(resourceId2, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHostedHardwareRenamed:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName, out extra);
				LogEntryAuditLogger.AuditHostedHardwareRenamed(resourceId2, resourceName, extra, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditHostedHardwareDeleted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName);
				LogEntryAuditLogger.AuditHostedHardwareDeleted(resourceId2, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditBasicUserRenamed:
				GetResourceSidInfoFromParameters(pendingLog.Parameters, out resourceId3, out resourceName, out extra);
				LogEntryAuditLogger.AuditBasicUserRenamed(extra, resourceName, resourceId3, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditBasicUserEdited:
				GetResourceSidInfoFromParameters(pendingLog.Parameters, out resourceId3, out resourceName);
				LogEntryAuditLogger.AuditBasicUserEdited(resourceId3, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditBasicUserDeleted:
				GetResourceSidInfoFromParameters(pendingLog.Parameters, out resourceId3, out resourceName);
				LogEntryAuditLogger.AuditBasicUserDelete(resourceId3, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditBasicUserPasswordChanged:
				GetResourceSidInfoFromParameters(pendingLog.Parameters, out resourceId3, out resourceName);
				LogEntryAuditLogger.AuditBasicUserPasswordChanged(resourceId3, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditBasicInfoEdited:
				GetResourceIdDetialsInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out details);
				LogEntryAuditLogger.AuditBasicOwnerInfoEdited(resourceId.ToString(), resourceName, details, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditBasicInfoDeleted:
				GetResourceIdInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditBasicOwnerInfoDelete(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRetentionOptionRoleRelationUpdated:
			{
				GetResourceRoleRelationFromParameters(pendingLog.Parameters, out roleName, out var retentionName, out resourceId);
				LogEntryAuditLogger.AuditRetentionOptionRoleRelationUpdated(resourceId.ToString(), roleName, retentionName, userCredentials, remoteIPAddress);
				break;
			}
			case LogEntryId.AuditMarkedDataUpdate:
				GetResourceIdInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditMarkedDataUpdated(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditMarkedDataDeleted:
				GetResourceIdInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditMarkedDataDelete(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditMarkedDataCreated:
				GetResourceIdInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditMarkedDataCreated(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditMediaRestrictionUpdated:
				GetResourceIdInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditMediaRestrictionUpdated(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditMediaRestrictionDeleted:
				GetResourceIdInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditMediaRestrictionDeleted(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditMediaRestrictionCreated:
				GetResourceIdInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditMediaRestrictionCreated(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditMediaRestrictionLiveModeEntered:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra12);
				LogEntryAuditLogger.AuditMediaRestrictionLiveModeEntered(resourceId, resourceName, extra12, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditMediaRestrictionLiveModeExited:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra12, out extra22);
				LogEntryAuditLogger.AuditMediaRestrictionLiveModeExited(resourceId, resourceName, extra12, extra22, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditMediaRestrictionLiveUpdated:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra12);
				LogEntryAuditLogger.AuditMediaRestrictionLiveUpdated(resourceId, resourceName, extra12, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditMediaRestrictionLiveDeleted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra12);
				LogEntryAuditLogger.AuditMediaRestrictionLiveDeleted(resourceId, resourceName, extra12, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditPerfCountStateRuleUpdated:
				GetResourceIdInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditPerformanceStateRuleUpdated(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditPerfCountStateRuleDeleted:
				GetResourceIdInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditPerformanceStateRuleDelete(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditPerfCountStateRuleCreated:
				GetResourceIdInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditPerformanceStateRuleCreated(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditSystemDashboardConfigurationCreated:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName);
				LogEntryAuditLogger.AuditDashboardTileCreated(resourceId2.ToString(CultureInfo.CurrentCulture), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditSystemDashboardConfigurationDeleted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName);
				LogEntryAuditLogger.AuditDashboardTileDelete(resourceId2.ToString(CultureInfo.CurrentCulture), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditSystemDashboardConfigurationUpdated:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName);
				LogEntryAuditLogger.AuditDashboardTileUpdated(resourceId2.ToString(CultureInfo.CurrentCulture), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditClaimDeleted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId2, out resourceName);
				LogEntryAuditLogger.AuditClaimDeleted(resourceId2, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceRequestEdgeStorageRetrieval:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceRequestEdgeStorageRetrieval(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditPtzPatrollingProfileCreated:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra13);
				LogEntryAuditLogger.AuditPtzPatrollingProfileCreated(resourceId, resourceName, extra13, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditPtzPatrollingProfileChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra13);
				LogEntryAuditLogger.AuditPtzPatrollingProfileChanged(resourceId, resourceName, extra13, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditPtzPatrollingProfileDeleted:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra13);
				LogEntryAuditLogger.AuditPtzPatrollingProfileDeleted(resourceId, resourceName, extra13, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditPtzPresetCreated:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra14);
				LogEntryAuditLogger.AuditPtzPresetCreated(resourceId, resourceName, extra14, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditPtzPresetChanged:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra14);
				LogEntryAuditLogger.AuditPtzPresetChanged(resourceId, resourceName, extra14, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditPtzPresetDeleted:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra14);
				LogEntryAuditLogger.AuditPtzPresetDeleted(resourceId, resourceName, extra14, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditDeviceDatabaseDelete:
				GetResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out resourceHostId, out resourceHostName, out extra12);
				LogEntryAuditLogger.AuditDeviceDatabaseDelete(resourceId, resourceName, extra12, resourceHostName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditAudioMessageContentChanged:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditAudioMessageContentChanged(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditAudioMessageDeleted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditAudioMessageDeleted(resourceId.ToString(), resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRuleDeleted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditRuleDelete(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRuleCreated:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditRuleCreated(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRuleDisabled:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditRuleDisabled(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRuleEnabled:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditRuleEnabled(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRuleEdited:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditRuleEdited(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditRuleRenamed:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra);
				LogEntryAuditLogger.AuditRuleRenamed(extra, resourceName, resourceId, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditSearchCreated:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditSearchCreated(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditSearchEdited:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditSearchEdited(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditSearchDeleted:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName);
				LogEntryAuditLogger.AuditSearchDeleted(resourceId, resourceName, userCredentials, remoteIPAddress);
				break;
			case LogEntryId.AuditSearchRenamed:
				GetSimpleResourceInfoFromParameters(pendingLog.Parameters, out resourceId, out resourceName, out extra);
				LogEntryAuditLogger.AuditSearchRenamed(resourceId, resourceName, extra, userCredentials, remoteIPAddress);
				break;
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void Dispose(bool disposing)
	{
		if (!_isDisposed && disposing)
		{
			LogEntryAuditLogger = null;
			if (_vmoDataFilter != null)
			{
				_vmoDataFilter.Dispose();
				_vmoDataFilter = null;
			}
			if (_vmoDataAccessor != null)
			{
				_vmoDataAccessor.Dispose();
				_vmoDataAccessor = null;
			}
			if (_vmoLogPendingaccessor != null)
			{
				_vmoLogPendingaccessor.Dispose();
				_vmoLogPendingaccessor = null;
			}
		}
		_isDisposed = true;
	}

	private static Guid ConvertToGuid(string id)
	{
		if (!Guid.TryParse(id, out var result))
		{
			return Guid.Empty;
		}
		return result;
	}
}
