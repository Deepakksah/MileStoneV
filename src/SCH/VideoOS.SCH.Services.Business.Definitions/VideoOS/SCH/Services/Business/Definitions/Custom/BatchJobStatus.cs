using System;

namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class BatchJobStatus
{
	public string BatchJobID { get; set; }

	public int Status { get; set; }

	public int NoofFailedStarts { get; set; }

	public DateTime LastSuccess { get; set; }

	public string LastJobMessage { get; set; }

	public BatchJobStatus(string batchJobID, int status, int noofFailedStarts, DateTime lastSuccess, string lastJobMessage)
	{
		BatchJobID = batchJobID;
		Status = status;
		NoofFailedStarts = noofFailedStarts;
		LastSuccess = lastSuccess;
		LastJobMessage = lastJobMessage;
	}
}
