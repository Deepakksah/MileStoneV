-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_create_role]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_create_role]
GO

CREATE PROCEDURE [security_create_role]
	@IDRole [GuidType],
	@Name [NameType],
	@Description [DescriptionType],
	@DualAuthorizationRequired BIT,
	@PtzReservedAsAnonymous BIT,
	@AllowMobileClientLogOn BIT,
	@AllowSmartClientLogOn BIT,
	@AllowWebClientLogOn BIT
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @LastModified DATETIME

		BEGIN TRAN

		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		SET @LastModified = GETUTCDATE()

		EXEC [security_create_role_transaction] @IDRole, 2, @Name, @Description, @DualAuthorizationRequired, @PtzReservedAsAnonymous, @AllowMobileClientLogOn, @AllowSmartClientLogOn, @AllowWebClientLogOn, @LastModified, NULL, NULL

		-- Add default Smart Client login permission
		INSERT INTO [SecurityPermissions] ([LastModified], [IDRole], [SecurityAttributeID], [Value], [IDTimeProfile], [IsDefaultTimeProfile])
		VALUES (@LastModified, @IDRole, 'APPLICATIONACCESSCLIENTLOGIN', 'True', null, 1)

		DECLARE @dataSetSecurityPermissionsAfter XML
		SET @dataSetSecurityPermissionsAfter = dbo.securityPermissionsAsDataSet(@IDRole)

		EXEC [create_task_transaction]
			@type = 'diffgram_security_insert',
			@target = '909749AE-88C4-4780-A381-383D691E98FA',
			@data_a = @dataSetSecurityPermissionsAfter,
			@current_time = @LastModified

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