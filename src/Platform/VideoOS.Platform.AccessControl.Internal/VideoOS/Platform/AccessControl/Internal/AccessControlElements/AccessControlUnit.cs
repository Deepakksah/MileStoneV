using System;
using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public class AccessControlUnit : AccessControlOperationableInstance
{
	public AccessControlUnit Parent => base.AccessControlSystemManager.AccessControlSystemConfiguration.GetParentAccessControlUnit(this);

	public IEnumerable<AccessControlUnit> Children => base.AccessControlSystemManager.AccessControlSystemConfiguration.GetChildAccessControlUnits(this);

	public PositioningInformation Position
	{
		get
		{
			return base.AccessControlSystemManager.AccessControlSystemConfiguration.GetAccessControlUnitPosition(this);
		}
		set
		{
			if (value != null && value != Position)
			{
				base.AccessControlSystemManager.AccessControlSystemConfiguration.SetAccessControlUnitPosition(this, value);
			}
		}
	}

	public override Guid TypeId => AccessControlElementTypeIds.AccessControlUnitId;

	public AccessControlUnit(Guid id, AccessControlSystemManager accessControlSystemManager)
		: base(id, accessControlSystemManager)
	{
	}
}
