using System;
using System.Globalization;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration;

public static class DriverNumbering
{
	public const int MinDriverNumber = 1;

	public const int MaxDriverNumber = 59999;

	public const int MinNativeDriverNumber = 1;

	public const int MaxNativeDriverNumber = 9999;

	public const int MinTestDriverNumber = 10000;

	public const int MaxTestDriverNumber = 19999;

	public const int MinVpsDriverNumber = 20001;

	public const int MaxVpsDriverNumber = 20001;

	public const int MinDigitalDeviceNumber = 20202;

	public const int MaxDigitalDeviceNumber = 20202;

	public const int MinIcpDriverNumber = 30000;

	public const int ArculesDriverNumber = 30005;

	public const int MaxIcpDriverNumber = 39999;

	public const int MinMipDriverNumber = 50000;

	public const int MaxMipDriverNumber = 59999;

	public static bool IsNativeDriver(int driverNumber)
	{
		if (driverNumber >= 1)
		{
			return driverNumber <= 9999;
		}
		return false;
	}

	public static bool IsTestDriver(int driverNumber)
	{
		if (driverNumber >= 10000)
		{
			return driverNumber <= 19999;
		}
		return false;
	}

	public static bool IsVpsDriver(int driverNumber)
	{
		if (driverNumber >= 20001)
		{
			return driverNumber <= 20001;
		}
		return false;
	}

	public static bool IsIcpDriver(int driverNumber)
	{
		if (driverNumber >= 30000 && driverNumber <= 39999)
		{
			return !IsArculesMipDriver(driverNumber);
		}
		return false;
	}

	public static bool IsMipDriver(int driverNumber)
	{
		if (!IsArculesMipDriver(driverNumber))
		{
			if (driverNumber >= 50000)
			{
				return driverNumber <= 59999;
			}
			return false;
		}
		return true;
	}

	public static bool IsArculesMipDriver(int driverNumber)
	{
		return driverNumber == 30005;
	}

	public static bool IsMipDriverSubtype(Subtypes subtype)
	{
		if (!subtype.HasFlag(Subtypes.MipDriver))
		{
			return subtype.HasFlag(Subtypes.MipArculesDriver);
		}
		return true;
	}

	public static bool IsDigitalDeviceDriver(int driverNumber)
	{
		if (driverNumber >= 20202)
		{
			return driverNumber <= 20202;
		}
		return false;
	}

	public static DriverKind GetDriverKind(int driverNumber)
	{
		if (driverNumber < 1 || driverNumber > 59999)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Driver number must be between {0} and {1}", 1, 59999));
		}
		if (IsNativeDriver(driverNumber))
		{
			return DriverKind.NativeDriver;
		}
		if (IsTestDriver(driverNumber))
		{
			return DriverKind.TestDriver;
		}
		if (IsVpsDriver(driverNumber))
		{
			return DriverKind.VpsDriver;
		}
		if (IsIcpDriver(driverNumber))
		{
			return DriverKind.IcpDriver;
		}
		if (IsMipDriver(driverNumber))
		{
			return DriverKind.MipDriver;
		}
		return DriverKind.OtherDriver;
	}
}
