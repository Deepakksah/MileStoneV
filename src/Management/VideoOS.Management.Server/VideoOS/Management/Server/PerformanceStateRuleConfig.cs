using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;
using VideoOS.Management.VmoClient;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public sealed class PerformanceStateRuleConfig
{
	public string CounterRuleId { get; set; }

	public double CriticalThreshold { get; set; }

	public double WarningThreshold { get; set; }

	public string RuleName { get; set; }

	public int TimeWindow { get; set; }

	public int RuleType { get; set; }

	public bool Disabled { get; set; }

	public long Unit { get; set; }

	public bool Direction { get; set; }

	public string CategoryName { get; set; }

	public string CounterName { get; set; }

	public static PerformanceStateRuleConfig[] QueryPerformanceStateRuleConfigByIds(IGeminiRequestContext requestContext, int dataCollector, string[] ids)
	{
		PerformanceStateRuleConfig[] configs = null;
		using (PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreateComponent<PerformanceStateRuleConfigComponent>())
		{
			configs = performanceStateRuleConfigComponent.QueryCountersStateRulesDataByIds(dataCollector, ids);
		}
		CheckSecurityPermissions(requestContext, "GENERIC_READ", ref configs);
		List<PerformanceStateRuleConfig> list = new List<PerformanceStateRuleConfig>();
		PerformanceStateRuleConfig[] array = configs;
		for (int i = 0; i < array.Length; i++)
		{
			PerformanceStateRuleConfig config = array[i];
			ValidateThresholds(ref config);
			list.Add(config);
		}
		return list.ToArray();
	}

	public static PerformanceStateRuleConfig[] QueryDatacollectorPerformanceStateRuleConfigByIds(IGeminiRequestContext requestContext, int dataCollector, string[] ids)
	{
		return (from c in QueryPerformanceStateRuleConfigByIds(requestContext, dataCollector, ids)
			where !PerformanceStateRuleConfigType.TriggeredByManagementServer.Contains(c.RuleType)
			select c).ToArray();
	}

	public static PerformanceStateRuleConfig[] QueryServerPerformanceStateRuleConfigByIds(IGeminiRequestContext requestContext, int dataCollector, string[] ids)
	{
		return (from c in QueryPerformanceStateRuleConfigByIds(requestContext, dataCollector, ids)
			where PerformanceStateRuleConfigType.TriggeredByManagementServer.Contains(c.RuleType)
			select c).ToArray();
	}

	private static void UpdateInitValues(IGeminiRequestContext requestContext, string id, out double criticalValue, out double warningValue)
	{
		using PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreateComponent<PerformanceStateRuleConfigComponent>();
		performanceStateRuleConfigComponent.GetPerformanceCounterDefaultValues(id, out criticalValue, out warningValue);
	}

	private static void CheckSecurityPermissions(IGeminiRequestContext requestContext, string securityAction, ref PerformanceStateRuleConfig[] configs)
	{
		List<PerformanceStateRuleConfig> list = configs.ToList();
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		List<PerformanceStateRuleConfig> list2 = new List<PerformanceStateRuleConfig>();
		PerformanceStateRuleConfig[] array = configs;
		foreach (PerformanceStateRuleConfig performanceStateRuleConfig in array)
		{
			if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SystemMonitor, securityAction, performanceStateRuleConfig.CounterRuleId))
			{
				list2.Add(performanceStateRuleConfig);
			}
		}
		foreach (PerformanceStateRuleConfig item in list2)
		{
			list.Remove(item);
		}
		configs = list.ToArray();
	}

	public static void CheckSecurityPermission(IGeminiRequestContext requestContext, string securityAction)
	{
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SystemMonitor, securityAction, "\\"))
		{
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
	}

	public static PerformanceStateRuleConfig CreatPerformanceStateRuleConfig(IGeminiRequestContext requestContext, string counterRuleId, double criticalThreshold, double warningThreshold, int timeWindow, string name, int ruleType)
	{
		ValidateParameters(counterRuleId, name, timeWindow);
		PerformanceStateRuleConfig config = new PerformanceStateRuleConfig
		{
			CounterRuleId = counterRuleId,
			CriticalThreshold = criticalThreshold,
			WarningThreshold = warningThreshold,
			TimeWindow = timeWindow,
			RuleType = ruleType,
			RuleName = name
		};
		Guid sessionId = Guid.NewGuid();
		using (PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreateComponent<PerformanceStateRuleConfigComponent>())
		{
			performanceStateRuleConfigComponent.CreateCountersStateRulesData(config, sessionId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		PerformanceStateRuleConfig performanceStateRuleConfig = QueryPerformanceStateRuleConfigById(requestContext, counterRuleId);
		SystemMonitorManager.RegisterCreatedItems(requestContext, performanceStateRuleConfig);
		return performanceStateRuleConfig;
	}

	private static void ValidateParameters(string counterRuleId, string name, int window)
	{
		Validation.CheckObjectId(counterRuleId, "CounterRuleId", allowNullOrEmpty: false);
		Validation.CheckName(name, "PerformanceStateRuleName");
		if (window < 0)
		{
			throw new InvalidArgumentException("Time window can not be negative");
		}
	}

	public static PerformanceStateRuleConfig UpdatePerformanceStateRuleConfig(IGeminiRequestContext requestContext, string counterRuleId, double criticalThreshold, double warningThreshold, int timeWindow, string name, int ruleType, bool disabled)
	{
		ValidateParameters(counterRuleId, name, timeWindow);
		CheckSecurityPermission(requestContext, "GENERIC_WRITE");
		PerformanceStateRuleConfig performanceStateRuleConfig = ValidateExistingCounterRuleConfig(requestContext, counterRuleId, criticalThreshold, warningThreshold);
		PerformanceStateRuleConfig performanceStateRuleConfig2 = new PerformanceStateRuleConfig
		{
			CounterRuleId = counterRuleId,
			CriticalThreshold = criticalThreshold,
			WarningThreshold = warningThreshold,
			TimeWindow = timeWindow,
			RuleName = name,
			RuleType = ruleType,
			Disabled = disabled
		};
		if (performanceStateRuleConfig.Disabled != performanceStateRuleConfig2.Disabled && !performanceStateRuleConfig2.Disabled && performanceStateRuleConfig2.CriticalThreshold.Equals(-1.0) && performanceStateRuleConfig2.WarningThreshold.Equals(-1.0))
		{
			UpdateInitValues(requestContext, performanceStateRuleConfig2.CounterRuleId, out var criticalValue, out var warningValue);
			performanceStateRuleConfig2.CriticalThreshold = criticalValue;
			performanceStateRuleConfig2.WarningThreshold = warningValue;
		}
		Guid sessionId = Guid.NewGuid();
		using (PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreateComponent<PerformanceStateRuleConfigComponent>())
		{
			performanceStateRuleConfigComponent.UpdateCountersStateRulesData(performanceStateRuleConfig2, sessionId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		PerformanceStateRuleConfig performanceStateRuleConfig3 = QueryPerformanceStateRuleConfigById(requestContext, counterRuleId);
		SystemMonitorManager.RegisterUpdatedItems(requestContext, performanceStateRuleConfig, performanceStateRuleConfig3);
		if (!performanceStateRuleConfig.Disabled && performanceStateRuleConfig3.Disabled)
		{
			CounterStateChangeData[] array = SystemMonitorManager.SetStatesByRule(requestContext, performanceStateRuleConfig.CounterRuleId, 0);
			foreach (CounterStateChangeData counterStateChangeData in array)
			{
				SystemMonitorManager.RuleEventType ruleEventType = SystemMonitorManager.GetRuleEventType(counterStateChangeData);
				List<SystemMonitorManager.EventSource> sources = SystemMonitorManager.LookupSources(requestContext, counterStateChangeData, ruleEventType);
				SystemMonitorStateHandler.AddState(requestContext, counterStateChangeData, sources, ruleEventType);
			}
		}
		return performanceStateRuleConfig3;
	}

	public static PerformanceStateRuleConfig ValidateExistingCounterRuleConfig(IGeminiRequestContext requestContext, string counterRuleId, double criticalThreshold, double warningThreshold)
	{
		PerformanceStateRuleConfig obj = QueryPerformanceStateRuleConfigById(requestContext, counterRuleId) ?? throw new ArgumentException("Specified performance state rule does not exist");
		if (obj.Direction)
		{
			if (criticalThreshold < warningThreshold)
			{
				throw new InvalidArgumentException("Critical threshold should be higher than warning threshold");
			}
		}
		else if (criticalThreshold > warningThreshold)
		{
			throw new InvalidArgumentException("Critical threshold should be lower than warning threshold");
		}
		return obj;
	}

	public static void DeletePerformanceStateRuleConfig(IGeminiRequestContext requestContext, string counterRuleId)
	{
		Validation.CheckObjectId(counterRuleId, "CounterRuleId", allowNullOrEmpty: false);
		PerformanceStateRuleConfig existingConfig = QueryPerformanceStateRuleConfigById(requestContext, counterRuleId);
		Guid sessionId = Guid.NewGuid();
		using (PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreateComponent<PerformanceStateRuleConfigComponent>())
		{
			performanceStateRuleConfigComponent.DeleteCountersStateRulesData(counterRuleId, sessionId);
		}
		using (VmoLogManager vmoLogManager = new VmoLogManager(requestContext))
		{
			vmoLogManager.LogSession(sessionId, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		}
		SystemMonitorManager.RegisterDeletedItems(requestContext, existingConfig);
	}

	public static PerformanceStateRuleConfig QueryPerformanceStateRuleConfigById(IGeminiRequestContext requestContext, string counterRuleId)
	{
		Validation.CheckObjectId(counterRuleId, "CounterRuleId", allowNullOrEmpty: false);
		CheckSecurityPermission(requestContext, "GENERIC_READ");
		PerformanceStateRuleConfig config;
		using (PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreateComponent<PerformanceStateRuleConfigComponent>())
		{
			config = performanceStateRuleConfigComponent.QueryCountersStateRulesDataById(counterRuleId);
		}
		ValidateThresholds(ref config);
		return config;
	}

	private static void ValidateThresholds(ref PerformanceStateRuleConfig config)
	{
		if (config.CriticalThreshold > (double)config.Unit)
		{
			config.CriticalThreshold = config.Unit;
		}
		if (config.WarningThreshold > (double)config.Unit)
		{
			config.WarningThreshold = config.Unit;
		}
	}

	public static void RegisterStateChange(IGeminiRequestContext requestContext, CounterStateChangeData data)
	{
		if (SystemMonitorManager.IsOldMultiRuleState(data.CounterRuleId, data.CounterPath) || SystemMonitorManager.CameraStateChangeOnOtherDevice(requestContext, data.CounterPath, data.CounterRuleId))
		{
			return;
		}
		List<SystemMonitorManager.EventSource> allSources = null;
		bool flag = false;
		using (PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreateComponent<PerformanceStateRuleConfigComponent>())
		{
			if (PerformanceCounterInstance.InstanceNameContainsId(data.CounterPath, out var id))
			{
				data.CounterPath = RemoveItemNameFromCounterPath(data.CounterPath, id);
			}
			bool flag2 = SystemMonitorManager.LookupSourcesRequired(data.CounterRuleId);
			ResultCollection resultCollection = performanceStateRuleConfigComponent.RegisterStateChange(data, flag2);
			flag = resultCollection.GetCurrent<bool>().Items[0];
			if (flag && flag2)
			{
				resultCollection.NextResult();
				allSources = resultCollection.GetCurrent<SystemMonitorManager.EventSource>().Items;
			}
		}
		if (!flag)
		{
			return;
		}
		SystemMonitorManager.RuleEventType ruleEventType = SystemMonitorManager.GetRuleEventType(data);
		if (ruleEventType == null)
		{
			ServerExceptionUtilities.LogException(new InvalidArgumentException("Error while register state change. Type of the event is invalid."));
			return;
		}
		allSources = SystemMonitorManager.FilterSources(data, allSources, ruleEventType);
		if (data.State != 0)
		{
			SystemMonitorManager.TriggerEvent(requestContext, allSources, ruleEventType);
		}
		SystemMonitorStateHandler.AddState(requestContext, data, allSources, ruleEventType);
	}

	private static string RemoveItemNameFromCounterPath(string counterPath, Guid id)
	{
		int num = counterPath.LastIndexOf('(');
		int num2 = counterPath.LastIndexOf(')');
		if (num != -1 && num2 != -1)
		{
			string[] obj = new string[5]
			{
				counterPath.Substring(0, num),
				"([",
				null,
				null,
				null
			};
			Guid guid = id;
			obj[2] = guid.ToString();
			obj[3] = "]";
			obj[4] = counterPath.Substring(num2, counterPath.Length - num2);
			return string.Concat(obj);
		}
		return counterPath;
	}

	public static void DeleteState(IGeminiRequestContext requestContext, CounterStateChangeData data)
	{
		if (SystemMonitorManager.IsOldMultiRuleState(data.CounterRuleId, data.CounterPath))
		{
			return;
		}
		using PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreateComponent<PerformanceStateRuleConfigComponent>();
		performanceStateRuleConfigComponent.DeleteState(data);
	}

	public static void DeleteStateDataFromDatacollector(IGeminiRequestContext requestContext, int datacollectorId, string[] activePaths)
	{
		using IPerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreatePerformanceStateRuleConfigComponent();
		performanceStateRuleConfigComponent.DeleteStatesFromDatacollector(datacollectorId, activePaths.ToList().Append("\\VideoOS Recording Server Storage").ToArray());
	}
}
