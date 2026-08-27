-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_claim_update]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [security_claim_update]
GO

CREATE PROCEDURE [security_claim_update]
	@IDClaim		INT,
	@name			NVARCHAR(450),
	@displayName	NVARCHAR(128)
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @rowCount INT

		BEGIN TRAN

		-- Ensure that the name is unique for the login provider
		IF @name IS NOT NULL AND EXISTS (
			SELECT	1
			FROM	[SecurityClaim] sc1
					INNER JOIN [SecurityClaim] sc2 ON
					sc1.[IDClaim] <> sc2.[IDClaim] AND
					sc1.[LoginProviderId] = sc2.[LoginProviderId]
					WHERE sc1.[IDClaim] <> @IDClaim AND sc1.[ClaimName] = @name COLLATE Latin1_General_100_BIN)
		BEGIN
			-- The name already exists
			SET @ErrorArgs = @procedureName + N': %%Name=' + @name + N'%%'
			;THROW 60373, @ErrorArgs, 1;
		END
 
		-- Ensure that the display name is unique for the login provider
		IF @displayName IS NOT NULL AND EXISTS (
			SELECT	1
			FROM	[SecurityClaim] SC1
					INNER JOIN [SecurityClaim] sc2 ON
					sc1.[IDClaim] <> sc2.[IDClaim] AND
					sc1.[LoginProviderId] = sc2.[LoginProviderId]
					WHERE SC1.[IDClaim] <> @IDClaim AND SC1.[DisplayName] = @displayName)
		BEGIN
			-- The display name already exists
			SET @ErrorArgs = @procedureName + N': %%DisplayName=' + @displayName + N'%%'
			;THROW 60366, @ErrorArgs, 1;
		END

		UPDATE	[SecurityClaim]
		SET		[ClaimName] = COALESCE(@name, [ClaimName]),
				[DisplayName] = COALESCE(@displayName, [DisplayName])
		WHERE	[IDClaim] = @IDClaim
	
		SELECT @rowCount = @@ROWCOUNT

		IF(@rowCount = 0)
		BEGIN
			-- The row does not exist
			;THROW 60367, @procedureName, 1;
		END

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