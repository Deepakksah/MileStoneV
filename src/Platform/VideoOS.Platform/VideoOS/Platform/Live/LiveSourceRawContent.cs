using System;

namespace VideoOS.Platform.Live;

public class LiveSourceRawContent : IDisposable
{
	public virtual byte[] Content { get; set; }

	public virtual string PrivacyMask { get; set; }

	public IDisposable ObjectToDispose { get; set; }

	public RawLiveSource RawLiveSource { get; set; }

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (ObjectToDispose != null)
			{
				ObjectToDispose.Dispose();
				ObjectToDispose = null;
			}
			if (RawLiveSource != null)
			{
				RawLiveSource.FinishedProcessingFrame();
				RawLiveSource = null;
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	~LiveSourceRawContent()
	{
		Dispose(disposing: false);
	}
}
