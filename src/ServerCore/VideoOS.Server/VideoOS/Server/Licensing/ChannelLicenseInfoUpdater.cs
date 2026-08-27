using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.Licensing;

public sealed class ChannelLicenseInfoUpdater : ILicenseInfoUpdater
{
	private ILicenseManager _licenseManager;

	private LicenseInformation _licenseInfo;

	private ILogger _licenseLogger;

	public ChannelLicenseInfoUpdater(ILicenseManager licenseManager, LicenseInformation licenseInfo)
	{
		_licenseManager = licenseManager;
		_licenseInfo = licenseInfo;
		_licenseLogger = Telemetry.GetILogger("Licensing");
	}

	public void UpdateLicenseInformation(Guid recorderId, IHardwareLicenseCountEvaluator hardwareLicenseCountEvaluator, ILicenseSnapshot licenseSnapshot, IHardwareInfo hardwareInfo)
	{
		if (licenseSnapshot == null)
		{
			throw new ArgumentNullException("licenseSnapshot");
		}
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		bool flag = hardwareInfo.EnabledCameras.Count() == 0;
		if (flag)
		{
			hardwareInfo.ChannelLicenseCount = 1;
		}
		bool flag2 = hardwareLicenseCountEvaluator?.UpdateLicenseCount(hardwareInfo) ?? false;
		if (flag2 || flag)
		{
			IHardwareLicenseInfo hardwareLicense = licenseSnapshot.GetHardwareLicense(hardwareInfo);
			hardwareInfo.LicenseState = ResolveLicenseState(hardwareLicense, hardwareInfo, recorderId);
			UpdateHardwareLicenseCount(hardwareInfo);
			return;
		}
		if (hardwareInfo.LicenseState == HardwareLicenseState.Unknown)
		{
			_licenseLogger.LogDebug("UpdateLicenseInformation  - {HardwareName} as Unknown License", hardwareInfo.Name);
			if (_licenseManager.IsHardwareActivated(hardwareInfo) || hardwareInfo.HasActivatedCameras)
			{
				hardwareInfo.LicenseState = HardwareLicenseState.Valid;
				_licenseLogger.LogDebug("UpdateLicenseInformation  - {HardwareName} as Valid License", hardwareInfo.Name);
			}
			else
			{
				hardwareInfo.LicenseState = HardwareLicenseState.Free;
				_licenseLogger.LogDebug("UpdateLicenseInformation  - {HardwareName} as Free License", hardwareInfo.Name);
			}
		}
		if (!hardwareInfo.IsInterconnectedProduct)
		{
			if (hardwareInfo.DriverNumber.Equals(30005))
			{
				_licenseLogger.LogDebug("UpdateLicenseInformation  - for {HardwareName} hardware", hardwareInfo.Name);
				UpdateArculesHardwareLicenseCountBasedOnDeviceLicenseState(recorderId, licenseSnapshot, hardwareInfo);
			}
			else
			{
				_licenseLogger.LogDebug("UpdateLicenseInformation  - {HardwareName} not IsInterconnectedProduct", hardwareInfo.Name);
				UpdateHardwareLicenseCountBasedOnDeviceLicenseState(recorderId, licenseSnapshot, hardwareInfo);
			}
		}
		else
		{
			_licenseLogger.LogDebug("UpdateLicenseInformation  - {HardwareName} is IsInterconnectedProduct", hardwareInfo.Name);
			UpdateICPFeedsLicenseInformation(recorderId, licenseSnapshot, hardwareInfo);
		}
	}

