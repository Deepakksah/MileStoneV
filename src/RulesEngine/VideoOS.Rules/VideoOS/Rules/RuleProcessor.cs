using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Interfaces.Scheduler;
using VideoOS.Management.Common;
using VideoOS.Rules.Interfaces;
using VideoOS.Vms.Utilities.Scheduler;

namespace VideoOS.Rules;

public sealed class RuleProcessor : IRuleProcessor, IDisposable
{
	private class GlobalizedEventInfo
	{
		public Guid RuleId { get; private set; }

		public string RuleName { get; private set; }

		public Guid EventTypeId { get; private set; }

		public string EventName { get; private set; }

		public HashSet<Guid> Sources { get; private set; }

		public GlobalizedEventInfo(Guid ruleId, string ruleName, Guid eventTypeId, string eventName)
		{
			RuleId = ruleId;
			RuleName = ruleName;
			EventTypeId = eventTypeId;
			EventName = eventName;
			Sources = new HashSet<Guid>();
		}
	}

	private ILogger logger;

	private IEventDefinitionProvider eventDefinitionProvider;

	private IGroupConfiguration groupConfiguration;

	private Queue inputQueue = new Queue();

	private volatile bool isRunning;

	private Thread workThread;

	private Hashtable rulesByStartActionTarget = new Hashtable();

	private Hashtable rulesByStopActionTarget = new Hashtable();

	private Hashtable ruleTriggerGroups = new Hashtable();

	private Hashtable ruleSources = new Hashtable();

	private Hashtable anySources = new Hashtable();

	private Hashtable rules = new Hashtable();

	private Dictionary<Guid, List<GlobalizedEventInfo>> globalizedTriggerEvents = new Dictionary<Guid, List<GlobalizedEventInfo>>();

	private object ruleCollectionsLock = new object();

	private Scheduler scheduler = new Scheduler();

	public event EventHandler<EventProcessingEventArgs> EventProcessing;

	public event EventHandler<EventProcessingEventArgs> TriggerEventAsGlobal;

	public event EventHandler<EventProcessingEventArgs> EventProcessed;

	public event EventHandler<RuleProcessingEventArgs> RuleProcessing;

	public event EventHandler<RuleProcessingErrorEventArgs> RuleProcessingError;

	public event EventHandler<RuleProcessingEventArgs> RuleProcessed;

	public event EventHandler<RuleActionProcessingEventArgs> RuleActionProcessing;

	public event EventHandler<RuleActionProcessingEventArgs> RuleActionProcessed;

	public event EventHandler<RuleEventArgs> RuleClearStateExternal;

	public RuleProcessor(ILogger logger, IEventDefinitionProvider eventDefinitionProvider, IGroupConfiguration groupConfiguration)
	{
		this.logger = logger;
		this.eventDefinitionProvider = eventDefinitionProvider;
		this.groupConfiguration = groupConfiguration;
		this.groupConfiguration.GroupMemberAdded += GroupConfiguration_GroupMemberAdded;
		this.groupConfiguration.GroupMemberDeleted += GroupConfiguration_GroupMemberDeleted;
	}

	private void OnEventProcessing(EventProcessingEventArgs e)
	{
		logger.LogDebug("OnEventProcessing, EventTypeId: {EventTypeId}", e.TriggerEventBase.EventTypeId);
		if (this.EventProcessing != null)
		{
			this.EventProcessing(this, e);
		}
	}

	private void OnTriggerEventAsGlobal(EventProcessingEventArgs e)
	{
		logger.LogInformation("OnTriggerEventAsGlobal, EventTypeId: {EventTypeId}", e.TriggerEventBase.EventTypeId);
		if (this.TriggerEventAsGlobal != null)
		{
			this.TriggerEventAsGlobal(this, e);
		}
	}

	private void OnEventProcessed(EventProcessingEventArgs e)
	{
		logger.LogDebug("OnEventProcessed, EventTypeId: {EventTypeId}", e.TriggerEventBase.EventTypeId);
		if (this.EventProcessed != null)
		{
			this.EventProcessed(this, e);
		}
	}

	private void OnRuleProcessing(RuleProcessingEventArgs e)
	{
		if (this.RuleProcessing != null)
		{
			this.RuleProcessing(this, e);
		}
	}

	private void OnRuleProcessingError(RuleProcessingErrorEventArgs e)
	{
		if (this.RuleProcessingError != null)
		{
			this.RuleProcessingError(this, e);
		}
	}

	private void OnRuleProcessed(RuleProcessingEventArgs e)
	{
		if (this.RuleProcessed != null)
		{
			this.RuleProcessed(this, e);
		}
	}

	private void OnRuleActionProcessing(RuleActionProcessingEventArgs e)
	{
		if (this.RuleActionProcessing != null)
		{
			this.RuleActionProcessing(this, e);
		}
	}

	private void OnRuleActionProcessed(RuleActionProcessingEventArgs e)
	{
		if (this.RuleActionProcessed != null)
		{
			this.RuleActionProcessed(this, e);
		}
	}

	private void OnRuleClearStateExternal(RuleEventArgs e)
	{
		if (this.RuleClearStateExternal != null)
		{
			this.RuleClearStateExternal(this, e);
		}
	}

	public void Start()
	{
		logger.LogInformation("RuleProcessor starting");
		isRunning = true;
		workThread = new Thread(RuleProcessorThreadFunction);
		workThread.Name = "RuleProcessor-WorkThread";
		workThread.IsBackground = true;
		workThread.Start();
	}

	public void Stop()
	{
		logger.LogInformation("RuleProcessor stopping");
		isRunning = false;
		lock (inputQueue)
		{
			Monitor.Pulse(inputQueue);
		}
		if (workThread != null)
		{
			workThread.Join();
		}
		scheduler.CancelAllTasks();
		logger.LogInformation("RuleProcessor stopped");
	}

	public void EnableEventQueueing()
	{
		isRunning = true;
	}

	public void UpdateGroupConfiguration(IGroupConfigurationHelper groupHelper)
	{
		lock (ruleCollectionsLock)
		{
			groupConfiguration.SetOrUpdateConfiguration(groupHelper, firstUpdate: false);
		}
	}

	public IRule[] GetRulesByGroupId(Guid groupId)
	{
		IRule[] array;
		lock (ruleCollectionsLock)
		{
			if (!(ruleTriggerGroups[groupId] is ArrayList arrayList))
			{
				array = Array.Empty<Rule>();
				array = array;
			}
			else
			{
				IRule[] array2 = (Rule[])arrayList.ToArray(typeof(Rule));
				array = array2;
			}
		}
		return array;
	}

	public void AddRule(IRule rule)
	{
		lock (ruleCollectionsLock)
		{
			if (rules.ContainsKey(rule.ID))
			{
				logger.LogWarning("Attempting to add a rule which has already been registered");
				return;
			}
			rules.Add(rule.ID, rule);
			RegisterGlobalizedEvents(rule);
			RegisterRuleBySubRuleSources(rule.StartRule);
			RegisterRuleBySubRuleActionTargets(rule.StartRule);
			if (rule.StopRule != null)
			{
				if (rule.StopRule.RuleType != RuleType.Timer && rule.StopRule.RuleType != RuleType.TimeInterval)
				{
					RegisterRuleBySubRuleSources(rule.StopRule);
				}
				RegisterRuleBySubRuleActionTargets(rule.StopRule);
			}
		}
	}

