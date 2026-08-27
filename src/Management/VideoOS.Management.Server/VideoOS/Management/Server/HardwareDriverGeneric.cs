using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class HardwareDriverGeneric
{
	public static GenericObject2[] QueryRecordingServerHardwareDrivers(IGeminiRequestContext requestContext, string recordingServerId, bool used)
	{
		return ConvertToGeneric(HardwareDriver.QueryRecordingServerHardwareDrivers(requestContext, recordingServerId, used));
	}

	public static GenericObject2[] QueryFailoverServerHardwareDrivers(IGeminiRequestContext requestContext, string recordingServerId)
	{
		return ConvertToGeneric(HardwareDriver.QueryFailoverServerHardwareDrivers(requestContext, recordingServerId));
	}

	public static GenericObject2[] QueryActiveRecordingServerHardwareDrivers(IGeminiRequestContext requestContext, string recordingServerId)
	{
		return ConvertToGeneric(HardwareDriver.QueryActiveHardwareDrivers(requestContext, recordingServerId));
	}

	public static GenericObject2 QueryActiveHardwareDriver(IGeminiRequestContext requestContext, string recordingServerId, string driverId)
	{
		return ConvertToGeneric(HardwareDriver.QueryActiveHardwareDriver(requestContext, recordingServerId, driverId));
	}

	private static GenericObject2 ConvertToGeneric(HardwareDriver hardwareDriver)
	{
		if (hardwareDriver == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", hardwareDriver.Id));
		list.Add(new ObjectProperty2("Name", hardwareDriver.Name));
		list.Add(new ObjectProperty2("DriverVersion", hardwareDriver.DriverVersion));
		list.Add(new ObjectProperty2("GroupName", hardwareDriver.GroupName));
		list.Add(new ObjectProperty2("Number", hardwareDriver.Number));
		list.Add(new ObjectProperty2("UseCount", hardwareDriver.UseCount));
		list.Add(new ObjectProperty2("DriverRevision", hardwareDriver.DriverRevision));
		list.Add(new ObjectProperty2("DriverType", hardwareDriver.DriverType.ToString()));
		return new GenericObject2(hardwareDriver.Id, "HardwareDriver", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToGeneric(HardwareDriver[] hardwareDrivers)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (HardwareDriver hardwareDriver in hardwareDrivers)
		{
			list.Add(ConvertToGeneric(hardwareDriver));
		}
		return list.ToArray();
	}
}
