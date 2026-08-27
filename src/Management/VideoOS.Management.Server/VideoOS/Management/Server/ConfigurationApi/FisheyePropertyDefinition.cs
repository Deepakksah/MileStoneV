using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.Integration;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class FisheyePropertyDefinition
{
	public static Property LensTypeProperty()
	{
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		list.Add(new ValueTypeInfo
		{
			Name = ConfigurationApiResources.PropertyFisheyeImmervisionPanomorph,
			Value = "0"
		});
		return new Property("RegisteredPanomorphLens", "0")
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyFisheyeLensType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyFisheyeLensType",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = list.ToArray()
		};
	}

	public static Property CameraOrientationProperty(DevicePosition orientation)
	{
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		list.Add(new ValueTypeInfo
		{
			Name = ConfigurationApiResources.FisheyePositionCeiling,
			Value = DevicePosition.Ceiling.ToString()
		});
		list.Add(new ValueTypeInfo
		{
			Name = ConfigurationApiResources.FisheyePositionWall,
			Value = DevicePosition.Wall.ToString()
		});
		list.Add(new ValueTypeInfo
		{
			Name = ConfigurationApiResources.FisheyePositionGround,
			Value = DevicePosition.Ground.ToString()
		});
		return new Property("Orientation", orientation.ToString())
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyFisheyeOrientation,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyFisheyeOrientation",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = list.ToArray()
		};
	}

	public static Property RegisteredPanomorphLensNumberProperty(string registeredPanomorphLensNumber, IReadOnlyCollection<LensProfile> lensProfiles)
	{
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (LensProfile lensProfile in lensProfiles)
		{
			list.Add(new ValueTypeInfo
			{
				Name = lensProfile.RegisteredPanomorphLensNumber,
				Value = lensProfile.RegisteredPanomorphLensNumber
			});
		}
		return new Property("RegisteredPanomorphLensNumber", registeredPanomorphLensNumber)
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyFisheyeRegisteredPanomorphLens,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyFisheyeRegisteredPanomorphLens",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = list.ToArray()
		};
	}

	public static Property FieldOfViewProperty(double fieldOfView)
	{
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		list.Add(new ValueTypeInfo
		{
			Name = "MinValue",
			Value = "80"
		});
		list.Add(new ValueTypeInfo
		{
			Name = "MaxValue",
			Value = "360"
		});
		return new Property("FisheyeFieldOfView", fieldOfView.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Double",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyFisheyeFieldOfView,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyFisheyeFieldOfView",
			UIImportance = UIImportance.Default,
			SortKey = 100,
			ValueTypeInfos = list.ToArray()
		};
	}
}
