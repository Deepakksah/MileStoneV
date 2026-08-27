-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF (EXISTS (SELECT 1 FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[dbo].[admin_update_license_values]') AND [type] IN (N'P', N'PC')))
BEGIN
	DROP PROC [dbo].[admin_update_license_values];
END
GO

IF EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.[schema_id] = ss.[schema_id] WHERE st.[name] = N'tvp_LicenseUpdateTable' AND ss.[name] = N'dbo')
BEGIN
	DROP TYPE [dbo].[tvp_LicenseUpdateTable]
END
GO

CREATE TYPE [dbo].[tvp_LicenseUpdateTable] AS TABLE (
	[Id] UNIQUEIDENTIFIER INDEX IX_TVP_LicenseUpdateTable_ID NONCLUSTERED NOT NULL,
	[UniqueId] VARCHAR(700) INDEX IX_TVP_LicenseUpdateTable_UniqueId NONCLUSTERED NOT NULL,
	[Expires] BIGINT NOT NULL
)
GO
