using System;
using System.Collections.Generic;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Properties;

namespace VideoOS.Management.VmoClient;

[EntityName("RecordingStorage")]
public class RecordingStorage : VmoObjectGuidBased, IUpdatable, IDeletable, ICreatable
{
	public string Name
	{
		get
		{
			return GetPropertyValue<string>("Name");
		}
		set
		{
			SetPropertyValue("Name", value);
		}
	}

	public string Description
	{
		get
		{
			return GetPropertyValue<string>("Description");
		}
		set
		{
			SetPropertyValue("Description", value);
		}
	}

	public DateTime CreatedDate => GetPropertyValue<DateTime>("CreatedDate");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public string Path => GetPropertyValue<string>("Path");

	public int RetainMinutes
	{
		get
		{
			return GetPropertyValue<int>("RetainMinutes");
		}
		set
		{
			SetPropertyValue("RetainMinutes", value);
		}
	}

	public long MaxSize
	{
		get
		{
			return GetPropertyValue<long>("MaxSize");
		}
		set
		{
			SetPropertyValue("MaxSize", value);
		}
	}

	public bool IsDefault => GetPropertyValue<bool>("IsDefault");

	public bool Signing
	{
		get
		{
			return GetPropertyValue<bool>("Signing");
		}
		set
		{
			SetPropertyValue("Signing", value);
		}
	}

	public StorageEncryptionMethod Encryption => GetPropertyValue<StorageEncryptionMethod>("EncryptionMethod");

	public string RecordingServerId => GetPropertyValue<string>("RecordingServerId");

	public RecordingServer RecordingServer => GetObjectById<RecordingServer>(RecordingServerId);

	public ArchiveStorageCollection ArchiveStorages { get; private set; }

	public RecordingStorage(RecordingServer recordingServer, string path)
		: this(recordingServer.Repositories, new VmoPropertyCollection(), recordingServer.Id, path)
	{
	}

	public RecordingStorage(VmoPropertyCollection properties, RecordingServer recordingServer, string path)
		: this(recordingServer.Repositories, properties, recordingServer.Id, path)
	{
	}

	public RecordingStorage(IVmoRepositories repositories, Guid recordingServerId, string path)
		: this(repositories, new VmoPropertyCollection(), recordingServerId, path)
	{
	}

	public RecordingStorage(IVmoRepositories repositories, VmoPropertyCollection objectProperties, Guid recordingServerId, string path)
		: this(string.Empty, objectProperties, new VmoRelationCollection(), repositories, isInitialized: false)
	{
		SetPropertyValue("Path", path);
		SetPropertyValue("RecordingServerId", recordingServerId.ToString());
		RetainMinutes = 1440;
		MaxSize = 1024L;
	}

	public RecordingStorage(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		ArchiveStorages = new ArchiveStorageCollection(LoadArchiveStorage, this, isInitialized);
	}

	public MoveStorageTask Move(string newPath)
	{
		ThrowIfNotCreatedOrDeleted();
		MoveStorageTask result = MoveStorageTask.StartNew(base.Repositories, base.Id, newPath);
		Refresh();
		return result;
	}

	public StorageInformation GetStorageInformation()
	{
		ThrowIfNotCreatedOrDeleted();
		VMOWebserviceObject recordingStorageInformation = base.Repositories.ServerCommunication.GetRecordingStorageInformation(base.InternalId);
		if (recordingStorageInformation == null)
		{
			return null;
		}
		return new StorageInformation(recordingStorageInformation.Properties);
	}

	public bool ValidateEncryptionPassword(string password)
	{
		ThrowIfNotCreatedOrDeleted();
		return base.Repositories.ServerCommunication.ValidateEncryptionPasswordRecordingStorage(base.InternalId, password);
	}

