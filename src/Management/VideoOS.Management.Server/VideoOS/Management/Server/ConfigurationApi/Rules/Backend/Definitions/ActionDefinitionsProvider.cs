using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Proxies;
using VideoOS.Management.Server.Properties;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public class ActionDefinitionsProvider
{
	private IMIPActionProxy _mipActionProxy;

	private IMIPKindProxy _mipKindProxy;

	private IRecorderPluginProxy _recorderPluginProxy;

	private ISiteProxy _siteProxy;

	private IGeminiApplicationCoreProxy _geminiApplicationCoreProxy;

	private IDictionary<ActionType, IActionDefinition> _definitions = new Dictionary<ActionType, IActionDefinition>();

	public ActionDefinitionsProvider(IMIPActionProxy mipActionProxy, IMIPKindProxy mipKindProxy, IRecorderPluginProxy recorderPluginProxy, ISiteProxy siteProxy, IGeminiApplicationCoreProxy geminiApplicationCoreProxy)
	{
		_mipActionProxy = mipActionProxy;
		_mipKindProxy = mipKindProxy;
		_recorderPluginProxy = recorderPluginProxy;
		_siteProxy = siteProxy;
		_geminiApplicationCoreProxy = geminiApplicationCoreProxy;
		AddDefinitions();
	}

	private void AddDefinitions()
	{
		_definitions[ActionType.StartRecording] = new StartRecordingActionDefinition();
		_definitions[ActionType.StopRecording] = new StopRecordingActionDefinition();
		_definitions[ActionType.PtzMoveToDefault] = new GotoDefaultPresetActionDefinition();
		_definitions[ActionType.PtzMoveToPreset] = new GotoPresetActionDefinition();
		_definitions[ActionType.SetDeviceOutputState] = new SetDeviceOutputStateActionDefinition();
		_definitions[ActionType.StartSetFramerate] = new StartSetFramerateActionDefinition();
		_definitions[ActionType.StopSetFramerate] = new StopSetFramerateActionDefinition();
		_definitions[ActionType.StartSetRecordingFramerate] = new StartSetRecordingFramerateActionDefinition();
		_definitions[ActionType.StopSetRecordingFramerate] = new StopSetRecordingFramerateActionDefinition();
		_definitions[ActionType.StartFeed] = new StartFeedActionDefinition();
		_definitions[ActionType.StopFeed] = new StopFeedActionDefinition();
		_definitions[ActionType.RetrieveEdgeStorage] = new RetrieveEdgeStorageActionDefinition();
		_definitions[ActionType.RetrieveEdgeStorageTimeOrMetadata] = new RetrieveEdgeStorageTimeOrMetadataActionDefinition();
		_definitions[ActionType.StartRecordAllFrames] = new StartRecordAllFramesActionDefinition();
		_definitions[ActionType.StopRecordAllFrames] = new StopRecordAllFramesActionDefinition();
		_definitions[ActionType.SetBookmark] = new SetBookmarkActionDefinition();
		_definitions[ActionType.CreateLogEntry] = new CreateLogEntryActionDefinition();
		_definitions[ActionType.VideoWallSetToCameras] = new SetVideoWallCamerasActionDefinition();
		_definitions[ActionType.VideoWallSetToText] = new SetVideoWallTextActionDefinition();
		_definitions[ActionType.SendSNMPTrap] = new SendSNMPTrapActionDefinition();
		_definitions[ActionType.VideoWallSetToPreset] = new SetVideoWallPresetActionDefinition();
		_definitions[ActionType.ActivateArchiving] = new ActivateArchivingActionDefinition();
		_definitions[ActionType.VideoWallRemoveCameras] = new RemoveMonitorCamerasActionDefinition();
		_definitions[ActionType.DeviceChangeSettings] = new DeviceChangeSettingsActionDefinition();
		_definitions[ActionType.StartPatrolling] = new StartPatrollingActionDefinition();
		_definitions[ActionType.StopPatrolling] = new StopPatrollingActionDefinition();
		_definitions[ActionType.PausePatrolling] = new PausePatrollingActionDefinition();
		_definitions[ActionType.ResumePatrolling] = new ResumePatrollingActionDefinition();
		_definitions[ActionType.SetMatrixView] = new SetMatrixViewActionDefinition();
	}

	public IDictionary<string, string> GetActionsForDisplay(IGeminiRequestContext requestContext, bool isStart, TriggerType ruleType)
	{
		Dictionary<string, string> actionsForRuleType = GetActionsForRuleType(requestContext, ruleType, isStart);
		MIPAction[] array = _mipActionProxy.QueryMIPActions(requestContext);
		foreach (MIPAction mIPAction in array)
		{
			string key = FormatMIPActionTypeString(mIPAction);
			if (IsActionValidForRuleType(requestContext, key, ruleType))
			{
				actionsForRuleType.Add(key, mIPAction.SelectionText);
			}
		}
		if (IsActionValidForRuleType(requestContext, ActionType.StartPluginOnDevices.ToString(), ruleType))
		{
			actionsForRuleType.Add(ActionType.StartPluginOnDevices.ToString(), ConfigurationApiResources.RuleActionStartPlugin);
		}
		if (IsActionValidForRuleType(requestContext, ActionType.StopPluginOnDevices.ToString(), ruleType))
		{
			actionsForRuleType.Add(ActionType.StopPluginOnDevices.ToString(), ConfigurationApiResources.RuleActionStopPlugin);
		}
		if (IsActionValidForRuleType(requestContext, ActionType.TriggerEventOnRemoteSite.ToString(), ruleType))
		{
			actionsForRuleType.Add(ActionType.TriggerEventOnRemoteSite.ToString(), ConfigurationApiResources.RuleActionTriggerUserDefinedEventOnRemoteSite);
		}
		return actionsForRuleType;
	}

	private IDictionary<string, string> GetRecorderPlugins(IGeminiRequestContext requestContext)
	{
		return _recorderPluginProxy.QueryPlugins(requestContext).ToDictionary((RecorderPluginData x) => x.RecorderPluginId.ToString(), (RecorderPluginData x) => x.Name);
	}

	private static string FormatMIPActionTypeString(MIPAction mipAction)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(mipAction.MIPKinds2);
		ActionType actionType = (xmlDocument.FirstChild.HasChildNodes ? ActionType.MIPAction2 : ActionType.MIPAction);
		return string.Format(CultureInfo.InvariantCulture, actionType.ToString() + ":" + mipAction.Id);
	}

	public IActionDefinition Get(IGeminiRequestContext requestContext, string key)
	{
		Guid mipActionId;
		ActionType actionType = ActionTypeHelper.GetActionType(key, out mipActionId);
		switch (actionType)
		{
		case ActionType.MIPAction:
		case ActionType.MIPAction2:
			return GetMIPActionDefinition(requestContext, actionType, mipActionId);
		case ActionType.StartPluginOnDevices:
			return new StartPluginOnDevicesActionDefinition(GetRecorderPlugins(requestContext));
		case ActionType.StopPluginOnDevices:
			return new StopPluginOnDevicesActionDefinition(GetRecorderPlugins(requestContext));
		case ActionType.TriggerEventOnRemoteSite:
			return new TriggerEventOnRemoteSiteActionDefinition(GetHomeSiteId(requestContext));
		default:
		{
			if (_definitions.TryGetValue(actionType, out var value))
			{
				return value;
			}
			return null;
		}
		}
	}

	private IActionDefinition GetMIPActionDefinition(IGeminiRequestContext requestContext, ActionType actionType, Guid mipActionId)
	{
		MIPAction mIPAction = _mipActionProxy.QueryMIPAction(requestContext, mipActionId.ToString());
		if (mIPAction != null)
		{
			try
			{
				Guid[] mipKinds;
				string[] itemTypes = GetItemTypes(requestContext, mIPAction.MIPKinds, isGroup: false, out mipKinds);
				string[] itemTypes2 = GetItemTypes(requestContext, mIPAction.MIPKinds, isGroup: true, out mipKinds);
				bool flag = mipKinds.Contains(new Guid("6d3bdb33-d913-470f-8784-3dd4ee94db8c"));
				if (actionType == ActionType.MIPAction)
				{
					return new MIPActionDefinition(mIPAction.Id, mIPAction.Name, mIPAction.SelectionText, itemTypes, itemTypes2, flag ? new Guid[0] : mipKinds, flag);
				}
				Guid[] mipKinds2;
				string[] itemTypes3 = GetItemTypes(requestContext, mIPAction.MIPKinds2, isGroup: false, out mipKinds2);
				string[] itemTypes4 = GetItemTypes(requestContext, mIPAction.MIPKinds2, isGroup: true, out mipKinds2);
				return new MIPAction2Definition(mIPAction.Id, mIPAction.Name, mIPAction.SelectionText, itemTypes, itemTypes2, mipKinds, itemTypes3, itemTypes4, mipKinds2);
			}
			catch (UnknownDeviceException)
			{
				return null;
			}
		}
		return null;
	}

	private string[] GetItemTypes(IGeminiRequestContext requestContext, string kindXmlString, bool isGroup, out Guid[] mipKinds)
	{
		HashSet<string> hashSet = new HashSet<string>();
		List<Guid> list = new List<Guid>();
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(kindXmlString);
		foreach (XmlNode childNode in xmlDocument.FirstChild.ChildNodes)
		{
			Guid guid = new Guid(childNode.SelectSingleNode("Kind").InnerText);
			string text = ItemTypeFromBuildInKind(guid, isGroup);
			if (text != null)
			{
				hashSet.Add(text);
			}
			else if (guid == new Guid("6d3bdb33-d913-470f-8784-3dd4ee94db8c"))
			{
				list.Add(guid);
				if (isGroup)
				{
					hashSet.Add("AccessControlRuleGroup");
					continue;
				}
				hashSet.Add("AccessControlSystem");
				hashSet.Add("AccessControlUnit");
			}
			else
			{
				if (_mipKindProxy.GetKind(requestContext, guid) == null)
				{
					throw new UnknownDeviceException();
				}
				list.Add(guid);
				hashSet.Add(isGroup ? "MIPKind" : "MIPItem");
			}
		}
		mipKinds = list.ToArray();
		return hashSet.ToArray();
	}

	private static string ItemTypeFromBuildInKind(Guid kind, bool isGroup)
	{
		if (Kind.Camera == kind)
		{
			if (!isGroup)
			{
				return "Camera";
			}
			return "CameraGroup";
		}
		if (Kind.Microphone == kind)
		{
			if (!isGroup)
			{
				return "Microphone";
			}
			return "MicrophoneGroup";
		}
		if (Kind.Microphone == kind)
		{
			if (!isGroup)
			{
				return "Microphone";
			}
			return "MicrophoneGroup";
		}
		if (Kind.Speaker == kind)
		{
			if (!isGroup)
			{
				return "Speaker";
			}
			return "SpeakerGroup";
		}
		if (Kind.Metadata == kind)
		{
			if (!isGroup)
			{
				return "Metadata";
			}
			return "MetadataGroup";
		}
		if (Kind.InputEvent == kind)
		{
			if (!isGroup)
			{
				return "InputEvent";
			}
			return "InputEventGroup";
		}
		if (Kind.Output == kind)
		{
			if (!isGroup)
			{
				return "Output";
			}
			return "OutputGroup";
		}
		return null;
	}

	private string GetHomeSiteId(IGeminiRequestContext requestContext)
	{
		return _siteProxy.QueryHomeSite(requestContext).Id;
	}

	private Dictionary<string, string> GetActionsForRuleType(IGeminiRequestContext requestContext, TriggerType ruleType, bool isStart)
	{
		return _definitions.Where((KeyValuePair<ActionType, IActionDefinition> m) => ConfigurationItemActionHelper.IsCorrectPosition(isStart, m.Value.Position) && AreRequiredFeaturesLicensed(requestContext, m.Value.ActionType) && m.Value.ValidForTriggerTypes.Contains(ruleType)).ToDictionary((KeyValuePair<ActionType, IActionDefinition> m) => m.Value.ActionType.ToString(), (KeyValuePair<ActionType, IActionDefinition> m) => m.Value.DisplayName);
	}

	public bool AreRequiredFeaturesLicensed(IGeminiRequestContext requestContext, string key)
	{
		string[] source = GetActionRequiredFeatures(requestContext, key).ToArray();
		if (!source.Any())
		{
			return true;
		}
		return source.All((string feature) => _geminiApplicationCoreProxy.IsFeatureLicensed(requestContext, feature));
	}

	public IEnumerable<string> GetActionRequiredFeatures(IGeminiRequestContext requestContext, string key)
	{
		return Get(requestContext, key)?.RequiredFeatures ?? new string[0];
	}

	public bool IsActionValidForRuleType(IGeminiRequestContext requestContext, string key, TriggerType ruleType)
	{
		return Get(requestContext, key)?.ValidForTriggerTypes.Contains(ruleType) ?? false;
	}
}
