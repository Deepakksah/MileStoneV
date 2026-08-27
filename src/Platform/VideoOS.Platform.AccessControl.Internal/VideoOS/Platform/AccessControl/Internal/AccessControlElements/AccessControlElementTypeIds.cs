using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlElements;

public class AccessControlElementTypeIds
{
	public static Guid AccessControlServerId = new Guid("DBB3A246-122E-498D-A804-6363A154A127");

	public static Guid AccessControlServerTypeId = new Guid("ED4A578E-8C02-41B2-AFA4-C9B3941A2CE2");

	public static Guid AccessControlUnitId = new Guid("F3B0A519-CE2F-4292-B637-97F93D441859");

	public static Guid AccessControlUnitTypeId = new Guid("0065ACC0-9588-49F9-B31B-34F2170DB1EF");

	public static Guid AccessControlCommandTypeId = new Guid("5D0D7644-0FDA-45D2-BC7C-C9C23E8B204E");

	public static Guid AccessControlEventTypeId = new Guid("674A37BE-A2ED-4E63-9C60-077E3DD946D7");

	public static Guid AccessControlStateTypeId = new Guid("BA01AE3E-92C5-4239-A948-285B857F689E");

	public static ReadOnlyCollection<Guid> AllAccessControlTypeIds => new List<Guid>(new Guid[7] { AccessControlServerId, AccessControlServerTypeId, AccessControlUnitId, AccessControlUnitTypeId, AccessControlCommandTypeId, AccessControlEventTypeId, AccessControlStateTypeId }).AsReadOnly();
}
