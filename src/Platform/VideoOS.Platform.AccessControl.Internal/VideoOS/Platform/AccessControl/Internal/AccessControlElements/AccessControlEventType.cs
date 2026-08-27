using System;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public class AccessControlEventType : AccessControlType
{
	public override Guid TypeId => AccessControlElementTypeIds.AccessControlEventTypeId;

	public AccessControlEventType(Guid id, AccessControlSystemManager accessControlSystemManager)
		: base(id, accessControlSystemManager)
	{
	}
}
