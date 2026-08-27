using System;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

public class LocationInfo
{
	private delegate uint GetValue(double source);

	private delegate void SetMask(BinaryFrameHeaderHelper.LocationMembersFlags flags);

	public const uint HorizontalDimentionFactor = 10000000u;

	public const uint MtoCmFactor = 100u;

	public BinaryFrameHeaderHelper.LocationSourceFlags Source { get; set; }

	public double? Longitude { get; set; }

	public double? Latitude { get; set; }

	public double? Altitude { get; set; }

	public double? HorizontalAccuracy { get; set; }

	public double? VerticalAccuracy { get; set; }

	public double? Azimuth { get; set; }

	public double? Speed { get; set; }

	public LocationInfo()
	{
		InitDefaultValues();
	}

	public LocationInfo(HeaderExtensionLocationInfo locationInfoExtension)
	{
		InitFrom(locationInfoExtension);
	}

	public void InitFrom(HeaderExtensionLocationInfo locationInfoExtension)
	{
		InitDefaultValues();
		Source = (BinaryFrameHeaderHelper.LocationSourceFlags)locationInfoExtension.Source;
		if (HeaderExtensionLocationInfoExtensions.CheckForMask(ref locationInfoExtension, BinaryFrameHeaderHelper.LocationMembersFlags.Longitude))
		{
			Longitude = VideoOS.Mobile.Portable.Utilities.Math.PutInRange(-180.0, 180.0, (double)locationInfoExtension.Longitude / 10000000.0);
		}
		if (HeaderExtensionLocationInfoExtensions.CheckForMask(ref locationInfoExtension, BinaryFrameHeaderHelper.LocationMembersFlags.Latitude))
		{
			Latitude = VideoOS.Mobile.Portable.Utilities.Math.PutInRange(-90.0, 90.0, (double)locationInfoExtension.Latitude / 10000000.0);
		}
		if (HeaderExtensionLocationInfoExtensions.CheckForMask(ref locationInfoExtension, BinaryFrameHeaderHelper.LocationMembersFlags.Altitude))
		{
			Altitude = (double)locationInfoExtension.Altitude / 100.0;
		}
		if (HeaderExtensionLocationInfoExtensions.CheckForMask(ref locationInfoExtension, BinaryFrameHeaderHelper.LocationMembersFlags.HorizontalAccuracy))
		{
			HorizontalAccuracy = (double)locationInfoExtension.HorizontalAccuracy / 100.0;
		}
		if (HeaderExtensionLocationInfoExtensions.CheckForMask(ref locationInfoExtension, BinaryFrameHeaderHelper.LocationMembersFlags.VerticalAccuracy))
		{
			VerticalAccuracy = (double)locationInfoExtension.VerticalAccuracy / 100.0;
		}
		if (HeaderExtensionLocationInfoExtensions.CheckForMask(ref locationInfoExtension, BinaryFrameHeaderHelper.LocationMembersFlags.Azimuth))
		{
			Azimuth = VideoOS.Mobile.Portable.Utilities.Math.PutInRange(-180.0, 180.0, (double)locationInfoExtension.Azimuth / 10000000.0);
		}
		if (HeaderExtensionLocationInfoExtensions.CheckForMask(ref locationInfoExtension, BinaryFrameHeaderHelper.LocationMembersFlags.Speed))
		{
			Speed = (double)locationInfoExtension.Speed / 100.0;
		}
	}

	public void InitDefaultValues()
	{
		Source = BinaryFrameHeaderHelper.LocationSourceFlags.Unknown;
		Longitude = null;
		Latitude = null;
		Altitude = null;
		HorizontalAccuracy = null;
		VerticalAccuracy = null;
		Azimuth = null;
		Speed = null;
	}

	public HeaderExtensionLocationInfo ConvertTo()
	{
		HeaderExtensionLocationInfo result = default(HeaderExtensionLocationInfo);
		result.Length = 48u;
		result.Version = 0u;
		result.Source = (uint)Source;
		result.Reserved = 0u;
		SetMask setMask = delegate(BinaryFrameHeaderHelper.LocationMembersFlags flags)
		{
			HeaderExtensionLocationInfoExtensions.SetMask(ref result, flags);
		};
		CheckSet(ref result.Longitude, Longitude, BinaryFrameHeaderHelper.LocationMembersFlags.Longitude, (double value) => (uint)(VideoOS.Mobile.Portable.Utilities.Math.PutInRange(0.0, 360.0, value) * 10000000.0), setMask);
		CheckSet(ref result.Latitude, Latitude, BinaryFrameHeaderHelper.LocationMembersFlags.Latitude, (double value) => (uint)(VideoOS.Mobile.Portable.Utilities.Math.PutInRange(0.0, 180.0, value) * 10000000.0), setMask);
		CheckSet(ref result.Altitude, Altitude, BinaryFrameHeaderHelper.LocationMembersFlags.Altitude, (double value) => (uint)System.Math.Round(value * 100.0), setMask);
		CheckSet(ref result.HorizontalAccuracy, HorizontalAccuracy, BinaryFrameHeaderHelper.LocationMembersFlags.HorizontalAccuracy, (double value) => (uint)System.Math.Round(value * 100.0), setMask);
		CheckSet(ref result.VerticalAccuracy, VerticalAccuracy, BinaryFrameHeaderHelper.LocationMembersFlags.VerticalAccuracy, (double value) => (uint)System.Math.Round(value * 100.0), setMask);
		CheckSet(ref result.Azimuth, Azimuth, BinaryFrameHeaderHelper.LocationMembersFlags.Azimuth, (double value) => (uint)(VideoOS.Mobile.Portable.Utilities.Math.PutInRange(0.0, 360.0, value) * 10000000.0), setMask);
		CheckSet(ref result.Speed, Speed, BinaryFrameHeaderHelper.LocationMembersFlags.Speed, (double value) => (uint)System.Math.Round(value * 100.0), setMask);
		return result;
	}

	private void CheckSet(ref uint fieldToSet, double? property, BinaryFrameHeaderHelper.LocationMembersFlags flag, GetValue getValue, SetMask setMask)
	{
		if (property.HasValue)
		{
			fieldToSet = getValue(property.Value);
			setMask(flag);
		}
	}
}
