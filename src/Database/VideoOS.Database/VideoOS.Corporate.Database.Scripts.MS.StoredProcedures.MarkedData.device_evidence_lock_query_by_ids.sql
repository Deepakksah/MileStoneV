-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[device_evidence_lock_query_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [device_evidence_lock_query_by_ids]
GO

CREATE PROCEDURE [dbo].[device_evidence_lock_query_by_ids]
	@evidence_lock_ids tvp_IdTableUniqueIdentifier READONLY
AS
BEGIN
	SET NOCOUNT ON
	
	SELECT DISTINCT	[IDMarkedData], [IDDevice], [IDRecorder] -- Return one row pr device (not pr track)
	FROM	[DeviceTrackEvidenceLock] d
			INNER JOIN @evidence_lock_ids i ON i.[ID] = d.[IDMarkedData]
END
GO