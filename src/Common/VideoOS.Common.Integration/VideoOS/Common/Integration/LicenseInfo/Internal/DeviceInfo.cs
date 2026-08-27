using System;
using System.Security.Cryptography;
using System.Text;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

public class DeviceInfo : IDeviceInfo
{
	public Guid Id { get; private set; }

	public Guid OriginalDeviceId { get; private set; }

	public string DeviceType { get; private set; }

	public bool Enabled { get; private set; }

	public DeviceInfoLicenseState LicenseState { get; set; }

	public string UniqueIdentifier { get; private set; }

	public int DeviceIndex { get; private set; }

	public IHardwareInfo HardwareInfo { get; }

	public DeviceInfo(Guid id, Guid originalDeviceId, string deviceType, int deviceIndex, bool enabled, IHardwareInfo hardwareInfo)
	{
		if (string.IsNullOrEmpty(deviceType))
		{
			throw new ArgumentNullException("deviceType");
		}
		if (id.Equals(Guid.Empty))
		{
			throw new ArgumentException("id can not be empty", "id");
		}
		if (originalDeviceId.Equals(Guid.Empty))
		{
			throw new ArgumentException("originalDeviceId can not be empty", "id");
		}
		HardwareInfo = hardwareInfo ?? throw new ArgumentNullException("hardwareInfo");
		Id = id;
		OriginalDeviceId = originalDeviceId;
		DeviceType = deviceType;
		Enabled = enabled;
		DeviceIndex = deviceIndex;
		LicenseState = DeviceInfoLicenseState.Unknown;
		UpdateUniqueIdentifier();
	}

	public override int GetHashCode()
	{
		return UniqueIdentifier.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		DeviceInfo deviceInfo = obj as DeviceInfo;
		if (obj == null || deviceInfo == null)
		{
			return false;
		}
		return UniqueIdentifier.Equals(deviceInfo.UniqueIdentifier, StringComparison.Ordinal);
	}

	private void UpdateUniqueIdentifier()
	{
		if (HardwareInfo.IsInterconnectedProduct)
		{
			UniqueIdentifier = OriginalDeviceId.ToString();
			return;
		}
		string text = HardwareInfo.MacAddress;
		if (string.IsNullOrEmpty(text))
		{
			text = MacAddressUtilities.EmptyMacAddress;
		}
		UniqueIdentifier = CreateDeviceHash(text, DeviceType, DeviceIndex);
	}

	private static string CreateDeviceHash(string macAddress, string deviceType, int deviceIndex)
	{
		byte[] bytes = Encoding.UTF8.GetBytes(FormattableString.Invariant($"{macAddress}:{deviceType}:{deviceIndex}"));
		using HashAlgorithm hashAlgorithm = SHA256.Create();
		return Convert.ToBase64String(hashAlgorithm.ComputeHash(bytes));
	}
}
