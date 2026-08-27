using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class DeviceData
{
	public Guid Id { get; set; }

	public Guid HardwareId { get; set; }

	public string DeviceType { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Streams { get; set; }

	public string Shortcut { get; set; }

	public string Settings { get; set; }

	public string Presets { get; set; }

	public bool MulticastEnabled { get; set; }

	public int DeviceIndex { get; set; }

	public bool EdgestoragePlaybackEnabled { get; set; }

	public bool ManualTimeoutEnabled { get; set; }

	public int ManualTimeoutMinutes { get; set; }

	public string GisPoint { get; set; }

	public string ShortName { get; set; }

	public int Icon { get; set; }

	public double CoverageDirection { get; set; }

	public double CoverageDepth { get; set; }

	public double CoverageFieldOfView { get; set; }

	public static IEnumerable<DeviceData> QueryEnabledDevicesByRecorderAdmin(IGeminiRequestContext requestContext, Guid id)
	{
		using IDeviceDataComponent deviceDataComponent = requestContext.CreateDeviceDataComponent();
		return deviceDataComponent.QueryEnabledDevicesByRecorderAdmin(id);
	}

	public static Dictionary<Guid, IEnumerable<DeviceData>> QueryEnabledDevicesByRoleId(IGeminiRequestContext requestContext, string[] roleIds)
	{
		using IDeviceDataComponent deviceDataComponent = requestContext.CreateDeviceDataComponent();
		return deviceDataComponent.QueryEnabledDevicesByRoleId(roleIds);
	}

	public static IEnumerable<Guid> QueryDeviceIdsByHardware(IGeminiRequestContext requestContext, Guid id)
	{
		using IDeviceDataComponent deviceDataComponent = requestContext.CreateDeviceDataComponent();
		return deviceDataComponent.QueryDeviceIdsByHardware(id);
	}
}
