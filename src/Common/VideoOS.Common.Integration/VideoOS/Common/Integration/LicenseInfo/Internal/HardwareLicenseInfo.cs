using System;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

public sealed class HardwareLicenseInfo : IHardwareLicenseInfo
{
	private readonly IHardwareInfo _hardwareInfo;

	public DateTime LastModified { get; private set; }

	public Guid Id { get; private set; }

	public long License { get; private set; }

	public DateTime ExpireDate => new DateTime(License, DateTimeKind.Utc);

	public LicenseUpdateAction UpdateType { get; set; }

	public string UniqueIdentifier => _hardwareInfo.MacAddress;

	public HardwareLicenseInfo(IHardwareInfo hardwareInfo, DateTime lastModified, long license)
	{
		if (lastModified.Kind != DateTimeKind.Utc)
		{
			throw new ArgumentException("DateTime must be Utc", "lastModified");
		}
		_hardwareInfo = hardwareInfo ?? throw new ArgumentNullException("hardwareInfo");
		Id = _hardwareInfo.Id;
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
