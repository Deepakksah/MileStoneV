using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public sealed class HardwareDriver
{
	private const string ArculesDriverName = "Arcules";

	public string Id { get; set; }

	public string Name { get; set; }

	public int Number { get; set; }

	public string GroupName { get; set; }

	public int UseCount { get; set; }

	public string DriverVersion { get; set; }

	public string DriverRevision { get; set; }

	public DriverType DriverType { get; set; }

	public static HardwareDriver[] QueryActiveHardwareDrivers(IGeminiRequestContext requestContext, string recordingServerId)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId);
		using IHardwareDriverComponent hardwareDriverComponent = requestContext.CreateHardwareDriverComponent();
		HardwareDriver[] array = hardwareDriverComponent.QueryActiveHardwareDrivers(recordingServerId);
		List<HardwareDriver> list = new List<HardwareDriver>();
		HardwareDriver[] array2 = array;
		foreach (HardwareDriver hardwareDriver in array2)
		{
			if ((DriverNumbering.IsVpsDriver(hardwareDriver.Number) && !GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "VPS")) || (DriverNumbering.IsArculesMipDriver(hardwareDriver.Number) && !GeminiApplicationCore.FeatureCacheManager.IsZeroLicensed(requestContext, "Arcules")))
			{
				continue;
			}
			if (!DriverNumbering.IsIcpDriver(hardwareDriver.Number))
			{
				list.Add(hardwareDriver);
			}
			else if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "InterconnectedProducts"))
			{
				string value = null;
				if (GeminiApplicationCore.FeatureCacheManager.FeatureParameterCount(requestContext, "InterconnectedProducts") == 0 || GeminiApplicationCore.FeatureCacheManager.TryGetFeatureParameterValue(requestContext, "InterconnectedProducts", hardwareDriver.Number.ToString(CultureInfo.InvariantCulture), out value))
				{
					list.Add(hardwareDriver);
				}
			}
		}
		return list.ToArray();
	}

	public static HardwareDriver QueryActiveHardwareDriver(IGeminiRequestContext requestContext, string recordingServerId, string driverId)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref driverId, "driverId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId);
		using IHardwareDriverComponent hardwareDriverComponent = requestContext.CreateHardwareDriverComponent();
		HardwareDriver hardwareDriver = hardwareDriverComponent.QueryActiveHardwareDriver(recordingServerId, driverId);
		if (DriverNumbering.IsIcpDriver(hardwareDriver.Number))
		{
			if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "InterconnectedProducts"))
			{
				throw new HardwareDriverNotFoundException();
			}
			string value = null;
			if (!GeminiApplicationCore.FeatureCacheManager.TryGetFeatureParameterValue(requestContext, "InterconnectedProducts", hardwareDriver.Number.ToString(CultureInfo.InvariantCulture), out value))
			{
				throw new HardwareDriverNotFoundException();
			}
		}
		return hardwareDriver;
	}

	public static HardwareDriver[] QueryRecordingServerHardwareDrivers(IGeminiRequestContext requestContext, string recordingServerId, bool usedOnly)
	{
		Validation.CheckObjectId(ref recordingServerId, "recordingServerId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recordingServerId);
		using IHardwareDriverComponent hardwareDriverComponent = requestContext.CreateHardwareDriverComponent();
		HardwareDriver[] source = hardwareDriverComponent.QueryRecorderHardwareDrivers(recordingServerId, usedOnly);
		bool icpIsLicensed = GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "InterconnectedProducts");
		bool vpsIsLicensed = GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "VPS");
		bool arculesIsLicensed = GeminiApplicationCore.FeatureCacheManager.IsZeroLicensed(requestContext, "Arcules");
		return source.Where((HardwareDriver driver) => (icpIsLicensed || !DriverNumbering.IsIcpDriver(driver.Number)) && (vpsIsLicensed || !DriverNumbering.IsVpsDriver(driver.Number)) && (arculesIsLicensed || !DriverNumbering.IsArculesMipDriver(driver.Number))).ToArray();
	}

	public static HardwareDriver[] QueryFailoverServerHardwareDrivers(IGeminiRequestContext requestContext, string failoverServerId)
	{
		Validation.CheckObjectId(ref failoverServerId, "failoverServerId", allowNullOrEmpty: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IHardwareDriverComponent hardwareDriverComponent = requestContext.CreateHardwareDriverComponent();
		HardwareDriver[] array = hardwareDriverComponent.QueryFailoverServerHardwareDrivers(failoverServerId);
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "InterconnectedProducts"))
		{
			return array.Where((HardwareDriver driver) => !DriverNumbering.IsIcpDriver(driver.Number)).ToArray();
		}
		return array;
	}
}
