using System;
using VideoOS.Platform.Util.Pcm;

namespace VideoOS.Platform.Live;

public class LiveSourcePcmContent : IDisposable
{
	public DateTime BeginTime { get; set; }

	public DateTime EndTime { get; set; }

	public virtual byte[] Content { get; set; }

	public IDisposable ObjectToDispose { get; set; }

	public PcmLiveSource PcmLiveSource { get; set; }

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (ObjectToDispose != null)
			{
				ObjectToDispose.Dispose();
				ObjectToDispose = null;
			}
			if (PcmLiveSource != null)
			{
				PcmLiveSource.FinishedProcessingFrame();
				PcmLiveSource = null;
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	~LiveSourcePcmContent()
	{
		Dispose(disposing: false);
	}

	public void CorrectTimeStamps()
	{
		if (PcmLiveSource != null && !(DateTime.MinValue == BeginTime))
		{
			EndTime = BeginTime.AddMilliseconds(PcmHelper.BytesCountToMilliseconds(Content.Length, (int)PcmLiveSource.PcmSourceSettings.SamplingRate, (int)PcmLiveSource.PcmSourceSettings.Channels, (int)PcmLiveSource.PcmSourceSettings.BitsPerSample >> 3));
		}
	}
}
