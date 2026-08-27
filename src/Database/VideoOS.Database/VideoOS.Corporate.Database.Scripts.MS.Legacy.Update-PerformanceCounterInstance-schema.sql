-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================


IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'is_obsolete' and Object_ID = Object_ID(N'PerformanceCounterInstance'))
BEGIN
	ALTER TABLE [PerformanceCounterInstance]
	ADD [is_obsolete] BIT NOT NULL DEFAULT 0
END
GO
