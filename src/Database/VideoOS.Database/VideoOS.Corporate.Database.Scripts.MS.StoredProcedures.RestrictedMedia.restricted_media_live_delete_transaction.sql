-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[restricted_media_live_delete_transaction]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [restricted_media_live_delete_transaction]
GO

CREATE PROCEDURE [restricted_media_live_delete_transaction]
	@IDDevices		[tvp_StringTableGuidType] READONLY
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @CurrentUtcTime DATETIME = GETUTCDATE()

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	-- Check the modified restricted media live device ids exits in the restricted media live table
	DECLARE @deviceIdCount INT
	SET @deviceIdCount = (SELECT COUNT(*) FROM [RestrictedMediaLive] WHERE [IDDevice] IN (SELECT ids.[ID] from @IDDevices ids))
	IF (@deviceIdCount <> (SELECT COUNT(*) FROM @IDDevices))
	BEGIN
		;THROW 60381, @procedureName, 1; -- Message like: Cannot delete the live media restriction. The live media restriction does not exist.
	END

	-- Create diffgrams for restricted media live
	DECLARE @device_id [GuidType]
	DECLARE @recorder_id [GuidType]
	DECLARE @diffgram_before AS XML
	DECLARE @deviceRestrictedMediaLiveBefore TABLE (
		[device_id] [GuidType] NOT NULL,
		[recorder_id] [GuidType] NOT NULL,
		[diffgram] XML NOT NULL
		);

	-- Capture before diffgrams
	INSERT @deviceRestrictedMediaLiveBefore ([device_id], [recorder_id], [diffgram])
		SELECT	[IDDevice], [IDRecorder], dbo.[deviceRestrictedMediaAsDataSet]([IDDevice], [IDDevice])
		FROM	[DeviceRestrictedMedia]
		WHERE	[IDRestrictedMedia] IN (SELECT ids.[ID] from @IDDevices ids)

	DECLARE device_restricted_media_cursor CURSOR FORWARD_ONLY READ_ONLY FOR
				SELECT	[before].[device_id],
						[before].[recorder_id],
						[before].[diffgram] AS [diffgram_before]
				FROM	@deviceRestrictedMediaLiveBefore [before]
	
	OPEN device_restricted_media_cursor
	BEGIN TRY
		WHILE (1 = 1)
		BEGIN
			FETCH device_restricted_media_cursor
			INTO @device_id, @recorder_id, @diffgram_before

			IF(@@FETCH_STATUS <> 0)
			BEGIN
				BREAK
			END

			-- Create diffgram task for all services for recorder
			EXEC [create_task_recorder_services_transaction]
				@type = 'diffgram_delete',
				@data_a = @diffgram_before,
				@current_time = @CurrentUtcTime,
				@recorder_id = @recorder_id
		END
	END TRY
	BEGIN CATCH
		CLOSE device_restricted_media_cursor
		DEALLOCATE device_restricted_media_cursor
		;THROW
	END CATCH

	CLOSE device_restricted_media_cursor
	DEALLOCATE device_restricted_media_cursor

	-- Start deleting RestrictedMedia live rows
	DELETE [RestrictedMediaLive] WHERE	[IDDevice] IN (SELECT ids.[ID] from @IDDevices ids)

	SELECT DISTINCT([recorder_id]) FROM @deviceRestrictedMediaLiveBefore
END
GO