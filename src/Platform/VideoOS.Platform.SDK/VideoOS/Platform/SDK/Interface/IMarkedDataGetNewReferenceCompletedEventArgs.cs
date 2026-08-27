namespace VideoOS.Platform.SDK.Interface;

public interface IMarkedDataGetNewReferenceCompletedEventArgs : IAsyncCompletedEventArgs
{
	IMarkedDataReference Result { get; }
}
