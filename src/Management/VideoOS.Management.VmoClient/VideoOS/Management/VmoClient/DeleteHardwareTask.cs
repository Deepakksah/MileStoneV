using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskDeleteHardware")]
public class DeleteHardwareTask : AsyncTask
{
	public Guid RecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "idRecorder");

	public Guid HardwareId => GetNestedPropertyValue<Guid>("InputParameters", "hardwareId");

	public string HardwareName => GetNestedPropertyValue<string>("InputParameters", "hardwareName");

	public override string Name => TaskResources.TaskDeleteHardware;

	public override string EntityName => HardwareName;

	public DeleteHardwareTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static DeleteHardwareTask StartNew(IVmoRepositories repositories, Guid hardwareId)
	{
		DeleteHardwareTask deleteHardwareTask = new DeleteHardwareTask(string.Empty, new VmoPropertyCollection(), repositories);
		deleteHardwareTask.SetNestedPropertyValue("InputParameters", "hardwareId", hardwareId.ToString().ToUpperInvariant());
		deleteHardwareTask.CreateObject(deleteHardwareTask);
		return deleteHardwareTask;
	}
}
