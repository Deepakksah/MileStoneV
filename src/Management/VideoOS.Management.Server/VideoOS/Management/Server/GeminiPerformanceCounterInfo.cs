using System.Diagnostics.PerformanceData;

namespace VideoOS.Management.Server;

public class GeminiPerformanceCounterInfo
{
	public int Id { get; private set; }

	public string Name { get; private set; }

	public CounterType CounterType { get; private set; }

	public string Uri { get; private set; }

	public GeminiPerformanceCounterInfo(int id, string name, CounterType type)
	{
		Id = id;
		Name = name;
		CounterType = type;
	}

	public GeminiPerformanceCounterInfo(int id, string name, CounterType type, string uri)
	{
		Id = id;
		Name = name;
		CounterType = type;
		Uri = uri;
	}
}
