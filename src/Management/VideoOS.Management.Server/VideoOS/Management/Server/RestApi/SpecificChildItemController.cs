using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

[Route("rest/{resourceNamePlural}/{id}/{childResourceNamePlural}/{childId}")]
public class SpecificChildItemController : ApiController
{
	public INameConvert NameConvert { get; set; } = new NameConvert();

	public IRestApiRequestContextFactory RequestContextFactory { get; set; } = new RestApiRequestContextFactory();

	public ISubController SubController { get; set; } = new ConfigurationApiSubController();

	public ISubController ChildSubController { get; set; } = new ConfigurationApiSubController();

	[HttpGet]
	public HttpResponseMessage Get(string resourceNamePlural, string id, string childResourceNamePlural, string childId)
	{
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Expected O, but got Unknown
		using IRestApiRequestContext restApiRequestContext = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(restApiRequestContext);
		restApiRequestContext.RestApiRequestParameters.FilterHandler.SetRootResourceName(resourceNamePlural);
		ResourceEntry resourcePlural = NameConvert.GetResourcePlural(resourceNamePlural);
		ResourceEntry resourcePlural2 = NameConvert.GetResourcePlural(childResourceNamePlural);
		ChildSubController.ResourceEntry = resourcePlural2;
		ChildSubController.LoadSpecificChildItem(restApiRequestContext, resourceNamePlural, id, childId);
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		if (!string.IsNullOrEmpty(restApiRequestContext.RestApiRequestParameters.Task))
		{
			throw new NotSupportedException();
		}
		SubController.ResourceEntry = resourcePlural;
		if (!restApiRequestContext.RestApiRequestParameters.NoData)
		{
			jsonBuilder.AppendElement("data", SubController.SerializeLoadedItem(restApiRequestContext));
		}
		if (restApiRequestContext.RestApiRequestParameters.Tasks)
		{
			jsonBuilder.AppendElement("tasks", SubController.GetTaskListInJson(restApiRequestContext));
		}
		if (restApiRequestContext.RestApiRequestParameters.Definition)
		{
			jsonBuilder.AppendElement("definitions", SubController.GetDefinitions(restApiRequestContext).ToString());
		}
		HttpResponseMessage obj = base.Request.CreateResponse(HttpStatusCode.OK);
		obj.Content = (HttpContent)new StringContent(jsonBuilder.ToJson(), Encoding.UTF8, "application/json");
		return obj;
	}

	[HttpPatch]
	public async Task<HttpResponseMessage> PatchAsync(string resourceNamePlural, string id, string childResourceNamePlural, string childId)
	{
		using IRestApiRequestContext context = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(context);
		ChildSubController.ResourceEntry = NameConvert.GetResourcePlural(childResourceNamePlural);
		string obj = await base.Request.Content.ReadAsStringAsync();
		if (string.IsNullOrEmpty(obj))
		{
			throw new ArgumentException("No content");
		}
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		ElementList elementList = new JsonReader(obj).Parse();
		HttpStatusCode statusCode = ChildSubController.Patch(context, elementList, jsonBuilder, resourceNamePlural, id, childId);
		HttpResponseMessage obj2 = base.Request.CreateResponse(statusCode);
		obj2.Content = (HttpContent)new StringContent(jsonBuilder.ToJson(), Encoding.UTF8, "application/json");
		return obj2;
	}

	[HttpPut]
	public async Task<HttpResponseMessage> PutAsync(string resourceNamePlural, string id, string childResourceNamePlural, string childId)
	{
		using IRestApiRequestContext context = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(context);
		ChildSubController.ResourceEntry = NameConvert.GetResourcePlural(childResourceNamePlural);
		string obj = await base.Request.Content.ReadAsStringAsync();
		if (string.IsNullOrEmpty(obj))
		{
			throw new ArgumentException("No content");
		}
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		ElementList elementList = new JsonReader(obj).Parse();
		HttpStatusCode statusCode = ChildSubController.Put(context, elementList, jsonBuilder, resourceNamePlural, id, childId);
		HttpResponseMessage obj2 = base.Request.CreateResponse(statusCode);
		obj2.Content = (HttpContent)new StringContent(jsonBuilder.ToJson(), Encoding.UTF8, "application/json");
		return obj2;
	}

	[HttpDelete]
	public HttpResponseMessage Delete(string resourceNamePlural, string id, string childResourceNamePlural, string childId)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Expected O, but got Unknown
		using IRestApiRequestContext restApiRequestContext = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(restApiRequestContext);
		ChildSubController.ResourceEntry = NameConvert.GetResourcePlural(childResourceNamePlural);
		string text = ChildSubController.Delete(restApiRequestContext, resourceNamePlural, id, childId);
		HttpResponseMessage obj = base.Request.CreateResponse(HttpStatusCode.OK);
		obj.Content = (HttpContent)new StringContent(text, Encoding.UTF8, "application/json");
		return obj;
	}

	[HttpPost]
	public async Task<HttpResponseMessage> PostAsync(string resourceNamePlural, string id, string childResourceNamePlural, string childId)
	{
		using IRestApiRequestContext context = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(context);
		string json = await base.Request.Content.ReadAsStringAsync();
		ChildSubController.ResourceEntry = NameConvert.GetResourcePlural(childResourceNamePlural);
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		ElementList elementList = new JsonReader(json).Parse();
		HttpStatusCode statusCode = ChildSubController.Post(context, elementList, jsonBuilder, childId);
		HttpResponseMessage obj = base.Request.CreateResponse(statusCode);
		obj.Content = (HttpContent)new StringContent(jsonBuilder.ToJson(), Encoding.UTF8, "application/json");
		return obj;
	}
}
