using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.Management.Common;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public static class SystemMonitorStateHandler
{
	public static void AddState(IGeminiRequestContext requestContext, CounterStateChangeData state, List<SystemMonitorManager.EventSource> sources, SystemMonitorManager.RuleEventType type)
	{
		GeminiApplicationCore.SystemMonitorStateManager.AddStateToSessions(requestContext, state, sources, type);
	}

	public static int StartStateSession(GeminiRequestContext requestContext)
	{
		((IGeminiRequestContext)requestContext).GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.SystemMonitor, "GENERIC_READ", "\\");
		return GeminiApplicationCore.SystemMonitorStateManager.StartStateSession(requestContext);
	}

	public static void StopStateSession(IGeminiRequestContext requestContext, int sessionId)
	{
		GeminiApplicationCore.SystemMonitorStateManager.StopStateSession(requestContext, sessionId);
	}

	public static CurrentState[] SubscribeServerStates(IGeminiRequestContext requestContext, int sessionId, int[] eventTypes, Guid[] itemGuids)
	{
		return GeminiApplicationCore.SystemMonitorStateManager.SubscribeServerStates(requestContext, sessionId, eventTypes, itemGuids);
	}

	public static CurrentState[] SubscribeCameraStates(IGeminiRequestContext requestContext, int sessionId, int[] eventTypes, Guid[] itemGuids)
	{
		return GeminiApplicationCore.SystemMonitorStateManager.SubscribeCameraStates(requestContext, sessionId, eventTypes, itemGuids);
	}

	public static void UnsubscribeStates(IGeminiRequestContext requestContext, int sessionId, int[] eventTypes, Guid[] itemGuids)
	{
		GeminiApplicationCore.SystemMonitorStateManager.UnsubscribeStates(requestContext, sessionId, eventTypes, itemGuids);
	}

	public static async Task<CurrentState[]> GetStates(IGeminiRequestContext requestContext, int sessionId, int timeout)
	{
		return await GeminiApplicationCore.SystemMonitorStateManager.GetStates(requestContext, sessionId, timeout);
	}

	public static CurrentState[] GetCurrentServerStates(IGeminiRequestContext requestContext, int[] eventTypes, Guid[] itemGuids)
	{
		return SystemMonitorStateManager.GetCurrentServerStates(requestContext, eventTypes, itemGuids);
	}

	public static CurrentState[] GetCurrentCameraStates(IGeminiRequestContext requestContext, int[] eventTypes, Guid[] itemGuids)
	{
		return SystemMonitorStateManager.GetCurrentCameraStates(requestContext, eventTypes, itemGuids);
	}
}
