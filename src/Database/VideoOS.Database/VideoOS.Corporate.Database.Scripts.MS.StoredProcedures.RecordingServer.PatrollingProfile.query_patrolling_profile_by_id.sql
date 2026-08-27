-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================
IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_patrolling_profile_by_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_patrolling_profile_by_id]
GO

CREATE PROCEDURE [query_patrolling_profile_by_id]
	@patrolling_profile_id [GuidType]
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
	FROM [PatrollingProfiles]
	WHERE [IDPatrollingProfile] = @patrolling_profile_id
END
GO
