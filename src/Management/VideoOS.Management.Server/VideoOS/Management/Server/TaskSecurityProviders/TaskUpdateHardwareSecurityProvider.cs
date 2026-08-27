using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.TaskSecurityProviders;

public class TaskUpdateHardwareSecurityProvider : ITaskSecurityProvider
{
	IEnumerable<Task> ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, IEnumerable<Task> tasks)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (Task task in tasks)
		{
			string value = task.TaskProperties.GetValue("recordingServerId");
			if (securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", value))
			{
				yield return task;
			}
		}
	}

	bool ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, Task task)
	{
		if (!requestContext.GetSecurityManagerService().HasObjectPermission(objectId: task.TaskProperties.GetValue("recordingServerId"), requestContext: requestContext, namespaceId: SecurityNamespaceIdentifier.RecordingServer, actionId: "ADMIN_HARDWARE"))
		{
			return false;
		}
		return true;
	}

	void ITaskSecurityProvider.CheckRead(IGeminiRequestContext requestContext, Task task)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(objectId: task.TaskProperties.GetValue("recordingServerId"), requestContext: requestContext, namespaceId: SecurityNamespaceIdentifier.RecordingServer, actionId: "ADMIN_HARDWARE");
	}

	void ITaskSecurityProvider.CheckStop(IGeminiRequestContext requestContext, Task task)
	{
		throw new NotSupportedException();
	}
}
