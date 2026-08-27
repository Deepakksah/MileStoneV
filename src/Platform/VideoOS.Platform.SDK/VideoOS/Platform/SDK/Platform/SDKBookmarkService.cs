using System;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.Data;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Platform;

public class SDKBookmarkService : BookmarkService
{
	private class BookmarkSearchData
	{
		public ServerId ServerId;

		public AsyncCallbackHandler AsyncHandler;

		public object AsyncState;

		public BookmarkSearchData(ServerId serverId, AsyncCallbackHandler asyncHdlr, object asyncState)
		{
			ServerId = serverId;
			AsyncHandler = asyncHdlr;
			AsyncState = asyncState;
		}
	}

	public override VideoOS.Platform.Data.Bookmark BookmarkCreate(FQID deviceFqid, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		ILoginServer loginServer = GetLoginServer(deviceFqid);
		if (loginServer == null)
		{
			return null;
		}
		_ = loginServer.LoginInfo.Token;
		DateTime utcNow = DateTime.UtcNow;
		if (timeBegin == DateTime.MinValue)
		{
			timeBegin = utcNow.AddSeconds(-10.0);
		}
		if (timeTrigged == DateTime.MinValue)
		{
			timeTrigged = utcNow;
			if (timeTrigged > utcNow)
			{
				timeTrigged = utcNow;
			}
		}
		if (timeEnd == DateTime.MinValue || timeEnd == DateTime.MaxValue)
		{
			timeEnd = timeTrigged.AddSeconds(10.0);
			if (timeEnd > utcNow)
			{
				timeEnd = utcNow;
			}
		}
		if (reference == null)
		{
			IBookmarkReference bookmarkReference = loginServer.BookmarkGetNewReference(deviceFqid.ObjectId, live: true);
			if (bookmarkReference == null)
			{
				return null;
			}
			reference = bookmarkReference.Reference;
		}
		IBookmark bookmark = loginServer.BookmarkCreate(deviceFqid.ObjectId, timeBegin, timeTrigged, timeEnd, reference, header, description);
		if (bookmark == null)
		{
			return null;
		}
		FQID bookmarkFQID = new FQID(GetServerId(deviceFqid.ServerId), deviceFqid.ObjectId, bookmark.Id, FolderType.No, Kind.Bookmark);
		return new VideoOS.Platform.Data.Bookmark
		{
			BookmarkFQID = bookmarkFQID,
			Description = bookmark.Description,
			Header = bookmark.Header,
			Reference = bookmark.Reference,
			TimeBegin = bookmark.TimeBegin,
			TimeEnd = bookmark.TimeEnd,
			TimeTrigged = bookmark.TimeTrigged,
			User = bookmark.User
		};
	}

	public override VideoOS.Platform.Data.Bookmark[] BookmarkSearchTime(ServerId serverId, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr)
	{
		ILoginServer loginServer = GetLoginServer(serverId);
		if (loginServer == null)
		{
			return null;
		}
		IMediaDeviceType[] deviceTypes = DeviceType2SMS(kinds);
		TimeDuration timeLimit = new TimeDuration
		{
			MicroSeconds = timeLimitUSec
		};
		IBookmark[] array = loginServer.BookmarkSearchTime(time, timeLimit, countLimit, deviceTypes, FQIDToDeviceId(fqids), optUsers, optSearchStr);
		if (array == null)
		{
			return null;
		}
		VideoOS.Platform.Data.Bookmark[] array2 = new VideoOS.Platform.Data.Bookmark[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			IBookmark bookmark = array[i];
			FQID bookmarkFQID = new FQID(serverId, bookmark.DeviceId, bookmark.Id, FolderType.No, Kind.Bookmark);
			array2[i] = new VideoOS.Platform.Data.Bookmark
			{
				BookmarkFQID = bookmarkFQID,
				Description = bookmark.Description,
				Header = bookmark.Header,
				Reference = bookmark.Reference,
				TimeBegin = bookmark.TimeBegin,
				TimeTrigged = bookmark.TimeTrigged,
				TimeEnd = bookmark.TimeEnd,
				User = bookmark.User
			};
		}
		return array2;
	}

