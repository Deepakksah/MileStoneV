using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.TaskSecurityProviders;

public class TaskFirmwareUpgradeSecurityProvider : ITaskSecurityProvider
{
	public bool CanRead(IGeminiRequestContext requestContext, Task task)
	{
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(objectId: task.TaskProperties.GetValue("hardwareId"), requestContext: requestContext, namespaceId: SecurityNamespaceIdentifier.Hardware, actionId: "GENERIC_WRITE"))
		{
			return false;
		}
		return true;
	}

	public IEnumerable<Task> CanRead(IGeminiRequestContext requestContext, IEnumerable<Task> tasks)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (Task task in tasks)
		{
			string value = task.TaskProperties.GetValue("hardwareId");
			if (securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", value))
			{
				yield return task;
			}
		}
	}

	public void CheckRead(IGeminiRequestContext requestContext, Task task)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(objectId: task.TaskProperties.GetValue("hardwareId"), requestContext: requestContext, namespaceId: SecurityNamespaceIdentifier.Hardware, actionId: "GENERIC_WRITE");
	}

	public void CheckStop(IGeminiRequestContext requestContext, Task task)
	{
		throw new NotSupportedException();
	}
}
