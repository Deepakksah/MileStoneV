-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_privacy_masks_before_time]') AND type = 'P')
DROP PROCEDURE [delete_privacy_masks_before_time]
GO

CREATE PROCEDURE [delete_privacy_masks_before_time]
    @IDDevice [GuidType],
    @KeepFromTime [DATETIME]
AS
BEGIN
    SET NOCOUNT ON

		-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetBefore XML

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- Select the latest mask (time) before or at 'KeepFromTime'
		DECLARE @BeforeTime DATETIME
		SET @BeforeTime = (SELECT MAX([LastModified]) FROM [DevicePrivacyMask] WITH(UPDLOCK) WHERE [IDDevice] = @IDDevice AND [LastModified] <= @KeepFromTime)

		IF(@BeforeTime IS NOT NULL)
		BEGIN
			-- Select the latest mask (time) before @BeforeTime if any
			DECLARE @AtOrBeforeTime DATETIME
			SET @AtOrBeforeTime = (SELECT MAX([LastModified]) FROM [DevicePrivacyMask] WHERE [IDDevice] = @IDDevice AND [LastModified] < @BeforeTime)
    
			IF(@AtOrBeforeTime IS NOT NULL)
			BEGIN
				-- Get data set before changes
				SET @dataSetBefore = dbo.devicePrivacyMaskAtOrBeforeTimeAsDataSet(@IDDevice, @AtOrBeforeTime)

				DELETE FROM [DevicePrivacyMask] WHERE
					[IDDevice] = @IDDevice AND
					[LastModified] <= @AtOrBeforeTime

				-- Create diffgram task for server service
				EXEC [create_task_server_services_transaction]
								@type = 'diffgram_delete',
								@data_a = @dataSetBefore,
								@current_time = @CurrentUtcTime
			END
		END

		COMMIT TRAN

		RETURN 0
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
