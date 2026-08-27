using System;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class PlaybackStatus
{
	public EventHandler OnStatusChange;

	private PlaybackFlags _futureStatus;

	private bool _inParamsChange;

	public PlaybackFlags Status { get; private set; }

	public PlaybackStatus()
	{
		Status = PlaybackFlags.Nothing;
		_futureStatus = PlaybackFlags.Nothing;
		_inParamsChange = false;
	}

	public void BeginChange()
	{
		if (!_inParamsChange)
		{
			_futureStatus = Status;
		}
		_inParamsChange = true;
	}

	public void EndChange(bool force = false)
	{
		if (_inParamsChange)
		{
			PlaybackFlags playbackFlags = _futureStatus ^ Status;
			if (playbackFlags != PlaybackFlags.Nothing || force)
			{
				Status = _futureStatus;
				PlaybackStatusEventArgs e = new PlaybackStatusEventArgs
				{
					ChangedStatusFlags = playbackFlags,
					CurrentStatusFlags = _futureStatus
				};
				if (OnStatusChange != null)
				{
					OnStatusChange(this, e);
				}
			}
		}
		_inParamsChange = false;
	}

	public void SetFlag(PlaybackFlags setFlag, bool force = false)
	{
		SetFlag(setFlag, setFlag, force);
	}

	public void SetFlag(PlaybackFlags setFlag, PlaybackFlags setMask, bool force = false)
	{
		bool num = !_inParamsChange;
		if (num)
		{
			BeginChange();
		}
		_futureStatus &= ~setMask;
		_futureStatus |= setFlag;
		if (num)
		{
			EndChange(force);
		}
	}

	public void ResetFlag(PlaybackFlags resetFlag)
	{
		ResetFlag(resetFlag, resetFlag);
	}

	public void ResetFlag(PlaybackFlags resetFlag, PlaybackFlags resetMask)
	{
		bool num = !_inParamsChange;
		if (num)
		{
			BeginChange();
		}
		_futureStatus &= ~resetFlag;
		if (num)
		{
			EndChange();
		}
	}

	public bool IsSetFlag(PlaybackFlags setFlag)
	{
		return (Status & setFlag) == setFlag;
	}

	public bool IsPartiallySetFlag(PlaybackFlags setFlag)
	{
		return (Status & setFlag) != 0;
	}
}
