-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Adds stored procedure to delete failover servers.
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_delete_mip_action]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_delete_mip_action]
GO

CREATE PROCEDURE [admin_delete_mip_action]
	@IDMIPAction [UNIQUEIDENTIFIER]
AS
BEGIN
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @id [UNIQUEIDENTIFIER]

		SELECT @id = [IDMIPAction]
		FROM [MIPAction] WITH(UPDLOCK)
		WHERE [IDMIPAction] = @IDMIPAction
	
		-- Make sure the MIP action exists
		IF(@id IS NULL)
		BEGIN
			;THROW 60471, @procedureName, 1;
		END

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
		
		BEGIN TRY
			-- Delete the MIP object
			DELETE [MIPAction]
			WHERE [IDMIPAction] = @IDMIPAction
		END TRY
		BEGIN CATCH
			;THROW 60471, @procedureName, 1;
		END CATCH

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