namespace VideoOS.Common.Integration.Threading;

public interface IWaitableResult
{
	IWorkItemResult GetWorkItemResult();

	IWorkItemResult<TResult> GetWorkItemResultT<TResult>();
}
