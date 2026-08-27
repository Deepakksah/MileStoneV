using System;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Platform.Admin;

public class AdminImageViewerControl : UserControl
{
	private AdminImageState _state = AdminImageState.Closed;

	public virtual AdminImageState State
	{
		get
		{
			return _state;
		}
		set
		{
			if (_state != value)
			{
				_state = value;
				FireStateChangedEvent();
			}
		}
	}

	public virtual bool Pause { get; set; }

	public virtual Image Image { get; set; }

	public virtual UserControl ImageUserControl { get; private set; }

	private event FrameReceivedHandler _frameReceivedEvent;

	private event EventHandler _stateChangedEvent;

	public virtual event FrameReceivedHandler FrameReceivedEvent
	{
		add
		{
			_frameReceivedEvent += value;
		}
		remove
		{
			_frameReceivedEvent -= value;
		}
	}

	public virtual event EventHandler StateChangedEvent
	{
		add
		{
			_stateChangedEvent += value;
		}
		remove
		{
			_stateChangedEvent -= value;
		}
	}

	public virtual bool OpenVideo(Guid cameraId)
	{
		throw new MIPException("Please use the ClientControl.Instance.GenerateAdminImageViewerControl(); method to create an instance of this class.");
	}

	public virtual void CloseVideo()
	{
	}

	public void FireFrameReceivedEvent(int width, int height, int size, int count)
	{
		if (this._frameReceivedEvent != null)
		{
			this._frameReceivedEvent(this, new VideoInfoArgs
			{
				Width = width,
				Height = height,
				DataSize = size,
				Count = count
			});
		}
	}

	public void FireStateChangedEvent()
	{
		if (this._stateChangedEvent != null)
		{
			this._stateChangedEvent(this, new EventArgs());
		}
	}
}
