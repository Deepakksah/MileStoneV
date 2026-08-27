using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Management.Server.Properties;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server.RestApi;

[Route("api/evidenceLocks/{pathId?}")]
public class EvidenceLocksController : ApiController
{
	public class EvidenceLockDataModel
	{
		public string description { get; set; }

		public string id { get; set; }

		public DateTime created { get; set; }

		public DateTime lastModified { get; set; }

		public DateTime? startTime { get; set; }

		public DateTime? tagTime { get; set; }

		public DateTime? endTime { get; set; }

		public string header { get; set; }

		public string reference { get; set; }

		public string user { get; set; }

		public DateTime retentionExpire { get; set; }

		public long retentionSize { get; set; }

		public RetentionOptionDataModel retentionOption { get; set; }

		public PathDataModel[] devicePaths { get; set; }

		public RelationsDataModel relations { get; set; }
	}

	public class PatchEvidenceLockDataModel
	{
		public string description { get; set; }

		public DateTime? startTime { get; set; }

		public DateTime? endTime { get; set; }

		public DateTime? tagTime { get; set; }

		public string header { get; set; }

		public string reference { get; set; }

		public DateTime? retentionExpire { get; set; }

		public RetentionOptionDataModel retentionOption { get; set; }

		public PathDataModel[] devicePaths { get; set; }
	}

	public class RetentionOptionDataModel
	{
		public int? retentionUnits { get; set; }

		public string retentionType { get; set; }
	}

	private readonly IRestApiRequestContextFactory _requestContextFactory;

	private static Dictionary<string, string> mediaTypesToResoure = new Dictionary<string, string>
	{
		{ "Camera", "cameras" },
		{ "Microphone", "microphones" },
		{ "Speaker", "speakers" },
		{ "", "unknown device id" }
	};

	public EvidenceLocksController()
		: this(new RestApiRequestContextFactory())
	{
	}

