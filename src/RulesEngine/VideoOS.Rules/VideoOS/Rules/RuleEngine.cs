using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Management.Common;
using VideoOS.Rules.Interfaces;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Rules;

public sealed class RuleEngine : IRuleEngine, IRuleActionDataProvider, IEventReceiver, IEventDefinitionProvider, IDisposable
{
	public static readonly Guid TargetItemActivatedEventId = new Guid("B0109E36-5CED-4E3F-B637-4E645A823DBF");

	public static readonly Guid TargetItemDeactivatedEventId = new Guid("B02CE8FA-21DC-469E-8C07-BD9616289694");

	public static readonly Guid GroupMemberAddedEventId = new Guid("A01CD5DA-EF57-476B-814B-344F776DA600");

	public static readonly Guid GroupMemberDeletedEventId = new Guid("A02E59BD-45D3-4DC7-901E-8B57C4DA31C9");

	public static readonly Guid PeriodicTimeEventId = SourceGuid.PeriodicTimerEvent;

	public static readonly Guid RelativeTimeEventId = SourceGuid.RelativeTimerEvent;

	public static readonly Guid Id = new Guid("A6EAFB52-F591-451d-B02E-522D3F875811");

	private static ILogger logger = Telemetry.GetILogger("RuleSystem");

	private IRuleProcessor ruleProcessor;

	private GroupConfiguration groupConfiguration;

	private TimeProfileConfiguration timeProfileConfiguration;

	private PeriodicTimeEventGenerator periodicTimeEventGenerator;

	private Hashtable actionFactoryMap;

	private string actionPluginsPath;

	private Hashtable eventDefinitions;

	private ITimeProvider timeProvider;

	public ILogger Logger => logger;

	public IGroupConfiguration GroupConfiguration
	{
		[DebuggerStepThrough]
		get
		{
			return groupConfiguration;
		}
	}

	public ITimeProfileConfiguration TimeProfileConfiguration
	{
		[DebuggerStepThrough]
		get
		{
			return timeProfileConfiguration;
		}
	}

	public string ActionPluginsPath
	{
		[DebuggerStepThrough]
		get
		{
			return actionPluginsPath;
		}
		[DebuggerStepThrough]
		set
		{
			actionPluginsPath = value;
		}
	}

	public bool IsFailover { get; private set; }

	public IRuleVariableManager RuleVariableManager { get; private set; }

	IGroupConfiguration IRuleEngine.GroupConfiguration => groupConfiguration;

	IRuleEngine IRuleActionDataProvider.RuleEngine
	{
		[DebuggerStepThrough]
		get
		{
			return this;
		}
	}

	private event EventHandler<TriggerEventEventArgs> _triggerEventReceived;

	public event EventHandler<TriggerEventEventArgs> TriggerEventReceived
	{
		add
		{
			_triggerEventReceived += value;
		}
		remove
		{
			_triggerEventReceived -= value;
		}
	}

	public event EventHandler<TriggerEventEventArgs> TriggerEventProcessing;

	public event EventHandler<TriggerEventEventArgs> TriggerEventAsGlobal;

	public event EventHandler<TriggerEventEventArgs> TriggerEventProcessed;

	public event EventHandler<UnregisterTriggerEventEventArgs> UnregisterEventReceived;

	public event EventHandler<UnregisterTriggerEventEventArgs> UnregisterEventProcessing;

	public event EventHandler<UnregisterTriggerEventEventArgs> UnregisterEventProcessed;

	public event EventHandler<RuleProcessingEventArgs> RuleProcessing;

	public event EventHandler<RuleProcessingEventArgs> RuleProcessed;

	public event EventHandler<RuleActionProcessingEventArgs> RuleActionProcessing;

	public event EventHandler<RuleActionProcessingEventArgs> RuleActionProcessed;

	public event EventHandler<RuleEventArgs> RuleClearStateExternal;

	public void OnTriggerEventReceived(TriggerEvent triggerEvent)
	{
		if (this._triggerEventReceived != null)
		{
			this._triggerEventReceived(this, new TriggerEventEventArgs(triggerEvent));
		}
	}

	public void OnTriggerEventProcessing(TriggerEvent triggerEvent)
	{
		if (this.TriggerEventProcessing != null)
		{
			this.TriggerEventProcessing(this, new TriggerEventEventArgs(triggerEvent));
		}
	}

	public void OnTriggerEventAsGlobal(TriggerEvent triggerEvent)
	{
		if (this.TriggerEventAsGlobal != null)
		{
			this.TriggerEventAsGlobal(this, new TriggerEventEventArgs(triggerEvent));
		}
	}

	public void OnTriggerEventProcessed(TriggerEvent triggerEvent)
	{
		if (this.TriggerEventProcessed != null)
		{
			this.TriggerEventProcessed(this, new TriggerEventEventArgs(triggerEvent));
		}
	}

