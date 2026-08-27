using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

[Route("rest/{resourceNamePlural}/{id}/{childResourceNamePlural}")]
public class ArrayOfChildItemsController : ApiController
{
	public INameConvert NameConvert { get; set; } = new NameConvert();

	public IRestApiRequestContextFactory RequestContextFactory { get; set; } = new RestApiRequestContextFactory();

	public ISubController SubController { get; set; } = new ConfigurationApiSubController();

	public ISubController ChildSubController { get; set; } = new ConfigurationApiSubController();

	[HttpGet]
	public HttpResponseMessage Get(string resourceNamePlural, string id, string childResourceNamePlural)
	{
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Expected O, but got Unknown
		using IRestApiRequestContext restApiRequestContext = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(restApiRequestContext);
		restApiRequestContext.RestApiRequestParameters.FilterHandler.SetRootResourceName(childResourceNamePlural);
		ResourceEntry resourcePlural = NameConvert.GetResourcePlural(resourceNamePlural);
		SubController.ResourceEntry = resourcePlural;
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		if (restApiRequestContext.RestApiRequestParameters.Count)
		{
			jsonBuilder.AppendKeyValue("count", SubController.GetCountOfChildItems(restApiRequestContext, resourceNamePlural, id, childResourceNamePlural).ToString());
		}
		else
		{
			string arrayOfChildItems = SubController.GetArrayOfChildItems(restApiRequestContext, resourceNamePlural, id, childResourceNamePlural);
			if (!restApiRequestContext.RestApiRequestParameters.NoData)
			{
				jsonBuilder.AppendElement("array", arrayOfChildItems);
			}
			if (restApiRequestContext.RestApiRequestParameters.Tasks)
			{
				ChildSubController.ResourceEntry = NameConvert.GetResourcePlural(childResourceNamePlural);
				jsonBuilder.AppendElement("tasks", ChildSubController.GetTaskListInJson(restApiRequestContext, resourcePlural, id));
			}
			if (restApiRequestContext.RestApiRequestParameters.Definition)
			{
				jsonBuilder.AppendElement("definitions", SubController.GetChildItemsDefinitions(restApiRequestContext).ToString());
			}
		}
		HttpResponseMessage obj = base.Request.CreateResponse(HttpStatusCode.OK);
		obj.Content = (HttpContent)new StringContent(jsonBuilder.ToJson(), Encoding.UTF8, "application/json");
		return obj;
	}

	[HttpPost]
	public async Task<HttpResponseMessage> PostAsync(string resourceNamePlural, string id, string childResourceNamePlural)
	{
		using IRestApiRequestContext context = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(context);
		string json = await base.Request.Content.ReadAsStringAsync();
		SubController.ResourceEntry = NameConvert.GetResourcePlural(resourceNamePlural);
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		ElementList elementList = new JsonReader(json).Parse();
		HttpStatusCode statusCode = SubController.Post(context, elementList, jsonBuilder, id, childResourceNamePlural);
		HttpResponseMessage obj = base.Request.CreateResponse(statusCode);
		obj.Content = (HttpContent)new StringContent(jsonBuilder.ToJson(), Encoding.UTF8, "application/json");
		return obj;
	}
}
