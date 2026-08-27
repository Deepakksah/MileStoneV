using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace VideoOS.Platform.Metadata;

public class NavigationalData : IXmlSerializable, IEquatable<NavigationalData>
{
	public const double MaxAzimuth = 180.0;

	public const double MinAzimuth = -180.0;

	public const double MaxLatitude = 90.0;

	public const double MinLatitude = -90.0;

	public const double MaxLongitude = 180.0;

	public const double MinLongitude = -180.0;

	public const double MinSpeed = 0.0;

	public const double MinAccuracy = double.Epsilon;

	private double? _latitude;

	private double? _longitude;

	private double? _verticalAccuracy;

	private double? _speed;

	private double? _horizontalAccuracy;

	private double? _azimuth;

	public double? Altitude { get; set; }

	public double? Azimuth
	{
		get
		{
			return _azimuth;
		}
		set
		{
			if (value > 180.0 || value < -180.0)
			{
				throw new ArgumentOutOfRangeException("value", "Azimuth must be a number in the range [-180; 180]");
			}
			_azimuth = value;
		}
	}

	public string GeodeticSystem { get; set; }

	public double? HorizontalAccuracy
	{
		get
		{
			return _horizontalAccuracy;
		}
		set
		{
			if (value < double.Epsilon)
			{
				throw new ArgumentOutOfRangeException("value", "Horizontal Accuracy must be a positive number");
			}
			_horizontalAccuracy = value;
		}
	}

	public double? Latitude
	{
		get
		{
			return _latitude;
		}
		set
		{
			if (value > 90.0 || value < -90.0)
			{
				throw new ArgumentOutOfRangeException("value", "Latitude must be a number in the range [-90; 90]");
			}
			_latitude = value;
		}
	}

	public double? Longitude
	{
		get
		{
			return _longitude;
		}
		set
		{
			if (value > 180.0 || value < -180.0)
			{
				throw new ArgumentOutOfRangeException("value", "Longitude must be a number in the range [-180; 180]");
			}
			_longitude = value;
		}
	}

	public double? VerticalAccuracy
	{
		get
		{
			return _verticalAccuracy;
		}
		set
		{
			if (value < double.Epsilon)
			{
				throw new ArgumentOutOfRangeException("value", "Vertical Accuracy must be a positive number");
			}
			_verticalAccuracy = value;
		}
	}

