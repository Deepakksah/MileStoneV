-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[markeddata_update_sizes]') AND type in (N'P', N'PC'))
DROP PROCEDURE [markeddata_update_sizes]
GO

CREATE PROCEDURE [markeddata_update_sizes]
	@IDMarkedData UNIQUEIDENTIFIER,
	@StickerSizes [tvp_StickerSizeTable] READONLY
AS	
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @MarkedDataType_EvidenceLock INT = 2

		DECLARE @rowCount INT
		DECLARE @marked_data_type INT

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		SELECT	@marked_data_type = [MarkDataType]
		FROM	[MarkedData] WITH(UPDLOCK)
		WHERE	[IDMarkedData] = @IDMarkedData

		-- Check that the marked data exist
		IF (@marked_data_type IS NULL)
		BEGIN
			-- Silently ignore if marked data does not exist
			ROLLBACK TRAN
			RETURN 0
		END

		IF (@marked_data_type <> @MarkedDataType_EvidenceLock)
		BEGIN
			-- Silently ignore if marked data is not an evidence lock
			ROLLBACK TRAN
			RETURN 0
		END

		UPDATE	mdd
		SET		mdd.[Size] = ss.[Size]
		FROM	[MarkedDataDevices] mdd
				INNER JOIN @StickerSizes ss ON ss.[DeviceId] = mdd.[IDDevice]
		WHERE	mdd.[IDMarkedData] = @IDMarkedData
				AND mdd.[Size] <> ss.[Size]
				AND ss.[Size] >= 0
	
		SELECT  @rowcount = @@ROWCOUNT
			
		IF (@rowcount <> 0)
		BEGIN
			-- At least one of the devices sizes were changed, so let's update total size
			IF NOT EXISTS (SELECT 1 FROM [MarkedDataDevices] WHERE [IDMarkedData] = @IDMarkedData AND [Size] < 0)
			BEGIN
				-- All MarkedDataDevice rows for this marked data has a size
				UPDATE	[MarkedData]
				SET		[LastModified] = @CurrentUtcTime,
						[ProtectionSize] = (SELECT SUM([Size]) FROM [MarkedDataDevices] WHERE [IDMarkedData] = @IDMarkedData)
				WHERE	[IDMarkedData] = @IDMarkedData
			END
		END

		COMMIT TRAN
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