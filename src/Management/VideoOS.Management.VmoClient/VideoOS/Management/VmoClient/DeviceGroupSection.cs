using System;

namespace VideoOS.Management.VmoClient;

public class DeviceGroupSection
{
	private readonly IVmoRepositories Repositories;

	private readonly Guid ManagementServerId;

	public CameraDeviceGroupCollection CameraDeviceGroups { get; private set; }

	public MicrophoneDeviceGroupCollection MicrophoneDeviceGroups { get; private set; }

	public MetadataDeviceGroupCollection MetadataDeviceGroups { get; private set; }

	public SpeakerDeviceGroupCollection SpeakerDeviceGroups { get; private set; }

	public OutputDeviceGroupCollection OutputDeviceGroups { get; private set; }

	public InputDeviceGroupCollection InputDeviceGroups { get; private set; }

	public DeviceGroupSection(ManagementServer managementServer)
	{
		Repositories = managementServer.Repositories;
		ManagementServerId = new Guid(managementServer.InternalId);
		CameraDeviceGroups = new CameraDeviceGroupCollection(LoadCameraDeviceGroups, managementServer, isInitialized: false);
		MicrophoneDeviceGroups = new MicrophoneDeviceGroupCollection(LoadMicrophoneDeviceGroups, managementServer, isInitialized: false);
		MetadataDeviceGroups = new MetadataDeviceGroupCollection(LoadMetadataDeviceGroups, managementServer, isInitialized: false);
		SpeakerDeviceGroups = new SpeakerDeviceGroupCollection(LoadSpeakerDeviceGroups, managementServer, isInitialized: false);
		OutputDeviceGroups = new OutputDeviceGroupCollection(LoadOutputDeviceGroups, managementServer, isInitialized: false);
		InputDeviceGroups = new InputDeviceGroupCollection(LoadInputDeviceGroups, managementServer, isInitialized: false);
	}

	private CameraDeviceGroup[] LoadCameraDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return Repositories.GetObjectsByParentId<ManagementServer, CameraDeviceGroup>(ManagementServerId, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new CameraDeviceGroup[0];
		}
	}

	private MicrophoneDeviceGroup[] LoadMicrophoneDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return Repositories.GetObjectsByParentId<ManagementServer, MicrophoneDeviceGroup>(ManagementServerId, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new MicrophoneDeviceGroup[0];
		}
	}

	private MetadataDeviceGroup[] LoadMetadataDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return Repositories.GetObjectsByParentId<ManagementServer, MetadataDeviceGroup>(ManagementServerId, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new MetadataDeviceGroup[0];
		}
	}

	private SpeakerDeviceGroup[] LoadSpeakerDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return Repositories.GetObjectsByParentId<ManagementServer, SpeakerDeviceGroup>(ManagementServerId, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new SpeakerDeviceGroup[0];
		}
	}

	private OutputDeviceGroup[] LoadOutputDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return Repositories.GetObjectsByParentId<ManagementServer, OutputDeviceGroup>(ManagementServerId, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new OutputDeviceGroup[0];
		}
	}

	private InputDeviceGroup[] LoadInputDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return Repositories.GetObjectsByParentId<ManagementServer, InputDeviceGroup>(ManagementServerId, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new InputDeviceGroup[0];
		}
	}
}
