using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace VideoOS.Management.Common;

public class EventSource
{
	[Flags]
	public enum FilterTypes
	{
		None = 0,
		Ptz = 1,
		MIP = 2
	}

	public string Type { get; set; }

	public Collection<EventSourceFilter> Filters { get; private set; }

	public FilterTypes Filter
	{
		get
		{
			return Filters.Select((EventSourceFilter f) => f.FilterType).Aggregate(FilterTypes.None, (FilterTypes f1, FilterTypes f2) => f1 | f2);
		}
		set
		{
			Filters = new Collection<EventSourceFilter>((from FilterTypes f in Enum.GetValues(typeof(FilterTypes))
				where f != FilterTypes.None && value.HasFlag(f)
				select new EventSourceFilter(f)).ToArray());
		}
	}

	public EventSource(string type)
		: this(type, FilterTypes.None)
	{
	}

	public EventSource(string type, FilterTypes filter)
	{
		Type = type;
		Filter = filter;
	}

	public EventSource(string type, params EventSourceFilter[] filters)
	{
		Type = type;
		Filters = new Collection<EventSourceFilter>(filters);
	}
}
