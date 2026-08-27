using System;
using System.Globalization;

namespace VideoOS.Common.Integration.Definition;

public abstract class Hwid
{
	public abstract string Text { get; }

	public static Hwid ParseHwid(string hwid)
	{
		if (hwid == null)
		{
			throw new ArgumentNullException("hwid");
		}
		try
		{
			if (hwid.StartsWith("hardwareptz", StringComparison.Ordinal))
			{
				return new HardwarePtzHwid(hwid);
			}
			if (hwid.StartsWith("hardware", StringComparison.Ordinal))
			{
				return new HardwareHwid(hwid);
			}
			if (hwid.StartsWith("deviceptz", StringComparison.Ordinal))
			{
				return new DevicePtzHwid(hwid);
			}
			if (hwid.StartsWith("device", StringComparison.Ordinal))
			{
				return new DeviceHwid(hwid);
			}
			if (hwid.StartsWith("stream", StringComparison.Ordinal))
			{
				return new StreamHwid(hwid);
			}
			if (hwid.StartsWith("metadataitem", StringComparison.Ordinal))
			{
				return new MetadataItemHwid(hwid);
			}
			if (hwid.StartsWith("event", StringComparison.Ordinal))
			{
				return new EventHwid(hwid);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception innerException)
		{
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The hwid '{0}' could not be parsed", hwid), "hwid", innerException);
		}
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "The value '{0}' was is not a valid hwid", hwid), "hwid");
	}
}
