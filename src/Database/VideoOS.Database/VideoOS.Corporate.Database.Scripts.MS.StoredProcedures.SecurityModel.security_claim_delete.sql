-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_claim_delete]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [security_claim_delete]
GO

CREATE PROCEDURE [security_claim_delete]
	@IDClaim		INT,
	@sessionId		[GuidType]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		-- Get name for logging
		DECLARE @name NVARCHAR(128)
		DECLARE @displayName NVARCHAR(128)

		SELECT	@name = [ClaimName],
				@displayName = [DisplayName]
		FROM	[SecurityClaim] WITH(UPDLOCK)
		WHERE	[IDClaim] = @IDClaim

		-- Ensure that row exists
		IF (@name IS NULL)
		BEGIN
			;THROW 60350, @procedureName, 1;
		END
		BEGIN TRY
			DELETE	[SecurityClaim]
			WHERE	[IDClaim] = @IDClaim
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 547) -- FK CONSTRAINT VIOLATION
			BEGIN
				-- The claim is used on a role
				;THROW 60370, @procedureName, 1;
			END
			;THROW
		END CATCH

		COMMIT TRAN

		EXEC create_pending_log @sessionId, 9102, @IDClaim, @name -- E.g. Claim '{0}' was deleted.
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