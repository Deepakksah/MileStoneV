-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_role_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_role_ids]
GO

CREATE PROCEDURE [security_query_role_ids]
AS
BEGIN
	SET NOCOUNT ON

	SELECT [IDRole]
	FROM [SecurityRoles]
END
GO