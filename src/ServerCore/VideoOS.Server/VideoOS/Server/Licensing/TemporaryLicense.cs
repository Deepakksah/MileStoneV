using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace VideoOS.Server.Licensing;

public sealed class TemporaryLicense : LicenseBase
{
	public DateTime ExpiryDateUtc { get; set; }

	public string ChangesWithoutActivationToken { get; private set; }

	public override string Type => "ffac8a74-85c4-4ce9-8099-8ce7a790387d";

	public TemporaryLicense(string macAddress, DateTime expiryDateUtc, string replacementToken)
		: base(macAddress)
	{
		if (expiryDateUtc.Kind != DateTimeKind.Utc)
		{
			throw new ArgumentException("Date must be UTC or max value", "expiryDateUtc");
		}
		ExpiryDateUtc = expiryDateUtc.Date;
		if (string.IsNullOrWhiteSpace(replacementToken))
		{
			ChangesWithoutActivationToken = string.Empty;
		}
		else
		{
			ChangesWithoutActivationToken = replacementToken;
		}
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

	public void SetChangesWithoutActivationToken(DateTime dateActivated)
	{
		ChangesWithoutActivationToken = CalculateYearIndex(dateActivated, DateTime.UtcNow).ToString(CultureInfo.InvariantCulture) + "_" + DateTime.UtcNow.ToString("d", CultureInfo.InvariantCulture);
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
			return num >= CalculateYearIndex(dateActivated, DateTime.UtcNow);
		}
		return true;
	}

	public static int CalculateYearIndex(DateTime dateActivated, DateTime dateAsked)
	{
		int num = dateAsked.Year - dateActivated.Year - 1;
		if (num < 0)
		{
			return 0;
		}
		int num2 = dateAsked.Month - dateActivated.Month;
		if (num2 == 0)
		{
			int num3 = dateAsked.Day - dateActivated.Day;
			if (num3 >= 0)
			{
				num++;
			}
		}
		else if (num2 > 0)
		{
			num++;
		}
		return num;
	}

	public override void WriteLicenseDetails(XmlTextWriter xmlTextWriter)
	{
		if (xmlTextWriter == null)
		{
			throw new ArgumentNullException("xmlTextWriter");
		}
		string s = Convert.ToString(ExpiryDateUtc.Ticks, CultureInfo.InvariantCulture);
		string value = Convert.ToBase64String(Encoding.Unicode.GetBytes(s));
		xmlTextWriter.WriteElementString("dlk", value);
		string value2 = Convert.ToBase64String(Encoding.Unicode.GetBytes(ChangesWithoutActivationToken));
		xmlTextWriter.WriteElementString("replacementToken", value2);
	}
}
