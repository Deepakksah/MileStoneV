using System;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Rule.Entities.State;
using VideoOS.DataCollector.Service.Rule.Events;

namespace VideoOS.DataCollector.Service;

public class CameraCounterRule : CounterRule
{
	public virtual StateChange RecorderDownState => new StateChange(State.Critical, -1f);

	public CameraCounterRule(int datacollectoId, CounterStateRuleConfiguration ruleConfiguration, bool isDefault, bool triggerPerValue, bool counterDirection, IStateHandlerFactory stateHandlerFactory, IOptions<RuleOptions> options)
		: base(datacollectoId, ruleConfiguration, isDefault, triggerPerValue, counterDirection, stateHandlerFactory, options)
	{
	}

	public override void OnRecordingServerDown()
	{
		(bool, StateChange) tuple = _stateHandler.AddStateChange(RecorderDownState);
		if (tuple.Item1 && tuple.Item2 != null)
		{
			StateChangeData stateChange = new StateChangeData(_dataCollectorId, base.RuleId, base.CounterPath, (int)tuple.Item2.CounterState, tuple.Item2.TriggerValue);
			AddDomainEvent(new StateChangeEvent(stateChange));
		}
		_isActive = false;
		_stateHandler.Reset();
	}

	public override void OnRecordingServerUp()
	{
		_isActive = true;
	}

	public static bool IsValidPath(string counterPath)
	{
		Guid result;
		return Guid.TryParse(ExtractIdFromPath(counterPath), out result);
	}

	private static string ExtractIdFromPath(string path)
	{
		int num = path.LastIndexOf('[');
		int num2 = path.LastIndexOf(']');
		if (num != -1 && num2 != -1 && num < num2)
		{
			return path.Substring(num + 1, num2 - num - 1);
		}
		return string.Empty;
	}
}
