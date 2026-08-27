using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi;
using VideoOS.Management.Server.RestApi.DataAccess;
using VideoOS.Management.Server.RestApi.Utils;
using VideoOS.Management.Server.SecurityModel;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Management.Server.RestApi.Controllers;

[Route("api/rolePermissions/{roleId?}")]
public class RolePermissionsController : ApiController
{
	private readonly IRestApiRequestContextFactory _requestContextFactory;

	private static HashSet<Guid> _deviceSecNamespaces = new HashSet<Guid>
	{
		SecurityNamespaceIdentifier.Camera,
		SecurityNamespaceIdentifier.Microphone,
		SecurityNamespaceIdentifier.Speaker,
		SecurityNamespaceIdentifier.Input,
		SecurityNamespaceIdentifier.Output,
		SecurityNamespaceIdentifier.Metadata
	};

	private static List<string> _deviceResourceNames = new List<string> { "cameras", "microphones", "speakers", "metadata", "outputs", "inputEvents" };

	private static HashSet<string> _deviceGroupResourceNames = new HashSet<string> { "cameraGroups", "microphoneGroups", "speakerGroups", "metadataGroups", "outputGroups", "inputEventGroups" };

	private static Dictionary<string, string> _deviceGroupToDeviceName = new Dictionary<string, string>
	{
		{ "cameraGroups", "cameras" },
		{ "microphoneGroups", "microphones" },
		{ "speakerGroups", "speakers" },
		{ "metadataGroups", "metadata" },
		{ "outputGroups", "outputs" },
		{ "inputEventGroups", "inputEvents" }
	};

	public RolePermissionsController()
		: this(new RestApiRequestContextFactory())
	{
	}

	public RolePermissionsController(IRestApiRequestContextFactory requestContextFactory)
	{
		_requestContextFactory = requestContextFactory;
	}

	[HttpGet]
	public IHttpActionResult Get()
	{
		throw new NotImplementedException();
	}

	[HttpPost]
	public IHttpActionResult Post([FromBody] JObject postContent)
	{
		using IRestApiRequestContext restApiRequestContext = _requestContextFactory.GetRequestContext(base.Request);
		SecurityManagerService service = restApiRequestContext.GetService<SecurityManagerService>();
		if (!service.IsAdministrator(restApiRequestContext))
		{
			return Unauthorized();
		}
		using (restApiRequestContext.CreateSecurityComponent())
		{
			if (restApiRequestContext.RestApiRequestParameters.Task == "GetAllPermissions")
			{
				PathDataModel path = ParseGetAllRequest(postContent);
				Dictionary<Guid, PermissionObjectSecurity> andCompileAllSecurityEntries = GetAndCompileAllSecurityEntries(restApiRequestContext, service, path);
				List<object> list = new List<object>();
				foreach (KeyValuePair<Guid, PermissionObjectSecurity> item in andCompileAllSecurityEntries)
				{
					dynamic val = new ExpandoObject();
					dynamic val2 = new ExpandoObject();
					val2.type = "roles";
					val2.id = item.Key.ToString();
					val.path = val2;
					val.permissionMask = item.Value.PermissionMask;
					if (item.Value.TimeProfiles != null && item.Value.TimeProfiles.Count != 0)
					{
						val.timeProfiles = item.Value.TimeProfiles;
					}
					if (item.Value.Priorities != null && item.Value.Priorities.Count != 0)
					{
						val.priorities = item.Value.Priorities;
					}
					list.Add(val);
				}
				Dictionary<string, object> content = new Dictionary<string, object> { { "array", list } };
				return Ok(content);
			}
			return BadRequest("Unknown task");
		}
	}

