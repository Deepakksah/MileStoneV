-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_site_transaction]') AND type = 'P')
DROP PROCEDURE [admin_update_site_transaction]
GO

CREATE PROCEDURE [admin_update_site_transaction]
	@IDSite GuidType,
	@Name NameType,
	@Description VARCHAR(MAX),
	@Addresses XML,
	@LastModified DateTime,
	@Version VARCHAR(15),
	@ServiceAccount VARCHAR(256),
	@SynchronizationStatus INT
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @CurrentUtcTime DATETIME
	SET @CurrentUtcTime = GETUTCDATE()
	
	UPDATE [Sites] 
	SET [Name] = @Name, 
		[Description] = @Description,
		[Addresses] = @Addresses,
		[LastModified] = @LastModified,
		[LastStatusHandshake] = @CurrentUtcTime,
		[Version] = @Version,
		[ServiceAccount] = @ServiceAccount,
		[SynchronizationStatus] = @SynchronizationStatus
	WHERE IDSite = @IDSite
END
GO
