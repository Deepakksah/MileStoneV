-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Search].[update_search_template]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Search].[update_search_template]
GO

CREATE PROCEDURE [Search].[update_search_template]
	@id uniqueidentifier,
	@name [NameType],
	@description [DescriptionType],
	@availability nvarchar(40),
	@includesScopeItems bit,
	@searchQuery varbinary(max),
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
			;THROW 66050, @procedureName, 1;
		END

		DECLARE @updated TABLE([Id] uniqueidentifier, [Name] [NameType], [Description] [DescriptionType], [IncludesScopeItems] bit)
		
		UPDATE [Search].[SearchTemplates]
		SET		[name] = @name,
				[description] = @description,
				[Availability] = @availability,
				[IncludesScopeItems] = @includesScopeItems,
				[SearchQuery] = @searchQuery,
				[Owner] = @owner
		OUTPUT deleted.Id, deleted.Name, deleted.Description, deleted.IncludesScopeItems INTO @updated
		WHERE [Id] = @id AND (([Availability] = 'Private' AND [Owner] = @owner) OR ([Availability] != 'Private'))
		
		COMMIT TRAN

		DECLARE @oldName [NameType], @oldDescription [DescriptionType], @oldIncludesScopeItems bit

		SELECT @oldName = [Name], @oldDescription = [Description], @oldIncludesScopeItems = [IncludesScopeItems] FROM @updated WHERE [Id] = @id

		IF (@oldName <> @name)
		BEGIN
			EXEC create_pending_log @sessionId, 22033, @id, @name, @oldName

			IF (@oldDescription <> @description OR @oldIncludesScopeItems <> @includesScopeItems)
			BEGIN
				EXEC create_pending_log @sessionId, 22031, @id, @name
			END
		END
		ELSE
		BEGIN
			EXEC create_pending_log @sessionId, 22031, @id, @name
		END

		SELECT [Id] = @Id, [Name] = @name, [Description] = @description, [Availability] = @availability, [IncludesScopeItems] = @includesScopeItems, [SearchQuery] = @searchQuery
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