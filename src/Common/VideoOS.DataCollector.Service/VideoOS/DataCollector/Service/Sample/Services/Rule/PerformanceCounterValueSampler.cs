using System.Threading.Tasks;
using VideoOS.DataCollector.Service.Entities;
using VideoOS.DataCollector.Service.Entities.DataCollection;
using VideoOS.DataCollector.Service.Persistence;
using VideoOS.DataCollector.Service.Rule.Entities.Sample;

namespace VideoOS.DataCollector.Service.Sample.Services.Rule;

public class PerformanceCounterValueSampler : IPerformanceCounterValueSampler, IRuleSampler
{
	private readonly IActivePerformanceCounterRepository _repository;

	private readonly PerformanceCounterReaderService _readerService;

	public PerformanceCounterValueSampler(IActivePerformanceCounterRepository repository, PerformanceCounterReaderService readerService)
	{
		_repository = repository;
		_readerService = readerService;
	}

	public Task<PerformanceCounterSample?> Sample(string counterPath)
	{
		if (_repository.Counters.TryGetValue(counterPath, out PerformanceCounterCollector value))
		{
			value?.SampleData(_readerService);
			PerformanceCounterValue value2 = value?.GetValue(_readerService);
			return Task.FromResult(Map(value2));
		}
		return Task.FromResult<PerformanceCounterSample>(null);
	}

	private PerformanceCounterSample? Map(PerformanceCounterValue? value)
	{
		if (value == null)
		{
			return null;
		}
		return new PerformanceCounterSample(value.CategoryName, value.CounterName, value.InstanceName, value.CollectionTime, value.RawValue, value.CalculatedValue);
	}
}
