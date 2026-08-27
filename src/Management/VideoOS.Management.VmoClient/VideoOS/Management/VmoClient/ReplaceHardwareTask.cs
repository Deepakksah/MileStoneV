using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskReplaceHardware")]
public class ReplaceHardwareTask : AsyncTask
{
	private ReplaceHardwareTaskResult successResult;

	public Guid HardwareId => GetNestedPropertyValue<Guid>("InputParameters", "hardwareId");

	public string HardwareName => GetNestedPropertyValue<string>("InputParameters", "hardwareName");

	public override string Name => TaskResources.TaskReplaceHardware;

	public override string EntityName => HardwareName;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public ReplaceHardwareTaskResult SuccessResult
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
			ReplaceHardwareTaskResult replaceHardwareTaskResult = null;
			try
			{
				replaceHardwareTaskResult = ReplaceHardwareTaskResult.FromXml(base.StatusDetails);
			}
			catch (InvalidOperationException)
			{
			}
			successResult = replaceHardwareTaskResult;
			return successResult;
		}
	}

	public ReplaceHardwareTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static ReplaceHardwareTask StartNew(IVmoRepositories repositories, Guid hardwareId, string hardwareAddress, ushort port, bool useHttps, ushort httpsPort, string userName, string password, int driverNumber, bool allowDeletingDisabledDevices)
	{
		ReplaceHardwareTask replaceHardwareTask = new ReplaceHardwareTask(string.Empty, new VmoPropertyCollection(), repositories);
		replaceHardwareTask.SetNestedPropertyValue("InputParameters", "hardwareId", hardwareId.ToString().ToUpperInvariant());
		replaceHardwareTask.SetNestedPropertyValue("InputParameters", "hardwareAddress", hardwareAddress);
		replaceHardwareTask.SetNestedPropertyValue("InputParameters", "port", (int)port);
		replaceHardwareTask.SetNestedPropertyValue("InputParameters", "useHttps", useHttps);
		replaceHardwareTask.SetNestedPropertyValue("InputParameters", "httpsPort", (int)httpsPort);
		replaceHardwareTask.SetNestedPropertyValue("InputParameters", "userName", userName);
		replaceHardwareTask.SetNestedPropertyValue("InputParameters", "password", password);
		replaceHardwareTask.SetNestedPropertyValue("InputParameters", "driverNumber", driverNumber.ToString(CultureInfo.InvariantCulture));
		replaceHardwareTask.SetNestedPropertyValue("InputParameters", "allowDeletingDisabledDevices", allowDeletingDisabledDevices);
		replaceHardwareTask.CreateObject(replaceHardwareTask);
		return replaceHardwareTask;
	}
}
