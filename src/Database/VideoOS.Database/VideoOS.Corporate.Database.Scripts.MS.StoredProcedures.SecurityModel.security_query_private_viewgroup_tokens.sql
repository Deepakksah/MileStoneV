-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_private_viewgroup_tokens]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_private_viewgroup_tokens]
GO

CREATE PROCEDURE [security_query_private_viewgroup_tokens]
AS
BEGIN
	SET NOCOUNT ON

	SELECT [object_id], [full_path] AS [token]
	FROM SecurityObject
	WHERE namespace_id = 'EC543C56-ADF7-4600-B9AD-97E166552BE9'
		AND parent_path = '/4BF8A8B0-54AB-4E82-994F-98D8E9584176/'

	--SELECT [object_id], [full_path] AS [token]
	--FROM SecurityObject
	--WHERE parent_path <> '' AND parent_path <> '/'
END
GO