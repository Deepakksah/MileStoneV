-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_tasks_expired_by_type]') AND type = 'P')
DROP PROCEDURE [delete_tasks_expired_by_type]
GO

CREATE PROCEDURE [delete_tasks_expired_by_type]
	@type [InternalIdType],
	@retain_minutes INT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @deleteBefore AS DATETIME
	SET @deleteBefore = DATEADD(MINUTE, -@retain_minutes, GETUTCDATE())

	DELETE	[Task]
	WHERE	[type] = @type
			AND [task_status] IN (2, 3) -- Succeeded = 2, Failed = 3
			AND [end_time] < @deleteBefore

END
GO
