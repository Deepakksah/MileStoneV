using System;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskAddHardware")]
public class AddHardwareTask : AsyncTask
{
	public Guid RecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "idRecorder");

	public string RecordingServerName => GetNestedPropertyValue<string>("InputParameters", "recordingServerName");

	public Uri Address => new Uri(GetNestedPropertyValue<string>("InputParameters", "address"));

	public string UserName => GetNestedPropertyValue<string>("InputParameters", "userName");

	public int DriverNumber => GetNestedPropertyValue<int>("InputParameters", "driverNumber");

	public int DefinitionXml => GetNestedPropertyValue<int>("InputParameters", "definitionXml");

	public override string Name => TaskResources.TaskAddHardware;

	public override string EntityName => RecordingServerName;

	public TaskError ErrorResult => base.ErrorResultInstance;

	public AddHardwareTaskResult SuccessResult
	{
		get
		{
			if (base.Status != AsyncTaskStatus.Succeeded)
			{
				throw new InvalidOperationException("Not possible to get success result if task status is not 'Succeeded'");
			}
			try
			{
				return AddHardwareTaskResult.FromXml(base.StatusDetails);
			}
			catch (InvalidOperationException)
			{
				return null;
			}
		}
	}

	public AddHardwareTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static AddHardwareTask StartNew(IVmoRepositories repositories, Guid recordingServerId, Uri address, string userName, string password, int driverNumber)
	{
		AddHardwareTask addHardwareTask = new AddHardwareTask(string.Empty, new VmoPropertyCollection(), repositories);
		addHardwareTask.SetNestedPropertyValue("InputParameters", "idRecorder", recordingServerId.ToString().ToUpperInvariant());
		addHardwareTask.SetNestedPropertyValue("InputParameters", "address", address.AbsoluteUri);
		addHardwareTask.SetNestedPropertyValue("InputParameters", "userName", userName);
		addHardwareTask.SetNestedPropertyValue("InputParameters", "password", password);
		addHardwareTask.SetNestedPropertyValue("InputParameters", "driverNumber", driverNumber);
		addHardwareTask.SetNestedPropertyValue("InputParameters", "definitionXml", string.Empty);
		addHardwareTask.CreateObject();
		return addHardwareTask;
	}

	public static AddHardwareTask StartNew(IVmoRepositories repositories, Guid recordingServerId, Uri address, string userName, string password, string definitionXml)
	{
		AddHardwareTask addHardwareTask = new AddHardwareTask(string.Empty, new VmoPropertyCollection(), repositories);
		addHardwareTask.SetNestedPropertyValue("InputParameters", "idRecorder", recordingServerId.ToString().ToUpperInvariant());
		addHardwareTask.SetNestedPropertyValue("InputParameters", "address", address.AbsoluteUri);
		addHardwareTask.SetNestedPropertyValue("InputParameters", "userName", userName);
		addHardwareTask.SetNestedPropertyValue("InputParameters", "password", password);
		addHardwareTask.SetNestedPropertyValue("InputParameters", "driverNumber", -1);
		addHardwareTask.SetNestedPropertyValue("InputParameters", "definitionXml", definitionXml);
		addHardwareTask.CreateObject();
		return addHardwareTask;
	}
}