	[HttpPost]
	public IHttpActionResult Post([FromBody] JObject postContent, string roleId)
	{
		using IRestApiRequestContext restApiRequestContext = _requestContextFactory.GetRequestContext(base.Request);
		if (string.IsNullOrEmpty(roleId))
		{
			throw new ArgumentException("Role id is required");
		}
		SecurityManagerService service = restApiRequestContext.GetService<SecurityManagerService>();
		if (!service.IsAdministrator(restApiRequestContext))
		{
			return Unauthorized();
		}
		if (Role.QueryRoleById(restApiRequestContext, roleId) == null)
		{
			throw new ArgumentException("Role does not exists");
		}
		using (restApiRequestContext.CreateSecurityComponent())
		{
			string task = restApiRequestContext.RestApiRequestParameters.Task;
			if (!(task == "GetPermissions"))
			{
				if (task == "SetPermissions")
				{
					Dictionary<string, List<PermissionObjectSecurity>> dictionary = GroupRequestByResourceName(ParseSetRequest(postContent));
					HashSet<Guid> allConfiguredTimeProfileIds = (from p in TimeProfile.QueryTimeProfileIds(restApiRequestContext)
						select new Guid(p)).ToHashSet();
					ValidateRequest(restApiRequestContext, service, dictionary, allConfiguredTimeProfileIds);
					foreach (string key in dictionary.Keys)
					{
						HashSet<string> ids = dictionary[key].Select((PermissionObjectSecurity e) => e.Path.Id).ToHashSet();
						SecurityNamespaceResourceNameConverter.TryGetSecurityNamespaceId(key, out var id);
						IGeminiSecurityNamespace securityNamespace = service.GetSecurityNamespace(restApiRequestContext, id);
						Dictionary<string, string> objectNames = GetObjectNames(restApiRequestContext, securityNamespace, key, ids);
						UpdateSecurityEntries(restApiRequestContext, securityNamespace, service, roleId, key, dictionary[key], objectNames, allConfiguredTimeProfileIds);
					}
					return Ok();
				}
				return BadRequest("Unknown task");
			}
			List<Tuple<string, string>> itemIdList = ParseGetRequest(postContent);
			Dictionary<string, HashSet<Guid>> groupByResource = GroupContentByResourceName(restApiRequestContext, service, itemIdList);
			Dictionary<Guid, PermissionObjectSecurity> andCompileSecurityEntries = GetAndCompileSecurityEntries(restApiRequestContext, service, roleId, groupByResource);
			List<object> list = new List<object>();
			foreach (KeyValuePair<Guid, PermissionObjectSecurity> item in andCompileSecurityEntries)
			{
				list.Add(item.Value);
			}
			Dictionary<string, object> content = new Dictionary<string, object> { { "array", list } };
			return Ok(content);
		}
	}

	private static void ValidateRequest(IGeminiRequestContext requestContext, SecurityManagerService securityManagerService, Dictionary<string, List<PermissionObjectSecurity>> groupRequestByResource, HashSet<Guid> allConfiguredTimeProfileIds)
	{
		foreach (string key in groupRequestByResource.Keys)
		{
			if (!SecurityNamespaceResourceNameConverter.TryGetSecurityNamespaceId(key, out var id))
			{
				throw new ArgumentException("Security namespace not found from resourceName: " + key);
			}
			ulong possiblePermissionsMask = SecurityPermissionDataTranslater.GetPossiblePermissionsMask(securityManagerService.GetSecurityNamespace(requestContext, id));
			ulong possibleTimeprofileMask = SecurityPermissionDataTranslater.GetPossibleTimeprofileMask(key);
			foreach (PermissionObjectSecurity item in groupRequestByResource[key])
			{
				if (item.Path == null || item.Path.Id == null || item.Path.Type == null)
				{
					throw new ArgumentException("Invalid path in request");
				}
				if ((item.PermissionMask & ~possiblePermissionsMask) != 0L)
				{
					throw new ArgumentException("Invalid permission mask");
				}
				if (item.TimeProfiles != null)
				{
					foreach (PermissionTimeprofile timeProfile in item.TimeProfiles)
					{
						if ((timeProfile.RelatedToMask & possibleTimeprofileMask) == 0L)
						{
							throw new ArgumentException("Invalid relatedToMask in timeprofile " + key + "\\" + item.Path.Id);
						}
						CheckValidTimeProfileId(timeProfile.TimeProfilePath?.Id, allConfiguredTimeProfileIds);
						if (timeProfile.TimeBackwards < 0 || timeProfile.TimeBackwards > SecurityPermissionDataTranslater.MaxTimeBackwards)
						{
							throw new ArgumentException("Invalid timeBackwards in timeprofile " + key + "\\" + item.Path.Id);
						}
					}
				}
				if (item.Priorities == null)
				{
					continue;
				}
				foreach (SecurityPriority priority in item.Priorities)
				{
					if (!SecurityPermissionDataTranslater.IsPriorityValid(key, priority.RelatedToMask, priority.Priority))
					{
						throw new ArgumentException("Invalid priority in " + key + "\\" + item.Path.Id);
					}
				}
			}
		}
	}

