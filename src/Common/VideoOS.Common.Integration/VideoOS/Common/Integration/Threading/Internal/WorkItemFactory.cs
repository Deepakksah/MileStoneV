using System;

namespace VideoOS.Common.Integration.Threading.Internal;

public class WorkItemFactory
{
	public static WorkItem CreateWorkItem(IWorkItemsGroup workItemsGroup, WIGStartInfo wigStartInfo, WorkItemCallback callback)
	{
		return CreateWorkItem(workItemsGroup, wigStartInfo, callback, null);
	}

	public static WorkItem CreateWorkItem(IWorkItemsGroup workItemsGroup, WIGStartInfo wigStartInfo, WorkItemCallback callback, WorkItemPriority workItemPriority)
	{
		return CreateWorkItem(workItemsGroup, wigStartInfo, callback, null, workItemPriority);
	}

	public static WorkItem CreateWorkItem(IWorkItemsGroup workItemsGroup, WIGStartInfo wigStartInfo, WorkItemInfo workItemInfo, WorkItemCallback callback)
	{
		return CreateWorkItem(workItemsGroup, wigStartInfo, workItemInfo, callback, null);
	}

	public static WorkItem CreateWorkItem(IWorkItemsGroup workItemsGroup, WIGStartInfo wigStartInfo, WorkItemCallback callback, object state)
	{
		if (wigStartInfo == null)
		{
			throw new ArgumentNullException("wigStartInfo");
		}
		ValidateCallback(callback);
		WorkItemInfo workItemInfo = new WorkItemInfo();
		workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
		workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
		workItemInfo.PostExecuteWorkItemCallback = wigStartInfo.PostExecuteWorkItemCallback;
		workItemInfo.CallToPostExecute = wigStartInfo.CallToPostExecute;
		workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
		workItemInfo.WorkItemPriority = wigStartInfo.WorkItemPriority;
		return new WorkItem(workItemsGroup, workItemInfo, callback, state);
	}

	public static WorkItem CreateWorkItem(IWorkItemsGroup workItemsGroup, WIGStartInfo wigStartInfo, WorkItemCallback callback, object state, WorkItemPriority workItemPriority)
	{
		if (wigStartInfo == null)
		{
			throw new ArgumentNullException("wigStartInfo");
		}
		ValidateCallback(callback);
		WorkItemInfo workItemInfo = new WorkItemInfo();
		workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
		workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
		workItemInfo.PostExecuteWorkItemCallback = wigStartInfo.PostExecuteWorkItemCallback;
		workItemInfo.CallToPostExecute = wigStartInfo.CallToPostExecute;
		workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
		workItemInfo.WorkItemPriority = workItemPriority;
		return new WorkItem(workItemsGroup, workItemInfo, callback, state);
	}

	public static WorkItem CreateWorkItem(IWorkItemsGroup workItemsGroup, WIGStartInfo wigStartInfo, WorkItemInfo workItemInfo, WorkItemCallback callback, object state)
	{
		if (workItemInfo == null)
		{
			throw new ArgumentNullException("workItemInfo");
		}
		ValidateCallback(callback);
		ValidateCallback(workItemInfo.PostExecuteWorkItemCallback);
		return new WorkItem(workItemsGroup, new WorkItemInfo(workItemInfo), callback, state);
	}

	public static WorkItem CreateWorkItem(IWorkItemsGroup workItemsGroup, WIGStartInfo wigStartInfo, WorkItemCallback callback, object state, PostExecuteWorkItemCallback postExecuteWorkItemCallback)
	{
		if (wigStartInfo == null)
		{
			throw new ArgumentNullException("wigStartInfo");
		}
		ValidateCallback(callback);
		ValidateCallback(postExecuteWorkItemCallback);
		WorkItemInfo workItemInfo = new WorkItemInfo();
		workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
		workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
		workItemInfo.PostExecuteWorkItemCallback = postExecuteWorkItemCallback;
		workItemInfo.CallToPostExecute = wigStartInfo.CallToPostExecute;
		workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
		workItemInfo.WorkItemPriority = wigStartInfo.WorkItemPriority;
		return new WorkItem(workItemsGroup, workItemInfo, callback, state);
	}

	public static WorkItem CreateWorkItem(IWorkItemsGroup workItemsGroup, WIGStartInfo wigStartInfo, WorkItemCallback callback, object state, PostExecuteWorkItemCallback postExecuteWorkItemCallback, WorkItemPriority workItemPriority)
	{
		if (wigStartInfo == null)
		{
			throw new ArgumentNullException("wigStartInfo");
		}
		ValidateCallback(callback);
		ValidateCallback(postExecuteWorkItemCallback);
		WorkItemInfo workItemInfo = new WorkItemInfo();
		workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
		workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
		workItemInfo.PostExecuteWorkItemCallback = postExecuteWorkItemCallback;
		workItemInfo.CallToPostExecute = wigStartInfo.CallToPostExecute;
		workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
		workItemInfo.WorkItemPriority = workItemPriority;
		return new WorkItem(workItemsGroup, workItemInfo, callback, state);
	}

	public static WorkItem CreateWorkItem(IWorkItemsGroup workItemsGroup, WIGStartInfo wigStartInfo, WorkItemCallback callback, object state, PostExecuteWorkItemCallback postExecuteWorkItemCallback, CallToPostExecute callToPostExecute)
	{
		if (wigStartInfo == null)
		{
			throw new ArgumentNullException("wigStartInfo");
		}
		ValidateCallback(callback);
		ValidateCallback(postExecuteWorkItemCallback);
		WorkItemInfo workItemInfo = new WorkItemInfo();
		workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
		workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
		workItemInfo.PostExecuteWorkItemCallback = postExecuteWorkItemCallback;
		workItemInfo.CallToPostExecute = callToPostExecute;
		workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
		workItemInfo.WorkItemPriority = wigStartInfo.WorkItemPriority;
		return new WorkItem(workItemsGroup, workItemInfo, callback, state);
	}

	public static WorkItem CreateWorkItem(IWorkItemsGroup workItemsGroup, WIGStartInfo wigStartInfo, WorkItemCallback callback, object state, PostExecuteWorkItemCallback postExecuteWorkItemCallback, CallToPostExecute callToPostExecute, WorkItemPriority workItemPriority)
	{
		if (wigStartInfo == null)
		{
			throw new ArgumentNullException("wigStartInfo");
		}
		ValidateCallback(callback);
		ValidateCallback(postExecuteWorkItemCallback);
		WorkItemInfo workItemInfo = new WorkItemInfo();
		workItemInfo.UseCallerCallContext = wigStartInfo.UseCallerCallContext;
		workItemInfo.UseCallerHttpContext = wigStartInfo.UseCallerHttpContext;
		workItemInfo.PostExecuteWorkItemCallback = postExecuteWorkItemCallback;
		workItemInfo.CallToPostExecute = callToPostExecute;
		workItemInfo.WorkItemPriority = workItemPriority;
		workItemInfo.DisposeOfStateObjects = wigStartInfo.DisposeOfStateObjects;
		return new WorkItem(workItemsGroup, workItemInfo, callback, state);
	}

	private static void ValidateCallback(Delegate callback)
	{
		if ((object)callback != null && callback.GetInvocationList().Length > 1)
		{
			throw new NotSupportedException();
		}
	}
}
