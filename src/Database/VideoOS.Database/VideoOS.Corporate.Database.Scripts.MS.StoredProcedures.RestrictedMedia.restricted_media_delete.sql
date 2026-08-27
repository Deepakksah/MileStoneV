-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[restricted_media_delete]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [restricted_media_delete]
GO

CREATE PROCEDURE [restricted_media_delete]
	@IDRestrictedMedia  UNIQUEIDENTIFIER,
	@session_id         [GuidType]
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @header [LongNameType]
		DECLARE @affectedRecorders TABLE ([recorder_id] [GuidType] NOT NULL);

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME = GETUTCDATE()

		-- Get values for logging and check for exists
		SELECT	@header = [Header]
		FROM	[RestrictedMedia] WITH(UPDLOCK)
		WHERE	[IDRestrictedMedia] = @IDRestrictedMedia

		-- Check that the restricted media exist
		IF (@header IS NULL)
		BEGIN
			;THROW 60376, @procedureName, 1; -- Message like: Cannot delete the media restriction. The media restriction does not exist.s
		END

		-- Create diffgrams for evidence locks
		DECLARE @device_id [GuidType]
		DECLARE @recorder_id [GuidType]
		DECLARE @dataSetBefore XML

		DECLARE device_restricted_media_cursor CURSOR FORWARD_ONLY READ_ONLY FOR
			SELECT	[IDRecorder], [IDDevice]
			FROM	[DeviceRestrictedMedia]
			WHERE	[IDRestrictedMedia] = @IDRestrictedMedia
	
		OPEN device_restricted_media_cursor
		BEGIN TRY
			WHILE (1 = 1)
			BEGIN
				FETCH device_restricted_media_cursor
				INTO @recorder_id, @device_id

				IF(@@FETCH_STATUS <> 0)
				BEGIN
					BREAK
				END

				SET @dataSetBefore = dbo.[deviceRestrictedMediaAsDataSet](@IDRestrictedMedia, @device_id)

				-- Create diffgram task for all services for recorder
				EXEC [create_task_recorder_services_transaction]
					@type = 'diffgram_delete',
					@data_a = @dataSetBefore,
					@current_time = @CurrentUtcTime,
					@recorder_id = @recorder_id

				INSERT INTO @affectedRecorders ([recorder_id]) VALUES (@recorder_id)
			END
		END TRY
		BEGIN CATCH
			CLOSE device_restricted_media_cursor
			DEALLOCATE device_restricted_media_cursor
			;THROW
		END CATCH

		CLOSE device_restricted_media_cursor
		DEALLOCATE device_restricted_media_cursor

		-- Start deleting RestrictedMedia
		DELETE [RestrictedMediaDevice]
		WHERE  [IDRestrictedMedia] = @IDRestrictedMedia

		DELETE [RestrictedMedia]
		WHERE  [IDRestrictedMedia] = @IDRestrictedMedia

		COMMIT TRAN

		DECLARE @AuditMediaRestrictionDeleted INT = 7122 -- Log: Restricted media '{header}' was deleted.
		EXEC create_pending_log @session_id, @AuditMediaRestrictionDeleted, @IDRestrictedMedia, @header

		SELECT DISTINCT([recorder_id]) FROM @affectedRecorders
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
	END CATCH
END
GO