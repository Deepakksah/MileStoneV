using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Platform.JsonHandling;
using VideoOS.Platform.Login;
using VideoOS.Platform.Proxy.RestApi;

namespace VideoOS.Platform.Proxy.SecurityApi;

public class SecurityApiClient : RestApiBase, ISecurityApiClient
{
	private const string JsonField_securityActions = "securityActions";

	private const string JsonField_id = "id";

	private const string JsonField_name = "name";

	private const string JsonField_resourceType = "resourceType";

	private const string JsonField_structure = "structure";

	private const string JsonField_userDefined = "userDefined";

	private const string JsonField_mask = "mask";

	private const string JsonField_isAdministrator = "isAdministrator";

	private const string JsonField_visible = "visible";

	private const string JsonField_securityMask = "securityMask";

	private const string JsonField_objectPermissions = "objectPermissions";

	private ConcurrentDictionary<Guid, SecurityNamespaceModel> _securityNamespaces = new ConcurrentDictionary<Guid, SecurityNamespaceModel>();

	private ConcurrentDictionary<Guid, EffectivePermissionsModel> _effectivePermissions = new ConcurrentDictionary<Guid, EffectivePermissionsModel>();

	private string _userOrRoleId;

	public new void Initialize(LoginSettings loginSettings, bool debug)
	{
		if (loginSettings == null)
		{
			throw new ArgumentNullException("loginSettings");
		}
		if (string.Compare(loginSettings.ServerProductInfo.ProductVersion, "24.2", ignoreCase: false, CultureInfo.InvariantCulture) >= 0)
		{
			base.Initialize(loginSettings, debug);
			return;
		}
		throw new CommunicationMIPException("Server does not support /securityNamespaces RestAPI");
	}

	public void Initialize(LoginSettings loginSettings, bool debug, string userOrRoleId)
	{
		if (string.IsNullOrEmpty(userOrRoleId))
		{
			throw new ArgumentNullException("userOrRoleId");
		}
		if (loginSettings == null)
		{
			throw new ArgumentNullException("loginSettings");
		}
		_userOrRoleId = userOrRoleId;
		if (string.Compare(loginSettings.ServerProductInfo.ProductVersion, "25.3", ignoreCase: false, CultureInfo.InvariantCulture) >= 0)
		{
			base.Initialize(loginSettings, debug);
			return;
		}
		throw new CommunicationMIPException("Server does not support effectivePermisssions with userOrRoleId parameter");
	}

	public void Close()
	{
		FlushCache();
		Dispose();
	}

