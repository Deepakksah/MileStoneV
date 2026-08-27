-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[markeddata_query_all_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [markeddata_query_all_ids]
GO

CREATE PROCEDURE [dbo].[markeddata_query_all_ids]
AS
BEGIN
	DECLARE @MarkedDataType_EvidenceLock INT = 2

	SELECT	[IDMarkedData]
	FROM	[MarkedData]
	WHERE	[MarkDataType] = @MarkedDataType_EvidenceLock
END
GO