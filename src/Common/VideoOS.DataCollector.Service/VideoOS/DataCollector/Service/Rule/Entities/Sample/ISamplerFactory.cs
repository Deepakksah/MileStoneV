namespace VideoOS.DataCollector.Service.Rule.Entities.Sample;

public interface ISamplerFactory
{
	INvidiaSampler GetNvidiaSampler();

	IPerformanceCounterValueSampler GetPerformanceCounterValueSampler();

	IServiceSampler GetServiceSampler();
}
