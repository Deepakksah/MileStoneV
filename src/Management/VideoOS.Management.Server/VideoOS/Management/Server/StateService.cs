using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Threading.Tasks;

namespace VideoOS.Management.Server;

[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class StateService : GeminiWebServiceBase, IStateService
{
	public int StartStateSession()
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("StartStateSession");
			EnterWebMethod(methodInformation);
			return SystemMonitorStateHandler.StartStateSession(base.RequestContext);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void StopStateSession(int sessionId)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("StopStateSession");
			methodInformation.AddParameter("sessionId", sessionId);
			EnterWebMethod(methodInformation);
			SystemMonitorStateHandler.StopStateSession(base.RequestContext, sessionId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public CurrentState[] SubscribeServerStates(int sessionId, int[] eventTypes, Guid[] itemGuids)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("SubscribeServerStates");
			methodInformation.AddParameter("sessionId", sessionId);
			methodInformation.AddArrayParameter("eventTypes", eventTypes);
			methodInformation.AddArrayParameter("itemGuids", itemGuids);
			EnterWebMethod(methodInformation);
			return SystemMonitorStateHandler.SubscribeServerStates(base.RequestContext, sessionId, eventTypes, itemGuids);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public CurrentState[] SubscribeCameraStates(int sessionId, int[] eventTypes, Guid[] itemGuids)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("SubscribeCameraStates");
			methodInformation.AddParameter("sessionId", sessionId);
			methodInformation.AddArrayParameter("eventTypes", eventTypes);
			methodInformation.AddArrayParameter("itemGuids", itemGuids);
			EnterWebMethod(methodInformation);
			return SystemMonitorStateHandler.SubscribeCameraStates(base.RequestContext, sessionId, eventTypes, itemGuids);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void UnsubscribeStates(int sessionId, int[] eventTypes, Guid[] itemGuids)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("UnsubscribeStates");
			methodInformation.AddParameter("sessionId", sessionId);
			methodInformation.AddArrayParameter("eventTypes", eventTypes);
			methodInformation.AddArrayParameter("itemGuids", itemGuids);
			EnterWebMethod(methodInformation);
			SystemMonitorStateHandler.UnsubscribeStates(base.RequestContext, sessionId, eventTypes, itemGuids);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Task<CurrentState[]> GetStates(int sessionId, int timeout)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetStates");
			methodInformation.AddParameter("sessionId", sessionId);
			methodInformation.AddParameter("timeout", timeout);
			EnterWebMethod(methodInformation);
			return SystemMonitorStateHandler.GetStates(base.RequestContext, sessionId, timeout);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public CurrentState[] GetCurrentServerStates(int[] eventTypes, Guid[] itemGuids)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetCurrentServerStates");
			methodInformation.AddArrayParameter("eventTypes", eventTypes);
			methodInformation.AddArrayParameter("itemGuids", itemGuids);
			EnterWebMethod(methodInformation);
			return SystemMonitorStateHandler.GetCurrentServerStates(base.RequestContext, eventTypes, itemGuids);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public CurrentState[] GetCurrentCameraStates(int[] eventTypes, Guid[] itemGuids)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetCurrentCameraStates");
			methodInformation.AddArrayParameter("eventTypes", eventTypes);
			methodInformation.AddArrayParameter("itemGuids", itemGuids);
			EnterWebMethod(methodInformation);
			return SystemMonitorStateHandler.GetCurrentCameraStates(base.RequestContext, eventTypes, itemGuids);
		}
		finally
		{
			LeaveWebMethod();
		}
	}
}
