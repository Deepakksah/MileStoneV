-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_task_by_target_and_type]') AND type = 'P')
DROP PROCEDURE [query_task_by_target_and_type]
GO

CREATE PROCEDURE [query_task_by_target_and_type]
	@target_id [GuidType],
	@type [InternalIdType]
AS
BEGIN
	SET NOCOUNT ON

	SELECT
		[pkid],
		[type],
		[data_a],
		[data_b],
		[start_time],
		[status_details],
		[task_status]
	FROM [Task]
	WHERE [type] = @type AND [target] = @target_id
END
GO
