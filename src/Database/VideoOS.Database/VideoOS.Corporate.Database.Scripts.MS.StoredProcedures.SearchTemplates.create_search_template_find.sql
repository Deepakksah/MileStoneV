-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Search].[create_search_template_find]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Search].[create_search_template_find]
GO

CREATE PROCEDURE [Search].[create_search_template_find]
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
		WHERE [CreateDate] < DATEADD(mi, -10, GETUTCDATE()) -- Delete all FindSearchTemplate that a 10 minutes old.

		DECLARE @id uniqueidentifier
		SET @id = NEWID()
		DECLARE @createDate datetime2
		SET @createDate = GETUTCDATE()

		INSERT INTO [Search].[SearchTemplatesFind]
		(
			[Id], [Count], [StartIndex], [KeywordFilter], [PrivateOnly], [CreateDate], [Owner]
		)
		VALUES 
		(
			@id, @count, @startIndex, @keywordFilter, @privateOnly, @createDate, @owner
		)

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