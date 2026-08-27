-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_serverkind_group]') AND type = 'P')
DROP PROCEDURE [create_serverkind_group]
GO

CREATE PROCEDURE [create_serverkind_group]
	@IDServerGroup [GuidType],
	@Name [NameType],
	@Description NVARCHAR(1024),
	@Kind [GuidType],
	@PluginId [GuidType],
	@CustomData NVARCHAR(MAX),
	@IDParentGroup [GuidType],
	@StructureType VARCHAR(64),
	@UserSID [SIDType],
	@custom_properties XML = NULL
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		DECLARE @ParentGroupName [NameType]
		--DECLARE @StructureType NVARCHAR(64)
		DECLARE @ParentPath NVARCHAR(MAX)
		DECLARE @Level INT
		DECLARE @OrderNo INT

		-- Construct the path
		SELECT
			@ParentGroupName = [Name],
			@ParentPath = [path] + @Name + '\',
			--@StructureType = [StructureType],
			@Level = [Level] + 1
		FROM [ServerGroup] WITH(UPDLOCK)
		WHERE [IDServerGroup] = @IDParentGroup

		-- parent group must exist
		IF(@ParentGroupName IS NULL)
		BEGIN
			;THROW 60056, @procedureName, 1;
		END

		-- Allow a maximum of 12 levels
		IF(@level = 12)
		BEGIN
			;THROW 60057, @procedureName, 1;
		END

		-- Determine default new order number	
		SELECT @OrderNo = COUNT(*)
		FROM [ServerGroup]
		WHERE [IDParentGroup] = @IDParentGroup

		BEGIN TRY
			INSERT [ServerGroup] (
				[IDServerGroup],
				[Name],
				[Description],
				[IDParentGroup],
				[StructureType],
				[OrderNo],
				[Level],
				[Path],
				[CreatedDate],
				[LastModified]
			)
			VALUES (
				@IDServerGroup,
				@Name,
				@Description,
				@IDParentGroup,
				@StructureType,
				@OrderNo,
				@level,
				@ParentPath,
				@CurrentUtcTime,
				@CurrentUtcTime
			)

			UPDATE [ServerGroup]
			SET [RelationIds] = dbo.GetServerGroupRelations(@IDServerGroup)
			WHERE [IDServerGroup] = @IDServerGroup

			INSERT [ServerGroupKind] (
				[IDServerGroup],
				[Kind],
				[PluginId],
				[CustomData]
			)
			VALUES (
				@IDServerGroup,
				@Kind,
				@PluginId,
				@CustomData
			)
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- INDEX VIOLATION
			BEGIN
				SET @ErrorArgs = @procedureName + N': %%Name=' + @Name + N'%%; %%groupName=' + @ParentGroupName + N'%%'
				;THROW 60055, @ErrorArgs, 1;
			END
			;THROW
		END CATCH

		-- Update custom properties
		EXEC [update_custom_properties_transaction]
			@object_id = @IDServerGroup,
			@custom_properties = @custom_properties
	
		DECLARE @class_id VARCHAR(128)
		SET @class_id = 'SERVER_GROUP'
	
		-- Get DataSet after changes
		DECLARE @dataSetAfter XML
		SET @dataSetAfter = dbo.serverGroupAsDataSet(@IDServerGroup)
	
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