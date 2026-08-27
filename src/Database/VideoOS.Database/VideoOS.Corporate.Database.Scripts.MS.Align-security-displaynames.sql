-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Security groups
UPDATE [SecurityNamespace]
SET [display_name] = 'System monitor'
WHERE [namespace_id] = '2D4048FA-41DA-435A-A800-2419137E4A3E'

UPDATE [SecurityNamespace]
SET [display_name] = 'Alarms'
WHERE [namespace_id] = '46BB0392-FD06-465B-8776-B90AC16639B3'

UPDATE [SecurityNamespace]
SET [display_name] = 'Transaction sources'
WHERE [namespace_id] = '9A1492D1-FCA0-44D7-BA9C-3699C99063A7'

UPDATE [SecurityNamespace]
SET [display_name] = 'Transaction definitions'
WHERE [namespace_id] = 'B4EE79CC-7D0B-449F-A53A-EBF0A3D1F14B'

UPDATE [SecurityNamespace]
SET [display_name] = 'Generic Events'
WHERE [namespace_id] = 'D4D19C01-03F2-4AC6-9D2B-5356B5DE62F1'

UPDATE [SecurityNamespace]
SET [display_name] = 'Recording Servers'
WHERE [namespace_id] = '9710735D-EB10-44EA-888D-FA8AB05A6433'

UPDATE [SecurityNamespace]
SET [display_name] = 'Failover Servers'
WHERE [namespace_id] = '6339122C-04F5-41F2-B3E3-3555997CFCEF'

UPDATE [SecurityNamespace]
SET [display_name] = 'Input'
WHERE [namespace_id] = '3ED79EF0-EE2F-40B8-A49A-17E9380C5932'

UPDATE [SecurityNamespace]
SET [display_name] = 'Output'
WHERE [namespace_id] = 'A95FCE90-753D-4CB7-86FB-F8CAD9F93B67'

UPDATE [SecurityNamespace]
SET [display_name] = 'Smart Wall'
WHERE [namespace_id] = '8732FCAC-A63C-46EA-9683-532B4C6644E7'


-- Update all manage security
UPDATE [SecurityAction]
SET [display_name] = 'Manage security'
WHERE [name] = 'ADMIN_SECURITY'

