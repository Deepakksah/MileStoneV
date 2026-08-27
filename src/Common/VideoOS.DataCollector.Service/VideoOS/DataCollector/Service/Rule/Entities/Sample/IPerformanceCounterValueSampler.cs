using System.Threading.Tasks;

namespace VideoOS.DataCollector.Service.Rule.Entities.Sample;

public interface IPerformanceCounterValueSampler : IRuleSampler
{
	Task<PerformanceCounterSample?> Sample(string counterPath);
}
