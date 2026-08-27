using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Reflection;
using System.Security.Principal;
using System.ServiceModel;
using System.Threading;
using System.Web;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;
using VideoOS.SCH.Services.DataAccess;
using VideoOS.SCH.Services.DataAccess.Generic;

namespace VideoOS.SCH.Services.Business.Components;

public class Diagnostics
{
	private delegate string GetConfigSettingDelegate();

	public string Ping(string pingText)
	{
		return pingText + " from " + Environment.MachineName + " @ " + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss 'GMT'", CultureInfo.InvariantCulture);
	}

	private TimeSpan GetSystemUpTime()
	{
		try
		{
			using ManagementClass managementClass = new ManagementClass("Win32_OperatingSystem");
			using ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectCollection.GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				DateTime value = ManagementDateTimeConverter.ToDateTime(((ManagementObject)managementObjectEnumerator.Current).Properties["LastBootUpTime"].Value.ToString());
				return DateTime.Now.Subtract(value);
			}
		}
		catch
		{
		}
		return TimeSpan.MinValue;
	}

	private static DateTime GetProcessStartTime(string processName)
	{
		using (ManagementClass managementClass = new ManagementClass("Win32_Process"))
		{
			using ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
			foreach (ManagementObject item in managementObjectCollection)
			{
				if (item["Name"].ToString().ToLower() == processName && item["CreationDate"] != null)
				{
					return ManagementDateTimeConverter.ToDateTime(item["CreationDate"].ToString());
				}
			}
		}
		return DateTime.MinValue;
	}

	private static DateTime GetSystemStartTime()
	{
		try
		{
			DateTime processStartTime = GetProcessStartTime("system");
			if (processStartTime == DateTime.MinValue)
			{
				processStartTime = GetProcessStartTime("services.exe");
			}
			return processStartTime;
		}
		catch
		{
		}
		return DateTime.MinValue;
	}

	private string GetSystemType()
	{
		try
		{
			using ManagementClass managementClass = new ManagementClass("Win32_ComputerSystem");
			using ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectCollection.GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				return ((ManagementObject)managementObjectEnumerator.Current).Properties["SystemType"].Value.ToString();
			}
		}
		catch
		{
		}
		return string.Empty;
	}

	private string GetOSTotalVisibleMemorySize()
	{
		try
		{
			using ManagementClass managementClass = new ManagementClass("Win32_OperatingSystem");
			using ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectCollection.GetEnumerator();
			if (managementObjectEnumerator.MoveNext() && ulong.TryParse(((ManagementObject)managementObjectEnumerator.Current).Properties["TotalVisibleMemorySize"].Value.ToString(), out var result))
			{
				return result.ToString("#,#", CultureInfo.InvariantCulture);
			}
		}
		catch
		{
		}
		return string.Empty;
	}

	private string GetOSFreePhysicalMemory()
	{
		try
		{
			using ManagementClass managementClass = new ManagementClass("Win32_OperatingSystem");
			using ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectCollection.GetEnumerator();
			if (managementObjectEnumerator.MoveNext() && ulong.TryParse(((ManagementObject)managementObjectEnumerator.Current).Properties["FreePhysicalMemory"].Value.ToString(), out var result))
			{
				return result.ToString("#,#", CultureInfo.InvariantCulture);
			}
		}
		catch
		{
		}
		return string.Empty;
	}

	private string GetOSStatus()
	{
		try
		{
			using ManagementClass managementClass = new ManagementClass("Win32_OperatingSystem");
			using ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectCollection.GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				return ((ManagementObject)managementObjectEnumerator.Current).Properties["Status"].Value.ToString();
			}
		}
		catch
		{
		}
		return string.Empty;
	}

	private string GetOSCaption()
	{
		try
		{
			using ManagementClass managementClass = new ManagementClass("Win32_OperatingSystem");
			using ManagementObjectCollection managementObjectCollection = managementClass.GetInstances();
			using ManagementObjectCollection.ManagementObjectEnumerator managementObjectEnumerator = managementObjectCollection.GetEnumerator();
			if (managementObjectEnumerator.MoveNext())
			{
				return ((ManagementObject)managementObjectEnumerator.Current).Properties["Caption"].Value.ToString();
			}
		}
		catch
		{
		}
		return string.Empty;
	}

	public DiagSimpleKVPair[] GetServerInfo(out DateTime serverUtcDateTime, out string serverTimeZone, out string serverName, out string osVersion, out string clrRuntimeVersion)
	{
		List<DiagSimpleKVPair> list = new List<DiagSimpleKVPair>();
		serverName = Environment.MachineName;
		list.Add(CreateSimpleKV("ServerName", serverName));
		serverUtcDateTime = DateTime.UtcNow;
		list.Add(CreateSimpleKV("ServerUtcDateTime", serverUtcDateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
		serverTimeZone = DateTime.Now.ToString("zzz");
		list.Add(CreateSimpleKV("ServerTimeZone", serverTimeZone));
		list.Add(CreateSimpleKV("OSCaption", GetOSCaption()));
		osVersion = Environment.OSVersion.VersionString;
		list.Add(CreateSimpleKV("OSVersion", osVersion));
		clrRuntimeVersion = Environment.Version.ToString();
		list.Add(CreateSimpleKV("CLRRuntimeVersion", clrRuntimeVersion));
		list.Add(CreateSimpleKV("SystemStartTime", GetSystemStartTime().ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss 'GMT'", CultureInfo.InvariantCulture)));
		list.Add(CreateSimpleKV("SystemUpTime", GetSystemUpTime().ToString()));
		list.Add(CreateSimpleKV("SystemType", GetSystemType()));
		list.Add(CreateSimpleKV("OSTotalVisibleMemorySize", GetOSTotalVisibleMemorySize()));
		list.Add(CreateSimpleKV("OSFreePhysicalMemory", GetOSFreePhysicalMemory()));
		list.Add(CreateSimpleKV("OSStatus", GetOSStatus()));
		return list.ToArray();
	}

	public DiagSimpleKVPair[] GetIdentities()
	{
		List<DiagSimpleKVPair> list = new List<DiagSimpleKVPair>();
		list.Add(CreateSimpleKV("[Service Channel ClientSecurityInfo]", null));
		list.Add(CreateSimpleKV("schClientAuthType", ServiceContext.Current.ClientAuthenticationType.ToString()));
		list.Add(CreateSimpleKV("schClientIsAnonymous", ServiceContext.Current.ClientIsAnonymous.ToString(CultureInfo.InvariantCulture)));
		list.Add(CreateSimpleKV("schClientUserID", ServiceContext.Current.ClientUserID));
		list.Add(CreateSimpleKV("schClientUserDomain", ServiceContext.Current.ClientUserDomain));
		list.Add(CreateSimpleKV("[ServiceModel security]", null));
		if (OperationContext.Current != null && OperationContext.Current.ServiceSecurityContext != null)
		{
			list.Add(CreateSimpleKV("SvcIsAnonymous", OperationContext.Current.ServiceSecurityContext.IsAnonymous.ToString(CultureInfo.InvariantCulture)));
			if (OperationContext.Current.ServiceSecurityContext.PrimaryIdentity != null)
			{
				list.Add(CreateSimpleKV("SvcPrimaryIdentityName", OperationContext.Current.ServiceSecurityContext.PrimaryIdentity.Name));
			}
			if (OperationContext.Current.ServiceSecurityContext.WindowsIdentity != null)
			{
				list.Add(CreateSimpleKV("SvcWindowsIdentityName", OperationContext.Current.ServiceSecurityContext.WindowsIdentity.Name));
			}
		}
		list.Add(CreateSimpleKV("[Windows identity]", null));
		if (WindowsIdentity.GetCurrent() != null)
		{
			list.Add(CreateSimpleKV("WindowsIdentityName", WindowsIdentity.GetCurrent().Name));
		}
		list.Add(CreateSimpleKV("[HttpContext]", null));
		if (HttpContext.Current != null && HttpContext.Current.Request != null)
		{
			list.Add(CreateSimpleKV("HttpReq.UserHostName", HttpContext.Current.Request.UserHostName));
			list.Add(CreateSimpleKV("HttpReq.UserHostAddress", HttpContext.Current.Request.UserHostAddress));
			list.Add(CreateSimpleKV("HttpReq.IsAuthenticated", HttpContext.Current.Request.IsAuthenticated.ToString(CultureInfo.InvariantCulture)));
			list.Add(CreateSimpleKV("HttpReq.LogonUserIdentity.AuthenticationType", HttpContext.Current.Request.LogonUserIdentity.AuthenticationType));
			list.Add(CreateSimpleKV("HttpReq.LogonUserIdentity.IsAnonymous", HttpContext.Current.Request.LogonUserIdentity.IsAnonymous.ToString(CultureInfo.InvariantCulture)));
			list.Add(CreateSimpleKV("HttpReq.LogonUserIdentity.IsAuthenticated", HttpContext.Current.Request.LogonUserIdentity.IsAuthenticated.ToString(CultureInfo.InvariantCulture)));
			list.Add(CreateSimpleKV("HttpReq.LogonUserIdentity.IsGuest", HttpContext.Current.Request.LogonUserIdentity.IsGuest.ToString(CultureInfo.InvariantCulture)));
			list.Add(CreateSimpleKV("HttpReq.LogonUserIdentity.IsSystem", HttpContext.Current.Request.LogonUserIdentity.IsSystem.ToString(CultureInfo.InvariantCulture)));
			list.Add(CreateSimpleKV("HttpReq.LogonUserIdentity.Name", HttpContext.Current.Request.LogonUserIdentity.Name));
		}
		list.Add(CreateSimpleKV("[Thread principal]", null));
		if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity != null && !string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name))
		{
			list.Add(CreateSimpleKV("ThreadIdentityName", Thread.CurrentPrincipal.Identity.Name));
		}
		return list.ToArray();
	}

	private DiagMessage ConvertToContractDiagMessage(VideoOS.SCH.Services.Business.Definitions.Custom.Message busMessage)
	{
		DiagMessage diagMessage = new DiagMessage();
		diagMessage.ID = busMessage.ID;
		diagMessage.MessageTypeName = busMessage.MessageTypeName;
		diagMessage.Created = busMessage.Created;
		if (busMessage.Data != null)
		{
			List<DiagSimpleKVPair> list = new List<DiagSimpleKVPair>();
			foreach (MessageData value in busMessage.Data.Values)
			{
				DiagSimpleKVPair diagSimpleKVPair = new DiagSimpleKVPair();
				diagSimpleKVPair.Key = value.Key;
				diagSimpleKVPair.Value = value.Value;
				list.Add(diagSimpleKVPair);
			}
			diagMessage.Data = list.ToArray();
		}
		return diagMessage;
	}

	private DiagMessage[] ConvertToContractDiagMessages(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.Message> busMessages)
	{
		List<DiagMessage> list = new List<DiagMessage>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.Message busMessage in busMessages)
		{
			list.Add(ConvertToContractDiagMessage(busMessage));
		}
		return list.ToArray();
	}

	private DiagSession ConvertToContractDiagSession(VideoOS.SCH.Services.Business.Definitions.Custom.Session busSession)
	{
		return new DiagSession
		{
			ID = busSession.ID.ToString(),
			MessageIDs = busSession.MessageIDs.ToArray(),
			ClientName = busSession.ClientName,
			ClientUserName = busSession.ClientUserName,
			ExpiryTime = busSession.ExpiryTime
		};
	}

	private DiagSession[] ConvertToContractDiagSessions(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.Session> busSessions)
	{
		List<DiagSession> list = new List<DiagSession>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.Session busSession in busSessions)
		{
			list.Add(ConvertToContractDiagSession(busSession));
		}
		return list.ToArray();
	}

	private DiagMatchOperator ConvertToContractDiagMatchOp(MatchOperator busMatchOp)
	{
		return new DiagMatchOperator
		{
			Name = busMatchOp.Name,
			Description = busMatchOp.Description
		};
	}

	private DiagMatchOperator[] ConvertToContractDiagMatchOps(IEnumerable<MatchOperator> busMatchOps)
	{
		List<DiagMatchOperator> list = new List<DiagMatchOperator>();
		foreach (MatchOperator busMatchOp in busMatchOps)
		{
			list.Add(ConvertToContractDiagMatchOp(busMatchOp));
		}
		return list.ToArray();
	}

	private DiagMessageWatch ConvertToContractMsgWatchItem(VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch busMsgWatchItem)
	{
		DiagMessageWatch diagMessageWatch = new DiagMessageWatch();
		diagMessageWatch.SessionID = busMsgWatchItem.SessionID.ToString();
		diagMessageWatch.MessageTypeName = busMsgWatchItem.MessageTypeName;
		if (busMsgWatchItem.Expressions != null)
		{
			List<DiagMatchExpression> list = new List<DiagMatchExpression>();
			foreach (MatchExpression expression in busMsgWatchItem.Expressions)
			{
				DiagMatchExpression diagMatchExpression = new DiagMatchExpression();
				diagMatchExpression.Data = new DiagSimpleKVPair();
				diagMatchExpression.Data.Key = expression.Data.Key;
				diagMatchExpression.Data.Value = expression.Data.Value;
				diagMatchExpression.OperatorName = expression.OperatorName;
				list.Add(diagMatchExpression);
			}
			diagMessageWatch.Expressions = list.ToArray();
		}
		return diagMessageWatch;
	}

	private DiagMessageWatch[] ConvertToContractMsgWatchItems(IEnumerable<VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch> busMsgWatchItems)
	{
		List<DiagMessageWatch> list = new List<DiagMessageWatch>();
		foreach (VideoOS.SCH.Services.Business.Definitions.Custom.MessageWatch busMsgWatchItem in busMsgWatchItems)
		{
			list.Add(ConvertToContractMsgWatchItem(busMsgWatchItem));
		}
		return list.ToArray();
	}

	private DiagBatchJobStatus ConvertToContractBatchJobStatus(BatchJobStatus busBatchJobStatus)
	{
		return new DiagBatchJobStatus
		{
			BatchJobID = busBatchJobStatus.BatchJobID,
			Status = busBatchJobStatus.Status,
			NoofFailedStarts = busBatchJobStatus.NoofFailedStarts,
			LastSuccess = busBatchJobStatus.LastSuccess,
			LastJobMessage = busBatchJobStatus.LastJobMessage
		};
	}

	private DiagBatchJobStatus[] ConvertToContractBatchJobStatusArr(IEnumerable<BatchJobStatus> busBatchJobStatusList)
	{
		List<DiagBatchJobStatus> list = new List<DiagBatchJobStatus>();
		foreach (BatchJobStatus busBatchJobStatus in busBatchJobStatusList)
		{
			list.Add(ConvertToContractBatchJobStatus(busBatchJobStatus));
		}
		return list.ToArray();
	}

	private string[] ConvertToSessionMsgIndicatorData(IEnumerable<Guid> busSessionIds)
	{
		List<string> list = new List<string>();
		foreach (Guid busSessionId in busSessionIds)
		{
			list.Add(busSessionId.ToString());
		}
		return list.ToArray();
	}

	public DiagServerAppData GetServerAppData()
	{
		DiagServerAppData diagServerAppData = new DiagServerAppData();
		IMessageDataMgr messageDataMgr = GenericDataMgr.GetMessageDataMgr();
		ISessionDataMgr sessionDataMgr = GenericDataMgr.GetSessionDataMgr();
		IMessageWatchDataMgr messageWatchDataMgr = GenericDataMgr.GetMessageWatchDataMgr();
		IBatchJobDataMgr batchJobDataMgr = GenericDataMgr.GetBatchJobDataMgr();
		diagServerAppData.LastMessageID = messageDataMgr.GetLastMessageID();
		bool moreMsgsToRet;
		List<VideoOS.SCH.Services.Business.Definitions.Custom.Message> messages = messageDataMgr.GetMessages(ConfigSettings.MaxResponseDataSize - 2000, out moreMsgsToRet);
		if (moreMsgsToRet)
		{
			messages.Add(new VideoOS.SCH.Services.Business.Definitions.Custom.Message(0, "-{Message list truncated}-", DateTime.MinValue, null));
		}
		diagServerAppData.Messages = ConvertToContractDiagMessages(messages);
		diagServerAppData.Sessions = ConvertToContractDiagSessions(sessionDataMgr.GetAllSessions().Values);
		diagServerAppData.MessageWatchItems = ConvertToContractMsgWatchItems(messageWatchDataMgr.GetMessageWatchers());
		diagServerAppData.MatchOperators = ConvertToContractDiagMatchOps(messageWatchDataMgr.GetAllMatchOperators(includeDescriptions: true).Values);
		diagServerAppData.BatchJobStatusArray = ConvertToContractBatchJobStatusArr(batchJobDataMgr.GetBatchJobStatus(null));
		return diagServerAppData;
	}

	public DiagSessionMsgIndicatorData GetSessionMsgIndicatorData()
	{
		DiagSessionMsgIndicatorData diagSessionMsgIndicatorData = new DiagSessionMsgIndicatorData();
		SessionMsgIndicatorData allCacheData = SessionMsgIndicator.Current.GetAllCacheData();
		diagSessionMsgIndicatorData.TotalMsgCount = allCacheData.TotalMsgCount;
		diagSessionMsgIndicatorData.LatestMsgCreateTime = allCacheData.LatestMsgCreateTime;
		diagSessionMsgIndicatorData.SessionIdsWithMessages = ConvertToSessionMsgIndicatorData(allCacheData.SessionsWithMessages);
		return diagSessionMsgIndicatorData;
	}

	private DiagSimpleKVPair CreateSimpleKV(string key, string value)
	{
		return new DiagSimpleKVPair
		{
			Key = key,
			Value = value
		};
	}

	public DiagSimpleKVPair[] GetProcessDetails()
	{
		List<DiagSimpleKVPair> list = new List<DiagSimpleKVPair>();
		int num = 0;
		list.Add(CreateSimpleKV("Process ID", Process.GetCurrentProcess().Id.ToString("####", CultureInfo.InvariantCulture)));
		list.Add(CreateSimpleKV("Process name", Process.GetCurrentProcess().ProcessName));
		list.Add(CreateSimpleKV("AppDomain name", AppDomain.CurrentDomain.FriendlyName));
		list.Add(CreateSimpleKV("WorkingSet", Process.GetCurrentProcess().WorkingSet64.ToString("###,###", CultureInfo.InvariantCulture)));
		list.Add(CreateSimpleKV("PeakWorkingSet", Process.GetCurrentProcess().PeakWorkingSet64.ToString("###,###", CultureInfo.InvariantCulture)));
		list.Add(CreateSimpleKV("VirtualMemorySize", Process.GetCurrentProcess().VirtualMemorySize64.ToString("###,###", CultureInfo.InvariantCulture)));
		list.Add(CreateSimpleKV("PeakVirtualMemorySize", Process.GetCurrentProcess().PeakVirtualMemorySize64.ToString("###,###", CultureInfo.InvariantCulture)));
		list.Add(CreateSimpleKV("ThreadCount", Process.GetCurrentProcess().Threads.Count.ToString(CultureInfo.InvariantCulture)));
		list.Add(CreateSimpleKV("StartTime", Process.GetCurrentProcess().StartTime.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss 'GMT'", CultureInfo.InvariantCulture)));
		string environmentVariable = Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS");
		if (environmentVariable != null && environmentVariable.Length > 0 && NumUtil.IsInt(environmentVariable))
		{
			num = Convert.ToInt32(environmentVariable);
		}
		list.Add(CreateSimpleKV("Num of CPUs", num.ToString(CultureInfo.InvariantCulture)));
		PerformanceCounter performanceCounter;
		float num2;
		if (num > 1)
		{
			for (int i = 0; i < num; i++)
			{
				performanceCounter = new PerformanceCounter("Processor", "% Processor Time", i.ToString(CultureInfo.InvariantCulture));
				performanceCounter.NextValue();
				num2 = 0f;
				for (int j = 0; j < 10; j++)
				{
					Thread.Sleep(100);
					num2 += performanceCounter.NextValue();
				}
				performanceCounter.Close();
				num2 /= 10f;
				list.Add(CreateSimpleKV("Processor usage (CPU No " + i.ToString(CultureInfo.InvariantCulture) + ")", num2.ToString("##0", CultureInfo.InvariantCulture)));
			}
		}
		performanceCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
		performanceCounter.NextValue();
		num2 = 0f;
		for (int k = 0; k < 10; k++)
		{
			Thread.Sleep(100);
			num2 += performanceCounter.NextValue();
		}
		performanceCounter.Close();
		list.Add(CreateSimpleKV("Processor usage (CPU total)", (num2 / 10f).ToString("##0", CultureInfo.InvariantCulture)));
		return list.ToArray();
	}

	public DiagAssemblyInfo[] GetAssemblyInfo()
	{
		List<DiagAssemblyInfo> list = new List<DiagAssemblyInfo>();
		Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
		foreach (Assembly assembly in assemblies)
		{
			DiagAssemblyInfo diagAssemblyInfo = new DiagAssemblyInfo();
			diagAssemblyInfo.Name = assembly.GetName().Name;
			diagAssemblyInfo.Version = assembly.GetName().Version.ToString();
			if (assembly.IsDynamic)
			{
				diagAssemblyInfo.CodeBase = "(Dynamic module)";
				diagAssemblyInfo.Location = "(Dynamic module)";
			}
			else
			{
				diagAssemblyInfo.CodeBase = assembly.CodeBase;
				diagAssemblyInfo.Location = assembly.Location;
			}
			list.Add(diagAssemblyInfo);
		}
		return list.ToArray();
	}

	private string GetConfigSetting(GetConfigSettingDelegate configSettingDelegate)
	{
		try
		{
			return configSettingDelegate();
		}
		catch (ServiceChannelException ex)
		{
			return "ServiceChannelException occurred: " + ex.Message;
		}
	}

	public DiagSimpleKVPair[] GetConfigSettings()
	{
		return new List<DiagSimpleKVPair>
		{
			CreateSimpleKV("SCHSessionTimeoutSecs", GetConfigSetting(() => ConfigSettings.SCHSessionTimeoutSecs.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("AppDataStorageType", GetConfigSetting(() => ConfigSettings.AppDataStorageType.ToString())),
			CreateSimpleKV("SQLServer.ConnectionString", GetConfigSetting(() => ConfigSettings.SQLServerConnectionString)),
			CreateSimpleKV("MessageTypeEnforced", GetConfigSetting(() => ConfigSettings.MessageTypeEnforced.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("MaxResponseDataSize", GetConfigSetting(() => ConfigSettings.MaxResponseDataSize.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("RecommendedMsgChkIntervalMs", GetConfigSetting(() => ConfigSettings.RecommendedMsgChkIntervalMs.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("MaxNoofMillisecsToWaitForMessages", GetConfigSetting(() => ConfigSettings.MaxNoofMillisecsToWaitForMessages.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("DeleteExpiredSessionsJobIntervalSecs", GetConfigSetting(() => ConfigSettings.DeleteExpiredSessionsJobIntervalSecs.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("DeleteNonSessionMessagesIntervalSecs", GetConfigSetting(() => ConfigSettings.DeleteNonSessionMessagesIntervalSecs.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("ServiceRegistrationIntervalSecs", GetConfigSetting(() => ConfigSettings.ServiceRegistrationIntervalSecs.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("SessionMsgIndicatorUsed", GetConfigSetting(() => ConfigSettings.SessionMsgIndicatorUsed.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("SessionMsgIndicatorUpdateIntervalMs", GetConfigSetting(() => ConfigSettings.SessionMsgIndicatorUpdateIntervalMs.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("WinEventLogSourceName", GetConfigSetting(() => ConfigSettings.WinEventLogSourceName)),
			CreateSimpleKV("MessageLogEnabled", GetConfigSetting(() => ConfigSettings.MessageLogEnabled.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("MessageLogFileName", GetConfigSetting(() => ConfigSettings.MessageLogFileName)),
			CreateSimpleKV("ServerCommandServiceTimeout", GetConfigSetting(() => ConfigSettings.ServerCommandServiceTimeout.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("IdentityTokenCacheEntryLifetimeInSeconds", GetConfigSetting(() => ConfigSettings.IdentityTokenCacheEntryLifetimeInSeconds.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("IdentityTokenCachePurgeIntervalInSeconds", GetConfigSetting(() => ConfigSettings.IdentityTokenCachePurgeIntervalInSeconds.ToString(CultureInfo.InvariantCulture))),
			CreateSimpleKV("IdentityTokenCacheInactiveTimeLimitInSeconds", GetConfigSetting(() => ConfigSettings.IdentityTokenCacheInactiveTimeLimitInSeconds.ToString(CultureInfo.InvariantCulture)))
		}.ToArray();
	}

	public bool TestTextLogger(out List<string> resultDetails)
	{
		resultDetails = new List<string>();
		try
		{
			new TextLogger("ServiceChannelServerLog", Application.ServerLogFolder).Log(0, "This is a test log entry, logged by the Diagnostics component, which indicates that the Service Channel TextLogger is working.", LogType.Information);
			resultDetails.Add("Test result is OK.");
			return true;
		}
		catch (Exception ex)
		{
			resultDetails.Clear();
			resultDetails.Add("Exception was thrown.");
			resultDetails.AddRange(Logger.UnwindStackTrace(ex));
			return false;
		}
	}
}
