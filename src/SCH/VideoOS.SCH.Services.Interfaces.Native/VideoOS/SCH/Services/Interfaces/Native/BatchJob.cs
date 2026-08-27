using System;
using VideoOS.SCH.Services.Business.Components;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.SCH.Services.Business.Definitions.Custom.Contracts;
using VideoOS.SCH.Services.Business.Workflows;

namespace VideoOS.SCH.Services.Interfaces.Native;

public class BatchJob : ServiceChannelServiceNative
{
	public BatchJob()
	{
		InitiateServiceContext();
	}

	protected sealed override OperationStatus InitiateServiceContext()
	{
		return base.InitiateServiceContext();
	}

	public OperationStatus SessionCleanup()
	{
		try
		{
			if (!new VideoOS.SCH.Services.Business.Workflows.BatchJob().SessionCleanup())
			{
				return CreateContractStatus(ServiceContext.Current.Errors, OperationStatusEnum.Failed);
			}
			return CreateContractStatus(ServiceContext.Current.Errors, OperationStatusEnum.Completed);
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			return CreateContractStatus(ServiceContext.Current.Errors, OperationStatusEnum.Failed);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public OperationStatus MessageCleanup()
	{
		try
		{
			if (!new VideoOS.SCH.Services.Business.Workflows.BatchJob().MessageCleanup())
			{
				return CreateContractStatus(ServiceContext.Current.Errors, OperationStatusEnum.Failed);
			}
			return CreateContractStatus(ServiceContext.Current.Errors, OperationStatusEnum.Completed);
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			return CreateContractStatus(ServiceContext.Current.Errors, OperationStatusEnum.Failed);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public OperationStatus UpdateSessionMsgIndicator()
	{
		try
		{
			if (!new VideoOS.SCH.Services.Business.Workflows.BatchJob().SessionMsgIndicatorUpdate())
			{
				return CreateContractStatus(ServiceContext.Current.Errors, OperationStatusEnum.Failed);
			}
			return CreateContractStatus(ServiceContext.Current.Errors, OperationStatusEnum.Completed);
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			return CreateContractStatus(ServiceContext.Current.Errors, OperationStatusEnum.Failed);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}

	public OperationStatus GetBatchJobDefinitions(out int deleteExpiredSessionsJobIntervalSecs, out int deleteNonSessionMessagesIntervalSecs, out int serviceRegistrationIntervalSecs, out int sessionMsgIndicatorUpdateIntervalMs, out bool sessionMsgIndicatorUsed)
	{
		deleteExpiredSessionsJobIntervalSecs = 0;
		deleteNonSessionMessagesIntervalSecs = 0;
		serviceRegistrationIntervalSecs = 0;
		sessionMsgIndicatorUpdateIntervalMs = 0;
		sessionMsgIndicatorUsed = false;
		try
		{
			BatchJobDefinitions batchJobDefinitions = VideoOS.SCH.Services.Business.Components.BatchJob.GetBatchJobDefinitions();
			deleteExpiredSessionsJobIntervalSecs = batchJobDefinitions.DeleteExpiredSessionsJobIntervalSecs;
			deleteNonSessionMessagesIntervalSecs = batchJobDefinitions.DeleteNonSessionMessagesIntervalSecs;
			serviceRegistrationIntervalSecs = batchJobDefinitions.ServiceRegistrationIntervalSecs;
			sessionMsgIndicatorUpdateIntervalMs = batchJobDefinitions.SessionMsgIndicatorUpdateIntervalMs;
			sessionMsgIndicatorUsed = batchJobDefinitions.SessionMsgIndicatorUsed;
			return CreateContractStatus(ServiceContext.Current.Errors, OperationStatusEnum.Completed);
		}
		catch (Exception ex)
		{
			AddErrAndLogBusinessException(ex);
			return CreateContractStatus(ServiceContext.Current.Errors, OperationStatusEnum.Failed);
		}
		finally
		{
			ServiceContext.Delete();
		}
	}
}
