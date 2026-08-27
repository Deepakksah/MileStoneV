-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Change 'Listen' action on Metadata to 'Live'
UPDATE [SecurityAction]
SET [name] = 'LIVE', [display_name] = 'Live'
WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' AND name = 'LISTEN'

UPDATE [SecurityAction]
SET [name] = 'LIVE', [display_name] = 'Live'
WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D' AND name = 'VIEW_LIVE'

-- Remove 'Speak' and bookmark actions from Metadata
DELETE [SecurityAction]
WHERE [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'
	AND [name] IN ('SPEAK', 'CREATE_BOOKMARKS', 'READ_BOOKMARKS', 'EDIT_BOOKMARKS', 'DELETE_BOOKMARKS')
GO
