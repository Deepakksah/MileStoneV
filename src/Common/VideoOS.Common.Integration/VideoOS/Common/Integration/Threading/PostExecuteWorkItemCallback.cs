namespace VideoOS.Common.Integration.Threading;

public delegate void PostExecuteWorkItemCallback(IWorkItemResult workItemResult);
public delegate void PostExecuteWorkItemCallback<TResult>(IWorkItemResult<TResult> workItemResult);