	private void EnqueueEvent(object evt)
	{
		lock (inputQueue)
		{
			inputQueue.Enqueue(evt);
			if (inputQueue.Count > 1)
			{
				logger.LogWarning("Event queue size = {InputQueueSize}", inputQueue.Count);
			}
			Monitor.PulseAll(inputQueue);
		}
	}

	private void RegisterRuleBySubRuleSources(ISubRule subRule)
	{
		if (subRule == null)
		{
			return;
		}
		lock (ruleCollectionsLock)
		{
			logger.LogInformation("Setting up {IsStartRule}Rule to listen for events, Rule: {ParentRuleId} ({ParentRuleName}); EventTypeId: {EventTypeId}", subRule.IsStartRule ? "Start" : "Stop", subRule.ParentRule.ID, subRule.ParentRule.Name, subRule.Trigger.EventTypeId);
			foreach (Guid sourceTargetGroupId in subRule.Trigger.GetSourceTargetGroupIds())
			{
				ArrayList arrayList = ruleTriggerGroups[sourceTargetGroupId] as ArrayList;
				if (arrayList == null)
				{
					arrayList = new ArrayList();
					ruleTriggerGroups[sourceTargetGroupId] = arrayList;
				}
				arrayList.Add(subRule.ParentRule);
			}
			List<Guid> sources = subRule.Trigger.GetSources();
			if (sources.Contains(SourceGuid.AnySource))
			{
				AddSubRuleToEventType(anySources, subRule, SourceGuid.AnySource);
				return;
			}
			foreach (Guid item in sources)
			{
				Hashtable hashtable = ruleSources[item] as Hashtable;
				if (hashtable == null)
				{
					hashtable = new Hashtable();
					ruleSources[item] = hashtable;
				}
				AddSubRuleToEventType(hashtable, subRule, item);
			}
		}
	}

	private void AddSubRuleToEventType(Hashtable eventTypeIds, ISubRule subRule, Guid ruleSource)
	{
		ArrayList arrayList = eventTypeIds[subRule.Trigger.EventTypeId] as ArrayList;
		if (arrayList == null)
		{
			arrayList = new ArrayList();
			eventTypeIds[subRule.Trigger.EventTypeId] = arrayList;
		}
		if (!arrayList.Contains(subRule))
		{
			arrayList.Add(subRule);
			logger.LogDebug("  Adding source, SourceId: {RuleSource}", ruleSource);
		}
	}

	private void RegisterRuleBySubRuleActionTargets(ISubRule subRule)
	{
		Hashtable hashtable = (subRule.IsStartRule ? rulesByStartActionTarget : rulesByStopActionTarget);
		foreach (IRuleAction action in subRule.Actions)
		{
			foreach (ActionTarget actionTarget in action.ActionTargets)
			{
				if (actionTarget.TargetType == ActionTargetType.Unspecified)
				{
					continue;
				}
				foreach (Guid targetItem in actionTarget.TargetItems)
				{
					ArrayList arrayList = hashtable[targetItem] as ArrayList;
					if (arrayList == null)
					{
						arrayList = (ArrayList)(hashtable[targetItem] = new ArrayList());
					}
					if (!arrayList.Contains(subRule.ParentRule))
					{
						arrayList.Add(subRule.ParentRule);
					}
				}
				foreach (Guid targetGroup in actionTarget.TargetGroups)
				{
					ArrayList arrayList3 = hashtable[targetGroup] as ArrayList;
					if (arrayList3 == null)
					{
						arrayList3 = (ArrayList)(hashtable[targetGroup] = new ArrayList());
					}
					if (!arrayList3.Contains(subRule.ParentRule))
					{
						arrayList3.Add(subRule.ParentRule);
					}
				}
			}
		}
	}

	private void RegisterGlobalizedEvents(IRule rule)
	{
		if (rule.StartRule.RuleType != RuleType.Event || rule.StopRule == null || rule.StopRule.RuleType != RuleType.Event)
		{
			return;
		}
		EventDefinition eventDefinition = eventDefinitionProvider.FindEventDefinitionFromEventTypeId(rule.StartRule.Trigger.EventTypeId);
		if (eventDefinition == null)
		{
			logger.LogWarning("RegisterGlobalizedEvents: Unable to find start event with id {EventTypeId}", rule.StartRule.Trigger.EventTypeId);
		}
		else
		{
			if (!eventDefinition.OccursGlobally)
			{
				return;
			}
			EventDefinition eventDefinition2 = eventDefinitionProvider.FindEventDefinitionFromEventTypeId(rule.StopRule.Trigger.EventTypeId);
			if (eventDefinition2 == null)
			{
				logger.LogWarning("RegisterGlobalizedEvents: Unable to find stop event with id {EventTypeId}", rule.StopRule.Trigger.EventTypeId);
			}
			else
			{
				if (eventDefinition2.OccursGlobally)
				{
					return;
				}
				lock (globalizedTriggerEvents)
				{
					if (!globalizedTriggerEvents.TryGetValue(rule.StopRule.Trigger.EventTypeId, out var value))
					{
						value = new List<GlobalizedEventInfo>();
						globalizedTriggerEvents.Add(rule.StopRule.Trigger.EventTypeId, value);
					}
					GlobalizedEventInfo globalizedEventInfo = value.FirstOrDefault((GlobalizedEventInfo i) => i.RuleId == rule.ID);
					if (globalizedEventInfo == null)
					{
						globalizedEventInfo = new GlobalizedEventInfo(rule.ID, rule.Name, eventDefinition2.ID, eventDefinition2.Name);
						value.Add(globalizedEventInfo);
					}
					foreach (Guid source in rule.StopRule.Trigger.GetSources())
					{
						globalizedEventInfo.Sources.Add(source);
					}
					logger.LogInformation("Globalize event '{EventTypeId}' ({EvntName}) by rule '{RuleId}' ({RuleName}) for the following sources: {Sources}", globalizedEventInfo.EventTypeId, globalizedEventInfo.EventName, globalizedEventInfo.RuleId, globalizedEventInfo.RuleName, string.Join(", ", globalizedEventInfo.Sources));
				}
			}
		}
	}

	public IRule RemoveRule(Guid ruleId)
	{
		lock (ruleCollectionsLock)
		{
			if (!(rules[ruleId] is IRule rule))
			{
				logger.LogWarning("Attempting to remove a rule which has not been registered");
				return null;
			}
			UnregisterGlobalizedEvents(rule);
			UnregisterRuleBySubRuleSources(rule.StartRule);
			UnregisterRuleBySubRuleActionTargets(rule.StartRule);
			if (rule.StopRule != null)
			{
				if (rule.StopRule.RuleType != RuleType.Timer && rule.StopRule.RuleType != RuleType.TimeInterval)
				{
					UnregisterRuleBySubRuleSources(rule.StopRule);
				}
				UnregisterRuleBySubRuleActionTargets(rule.StopRule);
			}
			rules.Remove(ruleId);
			return rule;
		}
	}

