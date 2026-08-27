-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[add_device_group_members]') AND type in (N'P', N'PC'))
DROP PROCEDURE [add_device_group_members]
GO

CREATE PROCEDURE [add_device_group_members]
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
		DECLARE @dataSetAfter XML

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @groupDeviceType [InternalIdType]
		DECLARE @groupBuiltIn AS BIT

		SELECT @groupDeviceType = [DeviceType],
				@groupBuiltIn = [BuiltIn]
		FROM [DeviceGroups] WITH(UPDLOCK) 
		WHERE [IDDeviceGroup] = @IDDeviceGroup

		-- Ensure that device group exists
		IF (@groupDeviceType IS NULL)
		BEGIN
			-- Parent does not exist
			;THROW 60175, @procedureName, 1;
		END

		-- Ensure that group is not a BuiltIn group
		IF (@groupBuiltIn = 1)
		BEGIN
			-- The parent group is built-in
			;THROW 60176, @procedureName, 1;
		END

		-- Ensure that the devices exists
		IF (EXISTS(	SELECT [ID] 
					FROM @DeviceIds ids
					WHERE NOT EXISTS (SELECT [IDDevice] From [Devices] d WITH(UPDLOCK) WHERE d.[IDDevice] = ids.[ID])))
		BEGIN
			-- Some of the specified devices does not exist
			;THROW 60177, @procedureName, 1;
		END

		-- Ensure that all devices are of the correct type according to device group type
		IF (EXISTS( SELECT [ID]
					FROM @DeviceIds ids
					INNER JOIN [Devices] d ON d.[IDDevice] = ids.[ID]
					WHERE d.[DeviceType] <> @groupDeviceType))
		BEGIN
			-- Some of the specified devices are not compatible with the group type
			;THROW 60178, @procedureName, 1;
		END

		BEGIN TRY
			INSERT INTO [DeviceGroupMembers] ([LastModified], [IDDeviceGroup], [IDDevice])
			SELECT @CurrentUtcTime, @IDDeviceGroup, [ID] 
			FROM @DeviceIds
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				;THROW 60179, @procedureName, 1;
			END
			;THROW
		END CATCH
	
		-- Get DataSet after changes
		SET @dataSetAfter = dbo.deviceGroupMembersTableAsDataSet(@IDDeviceGroup, @DeviceIds)

		-- Create diffgram
		EXEC [create_task_transaction]
			@type = 'diffgram_insert',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetAfter,
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