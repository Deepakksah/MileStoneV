using System;
using System.Collections.Generic;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Live;

public class LiveSourceContent : IDisposable
{
	private string _hardwareDecodingStatus = "ipp";

	private int _width;

	private int _height;

	public DateTime BeginTime { get; set; }

	public DateTime EndTime { get; set; }

	public byte[] Content { get; set; }

	public Dictionary<string, string> Properties { get; set; }

	public virtual string HardwareDecodingStatus
	{
		get
		{
			return _hardwareDecodingStatus;
		}
		set
		{
			_hardwareDecodingStatus = value;
		}
	}

	public int Width
	{
		get
		{
			if (_width == 0)
			{
				JpegUtil.FindWidthHeight(Content, out _width, out _height);
			}
			return _width;
		}
	}

	public int Height
	{
		get
		{
			if (_height == 0)
			{
				JpegUtil.FindWidthHeight(Content, out _width, out _height);
			}
			return _height;
		}
	}

	public bool CroppingDefined { get; set; }

	public double CropX { get; set; }

	public double CropY { get; set; }

	public double CropWidth { get; set; }

	public double CropHeight { get; set; }

	public object CustomData { get; set; }

	public LiveSource Source { get; set; }

	[Obsolete]
	public void Dispose(bool disposing)
	{
	}

	public virtual void Dispose()
	{
		if (Source != null)
		{
			Source.FinishedProcessingFrame();
			Source = null;
		}
		GC.SuppressFinalize(this);
	}

	~LiveSourceContent()
	{
		Dispose();
	}
}
