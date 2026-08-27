using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server.TaskSecurityProviders;

public class TaskAddIdpAdminSecurityProvider : ITaskSecurityProvider
{
	public IEnumerable<Task> CanRead(IGeminiRequestContext requestContext, IEnumerable<Task> tasks)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (Task task in tasks)
		{
			if (securityManagerService.HasApplicationPermission(requestContext, "GENERIC_READ"))
			{
				yield return task;
			}
		}
	}

	bool ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, Task task)
	{
		return requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ");
	}

	void ITaskSecurityProvider.CheckRead(IGeminiRequestContext requestContext, Task task)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
	}

	void ITaskSecurityProvider.CheckStop(IGeminiRequestContext requestContext, Task task)
	{
		throw new NotSupportedException();
	}
}
