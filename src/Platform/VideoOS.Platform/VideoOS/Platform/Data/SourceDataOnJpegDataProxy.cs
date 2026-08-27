using System;

namespace VideoOS.Platform.Data;

public class SourceDataOnJpegDataProxy : ISourceData
{
	private readonly JPEGData _jpegData;

	protected JPEGData JpegData => _jpegData;

	public DateTime DateTime
	{
		get
		{
			return _jpegData.DateTime;
		}
		set
		{
			_jpegData.DateTime = value;
		}
	}

	public bool IsNextAvailable
	{
		get
		{
			return _jpegData.IsNextAvailable;
		}
		set
		{
			_jpegData.IsNextAvailable = value;
		}
	}

	public DateTime NextDateTime
	{
		get
		{
			return _jpegData.NextDateTime;
		}
		set
		{
			_jpegData.NextDateTime = value;
		}
	}

	public bool IsPreviousAvailable
	{
		get
		{
			return _jpegData.IsPreviousAvailable;
		}
		set
		{
			_jpegData.IsPreviousAvailable = value;
		}
	}

	public DateTime PreviousDateTime
	{
		get
		{
			return _jpegData.PreviousDateTime;
		}
		set
		{
			_jpegData.PreviousDateTime = value;
		}
	}

	public SourceDataOnJpegDataProxy(JPEGData jpegData)
	{
		_jpegData = jpegData;
	}
}
