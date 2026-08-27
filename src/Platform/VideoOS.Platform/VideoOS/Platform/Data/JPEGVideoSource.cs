using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Data;

public class JPEGVideoSource : VideoSource
{
	private readonly JPEGVideoSourceImplementer _implementer;

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

	public bool AllowUpscaling
	{
		get
		{
			return _implementer.AllowUpscaling;
		}
		set
		{
			_implementer.AllowUpscaling = value;
		}
	}

	public int Compression
	{
		get
		{
			return _implementer.Compression;
		}
		set
		{
			_implementer.Compression = value;
		}
	}

	public object CustomData
	{
		get
		{
			return _implementer.CustomData;
		}
		set
		{
			_implementer.CustomData = value;
		}
	}

	public JPEGVideoSource(Item item)
		: base(item)
	{
		_implementer = new JPEGVideoSourceImplementer(item);
	}

	public void SetKeepAspectRatio(bool keepAspectRatio, bool fillSpace)
	{
		_implementer.SetKeepAspectRatio(keepAspectRatio, fillSpace);
	}

	public override void Init()
	{
		Init(Width, Height);
	}

	public override void Init(int width, int height)
	{
		Width = width;
		Height = height;
		_implementer.Init();
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

	public override void Close()
	{
		_implementer.Close();
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

	public JPEGData GetPrevious()
	{
		return _implementer.GetPrevious();
	}

	public virtual JPEGData GetPreviousSequence()
	{
		return _implementer.GetPreviousSequence();
	}

	public virtual JPEGData GetNextSequence()
	{
		return _implementer.GetNextSequence();
	}

	public virtual JPEGData GetBegin()
	{
		return _implementer.GetBegin();
	}

	public virtual JPEGData GetEnd()
	{
		return _implementer.GetEnd();
	}
}
