using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Common.ResultResponse;
using VideoOS.DataCollector.Service.Entities;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.PerfCounterConfiguration.Commands.RegisterDataCollector;
using VideoOS.DataCollector.Service.Persistence;
using VideoOS.DataCollector.Service.Rule.Entities.Rules;
using VideoOS.DataCollector.Service.Rule.Entities.State;
using VideoOS.DataCollector.Service.Rule.Refresh;
using VideoOS.DataCollector.Service.Rule.RuleConfiguration.Commands;

namespace VideoOS.DataCollector.Service.Rule.RuleConfiguration.Refresh.EventHandlers.RefreshRules;

public class RefreshRuleConfigurationEventHandler : INotificationHandler<RefreshRuleConfigurationEvent>
{
	public class RuleConfiguration
	{
		public bool IsDefault { get; }

		public CounterStateRuleConfiguration Rule { get; }

		public RuleConfiguration(CounterStateRuleConfiguration rule, bool isDefault)
		{
			Rule = rule;
			IsDefault = isDefault;
		}
	}

	private readonly IConfigurationCache _configurationCache;

	private readonly IMediator _mediator;

	private readonly ILogger<RefreshRuleConfigurationEventHandler> _logger;

	private readonly IActivePerformanceCounterRepository _activePerformanceCounterRepository;

	private readonly IRuleRepository _repository;

	private readonly IInstallerServicesRepository _servicesRepository;

	private readonly IStateHandlerFactory _stateHandlerFactory;

	private readonly ICounterRuleFactory _counterRuleFactory;

	private readonly INvidiaGPURuleFactory _nvidiaGPURuleFactory;

	public RefreshRuleConfigurationEventHandler(IConfigurationCache configurationCache, IActivePerformanceCounterRepository activePerformanceCounterRepository, IRuleRepository repository, IMediator mediator, IStateHandlerFactory stateHandlerFactory, IInstallerServicesRepository servicesRepository, ILogger<RefreshRuleConfigurationEventHandler> logger, ICounterRuleFactory counterRuleFactory, INvidiaGPURuleFactory nvidiaGPURuleFactory)
	{
		_configurationCache = configurationCache;
		_activePerformanceCounterRepository = activePerformanceCounterRepository;
		_repository = repository;
		_servicesRepository = servicesRepository;
		_mediator = mediator;
		_stateHandlerFactory = stateHandlerFactory;
		_logger = logger;
		_counterRuleFactory = counterRuleFactory;
		_nvidiaGPURuleFactory = nvidiaGPURuleFactory;
	}

	public async Task Handle(RefreshRuleConfigurationEvent notification, CancellationToken cancellationToken)
	{
		if (notification != null && !(notification.Item == null))
		{
			_repository.RuleConfigurations = notification.Item.CounterStateRules.Select((CounterStateRuleConfiguration r) => new RuleConfiguration(r, string.IsNullOrEmpty(r.CounterPath))).ToList();
			List<RuleConfiguration> ruleConfiguration = notification.Item.CounterStateRules.Select((CounterStateRuleConfiguration r) => new RuleConfiguration(r, string.IsNullOrEmpty(r.CounterPath))).ToList();
			await UpdateRules(_activePerformanceCounterRepository.Counters.Values, ruleConfiguration, cancellationToken);
			IEnumerable<string> activeRules = GetActiveRulePaths(_repository);
			IMediator mediator = _mediator;
			await mediator.Send((IRequest<Result<Unit>>)new DeleteNonActiveStatesServerCommand(await _configurationCache.GetOrAddDataCollectorIdAsync(cancellationToken), activeRules), cancellationToken);
		}
	}

