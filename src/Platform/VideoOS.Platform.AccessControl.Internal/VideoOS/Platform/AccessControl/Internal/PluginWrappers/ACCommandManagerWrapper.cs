using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Internal.PluginWrappers;

public class ACCommandManagerWrapper
{
	private ACCommandManager _acCommandManager;

	public ACCommandManagerWrapper(ACCommandManager acCommandManager)
	{
		_acCommandManager = acCommandManager;
	}

	public ACCommandResult ExecuteCommand(string operationableInstanceId, string commandTypeId, string vmsUsername)
	{
		return ACPluginWrapper.TryCallPlugin(() => _acCommandManager.ExecuteCommand(operationableInstanceId, commandTypeId, vmsUsername), _acCommandManager.GetType(), "ExecuteCommand()", new ACCommandResult(executed: false, "unknown error"));
	}

	public ACCommandResult ExecuteCommand(string operationableInstanceId, string commandTypeId, string username, string password, string vmsUsername)
	{
		return ACPluginWrapper.TryCallPlugin(() => _acCommandManager.ExecuteCommand(operationableInstanceId, commandTypeId, username, password, vmsUsername), _acCommandManager.GetType(), "ExecuteCommand(username, password)", new ACCommandResult(executed: false, "unknown error"));
	}
}
