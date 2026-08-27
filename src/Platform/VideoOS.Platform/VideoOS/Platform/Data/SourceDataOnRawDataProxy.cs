using System;

namespace VideoOS.Platform.Data;

public class SourceDataOnRawDataProxy : ISourceData
{
	private readonly RawVideoSourceData _rawData;

	protected RawVideoSourceData RawVideoSourceData => _rawData;

	public DateTime DateTime
	{
		get
		{
			return _rawData.DateTime;
		}
		set
		{
			_rawData.DateTime = value;
		}
	}

	public bool IsNextAvailable
	{
		get
		{
			return _rawData.IsNextAvailable;
		}
		set
		{
			_rawData.IsNextAvailable = value;
		}
	}

	public DateTime NextDateTime
	{
		get
		{
			return _rawData.NextDateTime;
		}
		set
		{
			_rawData.NextDateTime = value;
		}
	}

	public bool IsPreviousAvailable
	{
		get
		{
			return _rawData.IsPreviousAvailable;
		}
		set
		{
			_rawData.IsPreviousAvailable = value;
		}
	}

	public DateTime PreviousDateTime
	{
		get
		{
			return _rawData.PreviousDateTime;
		}
		set
		{
			_rawData.PreviousDateTime = value;
		}
	}

	public SourceDataOnRawDataProxy(RawVideoSourceData rawData)
	{
		_rawData = rawData;
	}
}