	private void UnregisterRuleBySubRuleSources(ISubRule subRule)
	{
		lock (ruleCollectionsLock)
		{
			logger.LogInformation("Removing sub rule, Rule: {ParentRuleId} ({ParentRuleName}); EventTypeId: {EventTypeId}", subRule.ParentRule.ID, subRule.ParentRule.Name, subRule.Trigger.EventTypeId);
			foreach (Guid sourceTargetGroupId in subRule.Trigger.GetSourceTargetGroupIds())
			{
				if (ruleTriggerGroups[sourceTargetGroupId] is ArrayList arrayList && arrayList.Contains(subRule.ParentRule))
				{
					arrayList.Remove(subRule.ParentRule);
					if (arrayList.Count == 0)
					{
						ruleTriggerGroups.Remove(sourceTargetGroupId);
					}
				}
			}
			ArrayList arrayList2 = new ArrayList();
			foreach (DictionaryEntry ruleSource in ruleSources)
			{
				if (!(ruleSource.Value is Hashtable hashtable) || !(hashtable[subRule.Trigger.EventTypeId] is ArrayList arrayList3) || !arrayList3.Contains(subRule))
				{
					continue;
				}
				arrayList3.Remove(subRule);
				if (arrayList3.Count == 0)
				{
					logger.LogInformation("  No more rules registered on the event type, SourceId: {EntryKey}", ruleSource.Key);
					hashtable.Remove(subRule.Trigger.EventTypeId);
					if (hashtable.Keys.Count == 0)
					{
						logger.LogInformation("  No more event types registered on the source");
						arrayList2.Add(ruleSource.Key);
					}
				}
			}
			foreach (Guid item in arrayList2)
			{
				ruleSources.Remove(item);
			}
			Hashtable hashtable2 = anySources;
			if (hashtable2 != null && hashtable2[subRule.Trigger.EventTypeId] is ArrayList arrayList4 && arrayList4.Contains(subRule))
			{
				arrayList4.Remove(subRule);
				if (arrayList4.Count == 0)
				{
					logger.LogInformation("  No more rules registered on the event type, SourceId: {AnySource}", SourceGuid.AnySource);
					hashtable2.Remove(subRule.Trigger.EventTypeId);
				}
			}
		}
	}

	private void UnregisterRuleBySubRuleActionTargets(ISubRule subRule)
	{
		Hashtable hashtable = (subRule.IsStartRule ? rulesByStartActionTarget : rulesByStopActionTarget);
		foreach (IRuleAction action in subRule.Actions)
		{
			foreach (ActionTarget actionTarget in action.ActionTargets)
			{
				if (actionTarget.TargetType == ActionTargetType.Unspecified)
				{
					continue;
				}
				foreach (Guid targetItem in actionTarget.TargetItems)
				{
					if (hashtable[targetItem] is ArrayList arrayList)
					{
						if (arrayList.Contains(subRule.ParentRule))
						{
							arrayList.Remove(subRule.ParentRule);
						}
						if (arrayList.Count == 0)
						{
							hashtable.Remove(targetItem);
						}
					}
				}
				foreach (Guid targetGroup in actionTarget.TargetGroups)
				{
					if (hashtable[targetGroup] is ArrayList arrayList2)
					{
						if (arrayList2.Contains(subRule.ParentRule))
						{
							arrayList2.Remove(subRule.ParentRule);
						}
						if (arrayList2.Count == 0)
						{
							hashtable.Remove(targetGroup);
						}
					}
				}
			}
		}
	}

	private void UnregisterGlobalizedEvents(IRule rule)
	{
		if (rule.StopRule == null || rule.StopRule.RuleType != RuleType.Event)
		{
			return;
		}
		lock (globalizedTriggerEvents)
		{
			if (globalizedTriggerEvents.TryGetValue(rule.StopRule.Trigger.EventTypeId, out var value))
			{
				value.RemoveAll((GlobalizedEventInfo info) => info.RuleId == rule.ID);
				if (value.Count == 0)
				{
					globalizedTriggerEvents.Remove(rule.StopRule.Trigger.EventTypeId);
				}
			}
		}
	}

	public void ModifyRule(IRule rule)
	{
		lock (ruleCollectionsLock)
		{
			RemoveRule(rule.ID);
			AddRule(rule);
		}
	}

	public void AddUnregisterTriggerEvent(UnregisterTriggerEvent unregisterTriggerEvent)
	{
		if (isRunning)
		{
			logger.LogInformation("Unregister event received, EventTypeId: {EventTypeId}; SourceId: {SourceId}", unregisterTriggerEvent.EventTypeId, unregisterTriggerEvent.SourceId);
			AddEvent(unregisterTriggerEvent);
		}
	}

	public void AddTriggerEvent(TriggerEvent triggerEvent)
	{
		if (isRunning)
		{
			logger.LogDebug("Trigger Event received, Event: {EventTypeId} ({Name}); SourceId: {SourceId}", triggerEvent.EventTypeId, triggerEvent.Name, triggerEvent.SourceId);
			AddEvent(triggerEvent);
		}
	}

	public void AddRuleActionExecutionRequest(IRuleActionExecutionRequest ruleActionExecutionRequest)
	{
		if (isRunning)
		{
			logger.LogInformation("RuleActionExecutionRequest added, Event: {EventTypeId} ({Name}); RuleId: {RuleId}; RuleActionInstanceId: {RuleActionInstanceId}", ruleActionExecutionRequest.TriggerEvent.EventTypeId, ruleActionExecutionRequest.TriggerEvent.Name, ruleActionExecutionRequest.RuleId, ruleActionExecutionRequest.RuleActionInstanceId);
			EnqueueEvent(ruleActionExecutionRequest);
		}
	}

	public void AddTargetChangeEvent(TargetChangeEvent targetChangeEvent)
	{
		if (isRunning)
		{
			logger.LogInformation("TargetChangeEvent added, TargetId: {TargetId}; TargetType: {TargetType}; ActionType: {ActionType}", targetChangeEvent.TargetId, targetChangeEvent.TargetType, targetChangeEvent.ActionType);
			EnqueueEvent(targetChangeEvent);
		}
	}

	public void AddClearStateExternalEvent(Guid ruleId)
	{
		if (isRunning)
		{
			logger.LogInformation("RuleClearStateExternal added, RuleId: {RuleId}", ruleId);
			EnqueueEvent(new RuleClearStateExternal(ruleId));
		}
	}

	private void AddEvent(TriggerEventBase eventBase)
	{
		EnqueueEvent(eventBase);
	}

