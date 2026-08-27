using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("Device")]
public abstract class Device : VmoObjectGuidBased, IUpdatable
{
	public string HardwareId => GetPropertyValue<string>("HardwareId");

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

	public DeviceType DeviceType => GetPropertyValue<DeviceType>("DeviceType");

	public int DeviceIndex => GetPropertyValue<int>("DeviceIndex");

	public string GisPoint
	{
		get
		{
			return GetPropertyValue<string>("GisPoint");
		}
		set
		{
			SetPropertyValue("GisPoint", value);
		}
	}

	public string ShortName
	{
		get
		{
			return GetPropertyValue<string>("ShortName");
		}
		set
		{
			SetPropertyValue("ShortName", value);
		}
	}

	public int Icon
	{
		get
		{
			return GetPropertyValue<int>("Icon");
		}
		set
		{
			SetPropertyValue("Icon", value);
		}
	}

	public double CoverageDirection
	{
		get
		{
			return GetPropertyValue<double>("CoverageDirection");
		}
		set
		{
			SetPropertyValue("CoverageDirection", value);
		}
	}

	public double CoverageDepth
	{
		get
		{
			return GetPropertyValue<double>("CoverageDepth");
		}
		set
		{
			SetPropertyValue("CoverageDepth", value);
		}
	}

	public double CoverageFieldOfView
	{
		get
		{
			return GetPropertyValue<double>("CoverageFieldOfView");
		}
		set
		{
			SetPropertyValue("CoverageFieldOfView", value);
		}
	}

	public Hardware Hardware => GetObjectById<Hardware>(HardwareId);

	public RecordingServer RecordingServer => GetObjectById<RecordingServer>(RecordingServerId);

	public override bool IsCustomPropertiesSupported => false;

	public Device(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void AddDeviceLink(string deviceId)
	{
		if (string.IsNullOrEmpty(deviceId))
		{
			throw new ArgumentNullException("deviceId");
		}
	}

	public void RemoveDeviceLink(string deviceId)
	{
		if (string.IsNullOrEmpty(deviceId))
		{
			throw new ArgumentNullException("deviceId");
		}
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(InternalId);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}
}
