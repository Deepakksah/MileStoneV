-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dataset_matrix_delete]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dataset_matrix_delete]
GO

CREATE PROCEDURE [dataset_matrix_delete]
	@Original_IDMatrix [GuidType]
AS
BEGIN
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()

	DECLARE @namespace NVARCHAR(260) = 'Matrix'

	-- Unregister security object
	BEGIN TRY
		EXEC [security_unregister_object_transaction]
			@namespace_name = @namespace,
			@object_id = @Original_IDMatrix,
			@CurrentUtcTime = @CurrentUtcTime
	END TRY
	BEGIN CATCH
		SET @ErrorArgs = dbo.[FormatErrorMessage](ERROR_NUMBER(), ERROR_SEVERITY(), ERROR_STATE(), ERROR_PROCEDURE(), ERROR_LINE(), ERROR_MESSAGE()) + N'%%; %%Namespace=' + @namespace + N'%%; %%ID=' + CAST(@Original_IDMatrix AS NVARCHAR(36)) + N'%%'
		;THROW 65001, @ErrorArgs, 1;
	END CATCH

	DELETE [Matrix]
	WHERE [IDMatrix] = @Original_IDMatrix
END
GO