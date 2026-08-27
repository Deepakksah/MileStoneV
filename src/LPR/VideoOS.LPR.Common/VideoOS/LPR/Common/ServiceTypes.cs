using System;

namespace VideoOS.LPR.Common;

public static class ServiceTypes
{
	public static readonly Guid ServiceType_EventServerService = new Guid("08AB8F23-9AEF-4298-9CAA-F39259FE7CDA");

	public static readonly Guid ServiceType_LogService = new Guid("3D6F1153-92AD-43F1-B467-9482FFD291B2");

	public static readonly Guid ServiceType_TransactService = new Guid("50CE62AA-2CDB-4eed-BB5E-6C487D41EE81");

	public static readonly Guid ServiceType_RegisteredService = new Guid("DC4E7BB6-D8C9-4179-80B9-37ED9715EE9A");

	public static readonly Guid ServiceType_ServiceChannel = new Guid("517AFD85-CDEE-4d50-A9EA-991C0801DA21");

	public static readonly Guid ServiceType_EnterpriseRecorder = new Guid("46ECECAA-5890-4B91-89B2-D071C312321F");

	public static readonly Guid ServiceType_LPRServerService = new Guid("BA177CB7-5A9B-42B8-988D-CFBA91CA44E3");

	public static readonly Guid ServiceType_GatewayService = new Guid("E46B7BF9-03CE-44EB-BBDC-8BA16D0AAA80");
}
