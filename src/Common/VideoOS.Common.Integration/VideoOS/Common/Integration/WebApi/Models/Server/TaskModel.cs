using System;
using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class TaskModel
{
	public long SequenceNumber { get; set; }

	[EnumDataType(typeof(ServerTaskTypeDto))]
	public ServerTaskTypeDto TaskType { get; set; }

	[EnumDataType(typeof(ServerTaskDiffGramTypeDto))]
	public ServerTaskDiffGramTypeDto TaskDiffGramType { get; set; }

	[Required]
	public string TaskXml { get; set; }

	public int PercentCompleted { get; set; }

	public string Details { get; set; }

	public string Status { get; set; }

	public TaskModel()
	{
	}

	public TaskModel(ServerTaskDto serverTaskDto)
	{
		if (serverTaskDto == null)
		{
			throw new ArgumentNullException("serverTaskDto");
		}
		Details = serverTaskDto.Details;
		PercentCompleted = serverTaskDto.PercentCompleted;
		SequenceNumber = serverTaskDto.SequenceNumber;
		Status = serverTaskDto.Status;
		TaskDiffGramType = serverTaskDto.TaskDiffGramType;
		TaskType = serverTaskDto.TaskType;
		TaskXml = serverTaskDto.TaskXml;
	}
}
