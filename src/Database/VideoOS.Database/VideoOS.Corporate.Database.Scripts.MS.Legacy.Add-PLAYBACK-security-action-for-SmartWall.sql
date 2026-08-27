-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentTime DateTime
SET @CurrentTime = GETUTCDATE()

DECLARE @SmartWallNamespaceId GuidType
SET @SmartWallNamespaceId = '8732FCAC-A63C-46EA-9683-532B4C6644E7'

-- Add 'PLAYBACK' security action to 'SmartWall' security namespace
IF NOT EXISTS(SELECT * FROM [SecurityAction] WHERE name = 'PLAYBACK' and namespace_id = @SmartWallNamespaceId)
BEGIN
	INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name])
	VALUES (@SmartWallNamespaceId, 'PLAYBACK', 'Playback', 6, 1, 64, NULL)
END

GO