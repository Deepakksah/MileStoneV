using System;

namespace VideoOS.Platform.Live;

public class LiveContentEventArgs : BaseLiveContentEventArgs
{
	public LiveSourceContent LiveContent;

	public new Exception Exception;
}