	public void OnUnregisterEventReceived(UnregisterTriggerEvent unregisterTriggerEvent)
	{
		if (this.UnregisterEventReceived != null)
		{
			this.UnregisterEventReceived(this, new UnregisterTriggerEventEventArgs(unregisterTriggerEvent));
		}
	}

	public void OnUnregisterEventProcessing(UnregisterTriggerEvent unregisterTriggerEvent)
	{
		if (this.UnregisterEventProcessing != null)
		{
			this.UnregisterEventProcessing(this, new UnregisterTriggerEventEventArgs(unregisterTriggerEvent));
		}
	}

	public void OnUnregisterEventProcessed(UnregisterTriggerEvent unregisterTriggerEvent)
	{
		if (this.UnregisterEventProcessed != null)
		{
			this.UnregisterEventProcessed(this, new UnregisterTriggerEventEventArgs(unregisterTriggerEvent));
		}
	}

	public RuleEngine()
		: this(isFailover: false)
	{
	}

	public RuleEngine(bool isFailover)
		: this(isFailover, null, highSpeedInterval: false)
	{
		ruleProcessor = new RuleProcessor(logger, this, groupConfiguration);
		SetupRuleProcessorEventHandlers();
	}

	public RuleEngine(bool isFailover, Hashtable actionFactoryMap, IRuleProcessor ruleProcessor, ITimeProvider timeProvider, bool highSpeedtimeInterval)
		: this(isFailover, actionFactoryMap, highSpeedtimeInterval)
	{
		this.ruleProcessor = ruleProcessor;
		SetupRuleProcessorEventHandlers();
		this.timeProvider = timeProvider;
	}

	private RuleEngine(bool isFailover, Hashtable actionFactoryMap, bool highSpeedInterval)
	{
		IsFailover = isFailover;
		RuleVariableManager = new RuleVariableManager();
		groupConfiguration = new GroupConfiguration();
		timeProfileConfiguration = new TimeProfileConfiguration();
		this.actionFactoryMap = actionFactoryMap ?? new Hashtable();
		timeProvider = timeProvider ?? new TimeProvider();
		eventDefinitions = new Hashtable();
		periodicTimeEventGenerator = new PeriodicTimeEventGenerator(1, (!highSpeedInterval) ? PeriodicTimeGranularity.Minutes : PeriodicTimeGranularity.Seconds);
		periodicTimeEventGenerator.Timer += PeriodicTimeEventGenerator_Timer;
		RegisterActionFactory(new SetVariableRuleActionFactory(this));
		RegisterActionFactory(new ResetVariableRuleActionFactory(this));
	}

	private void SetupRuleProcessorEventHandlers()
	{
		ruleProcessor.EventProcessing += ruleProcessor_EventProcessing;
		ruleProcessor.EventProcessed += ruleProcessor_EventProcessed;
		ruleProcessor.TriggerEventAsGlobal += ruleProcessor_TriggerEventAsGlobal;
		ruleProcessor.RuleProcessing += ruleProcessor_RuleProcessing;
		ruleProcessor.RuleProcessed += ruleProcessor_RuleProcessed;
		ruleProcessor.RuleActionProcessing += ruleProcessor_RuleActionProcessing;
		ruleProcessor.RuleActionProcessed += ruleProcessor_RuleActionProcessed;
		ruleProcessor.RuleClearStateExternal += ruleProcessor_RuleClearStateExternal;
	}

	public void Start()
	{
		ruleProcessor.Start();
		periodicTimeEventGenerator.Start();
		periodicTimeEventGenerator.GenerateEventNow();
	}

	public void Stop()
	{
		periodicTimeEventGenerator.Stop();
		ruleProcessor.Stop();
	}

	public void StartEventQueue()
	{
		ruleProcessor.EnableEventQueueing();
	}

	public void AddTriggerEvent(TriggerEvent triggerEvent)
	{
		if (triggerEvent == null)
		{
			throw new ArgumentNullException("triggerEvent");
		}
		triggerEvent.Definition = GetEventTypeDefinition(triggerEvent.EventTypeId);
		OnTriggerEventReceived(triggerEvent);
		ruleProcessor.AddTriggerEvent(triggerEvent);
	}

	public void AddUnregisterTriggerEvent(UnregisterTriggerEvent unregisterTriggerEvent)
	{
		OnUnregisterEventReceived(unregisterTriggerEvent);
		ruleProcessor.AddUnregisterTriggerEvent(unregisterTriggerEvent);
	}

	public void AddRuleActionExecutionRequest(IRuleActionExecutionRequest ruleActionExecutionRequest)
	{
		ruleProcessor.AddRuleActionExecutionRequest(ruleActionExecutionRequest);
	}

