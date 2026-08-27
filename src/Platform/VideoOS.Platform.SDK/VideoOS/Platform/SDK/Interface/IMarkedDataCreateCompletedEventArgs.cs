namespace VideoOS.Platform.SDK.Interface;

public interface IMarkedDataCreateCompletedEventArgs : IAsyncCompletedEventArgs
{
	IMarkedDataResult Result { get; }
}
