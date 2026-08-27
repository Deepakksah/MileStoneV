using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.BusinessObjects.ServerTask;

namespace VideoOS.Management.VmoClient;

[EntityName("TaskArchiving")]
public class ArchivingTask : AsyncTask
{
	private string entityName;

	public RecordingServer RecordingServer => GetVMOObjectById<RecordingServer>(GetNestedPropertyValue<string>("InputParameters", "idRecorder"));

	public Guid SourceStorageId => GetNestedPropertyValue<Guid>("InputParameters", "idStorageSource");

	public Guid TargetStorageId => GetNestedPropertyValue<Guid>("InputParameters", "idStorageTarget");

	public override string Name => TaskResources.TaskArchiving;

	public override string EntityName
	{
		get
		{
			if (entityName == null)
			{
				ArchiveStorage targetArchive = GetTargetArchive();
				if (targetArchive != null)
				{
					entityName = string.Format(CultureInfo.InvariantCulture, "{0} - {1}", targetArchive.RecordingStorage.Name, targetArchive.Name);
				}
				else
				{
					RecordingStorage storageOfBank = GetStorageOfBank(SourceStorageId);
					if (storageOfBank != null)
					{
						entityName = storageOfBank.Name;
					}
					else
					{
						entityName = TaskResources.NameOfUnknownStorage;
					}
				}
			}
			return entityName;
		}
	}

	public ArchivingTask(string internalId, VmoPropertyCollection objectProperties, IVmoRepositories repositories)
		: base(internalId, objectProperties, repositories)
	{
	}

	private ArchiveStorage GetTargetArchive()
	{
		return GetVMOObjectById<ArchiveStorage>(TargetStorageId.ToString());
	}

	private RecordingStorage GetStorageOfBank(Guid storageId)
	{
		foreach (RecordingStorage recordingStorage in RecordingServer.RecordingStorages)
		{
			if (recordingStorage.Id == storageId)
			{
				return recordingStorage;
			}
			foreach (ArchiveStorage archiveStorage in recordingStorage.ArchiveStorages)
			{
				if (archiveStorage.Id == storageId)
				{
					return recordingStorage;
				}
			}
		}
		return null;
	}
}