	public void SetConfiguration(IRuleConfiguration configurationDataSet)
	{
		if (configurationDataSet == null)
		{
			throw new ArgumentNullException("configurationDataSet");
		}
		lock (eventDefinitions.SyncRoot)
		{
			foreach (var eventTypesChange in configurationDataSet.EventTypesChanges)
			{
				RegisterEventTypeDefinition(eventTypesChange.xmlDefinition);
			}
		}
		GroupConfigurationHelper groupConfigurationHelper = new GroupConfigurationHelper();
		foreach (var deviceGroupChange in configurationDataSet.DeviceGroupChanges)
		{
			groupConfigurationHelper.GroupAdd(deviceGroupChange.deviceGroupId, deviceGroupChange.parentId);
		}
		foreach (var deviceGroupMemberChange in configurationDataSet.DeviceGroupMemberChanges)
		{
			groupConfigurationHelper.GroupMemberAdd(deviceGroupMemberChange.deviceGroupId, deviceGroupMemberChange.deviceId);
		}
		foreach (var serverGroupChange in configurationDataSet.ServerGroupChanges)
		{
			groupConfigurationHelper.GroupAdd(serverGroupChange.serverGroupId, serverGroupChange.parentId);
		}
		foreach (var serverGroupMembershipChange in configurationDataSet.ServerGroupMembershipChanges)
		{
			groupConfigurationHelper.GroupMemberAdd(serverGroupMembershipChange.serverGroupId, serverGroupMembershipChange.serverId);
		}
		groupConfigurationHelper.GroupAdd(RecorderGroupings.AllRecordingServers, Guid.Empty);
		foreach (var recorderChange in configurationDataSet.RecorderChanges)
		{
			groupConfigurationHelper.GroupMemberAdd(RecorderGroupings.AllRecordingServers, recorderChange.recorderId);
		}
		if (groupConfigurationHelper.HasChanges)
		{
			groupConfiguration.SetOrUpdateConfiguration(groupConfigurationHelper, firstUpdate: true);
		}
		timeProfileConfiguration.SetConfiguration(configurationDataSet.TimeProfileChanges);
		RuleVariableManager.SetConfiguration(configurationDataSet);
		foreach (var ruleChange in configurationDataSet.RuleChanges)
		{
			try
			{
				Rule rule = BuildRule(ruleChange);
				ruleProcessor.AddRule(rule);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				Logger.LogError("Error building rule, Rule: {IdRule} ({Name}); exception: {Exception}", ruleChange.ruleId, ruleChange.name, ex2);
			}
		}
	}

