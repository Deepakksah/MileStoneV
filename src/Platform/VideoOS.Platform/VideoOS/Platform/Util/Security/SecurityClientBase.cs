using System;
using System.Collections.Generic;
using VideoOS.Platform.Util.ImageServer;

namespace VideoOS.Platform.Util.Security;

public abstract class SecurityClientBase
{
	public abstract string ServiceName { get; }

	public string ServerType { get; set; }

	public abstract void Initialize();

	public abstract void Flush();

	public abstract void Reload();

	public abstract void Logoff();

	public abstract void RegisterSecurityObject(string securityNamespace, string objecid, string parentObjectId);

	public abstract void UnregisterSecurityObject(string securityNamespace, string objecid);

	public abstract SecurityAction[] QueryObjectClassActions(string classId);

	public abstract void CheckPermission(Guid kind, string objectId, string actionId);

	public abstract AccessControlEntry[] QueryAccessControlList(Guid kind, string identity, string objectId, List<string> actionIdList);

	public abstract void AddAccessControlEntries(Guid kind, string objectId, AccessControlEntry[] entries);

	public abstract void RemoveAccessControlEntries(Guid kind, string objectId, AccessControlEntry[] entries);

	public abstract bool[] IsPermittedByActionList(Guid kind, string identity, string[] actionIdList, string objectId);

	public abstract bool[] HasPermissionByObjectList(Guid kind, string identity, string actionId, string[] objectIdList);

	public abstract UserIdentification GetUserIdentificationViaName(string name);

	public abstract UserIdentification GetUserIdentificationViaSid(string sid);

	public abstract bool IsMember(string sidIdentity, string groupId);

	public virtual bool HasOverallPermission(string namespaceId, string actionId)
	{
		return false;
	}
}
