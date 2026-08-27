using System;

namespace VideoOS.Platform.SDK.Interface;

public interface ICreatePresetCompletedEventArgs : IAsyncCompletedEventArgs
{
	Guid Result { get; }
}
