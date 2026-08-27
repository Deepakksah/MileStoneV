-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_delete_matrix_access_entry]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_delete_matrix_access_entry]
GO

CREATE PROCEDURE [security_delete_matrix_access_entry]
	@SecurityAtribute NVARCHAR(100),
	@IDRole UNIQUEIDENTIFIER,
	@IDMatrix UNIQUEIDENTIFIER,
	@MatrixName NVARCHAR(250),
	@session_id [GuidType]
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()

		-- If there is no changes, let's return
		IF NOT EXISTS (SELECT [Value] FROM [SecurityPermissionsMatrix] 
			WHERE [SecurityAttributeID]  = @SecurityAtribute AND
				[IDRole] = @IDRole AND
				[IDMatrix] = @IDMatrix)
		BEGIN
			RETURN 0		-- no change
		END

		BEGIN TRAN

		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		-- delete access entry
		DELETE FROM [SecurityPermissionsMatrix]
		WHERE   [SecurityAttributeID] = @SecurityAtribute AND
				[IDRole] = @IDRole AND
				[IDMatrix] = @IDMatrix
	

		-- Audit log 
		DECLARE @namespace_id NVARCHAR(200)
		SET @namespace_id = '988D493D-1597-4BE6-AFF0-396BFD5D047A'

		DECLARE @namespaceName NVARCHAR(250)
		SET @namespaceName = '{MatrixApplicationName}' -- Matrix OEM

		DECLARE @roleName NameType
		SELECT @roleName= [Name] FROM [SecurityRoles] WHERE [IDRole] = @IDRole

		DECLARE @actionDisplayName NVARCHAR(260)
		SELECT @actionDisplayName = [display_name] FROM [SecurityAction] WHERE [namespace_id] = @namespace_id AND [legacy_security_name] = @SecurityAtribute

		IF(@actionDisplayName IS NULL OR @actionDisplayName = '')
		BEGIN
			SET @actionDisplayName = @SecurityAtribute
		END

		EXEC create_pending_log @session_id, 9015, @namespaceName, @actionDisplayName, @IDMatrix, @MatrixName, @IDRole, @roleName

		-- TODO: We need to handle if security changed
		SELECT CAST(1 AS BIT) AS security_changed

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