-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_device_group]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_device_group]
GO

CREATE PROCEDURE [create_device_group]
	@IDDeviceGroup [GuidType],
	@IDParentDeviceGroup [GuidType] = NULL,
	@DeviceType [InternalIdType],
	@Name [NameType],
	@Description NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @dataSetAfter XML
		DECLARE @parent_device_group_name [NameType]
		DECLARE @parentDeviceType [InternalIdType]
		DECLARE @parentBuiltIn AS BIT

		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		IF (@IDParentDeviceGroup IS NOT NULL)
		BEGIN
			SELECT @parentDeviceType = [DeviceType], @parentBuiltIn = [BuiltIn]
			FROM [DeviceGroups] WITH(UPDLOCK) 
			WHERE [IDDeviceGroup] = @IDParentDeviceGroup

			-- Ensure that parent exists
			IF (@parentDeviceType IS NULL OR @parentDeviceType <> @DeviceType)
			BEGIN
				-- Parent does not exist (with the specified device type)
				;THROW 60106, @procedureName, 1;
			END

			-- Ensure that parent is not a BuiltIn group
			IF (@parentBuiltIn = 1)
			BEGIN
				-- Parent does not exist (with the specified device type)
				;THROW 60107, @procedureName, 1;
			END

		END

		INSERT [DeviceGroups] (
			[LastModified],
			[IDDeviceGroup],
			[IDParentDeviceGroup],
			[DeviceType],
			[Name],
			[Description],
			[BuiltIn],
			[AutoInsertNew],
			[IsUserControlled]
		)
		VALUES (
			@CurrentUtcTime,
			@IDDeviceGroup,
			@IDParentDeviceGroup,
			@DeviceType,
			@Name,
			@Description,
			0,
			0,
			1
		)

		-- Get DataSet after changes
		SET @dataSetAfter = dbo.deviceGroupAsDataSet(@IDDeviceGroup)

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