-- Management Server
UPDATE [SecurityAction]
SET [display_name] = 'Read'
WHERE [name] = 'GENERIC_READ'
AND [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'

UPDATE [SecurityAction]
SET [display_name] = 'Edit'
WHERE [name] = 'GENERIC_WRITE'
AND [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'

UPDATE [SecurityAction]
SET [display_name] = 'System Monitor'
WHERE [name] = 'EXEC_CONFIG_REPORT'
AND [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'

UPDATE [SecurityAction]
SET [display_name] = 'Manage federated site hierarchy'
WHERE [name] = 'ADMIN_FEDERATED_HIERARCHY'
AND [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'

UPDATE [SecurityAction]
SET [display_name] = 'Back up configuration'
WHERE [name] = 'BACKUP_CONFIGURATION'
AND [namespace_id] = 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'

-- Recording Servers
UPDATE [SecurityAction]
SET [display_name] = 'Manage hardware'
WHERE [name] = 'ADMIN_HARDWARE'
AND [namespace_id] = '9710735D-EB10-44EA-888D-FA8AB05A6433'

UPDATE [SecurityAction]
SET [display_name] = 'Manage storage'
WHERE [name] = 'ADMIN_STORAGE'
AND [namespace_id] = '9710735D-EB10-44EA-888D-FA8AB05A6433'

-- Hardware
UPDATE [SecurityAction]
SET [display_name] = 'View hardware password'
WHERE [name] = 'VIEW_HARDWARE_PASSWORDS'
AND [namespace_id] = 'F97772D8-7B6D-4EA4-B6B5-EE4EA010FA2C'

-- Cameras
UPDATE [SecurityAction]
SET [display_name] = 'View live'
WHERE [name] = 'VIEW_LIVE'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

UPDATE [SecurityAction]
SET [display_name] = 'Retrieve remote recordings'
WHERE [name] = 'READ_REMOTE_RECORDINGS'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

UPDATE [SecurityAction]
SET [display_name] = 'Read evidence locks'
WHERE [name] = 'READ_EVIDENCE_LOCK'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

UPDATE [SecurityAction]
SET [display_name] = 'Delete and reduce evidence locks'
WHERE [name] = 'DELETE_EVIDENCE_LOCK'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

UPDATE [SecurityAction]
SET [display_name] = 'Create and extend evidence locks'
WHERE [name] = 'CREATE_EVIDENCE_LOCK'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

UPDATE [SecurityAction]
SET [display_name] = 'Manual PTZ'
WHERE [name] = 'PTZ_CONTROL'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

UPDATE [SecurityAction]
SET [display_name] = 'Lock/unlock PTZ presets'
WHERE [name] = 'MANAGE_LOCK_PTZ_PRESET'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

UPDATE [SecurityAction]
SET [display_name] = 'Reserve PTZ sessions'
WHERE [name] = 'RESERVE_PTZ_SESSION'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

UPDATE [SecurityAction]
SET [display_name] = 'Release PTZ sessions'
WHERE [name] = 'RELEASE_PTZ_SESSION'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

UPDATE [SecurityAction]
SET [display_name] = 'Activate PTZ presets or patrolling profiles'
WHERE [name] = 'ACTIVATE_PTZ_PRESET'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

UPDATE [SecurityAction]
SET [display_name] = 'Manage PTZ presets or patrolling profiles'
WHERE [name] = 'MANAGE_PTZ_PRESET'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

UPDATE [SecurityAction]
SET [display_name] = 'Lift privacy masks'
WHERE [name] = 'LIFT_PRIVACY_MASKING'
AND [namespace_id] = '623D03F8-C5D5-46BC-A2F4-4C03562D4F85C'

-- Microphone
UPDATE [SecurityAction]
SET [display_name] = 'Read evidence locks'
WHERE [name] = 'READ_EVIDENCE_LOCK'
AND [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'

UPDATE [SecurityAction]
SET [display_name] = 'Delete and reduce evidence locks'
WHERE [name] = 'DELETE_EVIDENCE_LOCK'
AND [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'

UPDATE [SecurityAction]
SET [display_name] = 'Create and extend evidence locks'
WHERE [name] = 'CREATE_EVIDENCE_LOCK'
AND [namespace_id] = '15F48F88-CA89-4926-9A84-2B02864EC77A'


-- Speaker
UPDATE [SecurityAction]
SET [display_name] = 'Read evidence locks'
WHERE [name] = 'READ_EVIDENCE_LOCK'
AND [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'

UPDATE [SecurityAction]
SET [display_name] = 'Delete and reduce evidence locks'
WHERE [name] = 'DELETE_EVIDENCE_LOCK'
AND [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'

UPDATE [SecurityAction]
SET [display_name] = 'Create and extend evidence locks'
WHERE [name] = 'CREATE_EVIDENCE_LOCK'
AND [namespace_id] = '48B602BC-E752-4BBF-8E2A-7DE01F53A6DD'

-- Metadata
UPDATE [SecurityAction]
SET [display_name] = 'Read evidence locks'
WHERE [name] = 'READ_EVIDENCE_LOCK'
AND [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'

UPDATE [SecurityAction]
SET [display_name] = 'Delete and reduce evidence locks'
WHERE [name] = 'DELETE_EVIDENCE_LOCK'
AND [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'

UPDATE [SecurityAction]
SET [display_name] = 'Create and extend evidence locks'
WHERE [name] = 'CREATE_EVIDENCE_LOCK'
AND [namespace_id] = 'EDE4D51C-F691-4894-9C0B-C3AE096DC04D'

-- Smart Wall
UPDATE [SecurityAction]
SET [display_name] = 'Create Smart Wall'
WHERE [name] = 'CREATE_SMARTWALL'
AND [namespace_id] = '8732FCAC-A63C-46EA-9683-532B4C6644E7'

-- Matrix
UPDATE [SecurityAction]
SET [display_name] = 'Create Matrix'
WHERE [name] = 'CREATE_MATRIX'
AND [namespace_id] = '988D493D-1597-4BE6-AFF0-396BFD5D047A'

-- Metadata Search
UPDATE [SecurityAction]
SET [display_name] = 'Read'
WHERE [name] = 'GENERIC_READ'
AND [namespace_id] = '55BF1320-0224-48C7-8F57-BD41090576C8'

-- Search
UPDATE [SecurityAction]
SET [display_name] = 'Read public searches'
WHERE [name] = 'READ_PUBLIC_SEARCH'
AND [namespace_id] = 'D2D396E4-819E-4226-AA7F-AF36896D78BC'

UPDATE [SecurityAction]
SET [display_name] = 'Create public searches'
WHERE [name] = 'CREATE_PUBLIC_SEARCH'
AND [namespace_id] = 'D2D396E4-819E-4226-AA7F-AF36896D78BC'

UPDATE [SecurityAction]
SET [display_name] = 'Edit public searches'
WHERE [name] = 'EDIT_PUBLIC_SEARCH'
AND [namespace_id] = 'D2D396E4-819E-4226-AA7F-AF36896D78BC'

UPDATE [SecurityAction]
SET [display_name] = 'Delete public searches'
WHERE [name] = 'DELETE_PUBLIC_SEARCH'
AND [namespace_id] = 'D2D396E4-819E-4226-AA7F-AF36896D78BC'
GO

