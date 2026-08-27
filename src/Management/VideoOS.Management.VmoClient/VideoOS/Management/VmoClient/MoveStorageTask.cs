using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskMoveRecordingStorage")]
public class MoveStorageTask : AsyncTask
{
	public Guid RecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "idRecorder");

	public Guid StorageId => GetNestedPropertyValue<Guid>("InputParameters", "idStorage");

	public string StorageName => GetNestedPropertyValue<string>("InputParameters", "storageName");

	public string TargetPath => GetNestedPropertyValue<string>("InputParameters", "pathTarget");

	public string SourcePath => GetNestedPropertyValue<string>("InputParameters", "pathSource");

	public string SourceBankId => GetNestedPropertyValue<string>("InputParameters", "bankIdSource");

	public string TargetBankId => GetNestedPropertyValue<string>("InputParameters", "bankIdTarget");

	public override string Name => TaskResources.TaskMoveRecordingStorage;

	public override string EntityName => GetNestedPropertyValue<string>("InputParameters", "storageName");

	public MoveStorageTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static MoveStorageTask StartNew(IVmoRepositories repositories, Guid recordingStorageId, string targetPath)
	{
		MoveStorageTask moveStorageTask = new MoveStorageTask(string.Empty, new VmoPropertyCollection(), repositories);
		moveStorageTask.SetNestedPropertyValue("InputParameters", "idStorage", recordingStorageId.ToString().ToUpperInvariant());
		moveStorageTask.SetNestedPropertyValue("InputParameters", "pathTarget", targetPath.ToString());
		moveStorageTask.CreateObject(moveStorageTask);
		return moveStorageTask;
	}
}
