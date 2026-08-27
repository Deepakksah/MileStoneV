using System;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public class AccessControlStateType : AccessControlType
{
	public override Guid TypeId => AccessControlElementTypeIds.AccessControlStateTypeId;

	public AccessControlStateType(Guid id, AccessControlSystemManager accessControlSystemManager)
		: base(id, accessControlSystemManager)
	{
	}
}