	private static Dictionary<string, string> GetObjectNames(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string resourceName, HashSet<string> ids)
	{
		if (IsDeviceNamespace(securityNamespace.NamespaceInfo.NamespaceGuid))
		{
			using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
			{
				return deviceComponent.QueryDeviceNamesByIds(ids);
			}
		}
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		using (requestContext.CreateSecurityComponent())
		{
			INameLookupService nameLookupService = requestContext.GetNameLookupService();
			foreach (string id in ids)
			{
				dictionary[id] = nameLookupService.GetObjectName(requestContext, securityNamespace.NamespaceInfo.NamespaceId, id);
			}
			return dictionary;
		}
	}

	private static Dictionary<Guid, PermissionObjectSecurity> GetAndCompileSecurityEntries(IRestApiRequestContext requestContext, SecurityManagerService securityManagerService, string roleId, Dictionary<string, HashSet<Guid>> groupByResource)
	{
		Dictionary<Guid, PermissionObjectSecurity> dictionary = new Dictionary<Guid, PermissionObjectSecurity>();
		foreach (string key2 in groupByResource.Keys)
		{
			HashSet<Guid> hashSet = groupByResource[key2];
			if (hashSet.Count == 0)
			{
				continue;
			}
			foreach (Guid item in hashSet)
			{
				dictionary[item] = new PermissionObjectSecurity(key2, item.ToString());
			}
			if (SecurityNamespaceResourceNameConverter.TryGetSecurityNamespaceId(key2, out var id))
			{
				SecurityNamespaceActionConverter securityNamespaceActionConverter = new SecurityNamespaceActionConverter(securityManagerService.GetSecurityNamespace(requestContext, id).NamespaceInfo);
				using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
				{
					RolePermissionObjectEntry[] array = securityComponent.QueryRolePermissionsForObjects(id, roleId, hashSet);
					if (array != null)
					{
						RolePermissionObjectEntry[] array2 = array;
						foreach (RolePermissionObjectEntry rolePermissionObjectEntry in array2)
						{
							Guid key = new Guid(rolePermissionObjectEntry.ObjectId);
							if (dictionary.TryGetValue(key, out var value) && securityNamespaceActionConverter.TryGetActionBit(rolePermissionObjectEntry.ActionName, out var id2))
							{
								value.StoreEntry(id2);
							}
						}
					}
					if (IsDeviceNamespace(id))
					{
						foreach (AccessControlDeviceTimeProfileEntry item2 in securityComponent.QueryTimeProfiles(roleId, hashSet))
						{
							ulong id3;
							if (SecurityPermissionDataTranslater.IsTimeProfileInFactAPriority(item2.ActionId, item2.Value, out var actionName, out var priorityValue))
							{
								if (securityNamespaceActionConverter.TryGetActionBit(actionName, out id3) && dictionary.TryGetValue(item2.DeviceId, out var value2))
								{
									value2.StorePriority(id3, priorityValue);
								}
								continue;
							}
							string actionName2 = SecurityPermissionDataTranslater.ConvertLegacyDeviceActionIdToActionName(item2.ActionId);
							if (securityNamespaceActionConverter.TryGetActionBit(actionName2, out id3) && !SecurityPermissionDataTranslater.ShouldBeIgnored(id3) && dictionary.TryGetValue(item2.DeviceId, out var value3))
							{
								value3.StoreTimeProfile(id3, item2.IsDefaultTimeProfile, item2.TimeProfileId, item2.TimeBackward);
							}
						}
						continue;
					}
					foreach (AccessControlTimeProfileObjectEntry item3 in securityComponent.QueryNonDeviceTimeProfiles(id, roleId, hashSet))
					{
						if (securityNamespaceActionConverter.TryGetActionBit(item3.ActionName, out var id4) && dictionary.TryGetValue(new Guid(item3.ObjectId), out var value4))
						{
							value4.StoreTimeProfile(id4, item3.IsDefaultTimeProfile, item3.TimeProfileId, 0);
						}
					}
				}
				continue;
			}
			throw new ArgumentException("No security namespace found for resource name: " + key2);
		}
		return dictionary;
	}

