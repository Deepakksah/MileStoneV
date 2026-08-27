-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[device_track_update_onlyusemediaafter]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [device_track_update_onlyusemediaafter]
GO

CREATE PROCEDURE [device_track_update_onlyusemediaafter]
	@IDTrack UNIQUEIDENTIFIER,
	@OnlyUseMediaAfter DateTime2
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY

		BEGIN TRAN

		DECLARE @dataSetBefore XML
		DECLARE @dataSetAfter XML
		DECLARE @rowCount int

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		IF NOT EXISTS (SELECT 1 FROM [DeviceTrack]  WITH(UPDLOCK) WHERE [IDTrack] = @IDTrack AND [CreatedDate] = [OnlyUseMediaAfter])
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END

		-- Get DataSet before changes (for DiffGram)
		SET @dataSetBefore = dbo.deviceTrackAsDataSet(@IDTrack)

		-- Update table 'DeviceTracks' with new settings 
		UPDATE [DeviceTrack]
		SET [OnlyUseMediaAfter] = @OnlyUseMediaAfter
		WHERE [IDTrack] = @IDTrack 		
		AND [OnlyUseMediaAfter] <> @OnlyUseMediaAfter 

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

		COMMIT TRAN
		RETURN 0
	END TRY
	BEGIN CATCH 
		IF @@TRANCOUNT > 0
		BEGIN 
			ROLLBACK TRAN
		END;
		THROW; -- Used from ServerService, where Error 60000 is NOT expected
	END CATCH;
END
GO
