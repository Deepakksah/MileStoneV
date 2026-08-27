-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[device_track_update]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [device_track_update]
GO

CREATE PROCEDURE [device_track_update]
	@IDTrack UNIQUEIDENTIFIER,
	@MergeEdge BIT,
	@DefaultPlayback BIT,
	@InUse BIT,
	@OnlyUseMediaAfter DateTime2
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML
		DECLARE @rowCount int

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		IF @@TRANCOUNT = 0
		BEGIN
			;THROW 60008, @procedureName, 1;
		END

		IF NOT EXISTS (SELECT 1 FROM [DeviceTrack]  WITH(UPDLOCK) WHERE [IDTrack] = @IDTrack)
		BEGIN
			;THROW 60000, @procedureName, 1;
		END

		-- Get DataSet before changes (for DiffGram)
		SET @dataSetBefore = dbo.deviceTrackAsDataSet(@IDTrack)

		-- Update table 'DeviceTracks' with new settings 
		UPDATE [DeviceTrack]
		SET [MergeEdge] = @MergeEdge,
			[DefaultPlayback] = @DefaultPlayback,
			[InUse] = @InUse,
			[OnlyUseMediaAfter] = COALESCE(@OnlyUseMediaAfter, [OnlyUseMediaAfter]),
			[CreatedDate] = COALESCE(@OnlyUseMediaAfter, [CreatedDate])
		WHERE [IDTrack] = @IDTrack AND
				([MergeEdge] <> @MergeEdge
				OR [DefaultPlayback] <> @DefaultPlayback
				OR [InUse] <> @InUse)

		SET @rowCount = @@ROWCOUNT

		IF @rowCount <> 0 
		BEGIN
			-- Get DataSet after changes
			SET @dataSetAfter = dbo.deviceTrackAsDataSet(@IDTrack)

			-- Register diffgrams for management server
			EXEC [create_task_server_services_transaction]
								@type = 'diffgram_update',
								@data_a = @dataSetBefore,
								@data_b = @dataSetAfter,
								@current_time = @CurrentUtcTime
		END

		RETURN 0
	END TRY
	BEGIN CATCH 
		DECLARE @shouldReThrowError BIT = dbo.[ShouldReThrowError](ERROR_NUMBER(), ERROR_SEVERITY())
		IF(@shouldReThrowError = 1)
			THROW;
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE())
		;THROW 60000, @ErrorArgs, 1;
    END CATCH;
END
GO
