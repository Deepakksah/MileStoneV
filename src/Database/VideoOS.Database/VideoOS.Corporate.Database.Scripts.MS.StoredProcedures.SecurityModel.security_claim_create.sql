-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_claim_create]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [security_claim_create]
GO

CREATE PROCEDURE [security_claim_create]
	@loginProviderId	UNIQUEIDENTIFIER,
	@name				NVARCHAR(450),
	@displayName		NVARCHAR(128),
	@caseSensitive		BIT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @claimId BIGINT

		BEGIN TRAN

		-- Ensure that the name is unique for the login provider
		IF EXISTS (	SELECT	1
					FROM	[SecurityClaim] WITH(UPDLOCK)
					WHERE	[LoginProviderId] = @loginProviderId AND [ClaimName] = @name)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%Name=' + @name + N'%%'
			;THROW 60348, @ErrorArgs, 1;
		END
 
		-- Ensure that the display name is unique for the login provider
		IF EXISTS (	SELECT	1
					FROM	[SecurityClaim] WITH(UPDLOCK)
					WHERE	[LoginProviderId] = @loginProviderId AND [DisplayName] = @displayName)
		BEGIN
			SET @ErrorArgs = @procedureName + N': %%DisplayName=' + @displayName + N'%%'
			;THROW 60349, @ErrorArgs, 1;
		END
	
		INSERT	[SecurityClaim] ([LoginProviderId], [ClaimName], [DisplayName], [CaseSensitive])
		VALUES (@loginProviderId, @name, @displayName, @caseSensitive)

		SELECT	@claimId = SCOPE_IDENTITY()

		COMMIT TRAN

		SELECT	[IDClaim],
				[LoginProviderId],
				[ClaimName],
				[CaseSensitive],
				[DisplayName]
		FROM	[SecurityClaim]
		WHERE	[IDClaim] = @claimId
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