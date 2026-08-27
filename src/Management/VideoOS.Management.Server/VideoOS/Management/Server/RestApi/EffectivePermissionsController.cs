using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using VideoOS.Management.Common;
using VideoOS.Management.Server.RestApi.Models;

namespace VideoOS.Management.Server.RestApi;

[Route("api/effectivePermissions/{namespaceId?}")]
public class EffectivePermissionsController : ApiController
{
	private readonly IRestApiRequestContextFactory _requestContextFactory;

	public EffectivePermissionsController()
		: this(new RestApiRequestContextFactory())
	{
	}

	public EffectivePermissionsController(IRestApiRequestContextFactory requestContextFactory)
	{
		_requestContextFactory = requestContextFactory;
	}

	[HttpGet]
	public IHttpActionResult Get(string namespaceId = null)
	{
		using IRestApiRequestContext requestContext = _requestContextFactory.GetRequestContext(base.Request);
		ValidateGet(requestContext, namespaceId, out var isAdministrator, out var geminiSecurityNamespace, out var objectIds, out var roleId, out var userSid);
		if (roleId != null || userSid != null)
		{
			if (!isAdministrator)
			{
				return BadRequest("Only administrators can query effective permissions for other users or roles.");
			}
			if (userSid == null)
			{
				userSid = roleId;
			}
			EffectivePermissionsModel value = BuildUserResult(requestContext, geminiSecurityNamespace, objectIds, userSid);
			Dictionary<string, object> content = new Dictionary<string, object> { { "data", value } };
			return Ok(content);
		}
		EffectivePermissionsModel value2 = (isAdministrator ? BuildAdminResult(namespaceId) : BuildUserResult(requestContext, geminiSecurityNamespace, objectIds));
		Dictionary<string, object> content2 = new Dictionary<string, object> { { "data", value2 } };
		return Ok(content2);
	}

	private static void ValidateGet(IRestApiRequestContext requestContext, string securityNamespaceId, out bool isAdministrator, out IGeminiSecurityNamespace geminiSecurityNamespace, out string[] objectIds, out string roleId, out string userSid)
	{
		if (securityNamespaceId == null)
		{
			throw new ArgumentException("namespaceId must be provided, like:  /effectivePermissions/623D03F8-C5D5-46BC-A2F4-4C03562D4F85 ");
		}
		if (!Guid.TryParse(securityNamespaceId, out var result))
		{
			throw new ArgumentException("Invalid security namespace id");
		}
		SecurityManagerService service = requestContext.GetService<SecurityManagerService>();
		geminiSecurityNamespace = service.GetSecurityNamespace(requestContext, result);
		if (geminiSecurityNamespace == null)
		{
			throw new ArgumentException("Invalid security namespace id");
		}
		isAdministrator = service.IsAdministrator(requestContext);
		objectIds = null;
		roleId = null;
		userSid = null;
		if (requestContext.RestApiRequestParameters.Parameters != null)
		{
			string value = requestContext.RestApiRequestParameters.Parameters.FirstOrDefault((KeyValuePair<string, string> k) => k.Key == "objectIds").Value;
			if (value != null)
			{
				objectIds = value.Split(',');
			}
			roleId = requestContext.RestApiRequestParameters.Parameters.FirstOrDefault((KeyValuePair<string, string> k) => k.Key == "roleId").Value;
			userSid = requestContext.RestApiRequestParameters.Parameters.FirstOrDefault((KeyValuePair<string, string> k) => k.Key == "userSid").Value;
		}
	}

	private EffectivePermissionsModel BuildAdminResult(string namespaceId)
	{
		return new EffectivePermissionsModel
		{
			id = namespaceId.ToUpper(),
			isAdministrator = true,
			objectPermissions = new ObjectPermissionsModel[0]
		};
	}

	private EffectivePermissionsModel BuildUserResult(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string[] objectIds, string userSid = null)
	{
		List<ObjectPermissionsModel> list = new List<ObjectPermissionsModel>();
		ulong permissionBit = securityNamespace.NamespaceInfo.Actions.FirstOrDefault((ActionDefinition a) => a.Name == "GENERIC_READ").PermissionBit;
		Guid guid = new Guid(securityNamespace.NamespaceInfo.NamespaceId);
		using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
		{
			if (objectIds == null)
			{
				objectIds = ((!(guid == SecurityNamespaceIdentifier.ManagementServer)) ? securityComponent.QuerySecurityObjects(guid, securityNamespace.NamespaceInfo.UserDefined) : new string[1] { "XPCO" });
			}
			if (guid == SecurityNamespaceIdentifier.ViewGroup)
			{
				list.Add(ConstructPrivateViewGroupMask(requestContext, securityNamespace));
			}
			string[] array = objectIds;
			foreach (string text in array)
			{
				AccessControlEntry2 accessControlEntry = ((userSid == null) ? securityNamespace.QueryPermissionMask(requestContext, text) : securityNamespace.QueryPermissionMaskForOther(requestContext, text, userSid));
				bool flag = guid == SecurityNamespaceIdentifier.RecordingServer || guid == SecurityNamespaceIdentifier.Hardware;
				if ((accessControlEntry.Allow & permissionBit) != 0 || flag)
				{
					Guid id = TrimPath(requestContext, text);
					list.Add(new ObjectPermissionsModel
					{
						id = id,
						mask = (accessControlEntry.Allow | permissionBit)
					});
				}
			}
		}
		return new EffectivePermissionsModel
		{
			id = securityNamespace.NamespaceInfo.NamespaceId.ToUpper(CultureInfo.InvariantCulture),
			isAdministrator = false,
			objectPermissions = list.ToArray()
		};
	}

	private ObjectPermissionsModel ConstructPrivateViewGroupMask(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace)
	{
		ServerGroup serverGroup = ServerGroup.QueryPrivateViewGroup(requestContext, requestContext.AuthenticatedUserSid);
		AccessControlEntry[] array = requestContext.GetService<SecuredSecurityManager>().QueryPermissions(requestContext, securityNamespace.NamespaceInfo.NamespaceId.ToString(), serverGroup.Id, requestContext.AuthenticatedUserSid);
		ulong num = 0uL;
		AccessControlEntry[] array2 = array;
		foreach (AccessControlEntry ace in array2)
		{
			ActionDefinition actionDefinition = securityNamespace.NamespaceInfo.Actions.FirstOrDefault((ActionDefinition a) => a.Name == ace.ActionId);
			if (actionDefinition != null)
			{
				num |= actionDefinition.PermissionBit;
			}
		}
		return new ObjectPermissionsModel
		{
			id = new Guid(serverGroup.Id),
			mask = num
		};
	}

	private Guid TrimPath(IGeminiRequestContext requestContext, string objectOrPath)
	{
		string input = ((objectOrPath.Length > 36) ? objectOrPath.Substring(objectOrPath.Length - 36) : ((!(objectOrPath == "XPCO")) ? objectOrPath : ServerInformation.GetServerInformation(requestContext).Id));
		if (Guid.TryParse(input, out var result))
		{
			return result;
		}
		return Guid.Empty;
	}
}
