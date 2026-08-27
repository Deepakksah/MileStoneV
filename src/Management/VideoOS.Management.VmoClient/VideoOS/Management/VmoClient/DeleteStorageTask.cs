using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskDeleteStorage")]
public class DeleteStorageTask : AsyncTask
{
	public Guid RecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "idRecorder");

	public Guid StorageId => GetNestedPropertyValue<Guid>("InputParameters", "idStorage");

	public string StorageName => GetNestedPropertyValue<string>("InputParameters", "storageName");

	public string StoragePath => GetNestedPropertyValue<string>("InputParameters", "storagePath");

	public override string Name => TaskResources.TaskDeleteStorage;

	public override string EntityName => StorageName;

	public DeleteStorageTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static DeleteStorageTask StartNew(IVmoRepositories repositories, Guid recordingStorageId, bool deleteIfOffline)
	{
		DeleteStorageTask deleteStorageTask = new DeleteStorageTask(string.Empty, new VmoPropertyCollection(), repositories);
		deleteStorageTask.SetNestedPropertyValue("InputParameters", "idStorage", recordingStorageId.ToString().ToUpperInvariant());
		deleteStorageTask.SetNestedPropertyValue("InputParameters", "deleteIfOffline", deleteIfOffline);
		deleteStorageTask.CreateObject(deleteStorageTask);
		return deleteStorageTask;
	}
}
