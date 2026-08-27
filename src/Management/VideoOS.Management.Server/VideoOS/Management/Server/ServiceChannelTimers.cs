using System;
using System.Threading;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;
using VideoOS.SCH.Services.Interfaces.Native;

namespace VideoOS.Management.Server;

public sealed class ServiceChannelTimers : IDisposable
{
	private Timer sessionCleanupTimer;

	private Timer messageCleanupTimer;

	private Timer sessionMsgIndicatorUpdateTimer;

	public void Initialize()
	{
		if (new BatchJob().GetBatchJobDefinitions(out var deleteExpiredSessionsJobIntervalSecs, out var deleteNonSessionMessagesIntervalSecs, out var _, out var sessionMsgIndicatorUpdateIntervalMs, out var sessionMsgIndicatorUsed).Status == VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed)
		{
			sessionCleanupTimer = new Timer(HandleSessionCleanupTimerCallback, null, deleteExpiredSessionsJobIntervalSecs * 1000, deleteExpiredSessionsJobIntervalSecs * 1000);
			messageCleanupTimer = new Timer(HandleMessageCleanupTimerCallback, null, deleteNonSessionMessagesIntervalSecs * 1000, deleteNonSessionMessagesIntervalSecs * 1000);
			if (sessionMsgIndicatorUsed && sessionMsgIndicatorUpdateIntervalMs > 0)
			{
				sessionMsgIndicatorUpdateTimer = new Timer(HandleSessionMsgIndicatorUpdateTimerCallback, null, sessionMsgIndicatorUpdateIntervalMs, sessionMsgIndicatorUpdateIntervalMs);
			}
		}
	}

	private void InternalDispose()
	{
		TextLoggerBase.Current.WriteTimeIntervalInMilliSec = 20;
		if (sessionCleanupTimer != null)
		{
			sessionCleanupTimer.Change(-1, -1);
			sessionCleanupTimer.Dispose();
		}
		if (messageCleanupTimer != null)
		{
			messageCleanupTimer.Change(-1, -1);
			messageCleanupTimer.Dispose();
		}
		if (sessionMsgIndicatorUpdateTimer != null)
		{
			sessionMsgIndicatorUpdateTimer.Change(-1, -1);
			sessionMsgIndicatorUpdateTimer.Dispose();
		}
	}

	private void HandleSessionCleanupTimerCallback(object data)
	{
		VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatus operationStatus = new BatchJob().SessionCleanup();
		if (operationStatus.Status == VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed && operationStatus.Errors != null)
		{
			_ = operationStatus.Errors.LongLength;
		}
	}

	private void HandleMessageCleanupTimerCallback(object data)
	{
		VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatus operationStatus = new BatchJob().MessageCleanup();
		if (operationStatus.Status == VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed && operationStatus.Errors != null)
		{
			_ = operationStatus.Errors.LongLength;
		}
	}

	private void HandleSessionMsgIndicatorUpdateTimerCallback(object data)
	{
		VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatus operationStatus = new BatchJob().UpdateSessionMsgIndicator();
		if (operationStatus.Status == VideoOS.SCH.Services.Business.Definitions.Custom.Contracts.OperationStatusEnum.Completed && operationStatus.Errors != null)
		{
			_ = operationStatus.Errors.LongLength;
		}
	}

	public void Dispose()
	{
		InternalDispose();
	}
}
