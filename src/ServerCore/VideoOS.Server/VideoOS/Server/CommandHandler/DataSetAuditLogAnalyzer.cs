using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Logging;
using VideoOS.Common.Integration.Properties;
using VideoOS.Common.Integration.Security;
using VideoOS.Server.DataComponents;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.CommandHandler;

public sealed class DataSetAuditLogAnalyzer
{
	public enum SpeakPriority
	{
		[Description("Very Low")]
		VeryLow = 1000,
		Low = 2000,
		Medium = 3000,
		High = 4000,
		[Description("Very High")]
		VeryHigh = 5000
	}

	private readonly IVmoCache _vmoCache;

	private readonly IConfigurationDataComponent _configurationDataComponent;

	private readonly ILocalizedLogger _auditAndSystemLogger;

	public DataSetAuditLogAnalyzer(IVmoCache vmoCache, IConfigurationDataComponent configurationDataComponent, ILocalizedLogger auditAndSystemLogger)
	{
		if (vmoCache == null)
		{
			throw new ArgumentNullException("vmoCache");
		}
		if (configurationDataComponent == null)
		{
			throw new ArgumentNullException("configurationDataComponent");
		}
		_vmoCache = vmoCache;
		_configurationDataComponent = configurationDataComponent;
		_auditAndSystemLogger = auditAndSystemLogger ?? throw new ArgumentNullException("auditAndSystemLogger");
	}

