using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Common.Integration.LicenseInfo;

public class LicenseCalculator : ILicenseCalculator
{
	private ILicenseDocument _licenseDocument;

	private IHardwareLicenseCountEvaluator _licenseCountEvaluator;

	private ICustomLicenseType _customLicenseType;

	private ILogger _licenseLogger;

	public bool CalculateLicenseRequirements(IEnumerable<IHardwareInfo> hardwareInfoList, out List<ILicenseCalculationResult> licenseCalculations)
	{
		if (_licenseCountEvaluator == null)
		{
			throw new InvalidOperationException("Initialize has not been called");
		}
		if (hardwareInfoList == null)
		{
			throw new ArgumentNullException("hardwareInfoList");
		}
		hardwareInfoList = MergeHardwareAndDevices(hardwareInfoList);
		licenseCalculations = new List<ILicenseCalculationResult>();
		int channelLicenseCount = _licenseDocument.ChannelLicenseCount;
		int num = 0;
		foreach (IHardwareInfo hardwareInfo in hardwareInfoList)
		{
			ILicenseCalculationResult licenseCalculationResult = CalculateLicenseRequirementsForHardware(hardwareInfo);
			if (licenseCalculationResult != null)
			{
				num += licenseCalculationResult.ChannelLicenses;
				licenseCalculations.Add(licenseCalculationResult);
				LoggerExtensions.LogDebug(_licenseLogger, "{HardwareInfoName} is processed with : {CalculationResultChannelLicenses}, {UsedChannelLicenseCount}/{ChannelLicenses}", new object[4] { hardwareInfo.Name, licenseCalculationResult.ChannelLicenses, num, channelLicenseCount });
			}
		}
		return num <= channelLicenseCount;
	}

	public ILicenseCalculationResult CalculateLicenseRequirementsForHardware(IHardwareInfo hardwareInfo)
	{
		ILicenseCalculationResult result = null;
		if (hardwareInfo == null || !hardwareInfo.Enabled || hardwareInfo.IsInterconnectedProduct)
		{
			return result;
		}
		if (_customLicenseType != null)
		{
			if (_customLicenseType.CustomLicenseIsUsed(hardwareInfo) != CustomLicensingType.Unknown)
			{
				LoggerExtensions.LogDebug(_licenseLogger, "{HardwareInfoName} is Zero License", new object[1] { hardwareInfo.Name });
				hardwareInfo.ChannelLicenseCount = 0;
			}
			else
			{
				_licenseCountEvaluator.UpdateLicenseCount(hardwareInfo);
			}
		}
		else
		{
			_licenseCountEvaluator.UpdateLicenseCount(hardwareInfo);
		}
		return LicenseFactory.CreateLicenseCalculationResult(hardwareInfo.MacAddress, hardwareInfo.DetectedModel ?? hardwareInfo.VendorProductId, hardwareInfo.DriverName, hardwareInfo.Name, hardwareInfo.ChannelLicenseCount);
	}

	public void Initialize(ILicenseDocument licenseDocument)
	{
		_licenseDocument = licenseDocument ?? throw new ArgumentNullException("licenseDocument");
		ILicenseWhitelist licenseWhitelist = new LicenseWhitelist(_licenseDocument.WhitelistAsXmlNode);
		_licenseCountEvaluator = new HardwareLicenseCountEvaluatorChannel(licenseWhitelist);
		_customLicenseType = new CustomLicenseType(licenseDocument);
		_licenseLogger = Telemetry.GetILogger("Licensing");
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static IEnumerable<IHardwareInfo> MergeHardwareAndDevices(IEnumerable<IHardwareInfo> hardwareList)
	{
		List<IHardwareInfo> list = new List<IHardwareInfo>();
		IHardwareDictionary hardwareDictionary = LicenseFactory.CreateNewHardwareDictionary();
		HashSet<string> hashSet = new HashSet<string>();
		foreach (IHardwareInfo hardware in hardwareList)
		{
			hardwareDictionary.AddHardware(hardware);
			if (hashSet.Add(hardware.MacAddress))
			{
				list.Add(LicenseFactory.CreateNewHardwareInfo(hardware.Id, hardware.RecorderId, hardware.Name, hardware.Enabled, hardware.MacAddress, hardware.SerialNumber, hardware.DriverName, hardware.DriverVersion, hardware.DriverNumber, hardware.DetectedModel, hardware.VendorProductId, hardware.IsInterconnectedProduct, new List<IDeviceInfo>()));
			}
		}
		Parallel.ForEach(list, delegate(IHardwareInfo hardwareInfo)
		{
			foreach (IHardwareInfo item in hardwareDictionary[hardwareInfo.MacAddress])
			{
				hardwareInfo.Devices.AddRange(item.EnabledCameras.Except(hardwareInfo.Devices));
			}
		});
		return list;
	}
}
