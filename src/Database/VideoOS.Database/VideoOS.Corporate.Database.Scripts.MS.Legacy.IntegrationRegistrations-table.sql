-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Mip].[IntegrationRegistrations]') AND type = 'U')
BEGIN
	CREATE TABLE [Mip].[IntegrationRegistrations]
	(
		[InstanceId] NVARCHAR(256) NOT NULL,
		[IntegrationId] UNIQUEIDENTIFIER NOT NULL,
		[Type] NVARCHAR(32) NOT NULL, -- Standalone, SmartClient, ManagementClient, EventServer, DeviceDriver
		[IntegrationVersion] NVARCHAR(256) NOT NULL,
		[IntegrationName] NVARCHAR(256) NOT NULL,
		[ManufacturerName] NVARCHAR(256) NOT NULL,
		[RegistrationTime] DATETIME NOT NULL,
		CONSTRAINT PK_IntegrationRegistrations PRIMARY KEY (InstanceId, IntegrationId, Type)
	)
END
GO