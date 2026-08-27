using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.Server.TaskSecurityProviders;

public class TaskActivateLicenseSecurityProvider : ITaskSecurityProvider
{
	bool ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, Task task)
	{
		return requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ");
	}

	IEnumerable<Task> ITaskSecurityProvider.CanRead(IGeminiRequestContext requestContext, IEnumerable<Task> tasks)
	{
		if (requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			return tasks;
		}
		return Enumerable.Empty<Task>();
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
