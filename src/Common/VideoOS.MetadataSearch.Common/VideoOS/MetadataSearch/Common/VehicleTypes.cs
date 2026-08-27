using System;
using System.Collections.Generic;
using VideoOS.MetadataSearch.Common.Resources;

namespace VideoOS.MetadataSearch.Common;

public static class VehicleTypes
{
	public static readonly Guid Bus = new Guid("20B19E23-6E48-4207-B033-1FDF3B48F167");

	public static readonly Guid Car = new Guid("FF222F02-F1AF-4DCB-B93B-272E36ED4050");

	public static readonly Guid Truck = new Guid("F7DDDEBB-09F7-4A07-AAA3-53C00FC6C151");

	public static readonly Guid Bicycle = new Guid("4D4FE47D-7A6A-47AC-9653-4316D92000F6");

	public static readonly Guid Motorcycle = new Guid("E3E361EA-8750-4B83-B503-D78DDACD1FE0");

	public static readonly Guid Bike = new Guid("B6F3114C-89BA-460F-BA99-CA7F3BFBE52B");

	public const string VehicleTypeBus = "Bus";

	public const string VehicleTypeCar = "Car";

	public const string VehicleTypeTruck = "Truck";

	public const string VehicleTypeBicycle = "Bicycle";

	public const string VehicleTypeMotorcycle = "Motorcycle";

	public const string VehicleTypeBike = "Bike";

	private static IReadOnlyDictionary<Guid, string> VehicleTypeStrings = new Dictionary<Guid, string>
	{
		{
			Bus,
			Strings.BusSearchFilterItem
		},
		{
			Car,
			Strings.CarSearchFilterItem
		},
		{
			Truck,
			Strings.TruckSearchFilterItem
		},
		{
			Bicycle,
			Strings.BicycleSearchFilterItem
		},
		{
			Motorcycle,
			Strings.MotorcycleSearchFilterItem
		},
		{
			Bike,
			Strings.BikeSearchFilterItem
		}
	};

	public static string GetValue(Guid vehicleTypeId)
	{
		try
		{
			return VehicleTypeStrings[vehicleTypeId];
		}
		catch (KeyNotFoundException innerException)
		{
			throw new InvalidOperationException("No vehicle type found for the ID", innerException);
		}
	}
}
