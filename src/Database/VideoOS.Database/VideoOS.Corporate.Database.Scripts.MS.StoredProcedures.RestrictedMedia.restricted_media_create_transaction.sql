-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[restricted_media_create_transaction]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [restricted_media_create_transaction]
GO

CREATE PROCEDURE [restricted_media_create_transaction]
	@IDRestrictedMedia  UNIQUEIDENTIFIER,
	@IDDevices          [tvp_StringTableGuidType] READONLY,
	@Header             [LongNameType],
	@Description        [DescriptionType2],
	@UserName           [NameType],
	@StartTime          DATETIME2,
	@EndTime            DATETIME2,
	@LastModified       DATETIME2
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

	DECLARE @affectedRecorders TABLE ([recorder_id] [GuidType] NOT NULL);
	INSERT	[RestrictedMedia]
			(
			[IDRestrictedMedia],
			[Header],
			[Description],
			[UserName],
			[StartTime],
			[EndTime],
			[LastModified]
			)
	VALUES (
			@IDRestrictedMedia,
			@Header,
			@Description,
			@UserName,
			@StartTime,
			@EndTime,
			@LastModified
		)

	INSERT [RestrictedMediaDevice] ([IDRestrictedMedia], [IDDevice])
	SELECT	@IDRestrictedMedia, ids.[Id]
	FROM	@IDDevices ids

	-- Create diffgrams
	DECLARE @device_id [GuidType]
	DECLARE @recorder_id [GuidType]
	DECLARE @dataSetAfter XML

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

			IF (@@FETCH_STATUS <> 0)
			BEGIN
				BREAK
			END

			-- Create diffgram task for all services for recorder
			SET @dataSetAfter = dbo.[deviceRestrictedMediaAsDataSet](@IDRestrictedMedia, @device_id)
		
			EXEC [create_task_recorder_services_transaction]
				@type = 'diffgram_insert',
				@data_a = @dataSetAfter,
				@current_time = @LastModified,
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

	SELECT DISTINCT([recorder_id]) FROM @affectedRecorders
END
GO