	private void UpdateArculesHardwareLicenseCountBasedOnDeviceLicenseState(Guid recorderId, ILicenseSnapshot licenseSnapshot, IHardwareInfo hardwareInfo)
	{
		foreach (IDeviceInfo item in hardwareInfo.Devices.Where((IDeviceInfo d) => RegisteredDeviceType.IsCamera(d.DeviceType)))
		{
			bool flag = false;
			if (_licenseManager.CountManager.IsCameraFeedLicensedOnSite(hardwareInfo, item) && !_licenseManager.CountManager.IsCameraFeedLicensedOnRecorder(hardwareInfo, item, recorderId))
			{
				item.LicenseState = DeviceInfoLicenseState.LicensedOnOtherRecordingServer;
				continue;
			}
			_licenseLogger.LogDebug("{HardwareName} is {LicenseState}", item.HardwareInfo.Name, item.LicenseState);
			switch (item.LicenseState)
			{
			case DeviceInfoLicenseState.Permanent:
				_licenseInfo.ArculesHardwareLicenses.Used++;
				break;
			case DeviceInfoLicenseState.Grace:
				_licenseInfo.ArculesHardwareLicenses.Temporary++;
				flag = true;
				break;
			case DeviceInfoLicenseState.Expired:
				_licenseInfo.ArculesHardwareLicenses.Expired++;
				flag = true;
				break;
			case DeviceInfoLicenseState.Unlicensed:
				_licenseInfo.ArculesHardwareLicenses.Missing++;
				break;
			case DeviceInfoLicenseState.ChangeWithoutActivation:
				_licenseInfo.ArculesHardwareLicenses.Replaced++;
				break;
			}
			DateTime deviceLicenseExpireDate = licenseSnapshot.GetDeviceLicenseExpireDate(item);
			if (flag && deviceLicenseExpireDate.Ticks < _licenseInfo.ArculesHardwareLicenses.FirstGraceExpirationDate.Ticks)
			{
				_licenseInfo.ArculesHardwareLicenses.FirstGraceExpirationDate = deviceLicenseExpireDate;
			}
		}
	}

	private void UpdateHardwareLicenseCount(IHardwareInfo hardwareInfo)
	{
		switch (hardwareInfo.LicenseState)
		{
		case HardwareLicenseState.Valid:
			if (hardwareInfo.DriverNumber.Equals(30005))
			{
				_licenseInfo.ArculesHardwareLicenses.Used += hardwareInfo.ChannelLicenseCount;
			}
			else
			{
				_licenseInfo.HardwareLicenses.Used += hardwareInfo.ChannelLicenseCount;
			}
			break;
		case HardwareLicenseState.Temporary:
			if (hardwareInfo.DriverNumber.Equals(30005))
			{
				_licenseInfo.ArculesHardwareLicenses.Temporary += hardwareInfo.ChannelLicenseCount;
			}
			else
			{
				_licenseInfo.HardwareLicenses.Temporary += hardwareInfo.ChannelLicenseCount;
			}
			break;
		case HardwareLicenseState.Expired:
			if (hardwareInfo.DriverNumber.Equals(30005))
			{
				_licenseInfo.ArculesHardwareLicenses.Expired += hardwareInfo.ChannelLicenseCount;
			}
			else
			{
				_licenseInfo.HardwareLicenses.Expired += hardwareInfo.ChannelLicenseCount;
			}
			break;
		case HardwareLicenseState.Missing:
			if (hardwareInfo.DriverNumber.Equals(30005))
			{
				_licenseInfo.ArculesHardwareLicenses.Missing += hardwareInfo.ChannelLicenseCount;
			}
			else
			{
				_licenseInfo.HardwareLicenses.Missing += hardwareInfo.ChannelLicenseCount;
			}
			break;
		case HardwareLicenseState.ChangesWithoutActivation:
			if (hardwareInfo.DriverNumber.Equals(30005))
			{
				_licenseInfo.ArculesHardwareLicenses.Replaced += hardwareInfo.ChannelLicenseCount;
			}
			else
			{
				_licenseInfo.HardwareLicenses.Replaced += hardwareInfo.ChannelLicenseCount;
			}
			break;
		}
		_licenseLogger.LogDebug("Used count is licensed as      {Used}", _licenseInfo.HardwareLicenses.Used);
		_licenseLogger.LogDebug("Temporary count is licensed as {Temporary}", _licenseInfo.HardwareLicenses.Temporary);
		_licenseLogger.LogDebug("Expired count is licensed as   {Expired}", _licenseInfo.HardwareLicenses.Expired);
		_licenseLogger.LogDebug("Missing count is licensed as   {Missing}", _licenseInfo.HardwareLicenses.Missing);
		_licenseLogger.LogDebug("Replaced count is licensed as  {Replaced}", _licenseInfo.HardwareLicenses.Replaced);
	}

