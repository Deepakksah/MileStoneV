#define TRACE
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Services;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public class SystemMonitorManager
{
	public class EventSource
	{
		public string SourceId { get; private set; }

		public string ServerType { get; private set; }

		public int DataCollectorId { get; private set; }

		public string Name { get; private set; }

		public EventSource(string sourceId, string serverType, int dataCollectorId, string name)
		{
			SourceId = sourceId;
			ServerType = serverType;
			DataCollectorId = dataCollectorId;
			Name = name;
		}
	}

	public class RuleEventType
	{
		public string RuleId { get; private set; }

		public Guid EventId { get; private set; }

		public int EventType { get; private set; }

		public CounterState State { get; private set; }

		public RuleEventType(string ruleId, Guid eventId, int type, CounterState state)
		{
			RuleId = ruleId;
			EventId = eventId;
			EventType = type;
			State = state;
		}
	}

	public class DeletedStateData
	{
		public string SourceId { get; private set; }

		public string ServerType { get; private set; }

		public int DataCollectorId { get; private set; }

		public string Name { get; private set; }

		public string RuleId { get; private set; }

		public string CounterPath { get; private set; }

		public int State { get; private set; }

		public double TriggerValue { get; private set; }

		public DeletedStateData(string sourceId, string serverType, int dataCollectorId, string name, string ruleId, string counterPath, int state, double triggerValue)
		{
			SourceId = sourceId;
			ServerType = serverType;
			DataCollectorId = dataCollectorId;
			Name = name;
			RuleId = ruleId;
			CounterPath = counterPath;
			State = state;
			TriggerValue = triggerValue;
		}
	}

	private static PerformanceCounterUpdatesManager _performanceCounterUpdatesManager = new PerformanceCounterUpdatesManager(new PerformanceCounterSenderFactory());

	public const string CpuUsage = "1E41EC7E-DA4A-44B4-BDF3-E2EB41E18B4C";

	public const string MemoryAvailable = "7F9350E4-55B5-43BF-8146-78D2DC531A42";

	public const string LiveFps = "47AD1879-4F04-4E71-8EE8-6F9D2DAC9148";

	public const string RecordingFps = "2917B174-7E8E-490E-A402-6F5A76E4F9DD";

	public const string CameraUsedSpace = "7AEFC712-5251-4760-8CEB-2D95DA85E5EF";

	public const string DiskFreeSpace = "47B9163F-D9EB-43B4-BB3F-4BCCA591D5F4";

	public const string ServiceAvailable = "063EC923-9E6B-4A95-B949-AE5A76DB8B5C";

	public const string RetentionTime = "BDFC173B-43C7-4600-A483-82ED37CFD73C";

	public const string CameraState = "4a86f306-5a6f-43ef-9c21-f7f52fa86ac8";

	public const string DatabaseAvailableState = "E8240A18-9A93-424D-B45C-E83DCDD99EB5";

	public const string ArchiveAvailableState = "D128E0E8-57E5-481E-8936-74423687CA82";

	public const string FailoverStartedState = "009B1207-FD5F-4506-9660-4074FF04A4A3";

	public const string FailoverErrorState = "E6330FA6-D3FE-4E35-AD50-21ECB7608931";

	public const string GPUDecoding = "FC6A2188-C1AF-48B5-ACB7-E88FE093D30C";

	public const string GPUMemory = "238C1803-601B-4441-9FDE-46E7CBE2FF6E";

	public const string GPURendering = "3732961A-025E-4D80-AEF9-5B884E6B2BAF";

	private static List<RuleEventType> _ruleEventTypes = new List<RuleEventType>();

	private static ConcurrentDictionary<Guid, DeviceType> _deviceTypesCache = new ConcurrentDictionary<Guid, DeviceType>();

	public static async void RegisterDeletedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig existingConfig)
	{
		await _performanceCounterUpdatesManager.PostDeletedItems(requestContext, existingConfig);
	}

	public static async void RegisterCreatedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig newConfig)
	{
		await _performanceCounterUpdatesManager.PostCreatedItems(requestContext, newConfig);
	}

	public static async void RegisterUpdatedItems(IGeminiRequestContext requestContext, PerformanceStateRuleConfig existingConfig, PerformanceStateRuleConfig newConfig)
	{
		await _performanceCounterUpdatesManager.PostUpdatedItems(requestContext, newConfig, existingConfig);
	}

	public static async void RegisterDeletedItems(IGeminiRequestContext requestContext, RuleSource existingRuleSource)
	{
		await _performanceCounterUpdatesManager.PostDeletedSources(requestContext, existingRuleSource);
	}

	public static async void RegisterCreatedItems(IGeminiRequestContext requestContext, RuleSource newRuleSource)
	{
		await _performanceCounterUpdatesManager.PostCreatedSources(requestContext, newRuleSource);
	}

	public static async void RegisterUpdatedItems(IGeminiRequestContext requestContext, RuleSource oldRuleSource, RuleSource newRuleSource)
	{
		await _performanceCounterUpdatesManager.PostUpdatedSources(requestContext, newRuleSource, oldRuleSource);
	}

	public static void TriggerEvent(IGeminiRequestContext geminiRequestContext, List<EventSource> sources, RuleEventType ruleType)
	{
		if (ruleType.EventType == 0 || ruleType.EventId.Equals(Guid.Empty))
		{
			ServerExceptionUtilities.LogException(new InvalidArgumentException("Error while triggering System Monitoring event. Type of the event is invalid."));
			return;
		}
		if (ruleType.EventType == 3 || ruleType.EventType == 4 || ruleType.EventType == 5)
		{
			try
			{
				foreach (EventSource source in sources)
				{
					SystemMonitorEvent.TriggerSystemMonitorEventDevice(geminiRequestContext, ruleType.EventId, new Guid(source.SourceId));
				}
				return;
			}
			catch (ArgumentNullException ex)
			{
				ServerExceptionUtilities.LogException(ex);
				return;
			}
			catch (UnauthorizedAccessException ex2)
			{
				ServerExceptionUtilities.LogException(ex2);
				return;
			}
			catch (ManagementServerServiceUnavailableException ex3)
			{
				ServerExceptionUtilities.LogException(ex3);
				return;
			}
			catch (RecordingServerCommunicationException ex4)
			{
				ServerExceptionUtilities.LogException(ex4);
				return;
			}
			catch (Exception ex5)
			{
				ServerExceptionUtilities.LogException(ex5);
				return;
			}
		}
		try
		{
			if (sources == null)
			{
				return;
			}
			foreach (EventSource source2 in sources)
			{
				SystemMonitorEvent.TriggerSystemMonitorEvent(geminiRequestContext, ruleType.EventId, new Guid(source2.SourceId));
			}
		}
		catch (ArgumentNullException ex6)
		{
			ServerExceptionUtilities.LogException(ex6);
		}
		catch (UnauthorizedAccessException ex7)
		{
			ServerExceptionUtilities.LogException(ex7);
		}
		catch (ManagementServerServiceUnavailableException ex8)
		{
			ServerExceptionUtilities.LogException(ex8);
		}
		catch (RecordingServerCommunicationException ex9)
		{
			ServerExceptionUtilities.LogException(ex9);
		}
		catch (Exception ex10)
		{
			ServerExceptionUtilities.LogException(ex10);
		}
	}

	public static bool LookupSourcesRequired(string ruleType)
	{
		if (ruleType.Equals("47AD1879-4F04-4E71-8EE8-6F9D2DAC9148", StringComparison.OrdinalIgnoreCase) || ruleType.Equals("2917B174-7E8E-490E-A402-6F5A76E4F9DD", StringComparison.OrdinalIgnoreCase) || ruleType.Equals("7AEFC712-5251-4760-8CEB-2D95DA85E5EF", StringComparison.OrdinalIgnoreCase) || ruleType.Equals("4a86f306-5a6f-43ef-9c21-f7f52fa86ac8", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		return true;
	}

	public static bool IsOldMultiRuleState(string ruleType, string instancePath)
	{
		if (ruleType.Equals("BDFC173B-43C7-4600-A483-82ED37CFD73C", StringComparison.OrdinalIgnoreCase) || ruleType.Equals("47B9163F-D9EB-43B4-BB3F-4BCCA591D5F4", StringComparison.OrdinalIgnoreCase) || ruleType.Equals("D128E0E8-57E5-481E-8936-74423687CA82", StringComparison.OrdinalIgnoreCase) || ruleType.Equals("E8240A18-9A93-424D-B45C-E83DCDD99EB5", StringComparison.OrdinalIgnoreCase))
		{
			return IsOldMultiRulePath(instancePath);
		}
		return false;
	}

	private static bool IsOldMultiRulePath(string counterPath)
	{
		if (counterPath == null)
		{
			throw new ArgumentNullException("counterPath");
		}
		return counterPath.LastIndexOf('(') != -1;
	}

	public static bool CameraStateChangeOnOtherDevice(IGeminiRequestContext requestContext, string counterPath, string type)
	{
		if (type.Equals("4a86f306-5a6f-43ef-9c21-f7f52fa86ac8", StringComparison.OrdinalIgnoreCase) || type.Equals("7AEFC712-5251-4760-8CEB-2D95DA85E5EF", StringComparison.OrdinalIgnoreCase) || type.Equals("47AD1879-4F04-4E71-8EE8-6F9D2DAC9148", StringComparison.OrdinalIgnoreCase) || type.Equals("2917B174-7E8E-490E-A402-6F5A76E4F9DD", StringComparison.OrdinalIgnoreCase))
		{
			string text = ExtractIdFromPath(counterPath);
			if (!Guid.TryParse(text, out var result))
			{
				return true;
			}
			if (_deviceTypesCache.TryGetValue(result, out var value))
			{
				return value != DeviceType.Camera;
			}
			using DeviceComponent deviceComponent = requestContext.CreateComponent<DeviceComponent>();
			Device device = deviceComponent.QueryDeviceById(text);
			if (device == null)
			{
				_deviceTypesCache.TryAdd(result, DeviceType.Unknown);
				return true;
			}
			_deviceTypesCache.TryAdd(result, device.DeviceType);
			return device.DeviceType != DeviceType.Camera;
		}
		return false;
	}

	public static List<EventSource> FilterSources(CounterStateChangeData data, List<EventSource> allSources, RuleEventType type)
	{
		List<EventSource> list = new List<EventSource>();
		switch (type.EventType)
		{
		case 3:
		case 4:
		case 5:
		case 10:
		{
			string text = ExtractIdFromPath(data.CounterPath);
			if (!string.IsNullOrEmpty(text))
			{
				list.Add(new EventSource(text, string.Empty, data.DataCollectorId, string.Empty));
			}
			else
			{
				ServerExceptionUtilities.LogException(new InvalidArgumentException("Error while handling System Monitoring state change. Device Id is not found."));
			}
			break;
		}
		case 8:
		{
			if (GetServiceIdFromCounterPath(data.CounterPath, out var serviceId))
			{
				list = allSources.Where((EventSource i) => i.ServerType.Equals(serviceId, StringComparison.OrdinalIgnoreCase)).ToList();
				if (list.Count != 1)
				{
					Trace.WriteLine("SystemMonitorManager: ServiceAvailable: Single source with specified type was not found");
				}
			}
			break;
		}
		case 7:
		case 11:
		case 12:
			if (allSources != null)
			{
				list = allSources.Where((EventSource i) => i.ServerType.Equals(ServiceTypes.ServiceType_RecordingService.ToString(), StringComparison.OrdinalIgnoreCase) || i.ServerType.Equals(ServiceTypes.ServiceType_FailoverService.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
				if (list.Count != 1)
				{
					Trace.WriteLine("SystemMonitorManager: StorageRetentionTime: Single source with specified type was not found");
				}
			}
			break;
		case 14:
		{
			string sourceId2 = ExtractIdFromPath(data.CounterPath);
			list.Add(new EventSource(sourceId2, ServiceTypes.ServiceType_FailoverService.ToString(), data.DataCollectorId, string.Empty));
			break;
		}
		case 18:
		{
			string sourceId = ExtractIdFromPath(data.CounterPath);
			list.Add(new EventSource(sourceId, ServiceTypes.ServiceType_FailoverService.ToString(), data.DataCollectorId, string.Empty));
			break;
		}
		case 15:
		case 16:
		case 17:
			if (allSources != null)
			{
				list = allSources.Where((EventSource i) => i.ServerType.Equals(ServiceTypes.ServiceType_RecordingService.ToString(), StringComparison.OrdinalIgnoreCase) || i.ServerType.Equals(ServiceTypes.ServiceType_FailoverService.ToString(), StringComparison.OrdinalIgnoreCase) || i.ServerType.Equals(ServiceTypes.ServiceType_MobileService.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
			}
			break;
		default:
			list = allSources;
			break;
		}
		return list;
	}

	public static List<EventSource> LookupSources(IGeminiRequestContext geminiRequestContext, CounterStateChangeData data, RuleEventType type)
	{
		List<EventSource> list = new List<EventSource>();
		switch (type.EventType)
		{
		case 3:
		case 4:
		case 5:
		case 10:
		{
			string text = ExtractIdFromPath(data.CounterPath);
			if (!string.IsNullOrEmpty(text) && Guid.TryParse(text, out var _))
			{
				list.Add(new EventSource(text, string.Empty, data.DataCollectorId, string.Empty));
			}
			else
			{
				ServerExceptionUtilities.LogException(new InvalidArgumentException("Error while handling System Monitoring state change. Device Id is not found."));
			}
			break;
		}
		case 8:
		{
			if (GetServiceIdFromCounterPath(data.CounterPath, out var serviceId))
			{
				list = (from i in GetSourceByDataCollector(geminiRequestContext, data.DataCollectorId)
					where i.ServerType.Equals(serviceId, StringComparison.OrdinalIgnoreCase)
					select i).ToList();
				if (list.Count != 1)
				{
					Trace.WriteLine("SystemMonitorManager: ServiceAvailable: Single source with specified type was not found");
				}
			}
			break;
		}
		case 7:
		case 11:
		case 12:
		{
			List<EventSource> sourceByDataCollector = GetSourceByDataCollector(geminiRequestContext, data.DataCollectorId);
			if (sourceByDataCollector != null)
			{
				list = sourceByDataCollector.Where((EventSource i) => i.ServerType.Equals(ServiceTypes.ServiceType_RecordingService.ToString(), StringComparison.OrdinalIgnoreCase) || i.ServerType.Equals(ServiceTypes.ServiceType_FailoverService.ToString(), StringComparison.OrdinalIgnoreCase)).ToList();
				if (list.Count != 1)
				{
					Trace.WriteLine("SystemMonitorManager: StorageRetentionTime: Single source with specified type was not found");
				}
			}
			break;
		}
		case 14:
		{
			string sourceId2 = ExtractIdFromPath(data.CounterPath);
			list.Add(new EventSource(sourceId2, ServiceTypes.ServiceType_FailoverService.ToString(), data.DataCollectorId, string.Empty));
			break;
		}
		case 18:
		{
			string sourceId = ExtractIdFromPath(data.CounterPath);
			list.Add(new EventSource(sourceId, ServiceTypes.ServiceType_FailoverService.ToString(), data.DataCollectorId, string.Empty));
			break;
		}
		default:
			list = GetSourceByDataCollector(geminiRequestContext, data.DataCollectorId);
			break;
		}
		return list;
	}

	public static RuleEventType GetRuleEventType(CounterStateChangeData data)
	{
		if (_ruleEventTypes.Count == 0)
		{
			GenerateEventRuleSources();
		}
		return _ruleEventTypes.FirstOrDefault((RuleEventType i) => string.Compare(i.RuleId, data.CounterRuleId, StringComparison.OrdinalIgnoreCase) == 0 && i.State == (CounterState)data.State);
	}

	public static string ExtractIdFromPath(string path)
	{
		int num = path.LastIndexOf('[');
		int num2 = path.LastIndexOf(']');
		if (num != -1 && num2 != -1 && num < num2)
		{
			return path.Substring(num + 1, num2 - num - 1);
		}
		return string.Empty;
	}

	public static List<EventSource> GetSourceByDataCollector(IGeminiRequestContext geminiRequestContext, int dataCollectorId)
	{
		using PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = geminiRequestContext.CreateComponent<PerformanceStateRuleConfigComponent>();
		performanceStateRuleConfigComponent.QueryAllMachinesByServerType(PerformanceRuleServiceTypes.RuleServiceTypes).TryGetValue(dataCollectorId, out var value);
		return value;
	}

	public static IEnumerable<EventSource> GetServerSourcesByIds(IGeminiRequestContext geminiRequestContext, IEnumerable<Guid> ids)
	{
		using PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = geminiRequestContext.CreateComponent<PerformanceStateRuleConfigComponent>();
		return performanceStateRuleConfigComponent.QueryAllMachinesByServerType(PerformanceRuleServiceTypes.RuleServiceTypes).Values.SelectMany((List<EventSource> s) => s.Where((EventSource e) => ids.Contains(Guid.Parse(e.ServerType)) || ids.Contains(Guid.Parse(e.SourceId))));
	}

	public static Dictionary<string, int> GetCameraSourcesByIds(IGeminiRequestContext geminiRequestContext, IEnumerable<Guid> ids)
	{
		if (ids == null || !ids.Any())
		{
			return new Dictionary<string, int>();
		}
		string[] list;
		if (ids.Contains(WellKnownDevicesGroupIds.AllCameras))
		{
			using DeviceComponent deviceComponent = geminiRequestContext.CreateComponent<DeviceComponent>();
			list = deviceComponent.QueryDeviceIdsGeneric(null, null, DeviceTypeSearch.Camera);
		}
		else
		{
			list = ids.Select((Guid i) => Convert.ToString(i, CultureInfo.InvariantCulture)).ToArray();
		}
		using PerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = geminiRequestContext.CreateComponent<PerformanceStateRuleConfigComponent>();
		return performanceStateRuleConfigComponent.QueryDataCollectorIdsByDeviceIds(list);
	}

	public static List<int> GetActiveRecorders(IGeminiRequestContext requestContext)
	{
		using IPerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreatePerformanceStateRuleConfigComponent();
		return performanceStateRuleConfigComponent.QueryActiveRecorders();
	}

	private static void GenerateEventRuleSources()
	{
		_ruleEventTypes.Add(new RuleEventType("BDFC173B-43C7-4600-A483-82ED37CFD73C", BuiltInEventTypes.EventIDs.SystemMonitorRetentionTimeCritical, 7, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("BDFC173B-43C7-4600-A483-82ED37CFD73C", BuiltInEventTypes.EventIDs.SystemMonitorRetentionTimeWarning, 7, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("BDFC173B-43C7-4600-A483-82ED37CFD73C", BuiltInEventTypes.EventIDs.SystemMonitorRetentionTimeNormal, 7, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("BDFC173B-43C7-4600-A483-82ED37CFD73C", BuiltInEventTypes.EventIDs.SystemMonitorRetentionTimeUndefined, 7, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("1E41EC7E-DA4A-44B4-BDF3-E2EB41E18B4C", BuiltInEventTypes.EventIDs.SystemMonitorCpuUsageCritical, 1, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("1E41EC7E-DA4A-44B4-BDF3-E2EB41E18B4C", BuiltInEventTypes.EventIDs.SystemMonitorCpuUsageWarning, 1, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("1E41EC7E-DA4A-44B4-BDF3-E2EB41E18B4C", BuiltInEventTypes.EventIDs.SystemMonitorCpuUsageNormal, 1, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("1E41EC7E-DA4A-44B4-BDF3-E2EB41E18B4C", BuiltInEventTypes.EventIDs.SystemMonitorCpuUsageUndefined, 1, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("47B9163F-D9EB-43B4-BB3F-4BCCA591D5F4", BuiltInEventTypes.EventIDs.SystemMonitorFreeSpaceCritical, 6, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("47B9163F-D9EB-43B4-BB3F-4BCCA591D5F4", BuiltInEventTypes.EventIDs.SystemMonitorFreeSpaceWarning, 6, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("47B9163F-D9EB-43B4-BB3F-4BCCA591D5F4", BuiltInEventTypes.EventIDs.SystemMonitorFreeSpaceNormal, 6, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("47B9163F-D9EB-43B4-BB3F-4BCCA591D5F4", BuiltInEventTypes.EventIDs.SystemMonitorFreeSpaceUndefined, 6, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("47AD1879-4F04-4E71-8EE8-6F9D2DAC9148", BuiltInEventTypes.EventIDs.SystemMonitorLiveFpsCritical, 3, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("47AD1879-4F04-4E71-8EE8-6F9D2DAC9148", BuiltInEventTypes.EventIDs.SystemMonitorLiveFpsWarning, 3, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("47AD1879-4F04-4E71-8EE8-6F9D2DAC9148", BuiltInEventTypes.EventIDs.SystemMonitorLiveFpsNormal, 3, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("47AD1879-4F04-4E71-8EE8-6F9D2DAC9148", BuiltInEventTypes.EventIDs.SystemMonitorLiveFpsUndefined, 3, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("7F9350E4-55B5-43BF-8146-78D2DC531A42", BuiltInEventTypes.EventIDs.SystemMonitorMemoryUsageCritical, 2, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("7F9350E4-55B5-43BF-8146-78D2DC531A42", BuiltInEventTypes.EventIDs.SystemMonitorMemoryUsageWarning, 2, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("7F9350E4-55B5-43BF-8146-78D2DC531A42", BuiltInEventTypes.EventIDs.SystemMonitorMemoryUsageNormal, 2, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("7F9350E4-55B5-43BF-8146-78D2DC531A42", BuiltInEventTypes.EventIDs.SystemMonitorMemoryUsageUndefined, 2, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("2917B174-7E8E-490E-A402-6F5A76E4F9DD", BuiltInEventTypes.EventIDs.SystemMonitorRecordingFpsCritical, 4, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("2917B174-7E8E-490E-A402-6F5A76E4F9DD", BuiltInEventTypes.EventIDs.SystemMonitorRecordingFpsWarning, 4, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("2917B174-7E8E-490E-A402-6F5A76E4F9DD", BuiltInEventTypes.EventIDs.SystemMonitorRecordingFpsNormal, 4, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("2917B174-7E8E-490E-A402-6F5A76E4F9DD", BuiltInEventTypes.EventIDs.SystemMonitorRecordingFpsUndefined, 4, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("7AEFC712-5251-4760-8CEB-2D95DA85E5EF", BuiltInEventTypes.EventIDs.SystemMonitorUsedSpaceCritical, 5, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("7AEFC712-5251-4760-8CEB-2D95DA85E5EF", BuiltInEventTypes.EventIDs.SystemMonitorUsedSpaceWarning, 5, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("7AEFC712-5251-4760-8CEB-2D95DA85E5EF", BuiltInEventTypes.EventIDs.SystemMonitorUsedSpaceNormal, 5, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("7AEFC712-5251-4760-8CEB-2D95DA85E5EF", BuiltInEventTypes.EventIDs.SystemMonitorUsedSpaceUndefined, 5, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("4a86f306-5a6f-43ef-9c21-f7f52fa86ac8", Guid.Empty, 10, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("4a86f306-5a6f-43ef-9c21-f7f52fa86ac8", Guid.Empty, 10, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("4a86f306-5a6f-43ef-9c21-f7f52fa86ac8", Guid.Empty, 10, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("4a86f306-5a6f-43ef-9c21-f7f52fa86ac8", Guid.Empty, 10, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("063EC923-9E6B-4A95-B949-AE5A76DB8B5C", BuiltInEventTypes.EventIDs.SystemMonitorServiceAvailableNormal, 8, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("063EC923-9E6B-4A95-B949-AE5A76DB8B5C", Guid.Empty, 8, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("063EC923-9E6B-4A95-B949-AE5A76DB8B5C", BuiltInEventTypes.EventIDs.SystemMonitorServiceAvailableCritical, 8, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("063EC923-9E6B-4A95-B949-AE5A76DB8B5C", BuiltInEventTypes.EventIDs.SystemMonitorServiceAvailableUndefined, 8, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("D128E0E8-57E5-481E-8936-74423687CA82", Guid.Empty, 11, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("D128E0E8-57E5-481E-8936-74423687CA82", Guid.Empty, 11, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("D128E0E8-57E5-481E-8936-74423687CA82", Guid.Empty, 11, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("D128E0E8-57E5-481E-8936-74423687CA82", Guid.Empty, 11, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("E8240A18-9A93-424D-B45C-E83DCDD99EB5", Guid.Empty, 12, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("E8240A18-9A93-424D-B45C-E83DCDD99EB5", Guid.Empty, 12, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("E8240A18-9A93-424D-B45C-E83DCDD99EB5", Guid.Empty, 12, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("E8240A18-9A93-424D-B45C-E83DCDD99EB5", Guid.Empty, 12, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("009B1207-FD5F-4506-9660-4074FF04A4A3", Guid.Empty, 14, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("009B1207-FD5F-4506-9660-4074FF04A4A3", Guid.Empty, 14, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("009B1207-FD5F-4506-9660-4074FF04A4A3", Guid.Empty, 14, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("009B1207-FD5F-4506-9660-4074FF04A4A3", Guid.Empty, 14, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("E6330FA6-D3FE-4E35-AD50-21ECB7608931", Guid.Empty, 18, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("E6330FA6-D3FE-4E35-AD50-21ECB7608931", Guid.Empty, 18, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("E6330FA6-D3FE-4E35-AD50-21ECB7608931", Guid.Empty, 18, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("E6330FA6-D3FE-4E35-AD50-21ECB7608931", Guid.Empty, 18, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("FC6A2188-C1AF-48B5-ACB7-E88FE093D30C", BuiltInEventTypes.EventIDs.SystemMonitorGPUDecodingCritical, 15, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("FC6A2188-C1AF-48B5-ACB7-E88FE093D30C", BuiltInEventTypes.EventIDs.SystemMonitorGPUDecodingWarning, 15, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("FC6A2188-C1AF-48B5-ACB7-E88FE093D30C", BuiltInEventTypes.EventIDs.SystemMonitorGPUDecodingNormal, 15, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("FC6A2188-C1AF-48B5-ACB7-E88FE093D30C", BuiltInEventTypes.EventIDs.SystemMonitorGPUDecodingUndefined, 15, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("238C1803-601B-4441-9FDE-46E7CBE2FF6E", BuiltInEventTypes.EventIDs.SystemMonitorGPUMemoryCritical, 16, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("238C1803-601B-4441-9FDE-46E7CBE2FF6E", BuiltInEventTypes.EventIDs.SystemMonitorGPUMemoryWarning, 16, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("238C1803-601B-4441-9FDE-46E7CBE2FF6E", BuiltInEventTypes.EventIDs.SystemMonitorGPUMemoryNormal, 16, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("238C1803-601B-4441-9FDE-46E7CBE2FF6E", BuiltInEventTypes.EventIDs.SystemMonitorGPUMemoryUndefined, 16, CounterState.Undefined));
		_ruleEventTypes.Add(new RuleEventType("3732961A-025E-4D80-AEF9-5B884E6B2BAF", BuiltInEventTypes.EventIDs.SystemMonitorGPURenderingCritical, 17, CounterState.Critical));
		_ruleEventTypes.Add(new RuleEventType("3732961A-025E-4D80-AEF9-5B884E6B2BAF", BuiltInEventTypes.EventIDs.SystemMonitorGPURenderingWarning, 17, CounterState.Warning));
		_ruleEventTypes.Add(new RuleEventType("3732961A-025E-4D80-AEF9-5B884E6B2BAF", BuiltInEventTypes.EventIDs.SystemMonitorGPURenderingNormal, 17, CounterState.Normal));
		_ruleEventTypes.Add(new RuleEventType("3732961A-025E-4D80-AEF9-5B884E6B2BAF", BuiltInEventTypes.EventIDs.SystemMonitorGPURenderingUndefined, 17, CounterState.Undefined));
	}

	public static CounterStateChangeData[] SetStatesByRule(IGeminiRequestContext requestContext, string counterRuleId, int newState)
	{
		using IPerformanceStateRuleConfigComponent performanceStateRuleConfigComponent = requestContext.CreatePerformanceStateRuleConfigComponent();
		return performanceStateRuleConfigComponent.SetStatesByRule(counterRuleId, newState);
	}

	public static bool GetServiceIdFromCounterPath(string counterPath, out string instanceName)
	{
		int num = counterPath.LastIndexOf('(');
		int num2 = counterPath.LastIndexOf(')');
		if (num != -1 && num2 != -1 && num < num2)
		{
			instanceName = counterPath.Substring(num + 1, num2 - num - 1);
			return true;
		}
		instanceName = string.Empty;
		return false;
	}
}
