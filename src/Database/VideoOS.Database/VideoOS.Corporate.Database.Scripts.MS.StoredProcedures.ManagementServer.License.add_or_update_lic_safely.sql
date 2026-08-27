-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[add_or_update_lic_safely]') AND [type] IN (N'P', N'PC'))
	DROP PROCEDURE [add_or_update_lic_safely]
GO

CREATE PROCEDURE [add_or_update_lic_safely]
	@IDLicense UNIQUEIDENTIFIER,
	@PreviousLastModified [DATETIME],
	@Data VARBINARY(MAX)
	
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @operation VARCHAR(10)
		DECLARE @last_modified_current DATETIME

		DECLARE @CurrentUtcTime DATETIME
		DECLARE @rowCount INT

		BEGIN TRANSACTION

		SELECT  @last_modified_current = l.[LastModified] 
		FROM [License] l WITH (UPDLOCK, ROWLOCK, HOLDLOCK)
		WHERE l.[IDLicense] = @IDLicense

		SET @rowCount = @@ROWCOUNT

		SET @CurrentUtcTime = GETUTCDATE()

		IF  @rowCount = 0
		BEGIN
			SET @operation = N'INSERT'
			INSERT [License] (
				[IDLicense],
				[LastModified],
				[Data]
			)
			VALUES (
				@IDLicense,
				@CurrentUtcTime,
				@Data
			)
		END
		ELSE
		BEGIN
				-- Check that last modified doesn't prevent user from updating object
			IF (@last_modified_current <> @PreviousLastModified)
			BEGIN
				;THROW 60252, @procedureName, 1;
			END

			SET @operation = N'UPDATE'

			UPDATE [License]
			SET [LastModified] = @CurrentUtcTime,
			[Data] = @Data
			WHERE [IDLicense] = @IDLicense
		END

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
