-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'position' and Object_ID = Object_ID(N'SystemDashboardConfiguration'))
BEGIN
	ALTER TABLE [SystemDashboardConfiguration]
	ADD [position] INT NOT NULL DEFAULT 2147483647 
END
GO
