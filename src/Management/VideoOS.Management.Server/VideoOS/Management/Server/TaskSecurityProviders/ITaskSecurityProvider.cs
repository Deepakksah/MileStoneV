using System.Collections.Generic;

namespace VideoOS.Management.Server.TaskSecurityProviders;

public interface ITaskSecurityProvider
{
	bool CanRead(IGeminiRequestContext requestContext, Task task);

	IEnumerable<Task> CanRead(IGeminiRequestContext requestContext, IEnumerable<Task> tasks);

	void CheckRead(IGeminiRequestContext requestContext, Task task);

	void CheckStop(IGeminiRequestContext requestContext, Task task);
}
