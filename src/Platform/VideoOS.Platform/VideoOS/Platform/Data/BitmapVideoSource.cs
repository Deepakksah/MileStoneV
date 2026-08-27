using System;
using System.Collections.Generic;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Data;

public class BitmapVideoSource : VideoSource
{
	private readonly BitmapVideoSourceImplementer _implementer;

	public object SessionId => _implementer.SessionId;

	public int Height
	{
		get
		{
			return _implementer.Height;
		}
		set
		{
			_implementer.Height = value;
		}
	}

	public int Width
	{
		get
		{
			return _implementer.Width;
		}
		set
		{
			_implementer.Width = value;
		}
	}

	public BitmapVideoSource(Item item)
		: base(item)
	{
		_implementer = new BitmapVideoSourceImplementer(item);
		new TrackPageViewData(typeof(BitmapVideoSource)).Send();
	}

	public override void Init()
	{
		Init(Width, Height);
	}

	public override void Init(int width, int height)
	{
		_implementer.Init(width, height, BitmapFormat.BGR24);
	}

	public virtual void Init(int width, int height, BitmapFormat format)
	{
		_implementer.Init(width, height, format);
	}

	public override void Close()
	{
		_implementer.Close();
	}

	public void SetKeepAspectRatio(bool keepAspectRatio, bool fillSpace)
	{
		_implementer.SetKeepAspectRatio(keepAspectRatio, fillSpace);
	}

	public void SuspendSettingsUpdate()
	{
		_implementer.SuspendSettingsUpdate();
	}

	public void ResumeSettingsUpdate()
	{
		_implementer.ResumeSettingsUpdate();
	}

	public void SetWidthHeight()
	{
		_implementer.SetWidthHeight();
	}

	public void SetCropRectangle(double x, double y, double width, double height)
	{
		_implementer.SetCropRectangle(x, y, width, height);
	}

	public override object Get(DateTime dateTime)
	{
		return _implementer.Get(dateTime);
	}

	public override List<object> Get(DateTime dateTime, TimeSpan maxTimeAfter, int maxCountAfter)
	{
		return _implementer.Get(dateTime, maxTimeAfter, maxCountAfter);
	}

	public override object GetNearest(DateTime dateTime)
	{
		return _implementer.GetNearest(dateTime);
	}

	public override object GetAtOrBefore(DateTime dateTime)
	{
		return _implementer.GetAtOrBefore(dateTime);
	}

	public bool GoToWithResult(DateTime timestamp, string playbackPlayModeData)
	{
		return _implementer.GoToWithResult(timestamp, playbackPlayModeData);
	}

	public void GoTo(DateTime timestamp, string playbackPlayModeData)
	{
		_implementer.GoTo(timestamp, playbackPlayModeData);
	}

	public override object GetNext()
	{
		return _implementer.GetNext();
	}

	public BitmapData GetPrevious()
	{
		return _implementer.GetPrevious();
	}

	public BitmapData GetPreviousSequence()
	{
		return _implementer.GetPreviousSequence();
	}

	public BitmapData GetNextSequence()
	{
		return _implementer.GetNextSequence();
	}

	public BitmapData GetBegin()
	{
		return _implementer.GetBegin();
	}

	public BitmapData GetEnd()
	{
		return _implementer.GetEnd();
	}
}
