-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[add_token]') AND type in (N'P', N'PC'))
DROP PROCEDURE [add_token]
GO

CREATE PROCEDURE [add_token]
	@SID SIDType,
	@Expires bigint,
	@ManagementClientToken BIT
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @ServerLanAddress NVARCHAR(MAX)
		DECLARE @ServerWanAddress NVARCHAR(MAX)
		DECLARE @Token NVARCHAR(MAX)
		SELECT @ServerLanAddress = 
			CASE
				WHEN [Setting] IS NULL OR LEN([Setting]) = 0 THEN '' 
				WHEN [Setting] LIKE '%/' THEN REPLACE([Setting], N'http://', '') + N'/ServerConnector'
				ELSE REPLACE([Setting], N'http://', '') + N'//ServerConnector'
			END
		FROM [SystemSettings]
		WHERE [Name] = 'ServerLanAddress'

		SELECT @ServerWanAddress = 
			CASE
				WHEN [Setting] IS NULL OR LEN([Setting]) = 0 THEN '' 
				WHEN [Setting] LIKE '%/' THEN REPLACE([Setting], N'http://', '') + N'/ServerConnector'
				ELSE REPLACE([Setting], N'http://', '') + N'//ServerConnector'
			END
		FROM [SystemSettings]
		WHERE [Name] = 'ServerWanAddress'

		SELECT @Token = 'TOKEN#' + LOWER(CAST(NEWID() AS NVARCHAR(MAX))) + '#' + @ServerLanAddress + '#' + @ServerWanAddress

		INSERT INTO Token (TokenId, SID, Expires, RegistrationTime, ManagementClientToken)
		VALUES (@Token, @SID, @Expires, GETUTCDATE(), @ManagementClientToken )
	
		SELECT [TokenId], [SID], [Expires], [RegistrationTime], [ManagementClientToken]
		FROM [Token]
		WHERE [TokenId] = @Token

		COMMIT TRANSACTION
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