-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DELETE FROM RecorderPluginDevice
WHERE [IDRecorderPlugin] = 'BCEC0397-F29C-412E-9E97-4D3591F48214'

GO