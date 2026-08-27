using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Manager;

public static class ProductInstallerIds
{
	public const string MainProduct = "B8D7003E-3AD5-433C-8CE2-3077D50D51C9";

	public const string RecordingServer = "73EC26F2-DD4C-41C4-B3DA-11BC015BC637";

	public const string ManagementClient = "04F65585-AA46-46D2-9D75-0C2C037411D1";

	public const string SmartClient64 = "2DAA8349-5698-4F3F-B634-F31AE3159CC6";

	public const string EventServer = "0E672E09-1D0E-4E72-AA8C-235CC5AB8FF6";

	public const string LogServer = "02D18E3D-3DC9-4063-BEDB-201454D970B2";

	public const string MobileServer = "85EC2E87-AA87-419C-B916-6D162C709A40";

	public const string DLNAServer = "044A1C75-BAA5-4DF9-8BCE-EBB606850FCF";

	public const string OnvifServer = "C2E0A95E-432F-4E81-BF2F-76574D53D104";

	public const string LPRServer = "0811DC0D-7EE0-4F33-9EED-938683780E76";

	public const string GatewayService = "6BDAD892-E5E2-4FBF-B774-527425C0C9D4";

	public const string SafeKit = "6CC1DA5E-431A-4D05-B2B0-53179EC5534B";

	[Obsolete("Set InstallerConfig/InstallerSettings/UseSharedInstallPath in the product installer's InstallerConfig.xml to true")]
	public static IEnumerable<string> ProductsWithSharedInstallFolder()
	{
		return new List<string>
		{
			"B8D7003E-3AD5-433C-8CE2-3077D50D51C9", "73EC26F2-DD4C-41C4-B3DA-11BC015BC637", "04F65585-AA46-46D2-9D75-0C2C037411D1", "2DAA8349-5698-4F3F-B634-F31AE3159CC6", "0E672E09-1D0E-4E72-AA8C-235CC5AB8FF6", "02D18E3D-3DC9-4063-BEDB-201454D970B2", "85EC2E87-AA87-419C-B916-6D162C709A40", "044A1C75-BAA5-4DF9-8BCE-EBB606850FCF", "C2E0A95E-432F-4E81-BF2F-76574D53D104", "0811DC0D-7EE0-4F33-9EED-938683780E76",
			"6CC1DA5E-431A-4D05-B2B0-53179EC5534B", "6BDAD892-E5E2-4FBF-B774-527425C0C9D4"
		};
	}
}