	private async Task UpdateRules(ICollection<PerformanceCounterCollector> activePerformanceCounters, List<RuleConfiguration> ruleConfiguration, CancellationToken cancellationToken)
	{
		HashSet<string> activeCounterRules = new HashSet<string>();
		Dictionary<string, RuleConfiguration> nonDefaultCounterRules = ruleConfiguration.Where((RuleConfiguration r) => !r.IsDefault).ToDictionary((RuleConfiguration r) => r.Rule.CounterPath);
		await UpdateNonCounterRules(ruleConfiguration, cancellationToken);
		activeCounterRules = await UpdateNonDefaultCounterRules(nonDefaultCounterRules, activePerformanceCounters, activeCounterRules, cancellationToken);
		activeCounterRules = await UpdateDefaultCounterRules(nonDefaultCounterRules, ruleConfiguration, activeCounterRules, activePerformanceCounters, cancellationToken);
		await RemoveObsoleteRules((from r in _repository.CounterRules
			where !activeCounterRules.Contains(r.Key)
			select r.Value.CounterPath).ToArray(), cancellationToken);
	}

	private async Task UpdateNonCounterRules(List<RuleConfiguration> ruleConfiguration, CancellationToken cancellationToken)
	{
		Dictionary<string, Action<CounterStateRuleConfiguration, int>> nonCounterAddActions = GenerateNonCounterActions();
		foreach (RuleConfiguration item in ruleConfiguration.Where((RuleConfiguration r) => nonCounterAddActions.ContainsKey(r.Rule.CounterRuleId)))
		{
			Action<CounterStateRuleConfiguration, int> action = nonCounterAddActions[item.Rule.CounterRuleId];
			CounterStateRuleConfiguration rule = item.Rule;
			action(rule, await _configurationCache.GetOrAddDataCollectorIdAsync(cancellationToken));
		}
	}

	private async Task<HashSet<string>> UpdateNonDefaultCounterRules(Dictionary<string, RuleConfiguration> specificCounterRules, ICollection<PerformanceCounterCollector> activePerformanceCounters, HashSet<string> activeCounterRules, CancellationToken cancellationToken)
	{
		foreach (RuleConfiguration ruleConfig in specificCounterRules.Values)
		{
			PerformanceCounterCollector performanceCounterCollector = activePerformanceCounters.FirstOrDefault((PerformanceCounterCollector i) => i.CategoryName.Equals(ruleConfig.Rule.CategoryName, StringComparison.OrdinalIgnoreCase) && i.CounterName.Equals(ruleConfig.Rule.CounterName, StringComparison.OrdinalIgnoreCase));
			if (performanceCounterCollector != null && !string.IsNullOrEmpty(ruleConfig.Rule.CounterPath))
			{
				string text = await AddOrUpdateCounterRule(ruleConfig.Rule, ruleConfig.Rule.CounterPath, isDefault: false, performanceCounterCollector.TriggerStatePerValue, performanceCounterCollector.CounterDirection, cancellationToken);
				if (text != null)
				{
					activeCounterRules.Add(text);
				}
			}
		}
		return activeCounterRules;
	}

	private async Task<HashSet<string>> UpdateDefaultCounterRules(Dictionary<string, RuleConfiguration> specificCounterRules, List<RuleConfiguration> ruleConfiguration, HashSet<string> activeCounterRules, ICollection<PerformanceCounterCollector> activePerformanceCounters, CancellationToken cancellationToken)
	{
		foreach (RuleConfiguration ruleConfig in ruleConfiguration.Where((RuleConfiguration r) => r.IsDefault))
		{
			IEnumerable<PerformanceCounterCollector> enumerable = activePerformanceCounters.Where((PerformanceCounterCollector p) => p.CategoryName == ruleConfig.Rule.CategoryName && p.CounterName == ruleConfig.Rule.CounterName);
			foreach (PerformanceCounterCollector item in enumerable)
			{
				if (!specificCounterRules.ContainsKey(item.Path))
				{
					string text = await AddOrUpdateCounterRule(ruleConfig.Rule, item.Path, isDefault: true, item.TriggerStatePerValue, item.CounterDirection, cancellationToken);
					if (text != null)
					{
						activeCounterRules.Add(text);
					}
				}
			}
		}
		return activeCounterRules;
	}

