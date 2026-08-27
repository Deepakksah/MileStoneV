using VideoOS.Platform.AccessControl.Internal.AccessControlElements;

namespace VideoOS.Platform.AccessControl.Internal.Data.State;

public class AccessControlStateChange
{
	public AccessControlOperationableInstance AccessControlOperationableInstance { get; private set; }

	public AccessControlState AccessControlState { get; private set; }

	public AccessControlStateChange(AccessControlOperationableInstance accessControlOperationableInstance, AccessControlState accessControlState)
	{
		AccessControlOperationableInstance = accessControlOperationableInstance;
		AccessControlState = accessControlState;
	}
}
