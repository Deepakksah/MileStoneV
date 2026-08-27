-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Reorder existing attributes
DECLARE @preDisplayOrder int
SET @preDisplayOrder = (SELECT [DisplayOrder] from [SecurityAttributes] where SecurityType = 'PTZ' AND [SecurityAttributeID] = 'DEVICEPTZPRIORITY')

UPDATE [SecurityAttributes]
SET [DisplayOrder] = (SELECT [DisplayOrder] from [SecurityAttributes] where SecurityType = 'PTZ' AND [SecurityAttributeID] = 'DEVICEPTZPRESETS')
WHERE SecurityType = 'PTZ' AND [SecurityAttributeID] = 'DEVICEPTZPRIORITY'

UPDATE [SecurityAttributes]
SET [DisplayOrder] = @preDisplayOrder
WHERE SecurityType = 'PTZ' AND [SecurityAttributeID] = 'DEVICEPTZPRESETS'
