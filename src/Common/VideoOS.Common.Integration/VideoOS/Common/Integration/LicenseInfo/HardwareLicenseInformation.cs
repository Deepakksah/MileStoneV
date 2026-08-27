using System;

namespace VideoOS.Common.Integration.LicenseInfo;

public class HardwareLicenseInformation
{
	public int Available { get; set; }

	public int Used { get; set; }

	public int Temporary { get; set; }

	public int Expired { get; set; }

	public int Replaced { get; set; }

	public int Missing { get; set; }

	public int TotalReplacementLimit { get; set; }

	public DateTime FirstGraceExpirationDate { get; set; }

	public HardwareLicenseInformation()
	{
		FirstGraceExpirationDate = DateTime.MaxValue;
	}
}
