using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Validation;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Definitions;

public class StartPluginOnDevicesActionDefinition : IActionDefinition
{
	private static IDictionary<string, string> _recorderPlugins;

	public string ActionType => VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain.ActionType.StartPluginOnDevices.ToString();

	public IDictionary<string, RuleParameterDefinition> Parameters => CreateParameters();

	public string CounteractionActionType => VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain.ActionType.None.ToString();

	public string DisplayName => ConfigurationApiResources.RuleActionStartPlugin;

	public ActionPosition Position => ActionPosition.None;

	public IEnumerable<string> RequiredFeatures => new string[0];

	public IEnumerable<TriggerType> ValidForTriggerTypes => new TriggerType[3]
	{
		TriggerType.Event,
		TriggerType.TimeInterval,
		TriggerType.Timer
	};

	public StartPluginOnDevicesActionDefinition(IDictionary<string, string> recorderPlugins)
	{
		_recorderPlugins = recorderPlugins;
	}

	private static IDictionary<string, RuleParameterDefinition> CreateParameters()
	{
		return new RuleParameterDefinition[3]
		{
			new RuleParameterDefinition(_recorderPlugins)
			{
				Name = "PluginIds",
				ValueType = typeof(Guid[]),
				SpecialPropertyType = "Separator",
				ValidatorType = typeof(RecorderPluginValueValidator)
			},
			new RuleParameterDefinition
			{
				Name = "DeviceIds",
				PairedParameter = "DeviceGroupIds",
				ValueType = typeof(Guid[]),
				EmptyAllowed = true,
				SpecialPropertyType = "PathList",
				AllowedItemTypes = new string[4] { "Camera", "Speaker", "Microphone", "Metadata" },
				ValidatorType = typeof(DeviceParameterValueValidator)
			},
			new RuleParameterDefinition
			{
				Name = "DeviceGroupIds",
				PropertyName = "DeviceIds",
				PairedParameter = "DeviceIds",
				ValueType = typeof(Guid[]),
				EmptyAllowed = true,
				SpecialPropertyType = "PathList",
				AllowedItemTypes = new string[4] { "CameraGroup", "SpeakerGroup", "MicrophoneGroup", "MetadataGroup" },
				ValidatorType = typeof(DeviceGroupParameterValueValidator)
			}
		}.ToDictionary((RuleParameterDefinition s) => s.Name);
	}
}
