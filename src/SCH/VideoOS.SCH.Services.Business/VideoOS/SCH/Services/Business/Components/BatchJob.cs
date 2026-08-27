using System;
using System.Reflection;
using VideoOS.SCH.Base;
using VideoOS.SCH.Services.Business.Definitions.Custom;
using VideoOS.SCH.Services.DataAccess;
using VideoOS.SCH.Services.DataAccess.Generic;

namespace VideoOS.SCH.Services.Business.Components;

public class BatchJob
{
	public enum JobStatus
	{
		Unknown,
		Inactive,
		Active
	}

	public static BatchJobDefinitions GetBatchJobDefinitions()
	{
		ServiceContext.Current.LogToServerLog(0, "Starting to execute BC.GetBatchJobDefinitions() [assembly version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + "]", LogType.Information);
		return new BatchJobDefinitions
		{
			DeleteExpiredSessionsJobIntervalSecs = ConfigSettings.DeleteExpiredSessionsJobIntervalSecs,
			DeleteNonSessionMessagesIntervalSecs = ConfigSettings.DeleteNonSessionMessagesIntervalSecs,
			ServiceRegistrationIntervalSecs = ConfigSettings.ServiceRegistrationIntervalSecs,
			SessionMsgIndicatorUpdateIntervalMs = ConfigSettings.SessionMsgIndicatorUpdateIntervalMs,
			SessionMsgIndicatorUsed = ConfigSettings.SessionMsgIndicatorUsed
		};
	}

	public static bool SetLastJobMessage(string batchJobID, string lastJobMessage, bool createJobIfNotExist)
	{
		IBatchJobDataMgr batchJobDataMgr = GenericDataMgr.GetBatchJobDataMgr();
		if (createJobIfNotExist && !batchJobDataMgr.JobStatusExist(batchJobID))
		{
			return batchJobDataMgr.CreateJobStatus(new BatchJobStatus(batchJobID, 1, 0, new DateTime(1900, 1, 1, 0, 0, 0), lastJobMessage));
		}
		return batchJobDataMgr.SetLastJobMessage(batchJobID, lastJobMessage);
	}

	public static bool SetLastJobMessage(string batchJobID, string lastJobMessage)
	{
		return SetLastJobMessage(batchJobID, lastJobMessage, createJobIfNotExist: true);
	}

	public static bool JobStatusExist(string batchJobID)
	{
		return GenericDataMgr.GetBatchJobDataMgr().JobStatusExist(batchJobID);
	}

	public static bool CreateJobStatus(BatchJobStatus batchJobStatus)
	{
		return GenericDataMgr.GetBatchJobDataMgr().CreateJobStatus(batchJobStatus);
	}

	public static bool UpdateJobStatus(string jobIDOfUpdatedRecord, int statusOfUpdatedRecord, int statusToUpdate)
	{
		return GenericDataMgr.GetBatchJobDataMgr().UpdateJobStatus(jobIDOfUpdatedRecord, statusOfUpdatedRecord, statusToUpdate);
	}

	public static bool UpdateJobStatus(string jobIDOfUpdatedRecord, int statusToUpdate)
	{
		return GenericDataMgr.GetBatchJobDataMgr().UpdateJobStatus(jobIDOfUpdatedRecord, statusToUpdate);
	}

	public static bool IncreaseNoofFailedStarts(string batchJobID)
	{
		return GenericDataMgr.GetBatchJobDataMgr().IncreaseNoofFailedStarts(batchJobID);
	}

	public static bool SetLastSuccessInfo(string batchJobID, DateTime lastSuccess, string lastJobMessage)
	{
		return GenericDataMgr.GetBatchJobDataMgr().SetLastSuccessInfo(batchJobID, lastSuccess, lastJobMessage);
	}
}
