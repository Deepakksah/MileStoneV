using System;
using VideoOS.Platform.Admin;
using VideoOS.Platform.Resources;

namespace VideoOS.Platform.AccessControl.Internal;

public class AccessControlKind
{
	public static Guid AccessControlServerKind;

	public static Guid AccessControlUnitKind;

	public static Guid AccessControlEventSourceKind;

	static AccessControlKind()
	{
		AccessControlServerKind = new Guid("DBB3A246-122E-498D-A804-6363A154A127");
		AccessControlUnitKind = new Guid("F3B0A519-CE2F-4292-B637-97F93D441859");
		AccessControlEventSourceKind = new Guid("6D3BDB33-D913-470F-8784-3DD4EE94DB8C");
		Kind.DefaultTypeToCategoryTable.Add(AccessControlServerKind, VideoOS.Platform.Admin.Category.Text);
		Kind.DefaultTypeToCategoryTable.Add(AccessControlUnitKind, VideoOS.Platform.Admin.Category.Text);
		Kind.DefaultTypeToCategoryTable.Add(AccessControlEventSourceKind, VideoOS.Platform.Admin.Category.Text);
		Kind.DefaultTypeToNameTable.Add(AccessControlServerKind, Strings.AccessControlSystem);
		Kind.DefaultTypeToNameTable.Add(AccessControlUnitKind, Strings.AccessControlUnit);
		Kind.DefaultTypeToNameTable.Add(AccessControlEventSourceKind, Strings.AccessControlEventSourceKind);
	}
}
