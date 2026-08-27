-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_matrix]') AND type in (N'P', N'PC'))
DROP PROCEDURE [create_matrix]
GO

CREATE PROCEDURE [create_matrix]
	@IDMatrix [GuidType],
	@name [NameType],
	@description NVARCHAR(MAX),
	@address VARCHAR(256),
	@port int,
	@password VARCHAR(256)
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

		BEGIN TRY
			INSERT [Matrix] (
				[IDMatrix],
				[Name],
				[Description],
				[Address],
				[Port],
				[Password],
				[LastUpdated]
			)
			VALUES (
				@IDMatrix,
				@Name,
				@description,
				@address,
				@port,
				@password,
				@CurrentUtcTime
			)

			-- Register the object in the security system
			EXEC security_register_object_transaction
				@namespace_name = 'Matrix',
				@object_id = @IDMatrix,
				@parent_path = '/',
				@CurrentUtcTime = @CurrentUtcTime

		END TRY
		BEGIN CATCH
			IF(ERROR_NUMBER() = 2601 OR ERROR_NUMBER() = 2627) -- CONSTRAINT VIOLATION
			BEGIN
				-- The name already exists 
				SET @ErrorArgs = @procedureName + N': %%name=' + @name + N'%%'
				;THROW 60212, @ErrorArgs, 1;
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