	private async Task<string?> AddOrUpdateCounterRule(CounterStateRuleConfiguration ruleConfig, string path, bool isDefault, bool triggerPerValue, bool direction, CancellationToken cancellationToken)
	{
		if (!_counterRuleFactory.ValidateCounter(ruleConfig, path))
		{
			return null;
		}
		string key = ruleConfig.CounterRuleId + path;
		if (!_repository.CounterRules.TryGetValue(key, out IRule value))
		{
			CounterStateRuleConfiguration newRuleConfiguration = ruleConfig with
			{
				CounterPath = path
			};
			if (_repository.MultiRules.TryGetValue(newRuleConfiguration.CounterRuleId, out IMultiCounterRule value2))
			{
				value2.RegisterPerformanceCounterInstance(newRuleConfiguration.CounterPath);
			}
			ICounterRuleFactory counterRuleFactory = _counterRuleFactory;
			value = counterRuleFactory.CreateCounterRule(await _configurationCache.GetOrAddDataCollectorIdAsync(cancellationToken), newRuleConfiguration, isDefault, triggerPerValue, direction, _stateHandlerFactory);
			_repository.CounterRules.TryAdd(key, value);
			string message = string.Format(CultureInfo.InvariantCulture, "Adding system dashboard threshold: Name: {0}, Counter: {1}, Warning: {2}, Critical: {3}, Time: {4}", newRuleConfiguration.Name, path, newRuleConfiguration.WarningThreshold, newRuleConfiguration.CriticalThreshold, newRuleConfiguration.TimeWindow);
			_logger.LogInformation(message);
		}
		else if (value.Refresh(ruleConfig))
		{
			string message2 = string.Format(CultureInfo.InvariantCulture, "Updating system dashboard threshold: Name: {0}, Counter: {1}, Warning: {2}, Critical: {3}, Time: {4}", ruleConfig.Name, value.CounterPath, ruleConfig.WarningThreshold, ruleConfig.CriticalThreshold, ruleConfig.TimeWindow);
			_logger.LogInformation(message2);
		}
		return key;
	}

	private async Task RemoveObsoleteRules(string[] obsoleteCounterPaths, CancellationToken cancellationToken)
	{
		await RemoveObsoleteRules(obsoleteCounterPaths, _repository.CounterRules, cancellationToken);
		await RemoveObsoleteRules(obsoleteCounterPaths, _repository.NonCounterRules, cancellationToken);
	}

	private async Task RemoveObsoleteRules(string[] obsoleteCounterPaths, ConcurrentDictionary<string, IRule> rules, CancellationToken cancellationToken)
	{
		List<IRule> list = new List<IRule>();
		foreach (string text in obsoleteCounterPaths)
		{
			foreach (IRule value2 in rules.Values)
			{
				if (value2.CounterPath == text)
				{
					list.Add(value2);
				}
			}
		}
		foreach (IRule rule in list)
		{
			if (!rules.TryRemove(rule.RuleId + rule.CounterPath, out IRule value))
			{
				continue;
			}
			if (_repository.MultiRules.TryGetValue(value.RuleId, out IMultiCounterRule multiRule))
			{
				var (flag, stateChange) = await multiRule.UnRegisterPerformanceCounterInstance(value.CounterPath);
				if (flag && stateChange != null)
				{
					StateChangeData stateChange2 = new StateChangeData(await _configurationCache.GetOrAddDataCollectorIdAsync(cancellationToken), rule.RuleId, rule.CounterPath, (int)stateChange.CounterState, stateChange.TriggerValue);
					await _mediator.Send((IRequest<Result<Unit>>)new StateChangeServerCommand(stateChange2), cancellationToken);
				}
				multiRule.ResetState();
			}
			else
			{
				_logger.LogInformation("Deleting state: " + value.CounterPath + " ");
				await SendDeletedState(value, new StateChange(State.Undefined, -1f), cancellationToken);
			}
			multiRule = null;
		}
	}

	private IEnumerable<string> GetActiveRulePaths(IRuleRepository repository)
	{
		List<string> list = repository.CounterRules.Values.Select((IRule r) => r.CounterPath).ToList();
		list.AddRange(repository.NonCounterRules.Values.Select((IRule r) => r.CounterPath).ToList());
		list.AddRange(repository.MultiRules.Values.Select((IMultiCounterRule r) => r.CounterPath).ToList());
		return list;
	}

