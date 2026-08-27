using System;

namespace VideoOS.Platform.Data;

public class SourceDataOnBitmapDataProxy : ISourceData
{
	private readonly BitmapData _bitmapData;

	protected BitmapData BitmapData => _bitmapData;

	public DateTime DateTime
	{
		get
		{
			return _bitmapData.DateTime;
		}
		set
		{
			_bitmapData.DateTime = value;
		}
	}

	public bool IsNextAvailable
	{
		get
		{
			return _bitmapData.IsNextAvailable;
		}
		set
		{
			_bitmapData.IsNextAvailable = value;
		}
	}

	public DateTime NextDateTime
	{
		get
		{
			return _bitmapData.NextDateTime;
		}
		set
		{
			_bitmapData.NextDateTime = value;
		}
	}

	public bool IsPreviousAvailable
	{
		get
		{
			return _bitmapData.IsPreviousAvailable;
		}
		set
		{
			_bitmapData.IsPreviousAvailable = value;
		}
	}

	public DateTime PreviousDateTime
	{
		get
		{
			return _bitmapData.PreviousDateTime;
		}
		set
		{
			_bitmapData.PreviousDateTime = value;
		}
	}

	public SourceDataOnBitmapDataProxy(BitmapData bitmapData)
	{
		_bitmapData = bitmapData;
	}
}
