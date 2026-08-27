namespace VideoOS.Platform.SDK.Interface;

public interface IBookmarkCreateCompletedEventArgs : IAsyncCompletedEventArgs
{
	IBookmark Result { get; }
}
