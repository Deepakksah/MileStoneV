using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskMoveDevice")]
public class MoveDeviceTask : AsyncTask
{
	public Guid RecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "idRecorder");

	public Guid StorageId => GetNestedPropertyValue<Guid>("InputParameters", "idStorage");

	public Guid DeviceId => GetNestedPropertyValue<Guid>("InputParameters", "idDevice");

	public string DeviceName => GetNestedPropertyValue<string>("InputParameters", "deviceName");

	public Guid TargetStorageId => GetNestedPropertyValue<Guid>("InputParameters", "idStorageTarget");

	public bool MoveData => GetNestedPropertyValue<int>("InputParameters", "moveData") == 1;

	public override string Name => TaskResources.TaskMoveDevice;

	public override string EntityName => DeviceName;

	public MoveDeviceTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static MoveDeviceTask StartNew(IVmoRepositories repositories, Guid deviceId, Guid targetStorageId)
	{
		MoveDeviceTask moveDeviceTask = new MoveDeviceTask(string.Empty, new VmoPropertyCollection(), repositories);
		moveDeviceTask.SetNestedPropertyValue("InputParameters", "idDevice", deviceId.ToString().ToUpperInvariant());
		moveDeviceTask.SetNestedPropertyValue("InputParameters", "idStorageTarget", targetStorageId.ToString().ToUpperInvariant());
		moveDeviceTask.SetNestedPropertyValue("InputParameters", "moveData", 0);
		moveDeviceTask.CreateObject(moveDeviceTask);
		return moveDeviceTask;
	}
}
