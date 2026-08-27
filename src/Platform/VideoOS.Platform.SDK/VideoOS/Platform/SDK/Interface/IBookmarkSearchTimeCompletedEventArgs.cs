namespace VideoOS.Platform.SDK.Interface;

public interface IBookmarkSearchTimeCompletedEventArgs : IAsyncCompletedEventArgs
{
	IBookmark[] Result { get; }
}
