using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MotionDetection")]
public sealed class MotionDetection : VmoObjectGuidBased, IUpdatable
{
	private Grid _excludeRegions;

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

	public bool ManualSensitivityEnabled
	{
		get
		{
			return GetPropertyValue<bool>("ManualSensitivityEnabled");
		}
		set
		{
			SetPropertyValue("ManualSensitivityEnabled", value);
		}
	}

	public int ManualSensitivity
	{
		get
		{
			return GetPropertyValue<int>("MotionPixThreshold");
		}
		set
		{
			SetPropertyValue("MotionPixThreshold", value);
		}
	}

	public int Threshold
	{
		get
		{
			return GetPropertyValue<int>("MotionPixCountThreshold");
		}
		set
		{
			SetPropertyValue("MotionPixCountThreshold", value);
		}
	}

	public bool KeyframesOnly
	{
		get
		{
			return GetPropertyValue<bool>("KeyframesOnly");
		}
		set
		{
			SetPropertyValue("KeyframesOnly", value);
		}
	}

	public ProcessTime.ProcessTimeType ProcessTime
	{
		get
		{
			return GetPropertyValue<ProcessTime.ProcessTimeType>("ProcessTime");
		}
		set
		{
			SetPropertyValue("ProcessTime", value);
		}
	}

	public DetectionMethod.DetectionMethodType DetectionMethod
	{
		get
		{
			return GetPropertyValue<DetectionMethod.DetectionMethodType>("DetectionMethod");
		}
		set
		{
			SetPropertyValue("DetectionMethod", value);
		}
	}

	public HardwareAccelerationMode.HardwareAccelerationModeType HardwareAccelerationMode
	{
		get
		{
			return GetPropertyValue<HardwareAccelerationMode.HardwareAccelerationModeType>("HardwareAccelerationMode");
		}
		set
		{
			SetPropertyValue("HardwareAccelerationMode", value);
		}
	}

	public bool GenerateMotionMetadata
	{
		get
		{
			return GetPropertyValue<bool>("GenerateMotionMetadata");
		}
		set
		{
			SetPropertyValue("GenerateMotionMetadata", value);
		}
	}

	public bool UseExcludeRegions
	{
		get
		{
			return GetPropertyValue<bool>("UseExcludeRegions");
		}
		set
		{
			SetPropertyValue("UseExcludeRegions", value);
		}
	}

	public Grid ExcludeRegions
	{
		get
		{
			if (_excludeRegions == null)
			{
				GridSize.GridSizeType propertyValue = GetPropertyValue<GridSize.GridSizeType>("GridSize");
				string propertyValue2 = GetPropertyValue<string>("MotionDetectionExcludeRegions");
				_excludeRegions = new Grid(propertyValue2, propertyValue);
				_excludeRegions.GridChanged += ExcludeRegionsChanged;
			}
			return _excludeRegions;
		}
	}

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public string CameraId => GetPropertyValue<string>("DeviceId");

	public CameraDevice CameraDevice => GetObjectById<CameraDevice>(CameraId);

	public override bool IsCustomPropertiesSupported => false;

	public MotionDetection(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	private void ExcludeRegionsChanged(object sender, EventArgs e)
	{
		SetPropertyValue("GridSize", _excludeRegions.GridSize);
		SetPropertyValue("MotionDetectionExcludeRegions", _excludeRegions.GetGridAsString());
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

	protected override void PropertiesLoaded()
	{
		base.PropertiesLoaded();
		if (_excludeRegions != null)
		{
			GridSize.GridSizeType propertyValue = GetPropertyValue<GridSize.GridSizeType>("GridSize");
			string propertyValue2 = GetPropertyValue<string>("MotionDetectionExcludeRegions");
			_excludeRegions.Initialize(propertyValue2, propertyValue);
		}
	}
}
