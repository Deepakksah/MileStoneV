-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_update_role_identity]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_update_role_identity]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_update_identities]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_update_identities]
GO

CREATE PROCEDURE [security_update_identities]
	@CurrentUtcTime DATETIME
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @membersChanged BIT

		-- TODO: We still need to handle this better
		SET @membersChanged = 1

		BEGIN TRAN

		-- Lock tables for update
		EXEC [security_table_lock] @LockLevel = 0

		-- Update existing members
		UPDATE [SecurityIdentity]
		SET [Type] = tmp.[Type],
			[DisplayName] = tmp.[DisplayName],
			[Description] = tmp.[Description],
			[Domain] = tmp.[Domain],
			[AccountName] = tmp.[AccountName],
			[DistinguishedName] = tmp.[DistinguishedName],
			[Deleted] = 0,
			[LastModified] = @CurrentUtcTime
		FROM [SecurityIdentity] si
		INNER JOIN [TempSecurityIdentities] tmp ON si.[SID] = tmp.[SID] AND tmp.[SPID] = @@SPID
		WHERE si.[DisplayName] != tmp.[DisplayName]
			OR si.[Description] != tmp.[Description]
			OR si.[Domain] != tmp.[Domain]
			OR si.[AccountName] != tmp.[AccountName]
			OR si.[DistinguishedName] != tmp.[DistinguishedName]

		SELECT @membersChanged AS [MembersChanged]

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