using System;
using System.Globalization;

namespace VideoOS.Server.Licensing;

public sealed class TemporaryDeviceLicense
{
	public DateTime ExpiryDateUtc { get; private set; }

	public string MacAddress { get; private set; }

	public string UniqueIdentifier { get; private set; }

	public Guid OriginalDeviceId { get; set; }

	public string ChangesWithoutActivationToken { get; private set; }

	public TemporaryDeviceLicense(string macAddress, string uniqueDeviceId, DateTime expiryDateUtc, string changesWithoutActivationToken)
	{
		if (expiryDateUtc.Kind != DateTimeKind.Utc)
		{
			throw new ArgumentException("Date must be UTC or max value", "expiryDateUtc");
		}
		MacAddress = macAddress;
		UniqueIdentifier = uniqueDeviceId;
		ExpiryDateUtc = expiryDateUtc;
		ChangesWithoutActivationToken = changesWithoutActivationToken ?? string.Empty;
		OriginalDeviceId = Guid.Empty;
	}

	public void SetChangesWithoutActivationToken(DateTime dateActivated)
	{
		ChangesWithoutActivationToken = TemporaryLicense.CalculateYearIndex(dateActivated, DateTime.UtcNow).ToString(CultureInfo.InvariantCulture) + "_" + DateTime.UtcNow.ToString("d", CultureInfo.InvariantCulture);
	}

	public bool IsExpired()
	{
		return DateTime.UtcNow.Date > ExpiryDateUtc.Date;
	}

	public bool IsChangedWithoutActivation()
	{
		return !string.IsNullOrWhiteSpace(ChangesWithoutActivationToken);
	}

	public void ClearChangesWithoutActivationToken()
	{
		ChangesWithoutActivationToken = string.Empty;
	}

	public bool HasCurrentYearToken(DateTime dateActivated)
	{
		if (!IsChangedWithoutActivation())
		{
			return false;
		}
		if (ChangesWithoutActivationToken.Contains("_"))
		{
			int num = Convert.ToInt32(ChangesWithoutActivationToken.Split('_')[0], CultureInfo.InvariantCulture);
			return num >= TemporaryLicense.CalculateYearIndex(dateActivated, DateTime.UtcNow);
		}
		return true;
	}
}
