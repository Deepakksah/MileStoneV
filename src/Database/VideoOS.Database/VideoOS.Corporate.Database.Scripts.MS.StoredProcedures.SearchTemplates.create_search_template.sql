-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Search].[create_search_template]') AND type in (N'P', N'PC'))
DROP PROCEDURE [Search].[create_search_template]
GO

CREATE PROCEDURE [Search].[create_search_template]
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

		INSERT INTO [Search].[SearchTemplates]
		(
			[Id], [Name], [Description], [Availability], [IncludesScopeItems], [SearchQuery], [Owner]
		)
		VALUES 
		(
			@id, @name, @description, @availability, @includesScopeItems, @searchQuery, @owner
		)

		COMMIT TRAN

		EXEC create_pending_log @sessionId, 22030, @id, @name

		SELECT [Id] = @Id, [Name] = @name, [description] = @description, [Availability] = @availability, [IncludesScopeItems] = @includesScopeItems, [SearchQuery] = @searchQuery
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