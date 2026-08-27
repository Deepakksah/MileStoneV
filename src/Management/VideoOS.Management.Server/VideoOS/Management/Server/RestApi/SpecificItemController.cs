using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

[Route("rest/{resourceNamePlural}/{id}")]
public class SpecificItemController : ApiController
{
	public INameConvert NameConvert { get; set; } = new NameConvert();

	public IRestApiRequestContextFactory RequestContextFactory { get; set; } = new RestApiRequestContextFactory();

	public ISubController SubController { get; set; } = new ConfigurationApiSubController();

	[HttpGet]
	public HttpResponseMessage Get(string resourceNamePlural, string id)
	{
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Expected O, but got Unknown
		using IRestApiRequestContext restApiRequestContext = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(restApiRequestContext);
		SubController.ResourceEntry = NameConvert.GetResourcePlural(resourceNamePlural);
		restApiRequestContext.RestApiRequestParameters.FilterHandler.SetRootResourceName(resourceNamePlural);
		SubController.LoadSpecificItem(restApiRequestContext, id);
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		if (!string.IsNullOrEmpty(restApiRequestContext.RestApiRequestParameters.Task))
		{
			throw new NotSupportedException();
		}
		if (!restApiRequestContext.RestApiRequestParameters.NoData)
		{
			jsonBuilder.AppendElement("data", SubController.SerializeLoadedItem(restApiRequestContext));
		}
		if (restApiRequestContext.RestApiRequestParameters.Tasks)
		{
			jsonBuilder.AppendElement("tasks", SubController.GetTaskListInJson(restApiRequestContext));
		}
		if (restApiRequestContext.RestApiRequestParameters.Resources)
		{
			jsonBuilder.AppendElement("resources", SubController.GetChildResources(restApiRequestContext).ToString());
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
	public async Task<HttpResponseMessage> PatchAsync(string resourceNamePlural, string id)
	{
		using IRestApiRequestContext context = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(context);
		string obj = await base.Request.Content.ReadAsStringAsync();
		if (string.IsNullOrEmpty(obj))
		{
			throw new ArgumentException("No content");
		}
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		ElementList elementList = new JsonReader(obj).Parse();
		SubController.ResourceEntry = NameConvert.GetResourcePlural(resourceNamePlural);
		HttpStatusCode statusCode = SubController.Patch(context, elementList, jsonBuilder, id);
		HttpResponseMessage obj2 = base.Request.CreateResponse(statusCode);
		obj2.Content = (HttpContent)new StringContent(jsonBuilder.ToJson(), Encoding.UTF8, "application/json");
		return obj2;
	}

	[HttpPut]
	public async Task<HttpResponseMessage> PutAsync(string resourceNamePlural, string id)
	{
		using IRestApiRequestContext context = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(context);
		string obj = await base.Request.Content.ReadAsStringAsync();
		if (string.IsNullOrEmpty(obj))
		{
			throw new ArgumentException("No content");
		}
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		ElementList elementList = new JsonReader(obj).Parse();
		SubController.ResourceEntry = NameConvert.GetResourcePlural(resourceNamePlural);
		HttpStatusCode statusCode = SubController.Put(context, elementList, jsonBuilder, id);
		HttpResponseMessage obj2 = base.Request.CreateResponse(statusCode);
		obj2.Content = (HttpContent)new StringContent(jsonBuilder.ToJson(), Encoding.UTF8, "application/json");
		return obj2;
	}

	[HttpDelete]
	public HttpResponseMessage Delete(string resourceNamePlural, string id)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Expected O, but got Unknown
		using IRestApiRequestContext restApiRequestContext = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(restApiRequestContext);
		SubController.ResourceEntry = NameConvert.GetResourcePlural(resourceNamePlural);
		string text = SubController.Delete(restApiRequestContext, id);
		HttpResponseMessage obj = base.Request.CreateResponse(HttpStatusCode.OK);
		obj.Content = (HttpContent)new StringContent(text, Encoding.UTF8, "application/json");
		return obj;
	}

	[HttpPost]
	public async Task<HttpResponseMessage> PostAsync(string resourceNamePlural, string id)
	{
		using IRestApiRequestContext context = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(context);
		string json = await base.Request.Content.ReadAsStringAsync();
		SubController.ResourceEntry = NameConvert.GetResourcePlural(resourceNamePlural);
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		ElementList elementList = new JsonReader(json).Parse();
		HttpStatusCode statusCode = SubController.Post(context, elementList, jsonBuilder, id);
		HttpResponseMessage obj = base.Request.CreateResponse(statusCode);
		obj.Content = (HttpContent)new StringContent(jsonBuilder.ToJson(), Encoding.UTF8, "application/json");
		return obj;
	}
}
