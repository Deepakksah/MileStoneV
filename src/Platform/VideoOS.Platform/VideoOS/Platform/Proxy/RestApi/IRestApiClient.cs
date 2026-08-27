using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Proxy.RestApi;

public interface IRestApiClient
{
	bool EnableCache { get; set; }

	void Initialize(LoginSettings loginSettings, bool debugLog);

	void ClearCache();

	void ClearCache(string resourceType);

	JsonObject GetSpecificResource(string resourceType, string id, bool askResources);

	JsonObject GetSpecificResource(string resourceType, string id, string childResourceType, string childId);

	JsonObject GetSpecificResourceLastModifiedOnly(string resourceType, string id);

	JsonObject GetResources(string resourceType);

	JsonObject GetResources(string resourceType, string id, string childResourceType);

	JsonObject GetResourcesUrl(string url);

	JsonObject Patch(string resourceType, string id, string json);

	JsonObject Patch(string parentResourceType, string parentId, string resourceType, string id, string json);

	JsonObject Delete(string resourceType, string id);

	JsonObject Post(string resourceType, string id, string methodId, string json, out int httpStatus);

	JsonObject Post(string parentResourceType, string parentId, string resourceType, string id, string methodId, string json, out int httpStatus);

	JsonObject Put(string resourceType, string id, string json);

	void Dispose();
}
