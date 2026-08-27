using System.Diagnostics;

namespace VideoOS.Common.Integration.Threading.Internal;

public class STPPerformanceCounter
{
	private readonly PerformanceCounterType _pcType;

	protected string _counterHelp;

	protected string _counterName;

	public string Name => _counterName;

	public STPPerformanceCounter(string counterName, string counterHelp, PerformanceCounterType pcType)
	{
		_counterName = counterName;
		_counterHelp = counterHelp;
		_pcType = pcType;
	}

	public void AddCounterToCollection(CounterCreationDataCollection counterData)
	{
		CounterCreationData value = new CounterCreationData(_counterName, _counterHelp, _pcType);
		counterData.Add(value);
	}
}
