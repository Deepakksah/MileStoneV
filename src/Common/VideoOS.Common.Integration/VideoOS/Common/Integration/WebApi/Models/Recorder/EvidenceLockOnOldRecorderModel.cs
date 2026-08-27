using System;
using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Command;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class EvidenceLockOnOldRecorderModel : CommandModelBase
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
	public Guid IDEdgeTrack { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }

	public bool UseDeviceTrack { get; set; }

	public bool UseEdgeTrack { get; set; }

	public static EvidenceLockOnOldRecorderModel FromCommand(EvidenceLockOnOldRecorderCommand command)
	{
		if (command == null)
		{
			throw new ArgumentNullException("command");
		}
		return new EvidenceLockOnOldRecorderModel
		{
			ServerVersion = command.ServerVersion.ToString(),
			Operation = ToModel(command.Operation),
			IDMarkedData = command.IDMarkedData,
			IDDevice = command.IDDevice,
			IDEdgeTrack = command.IDEdgeTrack,
			StartTime = command.StartTime,
			EndTime = command.EndTime,
			UseDeviceTrack = command.UseDeviceTrack,
			UseEdgeTrack = command.UseEdgeTrack
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