	private void UpdateHardwareLicenseCountBasedOnDeviceLicenseState(Guid recorderId, ILicenseSnapshot licenseSnapshot, IHardwareInfo hardwareInfo)
	{
		foreach (IDeviceInfo item in hardwareInfo.Devices.Where((IDeviceInfo d) => RegisteredDeviceType.IsCamera(d.DeviceType)))
		{
			bool flag = false;
			if (_licenseManager.CountManager.IsCameraFeedLicensedOnSite(hardwareInfo, item) && !_licenseManager.CountManager.IsCameraFeedLicensedOnRecorder(hardwareInfo, item, recorderId))
			{
				item.LicenseState = DeviceInfoLicenseState.LicensedOnOtherRecordingServer;
				continue;
			}
			_licenseLogger.LogDebug("{HardwareName} is {LicenseState}", item.HardwareInfo.Name, item.LicenseState);
			switch (item.LicenseState)
			{
			case DeviceInfoLicenseState.Permanent:
				_licenseInfo.HardwareLicenses.Used++;
				break;
			case DeviceInfoLicenseState.Grace:
				_licenseInfo.HardwareLicenses.Temporary++;
				flag = true;
				break;
			case DeviceInfoLicenseState.Expired:
				_licenseInfo.HardwareLicenses.Expired++;
				flag = true;
				break;
			case DeviceInfoLicenseState.Unlicensed:
				_licenseInfo.HardwareLicenses.Missing++;
				break;
			case DeviceInfoLicenseState.ChangeWithoutActivation:
				_licenseInfo.HardwareLicenses.Replaced++;
				break;
			}
			DateTime deviceLicenseExpireDate = licenseSnapshot.GetDeviceLicenseExpireDate(item);
			if (flag && deviceLicenseExpireDate.Ticks < _licenseInfo.HardwareLicenses.FirstGraceExpirationDate.Ticks)
			{
				_licenseInfo.HardwareLicenses.FirstGraceExpirationDate = deviceLicenseExpireDate;
			}
		}
	}

	private Dictionary<string, DeviceFeedLicenseInfo> CalculateFeedLicenseCount(Guid recorderId, ILicenseSnapshot licenseSnapshot, IHardwareInfo hardwareInfo)
	{
		Dictionary<string, DeviceFeedLicenseInfo> dictionary = new Dictionary<string, DeviceFeedLicenseInfo>();
		foreach (IDeviceInfo device in hardwareInfo.Devices)
		{
			if (!device.Enabled)
			{
				continue;
			}
			if (!dictionary.TryGetValue(device.DeviceType, out var value))
			{
				value = (dictionary[device.DeviceType] = new DeviceFeedLicenseInfo(device.DeviceType));
			}
			if (!RegisteredDeviceType.IsCamera(device.DeviceType))
			{
				value.Licensed++;
				continue;
			}
			DateTime deviceLicenseExpireDate = licenseSnapshot.GetDeviceLicenseExpireDate(device);
			if (device.LicenseState != DeviceInfoLicenseState.UnlicensedDuplicate && deviceLicenseExpireDate.Ticks == 0L)
			{
				value.Unlicensed += hardwareInfo.LicenseCount;
			}
			else if (_licenseManager.CountManager.IsCameraFeedLicensedOnRecorder(hardwareInfo, device, recorderId))
			{
				if (deviceLicenseExpireDate.Ticks == DateTime.MaxValue.Ticks)
				{
					value.Licensed++;
				}
				else if (DateTime.UtcNow.Ticks < deviceLicenseExpireDate.Ticks)
				{
					value.Temporary++;
				}
				else
				{
					value.Expired++;
				}
				if (deviceLicenseExpireDate.Ticks < value.FirstGraceExpiryUtc.Ticks)
				{
					value.FirstGraceExpiryUtc = deviceLicenseExpireDate;
				}
			}
		}
		return dictionary;
	}

