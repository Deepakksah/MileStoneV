namespace VideoOS.Platform.SDK.Interface;

public interface IMarkedDataGetCompletedEventArgs : IAsyncCompletedEventArgs
{
	IMarkedData Result { get; }
}
