using System.Threading.Tasks;

namespace VideoOS.DataCollector.Service.Rule.Entities.Sample;

public interface INvidiaSampler : IRuleSampler
{
	Task<NvidiaSample?> Sample(NvidiaSampleType type);
}
