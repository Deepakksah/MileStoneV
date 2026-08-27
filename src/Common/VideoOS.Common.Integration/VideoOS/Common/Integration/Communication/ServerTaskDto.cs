using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public class ServerTaskDto
{
	public long SequenceNumber { get; set; }

	public ServerTaskTypeDto TaskType { get; set; }

	public ServerTaskDiffGramTypeDto TaskDiffGramType { get; set; }

	public string TaskXml { get; set; }

	public int PercentCompleted { get; set; }

	public string Details { get; set; }

	public string Status { get; set; }
}
