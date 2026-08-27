-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorage]') AND name = N'encryption_method')
BEGIN
	ALTER TABLE [RecordingStorage]
	ADD [encryption_method] [InternalIdType] NOT NULL DEFAULT ('None')
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorage]') AND name = N'encryption_password')
BEGIN
	ALTER TABLE [RecordingStorage]
	ADD [encryption_password] NVARCHAR(MAX) NOT NULL DEFAULT ('') 
END
GO

IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[RecordingStorage]') AND name = N'encryption_password_new')
BEGIN
	ALTER TABLE [RecordingStorage]
	ADD [encryption_password_new] NVARCHAR(MAX) NOT NULL DEFAULT ('')
END
GO
