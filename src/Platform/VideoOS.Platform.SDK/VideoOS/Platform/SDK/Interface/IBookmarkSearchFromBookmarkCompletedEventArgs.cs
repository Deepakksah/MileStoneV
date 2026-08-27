namespace VideoOS.Platform.SDK.Interface;

public interface IBookmarkSearchFromBookmarkCompletedEventArgs : IAsyncCompletedEventArgs
{
	IBookmark[] Result { get; }
}
