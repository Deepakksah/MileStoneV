---- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_RecordingStorageDevice_Devices]') AND parent_object_id = OBJECT_ID(N'[dbo].[RecordingStorageDevice]'))
	ALTER TABLE [dbo].[RecordingStorageDevice] DROP CONSTRAINT [FK_RecordingStorageDevice_Devices]
GO

ALTER TABLE [dbo].[RecordingStorageDevice] ADD CONSTRAINT [FK_RecordingStorageDevice_Devices] FOREIGN KEY([device_id]) REFERENCES [dbo].[Devices] ([IDDevice]) ON DELETE CASCADE
GO
