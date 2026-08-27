using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskSetEncryptionMethod")]
public class SetEncryptionMethodTask : AsyncTask
{
	public RecordingServer RecordingServer => GetVMOObjectById<RecordingServer>(GetNestedPropertyValue<string>("InputParameters", "idRecorder"));

	public RecordingStorage Storage => GetVMOObjectById<RecordingStorage>(GetNestedPropertyValue<string>("InputParameters", "idStorage"));

	public string StorageName => GetNestedPropertyValue<string>("InputParameters", "storageName");

	public StorageEncryptionMethod Method
	{
		get
		{
			if (!Enum.TryParse<StorageEncryptionMethod>(GetNestedPropertyValue<string>("InputParameters", "encryptionMethod"), out var result))
			{
				return StorageEncryptionMethod.Unknown;
			}
			return result;
		}
	}

	public override string Name => TaskResources.TaskSetEncryptionMethod;

	public override string EntityName => StorageName;

	public SetEncryptionMethodTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static SetEncryptionMethodTask StartNew(IVmoRepositories repositories, Guid recordingStorageId, StorageEncryptionMethod encryptionMethod, string password)
	{
		SetEncryptionMethodTask setEncryptionMethodTask = new SetEncryptionMethodTask(string.Empty, new VmoPropertyCollection(), repositories);
		setEncryptionMethodTask.SetNestedPropertyValue("InputParameters", "idStorage", recordingStorageId.ToString().ToUpperInvariant());
		setEncryptionMethodTask.SetNestedPropertyValue("InputParameters", "encryptionMethod", encryptionMethod);
		setEncryptionMethodTask.SetNestedPropertyValue("InputParameters", "password", password);
		setEncryptionMethodTask.CreateObject();
		return setEncryptionMethodTask;
	}
}
