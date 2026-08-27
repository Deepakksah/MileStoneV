using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskMoveArchiveStorage")]
public class MoveArchiveTask : AsyncTask
{
	public Guid RecordingServerId => GetNestedPropertyValue<Guid>("InputParameters", "idRecorder");

	public Guid StorageId => GetNestedPropertyValue<Guid>("InputParameters", "idStorage");

	public Guid ArchiveId => GetNestedPropertyValue<Guid>("InputParameters", "idArchive");

	public string ArchiveName => GetNestedPropertyValue<string>("InputParameters", "archiveName");

	public string TargetPath => GetNestedPropertyValue<string>("InputParameters", "pathTarget");

	public string TargetFolder => GetNestedPropertyValue<string>("InputParameters", "folderTarget");

	public string SourcePath => GetNestedPropertyValue<string>("InputParameters", "pathSource");

	public string SourceBankId => GetNestedPropertyValue<string>("InputParameters", "bankIdSource");

	public string TargetBankId => GetNestedPropertyValue<string>("InputParameters", "bankIdTarget");

	public string SourceFolder => GetNestedPropertyValue<string>("InputParameters", "folderSource");

	public override string Name => TaskResources.TaskMoveArchiveStorage;

	public override string EntityName => GetNestedPropertyValue<string>("InputParameters", "archiveName");

	public MoveArchiveTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	public static MoveArchiveTask StartNew(IVmoRepositories repositories, Guid archiveId, string targetPath)
	{
		MoveArchiveTask moveArchiveTask = new MoveArchiveTask(string.Empty, new VmoPropertyCollection(), repositories);
		moveArchiveTask.SetNestedPropertyValue("InputParameters", "idArchive", archiveId.ToString().ToUpperInvariant());
		moveArchiveTask.SetNestedPropertyValue("InputParameters", "pathTarget", targetPath.ToString());
		moveArchiveTask.CreateObject(moveArchiveTask);
		return moveArchiveTask;
	}
}
