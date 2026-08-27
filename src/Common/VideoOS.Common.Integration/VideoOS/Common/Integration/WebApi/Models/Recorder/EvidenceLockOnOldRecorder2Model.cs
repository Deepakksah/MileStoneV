using System;
using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class EvidenceLockOnOldRecorder2Model : CommandModelBase
{
	public enum EvidenceLockOperationModel
	{
		Add,
		Edit,
		Delete
	}

	[Required]
	public string ServerVersion { get; set; }

	[Required]
	public EvidenceLockOperationModel Operation { get; set; }

	[RequiredNonemptyGuid]
	public Guid IDMarkedData { get; set; }

	[RequiredNonemptyGuid]
	public Guid IDDevice { get; set; }

	[RequiredNonemptyGuid]
	public Guid IDTrack { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }

	public static EvidenceLockOnOldRecorder2Model FromCommand(EvidenceLockOnOldRecorderCommand2 command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		return new EvidenceLockOnOldRecorder2Model
		{
			ServerVersion = command.ServerVersion.ToString(),
			Operation = ToModel(command.Operation),
			IDMarkedData = command.IDMarkedData,
			IDDevice = command.IDDevice,
			IDTrack = command.IDTrack,
			StartTime = command.StartTime,
			EndTime = command.EndTime
		};
	}

	public EvidenceLockOnOldRecorderCommand2 ToCommand()
	{
		return new EvidenceLockOnOldRecorderCommand2
		{
			ServerVersion = new Version(ServerVersion),
			Operation = ToOperation(Operation),
			IDMarkedData = IDMarkedData,
			IDDevice = IDDevice,
			IDTrack = IDTrack,
			StartTime = StartTime,
			EndTime = EndTime
		};
	}

	private static EvidenceLockOperation ToOperation(EvidenceLockOperationModel value)
	{
		return value switch
		{
			EvidenceLockOperationModel.Add => EvidenceLockOperation.Add, 
			EvidenceLockOperationModel.Edit => EvidenceLockOperation.Edit, 
			EvidenceLockOperationModel.Delete => EvidenceLockOperation.Delete, 
			_ => throw new NotSupportedException(), 
		};
	}

	private static EvidenceLockOperationModel ToModel(EvidenceLockOperation value)
	{
		return value switch
		{
			EvidenceLockOperation.Add => EvidenceLockOperationModel.Add, 
			EvidenceLockOperation.Edit => EvidenceLockOperationModel.Edit, 
			EvidenceLockOperation.Delete => EvidenceLockOperationModel.Delete, 
			_ => throw new NotSupportedException(), 
		};
	}
}
