-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[device_has_markeddata]') AND type in (N'P', N'PC'))
DROP PROCEDURE [device_has_markeddata]
GO

CREATE PROCEDURE [device_has_markeddata]
	@IDDevice NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @MarkedDataType_EvidenceLock INT = 2
	
	SELECT 
		CASE WHEN EXISTS
		(
			SELECT	mdd.[IDDevice]
			FROM	[MarkedDataDevices] mdd
					INNER JOIN [MarkedData] md ON md.[IDMarkedData] = mdd.[IDMarkedData]
			WHERE	mdd.[IDDevice] = @IDDevice AND
					md.[MarkDataType] = @MarkedDataType_EvidenceLock
		)
		THEN CAST(1 AS BIT)
		ELSE CAST(0 AS BIT)
		END
END
GO