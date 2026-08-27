using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.TaskSecurityProviders;

public class TaskAddHardwareSecurityProvider : ITaskSecurityProvider
{
	bool ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, Task task)
	{
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(objectId: task.TaskProperties.GetValue("idRecorder"), requestContext: requestContext, namespaceId: SecurityNamespaceIdentifier.RecordingServer, actionId: "ADMIN_HARDWARE"))
		{
			return false;
		}
		return true;
	}

	IEnumerable<Task> ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, IEnumerable<Task> tasks)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (Task task in tasks)
		{
			string value = task.TaskProperties.GetValue("idRecorder");
			if (securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", value))
			{
				yield return task;
			}
		}
	}

	void ITaskSecurityProvider.CheckRead(IGeminiRequestContext requestContext, Task task)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(objectId: task.TaskProperties.GetValue("idRecorder"), requestContext: requestContext, namespaceId: SecurityNamespaceIdentifier.RecordingServer, actionId: "ADMIN_HARDWARE");
	}

	void ITaskSecurityProvider.CheckStop(IGeminiRequestContext requestContext, Task task)
	{
		throw new NotSupportedException();
	}
}
