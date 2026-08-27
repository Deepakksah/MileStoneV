using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Management.Server.RestApi.Models;
using VideoOS.Management.Server.SecurityModel;

namespace VideoOS.Management.Server.RestApi;

[Route("api/securityNamespaces/{pathId?}")]
public class SecurityNamespaceController : ApiController
{
	public const string JsonField_name = "name";

	public const string JsonField_displayName = "displayName";

	public const string JsonField_id = "id";

	public const string JsonField_securityActions = "securityActions";

	public const string JsonField_visible = "visible";

	private readonly IRestApiRequestContextFactory _requestContextFactory;

	private static Dictionary<Guid, string> securityNamespaceToResource = new Dictionary<Guid, string>
	{
		{
			new Guid("623D03F8-C5D5-46BC-A2F4-4C03562D4F85"),
			"cameras"
		},
		{
			new Guid("6339122C-04F5-41F2-B3E3-3555997CFCEF"),
			"failoverRecorders"
		},
		{
			new Guid("F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C"),
			"hardware"
		},
		{
			new Guid("3ED79EF0-EE2F-40B8-A49A-17E9380C5932"),
			"inputEvents"
		},
		{
			new Guid("988D493D-1597-4BE6-AFF0-396BFD5D047A"),
			"matrix"
		},
		{
			new Guid("EDE4D51C-F691-4894-9C0B-C3AE096DC04D"),
			"metadata"
		},
		{
			new Guid("15F48F88-CA89-4926-9A84-2B02864EC77A"),
			"microphones"
		},
		{
			new Guid("A95FCE90-753D-4CB7-86FB-F8CAD9F93B67"),
			"outputs"
		},
		{
			new Guid("9710735D-EB10-44EA-888D-FA8AB05A6433"),
			"recordingServers"
		},
		{
			new Guid("C24C91A7-7D89-459B-B78E-C4548C74640D"),
			"rules"
		},
		{
			new Guid("875F4A4E-4D78-4A8D-A978-290227253475"),
			"childSites"
		},
		{
			new Guid("8732FCAC-A63C-46EA-9683-532B4C6644E7"),
			"videoWalls"
		},
		{
			new Guid("48B602BC-E752-4BBF-8E2A-7DE01F53A6DD"),
			"speakers"
		},
		{
			new Guid("2D4048FA-41DA-435A-A800-2419137E4A3E"),
			string.Empty
		},
		{
			new Guid("D4FF31CD-D9AC-46DF-8E92-5B545D191BAF"),
			"userDefinedEvents"
		},
		{
			new Guid("EC543C56-ADF7-4600-B9AD-97E166552BE9"),
			"viewGroups"
		},
		{
			new Guid("C201AE6F-6FF3-41D4-BCBF-0F9DB5049988"),
			string.Empty
		},
		{
			new Guid("E9914E39-A67A-496C-A3C1-5B4AAAB28F15"),
			"sites"
		},
		{
			new Guid("55BF1320-0224-48C7-8F57-BD41090576C8"),
			"metadataSearch"
		},
		{
			new Guid("D2D396E4-819E-4226-AA7F-AF36896D78BC"),
			"saveSearches"
		}
	};

	public SecurityNamespaceController()
		: this(new RestApiRequestContextFactory())
	{
	}