	private static Dictionary<Guid, PermissionObjectSecurity> GetAndCompileAllSecurityEntries(IRestApiRequestContext requestContext, SecurityManagerService securityManagerService, PathDataModel path)
	{
		if (SecurityNamespaceResourceNameConverter.TryGetSecurityNamespaceId(path.Type, out var id))
		{
			SecurityNamespaceActionConverter securityNamespaceActionConverter = new SecurityNamespaceActionConverter(securityManagerService.GetSecurityNamespace(requestContext, id).NamespaceInfo);
			using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
			Dictionary<Guid, PermissionObjectSecurity> dictionary = new Dictionary<Guid, PermissionObjectSecurity>();
			RolePermissionObjectEntry[] array = securityComponent.QueryPermissionsForObject(id, new Guid(path.Id));
			if (array != null)
			{
				RolePermissionObjectEntry[] array2 = array;
				foreach (RolePermissionObjectEntry rolePermissionObjectEntry in array2)
				{
					if (Guid.TryParse(rolePermissionObjectEntry.RoleId, out var result) && securityNamespaceActionConverter.TryGetActionBit(rolePermissionObjectEntry.ActionName, out var id2))
					{
						if (dictionary.TryGetValue(result, out var value))
						{
							value.StoreEntry(id2);
							continue;
						}
						dictionary[result] = new PermissionObjectSecurity(path.Type, path.Id)
						{
							PermissionMask = id2
						};
					}
				}
			}
			if (IsDeviceNamespace(id))
			{
				foreach (AccessControlDeviceTimeProfileEntry item in securityComponent.QueryTimeProfilesAllRoles(new Guid(path.Id)))
				{
					ulong id3;
					if (SecurityPermissionDataTranslater.IsTimeProfileInFactAPriority(item.ActionId, item.Value, out var actionName, out var priorityValue))
					{
						if (securityNamespaceActionConverter.TryGetActionBit(actionName, out id3) && dictionary.TryGetValue(item.RoleId, out var value2))
						{
							value2.StorePriority(id3, priorityValue);
						}
						continue;
					}
					string actionName2 = SecurityPermissionDataTranslater.ConvertLegacyDeviceActionIdToActionName(item.ActionId);
					if (securityNamespaceActionConverter.TryGetActionBit(actionName2, out id3) && !SecurityPermissionDataTranslater.ShouldBeIgnored(id3) && dictionary.TryGetValue(item.RoleId, out var value3))
					{
						value3.StoreTimeProfile(id3, item.IsDefaultTimeProfile, item.TimeProfileId, item.TimeBackward);
					}
				}
			}
			else
			{
				foreach (AccessControlTimeProfileObjectEntry item2 in securityComponent.QueryNonDeviceTimeProfilesAllRoles(id, new Guid(path.Id)))
				{
					if (securityNamespaceActionConverter.TryGetActionBit(item2.ActionName, out var id4) && dictionary.TryGetValue(new Guid(item2.RoleId), out var value4))
					{
						value4.StoreTimeProfile(id4, item2.IsDefaultTimeProfile, item2.TimeProfileId, 0);
					}
				}
			}
			return dictionary;
		}
		throw new ArgumentException("No security namespace found for resource name: " + path.Type);
	}

	private static void UpdateSecurityEntries(IRestApiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, SecurityManagerService securityManagerService, string roleId, string resourceName, List<PermissionObjectSecurity> updates, Dictionary<string, string> resourceObjectNames, HashSet<Guid> allConfiguredTimeProfileIds)
	{
		new SecurityNamespaceActionConverter(securityNamespace.NamespaceInfo);
		HashSet<Guid> value = updates.Select((PermissionObjectSecurity u) => new Guid(u.Path.Id)).ToHashSet();
		Dictionary<string, HashSet<Guid>> dictionary = new Dictionary<string, HashSet<Guid>>();
		dictionary[resourceName] = value;
		Dictionary<Guid, PermissionObjectSecurity> andCompileSecurityEntries = GetAndCompileSecurityEntries(requestContext, securityManagerService, roleId, dictionary);
		using (requestContext.CreateSecurityComponent())
		{
			foreach (PermissionObjectSecurity update in updates)
			{
				if (!andCompileSecurityEntries.TryGetValue(new Guid(update.Path.Id), out var value2))
				{
					continue;
				}
				if (value2.PermissionMask != update.PermissionMask)
				{
					ulong removePermissions = value2.PermissionMask & ~update.PermissionMask;
					ulong addPermissions = ~value2.PermissionMask & update.PermissionMask;
					string value3 = "unknown";
					resourceObjectNames.TryGetValue(update.Path.Id, out value3);
					StoreAccessControlEntries(requestContext, securityNamespace, roleId, resourceName, update.Path.Id, value3, removePermissions, addPermissions);
				}
				List<PermissionTimeprofile> list = new List<PermissionTimeprofile>();
				Dictionary<ulong, int> dictionary2 = new Dictionary<ulong, int>();
				foreach (PermissionTimeprofile newTp in update.TimeProfiles ?? new List<PermissionTimeprofile>())
				{
					PermissionTimeprofile permissionTimeprofile = value2.TimeProfiles?.FirstOrDefault((PermissionTimeprofile t) => t.RelatedToMask == newTp.RelatedToMask);
					if (permissionTimeprofile == null || permissionTimeprofile.TimeBackwards != newTp.TimeBackwards || permissionTimeprofile.TimeProfilePath?.Id != newTp.TimeProfilePath?.Id)
					{
						list.Add(newTp);
					}
				}
				foreach (SecurityPriority priority in update.Priorities ?? new List<SecurityPriority>())
				{
					SecurityPriority securityPriority = value2.Priorities?.FirstOrDefault((SecurityPriority p) => p.RelatedToMask == priority.RelatedToMask);
					if (securityPriority != null)
					{
						if (priority.Priority != securityPriority.Priority)
						{
							dictionary2[priority.RelatedToMask] = priority.Priority;
						}
					}
					else
					{
						dictionary2[priority.RelatedToMask] = priority.Priority;
					}
				}
				if (list.Count != 0 || dictionary2.Count != 0)
				{
					string value4 = "unknown";
					resourceObjectNames.TryGetValue(update.Path.Id, out value4);
					UpdateChangedTimeProfiles(requestContext, securityNamespace, roleId, update.Path, value4, list, dictionary2);
				}
			}
		}
	}