	private async Task SendDeletedState(IRule rule, StateChange stateChange, CancellationToken stoppingToken)
	{
		DeleteStateChangeData stateChange2 = new DeleteStateChangeData(await _configurationCache.GetOrAddDataCollectorIdAsync(stoppingToken), rule.RuleId, rule.CounterPath, (int)stateChange.CounterState, stateChange.TriggerValue);
		await _mediator.Send((IRequest<Result<Unit>>)new DeleteStateServerCommand(stateChange2), stoppingToken);
	}

	private void AddOrUpdateNvidiaGpuRule(CounterStateRuleConfiguration ruleConfiguration, int dataCollectorId)
	{
		string text = string.Format(CultureInfo.InvariantCulture, "\\{0}(_total)\\{1}", ruleConfiguration.CategoryName, ruleConfiguration.CounterName);
		string key = string.Format(CultureInfo.InvariantCulture, "{0}{1}", ruleConfiguration.CounterRuleId, text);
		if (!_repository.NonCounterRules.TryGetValue(key, out IRule value))
		{
			CounterStateRuleConfiguration counterStateRuleConfiguration = ruleConfiguration with
			{
				CounterPath = text
			};
			NvidiaGPURule value2 = _nvidiaGPURuleFactory.CreateGPURule(dataCollectorId, counterStateRuleConfiguration, _stateHandlerFactory);
			_repository.NonCounterRules.TryAdd(key, value2);
			string message = string.Format(CultureInfo.InvariantCulture, "Adding system dashboard threshold: Name: {0}, Counter: {1}, Warning: {2}, Critical: {3}, Time: {4}", counterStateRuleConfiguration.Name, counterStateRuleConfiguration.CounterPath, counterStateRuleConfiguration.WarningThreshold, counterStateRuleConfiguration.CriticalThreshold, counterStateRuleConfiguration.TimeWindow);
			_logger.LogInformation(message);
		}
		else
		{
			value.Refresh(ruleConfiguration);
		}
	}

	private void UpdateServiceAvailableRules(CounterStateRuleConfiguration ruleConfiguration, int dataCollectorId)
	{
		HashSet<string> hashSet = new HashSet<string>();
		foreach (KeyValuePair<string, IRule> nonCounterRule in _repository.NonCounterRules)
		{
			if (nonCounterRule.Value is ServiceAvailableRule)
			{
				hashSet.Add(nonCounterRule.Key);
			}
		}
		foreach (KeyValuePair<string, string> service in _servicesRepository.Services)
		{
			string text = ruleConfiguration.CounterRuleId + service;
			if (!_repository.NonCounterRules.ContainsKey(text))
			{
				_repository.NonCounterRules.TryAdd(text, ServiceAvailableRuleFactory.CreateServiceAvailableRule(dataCollectorId, ruleConfiguration.Name, service.Value, service.Key, ruleConfiguration.CounterRuleId, _stateHandlerFactory));
			}
			else
			{
				hashSet.Remove(text);
			}
		}
		foreach (string item in hashSet)
		{
			_repository.NonCounterRules.Remove(item, out var _);
		}
	}

	private Dictionary<string, Action<CounterStateRuleConfiguration, int>> GenerateNonCounterActions()
	{
		return new Dictionary<string, Action<CounterStateRuleConfiguration, int>>(StringComparer.OrdinalIgnoreCase)
		{
			{ "063EC923-9E6B-4A95-B949-AE5A76DB8B5C", UpdateServiceAvailableRules },
			{ "FC6A2188-C1AF-48B5-ACB7-E88FE093D30C", AddOrUpdateNvidiaGpuRule },
			{ "238C1803-601B-4441-9FDE-46E7CBE2FF6E", AddOrUpdateNvidiaGpuRule },
			{ "3732961A-025E-4D80-AEF9-5B884E6B2BAF", AddOrUpdateNvidiaGpuRule }
		};
	}
}
