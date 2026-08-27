using System;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Video;

public class VideoPullProxy : IVideo, IControl
{
	private readonly IVideo _video;

	private readonly IVideoSub _videoSub;

	private CycleTaskMember _pullCycleTaskMember;

	private readonly object _pullCycleLocker = new object();

	private readonly int _timeoutMs;

	private DateTime _frameRequestTime = DateTime.UtcNow;

	private DateTime _frameReceivedTime = DateTime.UtcNow;

	public OnVideoFrame NewFrame { get; set; }

	public VideoPullProxy(IVideo video, IVideoSub videoSub, int fps = 1)
	{
		_video = video;
		_videoSub = videoSub;
		_video.NewFrame = OnVideoFrame;
		_timeoutMs = 1000 / fps;
		Start();
	}

	private void GetFrameCycleTask()
	{
		DateTime utcNow = DateTime.UtcNow;
		if ((utcNow - _frameRequestTime).TotalMilliseconds >= (double)_timeoutMs && _frameReceivedTime > _frameRequestTime)
		{
			_videoSub.GetNextFrame();
			_frameRequestTime = utcNow;
		}
	}

	private void OnVideoFrame(VideoFrame frame)
	{
		_frameReceivedTime = DateTime.UtcNow.AddMilliseconds(1.0);
		NewFrame?.Invoke(frame);
	}

	public void Start()
	{
		lock (_pullCycleLocker)
		{
			if (_pullCycleTaskMember == null)
			{
				_pullCycleTaskMember = new CycleTaskMember((Action)GetFrameCycleTask, (Action)null, (Action)null, 10);
				_pullCycleTaskMember.StartTask();
			}
		}
	}

	public void Stop()
	{
		lock (_pullCycleLocker)
		{
			_pullCycleTaskMember?.SignalTaskToStop();
			_pullCycleTaskMember?.Dispose();
			_pullCycleTaskMember = null;
		}
	}
}
