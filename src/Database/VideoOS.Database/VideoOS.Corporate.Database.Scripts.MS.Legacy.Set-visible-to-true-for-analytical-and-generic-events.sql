-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [SecurityAction]
SET visible = 1
WHERE [namespace_id] in ('2E319F3E-0350-4DF1-90C6-633EAD31C117', 'D4D19C01-03F2-4AC6-9D2B-5356B5DE62F1')
AND [Name] != 'ADMIN_SECURITY' 
GO