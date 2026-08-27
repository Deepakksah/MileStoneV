using System;

namespace VideoOS.Platform.Live;

public class LiveContentPcmEventArgs : BaseLiveContentEventArgs
{
	public LiveSourcePcmContent LiveContent;

	public new Exception Exception;
}
