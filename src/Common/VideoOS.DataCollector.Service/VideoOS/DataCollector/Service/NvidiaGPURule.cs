using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using VideoOS.DataCollector.Service.Configuration;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;

namespace VideoOS.DataCollector.Service;

public class NvidiaGPURule : ThresholdRule
{
	public const string GPUDecodingRuleId = "FC6A2188-C1AF-48B5-ACB7-E88FE093D30C";

	public const string GPUMemoryRuleId = "238C1803-601B-4441-9FDE-46E7CBE2FF6E";

	public const string GPURenderingRuleId = "3732961A-025E-4D80-AEF9-5B884E6B2BAF";

	public const string CategoryName = "VideoOS Recording Server GPU Performance";

	public const string DecoderUsageName = "% Decoder Usage";

	public const string RenderingUsageName = "% Rendering Usage";

	public const string MemoryUsageName = "% Memory Usage";

	public NvidiaGPURule(int dataCollectorId, CounterStateRuleConfiguration ruleConfiguration, IStateHandlerFactory stateFactory, IOptions<RuleOptions> options)
		: base(dataCollectorId, ruleConfiguration, isDefault: true, triggerPerValue: false, counterDirection: true, stateFactory, options)
	{
		_stateHandler.SetUndefined();
	}

	protected override async Task<(bool, StateChange?)> DoSample(ISamplerFactory samplerFactory, CancellationToken cancellationToken)
	{
		if (!_isActive)
		{
			return (false, null);
		}
		NvidiaSampleType type = NvidiaSampleType.None;
		string name = "";
		switch (base.RuleId.ToUpperInvariant())
		{
		case "FC6A2188-C1AF-48B5-ACB7-E88FE093D30C":
			type = NvidiaSampleType.DecoderLoad;
			name = "% Decoder Usage";
			break;
		case "3732961A-025E-4D80-AEF9-5B884E6B2BAF":
			type = NvidiaSampleType.DeviceLoad;
			name = "% Rendering Usage";
			break;
		case "238C1803-601B-4441-9FDE-46E7CBE2FF6E":
			type = NvidiaSampleType.GetMemoryUsage;
			name = "% Memory Usage";
			break;
		}
		NvidiaSample nvidiaSample = await samplerFactory.GetNvidiaSampler().Sample(type);
		if (nvidiaSample == null)
		{
			return (false, null);
		}
		PerformanceCounterSample counterValue = new PerformanceCounterSample("VideoOS Recording Server GPU Performance", name, "_total", DateTime.UtcNow, nvidiaSample.Value, nvidiaSample.Value);
		return _stateHandler.AddCounterValue(counterValue);
	}
}