	private static void StoreAccessControlEntries(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string roleId, string resourceName, string id, string objectName, ulong removePermissions, ulong addPermissions)
	{
		List<AccessControlEntry> list = new List<AccessControlEntry>();
		List<AccessControlEntry> list2 = new List<AccessControlEntry>();
		for (int i = 1; i < 40; i++)
		{
			ulong num = (ulong)(1L << i - 1);
			if ((addPermissions & num) != 0L)
			{
				list.Add(ConstructACE(securityNamespace, roleId, num));
			}
			if ((removePermissions & num) != 0L)
			{
				list2.Add(ConstructACE(securityNamespace, roleId, num));
			}
		}
		if (list.Count > 0 && list2.Count > 0)
		{
			requestContext.GetService<SecuredSecurityManager>().AddRemovePermissions(requestContext, securityNamespace, id, objectName, list.ToArray(), list2.ToArray());
		}
		else if (list.Count > 0)
		{
			requestContext.GetService<SecuredSecurityManager>().AddPermissions(requestContext, securityNamespace, id, objectName, list.ToArray());
		}
		else if (list2.Count > 0)
		{
			requestContext.GetService<SecuredSecurityManager>().RemovePermissions(requestContext, securityNamespace, id, objectName, list2.ToArray());
		}
	}

	private static AccessControlEntry ConstructACE(IGeminiSecurityNamespace securityNamespace, string roleId, ulong mask)
	{
		return new AccessControlEntry((securityNamespace.NamespaceInfo.Actions.FirstOrDefault((ActionDefinition a) => a.PermissionBit == mask) ?? throw new InvalidOperationException($"RolePermission: SecurityAction not found for mask {mask}, in NS:{securityNamespace.NamespaceInfo.NamespaceId} - ignored")).Name, roleId, accessDeny: false);
	}

