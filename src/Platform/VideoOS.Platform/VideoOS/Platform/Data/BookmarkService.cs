using System;
using System.Diagnostics;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Data;

public class BookmarkService
{
	public class BookmarkSearchCompletedEventArgs : AsyncResultEventArgs
	{
		public Exception Exception { get; public set; }

		public bool Cancelled { get; public set; }

		public new Bookmark[] Result => (Bookmark[])base.Result;

		public BookmarkSearchCompletedEventArgs(Bookmark[] results, Exception exception, bool cancelled, object userState)
		{
			base.Result = results;
			Exception = exception;
			Cancelled = cancelled;
			AsyncState = userState;
		}
	}

	private static BookmarkService _instance;

	private static bool _telemetryDataSent;

	public static BookmarkService Instance
	{
		[DebuggerStepThrough]
		get
		{
			if (_instance == null)
			{
				_instance = new BookmarkService();
			}
			if (!_telemetryDataSent)
			{
				new TrackPageViewData(typeof(BookmarkService)).Send();
				_telemetryDataSent = true;
			}
			return _instance;
		}
		set
		{
			_instance = value;
		}
	}

	public virtual Bookmark BookmarkCreate(FQID deviceFqid, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return null;
	}

	public virtual Bookmark[] BookmarkSearchTime(ServerId serverId, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr)
	{
		return null;
	}

	public virtual Bookmark[] BookmarkSearchTime(ServerId serverId, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return null;
	}

	public virtual Bookmark[] BookmarkSearchTime(UserContext userContext, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr)
	{
		throw new NotImplementedException();
	}

	public virtual Bookmark[] BookmarkSearchTime(UserContext userContext, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		throw new NotImplementedException();
	}

	public virtual Bookmark[] BookmarkSearchFromBookmark(FQID fromBookmarkFQID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr)
	{
		return null;
	}

	public virtual Bookmark[] BookmarkSearchFromBookmark(FQID fromBookmarkFQID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return null;
	}

	public virtual Bookmark[] BookmarkSearchFromBookmark(UserContext userContext, Guid fromBookmarkID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr)
	{
		throw new NotImplementedException();
	}

	public virtual Bookmark[] BookmarkSearchFromBookmark(UserContext userContext, Guid fromBookmarkID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		throw new NotImplementedException();
	}

	public virtual void BookmarkSearchTimeAsync(ServerId serverId, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, AsyncCallbackHandler resultCallbackEventHandler, object asyncState)
	{
	}

	public virtual void BookmarkSearchTimeAsync(ServerId serverId, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, AsyncCallbackHandler resultCallbackEventHandler, object asyncState)
	{
	}

	public virtual void BookmarkSearchFromBookmarkAsync(FQID fromBookmarkFQID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, AsyncCallbackHandler resultCallbackEventHandler, object asyncState)
	{
	}

	public virtual void BookmarkSearchFromBookmarkAsync(FQID fromBookmarkFQID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, AsyncCallbackHandler resultCallbackEventHandler, object asyncState)
	{
	}

	public virtual Bookmark BookmarkGet(FQID bookmarkFQID)
	{
		return null;
	}

	public virtual Bookmark BookmarkGet(UserContext userContext, Guid objectId)
	{
		throw new NotImplementedException();
	}

	public virtual Bookmark BookmarkUpdate(Bookmark bookmark)
	{
		return null;
	}

	public virtual void BookmarkDelete(FQID bookmarkFQID)
	{
	}

	public virtual void BookmarkDelete(Bookmark bookmark)
	{
	}

	public virtual BookmarkReference BookmarkGetNewReference(FQID deviceFQID, bool live)
	{
		return null;
	}
}
