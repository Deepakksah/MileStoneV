using System;
using System.Collections.Generic;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util.ImageServer;

namespace VideoOS.Platform.Util.Security;

public class SecurityClientXPCOWrapper : SecurityClientBase, IDisposable
{
	private SecurityClientXPCO _securityClient;

	private static Dictionary<string, Dictionary<string, Dictionary<string, bool>>> _identityActionObject = new Dictionary<string, Dictionary<string, Dictionary<string, bool>>>();

	private object _identityLock = new object();

	public override string ServiceName => _securityClient.ServiceName;

	public SecurityClientXPCOWrapper(LoginSettings loginSettings)
	{
		if (loginSettings.IdentityTokenCache != null)
		{
			_securityClient = new SecurityClientXPCOOAuth(loginSettings);
		}
		else
		{
			_securityClient = new SecurityClientXPCO(loginSettings);
		}
	}

	public override void Initialize()
	{
		_securityClient.Initialize();
	}

	public override void Flush()
	{
		_identityActionObject = new Dictionary<string, Dictionary<string, Dictionary<string, bool>>>();
	}

	public override void Reload()
	{
		Flush();
	}

	public override void Logoff()
	{
		_securityClient.Logoff();
	}

	public override SecurityAction[] QueryObjectClassActions(string classId)
	{
		return _securityClient.QueryObjectClassActions(classId);
	}

	public override void CheckPermission(Guid kind, string objectId, string actionId)
	{
		if (!_securityClient.HasPermission(kind, objectId, actionId))
		{
			throw new NotAuthorizedMIPException("Access not permitted");
		}
	}

	public override bool IsMember(string sidIdentity, string groupId)
	{
		return _securityClient.IsMember(sidIdentity, groupId);
	}

	public override AccessControlEntry[] QueryAccessControlList(Guid kind, string identity, string objectId, List<string> actionIdList)
	{
		string[] array = actionIdList.ToArray();
		bool[] array2 = _securityClient.HasPermissionByActionList(kind.ToString(), identity, array, objectId);
		List<AccessControlEntry> list = new List<AccessControlEntry>();
		for (int i = 0; i < array2.Length; i++)
		{
			if (array2[i])
			{
				list.Add(new AccessControlEntry
				{
					ActionId = array[i],
					Sid = identity
				});
			}
		}
		return list.ToArray();
	}

	public override void RegisterSecurityObject(string securityNamespace, string objectId, string parentObjectId)
	{
		_securityClient.RegisterObject(securityNamespace, objectId, parentObjectId);
	}

	public override void UnregisterSecurityObject(string securityNamespace, string objectId)
	{
		_securityClient.UnregisterObject(securityNamespace, objectId);
	}

	public override void AddAccessControlEntries(Guid namespaceId, string objectId, AccessControlEntry[] entries)
	{
		_securityClient.AddAccessControlEntries(namespaceId, objectId, entries);
	}

	public override void RemoveAccessControlEntries(Guid namespaceId, string objectId, AccessControlEntry[] entries)
	{
		_securityClient.RemoveAccessControlEntries(namespaceId, objectId, entries);
	}

	public override bool[] IsPermittedByActionList(Guid kind, string identity, string[] actionIdList, string objectId)
	{
		return _securityClient.HasPermissionByActionList(kind.ToString(), identity, actionIdList, objectId);
	}

	public override bool[] HasPermissionByObjectList(Guid kind, string identity, string actionId, string[] objectIdList)
	{
		try
		{
			lock (_identityLock)
			{
				if (!_identityActionObject.ContainsKey(identity))
				{
					_identityActionObject.Add(identity, new Dictionary<string, Dictionary<string, bool>>());
				}
				if (!_identityActionObject[identity].ContainsKey(actionId))
				{
					_identityActionObject[identity].Add(actionId, new Dictionary<string, bool>());
				}
			}
			bool[] array = new bool[objectIdList.Length];
			bool flag = false;
			bool flag2 = false;
			for (int i = 0; i < objectIdList.Length; i++)
			{
				flag = false;
				flag2 = false;
				lock (_identityLock)
				{
					if (_identityActionObject[identity][actionId].ContainsKey(objectIdList[i]))
					{
						flag = _identityActionObject[identity][actionId][objectIdList[i]];
						flag2 = true;
					}
				}
				if (!flag2)
				{
					try
					{
						flag = _securityClient.HasPermissionByObjectList(kind.ToString(), identity, actionId, new string[1] { objectIdList[i] })[0];
						lock (_identityLock)
						{
							_identityActionObject[identity][actionId][objectIdList[i]] = flag;
						}
					}
					catch (Exception)
					{
					}
				}
				array[i] = flag;
			}
			return array;
		}
		catch (Exception)
		{
			return new List<string>(objectIdList).ConvertAll((string x) => false).ToArray();
		}
	}

	public override bool HasOverallPermission(string namespaceId, string actionId)
	{
		bool result = false;
		try
		{
			result = _securityClient.HasPermission(namespaceId, actionId);
		}
		catch (Exception)
		{
		}
		return result;
	}

	public override UserIdentification GetUserIdentificationViaName(string userName)
	{
		Identity identity = _securityClient.ReadIdentityViaName(userName);
		if (identity == null)
		{
			return null;
		}
		return new UserIdentification(identity.AccountName, identity.Sid, "", identity.Deleted, identity.Description, identity.DisplayName, identity.DisplayName, identity.MemberOf, identity.Members, (UserIdType.UserIDType)identity.Type)
		{
			Domain = identity.Domain
		};
	}

	public override UserIdentification GetUserIdentificationViaSid(string userSid)
	{
		Identity identity = _securityClient.ReadIdentityViaSid(userSid);
		if (identity == null)
		{
			return null;
		}
		return new UserIdentification(identity.AccountName, identity.Sid, "", identity.Deleted, identity.Description, identity.DisplayName, identity.DisplayName, identity.MemberOf, identity.Members, (UserIdType.UserIDType)identity.Type)
		{
			Domain = identity.Domain
		};
	}

	protected void Dispose(bool disposing)
	{
		if (disposing && _securityClient != null)
		{
			_securityClient.Dispose();
			_securityClient = null;
		}
	}

	public void Dispose()
	{
	}
}
