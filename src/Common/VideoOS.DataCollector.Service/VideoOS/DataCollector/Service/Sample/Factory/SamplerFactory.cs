using VideoOS.DataCollector.Service.Rule.Entities.Sample;

namespace VideoOS.DataCollector.Service.Sample.Factory;

public class SamplerFactory : ISamplerFactory
{
	private readonly IServiceSampler _serviceSampler;

	private readonly INvidiaSampler _nvidiaSampler;

	private readonly IPerformanceCounterValueSampler _performanceCounterValueSampler;

	public SamplerFactory(IServiceSampler serviceSampler, INvidiaSampler nvidiaSampler, IPerformanceCounterValueSampler performanceCounterValueSampler)
	{
		_serviceSampler = serviceSampler;
		_nvidiaSampler = nvidiaSampler;
		_performanceCounterValueSampler = performanceCounterValueSampler;
	}

	public INvidiaSampler GetNvidiaSampler()
	{
		return _nvidiaSampler;
	}

	public IPerformanceCounterValueSampler GetPerformanceCounterValueSampler()
	{
		return _performanceCounterValueSampler;
	}

	public IServiceSampler GetServiceSampler()
	{
		return _serviceSampler;
	}
}
