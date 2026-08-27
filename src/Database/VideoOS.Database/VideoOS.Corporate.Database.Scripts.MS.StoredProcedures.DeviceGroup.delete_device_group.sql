-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[delete_device_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [delete_device_group]
GO

CREATE PROCEDURE [delete_device_group]
	@IDDeviceGroup [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetBefore XML
		DECLARE @groupBuiltIn AS BIT

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		SELECT @groupBuiltIn = [BuiltIn]
		FROM [DeviceGroups] WITH(UPDLOCK)
		WHERE [IDDeviceGroup] = @IDDeviceGroup

		-- Ensure that group exists
		IF (@groupBuiltIn IS NULL)
		BEGIN
			-- Group does not exist
			;THROW 60185, @procedureName, 1;
		END

		-- Ensure that group is not built-in
		IF (@groupBuiltIn = 1)
		BEGIN
			-- Group is built-in
			;THROW 60186, @procedureName, 1;
		END

		-- Ensure that group is empty (no child groups)
		IF EXISTS (SELECT [IDDeviceGroup] FROM [DeviceGroups] WITH(UPDLOCK) WHERE [IDParentDeviceGroup] = @IDDeviceGroup) OR
			EXISTS (SELECT [IDDeviceGroup] FROM [DeviceGroupMembers] WITH(UPDLOCK) WHERE [IDDeviceGroup] = @IDDeviceGroup)
		BEGIN
			-- Group is not empty
			;THROW 60187, @procedureName, 1;
		END

		-- Get data set before changes
		SET @dataSetBefore = dbo.deviceGroupAsDataSet(@IDDeviceGroup)

		-- Delete the group
		DELETE [DeviceGroups]
		WHERE [IDDeviceGroup] = @IDDeviceGroup

		-- Create diffgram
		EXEC [create_task_transaction]
			@type = 'diffgram_delete',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetBefore,
			@current_time = @CurrentUtcTime

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
	END CATCH;
END
GO