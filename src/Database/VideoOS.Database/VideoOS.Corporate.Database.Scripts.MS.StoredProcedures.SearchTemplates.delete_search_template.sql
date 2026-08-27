-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Search].[delete_search_template]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Search].[delete_search_template]
GO

CREATE PROCEDURE [Search].[delete_search_template]
	@id uniqueidentifier,
	@owner [SIDType],
	@sessionId [GuidType]
AS
BEGIN
	SET NOCOUNT ON
 
	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRAN

		DECLARE @read_id [GuidType]

		SELECT @read_id = [Id]
		FROM [Search].[SearchTemplates] WITH(UPDLOCK)
		WHERE [Id] = @id AND (([Availability] = 'Private' AND [Owner] = @owner) OR ([Availability] != 'Private'))
	
		-- Make sure the search template exists
		IF (@read_id IS NULL)
		BEGIN
			;THROW 66051, @procedureName, 1;
		END

		DECLARE @deleted TABLE([Id] uniqueidentifier, [Name] [NameType], [Description] [DescriptionType], [Availability] nvarchar(40), [IncludesScopeItems] bit, [SearchQuery] varbinary(max))
		
		DELETE FROM [Search].[SearchTemplates]
		OUTPUT deleted.[Id], deleted.[Name], deleted.[Description], deleted.[Availability], deleted.[IncludesScopeItems], deleted.[SearchQuery] INTO @deleted
		WHERE [Id] = @id AND (([Availability] = 'Private' AND [Owner] = @owner) OR ([Availability] != 'Private'))

		COMMIT TRAN

		DECLARE @name [NameType]
		SET @name = (SELECT [Name] FROM @deleted WHERE [Id] = @id)
		IF (@name IS NOT NULL)
		BEGIN
			EXEC create_pending_log @sessionId, 22032, @id, @name
		END

		SELECT [Id], [Name], [Description], [Availability], [IncludesScopeItems], [SearchQuery]
		FROM @deleted
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