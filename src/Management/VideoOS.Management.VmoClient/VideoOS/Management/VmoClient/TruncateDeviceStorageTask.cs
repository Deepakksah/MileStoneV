using System;
using VideoOS.Management.Common;
using VideoOS.Management.Common.Tasks;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskTruncateDeviceStorage")]
public class TruncateDeviceStorageTask : AsyncTask
{
	public Guid RecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "idRecorder");

	public Guid DeviceId => GetNestedPropertyValue<Guid>("InputParameters", "idDevice");

	public string DeviceName => GetNestedPropertyValue<string>("InputParameters", "deviceName");

	public override string Name => TaskResources.TaskTruncateDeviceStorage;

	public override string EntityName => GetNestedPropertyValue<string>("InputParameters", "deviceName");

	public TaskError ErrorResult => base.ErrorResultInstance;

	public TruncateDeviceStorageTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static TruncateDeviceStorageTask StartNew(IVmoRepositories repositories, Guid deviceId)
	{
		TruncateDeviceStorageTask truncateDeviceStorageTask = new TruncateDeviceStorageTask(string.Empty, new VmoPropertyCollection(), repositories);
		truncateDeviceStorageTask.SetNestedPropertyValue("InputParameters", "idDevice", deviceId.ToString().ToUpperInvariant());
		truncateDeviceStorageTask.CreateObject(truncateDeviceStorageTask);
		return truncateDeviceStorageTask;
	}

	public static TruncateDeviceStorageTask StartNew(IVmoRepositories repositories, Guid deviceId, Guid recordingServerId)
	{
		TruncateDeviceStorageTask truncateDeviceStorageTask = new TruncateDeviceStorageTask(string.Empty, new VmoPropertyCollection(), repositories);
		truncateDeviceStorageTask.SetNestedPropertyValue("InputParameters", "idDevice", deviceId.ToString().ToUpperInvariant());
		truncateDeviceStorageTask.SetNestedPropertyValue("InputParameters", "idRecorder", recordingServerId.ToString().ToUpperInvariant());
		truncateDeviceStorageTask.CreateObject(truncateDeviceStorageTask);
		return truncateDeviceStorageTask;
	}
}
