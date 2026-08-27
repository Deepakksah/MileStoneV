using System;

namespace VideoOS.Common.Services;

public static class ServiceTypes
{
	public static readonly Guid ServiceType_EventServerService = new Guid("08AB8F23-9AEF-4298-9CAA-F39259FE7CDA");

	[Obsolete("ServiceType_LegacyLogService is deprecated. Please use ServiceType_LogService instead.")]
	public static readonly Guid ServiceType_LegacyLogService = new Guid("002B1B2F-0817-4592-8C62-2FE5107C160C");

	public static readonly Guid ServiceType_TransactService = new Guid("50CE62AA-2CDB-4eed-BB5E-6C487D41EE81");

	public static readonly Guid ServiceType_RegisteredService = new Guid("DC4E7BB6-D8C9-4179-80B9-37ED9715EE9A");

	public static readonly Guid ServiceType_AuthorizationServer = new Guid("C42AA231-0D16-4B5F-9157-33A818D4837F");

	public static readonly Guid ServiceType_ReportService = new Guid("A9F27A6A-9488-44E3-AC0B-0CB1C86CCD2A");

	public static readonly Guid ServiceType_IncidentManagerService = new Guid("3CE6B5E2-D861-4C0B-9085-00B938B2F45A");

	public static readonly Guid ServiceType_LicensePlateRecognitionService = new Guid("BA177CB7-5A9B-42B8-988D-CFBA91CA44E3");

	public static readonly Guid ServiceType_MobileService = new Guid("7510BBAE-DBA5-4C2E-B037-74A0222484C6");

	public static readonly Guid ServiceType_OnvifService = new Guid("A94F5DFB-784D-45A6-AB6D-4862C4586057");

	public static readonly Guid ServiceType_CorporateService = new Guid("A10CD823-BB4E-4D31-A162-DD236FC78FA6");

	public static readonly Guid ServiceType_EnterpriseService = new Guid("C66FE476-863D-4015-91A7-4AA8742F57B9");

	public static readonly Guid ServiceType_ManagementService = new Guid("2D275C3A-50BC-4435-B0AD-ED3381FEDC1D");

	public static readonly Guid ServiceType_RecordingService = new Guid("2B2D3601-5857-44A3-880F-BE7957B5B27E");

	public static readonly Guid ServiceType_CentralService = new Guid("46ECECAA-5890-4B91-89B2-D071C312321F");

	public static readonly Guid ServiceType_FailoverService = new Guid("3C944CF2-668F-4956-9EBE-A1C3F6BD6CD4");

	public static readonly Guid ServiceType_LogService = new Guid("3D6F1153-92AD-43F1-B467-9482FFD291B2");

	public static readonly Guid ServiceType_GatewayService = new Guid("E46B7BF9-03CE-44EB-BBDC-8BA16D0AAA80");

	public static readonly Guid ServiceType_ProcessingServerService = new Guid("5B41B840-4DA8-4044-9AB5-B8C5E4E7C59A");

	public static readonly Guid ServiceType_AlarmsService = new Guid("6666C134-369A-4839-82B2-83DD109EA8A4");
}
