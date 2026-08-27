namespace VideoOS.Platform.SDK.Interface;

public interface IBookmarkGetNewReferenceCompletedEventArgs : IAsyncCompletedEventArgs
{
	IBookmarkReference Result { get; }
}
