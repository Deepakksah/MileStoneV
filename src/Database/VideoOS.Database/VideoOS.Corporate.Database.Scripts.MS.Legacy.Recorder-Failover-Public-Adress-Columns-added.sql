-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Adds columns WebServerUri and WebServerUriConfiguration to FailoverServers
IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND 
		name = N'WebServerUri')
BEGIN
	ALTER TABLE [FailoverServers]
		ADD [WebServerUri] [dbo].[UriType] NULL
END
GO

IF NOT EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[FailoverServers]') AND 
		name = N'WebServerUriConfiguration')
BEGIN
	ALTER TABLE [FailoverServers]
		ADD [WebServerUriConfiguration] [ntext] NULL
END
GO