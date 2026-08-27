-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_task]') AND type = 'P')
DROP PROCEDURE [create_task]
GO

CREATE PROCEDURE [create_task]
	@type VARCHAR(50),
	@target[GuidType],
	@data_a [XML],
	@percent_completed INT,
	@start_time DATETIME
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		-- First create the new task row - with default task data
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
			NULL,
			@percent_completed,
			'',
			0,
			@start_time,
			NULL
		)

		DECLARE @task_id BIGINT
		SET @task_id = @@IDENTITY

		-- Set the id attribute
		UPDATE [Task]
		SET [data_a].modify('replace value of (/task/@id)[1] with sql:variable("@task_id")')
		WHERE [pkid] = @task_id

		SELECT @task_id AS task_id
	
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		THROW; -- Used from ServerService, where Error 60000 is NOT expected
	END CATCH;
END
GO