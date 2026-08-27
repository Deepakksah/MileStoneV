using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.Common.Proxy.Server.WCF.Net;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.BookmarkApi;
using VideoOS.Platform.SDK.Config.Shared;
using VideoOS.Platform.SDK.Interface;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK.Config;

public class LoginServerRest : LoginServerOauth
{
	private BookmarkApiClient _bookmarkApiClient;

	private object _bookmarkClientLock = new object();

	private LoginSettings _loginSettings;

	private BookmarkApiClient BookmarkApiClient
	{
		get
		{
			lock (_bookmarkClientLock)
			{
				if (_bookmarkApiClient == null)
				{
					_bookmarkApiClient = new BookmarkApiClient();
					_bookmarkApiClient.Initialize(_loginSettings, ServerTrace.TraceServerCommunication);
				}
			}
			return _bookmarkApiClient;
		}
	}

	public LoginServerRest(UserContext userContext, ISystemInfo systemInfo)
		: base(userContext, systemInfo)
	{
		_loginSettings = UserContext.GetLoginSettings(systemInfo.ServerId);
	}

	public override void LogoutService(Guid instanceGuid, string token)
	{
		lock (_bookmarkClientLock)
		{
			if (_bookmarkApiClient != null)
			{
				_bookmarkApiClient.Dispose();
				_bookmarkApiClient = null;
			}
		}
		base.LogoutService(instanceGuid, token);
	}

	public override IBookmarkReference BookmarkGetNewReference(Guid deviceId, bool live)
	{
		NewBookmarkReferenceDataModel newBookmarkReferenceDataModel = BookmarkApiClient.BookmarkGetNewReference(deviceId, live);
		return new BookmarkReference
		{
			Reference = newBookmarkReferenceDataModel.reference,
			Time = DateTime.Parse(newBookmarkReferenceDataModel.time)
		};
	}

	public override Task<IBookmark[]> BookmarkSearchFromBookmarkAsync(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		Task<IBookmark[]> task = new Task<IBookmark[]>(() => BookmarkSearchFromBookmark(fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr));
		task.Start();
		return task;
	}

	public override Task<IBookmark[]> BookmarkSearchFromBookmarkAsync(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		Task<IBookmark[]> task = new Task<IBookmark[]>(() => BookmarkSearchFromBookmark(fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter));
		task.Start();
		return task;
	}

	public override Task<IBookmark[]> BookmarkSearchTimeAsync(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		Task<IBookmark[]> task = new Task<IBookmark[]>(() => BookmarkSearchTime(time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr));
		task.Start();
		return task;
	}

	public override Task<IBookmark[]> BookmarkSearchTimeAsync(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		Task<IBookmark[]> task = new Task<IBookmark[]>(() => BookmarkSearchTime(time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter));
		task.Start();
		return task;
	}

	public override void BookmarkDelete(Guid bookmarkId)
	{
		BookmarkApiClient.BookmarkDelete(bookmarkId);
	}

	public override IBookmark BookmarkCreate(Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return ConvertRestToWcfModel(BookmarkApiClient.BookmarkCreate(deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description));
	}

	public override IBookmark[] BookmarkSearchTime(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		BookmarkDataModel[] array = BookmarkApiClient.BookmarkSearchTime(time, TimeSpan.FromMilliseconds(timeLimit.MicroSeconds / 1000), countLimit, ConvertMediaToResources(deviceTypes), optDeviceIds, optUsers, optSearchStr, searchHeader: true, searchDescription: true, searchReference: true, searchUser: true, null, null);
		List<IBookmark> list = new List<IBookmark>();
		BookmarkDataModel[] array2 = array;
		foreach (BookmarkDataModel bookmarkDataModel in array2)
		{
			list.Add(ConvertRestToWcfModel(bookmarkDataModel));
		}
		return list.ToArray();
	}

	public override IBookmark[] BookmarkSearchTime(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		BookmarkDataModel[] array = BookmarkApiClient.BookmarkSearchTime(time, TimeSpan.FromMilliseconds(timeLimit.MicroSeconds / 1000), countLimit, ConvertMediaToResources(deviceTypes), optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
		List<IBookmark> list = new List<IBookmark>();
		BookmarkDataModel[] array2 = array;
		foreach (BookmarkDataModel bookmarkDataModel in array2)
		{
			list.Add(ConvertRestToWcfModel(bookmarkDataModel));
		}
		return list.ToArray();
	}

	public override IBookmark BookmarkGet(Guid bookmarkId)
	{
		BookmarkDataModel bookmarkDataModel = BookmarkApiClient.BookmarkGet(bookmarkId);
		if (bookmarkDataModel == null)
		{
			return null;
		}
		return ConvertRestToWcfModel(bookmarkDataModel);
	}

	public override IBookmark BookmarkUpdate(Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		BookmarkApiClient.BookmarkUpdate(bookmarkId, timeBegin, timeTrigged, timeEnd, reference, header, description);
		return BookmarkGet(bookmarkId);
	}

	public override IBookmark[] BookmarkSearchFromBookmark(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		BookmarkDataModel[] array = BookmarkApiClient.BookmarkSearchFromBookmark(fromBookmarkId.ToString(), TimeSpan.FromMilliseconds(timeLimit.MicroSeconds / 1000), countLimit, ConvertMediaToResources(deviceTypes), optDeviceIds, optUsers, optSearchStr, searchHeader: true, searchDescription: true, searchReference: true, searchUser: true, null, null);
		List<IBookmark> list = new List<IBookmark>();
		BookmarkDataModel[] array2 = array;
		foreach (BookmarkDataModel bookmarkDataModel in array2)
		{
			list.Add(ConvertRestToWcfModel(bookmarkDataModel));
		}
		return list.ToArray();
	}

	public override IBookmark[] BookmarkSearchFromBookmark(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		BookmarkDataModel[] array = BookmarkApiClient.BookmarkSearchFromBookmark(fromBookmarkId.ToString(), TimeSpan.FromMilliseconds(timeLimit.MicroSeconds / 1000), countLimit, ConvertMediaToResources(deviceTypes), optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
		List<IBookmark> list = new List<IBookmark>();
		BookmarkDataModel[] array2 = array;
		foreach (BookmarkDataModel bookmarkDataModel in array2)
		{
			list.Add(ConvertRestToWcfModel(bookmarkDataModel));
		}
		return list.ToArray();
	}

	private static IBookmark ConvertRestToWcfModel(BookmarkDataModel bookmarkDataModel)
	{
		return new Bookmark
		{
			DeviceId = bookmarkDataModel.devicePath.id,
			TimeBegin = bookmarkDataModel.timeBegin.Value,
			TimeEnd = bookmarkDataModel.timeEnd.Value,
			Reference = bookmarkDataModel.reference,
			Header = bookmarkDataModel.header,
			Description = bookmarkDataModel.description,
			TimeTrigged = bookmarkDataModel.timeTriggered.Value,
			Id = bookmarkDataModel.relations.self.id,
			User = bookmarkDataModel.user
		};
	}

	private static string[] ConvertMediaToResources(IMediaDeviceType[] mediaTypes)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < mediaTypes.Length; i++)
		{
			switch (mediaTypes[i])
			{
			case IMediaDeviceType.Camera:
				list.Add("cameras");
				break;
			case IMediaDeviceType.Microphone:
				list.Add("microphones");
				break;
			case IMediaDeviceType.Speaker:
				list.Add("speakers");
				break;
			}
		}
		return list.ToArray();
	}
}
