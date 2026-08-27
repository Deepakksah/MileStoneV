-- This header is used by the dbupdate tool.
-- The TARGET poperty is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [dbo].[MetadataSearchProperties]
   SET [FilterId] = NULL 
   WHERE [PropertyName] = 'Colorspace'
GO