using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.TaskSecurityProviders;

public class TaskMoveHardwareSecurityProvider : ITaskSecurityProvider
{
	IEnumerable<Task> ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, IEnumerable<Task> tasks)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (Task task in tasks)
		{
			string value = task.TaskProperties.GetValue("hardwareId");
			if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", value))
			{
				continue;
			}
			string value2 = task.TaskProperties.GetValue("sourceRecorderId");
			if (securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", value2))
			{
				string value3 = task.TaskProperties.GetValue("destinationRecorderId");
				if (securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", value3))
				{
					yield return task;
				}
			}
		}
	}

	bool ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, Task task)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		string value = task.TaskProperties.GetValue("hardwareId");
		if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", value))
		{
			return false;
		}
		string value2 = task.TaskProperties.GetValue("sourceRecorderId");
		if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", value2))
		{
			return false;
		}
		string value3 = task.TaskProperties.GetValue("destinationRecorderId");
		if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", value3))
		{
			return false;
		}
		return true;
	}

	void ITaskSecurityProvider.CheckRead(IGeminiRequestContext requestContext, Task task)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		securityManagerService.CheckObjectPermission(objectId: task.TaskProperties.GetValue("hardwareId"), requestContext: requestContext, namespaceId: SecurityNamespaceIdentifier.Hardware, actionId: "GENERIC_READ");
		securityManagerService.CheckObjectPermission(objectId: task.TaskProperties.GetValue("sourceRecorderId"), requestContext: requestContext, namespaceId: SecurityNamespaceIdentifier.RecordingServer, actionId: "GENERIC_READ");
		securityManagerService.CheckObjectPermission(objectId: task.TaskProperties.GetValue("destinationRecorderId"), requestContext: requestContext, namespaceId: SecurityNamespaceIdentifier.RecordingServer, actionId: "GENERIC_READ");
	}

	void ITaskSecurityProvider.CheckStop(IGeminiRequestContext requestContext, Task task)
	{
		throw new NotSupportedException();
	}
}
