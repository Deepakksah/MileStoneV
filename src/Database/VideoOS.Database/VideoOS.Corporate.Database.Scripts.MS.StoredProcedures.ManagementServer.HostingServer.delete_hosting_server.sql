-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_hosting_server]') AND type = 'P')
DROP PROCEDURE [delete_hosting_server]
GO

CREATE PROCEDURE [delete_hosting_server]
	@pkid INT,
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)
	
	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Extract info for logging
		DECLARE @name [NameType]

		SELECT	@name = [name]
		FROM	[HostingServer] WITH(UPDLOCK)
		WHERE	[pkid] = @pkid

		-- Does the hosting server exist?
		IF NOT EXISTS(
			SELECT *
			FROM [HostingServer] WITH(UPDLOCK)
			WHERE [pkid] = @pkid
		)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%pkid=' + CAST(@pkid AS NVARCHAR(10)) + N'%%'
			;THROW 60604, @ErrorArgs, 1;
		END
	
		-- Does the hosting server have any hardware?
		IF EXISTS(
			SELECT *
			FROM [HostedHardware] WITH(UPDLOCK)
			WHERE [hosting_server_id] = @pkid
		)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%'
			;THROW 60605, @ErrorArgs, 1;
		END

		DECLARE @dataSetBefore XML
		SELECT @dataSetBefore = dbo.[hostingServerAsDataSet](@pkid)

		DELETE	[HostingServer]
		WHERE	[pkid] = @pkid

		-- Create diffgram task for all recorder services
		EXEC [create_task_recorder_services_transaction]
			@type = 'diffgram_delete',
			@data_a = @dataSetBefore,
			@current_time = @CurrentUtcTime

		COMMIT TRANSACTION

		-- AuditHostingServerDeleted = 20212
		EXEC create_pending_log @session_id, 20212, @pkid, @name
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN
			ROLLBACK TRANSACTION
		END;
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
	END CATCH;
END
GO