	public void SetAsDefault()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repositories.ServerCommunication.SetRecordingStorageAsDefault(base.InternalId);
		Refresh();
	}

	public AsyncTask[] EnableEncryption(StorageEncryptionMethod encryptionMethod, string password)
	{
		ThrowIfNotCreatedOrDeleted();
		if (encryptionMethod == StorageEncryptionMethod.None || encryptionMethod == StorageEncryptionMethod.Unknown)
		{
			throw new ArgumentException(VmoResources.VMO60092, "encryptionMethod");
		}
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.EnableRecordingStorageEncryption(base.InternalId, encryptionMethod, password);
		Refresh();
		List<AsyncTask> list = new List<AsyncTask>();
		VMOWebserviceObject[] array2 = array;
		foreach (VMOWebserviceObject vMOWebserviceObject in array2)
		{
			if (vMOWebserviceObject.Type == typeof(SetEncryptionMethodTask))
			{
				SetEncryptionMethodTask item = new SetEncryptionMethodTask(vMOWebserviceObject.Id, vMOWebserviceObject.Properties, base.Repositories);
				list.Add(item);
			}
			else if (vMOWebserviceObject.Type == typeof(SetEncryptionPasswordTask))
			{
				SetEncryptionPasswordTask item2 = new SetEncryptionPasswordTask(vMOWebserviceObject.Id, vMOWebserviceObject.Properties, base.Repositories);
				list.Add(item2);
			}
		}
		return list.ToArray();
	}

	public AsyncTask[] DisableEncryption(string password)
	{
		ThrowIfNotCreatedOrDeleted();
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.DisableRecordingStorageEncryption(base.InternalId, password);
		Refresh();
		List<AsyncTask> list = new List<AsyncTask>();
		VMOWebserviceObject[] array2 = array;
		foreach (VMOWebserviceObject vMOWebserviceObject in array2)
		{
			if (vMOWebserviceObject.Type == typeof(SetEncryptionMethodTask))
			{
				SetEncryptionMethodTask item = new SetEncryptionMethodTask(vMOWebserviceObject.Id, vMOWebserviceObject.Properties, base.Repositories);
				list.Add(item);
			}
			else if (vMOWebserviceObject.Type == typeof(SetEncryptionPasswordTask))
			{
				SetEncryptionPasswordTask item2 = new SetEncryptionPasswordTask(vMOWebserviceObject.Id, vMOWebserviceObject.Properties, base.Repositories);
				list.Add(item2);
			}
		}
		return list.ToArray();
	}

	public SetEncryptionMethodTask ChangeEncryptionMethod(StorageEncryptionMethod encryptionMethod, string password)
	{
		ThrowIfNotCreatedOrDeleted();
		if (encryptionMethod == StorageEncryptionMethod.None || encryptionMethod == StorageEncryptionMethod.Unknown)
		{
			throw new ArgumentException(VmoResources.VMO60092, "encryptionMethod");
		}
		SetEncryptionMethodTask result = SetEncryptionMethodTask.StartNew(base.Repositories, new Guid(InternalId), encryptionMethod, password);
		Refresh();
		return result;
	}

	public SetEncryptionPasswordTask ChangeEncryptionPassword(string oldPassword, string newPassword)
	{
		ThrowIfNotCreatedOrDeleted();
		SetEncryptionPasswordTask result = SetEncryptionPasswordTask.StartNew(base.Repositories, new Guid(InternalId), oldPassword, newPassword);
		Refresh();
		return result;
	}

	public void Create()
	{
		base.Repository.CreateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}

	public DeleteStorageTask Delete(bool deleteIfOffline)
	{
		ThrowIfNotCreatedOrDeleted();
		return DeleteStorageTask.StartNew(base.Repositories, base.Id, deleteIfOffline);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	public Device[] GetDevicesUsingStorage()
	{
		ThrowIfNotCreatedOrDeleted();
		return base.Repositories.GetObjectsByParentId<RecordingStorage, Device>(base.Id, refreshCachedObjects: false);
	}

	private ArchiveStorage[] LoadArchiveStorage(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<RecordingStorage, ArchiveStorage>(base.Id, refreshCachedObjects);
	}
}
