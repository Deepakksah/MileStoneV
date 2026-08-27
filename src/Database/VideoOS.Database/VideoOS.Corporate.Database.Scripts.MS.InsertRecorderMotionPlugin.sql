-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM dbo.RecorderPlugins WHERE [IDRecorderPlugin] = N'f020c881-72bf-4014-8504-2b4a5a658f1a')
INSERT INTO [dbo].[RecorderPlugins](LastModified, IDRecorderPlugin, Name, Description) VALUES (GETDATE(), 'f020c881-72bf-4014-8504-2b4a5a658f1a', 'VideoOS.Plugin.MotionDetection.MotionDetectionPlugin', '')
GO