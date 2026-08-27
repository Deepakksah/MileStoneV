-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityUserUpdatedCache]') AND type in (N'U'))
CREATE TABLE [SecurityUserUpdatedCache] (
    [IDUser] UNIQUEIDENTIFIER NOT NULL,
	[LastModified] DATETIME2 NOT NULL,

    CONSTRAINT [PK_SecurityUserUpdatedCache] PRIMARY KEY CLUSTERED
    (
        [IDUser] ASC
    ),
)

GO

