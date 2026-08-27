using System;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskFirmwareUpgrade")]
public class FirmwareUpgradeTask : AsyncTask
{
	public string HardwareName => GetNestedPropertyValue<string>("InputParameters", "hardwareName");

	public override string Name => TaskResources.TaskFirmwareUpgrade;

	public override string EntityName => HardwareName;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public FirmwareUpgradeTaskResult SuccessResult
	{
		get
		{
			if (base.Status != AsyncTaskStatus.Succeeded)
			{
				throw new InvalidOperationException("Not possible to get success result if task status is not 'Succeeded'");
			}
			try
			{
				return FirmwareUpgradeTaskResult.FromXml(base.StatusDetails);
			}
			catch (InvalidOperationException)
			{
				return null;
			}
		}
	}

	public FirmwareUpgradeTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static FirmwareUpgradeTask StartNew(IVmoRepositories repositories, Guid hardwareId, string fileId)
	{
		FirmwareUpgradeTask firmwareUpgradeTask = new FirmwareUpgradeTask(string.Empty, new VmoPropertyCollection(), repositories);
		firmwareUpgradeTask.SetNestedPropertyValue("InputParameters", "hardwareId", hardwareId.ToString().ToUpperInvariant());
		firmwareUpgradeTask.SetNestedPropertyValue("InputParameters", "fileId", fileId);
		firmwareUpgradeTask.CreateObject(firmwareUpgradeTask);
		return firmwareUpgradeTask;
	}
}
