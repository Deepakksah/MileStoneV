using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Services;
using VideoOS.Management.VmoClient;

namespace VideoOS.Management.Server;

public sealed class RuleSource
{
	public int DataCollectorId { get; set; }

	public int CounterId { get; set; }

	public string SourceId { get; set; }

	public string CounterPath { get; set; }

	public int RuleType { get; set; }

	public string CounterRuleId { get; set; }

	public string ParentId { get; set; }

	public string Name { get; set; }

	public bool Configurable { get; set; }

	public double CriticalThreshold { get; set; }

	public double WarningThreshold { get; set; }

	public int TimeWindow { get; set; }

	public long Unit { get; set; }

	public string CategoryName { get; set; }

	public string CounterName { get; set; }

	public bool HasOverride => TimeWindow > 0;

	public RuleSource()
	{
	}

	public RuleSource(int dataCollector, int counterId)
	{
		DataCollectorId = dataCollector;
		CounterId = counterId;
		SourceId = Guid.NewGuid().ToString();
	}

	public RuleSource(int dataCollector, int counterId, string sourceId)
	{
		DataCollectorId = dataCollector;
		CounterId = counterId;
		SourceId = sourceId;
	}

	public RuleSource(int dataCollector, int counterId, string sourceId, string counterPath, bool configurable, string critical, string warning, string timeWindow)
	{
		DataCollectorId = dataCollector;
		CounterId = counterId;
		SourceId = sourceId;
		CounterPath = counterPath;
		Configurable = configurable;
		CriticalThreshold = ((!string.IsNullOrEmpty(critical)) ? Convert.ToDouble(critical, CultureInfo.InvariantCulture) : (-1.0));
		WarningThreshold = ((!string.IsNullOrEmpty(warning)) ? Convert.ToDouble(warning, CultureInfo.InvariantCulture) : (-1.0));
		if (int.TryParse(timeWindow, out var result))
		{
			TimeWindow = result;
		}
		else
		{
			TimeWindow = -1;
		}
	}

	public static RuleSource[] QueryRuleSourceByIds(IGeminiRequestContext requestContext, int datacollectorId, string[] ids, bool generateSources)
	{
		List<RuleSource> sources = new List<RuleSource>();
		using (IRuleSourceComponent ruleSourceComponent = requestContext.CreateRuleSourceComponent())
		{
			sources = ruleSourceComponent.QueryRuleSourcesByIds(datacollectorId, ids, generateSources);
		}
		sources = ValidateThresholds(sources);
		if (generateSources)
		{
			sources = GenerateAdditionalSources(requestContext, sources);
		}
		return sources.ToArray();
	}

	private static List<RuleSource> ValidateThresholds(List<RuleSource> sources)
	{
		List<RuleSource> list = new List<RuleSource>();
		foreach (RuleSource source in sources)
		{
			RuleSource s = source;
			ValidateThreshold(ref s);
			list.Add(s);
		}
		return list;
	}

	private static void ValidateThreshold(ref RuleSource s)
	{
		if (s.Unit >= 0)
		{
			if (s.CriticalThreshold > (double)s.Unit)
			{
				s.CriticalThreshold = s.Unit;
			}
			if (s.WarningThreshold > (double)s.Unit)
			{
				s.WarningThreshold = s.Unit;
			}
		}
	}

	public static RuleSource QueryRuleSourceById(IGeminiRequestContext requestContext, string ruleSourceId)
	{
		Validation.CheckObjectId(ruleSourceId, "RuleSourceId", allowNullOrEmpty: false);
		using IRuleSourceComponent ruleSourceComponent = requestContext.CreateRuleSourceComponent();
		RuleSource s = ruleSourceComponent.QueryRuleSourceById(ruleSourceId);
		if (s != null)
		{
			ValidateThreshold(ref s);
		}
		return s;
	}

	public static List<RuleSource> QueryRuleSourceByCounterRuleId(IGeminiRequestContext requestContext, string counterRuleId)
	{
		Validation.CheckObjectId(counterRuleId, "counterRuleId", allowNullOrEmpty: false);
		using IRuleSourceComponent ruleSourceComponent = requestContext.CreateRuleSourceComponent();
		return ValidateThresholds(ruleSourceComponent.QueryRuleSourcesByCounterId(counterRuleId));
	}

