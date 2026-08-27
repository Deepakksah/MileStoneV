using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Data;

public class PcmAudioDataList : ISourceData, IPcmAudioData
{
	public IEnumerable<PcmAudioData> DataList { get; private set; }

	public byte[] AccumulatedData => GetAccumulatedData();

	public TimeSpan Duration => TimeSpan.FromMilliseconds(DataList.Select((PcmAudioData data) => data.Duration.TotalMilliseconds).Sum());

	public DateTime EndDateTime => DateTime + Duration;

	public IPcmSourceSettings PcmSourceSettings { get; set; }

	public DateTime DateTime
	{
		get
		{
			return DataList.First().DateTime;
		}
		set
		{
			throw new NotSupportedMIPException();
		}
	}

	public bool IsNextAvailable
	{
		get
		{
			return DataList.Last().IsNextAvailable;
		}
		set
		{
			throw new NotSupportedMIPException();
		}
	}

	public DateTime NextDateTime
	{
		get
		{
			return DataList.Last().NextDateTime;
		}
		set
		{
			throw new NotSupportedMIPException();
		}
	}

	public bool IsPreviousAvailable
	{
		get
		{
			return DataList.First().IsPreviousAvailable;
		}
		set
		{
			throw new NotSupportedMIPException();
		}
	}

	public DateTime PreviousDateTime
	{
		get
		{
			return DataList.First().PreviousDateTime;
		}
		set
		{
			throw new NotSupportedMIPException();
		}
	}

	public PcmAudioDataList(IEnumerable<PcmAudioData> dataList)
	{
		DataList = dataList;
	}

	private byte[] GetAccumulatedData()
	{
		return ByteArrayHelper.Combine(DataList.Select((PcmAudioData pcmAudioData) => pcmAudioData.Data).ToArray());
	}
}
