-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_management_server_name]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_management_server_name]
GO

CREATE PROCEDURE [query_management_server_name]
	@ServerId [GuidType]
AS
BEGIN
	SET NOCOUNT ON
	
	IF ((SELECT  IDSite
		FROM Sites 
		WHERE IDSiteType = 1) = @ServerId COLLATE SQL_Latin1_General_CP1_CI_AS)
	BEGIN
		SELECT Setting
		FROM SystemSettings
		WHERE Name = 'ManagementServerHostName'		
	END 
END
GO