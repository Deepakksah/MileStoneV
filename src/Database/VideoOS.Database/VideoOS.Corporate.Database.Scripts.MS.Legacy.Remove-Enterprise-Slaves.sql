-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_enterprise_servers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_enterprise_servers]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_legacy_GetEnterpriseServerValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_legacy_GetEnterpriseServerValue]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[securityPermissionsEnterpriseServerAsDataSet]') AND type in (N'FN', N'FS', N'FT', N'IF', N'TF'))
DROP FUNCTION [securityPermissionsEnterpriseServerAsDataSet]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SecurityPermissionsEnterpriseServer]') AND type in (N'U'))
DROP TABLE [SecurityPermissionsEnterpriseServer]
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[EnterpriseServers]') AND type in (N'U'))
DROP TABLE [EnterpriseServers]
GO

DELETE FROM [SecurityAttributes] WHERE SecurityAttributeID LIKE 'ENTERPRISE%'
GO

UPDATE ClientProfile
	SET Settings.modify('delete 
	/Settings/Setting[@key="ToolsMenuEnterpriseServers"][1]') 
	WHERE Settings.exist('/Settings/Setting[@key="ToolsMenuEnterpriseServers"]') = 1 -- EXISTS
	AND profile_type = 'ManagementClientProfile'
GO
