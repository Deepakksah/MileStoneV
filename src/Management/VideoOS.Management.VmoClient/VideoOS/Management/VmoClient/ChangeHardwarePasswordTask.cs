using System;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskChangeHardwarePassword")]
public class ChangeHardwarePasswordTask : AsyncTask
{
	public string HardwareName => GetNestedPropertyValue<string>("InputParameters", "hardwareName");

	public override string Name => TaskResources.TaskChangeHardwarePassword;

	public override string EntityName => HardwareName;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public ChangeHardwarePasswordTaskResult SuccessResult
	{
		get
		{
			if (base.Status != AsyncTaskStatus.Succeeded)
			{
				throw new InvalidOperationException("Not possible to get success result if task status is not 'Succeeded'");
			}
			try
			{
				return ChangeHardwarePasswordTaskResult.FromXml(base.StatusDetails);
			}
			catch (InvalidOperationException)
			{
				return null;
			}
		}
	}

	public ChangeHardwarePasswordTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static ChangeHardwarePasswordTask StartNew(IVmoRepositories repositories, Guid hardwareId, string username, string password)
	{
		ChangeHardwarePasswordTask changeHardwarePasswordTask = new ChangeHardwarePasswordTask(string.Empty, new VmoPropertyCollection(), repositories);
		changeHardwarePasswordTask.SetNestedPropertyValue("InputParameters", "hardwareId", hardwareId.ToString().ToUpperInvariant());
		changeHardwarePasswordTask.SetNestedPropertyValue("InputParameters", "userName", username);
		changeHardwarePasswordTask.SetNestedPropertyValue("InputParameters", "password", password);
		changeHardwarePasswordTask.CreateObject(changeHardwarePasswordTask);
		return changeHardwarePasswordTask;
	}
}