	public override VideoOS.Platform.Data.Bookmark[] BookmarkSearchTime(ServerId serverId, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		ILoginServer loginServer = GetLoginServer(serverId);
		if (loginServer == null)
		{
			return null;
		}
		IMediaDeviceType[] deviceTypes = DeviceType2SMS(kinds);
		TimeDuration timeLimit = new TimeDuration
		{
			MicroSeconds = timeLimitUSec
		};
		IBookmark[] array = loginServer.BookmarkSearchTime(time, timeLimit, countLimit, deviceTypes, FQIDToDeviceId(fqids), optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
		if (array == null)
		{
			return null;
		}
		VideoOS.Platform.Data.Bookmark[] array2 = new VideoOS.Platform.Data.Bookmark[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			IBookmark bookmark = array[i];
			FQID bookmarkFQID = new FQID(serverId, bookmark.DeviceId, bookmark.Id, FolderType.No, Kind.Bookmark);
			array2[i] = new VideoOS.Platform.Data.Bookmark
			{
				BookmarkFQID = bookmarkFQID,
				Description = bookmark.Description,
				Header = bookmark.Header,
				Reference = bookmark.Reference,
				TimeBegin = bookmark.TimeBegin,
				TimeTrigged = bookmark.TimeTrigged,
				TimeEnd = bookmark.TimeEnd,
				User = bookmark.User
			};
		}
		return array2;
	}

	public override void BookmarkSearchTimeAsync(ServerId serverId, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, AsyncCallbackHandler resultCallbackEventHandler, object asyncState)
	{
		ILoginServer loginServer = GetLoginServer(serverId);
		if (loginServer != null)
		{
			new BookmarkSearchData(serverId, resultCallbackEventHandler, asyncState);
			IMediaDeviceType[] deviceTypes = DeviceType2SMS(kinds);
			TimeDuration timeDuration = new TimeDuration();
			timeDuration.MicroSeconds = timeLimitUSec;
			Task<IBookmark[]> task = loginServer.BookmarkSearchTimeAsync(time, timeDuration, countLimit, deviceTypes, FQIDToDeviceId(fqids), optUsers, optSearchStr);
			InvokeEventHandler(resultCallbackEventHandler, task, serverId, asyncState);
		}
	}

	public override void BookmarkSearchTimeAsync(ServerId serverId, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, AsyncCallbackHandler resultCallbackEventHandler, object asyncState)
	{
		ILoginServer loginServer = GetLoginServer(serverId);
		if (loginServer != null)
		{
			new BookmarkSearchData(serverId, resultCallbackEventHandler, asyncState);
			IMediaDeviceType[] deviceTypes = DeviceType2SMS(kinds);
			TimeDuration timeDuration = new TimeDuration();
			timeDuration.MicroSeconds = timeLimitUSec;
			Task<IBookmark[]> task = loginServer.BookmarkSearchTimeAsync(time, timeDuration, countLimit, deviceTypes, FQIDToDeviceId(fqids), optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
			InvokeEventHandler(resultCallbackEventHandler, task, serverId, asyncState);
		}
	}

	private static void InvokeEventHandler(AsyncCallbackHandler callbackHandler, Task<IBookmark[]> task, ServerId serverId, object asyncState)
	{
		Task.Run(delegate
		{
			try
			{
				BookmarkSearchCompletedEventArgs result = new BookmarkSearchCompletedEventArgs(task.Result.Select((IBookmark re) => new VideoOS.Platform.Data.Bookmark
				{
					BookmarkFQID = new FQID(serverId, re.DeviceId, re.Id, FolderType.No, Kind.Bookmark),
					Description = re.Description,
					Header = re.Header,
					Reference = re.Reference,
					TimeBegin = re.TimeBegin,
					TimeTrigged = re.TimeTrigged,
					TimeEnd = re.TimeEnd,
					User = re.User
				}).ToArray(), null, cancelled: false, asyncState);
				callbackHandler(result, asyncState);
			}
			catch (AggregateException exception)
			{
				BookmarkSearchCompletedEventArgs result2 = new BookmarkSearchCompletedEventArgs(null, exception, cancelled: false, asyncState);
				callbackHandler(result2, asyncState);
			}
		});
	}

	public override VideoOS.Platform.Data.Bookmark[] BookmarkSearchTime(UserContext userContext, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr)
	{
		if (userContext == null)
		{
			return null;
		}
		IOrderedEnumerable<VideoOS.Platform.Data.Bookmark> source = from bookmark in (userContext.Configuration as SDKConfiguration).SystemInfoList.Values.Where((ISystemInfo systemInfo) => systemInfo.Populate()).SelectMany((ISystemInfo systemInfo) => BookmarkSearchTime(systemInfo.ServerId, time, timeLimitUSec, countLimit, kinds, fqids, optUsers, optSearchStr))
			orderby bookmark.TimeTrigged
			select bookmark;
		if (timeLimitUSec < 0)
		{
			return source.TakeLast(countLimit).ToArray();
		}
		return source.Take(countLimit).ToArray();
	}

	public override VideoOS.Platform.Data.Bookmark[] BookmarkSearchTime(UserContext userContext, DateTime time, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		if (userContext == null)
		{
			return null;
		}
		IOrderedEnumerable<VideoOS.Platform.Data.Bookmark> source = from bookmark in (userContext.Configuration as SDKConfiguration).SystemInfoList.Values.Where((ISystemInfo systemInfo) => systemInfo.Populate()).SelectMany((ISystemInfo systemInfo) => BookmarkSearchTime(systemInfo.ServerId, time, timeLimitUSec, countLimit, kinds, fqids, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter))
			orderby bookmark.TimeTrigged
			select bookmark;
		if (timeLimitUSec < 0)
		{
			return source.TakeLast(countLimit).ToArray();
		}
		return source.Take(countLimit).ToArray();
	}

	public override VideoOS.Platform.Data.Bookmark[] BookmarkSearchFromBookmark(FQID fromBookmarkFQID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr)
	{
		ILoginServer loginServer = GetLoginServer(fromBookmarkFQID);
		if (loginServer == null)
		{
			return null;
		}
		_ = loginServer.LoginInfo.Token;
		IMediaDeviceType[] deviceTypes = DeviceType2SMS(kinds);
		TimeDuration timeDuration = new TimeDuration();
		timeDuration.MicroSeconds = timeLimitUSec;
		IBookmark[] array = loginServer.BookmarkSearchFromBookmark(fromBookmarkFQID.ObjectId, timeDuration, countLimit, deviceTypes, FQIDToDeviceId(fqids), optUsers, optSearchStr);
		if (array == null)
		{
			return null;
		}
		VideoOS.Platform.Data.Bookmark[] array2 = new VideoOS.Platform.Data.Bookmark[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			IBookmark bookmark = array[i];
			FQID bookmarkFQID = new FQID(fromBookmarkFQID.ServerId, bookmark.DeviceId, bookmark.Id, FolderType.No, Kind.Bookmark);
			array2[i] = new VideoOS.Platform.Data.Bookmark
			{
				BookmarkFQID = bookmarkFQID,
				Description = bookmark.Description,
				Header = bookmark.Header,
				Reference = bookmark.Reference,
				TimeBegin = bookmark.TimeBegin,
				TimeTrigged = bookmark.TimeTrigged,
				TimeEnd = bookmark.TimeEnd,
				User = bookmark.User
			};
		}
		return array2;
	}

	public override VideoOS.Platform.Data.Bookmark[] BookmarkSearchFromBookmark(FQID fromBookmarkFQID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		ILoginServer loginServer = GetLoginServer(fromBookmarkFQID);
		if (loginServer == null)
		{
			return null;
		}
		_ = loginServer.LoginInfo.Token;
		IMediaDeviceType[] deviceTypes = DeviceType2SMS(kinds);
		TimeDuration timeDuration = new TimeDuration();
		timeDuration.MicroSeconds = timeLimitUSec;
		IBookmark[] array = loginServer.BookmarkSearchFromBookmark(fromBookmarkFQID.ObjectId, timeDuration, countLimit, deviceTypes, FQIDToDeviceId(fqids), optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
		if (array == null)
		{
			return null;
		}
		VideoOS.Platform.Data.Bookmark[] array2 = new VideoOS.Platform.Data.Bookmark[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			IBookmark bookmark = array[i];
			FQID bookmarkFQID = new FQID(fromBookmarkFQID.ServerId, bookmark.DeviceId, bookmark.Id, FolderType.No, Kind.Bookmark);
			array2[i] = new VideoOS.Platform.Data.Bookmark
			{
				BookmarkFQID = bookmarkFQID,
				Description = bookmark.Description,
				Header = bookmark.Header,
				Reference = bookmark.Reference,
				TimeBegin = bookmark.TimeBegin,
				TimeTrigged = bookmark.TimeTrigged,
				TimeEnd = bookmark.TimeEnd,
				User = bookmark.User
			};
		}
		return array2;
	}

	public override VideoOS.Platform.Data.Bookmark[] BookmarkSearchFromBookmark(UserContext userContext, Guid fromBookmarkID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr)
	{
		if (userContext == null)
		{
			return null;
		}
		VideoOS.Platform.Data.Bookmark fromBookmark = BookmarkGet(userContext, fromBookmarkID);
		IOrderedEnumerable<VideoOS.Platform.Data.Bookmark> source = from bookmarк in (userContext.Configuration as SDKConfiguration).SystemInfoList.Values.Where((ISystemInfo systemInfo) => systemInfo.Populate()).SelectMany((ISystemInfo systemInfo) => (systemInfo.ServerId == fromBookmark.BookmarkFQID.ServerId) ? BookmarkSearchFromBookmark(fromBookmark.BookmarkFQID, timeLimitUSec, countLimit, kinds, fqids, optUsers, optSearchStr) : BookmarkSearchTime(systemInfo.ServerId, fromBookmark.TimeTrigged, timeLimitUSec, countLimit, kinds, fqids, optUsers, optSearchStr))
			orderby bookmarк.TimeTrigged
			select bookmarк;
		if (timeLimitUSec < 0)
		{
			return source.TakeLast(countLimit).ToArray();
		}
		return source.Take(countLimit).ToArray();
	}

	public override VideoOS.Platform.Data.Bookmark[] BookmarkSearchFromBookmark(UserContext userContext, Guid fromBookmarkID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		if (userContext == null)
		{
			return null;
		}
		VideoOS.Platform.Data.Bookmark fromBookmark = BookmarkGet(userContext, fromBookmarkID);
		IOrderedEnumerable<VideoOS.Platform.Data.Bookmark> source = from bookmarк in (userContext.Configuration as SDKConfiguration).SystemInfoList.Values.Where((ISystemInfo systemInfo) => systemInfo.Populate()).SelectMany((ISystemInfo systemInfo) => (systemInfo.ServerId == fromBookmark.BookmarkFQID.ServerId) ? BookmarkSearchFromBookmark(fromBookmark.BookmarkFQID, timeLimitUSec, countLimit, kinds, fqids, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter) : BookmarkSearchTime(systemInfo.ServerId, fromBookmark.TimeTrigged, timeLimitUSec, countLimit, kinds, fqids, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter))
			orderby bookmarк.TimeTrigged
			select bookmarк;
		if (timeLimitUSec < 0)
		{
			return source.TakeLast(countLimit).ToArray();
		}
		return source.Take(countLimit).ToArray();
	}

	public override void BookmarkSearchFromBookmarkAsync(FQID fromBookmarkFQID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, AsyncCallbackHandler resultCallbackEventHandler, object asyncState)
	{
		ILoginServer loginServer = GetLoginServer(fromBookmarkFQID);
		if (loginServer != null)
		{
			new BookmarkSearchData(fromBookmarkFQID.ServerId, resultCallbackEventHandler, asyncState);
			IMediaDeviceType[] deviceTypes = DeviceType2SMS(kinds);
			TimeDuration timeDuration = new TimeDuration();
			timeDuration.MicroSeconds = timeLimitUSec;
			Task<IBookmark[]> task = loginServer.BookmarkSearchFromBookmarkAsync(fromBookmarkFQID.ObjectId, timeDuration, countLimit, deviceTypes, FQIDToDeviceId(fqids), optUsers, optSearchStr);
			InvokeEventHandler(resultCallbackEventHandler, task, fromBookmarkFQID.ServerId, asyncState);
		}
	}

	public override void BookmarkSearchFromBookmarkAsync(FQID fromBookmarkFQID, long timeLimitUSec, int countLimit, Guid[] kinds, FQID[] fqids, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, AsyncCallbackHandler resultCallbackEventHandler, object asyncState)
	{
		ILoginServer ls = GetLoginServer(fromBookmarkFQID);
		if (ls != null)
		{
			new BookmarkSearchData(fromBookmarkFQID.ServerId, resultCallbackEventHandler, asyncState);
			IMediaDeviceType[] mediaDeviceTypes = DeviceType2SMS(kinds);
			TimeDuration td = new TimeDuration();
			td.MicroSeconds = timeLimitUSec;
			Task.Run(delegate
			{
				Task<IBookmark[]> task = ls.BookmarkSearchFromBookmarkAsync(fromBookmarkFQID.ObjectId, td, countLimit, mediaDeviceTypes, FQIDToDeviceId(fqids), optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
				InvokeEventHandler(resultCallbackEventHandler, task, fromBookmarkFQID.ServerId, asyncState);
			});
		}
	}

	public override VideoOS.Platform.Data.Bookmark BookmarkGet(FQID bookmarkFQID)
	{
		if (bookmarkFQID == null)
		{
			return null;
		}
		ILoginServer loginServer = GetLoginServer(bookmarkFQID);
		if (loginServer == null)
		{
			return null;
		}
		_ = loginServer.LoginInfo.Token;
		IBookmark bookmark = loginServer.BookmarkGet(bookmarkFQID.ObjectId);
		if (bookmark == null)
		{
			return null;
		}
		return new VideoOS.Platform.Data.Bookmark
		{
			BookmarkFQID = new FQID
			{
				ServerId = bookmarkFQID.ServerId,
				ObjectId = bookmarkFQID.ObjectId,
				Kind = bookmarkFQID.Kind,
				ParentId = bookmark.DeviceId
			},
			Description = bookmark.Description,
			Header = bookmark.Header,
			Reference = bookmark.Reference,
			TimeBegin = bookmark.TimeBegin,
			TimeTrigged = bookmark.TimeTrigged,
			TimeEnd = bookmark.TimeEnd,
			User = bookmark.User
		};
	}

	public override VideoOS.Platform.Data.Bookmark BookmarkGet(UserContext userContext, Guid objectId)
	{
		if (userContext == null)
		{
			return null;
		}
		ISystemInfo systemInfo = (userContext.Configuration as SDKConfiguration).SystemInfoList.Values.Where((ISystemInfo systemInfo2) => systemInfo2.Populate()).SingleOrDefault((ISystemInfo systemInfo2) => systemInfo2.LoginServer.BookmarkGet(objectId) != null);
		if (systemInfo == null)
		{
			return null;
		}
		FQID bookmarkFQID = new FQID
		{
			ObjectId = objectId,
			ServerId = systemInfo.ServerId,
			Kind = Kind.Bookmark
		};
		return BookmarkGet(bookmarkFQID);
	}

	public override VideoOS.Platform.Data.Bookmark BookmarkUpdate(VideoOS.Platform.Data.Bookmark bookmark)
	{
		if (bookmark == null)
		{
			return null;
		}
		ILoginServer loginServer = GetLoginServer(bookmark.BookmarkFQID);
		if (loginServer == null)
		{
			return null;
		}
		IBookmark bookmark2 = loginServer.BookmarkUpdate(bookmark.BookmarkFQID.ObjectId, bookmark.BookmarkFQID.ParentId, bookmark.TimeBegin, bookmark.TimeTrigged, bookmark.TimeEnd, bookmark.Reference, bookmark.Header, bookmark.Description);
		if (bookmark2 == null)
		{
			return null;
		}
		return new VideoOS.Platform.Data.Bookmark
		{
			BookmarkFQID = bookmark.BookmarkFQID,
			Description = bookmark2.Description,
			Header = bookmark2.Header,
			Reference = bookmark2.Reference,
			TimeBegin = bookmark2.TimeBegin,
			TimeTrigged = bookmark2.TimeTrigged,
			TimeEnd = bookmark2.TimeEnd,
			User = bookmark2.User
		};
	}

	public override void BookmarkDelete(FQID bookmarkFQID)
	{
		if (!(bookmarkFQID == null))
		{
			ILoginServer loginServer = GetLoginServer(bookmarkFQID);
			if (loginServer != null)
			{
				_ = loginServer.LoginInfo.Token;
				loginServer.BookmarkDelete(bookmarkFQID.ObjectId);
			}
		}
	}

	public override void BookmarkDelete(VideoOS.Platform.Data.Bookmark bookmark)
	{
		if (bookmark != null)
		{
			BookmarkDelete(bookmark.BookmarkFQID);
		}
	}

	public override VideoOS.Platform.Data.BookmarkReference BookmarkGetNewReference(FQID deviceFQID, bool live)
	{
		if (deviceFQID == null)
		{
			throw new ArgumentException("device FQID cannot be null", "deviceFQID");
		}
		IBookmarkReference bookmarkReference = (GetLoginServer(deviceFQID) ?? throw new ArgumentException("Server for device is not online", "deviceFQID")).BookmarkGetNewReference(deviceFQID.ObjectId, live);
		return new VideoOS.Platform.Data.BookmarkReference
		{
			Reference = bookmarkReference.Reference,
			Time = bookmarkReference.Time
		};
	}

	private static ServerId GetServerId(ServerId serverId)
	{
		if (serverId.ServerType == "XPCO")
		{
			return serverId;
		}
		SDKConfiguration sDKConfiguration = ((serverId.UserContext != null) ? ((SDKConfiguration)serverId.UserContext.Configuration) : (Configuration.Instance as SDKConfiguration));
		if (sDKConfiguration == null)
		{
			return null;
		}
		Item item = sDKConfiguration.GetItem(serverId.Id, Kind.Server);
		int num = 5;
		while (item != null && item.FQID.ServerId.ServerType != "XPCO" && num-- > 0)
		{
			item = item.GetParent();
		}
		return item?.FQID.ServerId;
	}

	private static ILoginServer GetLoginServer(ServerId serverId)
	{
		ServerId serverId2 = GetServerId(serverId);
		SDKConfiguration sDKConfiguration = (SDKConfiguration)serverId.UserContext.Configuration;
		if (sDKConfiguration == null)
		{
			return null;
		}
		foreach (ISystemInfo value in sDKConfiguration.SystemInfoList.Values)
		{
			if (value.Populate() && value.Id == serverId2.Id)
			{
				return value.LoginServer;
			}
		}
		return null;
	}

	private static ILoginServer GetLoginServer(FQID fqid)
	{
		ServerId serverId = GetServerId(fqid.ServerId);
		SDKConfiguration sDKConfiguration = (SDKConfiguration)fqid.ServerId.UserContext.Configuration;
		if (sDKConfiguration == null)
		{
			return null;
		}
		foreach (ISystemInfo value in sDKConfiguration.SystemInfoList.Values)
		{
			if (value.Populate() && value.Id == serverId.Id)
			{
				return value.LoginServer;
			}
		}
		return null;
	}

	private static Guid[] FQIDToDeviceId(FQID[] fqids)
	{
		if (fqids == null)
		{
			return null;
		}
		Guid[] array = new Guid[fqids.Length];
		for (int i = 0; i < fqids.Length; i++)
		{
			array[i] = fqids[i].ObjectId;
		}
		return array;
	}

	public static IMediaDeviceType[] DeviceType2SMS(Guid[] types)
	{
		if (types == null)
		{
			return null;
		}
		IMediaDeviceType[] array = new IMediaDeviceType[types.Length];
		for (int i = 0; i < types.Length; i++)
		{
			array[i] = ((types[i] == Kind.Microphone) ? IMediaDeviceType.Microphone : ((types[i] == Kind.Speaker) ? IMediaDeviceType.Speaker : IMediaDeviceType.Camera));
		}
		return array;
	}
}
