-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_home_site]') AND type = 'P')
DROP PROCEDURE [admin_update_home_site]
GO

CREATE PROCEDURE [admin_update_home_site]
	@Name NameType,
	@Description VARCHAR(MAX),
	@Addresses XML,
	@Version VARCHAR(15),
	@ServiceAccount VARCHAR(256),
	@SynchronizationStatus INT,
	@custom_properties XML
AS
BEGIN
	SET NOCOUNT ON

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	DECLARE @ErrorArgs NVARCHAR(500)

	BEGIN TRY
		BEGIN TRANSACTION

		DECLARE @CurrentUtcTime DATETIME
		SET @CurrentUtcTime = GETUTCDATE()
	
		-- Does the home site exist?
		IF NOT EXISTS(
			SELECT *
			FROM [Sites] WITH(UPDLOCK)
			WHERE [IDSiteType] = 1)
		BEGIN
			;THROW 60404, @procedureName, 1;
		END

		IF
		(	
			@Name IS NULL AND 
			@Description IS NULL AND 
			@Addresses IS NULL AND 
			@Version IS NULL AND 
			@ServiceAccount IS NULL AND 
			@SynchronizationStatus IS NULL AND 
			@custom_properties IS NULL
		)
		BEGIN
			ROLLBACK TRAN
			RETURN 0
		END
	
		DECLARE @HomeSiteID GuidType
		SET @HomeSiteID = (SELECT [IDSite] FROM Sites WHERE [IDSiteType] = 1)
	
		-- Update the home site
		UPDATE [Sites] 
		SET [Name] = COALESCE(@Name, [Name]), 
			[Description] = COALESCE(@Description, [Description]),
			[Addresses] = COALESCE(@Addresses, [Addresses]),
			[LastModified] = @CurrentUtcTime,
			[LastStatusHandshake] = @CurrentUtcTime,
			[Version] = COALESCE(@Version, [Version]),
			[ServiceAccount] = COALESCE(@ServiceAccount, [ServiceAccount]),
			[SynchronizationStatus] = COALESCE(@SynchronizationStatus, [SynchronizationStatus])
		WHERE IDSite = @HomeSiteID

		-- Update custom properties
		IF(@custom_properties IS NOT NULL)
		BEGIN
			EXEC [update_custom_properties_transaction]
				@object_id = '909749AE-88C4-4780-A381-383D691E98FA',
				@custom_properties = @custom_properties
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