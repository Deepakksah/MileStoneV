-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_userupdatedcache_query_with_cleanup]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_userupdatedcache_query_with_cleanup]
GO

CREATE PROCEDURE [security_userupdatedcache_query_with_cleanup]
AS
BEGIN
	SET NOCOUNT ON

	DELETE FROM SecurityUserUpdatedCache WHERE( 
	CAST(IDUser AS char(36)) <> '00000000-0000-0000-0000-000000000000' AND
	CAST(IDUser AS char(36)) NOT IN (
	SELECT [SID] FROM SecurityIdentity
	WHERE Deleted = 0))

	SELECT
		[IDUser],
		[LastModified]
	FROM [SecurityUserUpdatedCache]
END
GO