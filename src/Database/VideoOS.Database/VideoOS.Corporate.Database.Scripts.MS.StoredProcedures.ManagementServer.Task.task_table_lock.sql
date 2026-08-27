-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[task_table_lock]') AND type in (N'P', N'PC'))
DROP PROCEDURE [task_table_lock]
GO

CREATE PROCEDURE [task_table_lock]
	@LockLevel INT
 AS
BEGIN

	-- This is an internal procedure that's used when a update
	-- involves multiple tables. The procedure is used to synchronize
	-- the update.

	-- The procedure is meant to be called within an active
	-- transaction.

	UPDATE [TaskTableLock] WITH ( UPDLOCK, ROWLOCK, HOLDLOCK )
	SET [LastUpdated] = GETUTCDATE()
	WHERE [LockLevel] >= @LockLevel

END
GO