	private void RuleProcessorThreadFunction()
	{
		logger.LogInformation("RuleProcessor started");
		while (true)
		{
			try
			{
				if (!isRunning)
				{
					break;
				}
				object nextInput = GetNextInput();
				if (!isRunning)
				{
					break;
				}
				if (nextInput != null)
				{
					if (nextInput is TriggerEventBase)
					{
						ProcessTriggerEvent((TriggerEventBase)nextInput);
					}
					else if (nextInput is RuleActionExecutionRequest)
					{
						ProcessRuleActionExecutionRequest((RuleActionExecutionRequest)nextInput);
					}
					else if (nextInput is TargetChangeEvent)
					{
						ProcessTargetChangeEvent((TargetChangeEvent)nextInput);
					}
					else if (nextInput is RuleClearStateExternal)
					{
						ProcessRuleClearStateExternal((RuleClearStateExternal)nextInput);
					}
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				logger.LogError("An unhandled error occurred in the rule processor. Error: {Exception}; StackTrace: {StackTrace}", ex2.Message, ex2.StackTrace);
			}
		}
	}

	private void ProcessRuleClearStateExternal(RuleClearStateExternal ruleClearStateExternal)
	{
		logger.LogDebug("ProcessRuleClearStateExternal(), RuleId: {RuleId}", ruleClearStateExternal.RuleId);
		OnRuleClearStateExternal(new RuleEventArgs(ruleClearStateExternal.RuleId));
	}

	private void ProcessTargetChangeEvent(TargetChangeEvent targetChangeEvent)
	{
		logger.LogDebug("ProcessTargetChangeEvent(), TargetId: {TargetId}", targetChangeEvent.TargetId);
		if (targetChangeEvent.ActionType == TargetChangeActionTypes.Activated)
		{
			HandleRulesAffectedByTargetActivated(targetChangeEvent);
			return;
		}
		logger.LogError("An unsupported TargetChangeEvent was processed by the RuleProcessor. TargetChangeEvent: {ActionType}", targetChangeEvent.ActionType);
	}

	private void ProcessRuleActionExecutionRequest(RuleActionExecutionRequest ruleActionExecutionRequest)
	{
		logger.LogDebug("ProcessRuleActionExecutionRequest(), Event: {EventTypeId} ({Name})", ruleActionExecutionRequest.TriggerEvent.EventTypeId, ruleActionExecutionRequest.TriggerEvent.Name);
		ISubRule[] array = null;
		if (ruleActionExecutionRequest.TriggerEvent.EventTypeId == RuleEngine.RelativeTimeEventId)
		{
			if (!(rules[ruleActionExecutionRequest.RuleId] is Rule { StopRule: not null } rule))
			{
				return;
			}
			array = new ISubRule[1] { rule.StopRule };
		}
		else
		{
			array = GetSubRules(ruleActionExecutionRequest.TriggerEvent);
		}
		ISubRule[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			SubRule subRule = (SubRule)array2[i];
			if (subRule.ParentRule.ID != ruleActionExecutionRequest.RuleId)
			{
				continue;
			}
			foreach (IRuleAction action in subRule.Actions)
			{
				if (!(action.InstanceId == ruleActionExecutionRequest.RuleActionInstanceId))
				{
					continue;
				}
				if (subRule.RuleType == RuleType.Event)
				{
					ProcessTriggerEventRuleAction(subRule.ParentRule, action, ruleActionExecutionRequest.ExpandedTargets, ruleActionExecutionRequest.TriggerEvent);
				}
				else if (subRule.RuleType == RuleType.Timer)
				{
					try
					{
						ScheduleRuleAction(subRule.ParentRule, action, ruleActionExecutionRequest.ExpandedTargets, ruleActionExecutionRequest.TriggerEvent);
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (Exception ex2)
					{
						logger.LogWarning("Error scheduling action when processing rule. Error: {Exception}", ex2.Message);
					}
				}
			}
		}
	}

	private void ProcessTriggerEvent(TriggerEventBase triggerEventBase)
	{
		try
		{
			logger.LogDebug("ProcessTriggerEvent(), EventTypeId: {EventTypeId}", triggerEventBase.EventTypeId);
			OnEventProcessing(new EventProcessingEventArgs(triggerEventBase));
			if (triggerEventBase is TriggerEvent)
			{
				TriggerEvent triggerEvent = (TriggerEvent)triggerEventBase;
				if (!triggerEvent.IsGlobal)
				{
					bool flag = false;
					lock (globalizedTriggerEvents)
					{
						if (globalizedTriggerEvents.TryGetValue(triggerEventBase.EventTypeId, out var value))
						{
							flag = value.Any((GlobalizedEventInfo info) => info.Sources.Contains(triggerEvent.SourceId));
						}
					}
					if (flag)
					{
						OnTriggerEventAsGlobal(new EventProcessingEventArgs(triggerEventBase));
						triggerEvent.IsGlobal = true;
					}
				}
			}
			ISubRule[] subRules = GetSubRules(triggerEventBase);
			foreach (ISubRule subRule in subRules)
			{
				try
				{
					if (!subRule.ParentRule.Enabled)
					{
						continue;
					}
					if (subRule is TimeIntervalSubRule || subRule is RecurrentTimeSubRule)
					{
						if (!triggerEventBase.IsRemoteEvent && !subRule.IsStopRule)
						{
							OnRuleProcessing(new RuleProcessingEventArgs(subRule.ParentRule, triggerEventBase));
							if (subRule is TimeIntervalSubRule)
							{
								ProcessTimeIntervalRule(subRule.ParentRule, (TriggerEvent)triggerEventBase);
							}
							if (subRule is RecurrentTimeSubRule)
							{
								ProcessRecurrentTimeRule(subRule.ParentRule, (TriggerEvent)triggerEventBase);
							}
							OnRuleProcessed(new RuleProcessingEventArgs(subRule.ParentRule, triggerEventBase));
						}
					}
					else
					{
						OnRuleProcessing(new RuleProcessingEventArgs(subRule.ParentRule, triggerEventBase));
						if (subRule.IsStopRule)
						{
							ProcessStopRule(subRule.ParentRule, triggerEventBase);
						}
						else if (subRule.IsStartRule)
						{
							ProcessStartRule(subRule.ParentRule, triggerEventBase);
						}
						OnRuleProcessed(new RuleProcessingEventArgs(subRule.ParentRule, triggerEventBase));
					}
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					logger.LogWarning("Error processing rule. Error: {Exception}", ex2.Message);
					OnRuleProcessingError(new RuleProcessingErrorEventArgs(subRule.ParentRule, triggerEventBase, ex2));
				}
			}
		}
		finally
		{
			OnEventProcessed(new EventProcessingEventArgs(triggerEventBase));
		}
	}

	private object GetNextInput()
	{
		lock (inputQueue)
		{
			if (!isRunning)
			{
				return null;
			}
			if (inputQueue.Count > 0)
			{
				return inputQueue.Dequeue();
			}
			if (!Monitor.Wait(inputQueue))
			{
				return null;
			}
			if (inputQueue.Count == 0)
			{
				return null;
			}
			return inputQueue.Dequeue();
		}
	}

	ISubRule[] IRuleProcessor.GetSubRules(TriggerEventBase triggerEventBase)
	{
		return GetSubRules(triggerEventBase);
	}

	public ISubRule[] GetSubRules(TriggerEventBase triggerEventBase)
	{
		ISubRule[] array;
		lock (ruleCollectionsLock)
		{
			List<SubRule> list = new List<SubRule>();
			if (anySources[triggerEventBase.EventTypeId] is ArrayList source)
			{
				list.AddRange(source.OfType<SubRule>());
			}
			if (ruleSources[triggerEventBase.SourceId] is Hashtable hashtable && hashtable[triggerEventBase.EventTypeId] is ArrayList source2)
			{
				list.AddRange(source2.OfType<SubRule>());
			}
			if (list == null)
			{
				array = Array.Empty<SubRule>();
				array = array;
			}
			else
			{
				ISubRule[] array2 = list.ToArray();
				array = array2;
			}
		}
		return array;
	}

	private void ProcessStartRule(IRule rule, TriggerEventBase triggerEventBase)
	{
		if (!(triggerEventBase is TriggerEvent))
		{
			return;
		}
		TriggerEvent triggerEvent = (TriggerEvent)triggerEventBase;
		bool flag = ProcessEventSubRule((EventSubRule)rule.StartRule, triggerEvent);
		if (!(rule.StopRule != null && rule.StopRule is TimerSubRule && flag))
		{
			return;
		}
		TimerSubRule timerSubRule = (TimerSubRule)rule.StopRule;
		TimeSpan value = new TimeSpan(0, 0, 0, timerSubRule.TimerValue);
		TriggerEvent triggerEvent2 = triggerEvent.Clone();
		triggerEvent2.EventTypeId = RuleEngine.RelativeTimeEventId;
		triggerEvent2.IsRemoteEvent = false;
		triggerEvent2.Time = triggerEvent2.Time.Add(value);
		foreach (IRuleAction action in timerSubRule.Actions)
		{
			ActionTargetCollection expandedTargets;
			try
			{
				expandedTargets = BuildRuleActionExpandedTargets(rule.StopRule, action, triggerEvent, triggerEvent.MetaData);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				logger.LogWarning("Error building expanded target list for action when scheduling action. Error: {Exception}", ex2.Message);
				continue;
			}
			try
			{
				ScheduleRuleAction(timerSubRule.ParentRule, action, expandedTargets, triggerEvent2);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				logger.LogWarning("Error scheduling action when processing rule. Error: {Exception}", ex4.Message);
			}
		}
	}

	private void ProcessStopRule(IRule rule, TriggerEventBase triggerEventBase)
	{
		if (triggerEventBase is TriggerEvent)
		{
			TriggerEvent triggerEvent = (TriggerEvent)triggerEventBase;
			ProcessEventSubRule((EventSubRule)rule.StopRule, triggerEvent);
		}
		else if (triggerEventBase is UnregisterTriggerEvent)
		{
			UnregisterTriggerEvent unregisterTriggerEvent = (UnregisterTriggerEvent)triggerEventBase;
			ProcessUnregisterEvent(rule.StopRule, unregisterTriggerEvent);
		}
	}

	private void ProcessUnregisterEvent(ISubRule subRule, UnregisterTriggerEvent unregisterTriggerEvent)
	{
		foreach (IRuleAction action in subRule.Actions)
		{
			ActionTargetCollection expandedTargets;
			try
			{
				expandedTargets = BuildRuleActionExpandedTargets(subRule, action, unregisterTriggerEvent);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				logger.LogWarning("Error building expanded target list for action when processing rule. Error: {Exception}", ex2.Message);
				continue;
			}
			ProcessUnregisterEventSubRuleAction(action, expandedTargets, unregisterTriggerEvent);
		}
	}

	private void ProcessUnregisterEventSubRuleAction(IRuleAction action, ActionTargetCollection expandedTargets, UnregisterTriggerEvent unregisterTriggerEvent)
	{
		if (action.HasPositiveDelay)
		{
			TaskInfo[] allScheduledTasks = scheduler.GetAllScheduledTasks();
			foreach (TaskInfo taskInfo in allScheduledTasks)
			{
				if (taskInfo.Task is ScheduledRuleAction scheduledRuleAction && scheduledRuleAction.TriggerEvent.SourceId == unregisterTriggerEvent.SourceId)
				{
					scheduler.CancelTask(taskInfo.TaskId);
				}
			}
		}
		try
		{
			action.HandleUnregisterEvent(expandedTargets, unregisterTriggerEvent);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			logger.LogWarning("Error handling unregister event for action. Error: {Exception}", ex2.Message);
		}
	}

	private void ProcessTimeIntervalRule(IRule rule, TriggerEvent triggerEvent)
	{
		TimeIntervalSubRule timeIntervalSubRule = (TimeIntervalSubRule)rule.StartRule;
		if (timeIntervalSubRule.Condition.IsTrue(triggerEvent))
		{
			if (timeIntervalSubRule.IsIntervalStarted)
			{
				return;
			}
			timeIntervalSubRule.IsIntervalStarted = true;
			{
				foreach (IRuleAction action in rule.StartRule.Actions)
				{
					ActionTargetCollection expandedTargets;
					try
					{
						expandedTargets = BuildRuleActionExpandedTargets(rule.StartRule, action, triggerEvent);
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (Exception ex2)
					{
						logger.LogWarning("Error building expanded target list for action when processing rule. Error: {Exception}", ex2.Message);
						continue;
					}
					ProcessTriggerEventRuleAction(rule, action, expandedTargets, triggerEvent);
				}
				return;
			}
		}
		if (!timeIntervalSubRule.IsIntervalStarted)
		{
			return;
		}
		timeIntervalSubRule.IsIntervalStarted = false;
		if (rule.StopRule == null)
		{
			return;
		}
		foreach (IRuleAction action2 in rule.StopRule.Actions)
		{
			ActionTargetCollection expandedTargets2;
			try
			{
				expandedTargets2 = BuildRuleActionExpandedTargets(rule.StopRule, action2, triggerEvent);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				logger.LogWarning("Error building expanded target list for action when processing rule. Error: {Exception}", ex4.Message);
				continue;
			}
			ProcessTriggerEventRuleAction(rule, action2, expandedTargets2, triggerEvent);
		}
	}

	private void ProcessRecurrentTimeRule(IRule rule, TriggerEvent triggerEvent)
	{
		if (!((RecurrentTimeSubRule)rule.StartRule).Condition.IsTrue(triggerEvent))
		{
			return;
		}
		foreach (IRuleAction action in rule.StartRule.Actions)
		{
			ActionTargetCollection expandedTargets;
			try
			{
				expandedTargets = BuildRuleActionExpandedTargets(rule.StartRule, action, triggerEvent);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				logger.LogWarning("Error building expanded target list for action when processing rule. Error: {Exception}", ex2.Message);
				continue;
			}
			ProcessTriggerEventRuleAction(rule, action, expandedTargets, triggerEvent);
		}
	}

	private bool ProcessEventSubRule(EventSubRule subRule, TriggerEvent triggerEvent)
	{
		if (!triggerEvent.IsRemoteEvent && !subRule.Condition.IsTrue(triggerEvent))
		{
			return false;
		}
		foreach (IRuleAction action in subRule.Actions)
		{
			ActionTargetCollection expandedTargets;
			try
			{
				expandedTargets = BuildRuleActionExpandedTargets(subRule, action, triggerEvent, triggerEvent.MetaData);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				logger.LogWarning("Error building expanded target list for action when processing rule. Error: {Exception}", ex2.Message);
				continue;
			}
			ProcessTriggerEventRuleAction(subRule.ParentRule, action, expandedTargets, triggerEvent);
		}
		return true;
	}

	private void ProcessTriggerEventRuleAction(IRule rule, IRuleAction action, ActionTargetCollection expandedTargets, TriggerEvent triggerEvent)
	{
		if (action.HasPositiveDelay)
		{
			try
			{
				ScheduleRuleAction(rule, action, expandedTargets, triggerEvent);
				return;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				logger.LogWarning("Error scheduling action when processing rule. Error: {Exception}", ex2.Message);
				return;
			}
		}
		try
		{
			ExecuteRuleAction(rule, action, expandedTargets, triggerEvent);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex4)
		{
			logger.LogWarning("Error executing action when processing rule. Error: {Exception}", ex4.Message);
		}
	}

	private void GroupConfiguration_GroupMemberAdded(object sender, GroupMemberChangeEventArgs e)
	{
		logger.LogDebug("RuleProcessor.GroupConfiguration_GroupMemberAdded(). GroupId: {GroupId}; MemberId: {MemberId}", e.GroupId, e.MemberId);
		lock (ruleCollectionsLock)
		{
			HandleRulesWithTriggerSourceAffectedByAddedGroupMember(e);
			HandleRulesWithActionsAffectedByAddedGroupMember(e);
		}
	}

	private void HandleRulesWithTriggerSourceAffectedByAddedGroupMember(GroupMemberChangeEventArgs e)
	{
		lock (ruleCollectionsLock)
		{
			ArrayList arrayList = new ArrayList();
			IRule[] rulesByGroupId = GetRulesByGroupId(e.GroupId);
			arrayList.AddRange(rulesByGroupId);
			Guid[] array = groupConfiguration.FindAncestorGroupsByGroup(e.GroupId);
			foreach (Guid groupId in array)
			{
				rulesByGroupId = GetRulesByGroupId(groupId);
				IRule[] array2 = rulesByGroupId;
				for (int j = 0; j < array2.Length; j++)
				{
					Rule rule = (Rule)array2[j];
					if (!arrayList.Contains(rule))
					{
						arrayList.Add(rule);
					}
				}
			}
			foreach (Rule item in arrayList)
			{
				RegisterNewGroupMember(e, item);
			}
		}
	}

	private void RegisterNewGroupMember(GroupMemberChangeEventArgs e, Rule rule)
	{
		RegisterGlobalizedEvents(rule);
		RegisterNewGroupMemberBySource(e.MemberId, rule.StartRule);
		if (rule.StopRule != null && rule.StopRule.RuleType != RuleType.Timer && rule.StopRule.RuleType != RuleType.TimeInterval)
		{
			RegisterNewGroupMemberBySource(e.MemberId, rule.StopRule);
		}
	}

	private void RegisterNewGroupMemberBySource(Guid ruleSource, ISubRule subRule)
	{
		if (subRule.Trigger.GetSources().Contains(ruleSource))
		{
			Hashtable hashtable = ruleSources[ruleSource] as Hashtable;
			if (hashtable == null)
			{
				hashtable = new Hashtable();
				ruleSources[ruleSource] = hashtable;
			}
			AddSubRuleToEventType(hashtable, subRule, ruleSource);
		}
	}

	private void HandleRulesWithActionsAffectedByAddedGroupMember(GroupMemberChangeEventArgs e)
	{
		lock (ruleCollectionsLock)
		{
			ArrayList arrayList = ((!(rulesByStartActionTarget[e.GroupId] is ArrayList arrayList2)) ? new ArrayList() : ((ArrayList)arrayList2.Clone()));
			groupConfiguration.FindAncestorGroupsByGroup(e.GroupId);
			Guid[] array = groupConfiguration.FindAncestorGroupsByGroup(e.GroupId);
			foreach (Guid guid in array)
			{
				if (!(rulesByStartActionTarget[guid] is ArrayList arrayList3))
				{
					continue;
				}
				foreach (Rule item in arrayList3)
				{
					if (!arrayList.Contains(item))
					{
						arrayList.Add(item);
					}
				}
			}
			foreach (Rule item2 in arrayList)
			{
				if (!item2.Enabled || !(item2.StartRule is TimeIntervalSubRule) || !((TimeIntervalSubRule)item2.StartRule).IsIntervalStarted)
				{
					continue;
				}
				foreach (IRuleAction action in item2.StartRule.Actions)
				{
					foreach (ActionTarget actionTarget in action.ActionTargets)
					{
						if (actionTarget.TargetType != ActionTargetType.Unspecified)
						{
							Guid[] array2 = CreateExpandedTargetList(actionTarget, allowDuplicates: true);
							if (Array.IndexOf(array2, e.MemberId) == Array.LastIndexOf(array2, e.MemberId))
							{
								ActionTargetCollection actionTargetCollection = new ActionTargetCollection();
								actionTargetCollection.Add(new ActionTarget(actionTarget.TargetType));
								actionTargetCollection[0].TargetItems.Add(e.MemberId);
								TriggerEvent triggerEvent = new TriggerEvent(RuleEngine.Id, RuleEngine.GroupMemberAddedEventId);
								triggerEvent.IsGlobal = true;
								ExecuteRuleAction(item2, action, actionTargetCollection, triggerEvent);
							}
						}
					}
				}
			}
		}
	}

	private void GroupConfiguration_GroupMemberDeleted(object sender, GroupMemberChangeEventArgs e)
	{
		logger.LogDebug("RuleProcessor.GroupConfiguration_GroupMemberDeleted(). GroupId: {GroupId}; MemberId: {MemberId}", e.GroupId, e.MemberId);
		lock (ruleCollectionsLock)
		{
			HandleRulesWithTriggerSourceAffectedByDeletedGroupMember(e);
			HandleRulesWithActionsAffectedByDeletedGroupMember(e);
		}
	}

	private void HandleRulesWithTriggerSourceAffectedByDeletedGroupMember(GroupMemberChangeEventArgs e)
	{
		lock (ruleCollectionsLock)
		{
			ArrayList arrayList = new ArrayList();
			IRule[] rulesByGroupId = GetRulesByGroupId(e.GroupId);
			arrayList.AddRange(rulesByGroupId);
			Guid[] array = groupConfiguration.FindAncestorGroupsByGroup(e.GroupId);
			foreach (Guid groupId in array)
			{
				rulesByGroupId = GetRulesByGroupId(groupId);
				IRule[] array2 = rulesByGroupId;
				for (int j = 0; j < array2.Length; j++)
				{
					Rule rule = (Rule)array2[j];
					if (!arrayList.Contains(rule))
					{
						arrayList.Add(rule);
					}
				}
			}
			foreach (Rule item in arrayList)
			{
				UnregisterGroupMember(e, item);
				if (!item.Enabled || item.StopRule == null)
				{
					continue;
				}
				List<Guid> sources = item.StopRule.Trigger.GetSources();
				if (sources.Contains(e.MemberId))
				{
					continue;
				}
				foreach (IRuleAction action in item.StopRule.Actions)
				{
					foreach (ActionTarget actionTarget in action.ActionTargets)
					{
						if (sources.Count == 0)
						{
							TriggerEvent triggerEvent = new TriggerEvent(e.MemberId, RuleEngine.GroupMemberDeletedEventId);
							ActionTargetCollection expandedTargets;
							try
							{
								expandedTargets = BuildRuleActionExpandedTargets(item.StopRule, action, triggerEvent);
							}
							catch (OutOfMemoryException)
							{
								throw;
							}
							catch (Exception ex2)
							{
								logger.LogWarning("Error building expanded target list for action when processing action. Error: {Exception}", ex2.Message);
								continue;
							}
							TriggerEvent triggerEvent2 = new TriggerEvent(RuleEngine.Id, RuleEngine.GroupMemberDeletedEventId);
							triggerEvent2.IsGlobal = true;
							ExecuteRuleAction(item, action, expandedTargets, triggerEvent2);
						}
						else if (actionTarget.TargetType != ActionTargetType.Unspecified && actionTarget.TargetItems.Contains(SpecialActionTargets.TriggeringDevice))
						{
							ActionTargetCollection actionTargetCollection = new ActionTargetCollection();
							actionTargetCollection.Add(new ActionTarget(actionTarget.TargetType));
							actionTargetCollection[0].TargetItems.Add(e.MemberId);
							TriggerEvent triggerEvent3 = new TriggerEvent(RuleEngine.Id, RuleEngine.GroupMemberDeletedEventId);
							triggerEvent3.IsGlobal = true;
							ExecuteRuleAction(item, action, actionTargetCollection, triggerEvent3);
						}
					}
				}
			}
		}
	}

	private void UnregisterGroupMember(GroupMemberChangeEventArgs e, Rule rule)
	{
		UnregisterGlobalizedEvents(rule);
		RegisterGlobalizedEvents(rule);
		UnregisterGroupMemberBySource(e.MemberId, rule.StartRule);
		if (rule.StopRule != null && rule.StopRule.RuleType != RuleType.Timer && rule.StopRule.RuleType != RuleType.TimeInterval)
		{
			UnregisterGroupMemberBySource(e.MemberId, rule.StopRule);
		}
	}

	private void UnregisterGroupMemberBySource(Guid ruleSource, ISubRule subRule)
	{
		if (ruleSources[ruleSource] is Hashtable eventTypeIds)
		{
			RemoveSubRuleFromEventType(eventTypeIds, subRule, ruleSource);
		}
	}

	private void RemoveSubRuleFromEventType(Hashtable eventTypeIds, ISubRule subRule, Guid ruleSource)
	{
		if (!(eventTypeIds[subRule.Trigger.EventTypeId] is ArrayList arrayList) || !arrayList.Contains(subRule))
		{
			return;
		}
		arrayList.Remove(subRule);
		if (arrayList.Count == 0)
		{
			logger.LogInformation("  No more rules registered on the event type, SourceId: {RuleSource}", ruleSource);
			eventTypeIds.Remove(subRule.Trigger.EventTypeId);
			if (eventTypeIds.Keys.Count == 0)
			{
				logger.LogInformation("  No more event types registered on the source");
				ruleSources.Remove(ruleSource);
			}
		}
	}

	private void HandleRulesAffectedByTargetActivated(TargetChangeEvent targetChangeEvent)
	{
		if (targetChangeEvent.TargetType != TargetChangeTargetTypes.Device)
		{
			return;
		}
		lock (ruleCollectionsLock)
		{
			ArrayList arrayList = ((!(rulesByStartActionTarget[targetChangeEvent.TargetId] is ArrayList arrayList2)) ? new ArrayList() : ((ArrayList)arrayList2.Clone()));
			Guid[] array = groupConfiguration.FindAncestorGroupsByMember(targetChangeEvent.TargetId);
			foreach (Guid guid in array)
			{
				if (!(rulesByStartActionTarget[guid] is ArrayList arrayList3))
				{
					continue;
				}
				foreach (Rule item in arrayList3)
				{
					if (!arrayList.Contains(item))
					{
						arrayList.Add(item);
					}
				}
			}
			foreach (Rule item2 in arrayList)
			{
				if (!item2.Enabled || !(item2.StartRule is TimeIntervalSubRule) || !((TimeIntervalSubRule)item2.StartRule).IsIntervalStarted)
				{
					continue;
				}
				foreach (IRuleAction action in item2.StartRule.Actions)
				{
					foreach (ActionTarget actionTarget in action.ActionTargets)
					{
						if (actionTarget.TargetType != ActionTargetType.Unspecified && Array.IndexOf(CreateExpandedTargetList(actionTarget, allowDuplicates: true), targetChangeEvent.TargetId) != -1)
						{
							ActionTargetCollection actionTargetCollection = new ActionTargetCollection();
							actionTargetCollection.Add(new ActionTarget(actionTarget.TargetType));
							actionTargetCollection[0].TargetItems.Add(targetChangeEvent.TargetId);
							TriggerEvent te = new TriggerEvent(RuleEngine.Id, RuleEngine.TargetItemActivatedEventId);
							ExecuteRuleAction(item2, action, actionTargetCollection, te);
						}
					}
				}
			}
		}
	}

	private void ScheduleRuleAction(IRule rule, IRuleAction action, ActionTargetCollection expandedTargets, TriggerEvent te)
	{
		logger.LogInformation("RuleProcessor starting to schedule rule action. Rule: {RuleId} ({RuleName}); ActionName: {ActionFullName}", rule.ID, rule.Name, action.GetType().FullName);
		OnRuleActionProcessing(new RuleActionProcessingEventArgs(rule, action, expandedTargets, te));
		action.Schedule(scheduler, expandedTargets, te);
		OnRuleActionProcessed(new RuleActionProcessingEventArgs(rule, action, expandedTargets, te));
		logger.LogDebug("RuleProcessor finished scheduling rule action. Rule: {RuleId} ({RuleName}); ActionName: {ActionFullName}", rule.ID, rule.Name, action.GetType().FullName);
	}

	private void ExecuteRuleAction(IRule rule, IRuleAction action, ActionTargetCollection expandedTargets, TriggerEvent te)
	{
		logger.LogInformation("RuleProcessor starting to execute rule action. Rule: {RuleId} ({RuleName}); ActionName: {ActionFullName}", rule.ID, rule.Name, action.GetType().FullName);
		OnRuleActionProcessing(new RuleActionProcessingEventArgs(rule, action, expandedTargets, te));
		action.Execute(expandedTargets, te);
		OnRuleActionProcessed(new RuleActionProcessingEventArgs(rule, action, expandedTargets, te));
		logger.LogDebug("RuleProcessor finished executing rule action. RuleId: {RuleId} ({RuleName}); ActionName: {ActionFullName}", rule.ID, rule.Name, action.GetType().FullName);
	}

	private void HandleRulesWithActionsAffectedByDeletedGroupMember(GroupMemberChangeEventArgs e)
	{
		lock (ruleCollectionsLock)
		{
			ArrayList arrayList = ((!(rulesByStopActionTarget[e.GroupId] is ArrayList arrayList2)) ? new ArrayList() : ((ArrayList)arrayList2.Clone()));
			groupConfiguration.FindAncestorGroupsByGroup(e.GroupId);
			Guid[] array = groupConfiguration.FindAncestorGroupsByGroup(e.GroupId);
			foreach (Guid guid in array)
			{
				if (!(rulesByStopActionTarget[guid] is ArrayList arrayList3))
				{
					continue;
				}
				foreach (Rule item in arrayList3)
				{
					if (!arrayList.Contains(item))
					{
						arrayList.Add(item);
					}
				}
			}
			foreach (Rule item2 in arrayList)
			{
				if (!item2.Enabled || (item2.StartRule is TimeIntervalSubRule && !((TimeIntervalSubRule)item2.StartRule).IsIntervalStarted))
				{
					continue;
				}
				foreach (IRuleAction action in item2.StopRule.Actions)
				{
					foreach (ActionTarget actionTarget in action.ActionTargets)
					{
						if (actionTarget.TargetType != ActionTargetType.Unspecified && Array.IndexOf(CreateExpandedTargetList(actionTarget, allowDuplicates: false), e.MemberId) < 0)
						{
							ActionTargetCollection actionTargetCollection = new ActionTargetCollection();
							actionTargetCollection.Add(new ActionTarget(actionTarget.TargetType));
							actionTargetCollection[0].TargetItems.Add(e.MemberId);
							TriggerEvent triggerEvent = new TriggerEvent(RuleEngine.Id, RuleEngine.GroupMemberDeletedEventId);
							triggerEvent.IsGlobal = true;
							ExecuteRuleAction(item2, action, actionTargetCollection, triggerEvent);
						}
					}
				}
			}
		}
	}

	private ActionTargetCollection BuildRuleActionExpandedTargets(ISubRule subRule, IRuleAction action, TriggerEventBase triggerEvent)
	{
		return BuildRuleActionExpandedTargets(subRule, action, triggerEvent, null);
	}

	private ActionTargetCollection BuildRuleActionExpandedTargets(ISubRule subRule, IRuleAction action, TriggerEventBase triggerEvent, EventMetaDataCollection metaData)
	{
		ActionTargetCollection actionTargetCollection = new ActionTargetCollection();
		foreach (ActionTarget actionTarget2 in action.ActionTargets)
		{
			ActionTarget actionTarget = new ActionTarget(actionTarget2.TargetType);
			actionTargetCollection.Add(actionTarget);
			switch (actionTarget2.TargetType)
			{
			case ActionTargetType.Device:
			case ActionTargetType.MIP:
			{
				actionTarget.TargetItems.AddRange(CreateExpandedTargetList(actionTarget2, allowDuplicates: false));
				int index;
				if ((index = actionTarget.TargetItems.IndexOf(SpecialActionTargets.TriggeringDevice)) >= 0)
				{
					actionTarget.TargetItems[index] = triggerEvent.SourceId;
				}
				if ((index = actionTarget.TargetItems.IndexOf(SpecialActionTargets.DevicesFromMetaData)) >= 0)
				{
					Guid[] array = CreateTargetDeviceListFromMetaData(metaData);
					if (array.Length != 0)
					{
						actionTarget.TargetItems.RemoveAt(index);
						actionTarget.TargetItems.AddRange(array);
					}
					else if (subRule.IsStopRule)
					{
						actionTarget.TargetItems[index] = SpecialActionTargets.AllDevicesStartedByRule;
					}
				}
				break;
			}
			default:
				throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "Unexpected action target type specified for action. TargetType: '{0}'", actionTarget2.TargetType.ToString()));
			case ActionTargetType.Unspecified:
				break;
			}
		}
		return actionTargetCollection;
	}

	private Guid[] CreateExpandedTargetList(ActionTarget actionTarget, bool allowDuplicates)
	{
		ICollection<Guid> collection = ((!allowDuplicates) ? ((ICollection<Guid>)new HashSet<Guid>()) : ((ICollection<Guid>)new List<Guid>()));
		if (actionTarget.TargetType == ActionTargetType.Unspecified)
		{
			return collection.ToArray();
		}
		HashSet<Guid> hashSet = new HashSet<Guid>();
		foreach (Guid targetGroup in actionTarget.TargetGroups)
		{
			if (hashSet.Add(targetGroup))
			{
				Guid[] array = groupConfiguration.FindAllDescendantGroupsByGroup(targetGroup);
				foreach (Guid item in array)
				{
					hashSet.Add(item);
				}
			}
		}
		foreach (Guid item3 in hashSet)
		{
			Guid[] array = groupConfiguration.FindChildMembersByGroup(item3);
			foreach (Guid item2 in array)
			{
				collection.Add(item2);
			}
		}
		foreach (Guid targetItem in actionTarget.TargetItems)
		{
			collection.Add(targetItem);
		}
		return collection.ToArray();
	}

	private Guid[] CreateTargetDeviceListFromMetaData(EventMetaDataCollection metaData)
	{
		List<Guid> list = new List<Guid>();
		if (metaData == null)
		{
			return null;
		}
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.IgnoreWhitespace = true;
		xmlReaderSettings.IgnoreComments = true;
		string text = string.Empty;
		try
		{
			if (metaData.TryGetValue("TargetDevices", out var value))
			{
				text = value.GetValueAsString();
				if (!string.IsNullOrEmpty(text))
				{
					logger.LogInformation("CreateTargetDeviceListFromMetaData(), metadata: {TargetDeviceXml}", text);
					using XmlReader reader = XmlReader.Create(new StringReader(text), xmlReaderSettings);
					ActionTarget actionTarget = new ActionTarget(ActionTargetType.Device);
					list = new List<Guid>();
					XPathNavigator xPathNavigator = new XPathDocument(reader).CreateNavigator();
					foreach (XPathNavigator item in xPathNavigator.Select("//targetDevices/deviceIds/deviceId"))
					{
						if (!string.IsNullOrEmpty(item.Value))
						{
							logger.LogInformation("CreateTargetDeviceListFromMetaData(), device id from metadata: {DeviceNavValue}", item.Value);
							actionTarget.TargetItems.Add(new Guid(item.Value));
						}
						else
						{
							logger.LogInformation("CreateTargetDeviceListFromMetaData(), null or empty device id from metadata received");
						}
					}
					foreach (XPathNavigator item2 in xPathNavigator.Select("//targetDevices/deviceGroupIds/deviceGroupId"))
					{
						if (!string.IsNullOrEmpty(item2.Value))
						{
							logger.LogInformation("CreateTargetDeviceListFromMetaData(), device group id from metadata: {DeviceGroupNav}", item2.Value);
							actionTarget.TargetGroups.Add(new Guid(item2.Value));
						}
						else
						{
							logger.LogInformation("CreateTargetDeviceListFromMetaData(), null or empty device group id from metadata received");
						}
					}
					list.AddRange(CreateExpandedTargetList(actionTarget, allowDuplicates: false));
				}
				else
				{
					logger.LogInformation("CreateTargetDeviceListFromMetaData(), null or empty metadata received");
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			logger.LogError("CreateTargetDeviceListFromMetaData(), parsing of TargetDevices from metadata failed:\n{Exception}\nMetadata: {TargetDevicesXml}", ex2.Message, text);
		}
		return list.ToArray();
	}

	public void Dispose()
	{
		((IDisposable)scheduler).Dispose();
	}
}
