-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_task_transaction]') AND type = 'P')
DROP PROCEDURE [create_task_transaction]
GO

CREATE PROCEDURE [create_task_transaction]
	@type VARCHAR(50),
	@target [GuidType],
	@data_a [XML],
	@data_b [XML] = NULL,
	@current_time DATETIME

AS
BEGIN
	SET NOCOUNT ON


	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	-- We need to make sure that tasks are committed in the order
	-- that thay are alos created in. This is to ensure that 
	-- tasks with higher numbers are not retrieved by a
	-- task manager before an earlier task (with lower number)
	-- has been committed.
	-- This is ensured by taking a table lock for tasks which
	-- means that no other thread can continue past the following
	-- line before a thread owning the lock is either committed or
	-- rolled back.
	EXEC [task_table_lock] @LockLevel = 0

	INSERT [Task] (
		[type],
		[target],
		[data_a],
		[data_b],
		[percent_completed],
		[status_details], 
		[task_status],
		[start_time],
		[end_time]
	)
	VALUES (
		@type,
		@target,
		@data_a,
		@data_b,
		0,
		'',
		0,
		@current_time,
		null
	)
	
	RETURN 0
END
GO