	public EventDefinition GetEventTypeDefinition(Guid eventId)
	{
		try
		{
			lock (eventDefinitions.SyncRoot)
			{
				return eventDefinitions[eventId] as EventDefinition;
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			return null;
		}
	}

	public bool IsUsedInRule(TriggerEventBase triggerEvent)
	{
		ISubRule[] subRules = ruleProcessor.GetSubRules(triggerEvent);
		for (int i = 0; i < subRules.Length; i++)
		{
			if (subRules[i].ParentRule.Enabled)
			{
				return true;
			}
		}
		return false;
	}

	private EventDefinition RegisterEventTypeDefinition(string eventDefinitionXml)
	{
		try
		{
			EventDefinition eventDefinition = new EventDefinition(eventDefinitionXml);
			eventDefinitions[eventDefinition.ID] = eventDefinition;
			return eventDefinition;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Logger.LogError("Failed to register event type definition in RuleEngine. Error: {Exception}", ex2);
			return null;
		}
	}

	private void UnregisterEventTypeDefinition(Guid eventId)
	{
		try
		{
			eventDefinitions.Remove(eventId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Logger.LogError("Failed to unregister event type definition in RuleEngine. Error: {Exception}", ex2);
		}
	}

	public void ConfigureRules(IRuleConfiguration ruleConfiguration)
	{
		if (ruleConfiguration == null)
		{
			throw new ArgumentNullException("ruleConfiguration");
		}
		if (ruleConfiguration.InitialConfiguration)
		{
			SetConfiguration(ruleConfiguration);
			return;
		}
		bool flag = false;
		ConfigureRuleEvents(ruleConfiguration);
		ConfigureRuleGroups(ruleConfiguration);
		try
		{
			RuleVariableManager.UpdateConfiguration(ruleConfiguration);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Logger.LogError("RuleEngine: Unable to update rule variable configuration. Error: {Exception}", ex2);
		}
		foreach (var ruleChange in ruleConfiguration.RuleChanges)
		{
			try
			{
				switch (ruleChange.modification)
				{
				case RuleModification.Add:
				{
					Rule rule2 = BuildRule(ruleChange);
					ruleProcessor.AddRule(rule2);
					flag |= rule2.StartRule.RuleType == RuleType.TimeInterval || rule2.StartRule.RuleType == RuleType.Timer;
					if (rule2.StopRule != null)
					{
						flag |= rule2.StopRule.RuleType == RuleType.TimeInterval || rule2.StopRule.RuleType == RuleType.Timer;
					}
					break;
				}
				case RuleModification.Update:
				{
					Rule rule = BuildRule(ruleChange);
					ruleProcessor.ModifyRule(rule);
					flag |= rule.StartRule.RuleType == RuleType.TimeInterval || rule.StartRule.RuleType == RuleType.Timer;
					if (rule.StopRule != null)
					{
						flag |= rule.StopRule.RuleType == RuleType.TimeInterval || rule.StopRule.RuleType == RuleType.Timer;
					}
					if (ruleChange.doClearEvent)
					{
						ruleProcessor.AddClearStateExternalEvent(rule.ID);
					}
					break;
				}
				case RuleModification.Remove:
					ruleProcessor.RemoveRule(ruleChange.ruleId);
					ruleProcessor.AddClearStateExternalEvent(ruleChange.ruleId);
					break;
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				Logger.LogError("ConfigureRules: Error updating rule, Rule: {IdRule} ({Mame}), exception: {Exception}", ruleChange.ruleId, ruleChange.name, ex4);
			}
		}
		if (flag | timeProfileConfiguration.UpdateConfiguration(ruleConfiguration.TimeProfileChanges, ruleConfiguration.InitialConfiguration))
		{
			periodicTimeEventGenerator.GenerateEventNow();
		}
	}

	private void ConfigureRuleGroups(IRuleConfiguration rulesInfo)
	{
		try
		{
			GroupConfigurationHelper groupConfigurationHelper = new GroupConfigurationHelper();
			if (rulesInfo.DeviceGroupChanges != null)
			{
				foreach (var deviceGroupChange in rulesInfo.DeviceGroupChanges)
				{
					switch (deviceGroupChange.modification)
					{
					case RuleModification.Add:
						groupConfigurationHelper.GroupAdd(deviceGroupChange.deviceGroupId, deviceGroupChange.parentId);
						break;
					case RuleModification.Remove:
						groupConfigurationHelper.GroupDelete(deviceGroupChange.deviceGroupId);
						break;
					}
				}
			}
			if (rulesInfo.DeviceGroupMemberChanges != null)
			{
				foreach (var deviceGroupMemberChange in rulesInfo.DeviceGroupMemberChanges)
				{
					switch (deviceGroupMemberChange.modification)
					{
					case RuleModification.Add:
						groupConfigurationHelper.GroupMemberAdd(deviceGroupMemberChange.deviceGroupId, deviceGroupMemberChange.deviceId);
						break;
					case RuleModification.Remove:
						groupConfigurationHelper.GroupMemberDelete(deviceGroupMemberChange.deviceGroupId, deviceGroupMemberChange.deviceId);
						break;
					}
				}
			}
			if (rulesInfo.ServerGroupChanges != null)
			{
				foreach (var serverGroupChange in rulesInfo.ServerGroupChanges)
				{
					switch (serverGroupChange.modification)
					{
					case RuleModification.Add:
						groupConfigurationHelper.GroupAdd(serverGroupChange.serverGroupId, serverGroupChange.parentId);
						break;
					case RuleModification.Remove:
						groupConfigurationHelper.GroupDelete(serverGroupChange.serverGroupId);
						break;
					}
				}
			}
			if (rulesInfo.ServerGroupMembershipChanges != null)
			{
				foreach (var serverGroupMembershipChange in rulesInfo.ServerGroupMembershipChanges)
				{
					switch (serverGroupMembershipChange.modification)
					{
					case RuleModification.Add:
						groupConfigurationHelper.GroupMemberAdd(serverGroupMembershipChange.serverGroupId, serverGroupMembershipChange.serverId);
						break;
					case RuleModification.Remove:
						groupConfigurationHelper.GroupMemberDelete(serverGroupMembershipChange.serverGroupId, serverGroupMembershipChange.serverId);
						break;
					}
				}
			}
			if (rulesInfo.RecorderChanges != null)
			{
				foreach (var recorderChange in rulesInfo.RecorderChanges)
				{
					switch (recorderChange.modification)
					{
					case RuleModification.Add:
						groupConfigurationHelper.GroupMemberAdd(RecorderGroupings.AllRecordingServers, recorderChange.recorderId);
						break;
					case RuleModification.Remove:
						groupConfigurationHelper.GroupMemberDelete(RecorderGroupings.AllRecordingServers, recorderChange.recorderId);
						break;
					}
				}
			}
			if (groupConfigurationHelper.HasChanges)
			{
				ruleProcessor.UpdateGroupConfiguration(groupConfigurationHelper);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Logger.LogError("RuleEngine: Unable to update group configuration. Error: {Exception}", ex2);
		}
	}

	private void ConfigureRuleEvents(IRuleConfiguration rulesInfo)
	{
		if (rulesInfo.EventTypesChanges == null)
		{
			return;
		}
		lock (eventDefinitions.SyncRoot)
		{
			foreach (var eventTypesChange in rulesInfo.EventTypesChanges)
			{
				switch (eventTypesChange.modification)
				{
				case RuleModification.Add:
					RegisterEventTypeDefinition(eventTypesChange.xmlDefinition);
					break;
				case RuleModification.Update:
					try
					{
						UnregisterEventTypeDefinition(eventTypesChange.eventTypeDeleteId);
						RegisterEventTypeDefinition(eventTypesChange.xmlDefinition);
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (Exception ex4)
					{
						Logger.LogError("Failed to register modified event type definition in RuleEngine. Error: {Exception}", ex4);
					}
					break;
				case RuleModification.Remove:
					try
					{
						UnregisterEventTypeDefinition(eventTypesChange.eventTypeDeleteId);
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (Exception ex2)
					{
						Logger.LogError("Failed to unregister deleted event type definition in RuleEngine. Error: {Exception}", ex2);
					}
					break;
				}
			}
		}
	}

	public void UpdateConfigurationFromDiffGram(IRuleConfigurationProcessing configurationDiff)
	{
		if (configurationDiff.HasAnyChanges)
		{
			configurationDiff.ProcessConfiguration(configurationDiff);
		}
	}

	public void DynamicRegisterRuleActions(IRuleActionDataProvider ruleActionData)
	{
		lock (actionFactoryMap.SyncRoot)
		{
			Logger.LogInformation("Dynamically loading actions from '{ActionPluginsPath}'", ActionPluginsPath);
			RuleActionFactoryManager ruleActionFactoryManager = new RuleActionFactoryManager();
			ruleActionFactoryManager.RuleActionsPath = ActionPluginsPath;
			ruleActionFactoryManager.Refresh();
			foreach (KeyValuePair<string, RuleActionFactoryInfo> factoryInfo in ruleActionFactoryManager.FactoryInfos)
			{
				try
				{
					string key = factoryInfo.Key;
					RegisterActionFactory(key, ruleActionFactoryManager.GetFactory(key, ruleActionData), factoryInfo.Value.Attribute.Replacement);
					Logger.LogInformation("Rule action factory dynamically loaded. Action name: '{ActionName}'", key);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					Logger.LogError("Failed to initialize RuleEngine with dynamically loaded action factory. Error: '{Exception}'", ex2.Message);
				}
			}
		}
	}

	public void RegisterActionFactory(RuleActionFactoryBase factory)
	{
		if (factory == null)
		{
			throw new ArgumentNullException("factory");
		}
		RuleActionFactoryAttribute ruleActionFactoryAttribute = factory.GetType().GetCustomAttributes(typeof(RuleActionFactoryAttribute), inherit: false)[0] as RuleActionFactoryAttribute;
		RegisterActionFactory(ruleActionFactoryAttribute.RuleActionName, factory, ruleActionFactoryAttribute.Replacement);
	}

	private void RegisterActionFactory(string name, RuleActionFactoryBase actionFactory, bool replacement)
	{
		lock (actionFactoryMap.SyncRoot)
		{
			if (actionFactory == null)
			{
				throw new ArgumentNullException("actionFactory");
			}
			if (actionFactoryMap.ContainsKey(name) && !replacement)
			{
				throw new RuleEngineApplicationException(string.Format(CultureInfo.InvariantCulture, "ActionFactory object with the specified name is already registered. Name: '{0}'", name));
			}
			if (actionFactoryMap.ContainsKey(name))
			{
				Logger.LogInformation("Rule action factory with name '{Name}' already registered with rule engine. Replacing with new factory.", name);
			}
			actionFactoryMap[name] = actionFactory;
			Logger.LogInformation("Rule action factory registered with rule engine. Action name: '{Name}', Implementation: '{ActionFactory}'", name, actionFactory.GetType().ToString());
		}
	}

	public void ClearRegisteredActionFactories()
	{
		lock (actionFactoryMap.SyncRoot)
		{
			actionFactoryMap.Clear();
		}
	}

	private Rule BuildRule((RuleModification modification, Guid ruleId, string name, string xmlDefinition, bool enabled, bool _) ruleInfo)
	{
		XmlDocument xmlDocument = new XmlDocument
		{
			XmlResolver = null
		};
		using (XmlReader reader = XmlReader.Create(new StringReader(ruleInfo.xmlDefinition), new XmlReaderSettings
		{
			XmlResolver = null
		}))
		{
			xmlDocument.Load(reader);
		}
		Rule rule = new Rule(ruleInfo.ruleId, ruleInfo.name);
		rule.Enabled = ruleInfo.enabled;
		XmlNode xmlNode = xmlDocument.DocumentElement.SelectSingleNode("StartRule");
		if (xmlNode == null)
		{
			throw new RuleConfigurationException("Failed to initialize rule. No start rule was specified.");
		}
		XmlNode xmlNode2 = xmlNode.SelectSingleNode("RuleType");
		if (xmlNode2 == null)
		{
			throw new RuleConfigurationException("Failed to initialize rule. No rule type was specified.");
		}
		rule.StartRule = CreateSubRule(xmlNode2);
		if (!rule.StartRule.CanBeStartRule)
		{
			throw new RuleConfigurationException(string.Format(CultureInfo.InvariantCulture, "The specified rule type ({0}) cannot be a start rule.", rule.StartRule.RuleType.ToString()));
		}
		rule.StartRule.Trigger = CreateRuleTrigger(xmlNode2);
		rule.StartRule.Condition = new RuleCondition(timeProvider);
		rule.StartRule.Condition.Initialize(xmlNode.SelectSingleNode("Conditions"), xmlNode2, this);
		rule.StartRule.Actions = InitializeActions(rule.StartRule, xmlNode.SelectSingleNode("Actions"));
		BuildStopRule(rule, xmlDocument.DocumentElement.SelectSingleNode("StopRule"));
		return rule;
	}

	private void BuildStopRule(Rule mainRule, XmlNode stopRuleNode)
	{
		if (stopRuleNode == null)
		{
			return;
		}
		XmlNode xmlNode = stopRuleNode.SelectSingleNode("RuleType");
		if (xmlNode == null)
		{
			return;
		}
		switch (mainRule.StartRule.RuleType)
		{
		case RuleType.Event:
		{
			mainRule.StopRule = CreateSubRule(xmlNode);
			mainRule.StopRule.Trigger = CreateRuleTrigger(xmlNode);
			mainRule.StopRule.Condition = new RuleCondition(timeProvider);
			mainRule.StopRule.Condition.Initialize(stopRuleNode.SelectSingleNode("Conditions"), xmlNode, this);
			mainRule.StopRule.Actions = InitializeActions(mainRule.StopRule, stopRuleNode.SelectSingleNode("Actions"));
			if (!(mainRule.StopRule is TimerSubRule))
			{
				break;
			}
			TimerSubRule timerSubRule = (TimerSubRule)mainRule.StopRule;
			XmlNode xmlNode2 = xmlNode.SelectSingleNode("TimerValue");
			if (xmlNode2 == null || xmlNode2.InnerText.Length == 0)
			{
				throw new RuleConfigurationException("No timer value specified for TimerRule");
			}
			try
			{
				timerSubRule.TimerValue = Convert.ToInt32(xmlNode2.InnerText, CultureInfo.InvariantCulture);
				if (timerSubRule.TimerValue < 1)
				{
					timerSubRule.TimerValue = 1;
				}
				break;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
				throw new RuleConfigurationException("Invalid timer value specified in TimerRule");
			}
		}
		case RuleType.TimeInterval:
		{
			RuleType ruleType;
			try
			{
				ruleType = (RuleType)Enum.Parse(typeof(RuleType), xmlNode.Attributes["type"].Value, ignoreCase: true);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception inner)
			{
				throw new RuleConfigurationException(string.Format(CultureInfo.InvariantCulture, "Could not parse RuleType into enumeration. String value:'{0}'", xmlNode.InnerText), inner);
			}
			if (ruleType != RuleType.TimeInterval)
			{
				throw new RuleConfigurationException("Rule type mismatch. Start rules of the type 'TimeInterval' can only have StopRules of type 'TimeInterval'");
			}
			mainRule.StopRule = CreateSubRule(RuleType.TimeInterval);
			mainRule.StopRule.Trigger = CreateRuleTrigger(xmlNode);
			mainRule.StopRule.Condition = new RuleCondition(timeProvider);
			mainRule.StopRule.Condition.Initialize(null, null, null);
			mainRule.StopRule.Actions = InitializeActions(mainRule.StopRule, stopRuleNode.SelectSingleNode("Actions"));
			break;
		}
		default:
			throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "New rule type ({0}) not implemented in BuildStopRule() in the RuleEngine", mainRule.StartRule.RuleType.ToString()));
		case RuleType.Timer:
			break;
		}
	}

	private static SubRule CreateSubRule(XmlNode ruleTypeNode)
	{
		if (ruleTypeNode == null)
		{
			throw new RuleConfigurationException("No rule type was specified for the rule");
		}
		RuleType ruleType;
		try
		{
			ruleType = (RuleType)Enum.Parse(typeof(RuleType), ruleTypeNode.Attributes["type"].Value, ignoreCase: true);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new RuleConfigurationException(string.Format(CultureInfo.InvariantCulture, "Could not parse RuleType into enumeration. String value:'{0}'", ruleTypeNode.InnerText), inner);
		}
		return CreateSubRule(ruleType);
	}

	private static SubRule CreateSubRule(RuleType ruleType)
	{
		return ruleType switch
		{
			RuleType.Event => new EventSubRule(), 
			RuleType.TimeInterval => new TimeIntervalSubRule(), 
			RuleType.Timer => new TimerSubRule(), 
			RuleType.RecurringTime => new RecurrentTimeSubRule(), 
			_ => throw new RuleConfigurationException(string.Format(CultureInfo.InvariantCulture, "Unexpected rule type was specified for the rule. RuleType: '{0}'", ruleType.ToString())), 
		};
	}

	public RuleTrigger CreateRuleTrigger(XmlNode ruleTypeNode)
	{
		if (ruleTypeNode == null)
		{
			throw new RuleConfigurationException("Failed to initialize rule trigger. No rule type was specified for the rule.");
		}
		RuleType ruleType;
		try
		{
			ruleType = (RuleType)Enum.Parse(typeof(RuleType), ruleTypeNode.Attributes["type"].Value, ignoreCase: true);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception inner)
		{
			throw new RuleConfigurationException(string.Format(CultureInfo.InvariantCulture, "Could not parse RuleType into enumeration. String value:'{0}'", ruleTypeNode.InnerText), inner);
		}
		switch (ruleType)
		{
		case RuleType.Event:
		{
			RuleTrigger ruleTrigger2 = new RuleTrigger(new Guid((ruleTypeNode.SelectSingleNode("EventType") ?? throw new RuleConfigurationException("No event type was specified for the rule")).InnerText));
			XmlNode sourcesNode = ruleTypeNode.SelectSingleNode("Sources");
			ruleTrigger2.InitializeSources(sourcesNode, groupConfiguration);
			if (ruleTrigger2.SourceConditions.Count == 0)
			{
				throw new RuleConfigurationException("No valid source specified for rule.");
			}
			return ruleTrigger2;
		}
		case RuleType.TimeInterval:
		case RuleType.RecurringTime:
		{
			RuleTrigger ruleTrigger = new RuleTrigger(PeriodicTimeEventId);
			ruleTrigger.SourceConditions = new List<SourceCondition>();
			ruleTrigger.SourceConditions.Add(new ListSourceCondition(new List<Guid> { Id }));
			return ruleTrigger;
		}
		case RuleType.Timer:
			return new RuleTrigger(RelativeTimeEventId)
			{
				SourceConditions = new List<SourceCondition>()
			};
		default:
			throw new RuleConfigurationException(string.Format(CultureInfo.InvariantCulture, "Unexpected rule type was specified for the rule. RuleType: '{0}'", ruleType.ToString()));
		}
	}

	private List<IRuleAction> InitializeActions(ISubRule rule, XmlNode actionsNode)
	{
		if (actionsNode == null)
		{
			return new List<IRuleAction>();
		}
		List<IRuleAction> list = new List<IRuleAction>();
		foreach (XmlNode childNode in actionsNode.ChildNodes)
		{
			list.Add(CreateAction(rule, childNode));
		}
		return list;
	}

	private IRuleAction CreateAction(ISubRule rule, XmlNode actionNode)
	{
		string text = actionNode.Attributes["name"].Value;
		if (text.Contains(":"))
		{
			text = text.Split(':')[0];
		}
		RuleActionFactoryBase ruleActionFactoryBase = null;
		lock (actionFactoryMap.SyncRoot)
		{
			ruleActionFactoryBase = actionFactoryMap[text] as RuleActionFactoryBase;
		}
		if (ruleActionFactoryBase == null)
		{
			throw new NotImplementedException(string.Format(CultureInfo.InvariantCulture, "Unknown action specified for rule. Action name:'{0}'", text));
		}
		IRuleAction ruleAction = ruleActionFactoryBase.CreateRuleAction(rule, actionNode);
		ruleAction.InstanceId = RuleActionFactoryBase.ParseInstanceId(actionNode);
		RuleActionFactoryAttribute ruleActionFactoryAttribute = ruleActionFactoryBase.GetType().GetCustomAttributes(typeof(RuleActionFactoryAttribute), inherit: false)[0] as RuleActionFactoryAttribute;
		ActionTarget actionTarget = new ActionTarget(ruleActionFactoryAttribute.ActionTargetType);
		ruleAction.ActionTargets.Add(actionTarget);
		if (ruleActionFactoryAttribute.TargetItemsAttributeName.Length > 0)
		{
			XmlNode actionParameterByName = RuleActionFactoryBase.GetActionParameterByName(actionNode, ruleActionFactoryAttribute.TargetItemsAttributeName);
			if (actionParameterByName == null)
			{
				throw new RuleConfigurationException(string.Format(CultureInfo.InvariantCulture, "'{0}' parameter for '{1}' action was not found", ruleActionFactoryAttribute.TargetItemsAttributeName, text));
			}
			actionTarget.TargetItems.AddRange(RuleActionFactoryBase.BuildGuidArray(actionParameterByName));
		}
		if (ruleActionFactoryAttribute.TargetGroupItemsAttributeName.Length > 0)
		{
			XmlNode actionParameterByName2 = RuleActionFactoryBase.GetActionParameterByName(actionNode, ruleActionFactoryAttribute.TargetGroupItemsAttributeName);
			if (actionParameterByName2 == null)
			{
				throw new RuleConfigurationException(string.Format(CultureInfo.InvariantCulture, "'{0}' parameter for '{1}' action was not found", ruleActionFactoryAttribute.TargetItemsAttributeName, text));
			}
			actionTarget.TargetGroups.AddRange(RuleActionFactoryBase.BuildGuidArray(actionParameterByName2));
		}
		return ruleAction;
	}

	public EventDefinition FindEventDefinitionFromEventTypeId(Guid eventTypeId)
	{
		EventDefinition result = null;
		lock (eventDefinitions.SyncRoot)
		{
			if (eventDefinitions[eventTypeId] is EventDefinition eventDefinition)
			{
				result = new EventDefinition(eventDefinition.SaveAsXml());
			}
		}
		return result;
	}

	private void PeriodicTimeEventGenerator_Timer(object sender, PeriodicTimerEventArgs e)
	{
		AddTriggerEvent(new TriggerEvent(Id, PeriodicTimeEventId, e.EventTime));
	}

	private void ruleProcessor_EventProcessing(object sender, EventProcessingEventArgs e)
	{
		if (e.TriggerEventBase is TriggerEvent)
		{
			OnTriggerEventProcessing((TriggerEvent)e.TriggerEventBase);
		}
		else if (e.TriggerEventBase is UnregisterTriggerEvent)
		{
			OnUnregisterEventProcessing((UnregisterTriggerEvent)e.TriggerEventBase);
		}
	}

	private void ruleProcessor_EventProcessed(object sender, EventProcessingEventArgs e)
	{
		if (e.TriggerEventBase is TriggerEvent)
		{
			OnTriggerEventProcessed((TriggerEvent)e.TriggerEventBase);
		}
		else if (e.TriggerEventBase is UnregisterTriggerEvent)
		{
			OnUnregisterEventProcessed((UnregisterTriggerEvent)e.TriggerEventBase);
		}
	}

	private void ruleProcessor_TriggerEventAsGlobal(object sender, EventProcessingEventArgs e)
	{
		if (e.TriggerEventBase is TriggerEvent)
		{
			OnTriggerEventAsGlobal((TriggerEvent)e.TriggerEventBase);
		}
	}

	private void ruleProcessor_RuleProcessing(object sender, RuleProcessingEventArgs e)
	{
		if (this.RuleProcessing != null)
		{
			this.RuleProcessing(this, e);
		}
	}

	private void ruleProcessor_RuleProcessed(object sender, RuleProcessingEventArgs e)
	{
		if (this.RuleProcessed != null)
		{
			this.RuleProcessed(this, e);
		}
	}

	private void ruleProcessor_RuleActionProcessing(object sender, RuleActionProcessingEventArgs e)
	{
		if (this.RuleActionProcessing != null)
		{
			this.RuleActionProcessing(this, e);
		}
	}

	private void ruleProcessor_RuleActionProcessed(object sender, RuleActionProcessingEventArgs e)
	{
		if (this.RuleActionProcessed != null)
		{
			this.RuleActionProcessed(this, e);
		}
	}

	private void ruleProcessor_RuleClearStateExternal(object sender, RuleEventArgs e)
	{
		if (this.RuleClearStateExternal != null)
		{
			this.RuleClearStateExternal(this, e);
		}
	}

	public void ActionTargetActivated(Guid targetId, TargetChangeTargetTypes targetType)
	{
		Logger.LogInformation("Registering active target to execute actions from TimeInterval rules. TargetId: {TargetId}; TargetType: {TargetType}", targetId, targetType);
		ruleProcessor.AddTargetChangeEvent(new TargetChangeEvent(targetId, targetType, TargetChangeActionTypes.Activated));
	}

	public void Dispose()
	{
		((IDisposable)periodicTimeEventGenerator).Dispose();
	}
}
