using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Platform.AccessControl.Plugin;

namespace VideoOS.Platform.AccessControl.Elements;

[Serializable]
[DataContract(Namespace = "")]
public sealed class ACUnit : ACOperationableInstance
{
	[DataMember]
	public string ParentUnitId { get; private set; }

	[DataMember]
	public string GisPoint { get; private set; }

	public double? Latitude
	{
		get
		{
			if (string.IsNullOrEmpty(GisPoint))
			{
				return null;
			}
			return Convert.ToDouble(GisPoint.Split(',')[0].Trim(), CultureInfo.InvariantCulture);
		}
	}

	public double? Longitude
	{
		get
		{
			if (string.IsNullOrEmpty(GisPoint))
			{
				return null;
			}
			return Convert.ToDouble(GisPoint.Split(',')[1].Trim(), CultureInfo.InvariantCulture);
		}
	}

	public ACUnit(string id, string name, string iconKey, IEnumerable<ACProperty> properties, string instanceTypeId, string parentUnitId)
		: this(id, name, iconKey, null, null, properties, instanceTypeId, parentUnitId, null)
	{
	}

	public ACUnit(string id, string name, string iconKey, IEnumerable<ACProperty> properties, string instanceTypeId, string parentUnitId, IEnumerable<string> unsupportedCommandTypeIds)
		: this(id, name, iconKey, null, null, properties, instanceTypeId, parentUnitId, unsupportedCommandTypeIds)
	{
	}

	public ACUnit(string id, string name, string iconKey, double? latitude, double? longitude, IEnumerable<ACProperty> properties, string instanceTypeId, string parentUnitId)
		: this(id, name, iconKey, latitude, longitude, properties, instanceTypeId, parentUnitId, null)
	{
	}

	public ACUnit(string id, string name, string iconKey, double? latitude, double? longitude, IEnumerable<ACProperty> properties, string instanceTypeId, string parentUnitId, IEnumerable<string> unsupportedCommandTypeIds)
		: base(id, name, iconKey, properties, instanceTypeId, unsupportedCommandTypeIds)
	{
		ParentUnitId = parentUnitId;
		SetPosition(latitude, longitude);
	}

	[OnDeserialized]
	private void OnDeserialized(StreamingContext context)
	{
	}

	public override bool Equals(object obj)
	{
		if (!(obj is ACUnit aCUnit))
		{
			return false;
		}
		if (ParentUnitId != aCUnit.ParentUnitId)
		{
			return false;
		}
		if (GisPoint != aCUnit.GisPoint)
		{
			return false;
		}
		return base.Equals(obj);
	}

	public void SetPosition(double? latitude, double? longitude)
	{
		if (latitude.HasValue && longitude.HasValue)
		{
			if (!ValidatePosition(latitude.Value, longitude.Value))
			{
				throw new ACConfigurationException("Invalid coordinates for ACUnit: " + base.Name + "! Latitude must be between -90 and 90. Longitude must be between -180 and 180.");
			}
			GisPoint = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", latitude, longitude);
		}
		else
		{
			GisPoint = string.Empty;
		}
	}

	public static bool ValidatePosition(double latitude, double longitude)
	{
		if (latitude >= -90.0 && latitude <= 90.0 && longitude >= -180.0)
		{
			return longitude <= 180.0;
		}
		return false;
	}
}
