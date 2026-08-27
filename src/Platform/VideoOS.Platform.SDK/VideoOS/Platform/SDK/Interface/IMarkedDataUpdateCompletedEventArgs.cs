namespace VideoOS.Platform.SDK.Interface;

public interface IMarkedDataUpdateCompletedEventArgs : IAsyncCompletedEventArgs
{
	IMarkedDataResult Result { get; }
}
