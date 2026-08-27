-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[BasicInfo]') AND type in (N'U'))
BEGIN
	CREATE TABLE [BasicInfo] (
		[id] [GuidType] NOT NULL,
		[name] NVARCHAR(256) NOT NULL,
		[property] NVARCHAR(256) NULL,
		[order] NVARCHAR(50) NOT NULL
		
		CONSTRAINT [PK_BasicInfo] PRIMARY KEY NONCLUSTERED
		(
			[id] ASC
		)
	)
END
GO