-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- DeleteKind - removes a MIP Kind from the table
IF OBJECT_ID ( '[Mip].[DeleteKind]', 'P' ) IS NOT NULL 
	DROP PROCEDURE [Mip].[DeleteKind];
GO

CREATE PROCEDURE [Mip].[DeleteKind]
	@KindId UNIQUEIDENTIFIER
AS
BEGIN
	SET NOCOUNT ON
	
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	IF EXISTS (SELECT * FROM [Mip].[Items] WHERE [Kind] = @KindId)
	BEGIN
		;THROW 60475, @procedureName, 1;
	END

	DELETE FROM [Mip].[Kinds]
	WHERE [KindId] = @KindId
END
GO