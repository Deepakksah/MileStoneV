using System;

namespace VideoOS.SqlUpdate.Core.Common;

[Flags]
public enum DatabaseValidationStatus
{
	Success = 1,
	Delete = 2,
	Create = 4,
	EmptyDatabase = 8,
	Upgrade = 0x10,
	VersionTooLow = 0x20,
	DatabaseTypeDowngradeNotSupported = 0x40,
	DatabaseCreationNotSupported = 0x80,
	DatabaseVersionDowngradeNotSupported = 0x100
}
