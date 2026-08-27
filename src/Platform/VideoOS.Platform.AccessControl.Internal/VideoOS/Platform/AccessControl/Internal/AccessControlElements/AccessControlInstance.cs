using System;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public abstract class AccessControlInstance : AccessControlElement
{
	public AccessControlType AccessControlType => base.AccessControlSystemManager?.AccessControlSystemConfiguration.GetAccessControlType(this);

	public AccessControlInstance(Guid id, AccessControlSystemManager accessControlSystemManager)
		: base(id, accessControlSystemManager)
	{
	}
}