	public SecurityNamespaceController(IRestApiRequestContextFactory requestContextFactory)
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
				List<object> value = new List<object>();
				dictionary.Add("tasks", value);
				return Ok(dictionary);
			}
			SecurityManagerService service = restApiRequestContext.GetService<SecurityManagerService>();
			List<object> list = new List<object>();
			foreach (IGeminiSecurityNamespace securityNamespace2 in service.GetSecurityNamespaces(restApiRequestContext))
			{
				list.Add(SecurityNamespaceToDataModel(securityNamespace2.NamespaceInfo));
			}
			dictionary["array"] = list.ToArray();
			return Ok(dictionary);
		}
		if (!Guid.TryParse(pathId, out var result))
		{
			throw new ArgumentException("Invalid id");
		}
		IGeminiSecurityNamespace securityNamespace = restApiRequestContext.GetSecurityManagerService().GetSecurityNamespace(restApiRequestContext, result);
		if (securityNamespace == null)
		{
			throw new PathNotFoundException();
		}
		dictionary["data"] = SecurityNamespaceToDataModel(securityNamespace.NamespaceInfo);
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
		if (!restApiRequestContext.GetService<SecurityManagerService>().IsAdministrator(restApiRequestContext))
		{
			return Unauthorized();
		}
		if (restApiRequestContext.RestApiRequestParameters.Task == null)
		{
			SecurityNamespaceModel securityNamespaceDataModel;
			Dictionary<string, object> content = PostCreateSecurityNamespace(restApiRequestContext, postContent, out securityNamespaceDataModel);
			return Created("securityNamespaces/" + securityNamespaceDataModel.id, content);
		}
		return BadRequest("Unknown task");
	}

	private static SecurityNamespaceModel SecurityNamespaceToDataModel(SecurityNamespaceInfo securityNamespaceInfo)
	{
		PathDataModel self = new PathDataModel
		{
			Type = "securityNamespaces",
			Id = securityNamespaceInfo.NamespaceId
		};
		RelationsDataModel relations = new RelationsDataModel
		{
			self = self
		};
		string text = ConvertSecurityNamespaceToResourceType(securityNamespaceInfo);
		return new SecurityNamespaceModel
		{
			name = securityNamespaceInfo.Name,
			displayName = securityNamespaceInfo.DisplayName,
			id = securityNamespaceInfo.NamespaceId,
			resourceType = text,
			userDefined = securityNamespaceInfo.UserDefined,
			structure = securityNamespaceInfo.Structure,
			securityActions = BuildActions(securityNamespaceInfo),
			relations = relations,
			relatedTimeprofileMask = SecurityPermissionDataTranslater.GetPossibleTimeprofileMask(text)
		};
	}

	private static VideoOS.Management.Server.RestApi.Models.SecurityAction[] BuildActions(SecurityNamespaceInfo securityNamespaceInfo)
	{
		List<VideoOS.Management.Server.RestApi.Models.SecurityAction> list = new List<VideoOS.Management.Server.RestApi.Models.SecurityAction>();
		foreach (ActionDefinition action in securityNamespaceInfo.Actions)
		{
			list.Add(new VideoOS.Management.Server.RestApi.Models.SecurityAction
			{
				name = action.Name,
				displayName = action.DisplayName,
				visible = action.Visible,
				securityMask = action.PermissionBit
			});
		}
		return list.ToArray();
	}

	private static Dictionary<string, object> PostCreateSecurityNamespace(IGeminiRequestContext requestContext, JObject postContent, out SecurityNamespaceModel securityNamespaceDataModel)
	{
		string stringValue = JObjectUtil.GetStringValue(postContent, "name", allowDefault: false);
		string stringValue2 = JObjectUtil.GetStringValue(postContent, "displayName", allowDefault: false);
		string stringValue3 = JObjectUtil.GetStringValue(postContent, "id", allowDefault: false);
		if (!Guid.TryParse(stringValue3, out var _))
		{
			throw new PropertyInvalidException("id must be valid GUID", "id");
		}
		JToken? obj = postContent.GetValue("securityActions") ?? throw new PropertyInvalidException("property missing", "securityActions");
		List<ActionDefinition> list = new List<ActionDefinition>();
		bool flag = false;
		bool flag2 = false;
		foreach (JToken item in obj.Children())
		{
			JObject? content = item.ToObject<JObject>();
			string stringValue4 = JObjectUtil.GetStringValue(content, "name", allowDefault: false);
			string stringValue5 = JObjectUtil.GetStringValue(content, "displayName", allowDefault: false);
			bool boolValue = JObjectUtil.GetBoolValue(content, "visible");
			list.Add(new ActionDefinition
			{
				Name = stringValue4,
				DisplayName = stringValue5,
				Visible = boolValue,
				PermissionBit = 0uL,
				NamespaceId = stringValue3
			});
			if (stringValue4 == "GENERIC_READ")
			{
				flag = true;
			}
			if (stringValue4 == "ADMIN_SECURITY")
			{
				flag2 = true;
				if (boolValue)
				{
					throw new PropertyInvalidException("The ADMIN_SECURITY action cannot be visible on all products, and must be false");
				}
			}
		}
		if (!flag)
		{
			throw new PropertyInvalidException("Actions must contain a definition for GENERIC_READ", "securityActions");
		}
		if (!flag2)
		{
			throw new PropertyInvalidException("Actions must contain a definition for ADMIN_SECURITY", "securityActions");
		}
		SecurityNamespaceInfo securityNamespaceInfo = new SecurityNamespaceInfo
		{
			NamespaceId = stringValue3.ToUpper(),
			Name = stringValue,
			DisplayName = stringValue2,
			Separator = '/',
			Structure = 1,
			UserDefined = true,
			Actions = list
		};
		try
		{
			requestContext.GetSecurityManagerService().CreateSecurityNamespace(requestContext, securityNamespaceInfo, Guid.Empty.ToString());
		}
		catch (RoleCreationException)
		{
			throw new ArgumentException("Either name or id already exists");
		}
		securityNamespaceDataModel = SecurityNamespaceToDataModel(securityNamespaceInfo);
		return new Dictionary<string, object> { ["result"] = securityNamespaceDataModel };
	}

	private static string ConvertSecurityNamespaceToResourceType(SecurityNamespaceInfo securityNamespaceInfo)
	{
		if (securityNamespaceInfo.UserDefined)
		{
			return securityNamespaceInfo.NamespaceId.ToString();
		}
		Guid key = new Guid(securityNamespaceInfo.NamespaceId);
		if (securityNamespaceToResource.TryGetValue(key, out var value))
		{
			return value;
		}
		return string.Empty;
	}
}
