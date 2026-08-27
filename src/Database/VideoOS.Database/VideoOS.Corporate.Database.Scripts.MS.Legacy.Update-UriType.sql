-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- First drop stored procedures that currently use the UriType
-- The stored procedures will be created again after all update scripts are completed.

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[recorder_create_or_update]') AND type in (N'P', N'PC'))
DROP PROCEDURE [recorder_create_or_update]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_create_failover_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_create_failover_server]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_query_failover_server_by_hostname]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_query_failover_server_by_hostname]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[admin_update_failover_server]') AND type in (N'P', N'PC'))
DROP PROCEDURE [admin_update_failover_server]
GO


-- Rename the original type (the ntext based type)
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'UriType' AND ss.name = N'dbo')
EXEC sp_rename N'UriType', N'UriTypeNText', N'USERDATATYPE'
GO

-- Create the new type (nvarchar(max))
CREATE TYPE [dbo].[UriType] FROM NVARCHAR(MAX) NOT NULL
GO


-- Now update all tables
ALTER TABLE [EnterpriseServers] ALTER COLUMN [Address] [UriType] NOT NULL

ALTER TABLE [FailoverServers] ALTER COLUMN [HostName] [UriType] NULL
ALTER TABLE [FailoverServers] ALTER COLUMN [WebServerUri] [UriType] NULL

ALTER TABLE [Hardware] ALTER COLUMN [URI] [UriType] NOT NULL

ALTER TABLE [LogAuditVMOQueue] ALTER COLUMN [UserLocation] [UriType] NOT NULL

ALTER TABLE [RegisteredServers] ALTER COLUMN [Addresses] [UriType] NOT NULL

-- Finally drop the old ntext based type (should no longer be referenced)
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'UriTypeNText' AND ss.name = N'dbo')
DROP TYPE [dbo].[UriTypeNText]
GO
