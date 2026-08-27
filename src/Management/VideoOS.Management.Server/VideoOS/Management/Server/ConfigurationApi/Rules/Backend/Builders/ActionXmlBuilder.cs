using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Rules.Adapters.XmlHelper;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public class ActionXmlBuilder : IActionXmlBuilder
{
	private readonly Dictionary<ActionType, string> _actionTypeToFunction = new Dictionary<ActionType, string>
	{
		{
			ActionType.StartRecording,
			"VideoOS.DeviceRecording"
		},
		{
			ActionType.StopRecording,
			"VideoOS.DeviceRecording"
		},
		{
			ActionType.PtzMoveToDefault,
			"VideoOS.GotoDefaultPreset"
		},
		{
			ActionType.SetDeviceOutputState,
			"VideoOS.SetDeviceOutput"
		},
		{
			ActionType.PtzMoveToPreset,
			"VideoOS.GotoPreset"
		},
		{
			ActionType.StartSetFramerate,
			"VideoOS.DeviceFramerate"
		},
		{
			ActionType.StopSetFramerate,
			"VideoOS.DeviceFramerate"
		},
		{
			ActionType.StartSetRecordingFramerate,
			"VideoOS.DeviceRecordingFramerate"
		},
		{
			ActionType.StopSetRecordingFramerate,
			"VideoOS.DeviceRecordingFramerate"
		},
		{
			ActionType.StartFeed,
			"VideoOS.DeviceStartStopFeed"
		},
		{
			ActionType.StopFeed,
			"VideoOS.DeviceStartStopFeed"
		},
		{
			ActionType.RetrieveEdgeStorage,
			"VideoOS.RetrieveEdgeStorageRecording"
		},
		{
			ActionType.RetrieveEdgeStorageTimeOrMetadata,
			"VideoOS.RetrieveEdgeStorageRecordingTimeOrMetadata"
		},
		{
			ActionType.StartRecordAllFrames,
			"VideoOS.RecordAllFrames"
		},
		{
			ActionType.StopRecordAllFrames,
			"VideoOS.RecordAllFrames"
		},
		{
			ActionType.SetBookmark,
			"VideoOS.SetBookmark"
		},
		{
			ActionType.CreateLogEntry,
			"VideoOS.CreateLogEntry"
		},
		{
			ActionType.VideoWallSetToCameras,
			"VideoOS.SetMonitorCameras"
		},
		{
			ActionType.VideoWallSetToText,
			"VideoOS.SetMonitorText"
		},
		{
			ActionType.SendSNMPTrap,
			"VideoOS.CreateSNMPTrap"
		},
		{
			ActionType.VideoWallSetToPreset,
			"VideoOS.SetVideoWallPreset"
		},
		{
			ActionType.ActivateArchiving,
			"VideoOS.ActivateArchiving"
		},
		{
			ActionType.VideoWallRemoveCameras,
			"VideoOS.RemoveMonitorCameras"
		},
		{
			ActionType.StartPluginOnDevices,
			"VideoOS.DeviceStartPlugin"
		},
		{
			ActionType.StopPluginOnDevices,
			"VideoOS.DeviceStopPlugin"
		},
		{
			ActionType.MIPAction,
			"VideoOS.MIPAction"
		},
		{
			ActionType.MIPAction2,
			"VideoOS.MIPAction2"
		},
		{
			ActionType.DeviceChangeSettings,
			"VideoOS.DeviceChangeSettings"
		},
		{
			ActionType.TriggerEventOnRemoteSite,
			"VideoOS.TriggerUserDefinedEventOnRemoteSite"
		},
		{
			ActionType.StartPatrolling,
			"VideoOS.DeviceStartStopPatrolling"
		},
		{
			ActionType.StopPatrolling,
			"VideoOS.DeviceStartStopPatrolling"
		},
		{
			ActionType.PausePatrolling,
			"VideoOS.DevicePauseResumePatrolling"
		},
		{
			ActionType.ResumePatrolling,
			"VideoOS.DevicePauseResumePatrolling"
		},
		{
			ActionType.SetMatrixView,
			"VideoOS.SetMatrixView"
		}
	};

	public string GetXml(ActionModel action)
	{
		action.NullCheck("", "GetXml", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Builders\\ActionXmlBuilder.cs", 53);
		Guid mipActionId;
		ActionType actionType = ActionTypeHelper.GetActionType(action.ActionType, out mipActionId);
		if (!_actionTypeToFunction.TryGetValue(actionType, out var value))
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Invalid action type: {0}", action.ActionType));
		}
		if (actionType == ActionType.MIPAction || actionType == ActionType.MIPAction2)
		{
			value = string.Format(CultureInfo.InvariantCulture, value + ":" + mipActionId.ToString());
		}
		List<Tuple<string, string>> list = new List<Tuple<string, string>>();
		foreach (KeyValuePair<string, RuleParameter> item2 in action.Parameters.Where((KeyValuePair<string, RuleParameter> p) => !p.Value.ExcludeFromDatabase))
		{
			string text = ((item2.Value.Values != null) ? string.Join(",", item2.Value.Values) : null);
			if (!string.IsNullOrEmpty(item2.Value.ParameterToAppend) && action.Parameters.ContainsKey(item2.Value.ParameterToAppend))
			{
				RuleParameter ruleParameter = action.Parameters[item2.Value.ParameterToAppend];
				string text2 = ((ruleParameter.Values != null) ? string.Join(",", ruleParameter.Values) : null);
				if (string.IsNullOrEmpty(text))
				{
					text = text2;
				}
				else if (text2 != null)
				{
					text = text + "," + text2;
				}
			}
			string item = (string.IsNullOrEmpty(item2.Value.XmlName) ? item2.Key : item2.Value.XmlName);
			list.Add(new Tuple<string, string>(item, text));
		}
		return RuleXmlHelper.MakeActionXml(value, list.Select((Tuple<string, string> nav) => nav.Item1).ToArray(), list.Select((Tuple<string, string> nav) => nav.Item2).ToArray());
	}
}
