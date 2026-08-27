namespace VideoOS.Management.Common;

public class EventSourceFilter
{
	public EventSource.FilterTypes FilterType { get; set; }

	public string Value { get; set; }

	public EventSourceFilter(EventSource.FilterTypes filterType)
		: this(filterType, string.Empty)
	{
	}

	public EventSourceFilter(EventSource.FilterTypes filterType, string value)
	{
		FilterType = filterType;
		Value = value;
	}
}
