using System.Collections.Generic;

namespace VideoOS.Common.Integration.Drivers;

public struct FirmwareUpgradeStatus
{
	public int CompletionPercentage { get; set; }

	public string NewFirmwareVersion { get; set; }

	public int RawStatus { get; set; }

	public override bool Equals(object obj)
	{
		if (obj is FirmwareUpgradeStatus firmwareUpgradeStatus)
		{
			if (CompletionPercentage == firmwareUpgradeStatus.CompletionPercentage && NewFirmwareVersion.Equals(firmwareUpgradeStatus.NewFirmwareVersion))
			{
				return RawStatus == firmwareUpgradeStatus.RawStatus;
			}
			return false;
		}
		return false;
	}

	public static bool operator ==(FirmwareUpgradeStatus left, FirmwareUpgradeStatus right)
	{
		return left.Equals(right);
	}

	public static bool operator !=(FirmwareUpgradeStatus left, FirmwareUpgradeStatus right)
	{
		return !left.Equals(right);
	}

	public override int GetHashCode()
	{
		return ((-1393695867 * -1521134295 + CompletionPercentage.GetHashCode()) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(NewFirmwareVersion)) * -1521134295 + RawStatus.GetHashCode();
	}
}
