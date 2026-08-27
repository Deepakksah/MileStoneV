using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskDeleteArchive")]
public class DeleteArchiveTask : AsyncTask
{
	public Guid RecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "idRecorder");

	public Guid StorageId => GetNestedPropertyValue<Guid>("InputParameters", "idStorage");

	public Guid ArchiveId => GetNestedPropertyValue<Guid>("InputParameters", "idArchive");

	public string ArchiveName => GetNestedPropertyValue<string>("InputParameters", "archiveName");

	public string ArchivePath => GetNestedPropertyValue<string>("InputParameters", "archivePath");

	public string ArchiveFolder => GetNestedPropertyValue<string>("InputParameters", "archiveFolder");

	public override string Name => TaskResources.TaskDeleteArchive;

	public override string EntityName => ArchiveName;

	public DeleteArchiveTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static DeleteArchiveTask StartNew(IVmoRepositories repositories, Guid archiveStorageId, bool deleteIfOffline)
	{
		DeleteArchiveTask deleteArchiveTask = new DeleteArchiveTask(string.Empty, new VmoPropertyCollection(), repositories);
		deleteArchiveTask.SetNestedPropertyValue("InputParameters", "idArchive", archiveStorageId.ToString().ToUpperInvariant());
		deleteArchiveTask.SetNestedPropertyValue("InputParameters", "deleteIfOffline", deleteIfOffline);
		deleteArchiveTask.CreateObject(deleteArchiveTask);
		return deleteArchiveTask;
	}
}
