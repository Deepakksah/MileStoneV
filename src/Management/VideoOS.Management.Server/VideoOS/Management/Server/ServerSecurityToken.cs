using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Security;
using VideoOS.Common.SharedTimeProfile;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public static class ServerSecurityToken
{
	private class TimeProfileList
	{
		public DateTime StartTime;

		public DateTime EndTime;

		public TimeSpan Timeout;

		public List<string> TestedTimeProfileList;

		public TimeProfileList(DateTime startTime, DateTime endTime, List<string> testedTimeProfileList)
		{
			StartTime = startTime;
			EndTime = endTime;
			TestedTimeProfileList = testedTimeProfileList;
			Timeout = EndTime - StartTime;
		}
	}

	public static SecurityToken ManagementLogin(IGeminiRequestContext requestContext, string locationIpAddress)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (!requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "CONNECT"))
		{
			using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
			{
				vmoLogManager.LogEntryAuditLogger.AuditLoginUnsuccessfullyInsufficientRights(LogEntryAuditLogger.LoginFrom.Administration, requestContext.UserDomainAccountName, locationIpAddress);
			}
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
		if (!requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			using (VmoLogManager vmoLogManager2 = new VmoLogManager(requestContext))
			{
				vmoLogManager2.LogEntryAuditLogger.AuditLoginUnsuccessfullyInsufficientRights(LogEntryAuditLogger.LoginFrom.Administration, requestContext.UserDomainAccountName, locationIpAddress);
			}
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
		if (!IdentityCache.IsMember(requestContext, requestContext.AuthenticatedUserSid, SecurityManager.AdministratorRoleId.ToString()) && !GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "DifferentiatedAdministratorSecurity"))
		{
			using (VmoLogManager vmoLogManager3 = new VmoLogManager(requestContext))
			{
				vmoLogManager3.LogEntryAuditLogger.AuditLoginUnsuccessfullyInsufficientRights(LogEntryAuditLogger.LoginFrom.Administration, requestContext.UserDomainAccountName, locationIpAddress);
			}
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
		int minutes = Convert.ToInt32(Config.Instance.GetAppSetting("ClientTokenTimoutMinutes", "240"), CultureInfo.InvariantCulture);
		TimeSpan timeout = new TimeSpan(0, minutes, 0);
		using ISecurityTokenComponent securityTokenComponent = requestContext.CreateSecurityTokenComponent();
		return securityTokenComponent.AddToken(requestContext.AuthenticatedUserSid, timeout, managementClientToken: true);
	}

	public static SecurityToken Login(IGeminiRequestContext requestContext, string locationIpAddress, bool isRefreshToken)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		DateTime evaluationTime = DateTime.UtcNow;
		string[] timeProfileIds = null;
		if (!requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "CONNECT"))
		{
			using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
			{
				vmoLogManager.LogEntryAuditLogger.AuditLoginUnsuccessfullyInsufficientRights(LogEntryAuditLogger.LoginFrom.CommandApi, requestContext.UserDomainAccountName, locationIpAddress);
			}
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
		if (!requestContext.GetSecurityManagerService().HasApplicationLoginPermission(requestContext, out evaluationTime, out timeProfileIds))
		{
			if (timeProfileIds.Length != 0)
			{
				using (VmoLogManager vmoLogManager2 = new VmoLogManager(requestContext))
				{
					vmoLogManager2.LogEntryAuditLogger.AuditLoginTimeBased(requestContext.UserDomainAccountName, locationIpAddress);
				}
				throw new TimeBasedUnauthorizedAccessException(VmoResources.VMO61008);
			}
			using (VmoLogManager vmoLogManager3 = new VmoLogManager(requestContext))
			{
				vmoLogManager3.LogEntryAuditLogger.AuditLoginUnsuccessfullyInsufficientRights(LogEntryAuditLogger.LoginFrom.CommandApi, requestContext.UserDomainAccountName, locationIpAddress);
			}
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
		int num = Convert.ToInt32(Config.Instance.GetAppSetting("ClientTokenTimoutMinutes", "240"), CultureInfo.InvariantCulture);
		TimeSpan timeSpan = new TimeSpan(0, num, 0);
		if (timeProfileIds != null && timeProfileIds.Any((string f) => f != null))
		{
			TimeProfile[] array = null;
			using (ITimeProfileComponent timeProfileComponent = requestContext.CreateTimeProfileComponent())
			{
				array = timeProfileComponent.QueryTimeProfilesByIds(timeProfileIds);
			}
			if (array != null && array.Length != 0)
			{
				timeSpan = GetTokenTimeout(array, timeSpan, evaluationTime);
			}
		}
		if (timeSpan < new TimeSpan(0, 1, 0))
		{
			using (VmoLogManager vmoLogManager4 = new VmoLogManager(requestContext))
			{
				vmoLogManager4.LogEntryAuditLogger.AuditLoginTimeBased(requestContext.UserDomainAccountName, locationIpAddress);
			}
			throw new TimeBasedUnauthorizedAccessException(VmoResources.VMO61008);
		}
		SecurityToken securityToken;
		using (ISecurityTokenComponent securityTokenComponent = requestContext.CreateSecurityTokenComponent())
		{
			securityToken = securityTokenComponent.AddToken(requestContext.AuthenticatedUserSid, timeSpan, managementClientToken: false);
		}
		securityToken.ExpireTimeLimited = timeSpan.TotalMinutes < (double)num;
		if (!isRefreshToken)
		{
			using VmoLogManager vmoLogManager5 = new VmoLogManager(requestContext);
			vmoLogManager5.LogEntryAuditLogger.AuditLoginSuccessfully(LogEntryAuditLogger.LoginFrom.CommandApi, requestContext.UserDomainAccountName, locationIpAddress);
		}
		return securityToken;
	}

	private static TimeSpan GetTokenTimeout(TimeProfile[] timeProfiles, TimeSpan serverTokenTimeout, DateTime evaluationTime)
	{
		bool flag = false;
		List<TimeProfileList> list = new List<TimeProfileList>();
		TimeProfileManager timeProfileManager = new TimeProfileManager();
		TimeSpan timeSpan = TimeSpan.Zero;
		TimeProfile[] array = timeProfiles;
		foreach (TimeProfile timeProfile in array)
		{
			Guid id = new Guid(timeProfile.Id);
			VideoOS.Common.SharedTimeProfile.TimeProfileType timeProfileType;
			if (timeProfile.Type == TimeProfileType.Calendar)
			{
				timeProfileType = VideoOS.Common.SharedTimeProfile.TimeProfileType.CalendarProfile;
			}
			else
			{
				if (timeProfile.Type != TimeProfileType.Sunclock)
				{
					return serverTokenTimeout;
				}
				timeProfileType = VideoOS.Common.SharedTimeProfile.TimeProfileType.SunclockProfile;
			}
			timeProfileManager.AddTimeProfile(id, timeProfile.DefinitionXml, timeProfileType);
			if (!timeProfileManager.EvaluateTimeProfile(id, evaluationTime))
			{
				continue;
			}
			TimeSpan appointmentDuration = timeProfileManager.GetAppointmentDuration(id, evaluationTime, serverTokenTimeout);
			if (appointmentDuration > TimeSpan.Zero)
			{
				List<string> list2 = new List<string>();
				list2.Add(timeProfile.Id);
				DateTime endTime = evaluationTime.Add(appointmentDuration);
				TimeProfileList timeProfileList = new TimeProfileList(evaluationTime, endTime, list2);
				list.Add(timeProfileList);
				if (timeSpan < timeProfileList.Timeout)
				{
					timeSpan = timeProfileList.Timeout;
				}
			}
		}
		int num = 1;
		while (!flag)
		{
			bool flag2 = false;
			List<TimeProfileList> list3 = new List<TimeProfileList>();
			foreach (TimeProfileList item in list)
			{
				if (item.TestedTimeProfileList.Count != num)
				{
					continue;
				}
				flag2 = true;
				array = timeProfiles;
				foreach (TimeProfile timeProfile2 in array)
				{
					Guid id2 = new Guid(timeProfile2.Id);
					if (!timeProfileManager.EvaluateTimeProfile(id2, item.EndTime))
					{
						continue;
					}
					TimeSpan appointmentDuration2 = timeProfileManager.GetAppointmentDuration(id2, item.EndTime, serverTokenTimeout);
					if (!(appointmentDuration2 > TimeSpan.Zero))
					{
						continue;
					}
					List<string> list4 = new List<string>();
					foreach (string testedTimeProfile in item.TestedTimeProfileList)
					{
						list4.Add(testedTimeProfile);
					}
					list4.Add(timeProfile2.Id);
					DateTime endTime2 = item.EndTime.Add(appointmentDuration2);
					TimeProfileList timeProfileList2 = new TimeProfileList(item.StartTime, endTime2, list4);
					list3.Add(timeProfileList2);
					if (timeSpan < timeProfileList2.Timeout)
					{
						timeSpan = timeProfileList2.Timeout;
					}
				}
			}
			foreach (TimeProfileList item2 in list3)
			{
				list.Add(item2);
			}
			num++;
			if (!flag2)
			{
				flag = true;
			}
			if (timeSpan >= serverTokenTimeout)
			{
				flag = true;
			}
		}
		if (timeSpan > serverTokenTimeout)
		{
			timeSpan = serverTokenTimeout;
		}
		return timeSpan;
	}
}