	public bool HasPermission(Guid securityNamespaceId, Guid objectId, string permissionName)
	{
		try
		{
			EnsureSecurityNamespaceLoaded(securityNamespaceId);
			EnsurePermissionLoaded(securityNamespaceId);
			SecurityNamespaceModel securityNamespaceModel = _securityNamespaces[securityNamespaceId];
			EffectivePermissionsModel effectivePermissionsModel = _effectivePermissions[securityNamespaceId];
			ulong num = securityNamespaceModel.LookupPermissionMask(permissionName);
			if (num == 0L)
			{
				throw new MIPException("Unknown permissionName: " + permissionName);
			}
			if (effectivePermissionsModel.isAdministrator)
			{
				return true;
			}
			ObjectPermissionsModel objectPermissionsModel = effectivePermissionsModel.LookupObject(objectId);
			if (objectPermissionsModel == null)
			{
				return false;
			}
			return (objectPermissionsModel.mask & num) != 0;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public async Task<bool> HasPermissionAsync(Guid securityNamespaceId, Guid objectId, string permissionName, CancellationToken cancellationToken)
	{
		_ = 1;
		try
		{
			await EnsureSecurityNamespaceLoadedAsync(securityNamespaceId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			await EnsurePermissionLoadedAsync(securityNamespaceId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			SecurityNamespaceModel securityNamespaceModel = _securityNamespaces[securityNamespaceId];
			EffectivePermissionsModel effectivePermissionsModel = _effectivePermissions[securityNamespaceId];
			ulong num = securityNamespaceModel.LookupPermissionMask(permissionName);
			if (num == 0L)
			{
				throw new MIPException("Unknown permissionName: " + permissionName);
			}
			if (effectivePermissionsModel.isAdministrator)
			{
				return true;
			}
			ObjectPermissionsModel objectPermissionsModel = effectivePermissionsModel.LookupObject(objectId);
			if (objectPermissionsModel == null)
			{
				return false;
			}
			return (objectPermissionsModel.mask & num) != 0;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public bool HasPermission(Guid securityNamespaceId, string permissionName)
	{
		return HasPermission(securityNamespaceId, securityNamespaceId, permissionName);
	}

	public void PreloadCache(IEnumerable<Guid> securityNamespaceIds)
	{
		foreach (Guid securityNamespaceId in securityNamespaceIds)
		{
			EnsureSecurityNamespaceLoaded(securityNamespaceId);
			EnsurePermissionLoaded(securityNamespaceId);
		}
	}

	public async Task PreloadCacheAsync(IEnumerable<Guid> securityNamespaceIds, CancellationToken cancellationToken)
	{
		foreach (Guid securityNamespaceId in securityNamespaceIds)
		{
			await EnsureSecurityNamespaceLoadedAsync(securityNamespaceId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			await EnsurePermissionLoadedAsync(securityNamespaceId, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
		}
	}

	public ObjectPermissionsModel[] GetAvailableObjects(Guid securityNamespaceId)
	{
		EnsureSecurityNamespaceLoaded(securityNamespaceId);
		EnsurePermissionLoaded(securityNamespaceId);
		if (_effectivePermissions.TryGetValue(securityNamespaceId, out var value))
		{
			return value.objectPermissions;
		}
		Guid guid = securityNamespaceId;
		throw new MIPException("No effective permissions found for security namespace: " + guid.ToString());
	}

	public void FlushCache()
	{
		_securityNamespaces.Clear();
		_effectivePermissions.Clear();
	}

	public void FlushCache(Guid securityNamespaceId)
	{
		_securityNamespaces.TryRemove(securityNamespaceId, out var _);
		_effectivePermissions.TryRemove(securityNamespaceId, out var _);
	}

	private void EnsureSecurityNamespaceLoaded(Guid securityNamespaceId)
	{
		if (!_securityNamespaces.ContainsKey(securityNamespaceId))
		{
			string relativeUrl = string.Format(CultureInfo.InvariantCulture, "securityNamespaces/{0}", securityNamespaceId);
			string requestWithRetry = GetRequestWithRetry(relativeUrl);
			if (requestWithRetry != null)
			{
				JsonObject jsonObject = new JsonObject(requestWithRetry);
				SecurityNamespaceModel value = JsonToSecurityNamespaceDataModel(jsonObject.GetChild("data"));
				_securityNamespaces.TryAdd(securityNamespaceId, value);
			}
		}
	}

	private async Task EnsureSecurityNamespaceLoadedAsync(Guid securityNamespaceId, CancellationToken cancellationToken)
	{
		if (!_securityNamespaces.ContainsKey(securityNamespaceId))
		{
			string relativeUrl = string.Format(CultureInfo.InvariantCulture, "securityNamespaces/{0}", securityNamespaceId);
			string text = await GetRequestWithRetryAsync(relativeUrl, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (text != null)
			{
				JsonObject jsonObject = new JsonObject(text);
				SecurityNamespaceModel value = JsonToSecurityNamespaceDataModel(jsonObject.GetChild("data"));
				_securityNamespaces.TryAdd(securityNamespaceId, value);
			}
		}
	}

	private void EnsurePermissionLoaded(Guid securityNamespaceId)
	{
		if (!_effectivePermissions.ContainsKey(securityNamespaceId))
		{
			string arg = ((_userOrRoleId != null) ? ("?userSid=" + _userOrRoleId) : string.Empty);
			string relativeUrl = string.Format(CultureInfo.InvariantCulture, "effectivePermissions/{0}{1}", securityNamespaceId, arg);
			string requestWithRetry = GetRequestWithRetry(relativeUrl);
			if (requestWithRetry != null)
			{
				JsonObject jsonObject = new JsonObject(requestWithRetry);
				EffectivePermissionsModel value = JsonToEffectivePermissionModel(jsonObject.GetChild("data"));
				_effectivePermissions.TryAdd(securityNamespaceId, value);
			}
		}
	}

	private async Task EnsurePermissionLoadedAsync(Guid securityNamespaceId, CancellationToken cancellationToken)
	{
		if (!_effectivePermissions.ContainsKey(securityNamespaceId))
		{
			string arg = ((_userOrRoleId != null) ? ("?userSid=" + _userOrRoleId) : string.Empty);
			string relativeUrl = string.Format(CultureInfo.InvariantCulture, "effectivePermissions/{0}{1}", securityNamespaceId, arg);
			string text = await GetRequestWithRetryAsync(relativeUrl, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
			if (text != null)
			{
				JsonObject jsonObject = new JsonObject(text);
				EffectivePermissionsModel value = JsonToEffectivePermissionModel(jsonObject.GetChild("data"));
				_effectivePermissions.TryAdd(securityNamespaceId, value);
			}
		}
	}

	private SecurityNamespaceModel JsonToSecurityNamespaceDataModel(JsonObject jsonObject)
	{
		JsonObject child = jsonObject.GetChild("securityActions");
		List<SecurityAction> list = new List<SecurityAction>();
		foreach (JsonObject child2 in child.GetChildren())
		{
			list.Add(new SecurityAction
			{
				displayName = child2.GetString("displayName"),
				name = child2.GetString("name"),
				visible = child2.GetBool("visible").Value,
				securityMask = child2.GetUlong("securityMask")
			});
		}
		return new SecurityNamespaceModel
		{
			name = jsonObject.GetString("name"),
			displayName = jsonObject.GetString("displayName"),
			id = jsonObject.GetString("id"),
			resourceType = jsonObject.GetString("resourceType"),
			structure = jsonObject.GetInteger("structure").Value,
			userDefined = jsonObject.GetBool("userDefined").Value,
			securityActions = list.ToArray()
		};
	}

	private EffectivePermissionsModel JsonToEffectivePermissionModel(JsonObject jsonObject)
	{
		JsonObject child = jsonObject.GetChild("objectPermissions");
		List<ObjectPermissionsModel> list = new List<ObjectPermissionsModel>();
		foreach (JsonObject child2 in child.GetChildren())
		{
			list.Add(new ObjectPermissionsModel
			{
				id = new Guid(child2.GetString("id")),
				mask = child2.GetUlong("mask")
			});
		}
		return new EffectivePermissionsModel
		{
			id = jsonObject.GetString("id"),
			isAdministrator = jsonObject.GetBool("isAdministrator").Value,
			objectPermissions = list.ToArray()
		};
	}
}
