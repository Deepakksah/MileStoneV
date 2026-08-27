using System;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public class AccessControlServerType : AccessControlOperationableType
{
	public override Guid TypeId => AccessControlElementTypeIds.AccessControlServerTypeId;

	public AccessControlServerType(Guid id, AccessControlSystemManager accessControlSystemManager)
		: base(id, accessControlSystemManager)
	{
	}
}