	public static RuleSource CreateRuleSource(IGeminiRequestContext requestContext, string sourceId, double critical, double warning, int timewindow, string counterRuleId, int dataCollectorId, int counterId, string parentId, string name)
	{
		ValidateParameters(sourceId, counterRuleId, timewindow);
		PerformanceStateRuleConfig.ValidateExistingCounterRuleConfig(requestContext, counterRuleId, critical, warning);
		PerformanceStateRuleConfig.CheckSecurityPermission(requestContext, "GENERIC_WRITE");
		RuleSource ruleSource = new RuleSource
		{
			SourceId = sourceId,
			CriticalThreshold = critical,
			WarningThreshold = warning,
			TimeWindow = timewindow,
			Configurable = true,
			CounterRuleId = counterRuleId,
			DataCollectorId = dataCollectorId,
			CounterId = counterId,
			ParentId = parentId
		};
		using (IRuleSourceComponent ruleSourceComponent = requestContext.CreateRuleSourceComponent())
		{
			ruleSourceComponent.CreateRuleSource(ruleSource);
		}
		RuleSource ruleSource2 = QueryRuleSourceById(requestContext, ruleSource.SourceId);
		ruleSource2.Name = name;
		SystemMonitorManager.RegisterCreatedItems(requestContext, ruleSource2);
		return ruleSource2;
	}

	public static RuleSource UpdateRuleSource(IGeminiRequestContext requestContext, string sourceId, string ruleSourceId, double critical, double warning, int timewindow, string name, string parent)
	{
		ValidateParameters(sourceId, ruleSourceId, timewindow);
		PerformanceStateRuleConfig.ValidateExistingCounterRuleConfig(requestContext, ruleSourceId, critical, warning);
		PerformanceStateRuleConfig.CheckSecurityPermission(requestContext, "GENERIC_WRITE");
		RuleSource oldRuleSource = QueryRuleSourceById(requestContext, sourceId);
		RuleSource ruleSource = new RuleSource
		{
			SourceId = sourceId,
			CriticalThreshold = critical,
			WarningThreshold = warning,
			TimeWindow = timewindow
		};
		using (IRuleSourceComponent ruleSourceComponent = requestContext.CreateRuleSourceComponent())
		{
			ruleSourceComponent.UpdateRuleSource(ruleSource);
		}
		RuleSource ruleSource2 = QueryRuleSourceById(requestContext, ruleSource.SourceId);
		ruleSource2.Name = name;
		ruleSource2.ParentId = parent;
		SystemMonitorManager.RegisterUpdatedItems(requestContext, oldRuleSource, ruleSource2);
		return ruleSource2;
	}

	public static void DeleteRuleSource(IGeminiRequestContext requestContext, string sourceId)
	{
		Validation.CheckObjectId(sourceId, "sourceId", allowNullOrEmpty: false);
		PerformanceStateRuleConfig.CheckSecurityPermission(requestContext, "GENERIC_WRITE");
		RuleSource existingRuleSource = QueryRuleSourceById(requestContext, sourceId);
		using (IRuleSourceComponent ruleSourceComponent = requestContext.CreateRuleSourceComponent())
		{
			ruleSourceComponent.DeleteRuleSource(sourceId);
		}
		SystemMonitorManager.RegisterDeletedItems(requestContext, existingRuleSource);
	}

	private static void ValidateParameters(string sourceId, string counterRuleId, int window)
	{
		Validation.CheckObjectId(counterRuleId, "CounterRuleId", allowNullOrEmpty: false);
		Validation.CheckObjectId(sourceId, "SourceId", allowNullOrEmpty: false);
		if (window < 0)
		{
			throw new InvalidArgumentException("Time window can not be negative");
		}
	}

