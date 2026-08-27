-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[remove_device_group_members]') AND type in (N'P', N'PC'))
DROP PROCEDURE [remove_device_group_members]
GO

CREATE PROCEDURE [remove_device_group_members]
	@IDDeviceGroup [GuidType],
	@DeviceIds tvp_StringTableGuidType READONLY
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

		DECLARE @groupBuiltIn AS BIT

		SELECT @groupBuiltIn = [BuiltIn]
		FROM [DeviceGroups] WITH(UPDLOCK) 
		WHERE [IDDeviceGroup] = @IDDeviceGroup

		-- Ensure that device group exists
		IF (@groupBuiltIn IS NULL)
		BEGIN
			-- Device group does not exist
			;THROW 60180, @procedureName, 1;
		END

		-- Ensure that group is not a BuiltIn group
		IF (@groupBuiltIn = 1)
		BEGIN
			-- The parent group is built-in
			;THROW 60181, @procedureName, 1;
		END

		-- Ensure that the devices exists
		IF (EXISTS(	SELECT [ID] 
					FROM @DeviceIds ids
					WHERE NOT EXISTS (SELECT [IDDevice] From [DeviceGroupMembers] dgm WITH(UPDLOCK) WHERE dgm.[IDDeviceGroup] = @IDDeviceGroup AND dgm.[IDDevice] = ids.[ID])))
		BEGIN
			-- Some of the specified devices does not exist
			;THROW 60182, @procedureName, 1;
		END

		-- Get data set before changes
		SET @dataSetBefore = dbo.deviceGroupMembersTableAsDataSet(@IDDeviceGroup, @DeviceIds)

		-- Delete the requested rows
		DELETE FROM [DeviceGroupMembers]
		WHERE [IDDeviceGroup] = @IDDeviceGroup
		AND [IDDevice] IN (SELECT [ID] FROM @DeviceIds)

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