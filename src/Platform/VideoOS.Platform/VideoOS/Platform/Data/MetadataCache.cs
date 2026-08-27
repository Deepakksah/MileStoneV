using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.Data;

public class MetadataCache
{
	private static readonly List<MetadataPlaybackData> TheEmptyCache = new List<MetadataPlaybackData>();

	private List<MetadataPlaybackData> _data = TheEmptyCache;

	private int? _indexOflastReturnedEntry;

	public MetadataPlaybackData FirstEntry
	{
		get
		{
			_indexOflastReturnedEntry = 0;
			return _data.FirstOrDefault();
		}
	}

	public MetadataPlaybackData LastEntry
	{
		get
		{
			_indexOflastReturnedEntry = _data.Count - 1;
			return _data.LastOrDefault();
		}
	}

	public void SetCacheContents(IEnumerable<MetadataPlaybackData> data)
	{
		_indexOflastReturnedEntry = null;
		_data = new List<MetadataPlaybackData>(data);
		_data.Sort((MetadataPlaybackData elem1, MetadataPlaybackData elem2) => elem1.DateTime.CompareTo(elem2.DateTime));
	}

	public MetadataPlaybackData FindCacheEntryAfter(DateTime timeStamp)
	{
		if (TimestampIsOutsideOfCache(timeStamp) || _data[_data.Count - 1].DateTime == timeStamp)
		{
			_indexOflastReturnedEntry = null;
			return null;
		}
		for (int i = 0; i < _data.Count; i++)
		{
			if (_data[i].DateTime > timeStamp)
			{
				_indexOflastReturnedEntry = i;
				return _data[i];
			}
		}
		return null;
	}

	public MetadataPlaybackData FindCacheEntryBefore(DateTime timeStamp)
	{
		if (TimestampIsOutsideOfCache(timeStamp) || _data[0].DateTime == timeStamp)
		{
			_indexOflastReturnedEntry = null;
			return null;
		}
		for (int num = _data.Count - 1; num >= 0; num--)
		{
			MetadataPlaybackData metadataPlaybackData = _data[num];
			if (metadataPlaybackData.DateTime < timeStamp)
			{
				_indexOflastReturnedEntry = num;
				return metadataPlaybackData;
			}
		}
		return null;
	}

	public MetadataPlaybackData GetNext()
	{
		if (!_indexOflastReturnedEntry.HasValue)
		{
			return null;
		}
		_indexOflastReturnedEntry++;
		if (_indexOflastReturnedEntry >= _data.Count)
		{
			_indexOflastReturnedEntry = null;
			return null;
		}
		return _data[_indexOflastReturnedEntry.Value];
	}

	public MetadataPlaybackData GetPrevious()
	{
		if (!_indexOflastReturnedEntry.HasValue)
		{
			return null;
		}
		_indexOflastReturnedEntry--;
		if (_indexOflastReturnedEntry < 0)
		{
			_indexOflastReturnedEntry = null;
			return null;
		}
		return _data[_indexOflastReturnedEntry.Value];
	}

	public MetadataPlaybackData GetNearest(DateTime timestamp)
	{
		if (TimestampIsOutsideOfCache(timestamp))
		{
			_indexOflastReturnedEntry = null;
			return null;
		}
		for (int i = 0; i < _data.Count - 1; i++)
		{
			DateTime dateTime = _data[i].DateTime;
			DateTime dateTime2 = _data[i + 1].DateTime;
			if (timestamp >= dateTime && timestamp <= dateTime2)
			{
				TimeSpan timeSpan = timestamp - dateTime;
				TimeSpan timeSpan2 = dateTime2 - timestamp;
				if (timeSpan < timeSpan2)
				{
					return _data[i];
				}
				return _data[i + 1];
			}
		}
		return null;
	}

	public MetadataPlaybackData GetAtOrBefore(DateTime timestamp)
	{
		if (TimestampIsOutsideOfCache(timestamp))
		{
			_indexOflastReturnedEntry = null;
			return null;
		}
		for (int num = _data.Count - 1; num >= 0; num--)
		{
			if (timestamp >= _data[num].DateTime)
			{
				return _data[num];
			}
		}
		return null;
	}

	public void Clear()
	{
		_data = TheEmptyCache;
	}

	private bool TimestampIsOutsideOfCache(DateTime timeStamp)
	{
		if (_data.Count == 0)
		{
			return true;
		}
		if (timeStamp < _data.First().DateTime)
		{
			return true;
		}
		if (timeStamp > _data.Last().DateTime)
		{
			return true;
		}
		return false;
	}
}
