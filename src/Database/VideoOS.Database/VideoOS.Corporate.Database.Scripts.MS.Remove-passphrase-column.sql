-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[RecorderInfo]'))
DROP VIEW [dbo].[RecorderInfo]
GO

IF EXISTS(SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[FailoverInfo]'))
DROP VIEW [dbo].[FailoverInfo]
GO

IF  EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[RecordingServerService]') AND  name = N'passphrase')
BEGIN
	EXEC('ALTER TABLE [RecordingServerService] DROP COLUMN [passphrase]')
END
GO
