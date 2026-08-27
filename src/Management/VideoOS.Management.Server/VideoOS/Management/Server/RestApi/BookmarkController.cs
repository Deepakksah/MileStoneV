using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Management.Server.Properties;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server.RestApi;

[Route("api/bookmarks/{pathId?}")]
public class BookmarkController : ApiController
{
	private readonly IRestApiRequestContextFactory _requestContextFactory;

	private static Dictionary<string, string> mediaTypesToResoure = new Dictionary<string, string>
	{
		{ "Camera", "cameras" },
		{ "Microphone", "microphones" },
		{ "Speaker", "speakers" },
		{ "", "unknown device id" }
	};

	public BookmarkController()
		: this(new RestApiRequestContextFactory())
	{
	}

	public BookmarkController(IRestApiRequestContextFactory requestContextFactory)
	{
		_requestContextFactory = requestContextFactory;
	}

	[HttpGet]
	public IHttpActionResult Get(string pathId = null)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		using IRestApiRequestContext restApiRequestContext = _requestContextFactory.GetRequestContext(base.Request);
		if (pathId == null)
		{
			if (restApiRequestContext.RestApiRequestParameters.Tasks)
			{
				List<object> list = new List<object>();
				Dictionary<string, object> item = new Dictionary<string, object>
				{
					{ "id", "searchTime" },
					{
						"displayName",
						ConfigurationApiResources.TaskBookmarkSearchTime
					}
				};
				list.Add(item);
				Dictionary<string, object> item2 = new Dictionary<string, object>
				{
					{ "id", "searchFromBookmark" },
					{
						"displayName",
						ConfigurationApiResources.TaskBookmarkSearchFromBookmark
					}
				};
				list.Add(item2);
				Dictionary<string, object> item3 = new Dictionary<string, object>
				{
					{ "id", "newReference" },
					{
						"displayName",
						ConfigurationApiResources.TaskBookmarkNewReference
					}
				};
				list.Add(item3);
				dictionary.Add("tasks", list);
				return Ok(dictionary);
			}
			throw new ArgumentException("Unknown request");
		}
		if (!Guid.TryParse(pathId, out var result))
		{
			throw new ArgumentException("Invalid id");
		}
		Bookmark bookmark = ServerCommandServiceBookmarkHandling.BookmarkGet(restApiRequestContext, result);
		if (bookmark == null)
		{
			throw new PathNotFoundException();
		}
		Dictionary<Guid, string> dictionary2 = new Dictionary<Guid, string>();
		Device device = Device.QueryDeviceById(restApiRequestContext, bookmark.DeviceId.ToString());
		if (device != null)
		{
			dictionary2[new Guid(device.Id)] = device.DeviceType.ToString();
		}
		dictionary["data"] = BookmarkToDataModel(bookmark, dictionary2, device?.Name ?? "");
		return Ok(dictionary);
	}

	[HttpPost]
	public IHttpActionResult Post([FromBody] JObject postContent, string pathId = null)
	{
		using IRestApiRequestContext restApiRequestContext = _requestContextFactory.GetRequestContext(base.Request);
		if (pathId != null)
		{
			throw new ArgumentException("No support for using URL with an id");
		}
		switch (restApiRequestContext.RestApiRequestParameters.Task)
		{
		case null:
		{
			Bookmark resultBookmark;
			Dictionary<string, object> content = PostCreateBookmark(restApiRequestContext, postContent, out resultBookmark);
			return Created($"bookmarks/{resultBookmark.Id}", content);
		}
		case "searchTime":
		{
			Dictionary<string, object> content = PostSearchTime(restApiRequestContext, postContent);
			return Ok(content);
		}
		case "searchFromBookmark":
		{
			Dictionary<string, object> content = PostSearchFromBookmark(restApiRequestContext, postContent);
			return Ok(content);
		}
		case "newReference":
			return Created("", PostCreateNewReference(restApiRequestContext, postContent));
		default:
			throw new ArgumentException("Unknown task");
		}
	}

	[HttpDelete]
	public IHttpActionResult Delete(string pathId = null)
	{
		using IRestApiRequestContext requestContext = _requestContextFactory.GetRequestContext(base.Request);
		if (pathId == null)
		{
			throw new ArgumentNullException("pathId");
		}
		if (!Guid.TryParse(pathId, out var result))
		{
			throw new ArgumentException("Invalid id");
		}
		ServerCommandServiceBookmarkHandling.BookmarkDelete(requestContext, result);
		return Ok();
	}

	[HttpPatch]
	public IHttpActionResult Patch([FromBody] PatchBookmarkDataModel postContent, string pathId = null)
	{
		using IRestApiRequestContext requestContext = _requestContextFactory.GetRequestContext(base.Request);
		if (postContent == null)
		{
			throw new ArgumentException("Incorrect formatted JSON");
		}
		if (pathId == null)
		{
			throw new ArgumentException("Bookmark id missing");
		}
		Guid bookmarkId = new Guid(pathId);
		Bookmark bookmark = ServerCommandServiceBookmarkHandling.BookmarkGet(requestContext, bookmarkId);
		if (bookmark == null)
		{
			throw new PathNotFoundException("Bookmark not found");
		}
		if (postContent.timeBegin.HasValue)
		{
			bookmark.TimeBegin = postContent.timeBegin.Value;
		}
		if (postContent.timeEnd.HasValue)
		{
			bookmark.TimeEnd = postContent.timeEnd.Value;
		}
		if (postContent.timeTriggered.HasValue)
		{
			bookmark.TimeTrigged = postContent.timeTriggered.Value;
		}
		if (postContent.header != null)
		{
			bookmark.Header = postContent.header;
		}
		if (postContent.description != null)
		{
			bookmark.Description = postContent.description;
		}
		if (postContent.reference != null)
		{
			bookmark.Reference = postContent.reference;
		}
		ServerCommandServiceBookmarkHandling.BookmarkUpdate(requestContext, bookmarkId, bookmark.TimeBegin, bookmark.TimeTrigged, bookmark.TimeEnd, bookmark.Reference, bookmark.Header, bookmark.Description);
		return Ok();
	}

	private static Dictionary<string, object> PostCreateBookmark(IRestApiRequestContext requestContext, JObject postContent, out Bookmark resultBookmark)
	{
		Guid deviceId = JObjectUtil.VerifyDeviceId(postContent);
		DateTime timeValue = JObjectUtil.GetTimeValue(postContent, "timeTriggered", DateTime.UtcNow);
		DateTime timeValue2 = JObjectUtil.GetTimeValue(postContent, "timeBegin", timeValue);
		DateTime timeValue3 = JObjectUtil.GetTimeValue(postContent, "timeEnd", timeValue);
		string text = null;
		if (postContent.TryGetValue("reference", out JToken value))
		{
			text = value.ToString();
		}
		string stringValue = JObjectUtil.GetStringValue(postContent, "header", allowDefault: true);
		string stringValue2 = JObjectUtil.GetStringValue(postContent, "description", allowDefault: true);
		if (string.IsNullOrWhiteSpace(text))
		{
			text = ServerCommandServiceBookmarkHandling.BookmarkGetNewReference(requestContext, deviceId, live: true).Reference;
		}
		Bookmark bookmark = ServerCommandServiceBookmarkHandling.BookmarkCreate(requestContext, deviceId, timeValue2, timeValue, timeValue3, text, stringValue, stringValue2);
		Device device = Device.QueryDeviceById(requestContext, bookmark.DeviceId.ToString());
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<Guid, string> dictionary2 = new Dictionary<Guid, string>();
		if (device != null)
		{
			dictionary2[new Guid(device.Id)] = device.DeviceType.ToString();
		}
		dictionary["result"] = BookmarkToDataModel(bookmark, dictionary2, device?.Name ?? "");
		resultBookmark = bookmark;
		return dictionary;
	}

	private static Dictionary<string, object> PostSearchTime(IRestApiRequestContext requestContext, JObject postContent)
	{
		DateTime dateTime = JObjectUtil.GetTimeValue(postContent, "time", null);
		if (dateTime.Kind != DateTimeKind.Utc)
		{
			dateTime = dateTime.ToUniversalTime();
		}
		DateTime timeValue = JObjectUtil.GetTimeValue(postContent, "endTime", DateTime.MinValue);
		TimeDuration timeLimit = ((!(timeValue != DateTime.MinValue)) ? new TimeDuration
		{
			MicroSeconds = JObjectUtil.GetTimeSpanValue(postContent, "timeLimit").Ticks / 10
		} : new TimeDuration
		{
			MicroSeconds = (timeValue - dateTime).Ticks / 10
		});
		int numberValue = JObjectUtil.GetNumberValue(postContent, "countLimit");
		if (numberValue == 0)
		{
			throw new ArgumentException("countLimit is 0");
		}
		string[] stringListValue = JObjectUtil.GetStringListValue(postContent, "users", allowDefault: true);
		JObjectUtil.FetchDeviceIdAndTypes(postContent, out var deviceIds, out var mediaTypes, out var _);
		if (deviceIds.Length == 0 && mediaTypes.Length == 0)
		{
			throw new ArgumentException("Either resourceTypes or devicePaths must be provided");
		}
		string stringValue = JObjectUtil.GetStringValue(postContent, "searchText", allowDefault: true);
		bool boolValue = JObjectUtil.GetBoolValue(postContent, "searchHeader");
		bool boolValue2 = JObjectUtil.GetBoolValue(postContent, "searchReference");
		bool boolValue3 = JObjectUtil.GetBoolValue(postContent, "searchUser");
		bool boolValue4 = JObjectUtil.GetBoolValue(postContent, "searchDescription");
		string stringValue2 = JObjectUtil.GetStringValue(postContent, "headerContains", allowDefault: true);
		string stringValue3 = JObjectUtil.GetStringValue(postContent, "descriptionContains", allowDefault: true);
		Bookmark[] array = ServerCommandServiceBookmarkHandling.BookmarkSearch(requestContext, dateTime, timeLimit, numberValue, mediaTypes, deviceIds, stringListValue, stringValue, boolValue, boolValue4, boolValue2, boolValue3, stringValue2, stringValue3, null);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<Guid, string> dictionary2 = new Dictionary<Guid, string>();
		Dictionary<Guid, string> dictionary3 = new Dictionary<Guid, string>();
		Bookmark[] array2;
		if (array.Any())
		{
			array2 = array;
			foreach (Bookmark bookmark in array2)
			{
				if (!dictionary2.ContainsKey(bookmark.DeviceId))
				{
					dictionary2.Add(bookmark.DeviceId, "");
				}
			}
			Device[] array3 = Device.QueryDevicesByIds(requestContext, dictionary2.Keys.Select((Guid k) => k.ToString()).ToArray());
			foreach (Device device in array3)
			{
				dictionary2[new Guid(device.Id)] = device.DeviceType.ToString();
				dictionary3[new Guid(device.Id)] = device.Name;
			}
		}
		List<object> list = new List<object>();
		array2 = array;
		foreach (Bookmark bookmark2 in array2)
		{
			string value = "";
			dictionary3.TryGetValue(bookmark2.DeviceId, out value);
			list.Add(BookmarkToDataModel(bookmark2, dictionary2, value));
		}
		dictionary["array"] = list.ToArray();
		return dictionary;
	}

	private static Dictionary<string, object> PostSearchFromBookmark(IRestApiRequestContext requestContext, JObject postContent)
	{
		string stringValue = JObjectUtil.GetStringValue(postContent, "fromBookmarkId", allowDefault: true);
		if (string.IsNullOrEmpty(stringValue) || !Guid.TryParse(stringValue, out var result))
		{
			throw new ArgumentException("fromBookmarkId is required");
		}
		DateTime timeValue = JObjectUtil.GetTimeValue(postContent, "endTime", DateTime.MinValue);
		TimeDuration timeLimit;
		if (timeValue != DateTime.MinValue)
		{
			Bookmark bookmark = ServerCommandServiceBookmarkHandling.BookmarkGet(requestContext, result);
			if (bookmark == null)
			{
				throw new ArgumentException(stringValue + " is not a valid bookmark id");
			}
			timeLimit = new TimeDuration
			{
				MicroSeconds = (long)(timeValue - bookmark.TimeTrigged).TotalMilliseconds * 1000
			};
		}
		else
		{
			timeLimit = new TimeDuration
			{
				MicroSeconds = JObjectUtil.GetTimeSpanValue(postContent, "timeLimit").Ticks / 10
			};
		}
		int numberValue = JObjectUtil.GetNumberValue(postContent, "countLimit");
		if (numberValue == 0)
		{
			throw new ArgumentException("countLimit is 0");
		}
		string[] stringListValue = JObjectUtil.GetStringListValue(postContent, "users", allowDefault: true);
		JObjectUtil.FetchDeviceIdAndTypes(postContent, out var deviceIds, out var mediaTypes, out var _);
		if (deviceIds.Length == 0 && mediaTypes.Length == 0)
		{
			throw new ArgumentException("Either resourceTypes or devicePaths must be provided");
		}
		string stringValue2 = JObjectUtil.GetStringValue(postContent, "searchText", allowDefault: true);
		bool boolValue = JObjectUtil.GetBoolValue(postContent, "searchHeader");
		bool boolValue2 = JObjectUtil.GetBoolValue(postContent, "searchReference");
		bool boolValue3 = JObjectUtil.GetBoolValue(postContent, "searchUser");
		bool boolValue4 = JObjectUtil.GetBoolValue(postContent, "searchDescription");
		string stringValue3 = JObjectUtil.GetStringValue(postContent, "headerContains", allowDefault: true);
		string stringValue4 = JObjectUtil.GetStringValue(postContent, "descriptionContains", allowDefault: true);
		Bookmark[] array = ServerCommandServiceBookmarkHandling.BookmarkSearch(requestContext, DateTime.MinValue, timeLimit, numberValue, mediaTypes, deviceIds, stringListValue, stringValue2, boolValue, boolValue4, boolValue2, boolValue3, stringValue3, stringValue4, stringValue);
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		Dictionary<Guid, string> dictionary2 = new Dictionary<Guid, string>();
		Dictionary<Guid, string> dictionary3 = new Dictionary<Guid, string>();
		Bookmark[] array2;
		if (array.Any())
		{
			array2 = array;
			foreach (Bookmark bookmark2 in array2)
			{
				if (!dictionary2.ContainsKey(bookmark2.DeviceId))
				{
					dictionary2.Add(bookmark2.DeviceId, "");
				}
			}
			Device[] array3 = Device.QueryDevicesByIds(requestContext, dictionary2.Keys.Select((Guid k) => k.ToString()).ToArray());
			foreach (Device device in array3)
			{
				dictionary2[new Guid(device.Id)] = device.DeviceType.ToString();
				dictionary3[new Guid(device.Id)] = device.Name;
			}
		}
		List<object> list = new List<object>();
		array2 = array;
		foreach (Bookmark bookmark3 in array2)
		{
			string value = "";
			dictionary3.TryGetValue(bookmark3.DeviceId, out value);
			list.Add(BookmarkToDataModel(bookmark3, dictionary2, value));
		}
		dictionary["array"] = list.ToArray();
		return dictionary;
	}

	private static Dictionary<string, object> PostCreateNewReference(IRestApiRequestContext requestContext, JObject postContent)
	{
		bool boolValue = JObjectUtil.GetBoolValue(postContent, "live");
		BookmarkReference reference = ServerCommandServiceBookmarkHandling.BookmarkGetNewReference(requestContext, JObjectUtil.VerifyDeviceId(postContent), boolValue);
		return new Dictionary<string, object> { ["result"] = ReferenceToJson(reference) };
	}

	private static NewBookmarkReferenceDataModel ReferenceToJson(BookmarkReference reference)
	{
		return new NewBookmarkReferenceDataModel
		{
			time = PropertyTypesHelper.GetValue(reference.Time),
			reference = reference.Reference
		};
	}

	private static BookmarkDataModel BookmarkToDataModel(Bookmark bookmark, Dictionary<Guid, string> mediaTypes, string deviceName)
	{
		PathDataModel self = new PathDataModel
		{
			Type = "bookmarks",
			Id = bookmark.Id.ToString()
		};
		RelationsDataModel relations = new RelationsDataModel
		{
			self = self
		};
		PathDataModel devicePath = new PathDataModel
		{
			Type = mediaTypesToResoure[mediaTypes[bookmark.DeviceId]],
			Id = bookmark.DeviceId.ToString()
		};
		return new BookmarkDataModel
		{
			devicePath = devicePath,
			user = bookmark.User,
			reference = bookmark.Reference,
			timeBegin = bookmark.TimeBegin,
			timeTriggered = bookmark.TimeTrigged,
			timeEnd = bookmark.TimeEnd,
			header = bookmark.Header,
			description = bookmark.Description,
			relations = relations,
			deviceName = deviceName
		};
	}
}
