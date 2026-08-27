-- This header is used by the dbupdate tool]
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on] The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes]
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[markeddata_query_ids_without_size]') AND type in (N'P', N'PC'))
DROP PROCEDURE [markeddata_query_ids_without_size]
GO

CREATE PROCEDURE [markeddata_query_ids_without_size]
AS
BEGIN
	DECLARE @MarkedDataType_EvidenceLock INT = 2

	SELECT	[IDMarkedData]
	FROM	[MarkedData]
	WHERE	[MarkDataType] = @MarkedDataType_EvidenceLock
			AND [ProtectionSize] = -1
END
GO