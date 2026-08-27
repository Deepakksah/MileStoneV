-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [RecordingStorageDevice]
   SET [prebuffer_enabled] = 0
      ,[prebuffer_seconds] = 1
 WHERE
	[prebuffer_seconds]=0
GO
