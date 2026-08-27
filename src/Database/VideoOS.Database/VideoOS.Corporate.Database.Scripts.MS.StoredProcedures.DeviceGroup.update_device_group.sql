-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[update_device_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [update_device_group]
GO

CREATE PROCEDURE [update_device_group]
	@IDDeviceGroup [GuidType],
	@name [NameType],
	@description [DescriptionType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	DECLARE @rowCount INT
	DECLARE @dataSetBefore XML
	DECLARE @dataSetAfter XML
	DECLARE @existing_device_group_id [GuidType]
	DECLARE @parent_device_group_name [NameType]

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	IF
	(
		@name IS NULL AND 
		@description IS NULL
	)
	BEGIN
		RETURN 0
	END

	BEGIN TRY
		BEGIN TRAN
	
		-- Make sure that device group exists
		SELECT @existing_device_group_id = [IDDeviceGroup]
		FROM [DeviceGroups] dg 
		WHERE dg.[IDDeviceGroup] = @IDDeviceGroup

		IF(@existing_device_group_id IS NULL)
		BEGIN
			;THROW 60109, @procedureName, 1;
		END

		SET @existing_device_group_id = null

		-- Make sure name is unique on level
		IF(@name IS NOT NULL)
		BEGIN
			SELECT @existing_device_group_id = IDDeviceGroup 
			FROM DeviceGroups
			WHERE IDParentDeviceGroup = 
			(
				SELECT IDParentDeviceGroup 
				FROM DeviceGroups 
				WHERE IDDeviceGroup = @IDDeviceGroup
			)
			AND Name = @name

			IF(@existing_device_group_id IS NOT NULL)
			BEGIN
				SET @parent_device_group_name = (SELECT [Name] FROM DeviceGroups WHERE IDDeviceGroup = (SELECT IDParentDeviceGroup FROM DeviceGroups WHERE IDDeviceGroup = @IDDeviceGroup))
				SET @ErrorArgs = @procedureName + N': %%Name=' + @name + N'%%; %%GroupName=' + @parent_device_group_name + N'%%'
				;THROW 60111, @ErrorArgs, 1;
			END
		END

		-- Get DataSet before changes (for DiffGram)
		SET @dataSetBefore = dbo.deviceGroupAsDataSet(@IDDeviceGroup)

		UPDATE [DeviceGroups]
		SET	[Name] = COALESCE(@name, [Name]),
			[Description] = COALESCE(@description, [Description]),
			[LastModified] = @CurrentUtcTime
		WHERE [IDDeviceGroup] = @IDDeviceGroup

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.deviceGroupAsDataSet(@IDDeviceGroup)

		-- Create diffgram
		EXEC [create_task_transaction]
			@type = 'diffgram_update',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetBefore,
			@data_b = @dataSetAfter,
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