using System;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public class AccessControlServer : AccessControlOperationableInstance
{
	public override Guid TypeId => AccessControlElementTypeIds.AccessControlServerId;

	public AccessControlServer(Guid id, AccessControlSystemManager accessControlSystemManager)
		: base(id, accessControlSystemManager)
	{
	}
}
