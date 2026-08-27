-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_validate_user]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_validate_user]
GO

CREATE PROC [security_validate_user]
	@username NVARCHAR(512),
	@password NVARCHAR(512)
AS
BEGIN
  SET NOCOUNT ON
 
  SELECT 1
END
GO