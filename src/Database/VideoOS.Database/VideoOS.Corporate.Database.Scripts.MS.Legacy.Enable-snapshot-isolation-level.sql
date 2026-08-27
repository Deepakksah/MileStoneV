-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- SUPPORTS_TRANSACTION: FALSE
-- =========================================================================

ALTER DATABASE [{DatabaseName}] SET  SINGLE_USER WITH ROLLBACK IMMEDIATE

ALTER DATABASE [{DatabaseName}] SET READ_COMMITTED_SNAPSHOT ON

ALTER DATABASE [{DatabaseName}] SET ALLOW_SNAPSHOT_ISOLATION ON

ALTER DATABASE [{DatabaseName}] SET  MULTI_USER WITH ROLLBACK IMMEDIATE
GO
