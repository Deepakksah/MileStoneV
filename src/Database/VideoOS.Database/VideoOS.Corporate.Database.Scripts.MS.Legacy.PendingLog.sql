-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LogPending]') AND type in (N'U'))
BEGIN
	CREATE TABLE [LogPending] (
		[session_id] GuidType NOT NULL,
		[log_id] int NOT NULL,
		[params] XML NOT NULL,
		[TimeStamp] DateTime NOT NULL DEFAULT GETDATE()
	)
END
GO