	private static List<RuleSource> GenerateAdditionalSources(IGeminiRequestContext requestContext, List<RuleSource> ruleSources)
	{
		List<DataCollectorParent> list = TryGetDataCollectorServices(requestContext, ruleSources);
		List<DataCollectorParent> machines = DataCollectorServerService.GetMachines(requestContext);
		using IRecordingStorageComponent recordingStorageComponent = requestContext.CreateRecordingStorageComponent();
		SynchronizeDataCollectorWithServices(list, machines, ruleSources);
		List<RuleSource> list2 = new List<RuleSource>();
		List<RuleSource> list3 = new List<RuleSource>();
		foreach (RuleSource source in ruleSources)
		{
			if (source.RuleType == 1 || source.RuleType == 2 || source.RuleType == 15 || source.RuleType == 16 || source.RuleType == 17)
			{
				if (list != null)
				{
					DataCollectorParent dataCollectorParent = list.FirstOrDefault((DataCollectorParent i) => i.DataCollectorId == source.DataCollectorId);
					if (dataCollectorParent != null)
					{
						list3.AddRange(AddNewChildrenToSource(dataCollectorParent, source, newSourcesConfigurable: false));
					}
				}
			}
			else if (source.RuleType == 6)
			{
				bool removeThisSource = false;
				RuleSource newS = FindRecorderParentForDrive(machines, source, ref removeThisSource);
				if (removeThisSource)
				{
					list2.Add(source);
				}
				else if (newS != null && list3.All((RuleSource i) => !i.SourceId.Equals(newS.SourceId, StringComparison.OrdinalIgnoreCase) || !i.CounterRuleId.Equals(newS.CounterRuleId, StringComparison.OrdinalIgnoreCase)))
				{
					list3.Add(newS);
				}
			}
			else if (source.RuleType == 7)
			{
				bool removeThisSource2 = false;
				RuleSource newS2 = FindParentForStorage(machines, recordingStorageComponent, source, ref removeThisSource2);
				if (removeThisSource2)
				{
					list2.Add(source);
				}
				else if (newS2 != null && list3.All((RuleSource i) => !i.SourceId.Equals(newS2.SourceId, StringComparison.OrdinalIgnoreCase) || !i.CounterRuleId.Equals(newS2.CounterRuleId, StringComparison.OrdinalIgnoreCase)))
				{
					list3.Add(newS2);
				}
			}
		}
		ruleSources.AddRange(list3);
		foreach (RuleSource item in list2)
		{
			ruleSources.Remove(item);
		}
		return ruleSources;
	}

	private static void SynchronizeDataCollectorWithServices(List<DataCollectorParent> dataCollectorMachines, List<DataCollectorParent> serverServices, List<RuleSource> ruleSources)
	{
		DataCollectorParent[] emptyDataCollectorMachines = dataCollectorMachines.Where((DataCollectorParent m) => serverServices.FirstOrDefault((DataCollectorParent s) => s.DataCollectorId == m.DataCollectorId) == null).ToArray();
		foreach (DataCollectorParent dc in dataCollectorMachines)
		{
			DataCollectorChild[] array = dc.Children.Where((DataCollectorChild c) => serverServices.FirstOrDefault((DataCollectorParent s) => s.ServerType.Equals(c.ServiceType, StringComparison.OrdinalIgnoreCase) && s.DataCollectorId == dc.DataCollectorId) == null).ToArray();
			foreach (DataCollectorChild item in array)
			{
				dc.Children.Remove(item);
			}
		}
		DataCollectorParent[] array2 = emptyDataCollectorMachines;
		foreach (DataCollectorParent item2 in array2)
		{
			dataCollectorMachines.Remove(item2);
		}
		RuleSource[] array3 = ruleSources.Where((RuleSource rS) => emptyDataCollectorMachines.FirstOrDefault((DataCollectorParent dataCollectorParent) => dataCollectorParent.DataCollectorId == rS.DataCollectorId) != null).ToArray();
		foreach (RuleSource item3 in array3)
		{
			ruleSources.Remove(item3);
		}
	}

	private static List<DataCollectorParent> TryGetDataCollectorServices(IGeminiRequestContext requestContext, List<RuleSource> ruleSources)
	{
		bool num = ruleSources.Any((RuleSource i) => i.RuleType == 1 || i.RuleType == 2 || i.RuleType == 15 || i.RuleType == 16 || i.RuleType == 17);
		List<DataCollectorParent> result = null;
		if (num)
		{
			result = DataCollectorService.GetMachines(requestContext);
		}
		return result;
	}

