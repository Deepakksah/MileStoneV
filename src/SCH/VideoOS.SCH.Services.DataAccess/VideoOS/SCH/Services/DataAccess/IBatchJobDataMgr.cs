using System;
using System.Collections.Generic;
using VideoOS.SCH.Services.Business.Definitions.Custom;

namespace VideoOS.SCH.Services.DataAccess;

public interface IBatchJobDataMgr
{
	bool CreateJobStatus(BatchJobStatus batchJobStatus);

	List<BatchJobStatus> GetBatchJobStatus(string batchJobID);

	bool UpdateJobStatus(BatchJobStatus batchJobStatus);

	bool UpdateJobStatus(string jobIDOfUpdatedRecord, int statusOfUpdatedRecord, int statusToUpdate);

	bool UpdateJobStatus(string jobIDOfUpdatedRecord, int statusToUpdate);

	bool SetLastSuccessInfo(string batchJobID, DateTime lastSuccess, string lastJobMessage);

	bool SetLastJobMessage(string batchJobID, string lastJobMessage);

	bool DeleteJobStatus(string batchJobID);

	bool JobStatusExist(string batchJobID);

	bool IncreaseNoofFailedStarts(string batchJobID);
}
