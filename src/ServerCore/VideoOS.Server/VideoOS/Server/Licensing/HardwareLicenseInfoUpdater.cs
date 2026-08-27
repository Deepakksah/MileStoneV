using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Server.Licensing;

public sealed class HardwareLicenseInfoUpdater : ILicenseInfoUpdater
{
	private ILicenseManager _licenseManager;

	private LicenseInformation _licenseInfo;

	public HardwareLicenseInfoUpdater(ILicenseManager licenseManager, LicenseInformation licenseInfo)
	{
		_licenseManager = licenseManager;
		_licenseInfo = licenseInfo;
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
		hardwareLicenseCountEvaluator?.UpdateLicenseCount(hardwareInfo);
		IHardwareLicenseInfo hardwareLicense = licenseSnapshot.GetHardwareLicense(hardwareInfo);
		hardwareInfo.LicenseState = ResolveLicenseState(hardwareLicense, hardwareInfo, recorderId);
		IncrementLicenseCount(hardwareInfo);
		UpdateFeedsLicenseInformation(recorderId, licenseSnapshot, hardwareInfo);
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
			IDeviceLicenseInfo deviceLicense = licenseSnapshot.GetDeviceLicense(device);
			if (deviceLicense == null || deviceLicense.License == 0L)
			{
				value.Unlicensed += hardwareInfo.LicenseCount;
			}
			else if (_licenseManager.CountManager.IsCameraFeedLicensedOnRecorder(hardwareInfo, device, recorderId))
			{
				long num = Convert.ToInt64(deviceLicense.License, CultureInfo.InvariantCulture);
				if (num == DateTime.MaxValue.Ticks)
				{
					value.Licensed++;
				}
				else if (DateTime.UtcNow.Ticks < num)
				{
					value.Temporary++;
				}
				else
				{
					value.Expired++;
				}
				if (num < value.FirstGraceExpiryUtc.Ticks)
				{
					value.FirstGraceExpiryUtc = new DateTime(num, DateTimeKind.Utc);
				}
			}
		}
		return dictionary;
	}

	private void IncrementLicenseCount(IHardwareInfo hardwareInfo)
	{
		if (!hardwareInfo.IsInterconnectedProduct)
		{
			switch (hardwareInfo.LicenseState)
			{
			case HardwareLicenseState.Valid:
				_licenseInfo.HardwareLicenses.Used++;
				break;
			case HardwareLicenseState.Temporary:
				_licenseInfo.HardwareLicenses.Temporary++;
				break;
			case HardwareLicenseState.Expired:
				_licenseInfo.HardwareLicenses.Expired++;
				break;
			case HardwareLicenseState.ChangesWithoutActivation:
				_licenseInfo.HardwareLicenses.Replaced++;
				break;
			case HardwareLicenseState.Missing:
				_licenseInfo.HardwareLicenses.Missing++;
				break;
			case HardwareLicenseState.Free:
				break;
			}
		}
	}

	private HardwareLicenseState ResolveLicenseState(IHardwareLicenseInfo hardwareLicenseInfo, IHardwareInfo hardwareInfo, Guid recorderId)
	{
		HardwareLicenseState hardwareLicenseState;
		if (hardwareLicenseInfo == null || hardwareLicenseInfo.License == 0L)
		{
			hardwareLicenseState = HardwareLicenseState.Missing;
		}
		else if (_licenseManager.CountManager.IsHardwareLicensedOnSite(hardwareInfo.MacAddress) && !_licenseManager.CountManager.IsHardwareLicensedOnRecorder(hardwareInfo.MacAddress, recorderId))
		{
			hardwareLicenseState = HardwareLicenseState.Free;
		}
		else
		{
			long num = Convert.ToInt64(hardwareLicenseInfo.License, CultureInfo.InvariantCulture);
			hardwareLicenseState = ((num == DateTime.MaxValue.Ticks) ? HardwareLicenseState.Valid : (_licenseManager.IsHardwareInCurrentYearChangesWithoutActivation(hardwareInfo.MacAddress) ? HardwareLicenseState.ChangesWithoutActivation : (_licenseManager.IsHardwareInChangesWithoutActivation(hardwareInfo.MacAddress) ? HardwareLicenseState.Free : ((DateTime.UtcNow.Date.Ticks > num) ? HardwareLicenseState.Expired : HardwareLicenseState.Temporary))));
			if (hardwareLicenseState != HardwareLicenseState.ChangesWithoutActivation && num < _licenseInfo.HardwareLicenses.FirstGraceExpirationDate.Ticks)
			{
				_licenseInfo.HardwareLicenses.FirstGraceExpirationDate = new DateTime(num, DateTimeKind.Utc);
			}
		}
		return hardwareLicenseState;
	}

	private void UpdateFeedsLicenseInformation(Guid recorderId, ILicenseSnapshot licenseSnapshot, IHardwareInfo hardwareInfo)
	{
		Dictionary<string, DeviceFeedLicenseInfo> dictionary = CalculateFeedLicenseCount(recorderId, licenseSnapshot, hardwareInfo);
		foreach (DeviceFeedLicenseInfo feedInfo in dictionary.Values)
		{
			Collection<FeedLicenseInformation> collection = (hardwareInfo.IsInterconnectedProduct ? _licenseInfo.ICPFeedLicenses : _licenseInfo.FeedLicenses);
			FeedLicenseInformation feedLicenseInformation = collection.FirstOrDefault((FeedLicenseInformation f) => string.Equals(f.TypeName, feedInfo.FeedType, StringComparison.Ordinal));
			if (feedLicenseInformation == null)
			{
				feedLicenseInformation = new FeedLicenseInformation(feedInfo.FeedType);
				collection.Add(feedLicenseInformation);
			}
			switch (hardwareInfo.LicenseState)
			{
			case HardwareLicenseState.Free:
			{
				long hardwareLicenseValue = _licenseManager.CountManager.GetHardwareLicenseValue(hardwareInfo.MacAddress);
				if (hardwareLicenseValue == DateTime.MaxValue.Ticks)
				{
					goto case HardwareLicenseState.Valid;
				}
				if (hardwareLicenseValue != 0L)
				{
					if (hardwareLicenseValue > DateTime.UtcNow.Ticks)
					{
						goto case HardwareLicenseState.Temporary;
					}
					goto case HardwareLicenseState.Expired;
				}
				goto case HardwareLicenseState.Missing;
			}
			case HardwareLicenseState.Valid:
			case HardwareLicenseState.ChangesWithoutActivation:
				feedLicenseInformation.Used += feedInfo.Licensed;
				feedLicenseInformation.Temporary += feedInfo.Temporary;
				feedLicenseInformation.Expired += feedInfo.Expired;
				feedLicenseInformation.Missing += feedInfo.Unlicensed;
				break;
			case HardwareLicenseState.Temporary:
				feedLicenseInformation.Temporary += feedInfo.Licensed;
				feedLicenseInformation.Temporary += feedInfo.Temporary;
				feedLicenseInformation.Expired += feedInfo.Expired;
				feedLicenseInformation.Missing += feedInfo.Unlicensed;
				break;
			case HardwareLicenseState.Expired:
				feedLicenseInformation.Expired += feedInfo.Licensed;
				feedLicenseInformation.Expired += feedInfo.Temporary;
				feedLicenseInformation.Expired += feedInfo.Expired;
				feedLicenseInformation.Missing += feedInfo.Unlicensed;
				break;
			case HardwareLicenseState.Missing:
				feedLicenseInformation.Missing += feedInfo.Licensed;
				feedLicenseInformation.Missing += feedInfo.Temporary;
				feedLicenseInformation.Missing += feedInfo.Expired;
				feedLicenseInformation.Missing += feedInfo.Unlicensed;
				break;
			default:
				throw new NotImplementedException("Missing support for hardware license state. State: " + hardwareInfo.LicenseState);
			}
			if (feedInfo.FirstGraceExpiryUtc.Ticks < _licenseInfo.HardwareLicenses.FirstGraceExpirationDate.Ticks)
			{
				_licenseInfo.HardwareLicenses.FirstGraceExpirationDate = new DateTime(feedInfo.FirstGraceExpiryUtc.Ticks, DateTimeKind.Utc);
			}
		}
	}
}
