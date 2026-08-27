using System;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class ConfigurationHelper
{
	public const string ExportAviCodecName = "Export/Avi/Codec";

	public static readonly Guid ExportAviCodecId = new Guid("{4593DA92-45CA-4C71-8A9D-4C2DB33B5B19}");

	public const string ExportDirLocation = "Export/Location";

	public static readonly Guid ExportDirLocationId = new Guid("{01426533-2D74-498D-AC5F-FE9D2C67C919}");

	public const string ExportOldExports = "Export/OldExports";

	public static readonly Guid ExportOldExportsId = new Guid("{607A92C7-A484-424A-8562-963E7D75CA61}");

	public const string ExportOldExportsProcessingStatus = "Export/OldExportsProcessingStatus";

	public static readonly Guid ExportOldExportsProcessingStatusId = new Guid("{50246961-297A-4CCC-B7F3-1A252CDE38CF}");

	public const string InvestigationsMoveStatus = "Investigation/InvestigationMoveStatus";

	public static readonly Guid InvestigationsMoveStatusId = new Guid("{BCE0E3F7-0C4F-4905-AAA0-2D3C462524BE}");

	public const string LogLocation = "Log/Location";

	public static readonly Guid LogLocationId = new Guid("{39696BE0-7074-41A9-B111-DA7F6C203400}");

	public const string LogDaysToKeep = "Log/DaysToKeep";

	public static readonly Guid LogDaysToKeepId = new Guid("{AF4E1B26-CE69-49B7-8076-0CEF44B8EC9E}");
}
