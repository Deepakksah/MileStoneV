-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_history_entries]') AND type = 'P')
DROP PROCEDURE [admin_query_failover_history_entries]
GO

CREATE PROCEDURE [admin_query_failover_history_entries]
	@IDFailover [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[IDFailoverHistoryEntry],
		[IDFailover],
		[Description],
		[IDRecorder],
		[StartTime],
		[EndTime],
		[LastModified]
	FROM [FailoverHistory]
	WHERE [IDFailover] = @IDFailover
END
GO