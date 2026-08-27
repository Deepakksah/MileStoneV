using System;

namespace VideoOS.Toolkit;

public interface ILiveSourceToolkit : ISourceToolkit
{
	public enum GetLiveStatus
	{
		LIVE_DATA_RETRIEVED,
		LIVE_REQUEST_TIMED_OUT
	}

	void StartLiveStream();

	void StopLiveStream();

	GetLiveStatus GetLiveData(out IData data, TimeSpan timeout);

	GetLiveStatus GetLiveData(out IData data);
}
