using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http;
using System.Xml;
using VideoOS.Common.Services;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.RestApi;

[Route("api/registeredServices/{id?}")]
public class RegisteredServicesController : ApiController
{
	private static List<Guid> XProtectServiceTypes = new List<Guid>
	{
		ServiceTypes.ServiceType_CorporateService,
		ServiceTypes.ServiceType_AuthorizationServer,
		ServiceTypes.ServiceType_LegacyLogService,
		ServiceTypes.ServiceType_LogService,
		ServiceTypes.ServiceType_EventServerService,
		ServiceTypes.ServiceType_MobileService,
		ServiceTypes.ServiceType_ReportService
	};

	private readonly IRestApiRequestContextFactory _requestContextFactory;

	public RegisteredServicesController()
		: this(new RestApiRequestContextFactory())
	{
	}

	public RegisteredServicesController(IRestApiRequestContextFactory requestContextFactory)
	{
		_requestContextFactory = requestContextFactory;
	}

	[HttpGet]
	[AllowAnonymous]
	public IHttpActionResult Get(string id = null)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		using (IRestApiRequestContext restApiRequestContext = _requestContextFactory.GetRequestContext(base.Request))
		{
			using (restApiRequestContext.CreateRegisteredServerComponent())
			{
				if (id == null)
				{
					bool disabled = restApiRequestContext.RestApiRequestParameters.Disabled;
					bool flag = restApiRequestContext.RestApiRequestParameters.Parameters.FirstOrDefault((KeyValuePair<string, string> p) => p.Key.Equals("VMSinternal", StringComparison.OrdinalIgnoreCase)).Value != null;
					ServiceInfoHandler[] services = RegisteredServerServiceHandling.GetServices(restApiRequestContext, restApiRequestContext.RemoteIPAddress, disabled);
					List<object> list = new List<object>();
					ServiceInfoHandler[] array = services;
					foreach (ServiceInfoHandler serviceInfoHandler in array)
					{
						if (flag || !IsServiceXProtectType(serviceInfoHandler.type))
						{
							list.Add(ConvertProperties(serviceInfoHandler));
						}
					}
					dictionary.Add("array", list.ToArray());
				}
				else
				{
					if (!Guid.TryParse(id, out var guid))
					{
						return BadRequest("Incorrect id");
					}
					ServiceInfoHandler serviceInfoHandler2 = RegisteredServerServiceHandling.GetServices(restApiRequestContext, restApiRequestContext.RemoteIPAddress, includeDisabled: true).FirstOrDefault((ServiceInfoHandler s) => s.instance == guid);
					if (serviceInfoHandler2 == null)
					{
						return NotFound();
					}
					dictionary.Add("data", ConvertProperties(serviceInfoHandler2));
				}
			}
		}
		return Ok(dictionary);
	}

	[HttpPost]
	public IHttpActionResult Post([FromBody] RegistrationServiceData postContent, string id = null)
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		if (id != null)
		{
			return BadRequest("Cannot post on a specific service registration");
		}
		if (postContent == null)
		{
			throw new ArgumentException("Invalid json format");
		}
		using IRestApiRequestContext requestContext = _requestContextFactory.GetRequestContext(base.Request);
		string text = GetString("name", postContent.name, nullAllowed: false);
		Guid guid = GetGuid("serviceType", postContent.serviceType, nullAllowed: false);
		Guid serverId = GetGuid("serverId", postContent.id, nullAllowed: false);
		string description = GetString("description", postContent.description, nullAllowed: false);
		string text2 = GetString("customData", postContent.customData, nullAllowed: true);
		string endpointDefinition = ExtractEndpointsAsXml(postContent.endpoints);
		if (string.IsNullOrEmpty(text))
		{
			throw new ArgumentException("name cannot be blank");
		}
		if (postContent.uris == null)
		{
			throw new ArgumentNullException("uris");
		}
		string[] uris = postContent.uris;
		string[] array = uris;
		for (int i = 0; i < array.Length; i++)
		{
			ValidateUri(array[i]);
		}
		if (RegisteredServer.QueryAllRegisteredServers(requestContext, includeDisabled: true).FirstOrDefault((RegisteredServer s) => s.IDServer == serverId) != null)
		{
			throw new ArgumentException("Duplicate id");
		}
		RegisteredServerServiceHandling.RegisterServiceWithEndpoints(requestContext, guid, serverId, uris, text, description, text2 ?? "", endpointDefinition);
		dictionary.Add("name", text);
		dictionary.Add("path", $"registeredServices/{serverId}");
		return Created($"registeredServices/{serverId}", dictionary);
	}

	[HttpDelete]
	public IHttpActionResult Delete(string id = null)
	{
		new Dictionary<string, object>();
		if (id == null)
		{
			return BadRequest("Id required");
		}
		if (!Guid.TryParse(id, out var guid))
		{
			return BadRequest("Incorrect id");
		}
		using (IRestApiRequestContext requestContext = _requestContextFactory.GetRequestContext(base.Request))
		{
			if (RegisteredServer.QueryAllRegisteredServers(requestContext, includeDisabled: true).FirstOrDefault((RegisteredServer s) => s.IDServer == guid) == null)
			{
				return NotFound();
			}
			RegisteredServer.UnregisterService(requestContext, guid);
		}
		return Ok();
	}

	[HttpPatch]
	public IHttpActionResult Patch([FromBody] RegistrationServiceData postContent, string id = null)
	{
		Dictionary<string, object> content = new Dictionary<string, object>();
		if (id == null)
		{
			return BadRequest("Id required");
		}
		if (!Guid.TryParse(id, out var guid))
		{
			return BadRequest("Incorrect id");
		}
		if (postContent == null)
		{
			throw new ArgumentException("Invalid json format");
		}
		using (IRestApiRequestContext restApiRequestContext = _requestContextFactory.GetRequestContext(base.Request))
		{
			ServiceInfoHandler serviceInfoHandler = RegisteredServerServiceHandling.GetServices(restApiRequestContext, restApiRequestContext.RemoteIPAddress, includeDisabled: true).FirstOrDefault((ServiceInfoHandler s) => s.instance == guid);
			if (serviceInfoHandler == null)
			{
				return NotFound();
			}
			if (postContent.enabled.HasValue)
			{
				RegisteredServer.EnableService(restApiRequestContext, guid, postContent.enabled.Value);
			}
			if (postContent.customData != null)
			{
				RegisteredServer.SetServiceData(restApiRequestContext, guid, postContent.customData);
			}
			if (postContent.dataCollectorId != null)
			{
				RegisteredServer.UpdateDataCollectorsForRegisteredServers(restApiRequestContext, guid, GetInt("dataCollectorId", postContent.dataCollectorId));
			}
			if (postContent.uris != null)
			{
				RegisteredServerServiceHandling.RegisterServiceWithEndpoints(restApiRequestContext, serviceInfoHandler.type, serviceInfoHandler.instance, postContent.uris, serviceInfoHandler.name, serviceInfoHandler.description, serviceInfoHandler.customData, serviceInfoHandler.endpointdefinition);
			}
		}
		return Ok(content);
	}

	private static string GetString(string name, string value, bool nullAllowed)
	{
		if (value == null && !nullAllowed)
		{
			throw new ArgumentNullException(name);
		}
		return value;
	}

	private static Guid GetGuid(string name, string value, bool nullAllowed)
	{
		if (value == null && !nullAllowed)
		{
			throw new ArgumentNullException(name);
		}
		if (!Guid.TryParse(value, out var result))
		{
			throw new ArgumentException(name + " must be a Guid");
		}
		return result;
	}

	private static int GetInt(string name, string value)
	{
		if (!int.TryParse(value, out var result))
		{
			throw new ArgumentException(name + " must be an integer");
		}
		return result;
	}

	private static bool IsServiceXProtectType(Guid serviceType)
	{
		return XProtectServiceTypes.Contains(serviceType);
	}

	private static string ExtractEndpointsAsXml(RegistrationServiceEndpoint[] endpoints)
	{
		XmlDocument xmlDocument = new XmlDocument
		{
			XmlResolver = null
		};
		XmlReader reader = XmlReader.Create(new StringReader("<endpoints></endpoints>"), new XmlReaderSettings
		{
			XmlResolver = null
		});
		xmlDocument.Load(reader);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("endpoints");
		if (endpoints != null)
		{
			foreach (RegistrationServiceEndpoint registrationServiceEndpoint in endpoints)
			{
				if (registrationServiceEndpoint.description == null)
				{
					registrationServiceEndpoint.description = string.Empty;
				}
				if (string.IsNullOrEmpty(registrationServiceEndpoint.name))
				{
					throw new ArgumentException("name must be defined");
				}
				if (string.IsNullOrEmpty(registrationServiceEndpoint.uri))
				{
					throw new ArgumentException("uri must be defined");
				}
				if (string.IsNullOrEmpty(registrationServiceEndpoint.authentication))
				{
					throw new ArgumentException("authentication must be defined");
				}
				if (!Enum.TryParse<AuthenticationType>(registrationServiceEndpoint.authentication, out var _))
				{
					throw new ArgumentException("authentication invalid, must be one of: Windows,Basic,Token,OAuth");
				}
				XmlNode xmlNode2 = xmlDocument.CreateNode(XmlNodeType.Element, "endpoint", null);
				XmlElement xmlElement = xmlDocument.CreateElement("name");
				xmlElement.InnerText = registrationServiceEndpoint.name;
				xmlNode2.AppendChild(xmlElement);
				XmlNode xmlNode3 = xmlDocument.CreateElement("description");
				xmlNode3.InnerText = registrationServiceEndpoint.description;
				xmlNode2.AppendChild(xmlNode3);
				XmlNode xmlNode4 = xmlDocument.CreateElement("authenticationmode");
				xmlNode4.InnerText = registrationServiceEndpoint.authentication;
				xmlNode2.AppendChild(xmlNode4);
				XmlNode xmlNode5 = xmlDocument.CreateElement("servicepath");
				xmlNode5.InnerText = registrationServiceEndpoint.uri.ToString();
				xmlNode2.AppendChild(xmlNode5);
				if (Uri.TryCreate(registrationServiceEndpoint.uri, UriKind.RelativeOrAbsolute, out var result2))
				{
					XmlNode xmlNode6 = xmlDocument.CreateElement("scheme");
					xmlNode6.InnerText = result2.Scheme;
					xmlNode2.AppendChild(xmlNode6);
					xmlNode.AppendChild(xmlNode2);
					continue;
				}
				throw new ArgumentException("Scheme on uri invalid");
			}
		}
		return xmlDocument.OuterXml;
	}

	private static void ValidateUri(string uri)
	{
		if (Uri.TryCreate(uri, UriKind.RelativeOrAbsolute, out var _))
		{
			return;
		}
		throw new ArgumentException("Invalid URI format");
	}

	public static object ConvertProperties(ServiceInfoHandler service)
	{
		List<object> list = new List<object>();
		ServiceEndpointHandler[] endpoints = service.endpoints;
		foreach (ServiceEndpointHandler serviceEndpointHandler in endpoints)
		{
			Dictionary<string, object> item = new Dictionary<string, object>
			{
				{ "uri", serviceEndpointHandler.uri },
				{ "name", serviceEndpointHandler.name },
				{ "description", serviceEndpointHandler.description },
				{ "authentication", serviceEndpointHandler.authentication }
			};
			list.Add(item);
		}
		return new Dictionary<string, object>
		{
			{ "name", service.name },
			{ "description", service.description },
			{ "id", service.instance },
			{ "enabled", service.enabled },
			{ "trusted", service.trusted },
			{ "serviceType", service.type },
			{ "customData", service.customData },
			{ "dataCollectorId", service.dataCollectorId },
			{
				"endpoints",
				list.ToArray()
			},
			{ "uris", service.uri }
		};
	}
}
