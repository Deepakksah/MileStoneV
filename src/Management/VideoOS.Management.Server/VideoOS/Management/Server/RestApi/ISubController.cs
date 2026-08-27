using System.Net;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

public interface ISubController
{
	ResourceEntry ResourceEntry { get; set; }

	string GetArrayOfItems(IRestApiRequestContext context);

	int GetCountOfItems(IRestApiRequestContext context);

	void LoadSpecificItem(IRestApiRequestContext context, string pathId);

	void LoadSpecificChildItem(IRestApiRequestContext context, string resourceNamePlural, string id, string childId);

	string SerializeLoadedItem(IRestApiRequestContext context);

	string Delete(IRestApiRequestContext context, string pathId);

	string Delete(IRestApiRequestContext context, string resourceNamePlural, string id, string childId);

	string GetArrayOfChildItems(IRestApiRequestContext context, string resourceNamePlural, string id, string childType);

	int GetCountOfChildItems(IRestApiRequestContext context, string resourceNamePlural, string id, string childType);

	HttpStatusCode Post(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb);

	HttpStatusCode Post(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string id);

	HttpStatusCode Post(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string id, string childResourceType);

	HttpStatusCode Patch(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string id);

	HttpStatusCode Patch(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string resourceNamePlural, string id, string childId);

	HttpStatusCode Put(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string id);

	HttpStatusCode Put(IRestApiRequestContext context, ElementList elementList, JsonBuilder jb, string resourceNamePlural, string id, string childId);

	string GetTaskListInJson(IRestApiRequestContext context);

	string GetTaskListInJson(IRestApiRequestContext context, ResourceEntry parentResource, string parentId);

	HttpStatusCode TaskParams(IRestApiRequestContext context, string pathId, JsonBuilder jb);

	string GetDefinitions(IRestApiRequestContext context);

	string GetChildResources(IRestApiRequestContext context);

	string GetChildItemsDefinitions(IRestApiRequestContext context);
}
