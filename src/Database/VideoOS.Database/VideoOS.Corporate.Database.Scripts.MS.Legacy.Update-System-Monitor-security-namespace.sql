  -- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [dbo].[SecurityNamespace] 
SET display_name = 'System Monitors'
WHERE namespace_id = '2D4048FA-41DA-435A-A800-2419137E4A3E'
GO