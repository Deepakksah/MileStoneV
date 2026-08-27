using System;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskUpdateHardware")]
public class UpdateHardwareTask : AsyncTask
{
	private UpdateHardwareTaskResult successResult;

	public Guid HardwareId => GetNestedPropertyValue<Guid>("InputParameters", "hardwareId");

	public string HardwareName => GetNestedPropertyValue<string>("InputParameters", "hardwareName");

	public override string Name => TaskResources.TaskUpdateHardware;

	public override string EntityName => HardwareName;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public UpdateHardwareTaskResult SuccessResult
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
			UpdateHardwareTaskResult updateHardwareTaskResult = null;
			try
			{
				updateHardwareTaskResult = UpdateHardwareTaskResult.FromXml(base.StatusDetails);
			}
			catch (InvalidOperationException)
			{
			}
			successResult = updateHardwareTaskResult;
			return successResult;
		}
	}

	public UpdateHardwareTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static UpdateHardwareTask StartNew(IVmoRepositories repositories, Guid hardwareId, string sessionDataId)
	{
		UpdateHardwareTask updateHardwareTask = new UpdateHardwareTask(string.Empty, new VmoPropertyCollection(), repositories);
		updateHardwareTask.SetNestedPropertyValue("InputParameters", "hardwareId", hardwareId.ToString().ToUpperInvariant());
		updateHardwareTask.SetNestedPropertyValue("InputParameters", "sessionDataId", sessionDataId);
		updateHardwareTask.CreateObject(updateHardwareTask);
		return updateHardwareTask;
	}
}
