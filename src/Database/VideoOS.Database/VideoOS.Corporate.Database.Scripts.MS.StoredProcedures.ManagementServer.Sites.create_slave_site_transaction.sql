-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[create_slave_site_transaction]') AND type = 'P')
DROP PROCEDURE [create_slave_site_transaction]
GO

CREATE PROCEDURE [create_slave_site_transaction]
	@IDSite GuidType, 
	@Name NameType,
	@Description VARCHAR(MAX),
	@IDSiteState INT,
	@Addresses XML,
	@IDMasterSite GuidType,
	@LastModified DateTime,
	@LastStatusHandshake DateTime,
	@Version VARCHAR(15),
	@ServiceAccount VARCHAR(256),
	@SynchronizationStatus INT
AS
BEGIN
	SET NOCOUNT ON
 
	
	 -- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)

	-- Make sure that a current transaction is active
	IF(@@TRANCOUNT = 0)
	BEGIN
		;THROW 60008, @procedureName, 1;
	END

	DECLARE @IDSiteType INT
	SET @IDSiteType = 3
	
	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()
	
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
		@IDSiteType,
		@Name,
		@Description,
		@IDSiteState,
		@LastModified,
		@LastStatusHandshake,
		@Addresses,
		@Version,
		@ServiceAccount,
		@SynchronizationStatus
	)
	
	-- Create site relation
	INSERT INTO [SitesRelation] (IDSiteMaster, IDSiteSlave)
	VALUES (@IDMasterSite, @IDSite)

	-- Register the object in the security system
	EXEC security_register_object_transaction
		@namespace_name = 'Site',
		@object_id = @IDSite,
		@parent_path = '/',
		@CurrentUtcTime = @CurrentUtcTime

	RETURN 0
END
GO