using System;

namespace VideoOS.Common.Integration.Threading;

public class WorkItemInfo
{
	public bool UseCallerCallContext { get; set; }

	public bool UseCallerHttpContext { get; set; }

	public bool DisposeOfStateObjects { get; set; }

	public CallToPostExecute CallToPostExecute { get; set; }

	public PostExecuteWorkItemCallback PostExecuteWorkItemCallback { get; set; }

	public WorkItemPriority WorkItemPriority { get; set; }

	public long Timeout { get; set; }

	public WorkItemInfo()
	{
		UseCallerCallContext = false;
		UseCallerHttpContext = false;
		DisposeOfStateObjects = false;
		CallToPostExecute = CallToPostExecute.Always;
		PostExecuteWorkItemCallback = SmartThreadPool.DefaultPostExecuteWorkItemCallback;
		WorkItemPriority = WorkItemPriority.Normal;
	}

	public WorkItemInfo(WorkItemInfo workItemInfo)
	{
		if (workItemInfo == null)
		{
			throw new ArgumentNullException("workItemInfo");
		}
		UseCallerCallContext = workItemInfo.UseCallerCallContext;
		UseCallerHttpContext = workItemInfo.UseCallerHttpContext;
		DisposeOfStateObjects = workItemInfo.DisposeOfStateObjects;
		CallToPostExecute = workItemInfo.CallToPostExecute;
		PostExecuteWorkItemCallback = workItemInfo.PostExecuteWorkItemCallback;
		WorkItemPriority = workItemInfo.WorkItemPriority;
		Timeout = workItemInfo.Timeout;
	}
}
