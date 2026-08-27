using System;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public class AccessControlUnitType : AccessControlOperationableType
{
	public override Guid TypeId => AccessControlElementTypeIds.AccessControlUnitTypeId;

	public AccessControlUnitType(Guid id, AccessControlSystemManager accessControlSystemManager)
		: base(id, accessControlSystemManager)
	{
	}
}
