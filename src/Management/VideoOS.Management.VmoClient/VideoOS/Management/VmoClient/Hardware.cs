using System;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("Hardware")]
public class Hardware : VmoObjectGuidBased, IUpdatable, IDeletable
{
	public string HardwareDriverId => GetPropertyValue<string>("HardwareDriverId");

	public string RecordingServerId => GetPropertyValue<string>("RecordingServerId");

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

	public bool Enabled
	{
		get
		{
			return GetPropertyValue<bool>("Enabled");
		}
		set
		{
			SetPropertyValue("Enabled", value);
		}
	}

	public string Address
	{
		get
		{
			return GetPropertyValue<string>("Address");
		}
		set
		{
			SetPropertyValue("Address", value);
		}
	}

	public string UserName
	{
		get
		{
			return GetPropertyValue<string>("UserName");
		}
		set
		{
			SetPropertyValue("UserName", value);
		}
	}

	public string ProductId => GetPropertyValue<string>("ProductId");

	public int DriverNumber => GetPropertyValue<int>("DriverNumber");

	public RecordingServer RecordingServer => GetObjectById<RecordingServer>(RecordingServerId);

	public DeviceCollection Devices { get; private set; }

	public override bool IsCustomPropertiesSupported => false;

	public Hardware(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		Devices = new DeviceCollection(LoadDevices, this, isInitialized);
	}

	public HardwareDriver GetActiveDriver()
	{
		ThrowIfNotCreatedOrDeleted();
		return new HardwareDriver(base.Repositories.ServerCommunication.QueryActiveHardwareDriver(RecordingServerId, HardwareDriverId).Properties);
	}

	public void Refresh()
	{
		Refresh(updateRelations: false);
	}

	public void Refresh(bool updateRelations)
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(InternalId, updateRelations, updateRelatedObjects: true);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}

	private Device[] LoadDevices(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<Hardware, Device>(base.Id, refreshCachedObjects);
	}

	public MoveHardwareTask MoveToRecordingServer(Guid destinationRecordingServerId, Guid destinationStorageId, bool ignoreSourceRecordingServer)
	{
		ThrowIfNotCreatedOrDeleted();
		return MoveHardwareTask.StartNew(base.Repositories, base.Id, destinationRecordingServerId, destinationStorageId, ignoreSourceRecordingServer);
	}

	public ChangeHardwarePasswordTask HardwarePasswordChange(string password)
	{
		ThrowIfNotCreatedOrDeleted();
		return ChangeHardwarePasswordTask.StartNew(base.Repositories, base.Id, null, password);
	}

	public ChangeHardwarePasswordTask HardwareCredentialsChange(string userName, string password)
	{
		ThrowIfNotCreatedOrDeleted();
		return ChangeHardwarePasswordTask.StartNew(base.Repositories, base.Id, userName, password);
	}

	public void ChangePassword(string password)
	{
		ThrowIfNotCreatedOrDeleted();
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		vmoPropertyCollection.Add("Password", password);
		base.Repositories.ServerCommunication.Execute("ChangeHardwarePassword", vmoPropertyCollection);
	}

	public string ReadPassword()
	{
		ThrowIfNotCreatedOrDeleted();
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		return base.Repositories.ServerCommunication.Execute("ReadHardwarePassword", vmoPropertyCollection)[0].Properties.GetPropertyValue<string>("Password");
	}

	public UpdateHardwareTask UpdateHardware(long sessionDataId)
	{
		ThrowIfNotCreatedOrDeleted();
		Guid hardwareId = Guid.Parse(InternalId);
		return UpdateHardwareTask.StartNew(base.Repositories, hardwareId, sessionDataId.ToString(CultureInfo.InvariantCulture));
	}

	public FirmwareUpgradeTask HardwareFirmwareUpgrade(string fileId)
	{
		ThrowIfCheckVersionFail(ServerVersions.v21_1);
		ThrowIfNotCreatedOrDeleted();
		return FirmwareUpgradeTask.StartNew(base.Repositories, base.Id, fileId);
	}
}