	public double? Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			if (value < 0.0)
			{
				throw new ArgumentOutOfRangeException("value", "Speed must not be a negative number");
			}
			_speed = value;
		}
	}

	public Version Version { get; set; }

	public NavigationalData()
	{
		Version = new Version(1, 0);
	}

	public XmlSchema GetSchema()
	{
		return null;
	}

	public void ReadXml(XmlReader reader)
	{
		reader.MoveToContent();
		int depth = reader.Depth;
		bool isEmptyElement = reader.IsEmptyElement;
		BlankAllFields();
		ReadAttributes(reader);
		reader.ReadStartElement();
		if (!isEmptyElement)
		{
			ReadChildren(reader, depth);
			reader.ReadEndElement();
		}
	}

	private void ReadChildren(XmlReader reader, int rootDepth)
	{
		do
		{
			if (reader.Depth == rootDepth + 1 && reader.NodeType == XmlNodeType.Element)
			{
				string localName = reader.LocalName;
				if (object.Equals(MetadataXml.AltitudeElement, localName))
				{
					reader.ReadStartElement();
					Altitude = reader.ReadContentAsDouble();
				}
				else if (object.Equals(MetadataXml.AzimuthElement, localName))
				{
					reader.ReadStartElement();
					Azimuth = reader.ReadContentAsDouble();
				}
				else if (object.Equals(MetadataXml.GeodeticSystemElement, localName))
				{
					reader.ReadStartElement();
					GeodeticSystem = reader.ReadContentAsString();
				}
				else if (object.Equals(MetadataXml.HorizontalAccuracyElement, localName))
				{
					reader.ReadStartElement();
					HorizontalAccuracy = reader.ReadContentAsDouble();
				}
				else if (object.Equals(MetadataXml.LatitudeElement, localName))
				{
					reader.ReadStartElement();
					Latitude = reader.ReadContentAsDouble();
				}
				else if (object.Equals(MetadataXml.LongitudeElement, localName))
				{
					reader.ReadStartElement();
					Longitude = reader.ReadContentAsDouble();
				}
				else if (object.Equals(MetadataXml.VerticalAccuracyElement, localName))
				{
					reader.ReadStartElement();
					VerticalAccuracy = reader.ReadContentAsDouble();
				}
				else if (object.Equals(MetadataXml.SpeedElement, localName))
				{
					reader.ReadStartElement();
					Speed = reader.ReadContentAsDouble();
				}
			}
		}
		while (reader.Depth != rootDepth && reader.Read());
	}

	private void BlankAllFields()
	{
		Altitude = null;
		Azimuth = null;
		GeodeticSystem = null;
		HorizontalAccuracy = null;
		Latitude = null;
		Longitude = null;
		Speed = null;
		Version = null;
		VerticalAccuracy = null;
	}

	private void ReadAttributes(XmlReader reader)
	{
		while (reader.MoveToNextAttribute())
		{
			string localName = reader.LocalName;
			if (object.Equals(MetadataXml.VersionAttribute, localName))
			{
				Version = new Version(reader.ReadContentAsString());
			}
		}
	}

	public void WriteXml(XmlWriter writer)
	{
		writer.WriteAttributeString(MetadataXml.VersionAttribute, Version.ToString(2));
		if (Latitude.HasValue)
		{
			writer.WriteElementString(MetadataXml.LatitudeElement, Latitude.Value.ToString(MetadataXml.Culture));
		}
		if (Longitude.HasValue)
		{
			writer.WriteElementString(MetadataXml.LongitudeElement, Longitude.Value.ToString(MetadataXml.Culture));
		}
		if (Altitude.HasValue)
		{
			writer.WriteElementString(MetadataXml.AltitudeElement, Altitude.Value.ToString(MetadataXml.Culture));
		}
		if (Azimuth.HasValue)
		{
			writer.WriteElementString(MetadataXml.AzimuthElement, Azimuth.Value.ToString(MetadataXml.Culture));
		}
		if (HorizontalAccuracy.HasValue)
		{
			writer.WriteElementString(MetadataXml.HorizontalAccuracyElement, HorizontalAccuracy.Value.ToString(MetadataXml.Culture));
		}
		if (VerticalAccuracy.HasValue)
		{
			writer.WriteElementString(MetadataXml.VerticalAccuracyElement, VerticalAccuracy.Value.ToString(MetadataXml.Culture));
		}
		if (Speed.HasValue)
		{
			writer.WriteElementString(MetadataXml.SpeedElement, Speed.Value.ToString(MetadataXml.Culture));
		}
		if (GeodeticSystem != null)
		{
			writer.WriteElementString(MetadataXml.GeodeticSystemElement, GeodeticSystem);
		}
	}

	public bool Equals(NavigationalData other)
	{
		if (other == null)
		{
			return false;
		}
		if (this == other)
		{
			return true;
		}
		if (Altitude.Equals(other.Altitude) && Azimuth.Equals(other.Azimuth) && string.Equals(GeodeticSystem, other.GeodeticSystem) && HorizontalAccuracy.Equals(other.HorizontalAccuracy) && Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude) && VerticalAccuracy.Equals(other.VerticalAccuracy) && Speed.Equals(other.Speed))
		{
			return object.Equals(Version, other.Version);
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj == null)
		{
			return false;
		}
		if (this == obj)
		{
			return true;
		}
		if (obj.GetType() != GetType())
		{
			return false;
		}
		return Equals((NavigationalData)obj);
	}

	public override int GetHashCode()
	{
		return (((((((((((((((Altitude.GetHashCode() * 397) ^ Azimuth.GetHashCode()) * 397) ^ ((GeodeticSystem != null) ? GeodeticSystem.GetHashCode() : 0)) * 397) ^ HorizontalAccuracy.GetHashCode()) * 397) ^ Latitude.GetHashCode()) * 397) ^ Longitude.GetHashCode()) * 397) ^ VerticalAccuracy.GetHashCode()) * 397) ^ Speed.GetHashCode()) * 397) ^ ((Version != null) ? Version.GetHashCode() : 0);
	}
}