	public List<(DateTime, LogEntryAudit)> AnalyzeAndCreateAuditLogs(SecurityDataSet allChanges, string managementServerName, string userCredentials, string locationIPAddress)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		List<(DateTime, LogEntryAudit)> list = new List<(DateTime, LogEntryAudit)>();
		DateTime dateTime = DateTime.UtcNow;
		if (allChanges.SecurityPermissionsDevice.Count > 0)
		{
			DataTable changes = allChanges.SecurityPermissionsDevice.GetChanges();
			string text = string.Empty;
			string text2 = string.Empty;
			foreach (SecurityDataSet.SecurityPermissionsDeviceRow row in changes.Rows)
			{
				Guid guid = new Guid(row.GetRoleId());
				Guid deviceId = new Guid(row.GetDeviceId());
				string securityAttributeId = row.GetSecurityAttributeId();
				List<LogEntryAudit> list2 = new List<LogEntryAudit>();
				string orCreateRoleName = GetOrCreateRoleName(dictionary, guid);
				string orCreateDeviceName = GetOrCreateDeviceName(dictionary, deviceId);
				LegacySecurityActionInformation securityActionInformation = LegacySecurityAttributeMapper.Instance.GetSecurityActionInformation(securityAttributeId);
				bool flag = securityAttributeId == "DEVICESPEAKPRIORITY" || securityAttributeId == "DEVICEPTZPRIORITY";
				if (!flag)
				{
					text = GetOrCreateNamespaceName(dictionary, securityActionInformation);
					text2 = GetOrCreateActionName(dictionary, securityActionInformation);
				}
				if (row.RowState == DataRowState.Added)
				{
					if (flag)
					{
						list2 = HandleAddedSecuritySettingLog(_auditAndSystemLogger, securityAttributeId, managementServerName, userCredentials, locationIPAddress, row, guid, orCreateRoleName, orCreateDeviceName);
					}
					else
					{
						list2.Add(LogEntryAuditFactory.AuditRoleObjectSecurityGranted(guid, orCreateRoleName, managementServerName, text2, text, orCreateDeviceName, userCredentials, locationIPAddress));
						_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityGranted, guid, orCreateRoleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityGranted, text2, text, orCreateDeviceName);
						LogEntryAudit logEntryAudit = HandleAddedPermissionTimeProfileLog(_auditAndSystemLogger, managementServerName, userCredentials, locationIPAddress, row, guid, orCreateRoleName, text2, text, orCreateDeviceName, _vmoCache);
						if (logEntryAudit != null)
						{
							list2.Add(logEntryAudit);
						}
						logEntryAudit = HandleAddedPermissionPlaybackLog(_auditAndSystemLogger, managementServerName, userCredentials, locationIPAddress, row, guid, orCreateRoleName, text2, text, orCreateDeviceName);
						if (logEntryAudit != null)
						{
							list2.Add(logEntryAudit);
						}
					}
				}
				else if (row.RowState == DataRowState.Modified)
				{
					if (flag)
					{
						LogEntryAudit logEntryAudit2 = HandleModifiedSecuritySettingLog(_auditAndSystemLogger, securityAttributeId, managementServerName, userCredentials, locationIPAddress, row, guid, orCreateRoleName, orCreateDeviceName);
						if (logEntryAudit2 != null)
						{
							list2.Add(logEntryAudit2);
						}
					}
					else
					{
						LogEntryAudit logEntryAudit3 = HandleModifiedPermissionTimeProfileLog(_auditAndSystemLogger, managementServerName, userCredentials, locationIPAddress, row, guid, orCreateRoleName, text2, text, orCreateDeviceName, _vmoCache);
						if (logEntryAudit3 != null)
						{
							list2.Add(logEntryAudit3);
						}
						logEntryAudit3 = HandleModifiedPermissionPlaybackLog(_auditAndSystemLogger, managementServerName, userCredentials, locationIPAddress, row, guid, orCreateRoleName, text2, text, orCreateDeviceName);
						if (logEntryAudit3 != null)
						{
							list2.Add(logEntryAudit3);
						}
					}
				}
				else if (row.RowState == DataRowState.Deleted)
				{
					if (flag)
					{
						list2 = HandleDeletedSecuritySettingLog(_auditAndSystemLogger, securityAttributeId, managementServerName, userCredentials, locationIPAddress, row, guid, orCreateRoleName, orCreateDeviceName);
					}
					else
					{
						list2.Add(LogEntryAuditFactory.AuditRoleObjectSecurityRevoked(guid, orCreateRoleName, managementServerName, text2, text, orCreateDeviceName, userCredentials, locationIPAddress));
						_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRevoked, guid, orCreateRoleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRevoked, text2, text, orCreateDeviceName);
					}
				}
				int num = 0;
				DateTime dateTime2 = dateTime;
				foreach (LogEntryAudit item in list2)
				{
					dateTime2 = dateTime.AddMilliseconds(num * 5);
					list.Add((dateTime2, item));
					num++;
				}
				dateTime = dateTime2;
			}
		}
		if (allChanges.SecurityPermissionsMatrix.Count > 0)
		{
			CreatePermissionMatrixLogs(allChanges, managementServerName, userCredentials, locationIPAddress, dictionary, list, dateTime);
		}
		if (allChanges.SecurityPermissions.Count > 0)
		{
			CreateAllowLoginWithinTimeProfileLogs(allChanges, managementServerName, userCredentials, locationIPAddress, dictionary, list, dateTime);
		}
		if (allChanges.TimeProfileDefaultRoleRelation.Count > 0)
		{
			CreateDefaultTimeProfileLogs(allChanges, managementServerName, userCredentials, locationIPAddress, dictionary, list, dateTime);
		}
		return list;
	}

	private void CreateAllowLoginWithinTimeProfileLogs(SecurityDataSet allChanges, string managementServerName, string userCredentials, string locationIPAddress, Dictionary<string, string> nameCache, List<(DateTime, LogEntryAudit)> auditLogEntries, DateTime now)
	{
		DataTable changes = allChanges.SecurityPermissions.GetChanges();
		foreach (SecurityDataSet.SecurityPermissionsRow row in changes.Rows)
		{
			Guid guid = new Guid(row.GetRoleId());
			string orCreateRoleName = GetOrCreateRoleName(nameCache, guid);
			Guid empty = Guid.Empty;
			Guid empty2 = Guid.Empty;
			if (row.RowState == DataRowState.Modified)
			{
				LogEntryAudit logEntryAudit = HandleModifiedPermissionOnlyAllowLoginWithinTimeProfileLog(_auditAndSystemLogger, managementServerName, userCredentials, locationIPAddress, row, orCreateRoleName, guid, _vmoCache);
				if (logEntryAudit != null)
				{
					auditLogEntries.Add((now, logEntryAudit));
				}
			}
			else if (row.RowState == DataRowState.Added)
			{
				empty2 = new Guid(row.GetTimeProfileId(DataRowVersion.Current));
				string text = "Always";
				string orCreateTimeProfileName = GetOrCreateTimeProfileName(nameCache, empty2);
				LogEntryAudit item = LogEntryAuditFactory.AuditRoleOnlyAllowLoginWithinTimeProfileChanged(managementServerName, text, orCreateTimeProfileName, orCreateRoleName, guid.ToString(), userCredentials, locationIPAddress);
				_auditAndSystemLogger.Log(LogEntryId.AuditRoleOnlyAllowLoginWithinTimeProfileChanged, guid, text, managementServerName, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleOnlyAllowLoginWithinTimeProfileChanged, text, orCreateTimeProfileName, orCreateRoleName);
				auditLogEntries.Add((now, item));
			}
			else if (row.RowState == DataRowState.Deleted)
			{
				empty = new Guid(row.GetTimeProfileId(DataRowVersion.Original));
				string orCreateTimeProfileName2 = GetOrCreateTimeProfileName(nameCache, empty2);
				string text2 = "Always";
				LogEntryAudit item2 = LogEntryAuditFactory.AuditRoleOnlyAllowLoginWithinTimeProfileChanged(managementServerName, orCreateTimeProfileName2, text2, orCreateRoleName, guid.ToString(), userCredentials, locationIPAddress);
				_auditAndSystemLogger.Log(LogEntryId.AuditRoleOnlyAllowLoginWithinTimeProfileChanged, guid, orCreateTimeProfileName2, managementServerName, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleOnlyAllowLoginWithinTimeProfileChanged, orCreateTimeProfileName2, text2, orCreateRoleName);
				auditLogEntries.Add((now, item2));
			}
		}
	}

	private void CreateDefaultTimeProfileLogs(SecurityDataSet allChanges, string managementServerName, string userCredentials, string locationIPAddress, Dictionary<string, string> nameCache, List<(DateTime, LogEntryAudit)> auditLogEntries, DateTime now)
	{
		DataTable changes = allChanges.TimeProfileDefaultRoleRelation.GetChanges();
		foreach (SecurityDataSet.TimeProfileDefaultRoleRelationRow row in changes.Rows)
		{
			Guid guid = new Guid(row.GetRoleId());
			string orCreateRoleName = GetOrCreateRoleName(nameCache, guid);
			Guid guid2 = Guid.Empty;
			Guid guid3 = Guid.Empty;
			if (row.RowState == DataRowState.Modified)
			{
				guid2 = new Guid(row.GetTimeProfileId(DataRowVersion.Original));
				guid3 = new Guid(row.GetTimeProfileId(DataRowVersion.Current));
			}
			else if (row.RowState == DataRowState.Added)
			{
				guid3 = new Guid(row.GetTimeProfileId(DataRowVersion.Current));
			}
			else if (row.RowState == DataRowState.Deleted)
			{
				guid2 = new Guid(row.GetTimeProfileId(DataRowVersion.Original));
			}
			string text = ((guid2 == Guid.Empty) ? "Always" : GetOrCreateTimeProfileName(nameCache, guid2));
			string text2 = ((guid3 == Guid.Empty) ? "Always" : GetOrCreateTimeProfileName(nameCache, guid3));
			auditLogEntries.Add((now, LogEntryAuditFactory.AuditRoleDefaultTimeProfileChanged(managementServerName, text, text2, orCreateRoleName, guid.ToString(), userCredentials, locationIPAddress)));
			_auditAndSystemLogger.Log(LogEntryId.AuditRoleDefaultTimeProfileChanged, guid, text, managementServerName, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleDefaultTimeProfileChanged, text, text2, orCreateRoleName);
		}
	}

	private void CreatePermissionMatrixLogs(SecurityDataSet allChanges, string managementServerName, string userCredentials, string locationIPAddress, Dictionary<string, string> nameCache, List<(DateTime, LogEntryAudit)> auditLogEntries, DateTime now)
	{
		DataTable changes = allChanges.SecurityPermissionsMatrix.GetChanges();
		foreach (SecurityDataSet.SecurityPermissionsMatrixRow row in changes.Rows)
		{
			Guid guid = new Guid(row.GetRoleId());
			Guid matrixId = new Guid(row.GetMatrixId());
			string securityAttributeId = row.GetSecurityAttributeId();
			string orCreateRoleName = GetOrCreateRoleName(nameCache, guid);
			string orCreateMatrixName = GetOrCreateMatrixName(nameCache, matrixId);
			LegacySecurityActionInformation securityActionInformation = LegacySecurityAttributeMapper.Instance.GetSecurityActionInformation(securityAttributeId);
			string orCreateNamespaceName = GetOrCreateNamespaceName(nameCache, securityActionInformation);
			string orCreateActionName = GetOrCreateActionName(nameCache, securityActionInformation);
			if (row.RowState == DataRowState.Added)
			{
				auditLogEntries.Add((now, LogEntryAuditFactory.AuditRoleObjectSecurityGranted(guid, orCreateRoleName, managementServerName, orCreateActionName, orCreateNamespaceName, orCreateMatrixName, userCredentials, locationIPAddress)));
				_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityGranted, guid, orCreateRoleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityGranted, orCreateActionName, orCreateNamespaceName, orCreateMatrixName);
			}
			else if (row.RowState == DataRowState.Deleted)
			{
				auditLogEntries.Add((now, LogEntryAuditFactory.AuditRoleObjectSecurityRevoked(guid, orCreateRoleName, managementServerName, orCreateActionName, orCreateNamespaceName, orCreateMatrixName, userCredentials, locationIPAddress)));
				_auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRevoked, guid, orCreateRoleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRevoked, orCreateActionName, orCreateNamespaceName, orCreateMatrixName);
			}
		}
	}

	private static LogEntryAudit HandleAddedPermissionPlaybackLog(ILocalizedLogger auditAndSystemLogger, string managementServerName, string userCredentials, string locationIpAddress, SecurityDataSet.SecurityPermissionsDeviceRow row, Guid roleId, string roleName, string actionName, string namespaceName, string deviceName)
	{
		if (row.TimeBackward == 0)
		{
			return null;
		}
		TimeSpan timeSpan = TimeSpan.FromMinutes(row.TimeBackward);
		if (timeSpan < TimeSpan.FromDays(1.0))
		{
			auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIpAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes, actionName, namespaceName, row.TimeBackward, deviceName);
			return LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, row.TimeBackward, userCredentials, locationIpAddress);
		}
		auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIpAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays, actionName, namespaceName, timeSpan.Days, deviceName);
		return LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, timeSpan.Days, userCredentials, locationIpAddress);
	}

	private static LogEntryAudit HandleModifiedPermissionPlaybackLog(ILocalizedLogger auditAndSystemLogger, string managementServerName, string userCredentials, string locationIpAddress, SecurityDataSet.SecurityPermissionsDeviceRow row, Guid roleId, string roleName, string actionName, string namespaceName, string deviceName)
	{
		TimeSpan timeSpan = TimeSpan.FromMinutes(row.GetTimeBackward(DataRowVersion.Original));
		TimeSpan timeSpan2 = TimeSpan.FromMinutes(row.GetTimeBackward(DataRowVersion.Current));
		if (timeSpan == timeSpan2)
		{
			return null;
		}
		if (timeSpan == TimeSpan.Zero)
		{
			if (timeSpan2 == TimeSpan.Zero)
			{
				return null;
			}
			if (timeSpan2 < TimeSpan.FromDays(1.0))
			{
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIpAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes, actionName, namespaceName, (int)timeSpan2.TotalMinutes, deviceName);
				return LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, (int)timeSpan2.TotalMinutes, userCredentials, locationIpAddress);
			}
			auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIpAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays, actionName, namespaceName, (int)timeSpan2.TotalDays, deviceName);
			return LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, (int)timeSpan2.TotalDays, userCredentials, locationIpAddress);
		}
		if (timeSpan < TimeSpan.FromDays(1.0))
		{
			if (timeSpan2 == TimeSpan.Zero)
			{
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIpAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction, actionName, namespaceName, (int)timeSpan.TotalMinutes, deviceName);
				return LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, (int)timeSpan.TotalMinutes, userCredentials, locationIpAddress);
			}
			if (timeSpan2 < TimeSpan.FromDays(1.0))
			{
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIpAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes, actionName, namespaceName, (int)timeSpan.TotalMinutes, (int)timeSpan2.TotalMinutes, deviceName);
				return LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, (int)timeSpan.TotalMinutes, (int)timeSpan2.TotalMinutes, userCredentials, locationIpAddress);
			}
			auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIpAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays, actionName, namespaceName, (int)timeSpan.TotalMinutes, (int)timeSpan2.TotalDays, deviceName);
			return LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, (int)timeSpan.TotalMinutes, (int)timeSpan2.TotalDays, userCredentials, locationIpAddress);
		}
		if (timeSpan2 == TimeSpan.Zero)
		{
			auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIpAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction, actionName, namespaceName, (int)timeSpan.TotalDays, deviceName);
			return LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, (int)timeSpan.TotalDays, userCredentials, locationIpAddress);
		}
		if (timeSpan2 < TimeSpan.FromDays(1.0))
		{
			auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIpAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes, actionName, namespaceName, (int)timeSpan.TotalDays, (int)timeSpan2.TotalMinutes, deviceName);
			return LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, (int)timeSpan.TotalDays, (int)timeSpan2.TotalMinutes, userCredentials, locationIpAddress);
		}
		auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIpAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays, actionName, namespaceName, (int)timeSpan.TotalDays, (int)timeSpan2.TotalDays, deviceName);
		return LogEntryAuditFactory.AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, (int)timeSpan.TotalDays, (int)timeSpan2.TotalDays, userCredentials, locationIpAddress);
	}

	private static List<LogEntryAudit> HandleDeletedSecuritySettingLog(ILocalizedLogger auditAndSystemLogger, string securityAttributeId, string managementServerName, string userCredentials, string locationIPAddress, SecurityDataSet.SecurityPermissionsDeviceRow row, Guid roleId, string roleName, string deviceName)
	{
		List<LogEntryAudit> list = new List<LogEntryAudit>();
		if (securityAttributeId == "DEVICESPEAKPRIORITY")
		{
			SpeakPriority speakPriority = SpeakPriority.Medium;
			string text = "Speak priority";
			string text2 = "Speakers";
			list.Add(LogEntryAuditFactory.AuditRoleObjectSecurityPriorityRevoked(roleId, roleName, managementServerName, text, text2, deviceName, speakPriority.ToDescription(), userCredentials, locationIPAddress));
			auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityRevoked, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityRevoked, text, text2, deviceName, speakPriority);
			SpeakPriority speakPriority2 = (SpeakPriority)Enum.Parse(typeof(SpeakPriority), GetValue(row, DataRowVersion.Original), ignoreCase: true);
			if (speakPriority2 != speakPriority)
			{
				list.Add(LogEntryAuditFactory.AuditRoleObjectSecurityPriorityChanged(roleId, roleName, managementServerName, text, text2, speakPriority2.ToDescription(), speakPriority.ToDescription(), deviceName, userCredentials, locationIPAddress));
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityChanged, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityChanged, text, text2, speakPriority2.ToDescription(), speakPriority.ToDescription(), deviceName);
			}
		}
		else if (securityAttributeId == "DEVICEPTZPRIORITY")
		{
			string text3 = "3000";
			string text4 = "PTZ Priority";
			string text5 = "Cameras";
			list.Add(LogEntryAuditFactory.AuditRoleObjectSecurityPriorityRevoked(roleId, roleName, managementServerName, text4, text5, deviceName, text3, userCredentials, locationIPAddress));
			auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityRevoked, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityRevoked, text4, text5, deviceName, text3);
			string value = GetValue(row, DataRowVersion.Original);
			if (value != text3)
			{
				list.Add(LogEntryAuditFactory.AuditRoleObjectSecurityPriorityChanged(roleId, roleName, managementServerName, text4, text5, value, text3, deviceName, userCredentials, locationIPAddress));
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityChanged, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityChanged, text4, text5, value, text3, deviceName);
			}
		}
		return list;
	}

	private static LogEntryAudit HandleModifiedSecuritySettingLog(ILocalizedLogger auditAndSystemLogger, string securityAttributeId, string managementServerName, string userCredentials, string locationIPAddress, SecurityDataSet.SecurityPermissionsDeviceRow row, Guid roleId, string roleName, string deviceName)
	{
		if (securityAttributeId == "DEVICESPEAKPRIORITY")
		{
			SpeakPriority speakPriority = (SpeakPriority)Enum.Parse(typeof(SpeakPriority), GetValue(row, DataRowVersion.Original), ignoreCase: true);
			SpeakPriority speakPriority2 = (SpeakPriority)Enum.Parse(typeof(SpeakPriority), GetValue(row, DataRowVersion.Current), ignoreCase: true);
			if (speakPriority != speakPriority2)
			{
				string text = speakPriority.ToDescription();
				string text2 = speakPriority2.ToDescription();
				string text3 = "Speak priority";
				string text4 = "Speakers";
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityChanged, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityChanged, text3, text4, text, text2, deviceName);
				return LogEntryAuditFactory.AuditRoleObjectSecurityPriorityChanged(roleId, roleName, managementServerName, text3, text4, speakPriority.ToDescription(), speakPriority2.ToDescription(), deviceName, userCredentials, locationIPAddress);
			}
		}
		else if (securityAttributeId == "DEVICEPTZPRIORITY")
		{
			string value = GetValue(row, DataRowVersion.Original);
			string value2 = GetValue(row, DataRowVersion.Current);
			if (value != value2)
			{
				string text5 = "PTZ Priority";
				string text6 = "Cameras";
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityChanged, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityChanged, text5, text6, value, value2, deviceName);
				return LogEntryAuditFactory.AuditRoleObjectSecurityPriorityChanged(roleId, roleName, managementServerName, text5, text6, value, value2, deviceName, userCredentials, locationIPAddress);
			}
		}
		return null;
	}

	private static List<LogEntryAudit> HandleAddedSecuritySettingLog(ILocalizedLogger auditAndSystemLogger, string securityAttributeId, string managementServerName, string userCredentials, string locationIPAddress, SecurityDataSet.SecurityPermissionsDeviceRow row, Guid roleId, string roleName, string deviceName)
	{
		List<LogEntryAudit> list = new List<LogEntryAudit>();
		if (securityAttributeId == "DEVICESPEAKPRIORITY")
		{
			SpeakPriority speakPriority = SpeakPriority.Medium;
			string text = "Speak priority";
			string text2 = "Speakers";
			list.Add(LogEntryAuditFactory.AuditRoleObjectSecurityPriorityGranted(roleId, roleName, managementServerName, text, text2, deviceName, speakPriority.ToDescription(), userCredentials, locationIPAddress));
			auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityGranted, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityGranted, text, text2, deviceName, speakPriority.ToDescription());
			SpeakPriority speakPriority2 = (SpeakPriority)Enum.Parse(typeof(SpeakPriority), GetValue(row, DataRowVersion.Current), ignoreCase: true);
			if (speakPriority2 != speakPriority)
			{
				list.Add(LogEntryAuditFactory.AuditRoleObjectSecurityPriorityChanged(roleId, roleName, managementServerName, text, text2, speakPriority.ToDescription(), speakPriority2.ToDescription(), deviceName, userCredentials, locationIPAddress));
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityChanged, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityChanged, text, text2, speakPriority.ToDescription(), speakPriority2.ToDescription(), deviceName);
			}
		}
		else if (securityAttributeId == "DEVICEPTZPRIORITY")
		{
			string text3 = "3000";
			string text4 = "PTZ Priority";
			string text5 = "Cameras";
			list.Add(LogEntryAuditFactory.AuditRoleObjectSecurityPriorityGranted(roleId, roleName, managementServerName, text4, text5, deviceName, text3, userCredentials, locationIPAddress));
			auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityGranted, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityGranted, text4, text5, deviceName, text3);
			string value = GetValue(row, DataRowVersion.Current);
			if (value != text3)
			{
				list.Add(LogEntryAuditFactory.AuditRoleObjectSecurityPriorityChanged(roleId, roleName, managementServerName, text4, text5, text3, value, deviceName, userCredentials, locationIPAddress));
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityPriorityChanged, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityPriorityChanged, text4, text5, text3, value, deviceName);
			}
		}
		return list;
	}

	private static LogEntryAudit HandleModifiedPermissionOnlyAllowLoginWithinTimeProfileLog(ILocalizedLogger auditAndSystemLogger, string managementServerName, string userCredentials, string locationIPAddress, SecurityDataSet.SecurityPermissionsRow row, string roleName, Guid roleId, IVmoCache vmoCache)
	{
		string text = string.Empty;
		string text2 = string.Empty;
		if (IsTimeProfileDefault(row, DataRowVersion.Current))
		{
			text2 = "Default";
			if (IsTimeProfileDefault(row, DataRowVersion.Original))
			{
				return null;
			}
			if (IsTimeProfileAlways(row, DataRowVersion.Original))
			{
				text = "Always";
			}
			if (IsTimeProfileCustom(row, DataRowVersion.Original))
			{
				text = vmoCache.GetTimeProfileName(new Guid(row.GetTimeProfileId(DataRowVersion.Original)));
			}
		}
		else if (IsTimeProfileAlways(row, DataRowVersion.Current))
		{
			text2 = "Always";
			if (IsTimeProfileAlways(row, DataRowVersion.Original))
			{
				return null;
			}
			if (IsTimeProfileDefault(row, DataRowVersion.Original))
			{
				text = "Default";
			}
			if (IsTimeProfileCustom(row, DataRowVersion.Original))
			{
				text = vmoCache.GetTimeProfileName(new Guid(row.GetTimeProfileId(DataRowVersion.Original)));
			}
		}
		else if (IsTimeProfileCustom(row, DataRowVersion.Current))
		{
			text2 = vmoCache.GetTimeProfileName(new Guid(row.GetTimeProfileId(DataRowVersion.Current)));
			if (IsTimeProfileAlways(row, DataRowVersion.Original))
			{
				text = "Always";
			}
			if (IsTimeProfileDefault(row, DataRowVersion.Original))
			{
				text = "Default";
			}
			if (IsTimeProfileCustom(row, DataRowVersion.Original))
			{
				if (!(row.GetTimeProfileId(DataRowVersion.Original) != row.GetTimeProfileId(DataRowVersion.Current)))
				{
					return null;
				}
				text = vmoCache.GetTimeProfileName(new Guid(row.GetTimeProfileId(DataRowVersion.Original)));
			}
		}
		auditAndSystemLogger.Log(LogEntryId.AuditRoleOnlyAllowLoginWithinTimeProfileChanged, roleId, text, managementServerName, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleOnlyAllowLoginWithinTimeProfileChanged, text, text2, roleName);
		return LogEntryAuditFactory.AuditRoleOnlyAllowLoginWithinTimeProfileChanged(managementServerName, text, text2, roleName, roleId.ToString(), userCredentials, locationIPAddress);
	}

	private static LogEntryAudit HandleModifiedPermissionTimeProfileLog(ILocalizedLogger auditAndSystemLogger, string managementServerName, string userCredentials, string locationIPAddress, SecurityDataSet.SecurityPermissionsDeviceRow row, Guid roleId, string roleName, string actionName, string namespaceName, string deviceName, IVmoCache vmoCache)
	{
		if (IsTimeProfileDefault(row, DataRowVersion.Current))
		{
			if (IsTimeProfileAlways(row, DataRowVersion.Original))
			{
				string text = "Always";
				string text2 = "Default";
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange, actionName, namespaceName, text, text2, deviceName);
				return LogEntryAuditFactory.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, text, text2, userCredentials, locationIPAddress);
			}
			if (IsTimeProfileDefault(row, DataRowVersion.Original))
			{
				return null;
			}
			if (IsTimeProfileCustom(row, DataRowVersion.Original))
			{
				string timeProfileName = vmoCache.GetTimeProfileName(new Guid(row.GetIDTimeProfile(DataRowVersion.Original)));
				string text3 = "Default";
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityTimeProfileToSystemProfileChange, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityTimeProfileToSystemProfileChange, actionName, namespaceName, timeProfileName, text3, deviceName);
				return LogEntryAuditFactory.AuditRoleObjectSecurityTimeProfileToSystemProfileChange(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, timeProfileName, text3, userCredentials, locationIPAddress);
			}
		}
		else if (IsTimeProfileAlways(row, DataRowVersion.Current))
		{
			if (IsTimeProfileAlways(row, DataRowVersion.Original))
			{
				return null;
			}
			if (IsTimeProfileDefault(row, DataRowVersion.Original))
			{
				string text4 = "Default";
				string text5 = "Always";
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange, actionName, namespaceName, text4, text5, deviceName);
				return LogEntryAuditFactory.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, text4, text5, userCredentials, locationIPAddress);
			}
			if (IsTimeProfileCustom(row, DataRowVersion.Original))
			{
				string timeProfileName2 = vmoCache.GetTimeProfileName(new Guid(row.GetIDTimeProfile(DataRowVersion.Original)));
				string text6 = "Always";
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityTimeProfileToSystemProfileChange, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityTimeProfileToSystemProfileChange, actionName, namespaceName, timeProfileName2, text6, deviceName);
				return LogEntryAuditFactory.AuditRoleObjectSecurityTimeProfileToSystemProfileChange(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, timeProfileName2, text6, userCredentials, locationIPAddress);
			}
		}
		else if (IsTimeProfileCustom(row, DataRowVersion.Current))
		{
			string timeProfileName3 = vmoCache.GetTimeProfileName(new Guid(row.IDTimeProfile));
			if (IsTimeProfileAlways(row, DataRowVersion.Original))
			{
				string text7 = "Always";
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToProfileChange, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecuritySystemTimeProfileToProfileChange, actionName, namespaceName, text7, timeProfileName3, deviceName);
				return LogEntryAuditFactory.AuditRoleObjectSecuritySystemTimeProfileToProfileChange(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, text7, timeProfileName3, userCredentials, locationIPAddress);
			}
			if (IsTimeProfileDefault(row, DataRowVersion.Original))
			{
				string text8 = "Default";
				auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToProfileChange, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecuritySystemTimeProfileToProfileChange, actionName, namespaceName, text8, timeProfileName3, deviceName);
				return LogEntryAuditFactory.AuditRoleObjectSecuritySystemTimeProfileToProfileChange(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, text8, timeProfileName3, userCredentials, locationIPAddress);
			}
			if (IsTimeProfileCustom(row, DataRowVersion.Original))
			{
				if (row.GetIDTimeProfile(DataRowVersion.Original) != row.GetIDTimeProfile(DataRowVersion.Current))
				{
					string timeProfileName4 = vmoCache.GetTimeProfileName(new Guid(row.GetIDTimeProfile(DataRowVersion.Original)));
					auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecurityTimeProfileToProfileChange, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecurityTimeProfileToProfileChange, actionName, namespaceName, timeProfileName4, timeProfileName3, deviceName);
					return LogEntryAuditFactory.AuditRoleObjectSecurityTimeProfileToProfileChange(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, timeProfileName4, timeProfileName3, userCredentials, locationIPAddress);
				}
				return null;
			}
		}
		return null;
	}

	private static LogEntryAudit HandleAddedPermissionTimeProfileLog(ILocalizedLogger auditAndSystemLogger, string managementServerName, string userCredentials, string locationIPAddress, SecurityDataSet.SecurityPermissionsDeviceRow row, Guid roleId, string roleName, string actionName, string namespaceName, string deviceName, IVmoCache vmoCache)
	{
		if (IsTimeProfileDefault(row, DataRowVersion.Current))
		{
			string text = "Always";
			string text2 = "Default";
			auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Server, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange, actionName, namespaceName, text, text2, deviceName);
			return LogEntryAuditFactory.AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, text, text2, userCredentials, locationIPAddress);
		}
		if (IsTimeProfileCustom(row, DataRowVersion.Current))
		{
			string timeProfileName = vmoCache.GetTimeProfileName(new Guid(row.IDTimeProfile));
			string text3 = "Always";
			auditAndSystemLogger.Log(LogEntryId.AuditRoleObjectSecuritySystemTimeProfileToProfileChange, roleId, roleName, managementServerName, ResourceType.Roles, SourceType.Role, locationIPAddress, userCredentials, AuditType.Management, AccessGranted.Granted, () => LogDefinitions.AuditRoleObjectSecuritySystemTimeProfileToProfileChange, actionName, namespaceName, text3, timeProfileName, deviceName);
			return LogEntryAuditFactory.AuditRoleObjectSecuritySystemTimeProfileToProfileChange(roleId, roleName, managementServerName, actionName, namespaceName, deviceName, text3, timeProfileName, userCredentials, locationIPAddress);
		}
		return null;
	}

	private static bool IsTimeProfileAlways(SecurityDataSet.SecurityPermissionsDeviceRow row, DataRowVersion dataRowVersion)
	{
		if (!row.GetIsDefaultTimeProfile(dataRowVersion))
		{
			return row.IsIDTimeProfileNull(dataRowVersion);
		}
		return false;
	}

	private static bool IsTimeProfileDefault(SecurityDataSet.SecurityPermissionsDeviceRow row, DataRowVersion dataRowVersion)
	{
		return row.GetIsDefaultTimeProfile(dataRowVersion);
	}

	private static bool IsTimeProfileAlways(SecurityDataSet.SecurityPermissionsRow row, DataRowVersion dataRowVersion)
	{
		if (!row.GetIsDefaultTimeProfile(dataRowVersion))
		{
			return row.IsIDTimeProfileNull(dataRowVersion);
		}
		return false;
	}

	private static bool IsTimeProfileDefault(SecurityDataSet.SecurityPermissionsRow row, DataRowVersion dataRowVersion)
	{
		return row.GetIsDefaultTimeProfile(dataRowVersion);
	}

	private static string GetValue(SecurityDataSet.SecurityPermissionsDeviceRow row, DataRowVersion dataRowVersion)
	{
		return row.GetValue(dataRowVersion);
	}

	private static bool IsTimeProfileCustom(SecurityDataSet.SecurityPermissionsDeviceRow row, DataRowVersion dataRowVersion)
	{
		if (!row.GetIsDefaultTimeProfile(dataRowVersion))
		{
			return !row.IsIDTimeProfileNull(dataRowVersion);
		}
		return false;
	}

	private static bool IsTimeProfileCustom(SecurityDataSet.SecurityPermissionsRow row, DataRowVersion dataRowVersion)
	{
		if (!row.GetIsDefaultTimeProfile(dataRowVersion))
		{
			return !row.IsIDTimeProfileNull(dataRowVersion);
		}
		return false;
	}

	private static string GetOrCreateNamespaceName(Dictionary<string, string> nameList, LegacySecurityActionInformation actionInfo)
	{
		string key = actionInfo.NamespaceId.ToString();
		nameList.TryGetValue(key, out var value);
		if (value == null)
		{
			value = SecurityNamespaceCache.GetSecurityNamespaceNameByLegacySecurityAction(actionInfo);
			nameList.Add(key, value);
		}
		return value;
	}

	private static string GetOrCreateActionName(Dictionary<string, string> nameList, LegacySecurityActionInformation actionInfo)
	{
		string key = actionInfo.NamespaceId.ToString() + actionInfo.SecurityAction;
		nameList.TryGetValue(key, out var value);
		if (value == null)
		{
			value = SecurityNamespaceCache.GetActionNameByLegacySecurityAction(actionInfo);
			nameList.Add(key, value);
		}
		return value;
	}

	private string GetOrCreateDeviceName(Dictionary<string, string> nameList, Guid deviceId)
	{
		nameList.TryGetValue(deviceId.ToString(), out var value);
		if (value == null)
		{
			value = _configurationDataComponent.FindDeviceNameByDeviceId(deviceId);
			nameList.Add(deviceId.ToString(), value);
		}
		return value;
	}

	private string GetOrCreateMatrixName(Dictionary<string, string> nameList, Guid matrixId)
	{
		nameList.TryGetValue(matrixId.ToString(), out var value);
		if (value == null)
		{
			value = _configurationDataComponent.FindMatrixNameByMatrixId(matrixId);
			nameList.Add(matrixId.ToString(), value);
		}
		return value;
	}

	private string GetOrCreateRoleName(Dictionary<string, string> nameList, Guid roleId)
	{
		nameList.TryGetValue(roleId.ToString(), out var value);
		if (value == null)
		{
			value = _configurationDataComponent.FindRoleNameByRoleId(roleId);
			nameList.Add(roleId.ToString(), value);
		}
		return value;
	}

	private string GetOrCreateTimeProfileName(Dictionary<string, string> nameList, Guid timeProfileId)
	{
		nameList.TryGetValue(timeProfileId.ToString(), out var value);
		if (value == null)
		{
			value = _configurationDataComponent.FindTimeProfileNameByTimeProfileId(timeProfileId);
			nameList.Add(timeProfileId.ToString(), value);
		}
		return value;
	}
}
