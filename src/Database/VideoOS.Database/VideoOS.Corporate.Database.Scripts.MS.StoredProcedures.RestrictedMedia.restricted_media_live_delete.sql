-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[restricted_media_live_delete]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [restricted_media_live_delete]
GO

CREATE PROCEDURE [restricted_media_live_delete]
	@IDDevices          [tvp_StringTableGuidType] READONLY,
	@session_id         [GuidType]
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @device_id		[GuidType]
		DECLARE @device_name	NVARCHAR(256)
		DECLARE @device_type	VARCHAR(100)

		DECLARE @AuditRestrictedMediaLiveDeleted INT = 7126

		BEGIN TRAN

		EXEC restricted_media_live_delete_transaction @IDDevices
	
		DECLARE device_restricted_media_cursor CURSOR FORWARD_ONLY READ_ONLY FOR
			SELECT	[IDDevice], [Name], [DeviceType]
			FROM	[Devices]
			WHERE	[IDDevice] IN (SELECT ids.[ID] FROM @IDDevices ids)
	
		OPEN device_restricted_media_cursor
		BEGIN TRY
			WHILE (1 = 1)
			BEGIN
				FETCH device_restricted_media_cursor INTO @device_id, @device_name, @device_type

				IF (@@FETCH_STATUS <> 0)
				BEGIN
					BREAK
				END

				EXEC create_pending_log @session_id, @AuditRestrictedMediaLiveDeleted, @device_id, @device_name, @device_type
			END
		END TRY
		BEGIN CATCH
			CLOSE device_restricted_media_cursor
			DEALLOCATE device_restricted_media_cursor
			;THROW
		END CATCH

		CLOSE device_restricted_media_cursor
		DEALLOCATE device_restricted_media_cursor

		COMMIT TRAN
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