	private static void UpdateChangedTimeProfiles(IGeminiRequestContext requestContext, IGeminiSecurityNamespace securityNamespace, string roleId, PathDataModel path, string objectName, List<PermissionTimeprofile> changedProfiles, Dictionary<ulong, int> newPriorities)
	{
		if (IsDeviceNamespace(securityNamespace.NamespaceInfo.NamespaceGuid))
		{
			List<AccessControlDeviceTimeProfileEntry> list = new List<AccessControlDeviceTimeProfileEntry>();
			foreach (PermissionTimeprofile changedProfile in changedProfiles)
			{
				bool flag = changedProfile.TimeProfilePath?.Id == SecurityPermissionDataTranslater.DefaultTimeProfileId || string.IsNullOrEmpty(changedProfile.TimeProfilePath?.Id);
				string timeProfileId = ((changedProfile.TimeProfilePath?.Id == SecurityPermissionDataTranslater.AlwaysTimeProfileId) ? null : (flag ? string.Empty : changedProfile.TimeProfilePath?.Id));
				int timeBackwards = changedProfile.TimeBackwards;
				string actionIdFromMask = SecurityPermissionDataTranslater.GetActionIdFromMask(securityNamespace, changedProfile.RelatedToMask);
				list.Add(new AccessControlDeviceTimeProfileEntry
				{
					ActionId = actionIdFromMask,
					TimeProfileId = timeProfileId,
					TimeBackward = timeBackwards,
					DeviceId = new Guid(path.Id),
					IsDefaultTimeProfile = flag,
					RoleId = new Guid(roleId),
					Value = "True"
				});
			}
			foreach (KeyValuePair<ulong, int> newPriority in newPriorities)
			{
				string actionId = SecurityPermissionDataTranslater.ConvertPriorityActionId(securityNamespace, newPriority.Key);
				list.Add(new AccessControlDeviceTimeProfileEntry
				{
					ActionId = actionId,
					TimeProfileId = null,
					TimeBackward = 0,
					DeviceId = new Guid(path.Id),
					IsDefaultTimeProfile = false,
					RoleId = new Guid(roleId),
					Value = newPriority.Value.ToString(CultureInfo.InvariantCulture)
				});
			}
			if (list.Count > 0)
			{
				requestContext.GetService<SecuredSecurityManager>().UpdateDeviceTimeProfilePermissions(requestContext, securityNamespace, list);
			}
			return;
		}
		foreach (PermissionTimeprofile changedProfile2 in changedProfiles)
		{
			string actionNameFromBit = new SecurityNamespaceActionConverter(securityNamespace.NamespaceInfo).GetActionNameFromBit(changedProfile2.RelatedToMask);
			if (changedProfile2.TimeProfilePath?.Id == SecurityPermissionDataTranslater.AlwaysTimeProfileId)
			{
				requestContext.GetService<SecuredSecurityManager>().DeleteAccessControlTimeProfileEntry(requestContext, securityNamespace, path.Id, objectName, roleId, actionNameFromBit);
				continue;
			}
			bool isDefaultTimeProfile = false;
			string timeProfileId2 = null;
			if (changedProfile2.TimeProfilePath?.Id == SecurityPermissionDataTranslater.DefaultTimeProfileId)
			{
				isDefaultTimeProfile = true;
			}
			else
			{
				timeProfileId2 = changedProfile2.TimeProfilePath?.Id;
			}
			requestContext.GetService<SecuredSecurityManager>().AddUpdateAccessControlTimeProfileEntry(requestContext, securityNamespace, path.Id, objectName, roleId, actionNameFromBit, timeProfileId2, isDefaultTimeProfile);
		}
	}

	private static bool IsDeviceNamespace(Guid securityNamespaceId)
	{
		return _deviceSecNamespaces.Contains(securityNamespaceId);
	}

	private static Dictionary<string, HashSet<Guid>> GroupContentByResourceName(IRestApiRequestContext requestContext, ISecurityManagerService securityManagerService, List<Tuple<string, string>> itemIdList)
	{
		Dictionary<string, HashSet<Guid>> dictionary = new Dictionary<string, HashSet<Guid>>();
		Dictionary<string, HashSet<Guid>> dictionary2 = new Dictionary<string, HashSet<Guid>>();
		foreach (Tuple<string, string> itemId in itemIdList)
		{
			string item = itemId.Item1;
			string item2 = itemId.Item2;
			string text = item;
			HashSet<Guid> hashSet = new HashSet<Guid>();
			if (item == "securityNamespaces")
			{
				text = ConvertSecurityNamespaceIdToResourceName(item2);
				HashSet<Guid> allSecurityObjects = QueryAllObjectsWithinSecurityNamespace(requestContext, securityManagerService, item2);
				HashSet<Guid> other = FilterEnabledObjects(requestContext, text, allSecurityObjects);
				hashSet.UnionWith(other);
			}
			else if (IsDeviceGroup(item))
			{
				hashSet.UnionWith(QueryAllObjectsFromGroup(requestContext, item, item2));
				text = _deviceGroupToDeviceName[item];
			}
			else
			{
				Guid item3 = Guid.Parse(item2);
				hashSet.Add(item3);
				if (dictionary.TryGetValue(text, out var value))
				{
					value.Add(item3);
				}
				else
				{
					dictionary[text] = new HashSet<Guid> { item3 };
				}
			}
			if (dictionary2.TryGetValue(text, out var value2))
			{
				value2.UnionWith(hashSet);
			}
			else
			{
				dictionary2[text] = hashSet;
			}
		}
		foreach (string key in dictionary.Keys)
		{
			if (!SecurityNamespaceResourceNameConverter.TryGetSecurityNamespaceId(key, out var id))
			{
				throw new ArgumentException("Invalid path type: " + key);
			}
			string text2 = QuerySecurityObjectWithinNamespace(requestContext, securityManagerService, id, dictionary[key]);
			if (!string.IsNullOrEmpty(text2))
			{
				throw new ArgumentException("Path id:" + text2 + " not found within defined resource type: " + key);
			}
		}
		return dictionary2;
	}

