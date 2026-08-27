using System;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskMoveHardware")]
public class MoveHardwareTask : AsyncTask
{
	private MoveHardwareTaskResult successResult;

	public Guid HardwareId => GetNestedPropertyValue<Guid>("InputParameters", "hardwareId");

	public string HardwareName => GetNestedPropertyValue<string>("InputParameters", "hardwareName");

	public Guid SourceRecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "sourceRecorderId");

	public string SourceRecordingServerName => GetNestedPropertyValue<string>("InputParameters", "sourceRecorderName");

	public Guid DestinationRecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "destinationRecorderId");

	public string DestinationRecordingServerName => GetNestedPropertyValue<string>("InputParameters", "destinationRecorderName");

	public Guid DestinationStorageId => GetNestedPropertyValue<Guid>("InputParameters", "destinationStorageId");

	public string DestinationStorageName => GetNestedPropertyValue<string>("InputParameters", "destinationStorageName");

	public bool IgnoreSourceRecordingServer => GetNestedPropertyValue<int>("InputParameters", "ignoreSourceRecordingServer") != 0;

	public override string Name => TaskResources.TaskMoveHardware;

	public override string EntityName => HardwareName;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public MoveHardwareTaskResult SuccessResult
	{
		get
		{
			if (base.Status != AsyncTaskStatus.Succeeded)
			{
				throw new InvalidOperationException("Not possible to get success result if task status is not 'Succeeded'");
			}
			if (successResult != null)
			{
				return successResult;
			}
			MoveHardwareTaskResult moveHardwareTaskResult = null;
			try
			{
				moveHardwareTaskResult = MoveHardwareTaskResult.FromXml(base.StatusDetails);
			}
			catch (InvalidOperationException)
			{
			}
			foreach (TaskWarning warning in moveHardwareTaskResult.Warnings)
			{
				warning.Message = AsyncTask.ResolveLocalizedErrorMessage(warning.Message, warning.Code, warning.Arguments);
			}
			successResult = moveHardwareTaskResult;
			return successResult;
		}
	}

	public MoveHardwareTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static MoveHardwareTask StartNew(IVmoRepositories repositories, Guid hardwareId, Guid destinationRecordingServerId, Guid destinationStorageId, bool ignoreSourceRecordingServer)
	{
		MoveHardwareTask moveHardwareTask = new MoveHardwareTask(string.Empty, new VmoPropertyCollection(), repositories);
		moveHardwareTask.SetNestedPropertyValue("InputParameters", "hardwareId", hardwareId.ToString().ToUpperInvariant());
		moveHardwareTask.SetNestedPropertyValue("InputParameters", "destinationRecorderId", destinationRecordingServerId.ToString().ToUpperInvariant());
		moveHardwareTask.SetNestedPropertyValue("InputParameters", "destinationStorageId", destinationStorageId.ToString().ToUpperInvariant());
		moveHardwareTask.SetNestedPropertyValue("InputParameters", "ignoreSourceRecordingServer", ignoreSourceRecordingServer ? 1 : 0);
		moveHardwareTask.CreateObject(moveHardwareTask);
		return moveHardwareTask;
	}
}
