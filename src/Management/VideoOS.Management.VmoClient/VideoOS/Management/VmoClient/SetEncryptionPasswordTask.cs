using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskSetEncryptionPassword")]
public class SetEncryptionPasswordTask : AsyncTask
{
	public RecordingServer RecordingServer => GetVMOObjectById<RecordingServer>(GetNestedPropertyValue<string>("InputParameters", "idRecorder"));

	public RecordingStorage Storage => GetVMOObjectById<RecordingStorage>(GetNestedPropertyValue<string>("InputParameters", "idStorage"));

	public string StorageName => GetNestedPropertyValue<string>("InputParameters", "storageName");

	public override string Name => TaskResources.TaskSetEncryptionPassword;

	public override string EntityName => StorageName;

	public SetEncryptionPasswordTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static SetEncryptionPasswordTask StartNew(IVmoRepositories repositories, Guid recordingStorageId, string oldPassword, string newPassword)
	{
		SetEncryptionPasswordTask setEncryptionPasswordTask = new SetEncryptionPasswordTask(string.Empty, new VmoPropertyCollection(), repositories);
		setEncryptionPasswordTask.SetNestedPropertyValue("InputParameters", "idStorage", recordingStorageId.ToString().ToUpperInvariant());
		setEncryptionPasswordTask.SetNestedPropertyValue("InputParameters", "oldPassword", oldPassword);
		setEncryptionPasswordTask.SetNestedPropertyValue("InputParameters", "newPassword", newPassword);
		setEncryptionPasswordTask.CreateObject(setEncryptionPasswordTask);
		return setEncryptionPasswordTask;
	}
}