	private void UpdateICPFeedsLicenseInformation(Guid recorderId, ILicenseSnapshot licenseSnapshot, IHardwareInfo hardwareInfo)
	{
		Dictionary<string, DeviceFeedLicenseInfo> dictionary = CalculateFeedLicenseCount(recorderId, licenseSnapshot, hardwareInfo);
		foreach (DeviceFeedLicenseInfo feedInfo in dictionary.Values)
		{
			Collection<FeedLicenseInformation> iCPFeedLicenses = _licenseInfo.ICPFeedLicenses;
			FeedLicenseInformation feedLicenseInformation = iCPFeedLicenses.FirstOrDefault((FeedLicenseInformation f) => string.Equals(f.TypeName, feedInfo.FeedType, StringComparison.Ordinal));
			if (feedLicenseInformation == null)
			{
				feedLicenseInformation = new FeedLicenseInformation(feedInfo.FeedType);
				iCPFeedLicenses.Add(feedLicenseInformation);
			}
			feedLicenseInformation.Used += feedInfo.Licensed;
			feedLicenseInformation.Temporary += feedInfo.Temporary;
			feedLicenseInformation.Expired += feedInfo.Expired;
			feedLicenseInformation.Missing += feedInfo.Unlicensed;
			if (feedInfo.FirstGraceExpiryUtc.Ticks < _licenseInfo.HardwareLicenses.FirstGraceExpirationDate.Ticks)
			{
				_licenseInfo.HardwareLicenses.FirstGraceExpirationDate = new DateTime(feedInfo.FirstGraceExpiryUtc.Ticks, DateTimeKind.Utc);
			}
		}
	}

	private HardwareLicenseState ResolveLicenseState(IHardwareLicenseInfo licenseHardwareInfo, IHardwareInfo hardwareInfo, Guid recorderId)
	{
		HardwareLicenseState hardwareLicenseState = hardwareInfo.LicenseState;
		long num = Convert.ToInt64(licenseHardwareInfo?.License, CultureInfo.InvariantCulture);
		if (hardwareInfo.LicenseState == HardwareLicenseState.Unknown)
		{
			bool flag = hardwareInfo.EnabledCameras.Count > 0;
			hardwareLicenseState = ((!flag && _licenseManager.CountManager.IsHardwareWithoutEnabledCamerasLicensed(hardwareInfo) && !_licenseManager.CountManager.IsHardwareLicensedOnRecorder(hardwareInfo.MacAddress, recorderId)) ? HardwareLicenseState.Free : ((flag && _licenseManager.CountManager.IsHardwareLicensedOnSite(hardwareInfo.MacAddress) && !_licenseManager.CountManager.IsHardwareLicensedOnRecorder(hardwareInfo.MacAddress, recorderId)) ? HardwareLicenseState.Free : ((licenseHardwareInfo == null || licenseHardwareInfo.License == 0L) ? HardwareLicenseState.Missing : ((num == DateTime.MaxValue.Ticks) ? HardwareLicenseState.Valid : (_licenseManager.IsHardwareInCurrentYearChangesWithoutActivation(hardwareInfo.MacAddress) ? HardwareLicenseState.ChangesWithoutActivation : (_licenseManager.IsHardwareInChangesWithoutActivation(hardwareInfo.MacAddress) ? HardwareLicenseState.Free : ((DateTime.UtcNow.Date.Ticks > num) ? HardwareLicenseState.Expired : HardwareLicenseState.Temporary)))))));
		}
		if (hardwareLicenseState != HardwareLicenseState.ChangesWithoutActivation && num < _licenseInfo.HardwareLicenses.FirstGraceExpirationDate.Ticks && num > DateTime.MinValue.AddYears(2000).Ticks)
		{
			_licenseInfo.HardwareLicenses.FirstGraceExpirationDate = new DateTime(num, DateTimeKind.Utc);
		}
		return hardwareLicenseState;
	}
}
