using System;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public class AccessControlCommandType : AccessControlType
{
	public int Order => base.AccessControlSystemManager.AccessControlSystemConfiguration.GetOrder(this);

	public override Guid TypeId => AccessControlElementTypeIds.AccessControlCommandTypeId;

	public AccessControlCommandType(Guid id, AccessControlSystemManager accessControlSystemManager)
		: base(id, accessControlSystemManager)
	{
	}
}