	private static RuleSource FindRecorderParentForDrive(List<DataCollectorParent> dcItems, RuleSource source, ref bool removeThisSource)
	{
		if (!GetServerAndDrive(source.CounterPath, out var serverId, out var drive))
		{
			removeThisSource = true;
			return null;
		}
		DataCollectorParent dataCollectorParent = dcItems.FirstOrDefault((DataCollectorParent i) => i.Id.Equals(serverId, StringComparison.OrdinalIgnoreCase) && i.ServerType.Equals(ServiceTypes.ServiceType_RecordingService.ToString(), StringComparison.InvariantCultureIgnoreCase));
		if (dataCollectorParent == null)
		{
			removeThisSource = true;
			return null;
		}
		if (dataCollectorParent.DataCollectorId == source.DataCollectorId)
		{
			RuleSource ruleSource = new RuleSource(source.DataCollectorId, source.CounterId, dataCollectorParent.Id, source.CounterPath, configurable: false, null, null, null);
			ruleSource.Name = dataCollectorParent.ParentName;
			ruleSource.ParentId = null;
			ruleSource.CounterRuleId = source.CounterRuleId;
			ruleSource.CounterName = source.CounterName;
			ruleSource.CategoryName = source.CategoryName;
			ruleSource.RuleType = source.RuleType;
			source.Name = drive;
			source.ParentId = ruleSource.SourceId;
			return ruleSource;
		}
		DataCollectorParent dataCollectorParent2 = dcItems.FirstOrDefault((DataCollectorParent i) => i.DataCollectorId == source.DataCollectorId && i.ServerType.Equals(ServiceTypes.ServiceType_FailoverService.ToString(), StringComparison.InvariantCultureIgnoreCase));
		if (dataCollectorParent2 != null)
		{
			RuleSource ruleSource2 = new RuleSource(source.DataCollectorId, source.CounterId, dataCollectorParent2.Id, source.CounterPath, configurable: false, null, null, null);
			ruleSource2.Name = dataCollectorParent2.ParentName;
			ruleSource2.ParentId = null;
			ruleSource2.CounterRuleId = source.CounterRuleId;
			ruleSource2.CounterName = source.CounterName;
			ruleSource2.CategoryName = source.CategoryName;
			ruleSource2.RuleType = source.RuleType;
			source.Name = drive + " [" + dataCollectorParent.ParentName + "]";
			source.ParentId = ruleSource2.SourceId;
			return ruleSource2;
		}
		return null;
	}

	private static bool GetServerAndDrive(string counterPath, out string serverId, out string drive)
	{
		int num = counterPath.LastIndexOf('[');
		int num2 = counterPath.LastIndexOf(']');
		if (num > -1 && num2 > -1 && num < num2)
		{
			string[] array = counterPath.Substring(num + 1, num2 - num - 1).Split(' ');
			if (array.Length == 2)
			{
				drive = array[0];
				serverId = array[1];
				return true;
			}
		}
		serverId = string.Empty;
		drive = string.Empty;
		return false;
	}

	private static RuleSource FindParentForStorage(List<DataCollectorParent> dcItems, IRecordingStorageComponent recordingStorageComponent, RuleSource source, ref bool removeThisSource)
	{
		DataCollectorParent dataCollectorParent = dcItems.FirstOrDefault((DataCollectorParent i) => i.DataCollectorId == source.DataCollectorId);
		if (!GetStorageName(recordingStorageComponent, source.CounterPath, out var name))
		{
			removeThisSource = true;
			return null;
		}
		if (dataCollectorParent != null)
		{
			RuleSource ruleSource = new RuleSource(source.DataCollectorId, source.CounterId, dataCollectorParent.Id, source.CounterPath, configurable: false, null, null, null);
			ruleSource.Name = dataCollectorParent.ParentName;
			ruleSource.ParentId = null;
			ruleSource.CounterRuleId = source.CounterRuleId;
			ruleSource.CounterName = source.CounterName;
			ruleSource.CategoryName = source.CategoryName;
			ruleSource.RuleType = source.RuleType;
			source.Name = name;
			source.ParentId = ruleSource.SourceId;
			return ruleSource;
		}
		removeThisSource = true;
		return null;
	}

	private static bool GetStorageName(IRecordingStorageComponent recordingStorageComponent, string path, out string name)
	{
		name = string.Empty;
		int num = path.LastIndexOf('[');
		int num2 = path.LastIndexOf(']');
		if (num > -1 && num2 > -1 && num < num2)
		{
			string recordingStorageId = path.Substring(num + 1, num2 - num - 1);
			name = recordingStorageComponent.QueryRecordingStorage(recordingStorageId)?.Name;
			return !string.IsNullOrEmpty(name);
		}
		return false;
	}

	private static IEnumerable<RuleSource> AddNewChildrenToSource(DataCollectorParent machine, RuleSource source, bool newSourcesConfigurable)
	{
		List<RuleSource> list = new List<RuleSource>();
		source.Name = machine.ParentName;
		if (machine.Children == null)
		{
			return null;
		}
		foreach (DataCollectorChild child in machine.Children)
		{
			RuleSource ruleSource = new RuleSource(source.DataCollectorId, source.CounterId, child.Id, source.CounterPath, newSourcesConfigurable, null, null, null);
			ruleSource.Name = child.ServiceName;
			ruleSource.ParentId = source.SourceId;
			ruleSource.CounterName = source.CounterName;
			ruleSource.CategoryName = source.CategoryName;
			ruleSource.CounterRuleId = source.CounterRuleId;
			ruleSource.RuleType = source.RuleType;
			list.Add(ruleSource);
		}
		return list;
	}
}
