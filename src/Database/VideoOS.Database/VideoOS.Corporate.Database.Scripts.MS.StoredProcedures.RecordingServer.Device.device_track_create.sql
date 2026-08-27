-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[device_track_create]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [device_track_create]
GO

CREATE PROCEDURE [device_track_create]
	@IDDevice [GuidType],
	@IDTrack UNIQUEIDENTIFIER,
	@IDTrackDefinition UNIQUEIDENTIFIER,
	@MergeEdge BIT,
	@DefaultPlayback BIT,
	@InUse BIT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetAfter XML
		DECLARE @rowCount int

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @CurrentUtcTime2 DATETIME2 = SYSUTCDATETIME()

		BEGIN TRAN

		IF EXISTS (SELECT 1 FROM [DeviceTrack] WITH(UPDLOCK, HOLDLOCK) WHERE [IdTrack] = @IdTrack)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		-- Update table 'DeviceTracks' with new settings 
		INSERT [DeviceTrack] ([IDDevice], [IDTrack], [IDTrackDefinition], [MergeEdge], [DefaultPlayback], [InUse], [OnlyUseMediaAfter], [CreatedDate])
		VALUES (@IDDevice, @IDTrack, @IDTrackDefinition, @MergeEdge, @DefaultPlayback, @InUse, @CurrentUtcTime2, @CurrentUtcTime2)
		

		SET @rowCount = @@ROWCOUNT

		IF @rowCount <> 0 
		BEGIN
			-- Get DataSet after changes
			SET @dataSetAfter = dbo.deviceTrackAsDataSet(@IdTrack)

			-- Register diffgrams for management server
			EXEC [create_task_server_services_transaction]
								@type = 'diffgram_insert',
								@data_a = @dataSetAfter,
								@current_time = @CurrentUtcTime
		END

		COMMIT TRAN

		RETURN 0
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
