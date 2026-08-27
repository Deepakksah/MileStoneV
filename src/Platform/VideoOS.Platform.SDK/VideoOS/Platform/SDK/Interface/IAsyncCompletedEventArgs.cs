using System;

namespace VideoOS.Platform.SDK.Interface;

public interface IAsyncCompletedEventArgs
{
	bool Cancelled { get; }

	Exception Error { get; }

	object UserState { get; }
}
