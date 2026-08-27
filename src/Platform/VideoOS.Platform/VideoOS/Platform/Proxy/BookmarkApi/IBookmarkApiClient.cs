using System;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Proxy.BookmarkApi;

public interface IBookmarkApiClient
{
	void Initialize(LoginSettings loginSettings, bool debugLog);

	void Close();

	BookmarkDataModel BookmarkGet(Guid bookmarkId);

	NewBookmarkReferenceDataModel BookmarkGetNewReference(Guid deviceId, bool live);

	BookmarkDataModel BookmarkCreate(Guid deviceId, DateTime? timeBegin, DateTime timeTrigged, DateTime? timeEnd, string reference, string header, string description);

	void BookmarkDelete(Guid bookmarkId);

	void BookmarkUpdate(Guid bookmarkId, DateTime? timeBegin, DateTime? timeTrigged, DateTime? timeEnd, string reference, string header, string description);

	BookmarkDataModel[] BookmarkSearchTime(DateTime time, TimeSpan timeLimit, int countLimit, string[] resourceTypes, Guid[] deviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerContains, string descriptionContains);

	BookmarkDataModel[] BookmarkSearchFromBookmark(string fromBookmarkId, TimeSpan timeLimit, int countLimit, string[] resourceTypes, Guid[] deviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerContains, string descriptionContains);
}
