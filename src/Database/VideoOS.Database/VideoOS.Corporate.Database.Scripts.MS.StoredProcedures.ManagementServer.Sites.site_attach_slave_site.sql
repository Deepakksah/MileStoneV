-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[site_attach_slave_site]') AND type = 'P')
DROP PROCEDURE [site_attach_slave_site]
GO

CREATE PROCEDURE [site_attach_slave_site]
	@IDSite GuidType, 
	@Name NameType,
	@Description VARCHAR(MAX),
	@Addresses XML,
	@Version VARCHAR(15),
	@ServiceAccount VARCHAR(256),
	@SynchronizationStatus INT
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
	
		IF EXISTS(SELECT * FROM Sites WHERE IDSite = @IDSite)
		BEGIN
			-- Site already in config
			;THROW 60402, @procedureName, 1;
		END
	
		-- Does the home site exist?
		IF NOT EXISTS(
			SELECT *
			FROM [Sites] WITH(UPDLOCK)
			WHERE [IDSiteType] = 1)
		BEGIN
			;THROW 60404, @procedureName, 1;
		END
	
		DECLARE @HomeSiteID GuidType
		SET @HomeSiteID = (SELECT [IDSite] FROM Sites WHERE [IDSiteType] = 1)
	
		-- Create the site
		INSERT INTO [Sites] 
		(
			[IDSite],
			[IDSiteType],
			[Name],
			[Description],
			[IDSiteState],
			[LastModified],
			[LastStatusHandshake],
			[Addresses],
			[Version],
			[ServiceAccount],
			[SynchronizationStatus]
		)
		VALUES 
		(
			@IDSite,
			3, -- Slave
			@Name,
			@Description,
			2, -- AttachPending       
			@CurrentUtcTime,
			@CurrentUtcTime,
			@Addresses,
			@Version,
			@ServiceAccount,
			@SynchronizationStatus
		)
	
		-- Create site relation
		INSERT INTO [SitesRelation] (IDSiteMaster, IDSiteSlave)
		VALUES (@HomeSiteID, @IDSite)

		-- Register the object in the security system
		EXEC security_register_object_transaction
			@namespace_name = 'Site',
			@object_id = @IDSite,
			@parent_path = '/',
			@CurrentUtcTime = @CurrentUtcTime

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