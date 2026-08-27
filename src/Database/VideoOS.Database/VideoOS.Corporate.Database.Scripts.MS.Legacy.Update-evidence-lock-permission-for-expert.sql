-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Removed evidence lock security action for Cameras, Microphones, Speakers and Metadata in Expert mode
UPDATE [SecurityAction]
SET visible = 0
WHERE name = 'READ_EVIDENCE_LOCK'
AND namespace_id IN ('48B602BC-E752-4BBF-8E2A-7DE01F53A6DD','15F48F88-CA89-4926-9A84-2B02864EC77A','EDE4D51C-F691-4894-9C0B-C3AE096DC04D','623D03F8-C5D5-46BC-A2F4-4C03562D4F85')

UPDATE [SecurityAction]
SET visible = 0
WHERE name = 'CREATE_EVIDENCE_LOCK'
AND namespace_id IN ('48B602BC-E752-4BBF-8E2A-7DE01F53A6DD','15F48F88-CA89-4926-9A84-2B02864EC77A','EDE4D51C-F691-4894-9C0B-C3AE096DC04D','623D03F8-C5D5-46BC-A2F4-4C03562D4F85')

UPDATE [SecurityAction]
SET visible = 0
WHERE name = 'DELETE_EVIDENCE_LOCK'
AND namespace_id IN ('48B602BC-E752-4BBF-8E2A-7DE01F53A6DD','15F48F88-CA89-4926-9A84-2B02864EC77A','EDE4D51C-F691-4894-9C0B-C3AE096DC04D','623D03F8-C5D5-46BC-A2F4-4C03562D4F85')

GO