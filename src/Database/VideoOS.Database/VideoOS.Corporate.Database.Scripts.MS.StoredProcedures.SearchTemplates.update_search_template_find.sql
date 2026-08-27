-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Search].[update_search_template_find]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Search].[update_search_template_find]
GO

CREATE PROCEDURE [Search].[update_search_template_find]
	@id uniqueidentifier,
	@count int,
	@startIndex int,
	@keywordFilter nvarchar(256),
	@privateOnly bit,
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

		DECLARE @read_id [GuidType]

		SELECT @read_id = [id]
		FROM [Search].[SearchTemplatesFind] WITH(UPDLOCK)
		WHERE [id] = @id AND [Owner] = @owner
	
		-- Make sure the search template find exists
		IF (@read_id IS NULL)
		BEGIN
			;THROW 66050, @procedureName, 1;
		END

		DECLARE @createDate datetime2
		SET @createDate = GETUTCDATE()

		UPDATE [Search].[SearchTemplatesFind]
		SET		[Count] = @count,
				[StartIndex] = @startIndex,
				[KeywordFilter] = @keywordFilter,
				[PrivateOnly] = @privateOnly,
				[CreateDate] = @createDate
		WHERE [id] = @id AND [Owner] = @owner

		SELECT [Id] = @Id, [Count] = @count, [StartIndex] = @startIndex, [KeywordFilter] = @keywordFilter, [PrivateOnly] = @privateOnly
	
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