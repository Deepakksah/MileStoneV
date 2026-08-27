using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Platform.Proxy.EvidenceLockApi;

public class EvidenceLockApiClient : RestApiBase, IEvidenceLockApiClient
{
	private const string MyResourceName = "evidenceLocks";

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
		throw new CommunicationMIPException("Server does not support /evidenceLocks RestAPI");
	}

	public void Close()
	{
		Dispose();
	}

	public EvidenceLockDataModel EvidenceLockGet(Guid evidenceLockId)
	{
		string relativeUrl = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", "evidenceLocks", evidenceLockId);
		string requestWithRetry = GetRequestWithRetry(relativeUrl);
		if (requestWithRetry != null)
		{
			return JsonToEvidenceLockDataModel(new JsonObject(requestWithRetry).GetChild("data"));
		}
		return null;
	}

	public void EvidenceLockUpdate(Guid evidenceLockId, Guid[] deviceIds, DateTime? startTime, DateTime? tagTime, DateTime? endTime, string reference, string header, string description, int retentionUnits, string retentionType, DateTime? retentionExpire)
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		JsonBuilder jsonBuilder2 = new JsonBuilder(array: true);
		for (int i = 0; i < deviceIds.Length; i++)
		{
			Guid guid = deviceIds[i];
			JsonBuilder jsonBuilder3 = new JsonBuilder(array: false);
			jsonBuilder3.AppendKeyString("type", "cameras");
			jsonBuilder3.AppendKeyString("id", guid.ToString());
			jsonBuilder2.AppendElement(jsonBuilder3.ToJson());
		}
		jsonBuilder.AppendKeyValue("devicePaths", jsonBuilder2.ToJson());
		if (startTime.HasValue)
		{
			jsonBuilder.AppendKeyString("startTime", startTime.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (tagTime.HasValue)
		{
			jsonBuilder.AppendKeyString("tagTime", tagTime.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (endTime.HasValue)
		{
			jsonBuilder.AppendKeyString("endTime", endTime.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		jsonBuilder.AppendKeyString("header", header);
		jsonBuilder.AppendKeyString("description", description);
		jsonBuilder.AppendKeyString("reference", reference);
		if (retentionType != null && retentionUnits != 0)
		{
			JsonBuilder jsonBuilder4 = new JsonBuilder(array: false);
			jsonBuilder4.AppendKeyString("retentionType", retentionType);
			jsonBuilder4.AppendKeyValue("retentionUnits", retentionUnits.ToString(CultureInfo.InvariantCulture));
			jsonBuilder.AppendKeyValue("retentionOption", jsonBuilder4.ToString());
		}
		if (retentionExpire.HasValue)
		{
			jsonBuilder.AppendKeyString("retentionExpire", retentionExpire.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		string body = jsonBuilder.ToJson();
		string relativeUrl = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", "evidenceLocks", evidenceLockId);
		PatchRequestWithRetry(relativeUrl, body);
	}

	public EvidenceLockDataModel EvidenceLockCreate(Guid evidenceLockId, Guid[] deviceIds, DateTime startTime, DateTime tagTime, DateTime endTime, string reference, string header, string description, DateTime retentionExpire, int retentionUnits = 0, string retentionType = null)
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		JsonBuilder jsonBuilder2 = new JsonBuilder(array: true);
		for (int i = 0; i < deviceIds.Length; i++)
		{
			Guid guid = deviceIds[i];
			JsonBuilder jsonBuilder3 = new JsonBuilder(array: false);
			jsonBuilder3.AppendKeyString("type", "cameras");
			jsonBuilder3.AppendKeyString("id", guid.ToString());
			jsonBuilder2.AppendElement(jsonBuilder3.ToJson());
		}
		jsonBuilder.AppendKeyValue("devicePaths", jsonBuilder2.ToJson());
		jsonBuilder.AppendKeyString("id", evidenceLockId.ToString());
		jsonBuilder.AppendKeyString("startTime", startTime.ToString("o", CultureInfo.InvariantCulture));
		jsonBuilder.AppendKeyString("tagTime", tagTime.ToString("o", CultureInfo.InvariantCulture));
		jsonBuilder.AppendKeyString("endTime", endTime.ToString("o", CultureInfo.InvariantCulture));
		jsonBuilder.AppendKeyString("reference", reference);
		jsonBuilder.AppendKeyString("header", header);
		jsonBuilder.AppendKeyString("description", description);
		JsonBuilder jsonBuilder4 = new JsonBuilder(array: false);
		jsonBuilder4.AppendKeyString("retentionType", retentionType);
		jsonBuilder4.AppendKeyValue("retentionUnits", retentionUnits.ToString(CultureInfo.InvariantCulture));
		jsonBuilder.AppendKeyValue("retentionOption", jsonBuilder4.ToJson());
		jsonBuilder.AppendKeyString("retentionExpire", retentionExpire.ToString("o", CultureInfo.InvariantCulture));
		string body = jsonBuilder.ToJson();
		string relativeUrl = "evidenceLocks";
		var (text, num) = PostRequestWithRetry(relativeUrl, body);
		if (text != null)
		{
			JsonObject jsonObject = JsonObject.Parse(text);
			if (jsonObject != null)
			{
				JsonObject child = jsonObject.GetChild("result");
				if (num == 201 && child != null)
				{
					return JsonToEvidenceLockDataModel(child);
				}
			}
			throw new MIPException(string.Format(CultureInfo.InvariantCulture, "Bookmark: Unexpected HTTP status code: {0}", num));
		}
		return null;
	}

	public EvidenceLockDataModel[] EvidenceLockSearch(Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerContains, string descriptionContains, string[] users, DateTime? createdFromTime, DateTime? createdToTime, DateTime? markedDataFromTime, DateTime? markedDataToTime, DateTime? tagTimeFromTime, DateTime? tagTimeToTime, DateTime? retentionExpireFrom, DateTime? retentionExpireTo, int pageIndex, int pageSize, string sortOrder, bool sortAscending)
	{
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		if (deviceIds != null && deviceIds.Any())
		{
			JsonBuilder jsonBuilder2 = new JsonBuilder(array: true);
			for (int i = 0; i < deviceIds.Length; i++)
			{
				Guid guid = deviceIds[i];
				JsonBuilder jsonBuilder3 = new JsonBuilder(array: false);
				jsonBuilder3.AppendKeyString("type", "cameras");
				jsonBuilder3.AppendKeyString("id", guid.ToString());
				jsonBuilder2.AppendElement(jsonBuilder3.ToJson());
			}
			jsonBuilder.AppendKeyValue("devicePaths", jsonBuilder2.ToJson());
		}
		jsonBuilder.AppendKeyString("searchText", searchText);
		jsonBuilder.AppendKeyValue("searchHeader", searchHeader.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture));
		jsonBuilder.AppendKeyValue("searchDescription", searchDescription.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture));
		jsonBuilder.AppendKeyValue("searchReference", searchReference.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture));
		jsonBuilder.AppendKeyValue("searchUser", searchUser.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture));
		jsonBuilder.AppendKeyString("headerContains", headerContains);
		jsonBuilder.AppendKeyString("descriptionContains", descriptionContains);
		if (users != null && users.Any())
		{
			JsonBuilder jsonBuilder4 = new JsonBuilder(array: true);
			foreach (string elementWithBrackets in users)
			{
				jsonBuilder4.AppendElement(elementWithBrackets);
			}
			jsonBuilder.AppendKeyValue("users", jsonBuilder4.ToJson());
		}
		if (createdFromTime.HasValue)
		{
			jsonBuilder.AppendKeyString("createdFromTime", createdFromTime.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (createdToTime.HasValue)
		{
			jsonBuilder.AppendKeyString("createdToTime", createdToTime.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (markedDataFromTime.HasValue)
		{
			jsonBuilder.AppendKeyString("fromTime", markedDataFromTime.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (markedDataToTime.HasValue)
		{
			jsonBuilder.AppendKeyString("toTime", markedDataToTime.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (tagTimeFromTime.HasValue)
		{
			jsonBuilder.AppendKeyString("tagFromTime", tagTimeFromTime.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (tagTimeToTime.HasValue)
		{
			jsonBuilder.AppendKeyString("tagToTime", tagTimeToTime.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (retentionExpireFrom.HasValue)
		{
			jsonBuilder.AppendKeyString("expireFromTime", retentionExpireFrom.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (retentionExpireTo.HasValue)
		{
			jsonBuilder.AppendKeyString("expireToTime", retentionExpireTo.Value.ToString("o", CultureInfo.InvariantCulture));
		}
		if (sortOrder != null)
		{
			jsonBuilder.AppendKeyString("sortOrder", sortOrder);
		}
		jsonBuilder.AppendKeyValue("sortAscending", sortAscending.ToString(CultureInfo.InvariantCulture).ToLower(CultureInfo.InvariantCulture));
		string relativeUrl = string.Format(CultureInfo.InvariantCulture, "{0}?task=searchTime&page={1}&size={2}", "evidenceLocks", pageIndex, pageSize);
		string body = jsonBuilder.ToJson();
		var (text, num) = PostRequestWithRetry(relativeUrl, body);
		if (text != null)
		{
			JsonObject jsonObject = JsonObject.Parse(text);
			if (jsonObject != null)
			{
				JsonObject child = jsonObject.GetChild("array");
				if (num == 200 && child != null)
				{
					List<EvidenceLockDataModel> list = new List<EvidenceLockDataModel>();
					foreach (JsonObject child2 in child.GetChildren())
					{
						list.Add(JsonToEvidenceLockDataModel(child2));
					}
					return list.ToArray();
				}
			}
			throw new MIPException(string.Format(CultureInfo.InvariantCulture, "EvidenceLock: Unexpected HTTP status code: {0}", num));
		}
		return null;
	}

	public string EvidenceLockGetNewReference()
	{
		return "MarkedDataRef";
	}

	public void EvidenceLockDelete(Guid evidenceLockId)
	{
		string relativeUrl = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", "evidenceLocks", evidenceLockId);
		DeleteRequestWithRetry(relativeUrl);
	}

	private static EvidenceLockDataModel JsonToEvidenceLockDataModel(JsonObject json)
	{
		string text = json.GetChild("relations").GetChild("self").GetString("id");
		string reference = json.GetString("reference");
		DateTime value = DateTime.Parse(json.GetDateTime("startTime"), CultureInfo.InvariantCulture);
		DateTime value2 = DateTime.Parse(json.GetDateTime("tagTime"), CultureInfo.InvariantCulture);
		DateTime value3 = DateTime.Parse(json.GetDateTime("endTime"), CultureInfo.InvariantCulture);
		string header = json.GetString("header");
		string description = json.GetString("description");
		string user = json.GetString("user");
		DateTime retentionExpire = DateTime.Parse(json.GetDateTime("retentionExpire"), CultureInfo.InvariantCulture);
		DateTime created = DateTime.Parse(json.GetDateTime("Created"), CultureInfo.InvariantCulture);
		DateTime lastModified = DateTime.Parse(json.GetDateTime("LastModified"), CultureInfo.InvariantCulture);
		double num = json.GetDouble("retentionSize");
		RelationsDataModel relations = new RelationsDataModel
		{
			self = new PathDataModel
			{
				id = new Guid(text),
				type = "evidenceLocks"
			}
		};
		JsonObject child = json.GetChild("devicePaths");
		List<PathDataModel> list = new List<PathDataModel>();
		if (child != null)
		{
			foreach (JsonObject child3 in child.GetChildren())
			{
				string type = child3.GetString("type");
				Guid id = new Guid(child3.GetString("id"));
				PathDataModel item = new PathDataModel
				{
					type = type,
					id = id
				};
				list.Add(item);
			}
		}
		int? retentionUnits = null;
		string retentionType = null;
		JsonObject child2 = json.GetChild("retentionOption");
		if (child2 != null)
		{
			retentionUnits = child2.GetInteger("retentionUnits");
			retentionType = child2.GetString("retentionType");
		}
		return new EvidenceLockDataModel
		{
			id = text,
			devicePaths = list.ToArray(),
			user = user,
			reference = reference,
			startTime = value,
			tagTime = value2,
			endTime = value3,
			header = header,
			description = description,
			relations = relations,
			created = created,
			lastModified = lastModified,
			retentionExpire = retentionExpire,
			retentionSize = (long)num,
			retentionOption = new RetentionOptionDataModel
			{
				retentionUnits = retentionUnits,
				retentionType = retentionType
			}
		};
	}
}
