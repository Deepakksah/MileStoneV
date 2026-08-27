using System.Collections.Generic;
using VideoOS.Platform.AccessControl.Internal.InternalSerializationClasses;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

public class OperationableInstanceIdEnabledComparer : IEqualityComparer<OperationableInstanceConfigurationSerializeData.OperationableInstanceConfigurationData>
{
	public bool Equals(OperationableInstanceConfigurationSerializeData.OperationableInstanceConfigurationData x, OperationableInstanceConfigurationSerializeData.OperationableInstanceConfigurationData y)
	{
		if (x == y)
		{
			return true;
		}
		if (x == null || y == null)
		{
			return false;
		}
		if (x.AccessControlOperationableInstanceId == y.AccessControlOperationableInstanceId)
		{
			return x.Enabled == y.Enabled;
		}
		return false;
	}

	public int GetHashCode(OperationableInstanceConfigurationSerializeData.OperationableInstanceConfigurationData obj)
	{
		return obj?.AccessControlOperationableInstanceId.GetHashCode() ?? 0;
	}
}