	private static Dictionary<string, List<PermissionObjectSecurity>> GroupRequestByResourceName(List<PermissionObjectSecurity> entries)
	{
		Dictionary<string, List<PermissionObjectSecurity>> dictionary = new Dictionary<string, List<PermissionObjectSecurity>>();
		foreach (PermissionObjectSecurity entry in entries)
		{
			if (entry.Path == null || entry.Path.Type == null)
			{
				throw new ArgumentException("Invalid path: " + entry.Path);
			}
			if (dictionary.TryGetValue(entry.Path.Type, out var value))
			{
				value.Add(entry);
				continue;
			}
			dictionary[entry.Path.Type] = new List<PermissionObjectSecurity> { entry };
		}
		return dictionary;
	}

	private static List<Tuple<string, string>> ParseGetRequest(JObject postContent)
	{
		if (postContent == null)
		{
			throw new ArgumentNullException("postContent", "Post content cannot be null");
		}
		if (!postContent.TryGetValue("pathArray", out JToken value))
		{
			throw new ArgumentException("Post content must contain a 'pathArray' property", "postContent");
		}
		List<Tuple<string, string>> list = new List<Tuple<string, string>>();
		foreach (JToken item in (JArray)value)
		{
			JObject obj = (item as JObject) ?? throw new ArgumentException("Each item in 'pathArray' must be a JSON object", "postContent");
			string text = obj["id"]?.Value<string>();
			string text2 = obj["type"]?.Value<string>();
			if (text == null || text2 == null)
			{
				throw new ArgumentException("Each item in 'pathArray' must contain 'id' and 'type' properties", "postContent");
			}
			list.Add(new Tuple<string, string>(text2.ToString(), text.ToString()));
		}
		return list;
	}

	private static List<PermissionObjectSecurity> ParseSetRequest(JObject requestBody)
	{
		if (requestBody == null)
		{
			throw new ArgumentNullException("requestBody", "Post content cannot be null");
		}
		if (!requestBody.TryGetValue("array", out JToken value))
		{
			throw new ArgumentException("Post content must contain an 'array' property", "requestBody");
		}
		List<PermissionObjectSecurity> list = new List<PermissionObjectSecurity>();
		foreach (JToken item2 in (JArray)value)
		{
			PermissionObjectSecurity item = JsonConvert.DeserializeObject<PermissionObjectSecurity>(item2.ToString());
			list.Add(item);
		}
		return list;
	}

	private static PathDataModel ParseGetAllRequest(JObject postContent)
	{
		if (postContent == null)
		{
			throw new ArgumentNullException("postContent", "Post content cannot be null");
		}
		if (!postContent.TryGetValue("path", out JToken value) || !(value is JObject jObject))
		{
			throw new ArgumentException("Post content must contain a 'path' object", "postContent");
		}
		string text = jObject["id"]?.Value<string>();
		string text2 = jObject["type"]?.Value<string>();
		if (text == null || text2 == null)
		{
			throw new ArgumentException("The item in 'path' must contain 'id' and 'type' properties", "postContent");
		}
		return new PathDataModel
		{
			Id = text,
			Type = text2
		};
	}

	private static HashSet<Guid> QueryAllObjectsWithinSecurityNamespace(IRestApiRequestContext requestContext, ISecurityManagerService securityManagerService, string securityNamespaceId)
	{
		Guid guid = new Guid(securityNamespaceId);
		IGeminiSecurityNamespace securityNamespace = securityManagerService.GetSecurityNamespace(requestContext, guid);
		HashSet<Guid> hashSet = new HashSet<Guid>();
		if (guid == SecurityNamespaceIdentifier.UserDefinedEvent)
		{
			using (IUserDefinedEventComponent userDefinedEventComponent = requestContext.CreateUserDefinedEventComponent())
			{
				string[] array = userDefinedEventComponent.QueryUserDefinedEventIds();
				if (array != null)
				{
					string[] array2 = array;
					foreach (string g in array2)
					{
						Guid item = new Guid(g);
						if (!EventDefinition._external_System_events.Contains(item))
						{
							hashSet.Add(item);
						}
					}
				}
			}
			return hashSet;
		}
		using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
		{
			string[] array3 = securityComponent.QuerySecurityObjects(guid, securityNamespace.NamespaceInfo.UserDefined);
			if (array3 != null)
			{
				if (guid == SecurityNamespaceIdentifier.ViewGroup)
				{
					string[] array2 = array3;
					foreach (string text in array2)
					{
						if (!text.StartsWith("4BF8A8B0-54AB-4E82-994F-98D8E9584176", StringComparison.OrdinalIgnoreCase))
						{
							hashSet.Add(new Guid(text));
						}
					}
				}
				else
				{
					hashSet = array3.Select((string o) => new Guid(o)).ToHashSet();
				}
			}
		}
		return hashSet;
	}

