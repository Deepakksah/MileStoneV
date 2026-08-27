using VideoOS.DataCollector.Service.Entities.DataCollection;

namespace VideoOS.DataCollector.Service;

public class UnknownPerformanceCounterValue : PerformanceCounterValue
{
	public UnknownPerformanceCounterValue()
		: base(string.Empty, string.Empty, string.Empty)
	{
	}
}
