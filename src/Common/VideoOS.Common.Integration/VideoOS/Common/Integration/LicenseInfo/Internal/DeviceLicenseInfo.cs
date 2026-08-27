using System;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

public sealed class DeviceLicenseInfo : IDeviceLicenseInfo
{
	private readonly IDeviceInfo _deviceInfo;

	public DateTime LastModified { get; private set; }

	public Guid Id { get; private set; }

	public string UniqueIdentifier => _deviceInfo.UniqueIdentifier;

	public long License { get; private set; }

	public DateTime Expires => new DateTime(License, DateTimeKind.Utc);

	public LicenseUpdateAction UpdateType { get; set; }

	public DeviceLicenseInfo(IDeviceInfo deviceInfo, DateTime lastModified, long license)
	{
		if (lastModified.Kind != DateTimeKind.Utc)
		{
			throw new ArgumentException("DateTime must be Utc", "lastModified");
		}
		_deviceInfo = deviceInfo ?? throw new ArgumentNullException("deviceInfo");
		Id = deviceInfo.Id;
		LastModified = lastModified;
		License = license;
		UpdateType = LicenseUpdateAction.None;
	}

	public bool UpdateExpireDate(DateTime expireDate)
	{
		if (expireDate.Equals(DateTime.MinValue))
		{
			throw new ArgumentOutOfRangeException("expireDate");
		}
		if (expireDate.Kind != DateTimeKind.Utc)
		{
			throw new ArgumentException("Date must be Utc", "expireDate");
		}
		if (License != expireDate.Ticks)
		{
			License = expireDate.Ticks;
			return true;
		}
		return false;
	}
}
