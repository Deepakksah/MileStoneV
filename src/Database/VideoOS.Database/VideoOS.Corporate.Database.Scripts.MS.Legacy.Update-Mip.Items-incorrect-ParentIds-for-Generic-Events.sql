-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [Mip].[Items]
SET ParentId = '00000000-0000-0000-0000-000000000000'
WHERE Kind = 'D4D19C01-03F2-4AC6-9D2B-5356B5DE62F1'

GO