	private static HashSet<Guid> FilterEnabledObjects(IRestApiRequestContext requestContext, string resourceName, HashSet<Guid> allSecurityObjects)
	{
		if (_deviceResourceNames.Contains(resourceName))
		{
			using (IDeviceComponent deviceComponent = requestContext.CreateDeviceComponent())
			{
				return (from o in deviceComponent.FilterOnEnabledDevices(allSecurityObjects)
					select new Guid(o)).ToHashSet();
			}
		}
		return allSecurityObjects;
	}

	private static bool IsDeviceGroup(string type)
	{
		return _deviceGroupResourceNames.Contains(type);
	}

	private static HashSet<Guid> QueryAllObjectsFromGroup(IRestApiRequestContext requestContext, string groupResourceName, string groupId)
	{
		string resourceName = groupResourceName.Substring(0, groupResourceName.Length - 6);
		DeviceType deviceType = DeviceGroupConfigurationApi.ConvertItemTypeToDeviceType(new NameConvert().ResourcePluralToItemType(resourceName, throwForInvalid: true) + "Group");
		using (IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent())
		{
			List<Tuple<Guid, Guid>> list = DeviceGroup.QueryDeviceGroupParentAndGroupIdsNonAdmin(requestContext, groupId, deviceType);
			HashSet<Guid> hashSet = new HashSet<Guid>
			{
				new Guid(groupId)
			};
			foreach (Tuple<Guid, Guid> item in list)
			{
				hashSet.Add(item.Item1);
				hashSet.Add(item.Item2);
			}
			string[] array = deviceGroupComponent.QueryDeviceGroupMembers(hashSet.Select((Guid id) => id.ToString()).ToArray());
			if (array != null && array.Length != 0)
			{
				return array.Select((string m) => new Guid(m)).ToHashSet();
			}
		}
		return new HashSet<Guid>();
	}

	private static string QuerySecurityObjectWithinNamespace(IRestApiRequestContext requestContext, ISecurityManagerService securityManagerService, Guid securityNamespaceGuid, HashSet<Guid> idList)
	{
		securityManagerService.GetSecurityNamespace(requestContext, securityNamespaceGuid);
		using ISecurityComponent securityComponent = requestContext.CreateSecurityComponent();
		List<string> list = securityComponent.VerifySecurityObjects(securityNamespaceGuid, idList);
		if (list != null && list.Count > 0)
		{
			return list[0];
		}
		return null;
	}

	private static string ConvertSecurityNamespaceIdToResourceName(string securityNamespaceId)
	{
		if (string.IsNullOrEmpty(securityNamespaceId))
		{
			throw new ArgumentException("Security namespace ID cannot be null or empty", "securityNamespaceId");
		}
		if (!Guid.TryParse(securityNamespaceId, out var result))
		{
			throw new ArgumentException("Invalid security namespace ID: " + securityNamespaceId, "securityNamespaceId");
		}
		if (!SecurityNamespaceResourceNameConverter.TryGetResourceName(result, out var resourceName))
		{
			throw new ArgumentException("No resource name found for security namespace ID: " + securityNamespaceId, "securityNamespaceId");
		}
		return resourceName;
	}

	private static void CheckValidTimeProfileId(string idString, HashSet<Guid> allConfiguredTimeProfileIds)
	{
		if (idString != SecurityPermissionDataTranslater.AlwaysTimeProfileId && idString != SecurityPermissionDataTranslater.DefaultTimeProfileId && !string.IsNullOrEmpty(idString) && !allConfiguredTimeProfileIds.Contains(new Guid(idString)))
		{
			throw new ArgumentException("TimeProfile id " + idString + " not found in system");
		}
	}
}
