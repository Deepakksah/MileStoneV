-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_patrolling_profiles_by_ids]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_patrolling_profiles_by_ids]
GO

CREATE PROCEDURE [query_patrolling_profiles_by_ids]
	@patrolling_profile_ids tvp_StringTableGuidType READONLY
 AS
BEGIN
	SET NOCOUNT ON

	SELECT 
		CAST([IDPatrollingProfile] AS UNIQUEIDENTIFIER) AS [IDPatrollingProfile],
		[Name],
		CAST([IDDevice] AS UNIQUEIDENTIFIER) AS [IDDevice],
		[Description],
		[LastModified],
		[Definition]
	FROM [PatrollingProfiles] p
	INNER JOIN @patrolling_profile_ids ids ON p.IDPatrollingProfile = ids.ID
END
GO
