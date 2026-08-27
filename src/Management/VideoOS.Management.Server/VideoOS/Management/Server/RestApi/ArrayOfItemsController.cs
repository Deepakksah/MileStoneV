using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi;

[Route("rest/{resourceNamePlural}")]
public class ArrayOfItemsController : ApiController
{
	public INameConvert NameConvert { get; set; } = new NameConvert();

	public IRestApiRequestContextFactory RequestContextFactory { get; set; } = new RestApiRequestContextFactory();

	public ISubController SubController { get; set; } = new ConfigurationApiSubController();

	[HttpGet]
	public HttpResponseMessage Get(string resourceNamePlural)
	{
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		using IRestApiRequestContext restApiRequestContext = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(restApiRequestContext);
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		if (resourceNamePlural == "translations")
		{
			JsonBuilder jsonBuilder2 = BuildTranslations(restApiRequestContext);
			jsonBuilder.AppendKeyValue("array", jsonBuilder2.ToJson());
		}
		else
		{
			SubController.ResourceEntry = NameConvert.GetResourcePlural(resourceNamePlural);
			restApiRequestContext.RestApiRequestParameters.FilterHandler.SetRootResourceName(resourceNamePlural);
			if (restApiRequestContext.RestApiRequestParameters.Count)
			{
				jsonBuilder.AppendElement("count", SubController.GetCountOfItems(restApiRequestContext).ToString());
			}
			else
			{
				if (!restApiRequestContext.RestApiRequestParameters.NoData)
				{
					jsonBuilder.AppendElement("array", SubController.GetArrayOfItems(restApiRequestContext).ToString());
				}
				if (restApiRequestContext.RestApiRequestParameters.Tasks)
				{
					string taskListInJson = SubController.GetTaskListInJson(restApiRequestContext);
					if (!string.IsNullOrEmpty(taskListInJson))
					{
						jsonBuilder.AppendElement("tasks", taskListInJson);
					}
				}
			}
		}
		HttpResponseMessage obj = base.Request.CreateResponse(HttpStatusCode.OK);
		obj.Content = (HttpContent)new StringContent(jsonBuilder.ToJson(), Encoding.UTF8, "application/json");
		return obj;
	}

	[HttpPost]
	public async Task<HttpResponseMessage> PostAsync(string resourceNamePlural)
	{
		using IRestApiRequestContext context = RequestContextFactory.GetRequestContext(base.Request);
		RestApiSecurityUtil.CheckConnect(context);
		string json = await base.Request.Content.ReadAsStringAsync();
		SubController.ResourceEntry = NameConvert.GetResourcePlural(resourceNamePlural);
		ElementList elementList = new JsonReader(json).Parse();
		JsonBuilder jsonBuilder = new JsonBuilder(array: false);
		HttpStatusCode statusCode = SubController.Post(context, elementList, jsonBuilder);
		HttpResponseMessage obj = base.Request.CreateResponse(statusCode);
		obj.Content = (HttpContent)new StringContent(jsonBuilder.ToJson(), Encoding.UTF8, "application/json");
		return obj;
	}

	private static JsonBuilder BuildTranslations(IRestApiRequestContext context)
	{
		if (string.IsNullOrEmpty(context.RestApiRequestParameters.Language))
		{
			throw new ArgumentException("language parameter is missing");
		}
		Dictionary<string, string> translations = RestApiCache.GetTranslations(context, context.RestApiRequestParameters.Language);
		JsonBuilder jsonBuilder = new JsonBuilder(array: true);
		foreach (KeyValuePair<string, string> item in translations)
		{
			JsonBuilder jsonBuilder2 = new JsonBuilder(array: false);
			jsonBuilder2.AppendKeyString("id", item.Key);
			jsonBuilder2.AppendKeyString("text", JsonUtilities.JsonEscape(item.Value));
			jsonBuilder.AppendElement(jsonBuilder2.ToJson());
		}
		return jsonBuilder;
	}
}
