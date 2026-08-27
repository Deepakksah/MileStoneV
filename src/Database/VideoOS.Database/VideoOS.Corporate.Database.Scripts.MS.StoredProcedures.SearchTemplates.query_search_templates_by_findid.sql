-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Search].[query_search_templates_by_findid]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Search].[query_search_templates_by_findid]
GO

CREATE PROCEDURE [Search].[query_search_templates_by_findid]
	@id uniqueidentifier,
	@owner [SIDType]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		-- Remove all FindSearchTemplate rows that are obsolete.
		DELETE [Search].[SearchTemplatesFind]
		WHERE [CreateDate] < DATEADD(mi, -10, GETUTCDATE()) AND [Id] != @id -- Delete all FindSearchTemplates that a 10 minutes old.

		COMMIT TRAN

		DECLARE @read_id uniqueidentifier
		DECLARE @count int
		DECLARE @startIndex int
		DECLARE @keywordFilter nvarchar(256)
		DECLARE @privateOnly bit

		SELECT @read_id = [Id], @count = [Count], @startIndex = [StartIndex], @keywordFilter = [KeywordFilter], @privateOnly = [PrivateOnly]
		FROM [Search].[SearchTemplatesFind]
		WHERE [Id] = @id

		-- Make sure the search template find exists
		IF (@read_id IS NULL)
		BEGIN
			;THROW 66052, @procedureName, 1;
		END

		EXECUTE [Search].[query_search_templates_by_searchquery] @count, @startIndex, @keywordFilter, @privateOnly, @owner
  
		DELETE [Search].[SearchTemplatesFind]
		WHERE [Id] = @id
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