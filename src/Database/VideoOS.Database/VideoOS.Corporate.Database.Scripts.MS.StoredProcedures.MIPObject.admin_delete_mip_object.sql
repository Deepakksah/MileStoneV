-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Adds stored procedure to delete failover servers.
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_delete_mip_object]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_delete_mip_object]
GO

CREATE PROCEDURE [admin_delete_mip_object]
	@IDMIPObject [UNIQUEIDENTIFIER]
AS
BEGIN
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @id [UNIQUEIDENTIFIER]
		DECLARE @parent_id [UNIQUEIDENTIFIER]

		SELECT @id = [IDMIPObject]
		FROM [MIPObject] WITH(UPDLOCK)
		WHERE [IDMIPObject] = @IDMIPObject
	
		SELECT @parent_id = [IDServerGroup]
		FROM [ServerGroupMembership] WITH(UPDLOCK)
		WHERE [IDObject] = @IDMIPObject

		-- Make sure the MIP object exists
		IF(@id IS NULL)
		BEGIN
			;THROW 60470, @procedureName, 1;
		END

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Get data set before changes
		DECLARE @dataSetBefore XML
		SET @dataSetBefore = dbo.serverGroupMemberAsDataSet(@parent_id, @id)

		-- Remove from ServerGroup
		EXEC remove_server_group_member @IDMIPObject

		BEGIN TRY
			-- Delete the MIP object
			DELETE [MIPObject]
			WHERE [IDMIPObject] = @IDMIPObject
		END TRY
		BEGIN CATCH
			;THROW 60470, @procedureName, 1;
		END CATCH

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