-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Set the MetadataSearch GENERIC_READ SecurityAction visible flag, so the permission can be granted on all SKUs
UPDATE [dbo].[SecurityAction]
	SET [visible] = 1
	WHERE [namespace_id] = '55BF1320-0224-48C7-8F57-BD41090576C8' AND [name] = 'GENERIC_READ'

GO
