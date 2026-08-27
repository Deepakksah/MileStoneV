using System;
using System.Collections.ObjectModel;
using System.Linq;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.Client;

public class SequenceCache
{
	private DateTime _startCache = DateTime.MaxValue;

	private DateTime _endCache = DateTime.MinValue;

	private Collection<SequenceData> _cache = new Collection<SequenceData>();

	public DateTime StartCache => _startCache;

	public DateTime EndCache => _endCache;

	public ReadOnlyCollection<SequenceData> Items => new ReadOnlyCollection<SequenceData>(_cache);

	public void Clear()
	{
		_startCache = DateTime.MaxValue;
		_endCache = DateTime.MinValue;
		_cache.Clear();
	}

	public void Add(SequenceData data)
	{
		if (_cache.Any() && _cache.Last().EventSequence.StartDateTime == data.EventSequence.StartDateTime && _cache.Last().EventSequence.EndDateTime <= data.EventSequence.EndDateTime)
		{
			_cache.Remove(_cache.Last());
		}
		_cache.Add(data);
		DateTime dateTime = data.EventSequence.StartDateTime.ToLocalTime();
		DateTime dateTime2 = data.EventSequence.EndDateTime.ToLocalTime();
		if (dateTime2 > DateTime.Now)
		{
			dateTime2 = DateTime.Now;
		}
		if (dateTime < _startCache)
		{
			_startCache = dateTime;
		}
		if (dateTime2 > _endCache)
		{
			_endCache = dateTime2;
		}
	}
}
