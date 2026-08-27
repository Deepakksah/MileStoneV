using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

public static class ServerCommandServiceBookmarkHandling
{
	public static BookmarkReference BookmarkGetNewReference(IGeminiRequestContext requestContext, Guid deviceId, bool live)
	{
		try
		{
			MarkedDataReference markedDataReference = MarkedData.QueryMarkedDataReference(requestContext, new string[1] { deviceId.ToString() }, live, MarkedDataDefinitions.MarkedDataTypes.Bookmark);
			return new BookmarkReference
			{
				Reference = markedDataReference.Reference,
				Time = markedDataReference.Time
			};
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static Bookmark BookmarkCreate(IGeminiRequestContext requestContext, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		try
		{
			return ConvertVmo2ScsBookmark(MarkedData.CreateMarkedData(requestContext, Guid.NewGuid().ToString(), new string[1] { deviceId.ToString() }, reference, header, description, 8, timeBegin, timeTrigged, timeEnd, useRetention: false, 0, 0, DateTime.MaxValue));
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static Bookmark BookmarkUpdate(IGeminiRequestContext requestContext, Guid bookmarkId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		try
		{
			return ConvertVmo2ScsBookmark(MarkedData.UpdateMarkedData(requestContext, bookmarkId.ToString(), 8, null, reference, header, description, timeBegin, timeTrigged, timeEnd, useRetention: false, 0, 0, DateTime.UtcNow));
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static void BookmarkDelete(IGeminiRequestContext requestContext, Guid bookmarkId)
	{
		try
		{
			MarkedData.DeleteMarkedData(requestContext, bookmarkId.ToString(), MarkedDataDefinitions.MarkedDataTypes.Bookmark);
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static Bookmark BookmarkGet(IGeminiRequestContext requestContext, Guid bookmarkId)
	{
		try
		{
			return ConvertVmo2ScsBookmark(MarkedData.QueryMarkedDataItem(requestContext, bookmarkId.ToString(), 8));
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	public static Bookmark[] BookmarkSearch(IGeminiRequestContext requestContext, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearch, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string optFromBookmarkId)
	{
		try
		{
			List<DeviceType> list = new List<DeviceType>();
			if (deviceTypes != null)
			{
				for (int i = 0; i < deviceTypes.Length; i++)
				{
					switch (deviceTypes[i])
					{
					case MediaDeviceType.Camera:
						list.Add(DeviceType.Camera);
						break;
					case MediaDeviceType.Microphone:
						list.Add(DeviceType.Microphone);
						break;
					case MediaDeviceType.Speaker:
						list.Add(DeviceType.Speaker);
						break;
					}
				}
			}
			string[] optDeviceIds2 = optDeviceIds?.Select((Guid id) => id.ToString()).ToArray();
			MarkedDataDataContract[] array = MarkedData.QueryBookmarksSearch(requestContext, time, TimeSpan.FromTicks(timeLimit.MicroSeconds * 10), countLimit, list.ToArray(), optDeviceIds2, optUsers, optSearch, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, optFromBookmarkId);
			List<Bookmark> list2 = new List<Bookmark>();
			MarkedDataDataContract[] array2 = array;
			foreach (MarkedDataDataContract markedData in array2)
			{
				list2.Add(ConvertVmo2ScsBookmark(markedData));
			}
			if (timeLimit.MicroSeconds < 0)
			{
				list2.Reverse();
			}
			return list2.ToArray();
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
	}

	private static Bookmark ConvertVmo2ScsBookmark(MarkedDataDataContract markedData)
	{
		if (markedData == null)
		{
			return null;
		}
		return new Bookmark
		{
			Id = new Guid(markedData.Id),
			DeviceId = new Guid(markedData.DeviceIds.First()),
			User = markedData.UserName,
			Reference = markedData.Reference,
			Header = markedData.Header,
			Description = markedData.Description,
			TimeBegin = markedData.StartTime,
			TimeTrigged = markedData.TagTime,
			TimeEnd = markedData.EndTime
		};
	}
}
