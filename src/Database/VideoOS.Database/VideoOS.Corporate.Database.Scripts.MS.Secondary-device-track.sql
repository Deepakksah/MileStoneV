-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS(SELECT 1 FROM [DeviceTrackDefinition] WHERE [IDTrackDefinition] = '84FFF8B9-8CD1-46B2-A451-C4A87D4CBBB0')
BEGIN

	INSERT INTO [DeviceTrackDefinition] ([IDTrackDefinition], [IsPrimary], [IsEdge])
	VALUES('84FFF8B9-8CD1-46B2-A451-C4A87D4CBBB0', 0, 0)

END
GO
