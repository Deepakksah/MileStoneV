using System;

namespace VideoOS.Platform.AccessControl.Plugin;

public abstract class ACCommandManager
{
	public virtual ACCommandResult ExecuteCommand(string operationableInstanceId, string commandTypeId)
	{
		throw new NotImplementedException();
	}

	public virtual ACCommandResult ExecuteCommand(string operationableInstanceId, string commandTypeId, string vmsUsername)
	{
		return ExecuteCommand(operationableInstanceId, commandTypeId);
	}

	public virtual ACCommandResult ExecuteCommand(string operationableInstanceId, string commandTypeId, string username, string password)
	{
		throw new NotImplementedException();
	}

	public virtual ACCommandResult ExecuteCommand(string operationableInstanceId, string commandTypeId, string username, string password, string vmsUsername)
	{
		return ExecuteCommand(operationableInstanceId, commandTypeId, username, password);
	}
}
