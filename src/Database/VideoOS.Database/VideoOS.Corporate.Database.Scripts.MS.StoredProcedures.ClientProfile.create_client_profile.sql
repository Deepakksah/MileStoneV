-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_client_profile]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_client_profile]
GO

CREATE PROCEDURE [create_client_profile]
	@IDClientProfile [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
	@settings NVARCHAR(MAX),
	@profile_type VARCHAR(50)
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

		DECLARE @newPriority INT

		SELECT @newPriority = MAX(Priority) + 1
		FROM ClientProfile
		WHERE profile_type = @profile_type

		BEGIN TRY
			INSERT INTO [ClientProfile] 
			(
				IDClientProfile
				, Name
				, Description
				, LastModified
				, Priority
				, DefaultProfile
				, Settings
				, profile_type
			)
			VALUES 
			(
				@IDClientProfile,
				@name,
				@description,
				@CurrentUtcTime,
				@newPriority,
				0,
				@settings,
				@profile_type
			)
		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists on the server
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%'
				;THROW 60701, @ErrorArgs, 1;
			END
			;THROW
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