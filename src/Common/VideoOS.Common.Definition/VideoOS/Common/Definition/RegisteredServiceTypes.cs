using System;

namespace VideoOS.Common.Definition;

public static class RegisteredServiceTypes
{
	public static readonly Guid EventServerService = new Guid("08AB8F23-9AEF-4298-9CAA-F39259FE7CDA");

	public static readonly Guid LogService = new Guid("3D6F1153-92AD-43F1-B467-9482FFD291B2");

	public static readonly Guid ServiceChannel = new Guid("517AFD85-CDEE-4d50-A9EA-991C0801DA21");

	public static readonly Guid LicensePlateRecognitionService = new Guid("BA177CB7-5A9B-42B8-988D-CFBA91CA44E3");

	public static readonly Guid MobileService = new Guid("7510BBAE-DBA5-4C2E-B037-74A0222484C6");

	public static readonly Guid ManagementService = new Guid("2D275C3A-50BC-4435-B0AD-ED3381FEDC1D");

	public static readonly Guid RecordingService = new Guid("2B2D3601-5857-44A3-880F-BE7957B5B27E");

	public static readonly Guid FailoverService = new Guid("3C944CF2-668F-4956-9EBE-A1C3F6BD6CD4");

	public static readonly Guid GatewayService = new Guid("E46B7BF9-03CE-44EB-BBDC-8BA16D0AAA80");

	public static readonly Guid AlarmsService = new Guid("6666C134-369A-4839-82B2-83DD109EA8A4");

	[Obsolete("LegacyLogService is deprecated. Please use LogService instead.")]
	public static readonly Guid LegacyLogService = new Guid("002B1B2F-0817-4592-8C62-2FE5107C160C");
}
