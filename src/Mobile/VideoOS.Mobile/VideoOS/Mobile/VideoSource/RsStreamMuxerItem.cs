using System.Collections.Generic;

namespace VideoOS.Mobile.VideoSource;

public class RsStreamMuxerItem<TData>
{
	private List<TData> _dataList = new List<TData>();

	private readonly object _dataListLocker = new object();

	private int _missingCount;

	public IRsStream<TData> Source { get; private set; }

	public IRsStreamMuxerItemConsumer<TData> ConsumerIface
	{
		get
		{
			lock (_dataListLocker)
			{
				CalculateMissingCount();
				List<TData> dataList = _dataList;
				_dataList = new List<TData>();
				return new RsStreamMuxerItemConsumer<TData>
				{
					DataList = dataList,
					MissingCount = _missingCount
				};
			}
		}
		set
		{
			lock (_dataListLocker)
			{
				List<TData> list = new List<TData>(value.DataList);
				list.AddRange(_dataList);
				_dataList = list;
			}
		}
	}

	public RsStreamMuxerItem(IRsStream<TData> source)
	{
		Source = source;
		Source.OnData = OnStreamData;
	}

	private void OnStreamData(IEnumerable<TData> data)
	{
		lock (_dataListLocker)
		{
			_dataList.AddRange(data);
		}
	}

	private void CalculateMissingCount()
	{
		if (_dataList.Count == 0)
		{
			_missingCount++;
		}
		else
		{
			_missingCount = 0;
		}
	}
}
