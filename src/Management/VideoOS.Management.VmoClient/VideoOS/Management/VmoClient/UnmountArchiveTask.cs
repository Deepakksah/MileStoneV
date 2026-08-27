using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskUnmountArchiveStorage")]
public class UnmountArchiveTask : AsyncTask
{
	public Guid RecordingServerId => GetNestedInputPropertyValue<Guid>("idRecorder");

	public Guid StorageId => GetNestedInputPropertyValue<Guid>("idStorage");

	public Guid ArchiveId => GetNestedInputPropertyValue<Guid>("idArchive");

	public string ArchiveName => GetNestedInputPropertyValue<string>("archiveName");

	public string Path => GetNestedInputPropertyValue<string>("path");

	public string Folder => GetNestedInputPropertyValue<string>("folder");

	public string BankId => GetNestedInputPropertyValue<string>("bankId");

	public override string Name => TaskResources.TaskUnmountArchiveStorage;

	public override string EntityName => ArchiveName;

	public UnmountArchiveTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}
}
