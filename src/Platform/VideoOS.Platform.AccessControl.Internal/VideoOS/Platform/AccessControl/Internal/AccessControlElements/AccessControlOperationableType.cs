using System;
using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public abstract class AccessControlOperationableType : AccessControlType
{
	public IEnumerable<AccessControlCommandType> AccessControlCommandTypes => base.AccessControlSystemManager.AccessControlSystemConfiguration.GetAccessControlCommandTypes(this);

	public IEnumerable<AccessControlEventType> AccessControlEventTypes => base.AccessControlSystemManager.AccessControlSystemConfiguration.GetAccessControlEventTypes(this);

	public IEnumerable<AccessControlStateType> AccessControlStateTypes => base.AccessControlSystemManager.AccessControlSystemConfiguration.GetAccessControlStateTypes(this);

	protected AccessControlOperationableType(Guid id, AccessControlSystemManager accessControlSystemManager)
		: base(id, accessControlSystemManager)
	{
	}
}
