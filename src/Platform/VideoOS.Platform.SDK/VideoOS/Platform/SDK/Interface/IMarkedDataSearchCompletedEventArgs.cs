namespace VideoOS.Platform.SDK.Interface;

public interface IMarkedDataSearchCompletedEventArgs : IAsyncCompletedEventArgs
{
	IMarkedData[] Result { get; }
}
