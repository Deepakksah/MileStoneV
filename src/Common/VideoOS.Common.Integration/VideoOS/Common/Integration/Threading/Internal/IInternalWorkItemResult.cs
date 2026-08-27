namespace VideoOS.Common.Integration.Threading.Internal;

public interface IInternalWorkItemResult
{
	event WorkItemStateCallback OnWorkItemStarted;

	event WorkItemStateCallback OnWorkItemCompleted;
}