	public EvidenceLocksController(IRestApiRequestContextFactory requestContextFactory)
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
					{ "id", "newReference" },
					{
						"displayName",
						ConfigurationApiResources.TaskEvidenceLockNewReference
					}
				};
				list.Add(item2);
				dictionary.Add("tasks", list);
				return Ok(dictionary);
			}
			throw new ArgumentException("Unknown request");
		}
		if (!Guid.TryParse(pathId, out var result))
		{
			throw new ArgumentException("Invalid id");
		}
		MarkedDataDataContract markedDataDataContract = MarkedData.QueryMarkedDataItem(restApiRequestContext, result.ToString(), 2);
		if (markedDataDataContract == null)
		{
			throw new PathNotFoundException("evidenceLocks/" + pathId);
		}
		Dictionary<Guid, string> dictionary2 = new Dictionary<Guid, string>();
		new List<Device>();
		string[] deviceIds = markedDataDataContract.DeviceIds;
		foreach (string text in deviceIds)
		{
			Device device = Device.QueryDeviceById(restApiRequestContext, text.ToString());
			if (device != null)
			{
				dictionary2[new Guid(device.Id)] = device.DeviceType.ToString();
			}
		}
		dictionary["data"] = EvidenceLockToDataModel(markedDataDataContract, dictionary2);
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
			MarkedDataDataContract resultEvidenceLock;
			Dictionary<string, object> content = PostCreateEvidenceLock(restApiRequestContext, postContent, out resultEvidenceLock);
			return Created("evidenceLocks/" + resultEvidenceLock.Id, content);
		}
		case "searchTime":
		{
			Dictionary<string, object> content = PostSearchTime(restApiRequestContext, postContent);
			return Ok(content);
		}
		case "newReference":
			return Created("", PostCreateNewReference(restApiRequestContext));
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
		if (!Guid.TryParse(pathId, out var _))
		{
			throw new ArgumentException("Invalid id");
		}
		MarkedData.DeleteMarkedData(requestContext, pathId, MarkedDataDefinitions.MarkedDataTypes.MarkedData);
		return Ok();
	}

	[HttpPatch]
	public HttpResponseMessage Patch([FromBody] PatchEvidenceLockDataModel postContent, string pathId = null)
	{
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_0252: Expected O, but got Unknown
		using IRestApiRequestContext requestContext = _requestContextFactory.GetRequestContext(base.Request);
		if (postContent == null)
		{
			throw new ArgumentException("Incorrect formatted JSON");
		}
		if (pathId == null)
		{
			throw new ArgumentException("EvidenceLock id missing");
		}
		new Guid(pathId);
		MarkedDataDataContract markedDataDataContract = MarkedData.QueryMarkedDataItem(requestContext, pathId, 2);
		if (markedDataDataContract == null)
		{
			throw new PathNotFoundException("Evidence Lock not found");
		}
		if (postContent.startTime.HasValue)
		{
			markedDataDataContract.StartTime = postContent.startTime.Value;
		}
		if (postContent.endTime.HasValue)
		{
			markedDataDataContract.EndTime = postContent.endTime.Value;
		}
		if (postContent.tagTime.HasValue)
		{
			markedDataDataContract.TagTime = postContent.tagTime.Value;
		}
		if (postContent.header != null)
		{
			markedDataDataContract.Header = postContent.header;
		}
		if (postContent.description != null)
		{
			markedDataDataContract.Description = postContent.description;
		}
		if (postContent.reference != null)
		{
			markedDataDataContract.Reference = postContent.reference;
		}
		if (postContent.retentionOption != null)
		{
			if (postContent.retentionOption.retentionType != null)
			{
				markedDataDataContract.RetentionType = ConvertRetentionUnitsToInt(postContent.retentionOption.retentionType);
			}
			if (postContent.retentionOption.retentionUnits.HasValue)
			{
				markedDataDataContract.RetentionUnits = postContent.retentionOption.retentionUnits.Value;
			}
		}
		if (postContent.devicePaths != null)
		{
			markedDataDataContract.DeviceIds = JObjectUtil.VerifyDeviceIds(postContent.devicePaths);
		}
		if (postContent.retentionExpire.HasValue)
		{
			markedDataDataContract.RetentionExpire = postContent.retentionExpire.Value;
		}
		MarkedData.UpdateMarkedData(requestContext, pathId, markedDataDataContract.MarkedDataType, markedDataDataContract.DeviceIds, markedDataDataContract.Reference, markedDataDataContract.Header, markedDataDataContract.Description, markedDataDataContract.StartTime, markedDataDataContract.TagTime, markedDataDataContract.EndTime, markedDataDataContract.UseRetention, markedDataDataContract.RetentionType, markedDataDataContract.RetentionUnits, markedDataDataContract.RetentionExpire, out var faultDevices, out var warningDevices);
		if (faultDevices != null && faultDevices.Any())
		{
			throw new ArgumentException(faultDevices.First().Message);
		}
		HttpResponseMessage val = base.Request.CreateResponse(HttpStatusCode.OK);
		if (warningDevices != null && warningDevices.Any())
		{
			val.Content = (HttpContent)new StringContent("{ \"warning\" : \"" + warningDevices[0].Message + "\"}", Encoding.UTF8, "application/json");
		}
		return val;
	}

	private static Dictionary<string, object> PostCreateEvidenceLock(IRestApiRequestContext requestContext, JObject postContent, out MarkedDataDataContract resultEvidenceLock)
	{
		if (postContent == null)
		{
			throw new ArgumentException("Incorrect formatted JSON");
		}
		JObjectUtil.FetchDeviceIdAndTypes(postContent, out var deviceIds, out var _, out var deviceTypeIndex);
		string[] deviceIds2 = deviceIds.Select((Guid x) => x.ToString().ToUpper(CultureInfo.InvariantCulture)).ToArray();
		JToken value;
		Guid result;
		string markedDataId = ((!postContent.TryGetValue("id", out value) || !Guid.TryParse(value.Value<string>(), out result)) ? Guid.NewGuid().ToString() : value.Value<string>());
		JToken value2;
		string reference = ((!postContent.TryGetValue("reference", out value2)) ? "MarkedDataRef" : value2.ToString());
		DateTime timeValue = JObjectUtil.GetTimeValue(postContent, "startTime", null);
		DateTime timeValue2 = JObjectUtil.GetTimeValue(postContent, "tagTime", null);
		DateTime timeValue3 = JObjectUtil.GetTimeValue(postContent, "endTime", null);
		string stringValue = JObjectUtil.GetStringValue(postContent, "header", allowDefault: true);
		string stringValue2 = JObjectUtil.GetStringValue(postContent, "description", allowDefault: true);
		JObjectUtil.GetRetentionOptions(postContent, out var units, out var retentionType);
		int retentionType2 = ConvertRetentionUnitsToInt(retentionType);
		DateTime timeValue4 = JObjectUtil.GetTimeValue(postContent, "retentionExpire", null);
		if (units < 0)
		{
			throw new PropertyInvalidException("RetentionUnits must be 0 or positive", "RetentionUnits");
		}
		MarkedDataDataContract markedDataDataContract = MarkedData.CreateMarkedData(requestContext, markedDataId, deviceIds2, reference, stringValue, stringValue2, 2, timeValue, timeValue2, timeValue3, useRetention: true, retentionType2, units, timeValue4);
		Dictionary<string, object> result2 = new Dictionary<string, object> { ["result"] = EvidenceLockToDataModel(markedDataDataContract, deviceTypeIndex) };
		resultEvidenceLock = markedDataDataContract;
		return result2;
	}

	private static Dictionary<string, object> PostSearchTime(IRestApiRequestContext requestContext, JObject postContent)
	{
		if (postContent == null)
		{
			throw new ArgumentNullException("postContent");
		}
		JObjectUtil.FetchDeviceIdAndTypes(postContent, out var deviceIds, out var _, out var deviceTypeIndex);
		string[] deviceIds2 = deviceIds.Select((Guid x) => x.ToString()).ToArray();
		string stringValue = JObjectUtil.GetStringValue(postContent, "searchText", allowDefault: true);
		bool boolValue = JObjectUtil.GetBoolValue(postContent, "searchHeader");
		bool boolValue2 = JObjectUtil.GetBoolValue(postContent, "searchReference");
		bool boolValue3 = JObjectUtil.GetBoolValue(postContent, "searchUser");
		bool boolValue4 = JObjectUtil.GetBoolValue(postContent, "searchDescription");
		string[] stringListValue = JObjectUtil.GetStringListValue(postContent, "users", allowDefault: true);
		string stringValue2 = JObjectUtil.GetStringValue(postContent, "headerContains", allowDefault: true);
		string stringValue3 = JObjectUtil.GetStringValue(postContent, "descriptionContains", allowDefault: true);
		DateTime timeValue = JObjectUtil.GetTimeValue(postContent, "createdFromTime", DateTime.MinValue);
		DateTime timeValue2 = JObjectUtil.GetTimeValue(postContent, "createdToTime", DateTime.MinValue);
		DateTime timeValue3 = JObjectUtil.GetTimeValue(postContent, "fromTime", DateTime.MinValue);
		DateTime timeValue4 = JObjectUtil.GetTimeValue(postContent, "toTime", DateTime.MinValue);
		DateTime timeValue5 = JObjectUtil.GetTimeValue(postContent, "tagFromTime", DateTime.MinValue);
		DateTime timeValue6 = JObjectUtil.GetTimeValue(postContent, "tagToTime", DateTime.MinValue);
		DateTime timeValue7 = JObjectUtil.GetTimeValue(postContent, "expireFromTime", DateTime.MinValue);
		DateTime timeValue8 = JObjectUtil.GetTimeValue(postContent, "expireToTime", DateTime.MinValue);
		string sortOrder = JObjectUtil.GetStringValue(postContent, "sortOrder", allowDefault: true) ?? "Header";
		bool boolValue5 = JObjectUtil.GetBoolValue(postContent, "sortAscending", defaultValue: true);
		int pageStart = requestContext.RestApiRequestParameters.PageStart;
		int pageSize = requestContext.RestApiRequestParameters.PageSize;
		MarkedDataDataContract[] array = MarkedData.QueryMarkedDataSearch(requestContext, deviceIds2, stringValue, boolValue, boolValue4, boolValue2, boolValue3, stringListValue, stringValue2, stringValue3, timeValue, timeValue2, timeValue3, timeValue4, timeValue5, timeValue6, timeValue7, timeValue8, pageStart, pageSize, sortOrder, boolValue5);
		List<object> list = new List<object>();
		MarkedDataDataContract[] array2 = array;
		foreach (MarkedDataDataContract evidenceLock in array2)
		{
			list.Add(EvidenceLockToDataModel(evidenceLock, deviceTypeIndex));
		}
		return new Dictionary<string, object> { 
		{
			"array",
			list.ToArray()
		} };
	}

	private static Dictionary<string, object> PostCreateNewReference(IRestApiRequestContext requestContext)
	{
		BookmarkReference reference = new BookmarkReference
		{
			Reference = "MarkedDataRef",
			Time = DateTime.UtcNow
		};
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

	private static int ConvertRetentionUnitsToInt(string unitsName)
	{
		if (string.IsNullOrEmpty(unitsName))
		{
			return 0;
		}
		if (!Enum.TryParse<OptionType>(unitsName, out var result))
		{
			throw new PropertyInvalidException("Unknown value", "retentionType");
		}
		return (int)result;
	}

	private static string ConvertRetentionUnitsToString(int unitsIndex)
	{
		OptionType optionType = (OptionType)unitsIndex;
		return optionType.ToString();
	}

	private static EvidenceLockDataModel EvidenceLockToDataModel(MarkedDataDataContract evidenceLock, Dictionary<Guid, string> mediaTypes)
	{
		PathDataModel self = new PathDataModel
		{
			Type = "evidenceLocks",
			Id = evidenceLock.Id.ToString()
		};
		RelationsDataModel relations = new RelationsDataModel
		{
			self = self
		};
		List<PathDataModel> list = new List<PathDataModel>();
		string[] deviceIds = evidenceLock.DeviceIds;
		foreach (string text in deviceIds)
		{
			string value = "cameras";
			Guid key = new Guid(text);
			if (mediaTypes.TryGetValue(key, out var value2))
			{
				mediaTypesToResoure.TryGetValue(value2, out value);
			}
			PathDataModel item = new PathDataModel
			{
				Type = value,
				Id = text
			};
			list.Add(item);
		}
		return new EvidenceLockDataModel
		{
			id = evidenceLock.Id,
			devicePaths = list.ToArray(),
			user = evidenceLock.UserName,
			reference = evidenceLock.Reference,
			startTime = evidenceLock.StartTime,
			tagTime = evidenceLock.TagTime,
			endTime = evidenceLock.EndTime,
			header = evidenceLock.Header,
			description = evidenceLock.Description,
			relations = relations,
			created = evidenceLock.Created,
			lastModified = evidenceLock.LastModified,
			retentionExpire = evidenceLock.RetentionExpire,
			retentionSize = evidenceLock.RetentionSize,
			retentionOption = new RetentionOptionDataModel
			{
				retentionUnits = evidenceLock.RetentionUnits,
				retentionType = ConvertRetentionUnitsToString(evidenceLock.RetentionType)
			}
		};
	}
}
