-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_delete_namespace]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_delete_namespace]
GO

CREATE PROCEDURE [security_delete_namespace]
	@namespace_id UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		DECLARE @userDefined TINYINT

		BEGIN TRAN

		SELECT @userDefined = [user_defined]
		FROM [SecurityNamespace]
		WHERE [namespace_id] = @namespace_id

		IF(@userDefined IS NULL OR @userDefined = 0)
		BEGIN
			;THROW 60325, @procedureName, 1;
		END

		DELETE FROM [SecurityAccessEntry]
		WHERE security_object_id IN
		(
			SELECT security_object_id from SecurityObject
			WHERE [namespace_id] = @namespace_id
		)

		DELETE FROM [SecurityObject]
		WHERE [namespace_id] = @namespace_id

		DELETE FROM [SecurityAction]
		WHERE [namespace_id] = @namespace_id

		DELETE FROM [SecurityNamespace]
		WHERE [namespace_id] = @namespace_id

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