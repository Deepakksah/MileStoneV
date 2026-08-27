using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Common.Integration.LicenseInfo;

public class CustomLicenseType : ICustomLicenseType
{
	public const string FEATURE_NAME = "CustomLicenseType";

	public const string ZERO_LICENSE_TYPE_NAME = "ZeroLicenseType";

	private readonly bool _useFeatureFlag;

	private readonly ILicenseDocument _licenseDocument;

	private static ILogger licenseLogger;

	private readonly Dictionary<string, string> _cachedLicenseTypes;

	private readonly object _cacheLock = new object();

	public CustomLicenseType(ILicenseDocument licenseDocument)
	{
		_licenseDocument = licenseDocument;
		if (licenseDocument != null)
		{
			_useFeatureFlag = licenseDocument.FeatureExists("CustomLicenseType");
			licenseLogger = Telemetry.GetILogger("Licensing");
		}
		_cachedLicenseTypes = new Dictionary<string, string>();
	}

	private static CustomLicensingType ProcessZeroLicenseType(IHardwareInfo hardwareInfo, ILicenseDocument licenseDocument)
	{
		string parameterValue = null;
		bool flag = DriverNumbering.IsArculesMipDriver(hardwareInfo.DriverNumber) && licenseDocument.TryGetFeatureParameterValue("CustomLicenseType", hardwareInfo.DriverName, out parameterValue) && (CustomLicensingType)Enum.Parse(typeof(CustomLicensingType), parameterValue) == CustomLicensingType.ZeroLicenseType;
		if (hardwareInfo.EnabledCameraIndexes == null || hardwareInfo.EnabledCameraIndexes.Length == 0 || flag)
		{
			hardwareInfo.ChannelLicenseCount = 0;
			hardwareInfo.LicenseState = HardwareLicenseState.Free;
			ILogger obj = licenseLogger;
			if (obj != null)
			{
				LoggerExtensions.LogDebug(obj, "CustomLicenseType  - {HardwareInfoName} as Zero License", new object[1] { hardwareInfo.Name });
			}
			return CustomLicensingType.ZeroLicenseType;
		}
		ILogger obj2 = licenseLogger;
		if (obj2 != null)
		{
			LoggerExtensions.LogDebug(obj2, "CustomLicenseType  - {HardwareInfoName} as Unknown License", new object[1] { hardwareInfo.Name });
		}
		return CustomLicensingType.Unknown;
	}

	public CustomLicensingType CustomLicenseIsUsed(IHardwareInfo hardwareInfo)
	{
		if (!_useFeatureFlag || hardwareInfo == null || !hardwareInfo.Enabled)
		{
			return CustomLicensingType.Unknown;
		}
		string parameterValue = null;
		lock (_cacheLock)
		{
			if (!_cachedLicenseTypes.ContainsKey(hardwareInfo.DriverName))
			{
				_licenseDocument.TryGetFeatureParameterValue("CustomLicenseType", hardwareInfo.DriverName, out parameterValue);
				_cachedLicenseTypes[hardwareInfo.DriverName] = parameterValue;
			}
			parameterValue = _cachedLicenseTypes[hardwareInfo.DriverName];
		}
		if (string.IsNullOrEmpty(parameterValue))
		{
			return CustomLicensingType.Unknown;
		}
		if (parameterValue == "ZeroLicenseType")
		{
			return ProcessZeroLicenseType(hardwareInfo, _licenseDocument);
		}
		return CustomLicensingType.Unknown;
	}
}
