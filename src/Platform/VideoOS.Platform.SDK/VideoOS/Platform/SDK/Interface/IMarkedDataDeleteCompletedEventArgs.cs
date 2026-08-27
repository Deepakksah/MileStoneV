namespace VideoOS.Platform.SDK.Interface;

public interface IMarkedDataDeleteCompletedEventArgs : IAsyncCompletedEventArgs
{
	IMarkedDataResult[] Result { get; }
}
