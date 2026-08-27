using System;
using System.Collections.Generic;

namespace VideoOS.EventServer.Shared;

public static class ServiceTypes
{
	private static IEnumerable<Guid> _alarmSupportedRegisteredServices = new List<Guid> { ServiceTypeEventServerService, ServiceTypeLicensePlateRecognitionService, ServiceTypeLogService, ServiceTypeMobileService, ServiceTypeServiceChannel };

	public static Guid ServiceTypeEventServerService => new Guid("08AB8F23-9AEF-4298-9CAA-F39259FE7CDA");

	public static Guid ServiceTypeLogService => new Guid("3D6F1153-92AD-43F1-B467-9482FFD291B2");

	public static Guid ServiceTypeTransactService => new Guid("50CE62AA-2CDB-4eed-BB5E-6C487D41EE81");

	public static Guid ServiceTypeRegisteredService => new Guid("DC4E7BB6-D8C9-4179-80B9-37ED9715EE9A");

	public static Guid ServiceTypeServiceChannel => new Guid("517AFD85-CDEE-4d50-A9EA-991C0801DA21");

	public static Guid ServiceTypeEnterpriseRecorder => new Guid("46ECECAA-5890-4B91-89B2-D071C312321F");

	public static Guid ServiceTypeReportService => new Guid("A9F27A6A-9488-44E3-AC0B-0CB1C86CCD2A");

	public static Guid ServiceTypeLicensePlateRecognitionService => new Guid("BA177CB7-5A9B-42B8-988D-CFBA91CA44E3");

	public static Guid ServiceTypeMobileService => new Guid("7510BBAE-DBA5-4C2E-B037-74A0222484C6");

	public static Guid ServiceTypeCorporateService => new Guid("A10CD823-BB4E-4D31-A162-DD236FC78FA6");

	public static Guid ServiceTypeEnterpriseService => new Guid("C66FE476-863D-4015-91A7-4AA8742F57B9");

	public static IEnumerable<Guid> AlarmSupportedServices => _alarmSupportedRegisteredServices;
}
