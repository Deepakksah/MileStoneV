using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public class XmlRuleModelBuilder : IXmlRuleModelBuilder
{
	private readonly IConditionsParser _conditionsParser;

	private readonly IRuleTypeParser _ruleTypeParser;

	private static readonly IDictionary<Tuple<string, bool>, ActionType> _actionTypes = new Dictionary<Tuple<string, bool>, ActionType>
	{
		{
			new Tuple<string, bool>("VideoOS.DeviceRecording", item2: true),
			ActionType.StartRecording
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceRecording", item2: false),
			ActionType.StopRecording
		},
		{
			new Tuple<string, bool>("VideoOS.GotoDefaultPreset", item2: true),
			ActionType.PtzMoveToDefault
		},
		{
			new Tuple<string, bool>("VideoOS.SetDeviceOutput", item2: true),
			ActionType.SetDeviceOutputState
		},
		{
			new Tuple<string, bool>("VideoOS.GotoPreset", item2: true),
			ActionType.PtzMoveToPreset
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceFramerate", item2: true),
			ActionType.StartSetFramerate
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceFramerate", item2: false),
			ActionType.StopSetFramerate
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceRecordingFramerate", item2: true),
			ActionType.StartSetRecordingFramerate
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceRecordingFramerate", item2: false),
			ActionType.StopSetRecordingFramerate
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceStartStopFeed", item2: true),
			ActionType.StartFeed
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceStartStopFeed", item2: false),
			ActionType.StopFeed
		},
		{
			new Tuple<string, bool>("VideoOS.RetrieveEdgeStorageRecording", item2: true),
			ActionType.RetrieveEdgeStorage
		},
		{
			new Tuple<string, bool>("VideoOS.RetrieveEdgeStorageRecordingTimeOrMetadata", item2: true),
			ActionType.RetrieveEdgeStorageTimeOrMetadata
		},
		{
			new Tuple<string, bool>("VideoOS.RecordAllFrames", item2: true),
			ActionType.StartRecordAllFrames
		},
		{
			new Tuple<string, bool>("VideoOS.RecordAllFrames", item2: false),
			ActionType.StopRecordAllFrames
		},
		{
			new Tuple<string, bool>("VideoOS.SetBookmark", item2: true),
			ActionType.SetBookmark
		},
		{
			new Tuple<string, bool>("VideoOS.CreateLogEntry", item2: true),
			ActionType.CreateLogEntry
		},
		{
			new Tuple<string, bool>("VideoOS.SetMonitorCameras", item2: true),
			ActionType.VideoWallSetToCameras
		},
		{
			new Tuple<string, bool>("VideoOS.SetMonitorText", item2: true),
			ActionType.VideoWallSetToText
		},
		{
			new Tuple<string, bool>("VideoOS.CreateSNMPTrap", item2: true),
			ActionType.SendSNMPTrap
		},
		{
			new Tuple<string, bool>("VideoOS.SetVideoWallPreset", item2: true),
			ActionType.VideoWallSetToPreset
		},
		{
			new Tuple<string, bool>("VideoOS.ActivateArchiving", item2: true),
			ActionType.ActivateArchiving
		},
		{
			new Tuple<string, bool>("VideoOS.RemoveMonitorCameras", item2: true),
			ActionType.VideoWallRemoveCameras
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceStartPlugin", item2: true),
			ActionType.StartPluginOnDevices
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceStopPlugin", item2: true),
			ActionType.StopPluginOnDevices
		},
		{
			new Tuple<string, bool>("VideoOS.MIPAction", item2: true),
			ActionType.MIPAction
		},
		{
			new Tuple<string, bool>("VideoOS.MIPAction2", item2: true),
			ActionType.MIPAction2
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceChangeSettings", item2: true),
			ActionType.DeviceChangeSettings
		},
		{
			new Tuple<string, bool>("VideoOS.TriggerUserDefinedEventOnRemoteSite", item2: true),
			ActionType.TriggerEventOnRemoteSite
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceStartStopPatrolling", item2: true),
			ActionType.StartPatrolling
		},
		{
			new Tuple<string, bool>("VideoOS.DeviceStartStopPatrolling", item2: false),
			ActionType.StopPatrolling
		},
		{
			new Tuple<string, bool>("VideoOS.DevicePauseResumePatrolling", item2: true),
			ActionType.PausePatrolling
		},
		{
			new Tuple<string, bool>("VideoOS.DevicePauseResumePatrolling", item2: false),
			ActionType.ResumePatrolling
		},
		{
			new Tuple<string, bool>("VideoOS.SetMatrixView", item2: true),
			ActionType.SetMatrixView
		}
	};

	public XmlRuleModelBuilder(IConditionsParser conditionsParser, IRuleTypeParser ruleTypeParser)
	{
		_conditionsParser = conditionsParser;
		_ruleTypeParser = ruleTypeParser;
	}

	public RuleModel Build(Guid id, string name, string description, string ruleXml, bool enabled, IGeminiRequestContext requestContext)
	{
		RuleModel ruleModel = new RuleModel
		{
			RuleStart = new RulePartModel
			{
				IsStart = true
			},
			RuleStop = new RulePartModel
			{
				IsStart = false
			}
		};
		ruleModel.Name = name;
		ruleModel.Description = description;
		ruleModel.Id = id;
		ruleModel.Enabled = enabled;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(ruleXml);
		XmlNode node = xmlDocument.GetElementsByTagName("Rule")[0];
		XmlNode nodeByTagName = node.GetNodeByTagName("StartRule");
		XmlNode nodeByTagName2 = nodeByTagName.GetNodeByTagName("RuleType");
		if (!_ruleTypeParser.TryParseRuleType(nodeByTagName2, isStart: true, ruleModel.RuleStart, requestContext))
		{
			return null;
		}
		if (!CreateActions(ruleModel.RuleStart, nodeByTagName, isStartRule: true))
		{
			return null;
		}
		XmlNode nodeByTagName3 = node.GetNodeByTagName("StopRule");
		if (nodeByTagName3 != null && nodeByTagName3.HasChildNodes)
		{
			XmlNode nodeByTagName4 = nodeByTagName3.GetNodeByTagName("RuleType");
			if (!_ruleTypeParser.TryParseRuleType(nodeByTagName4, isStart: false, ruleModel.RuleStop, requestContext))
			{
				return null;
			}
			if (!CreateActions(ruleModel.RuleStop, nodeByTagName3, isStartRule: false))
			{
				return null;
			}
		}
		XmlNode nodeByTagName5 = nodeByTagName.GetNodeByTagName("Conditions");
		if (!_conditionsParser.TryParseConditions(nodeByTagName5, out var conditions))
		{
			return null;
		}
		ruleModel.RuleStart.Conditions = conditions.ToDictionary((ConditionModel s) => s.ConditionType);
		return ruleModel;
	}

	private static bool CreateActions(RulePartModel theRule, XmlNode ruleNode, bool isStartRule)
	{
		XmlNode nodeByTagName = ruleNode.GetNodeByTagName("Actions");
		if (nodeByTagName == null || !nodeByTagName.HasChildNodes)
		{
			return !isStartRule;
		}
		List<ActionModel> list = new List<ActionModel>(nodeByTagName.ChildNodes.Count);
		foreach (XmlNode childNode in nodeByTagName.ChildNodes)
		{
			try
			{
				ActionModel action = GetAction(childNode, isStartRule);
				if (action == null)
				{
					return false;
				}
				list.Add(action);
			}
			catch (ArgumentException)
			{
				return false;
			}
		}
		theRule.Actions = list;
		return true;
	}

	private static ActionModel GetAction(XmlNode actionXmlNode, bool isStartAction)
	{
		string actionType = GetActionType((actionXmlNode.Attributes["name"] ?? throw new ArgumentException("Invalid Action XML: " + actionXmlNode.OuterXml)).Value, isStartAction);
		if (actionType == ActionType.None.ToString())
		{
			return null;
		}
		List<RuleParameter> list = new List<RuleParameter>();
		foreach (XmlNode childNode in actionXmlNode.ChildNodes)
		{
			XmlAttribute xmlAttribute = childNode.Attributes["name"];
			if (xmlAttribute != null)
			{
				list.Add(new RuleParameter
				{
					Name = xmlAttribute.Value,
					Values = ((!string.IsNullOrEmpty(childNode.InnerText)) ? (from s in childNode.InnerText.Split(',')
						select s.Trim()).ToArray() : new string[0])
				});
			}
		}
		return new ActionModel
		{
			ActionType = actionType,
			Parameters = list.ToDictionary((RuleParameter s) => s.Name)
		};
	}

	private static string GetActionType(string functionName, bool isStartAction)
	{
		string[] array = functionName.Split(':');
		if (_actionTypes.TryGetValue(new Tuple<string, bool>(array[0], isStartAction), out var value) || (!isStartAction && _actionTypes.TryGetValue(new Tuple<string, bool>(array[0], item2: true), out value)))
		{
			string text = value.ToString();
			if (value == ActionType.MIPAction || value == ActionType.MIPAction2)
			{
				if (array[1] == "40121049-598f-4741-8fd3-0b00a64882d2")
				{
					return ActionType.None.ToString();
				}
				text = string.Format(CultureInfo.CurrentCulture, text + ":" + array[1]);
			}
			return text;
		}
		return ActionType.None.ToString();
	}
}
