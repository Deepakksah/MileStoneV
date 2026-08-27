using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Platform.Proxy.BookmarkApi;

public class BookmarkApiClient : RestApiBase, IBookmarkApiClient
{
	private int httpStatus;

	public new void Initialize(LoginSettings loginSettings, bool debug)
	{
		if (loginSettings == null)
		{
			throw new ArgumentNullException("loginSettings");
		}
		if (string.Compare(loginSettings.ServerProductInfo.ProductVersion, "24.2", ignoreCase: false, CultureInfo.InvariantCulture) >= 0)
		{
			base.Initialize(loginSettings, debug);
			return;
		}
		throw new CommunicationMIPException("Server does not support /bookmarks RestAPI");
	}

	public void Close()
	{
		Dispose();
	}

	public BookmarkDataModel BookmarkGet(Guid bookmarkId)
	{
		string relativeUrl = string.Format(CultureInfo.InvariantCulture, "bookmarks/{0}", bookmarkId);
		string requestWithRetry = GetRequestWithRetry(relativeUrl);
		if (requestWithRetry != null)
		{
			return JsonToBookmarkDataModel(new JsonObject(requestWithRetry).GetChild("data"));
		}
		return null;
	}

	public void BookmarkUpdate(Guid bookmarkId, DateTime? timeBegin, DateTime? timeTriggered, DateTime? timeEnd, string reference, string header, string description)
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		if (timeBegin.HasValue)
		{
			jsonBuilder.AppendKeyString("timeBegin", timeBegin.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (timeTriggered.HasValue)
		{
			jsonBuilder.AppendKeyString("timeTriggered", timeTriggered.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (timeEnd.HasValue)
		{
			jsonBuilder.AppendKeyString("timeEnd", timeEnd.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		jsonBuilder.AppendKeyString("header", header);
		jsonBuilder.AppendKeyString("description", description);
		jsonBuilder.AppendKeyString("reference", reference);
		string body = jsonBuilder.ToJson();
		string relativeUrl = string.Format(CultureInfo.InvariantCulture, "bookmarks/{0}", bookmarkId);
		PatchRequestWithRetry(relativeUrl, body);
	}

	public BookmarkDataModel BookmarkCreate(Guid deviceId, DateTime? timeBegin, DateTime timeTriggered, DateTime? timeEnd, string reference, string header, string description)
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		jsonBuilder.AppendKeyString("type", "cameras");
		jsonBuilder.AppendKeyString("id", deviceId.ToString());
		JsonBuilder jsonBuilder2 = new JsonBuilder(array: false);
		jsonBuilder2.AppendElement("devicePath", jsonBuilder.ToJson());
		if (timeBegin.HasValue)
		{
			jsonBuilder2.AppendKeyString("timeBegin", timeBegin.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		jsonBuilder2.AppendKeyString("timeTriggered", timeTriggered.ToString("o", CultureInfo.InvariantCulture));
		if (timeEnd.HasValue)
		{
			jsonBuilder2.AppendKeyString("timeEnd", timeEnd.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		jsonBuilder2.AppendKeyString("header", header);
		jsonBuilder2.AppendKeyString("description", description);
		jsonBuilder2.AppendKeyString("reference", reference);
		string body = jsonBuilder2.ToJson();
		string relativeUrl = "bookmarks";
		(string, int) tuple = PostRequestWithRetry(relativeUrl, body);
		string item = tuple.Item1;
		int num = (httpStatus = tuple.Item2);
		if (item != null)
		{
			JsonObject jsonObject = JsonObject.Parse(item);
			if (jsonObject != null)
			{
				JsonObject child = jsonObject.GetChild("result");
				if (num == 201 && child != null)
				{
					return JsonToBookmarkDataModel(child);
				}
			}
			throw new MIPException(string.Format(CultureInfo.InvariantCulture, "Bookmark: Unexpected HTTP status code: {0}", httpStatus));
		}
		return null;
	}

	public BookmarkDataModel[] BookmarkSearchTime(DateTime time, TimeSpan timeLimit, int countLimit, string[] resourceTypes, Guid[] deviceIds, string[] users, string searchString, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerContains, string descriptionContains)
	{
		return BookmarkSearchInternal("searchTime", time, null, timeLimit, countLimit, resourceTypes, deviceIds, users, searchString, searchHeader, searchDescription, searchReference, searchUser, headerContains, descriptionContains);
	}

	public BookmarkDataModel[] BookmarkSearchFromBookmark(string fromBookmarkId, TimeSpan timeLimit, int countLimit, string[] resourceTypes, Guid[] deviceIds, string[] users, string searchString, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerContains, string descriptionContains)
	{
		return BookmarkSearchInternal("searchFromBookmark", null, fromBookmarkId, timeLimit, countLimit, resourceTypes, deviceIds, users, searchString, searchHeader, searchDescription, searchReference, searchUser, headerContains, descriptionContains);
	}

	public NewBookmarkReferenceDataModel BookmarkGetNewReference(Guid deviceId, bool live)
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		jsonBuilder.AppendKeyString("type", "cameras");
		jsonBuilder.AppendKeyString("id", deviceId.ToString());
		JsonBuilder jsonBuilder2 = new JsonBuilder(array: false);
		jsonBuilder2.AppendKeyValue("live", live ? "true" : "false");
		jsonBuilder2.AppendElement("devicePath", jsonBuilder.ToJson());
		string body = jsonBuilder2.ToJson();
		string relativeUrl = "bookmarks?task=newReference";
		(string, int) tuple = PostRequestWithRetry(relativeUrl, body);
		string item = tuple.Item1;
		int num = (httpStatus = tuple.Item2);
		if (item != null)
		{
			JsonObject jsonObject = JsonObject.Parse(item);
			if (jsonObject != null)
			{
				JsonObject child = jsonObject.GetChild("result");
				if (num == 201 && child != null)
				{
					return new NewBookmarkReferenceDataModel
					{
						reference = child.GetString("reference"),
						time = child.GetString("time")
					};
				}
			}
			throw new MIPException(string.Format(CultureInfo.InvariantCulture, "Bookmark: Unexpected HTTP status code: {0}", httpStatus));
		}
		return null;
	}

	public void BookmarkDelete(Guid bookmarkId)
	{
		string relativeUrl = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", "bookmarks", bookmarkId);
		DeleteRequestWithRetry(relativeUrl);
	}

	private BookmarkDataModel[] BookmarkSearchInternal(string searchTask, DateTime? time, string fromBookmarkId, TimeSpan timeLimit, int countLimit, string[] resourceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerContains, string descriptionContains)
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		if (!string.IsNullOrEmpty(fromBookmarkId))
		{
			jsonBuilder.AppendKeyString("fromBookmarkId", fromBookmarkId);
		}
		if (time.HasValue)
		{
			jsonBuilder.AppendKeyString("time", time.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (optDeviceIds != null && optDeviceIds.Any())
		{
			JsonBuilder jsonBuilder2 = new JsonBuilder(array: true);
			for (int i = 0; i < optDeviceIds.Length; i++)
			{
				Guid guid = optDeviceIds[i];
				JsonBuilder jsonBuilder3 = new JsonBuilder(array: false);
				jsonBuilder3.AppendKeyString("type", "cameras");
				jsonBuilder3.AppendKeyString("id", guid.ToString());
				jsonBuilder2.AppendElement(jsonBuilder3.ToJson());
			}
			jsonBuilder.AppendElement("devicePaths", jsonBuilder2.ToJson());
		}
		if (resourceTypes != null && resourceTypes.Any())
		{
			JsonBuilder jsonBuilder4 = new JsonBuilder(array: true);
			string[] array = resourceTypes;
			foreach (string value in array)
			{
				jsonBuilder4.AppendString(value);
			}
			jsonBuilder.AppendKeyValue("resourceTypes", jsonBuilder4.ToJson());
		}
		jsonBuilder.AppendKeyString("timeLimit", timeLimit.ToString());
		jsonBuilder.AppendKeyValue("countLimit", countLimit.ToString(CultureInfo.InvariantCulture));
		if (optUsers != null && optUsers.Any())
		{
			JsonBuilder jsonBuilder5 = new JsonBuilder(array: true);
			string[] array = optUsers;
			foreach (string elementWithBrackets in array)
			{
				jsonBuilder5.AppendElement(elementWithBrackets);
			}
			jsonBuilder.AppendKeyValue("users", jsonBuilder5.ToJson());
		}
		if (!string.IsNullOrEmpty(headerContains))
		{
			jsonBuilder.AppendKeyString("headerContains", headerContains);
		}
		if (!string.IsNullOrEmpty(descriptionContains))
		{
			jsonBuilder.AppendKeyString("descriptionContains", descriptionContains);
		}
		if (!string.IsNullOrEmpty(optSearchStr))
		{
			jsonBuilder.AppendKeyString("searchText", optSearchStr);
		}
		if (searchUser)
		{
			jsonBuilder.AppendKeyValue("searchUser", "true");
		}
		if (searchHeader)
		{
			jsonBuilder.AppendKeyValue("searchHeader", "true");
		}
		if (searchDescription)
		{
			jsonBuilder.AppendKeyValue("searchDescription", "true");
		}
		if (searchReference)
		{
			jsonBuilder.AppendKeyValue("searchReference", "true");
		}
		string body = jsonBuilder.ToJson();
		string relativeUrl = "bookmarks?task=" + searchTask;
		(string, int) tuple = PostRequestWithRetry(relativeUrl, body);
		string item = tuple.Item1;
		int num = (httpStatus = tuple.Item2);
		if (item != null)
		{
			JsonObject jsonObject = JsonObject.Parse(item);
			if (jsonObject != null)
			{
				JsonObject child = jsonObject.GetChild("array");
				if (num == 200 && child != null)
				{
					List<BookmarkDataModel> list = new List<BookmarkDataModel>();
					foreach (JsonObject child2 in child.GetChildren())
					{
						list.Add(JsonToBookmarkDataModel(child2));
					}
					return list.ToArray();
				}
			}
			throw new MIPException(string.Format(CultureInfo.InvariantCulture, "Bookmark: Unexpected HTTP status code: {0}", httpStatus));
		}
		return null;
	}

	private static BookmarkDataModel JsonToBookmarkDataModel(JsonObject json)
	{
		string g = json.GetChild("relations").GetChild("self").GetString("id");
		string g2 = json.GetChild("devicePath").GetString("id");
		return new BookmarkDataModel
		{
			reference = json.GetString("reference"),
			timeBegin = DateTime.Parse(json.GetDateTime("timeBegin"), CultureInfo.InvariantCulture),
			timeTriggered = DateTime.Parse(json.GetDateTime("timeTriggered"), CultureInfo.InvariantCulture),
			timeEnd = DateTime.Parse(json.GetDateTime("timeEnd"), CultureInfo.InvariantCulture),
			header = json.GetString("header"),
			description = json.GetString("description"),
			devicePath = new PathDataModel
			{
				id = new Guid(g2),
				type = "cameras"
			},
			user = json.GetString("user"),
			relations = new RelationsDataModel
			{
				self = new PathDataModel
				{
					id = new Guid(g),
					type = "bookmarks"
				}
			}
		};
	}
}
