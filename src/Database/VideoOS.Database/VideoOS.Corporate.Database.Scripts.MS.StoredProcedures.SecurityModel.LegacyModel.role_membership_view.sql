-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[udfRoleMembers]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION dbo.udfRoleMembers;
GO

CREATE FUNCTION dbo.udfRoleMembers()
RETURNS @roleMembers TABLE 
(
    [IDRole] [GuidType],
    [MemberSID] [SIDType]
)
AS
BEGIN
	DECLARE @currentLevel INT
	SET @currentLevel = 0

	;WITH GroupList([IDRole], [MemberSID], [MemberLevel], [IsCycle], [Path]) AS
	(
		SELECT 
			sm.[Container], 
			sm.[MemberSID], 
			@currentLevel, 
			0 AS IsCycle, 
			'.' + CAST(sm.[MemberSID] AS VARCHAR(MAX)) + '.' AS [Path]
		FROM [SecurityMembership] sm

		UNION ALL

		SELECT 
			sm.[Container], 
			gl.[MemberSID], 
			gl.[MemberLevel] + 1,
			CASE WHEN gl.[Path] LIKE '%.' + CAST(sm.[MemberSID] AS VARCHAR(MAX)) + '.%' THEN 1 ELSE 0 END AS IsCycle,
			gl.[Path] + CAST(sm.[MemberSID] AS VARCHAR(MAX)) + '.' AS [Path]

		FROM [SecurityMembership] sm
		INNER JOIN GroupList gl ON sm.[MemberSID] = gl.[IDRole]
		WHERE gl.[MemberLevel] < 100 AND gl.[IsCycle] = 0
	)
	INSERT @roleMembers
	SELECT DISTINCT [IDRole], [MemberSID]
	FROM GroupList
	INNER JOIN [SecurityIdentity] si ON GroupList.[IDRole] = si.[SID] AND si.[Type] = 4
	ORDER BY IDRole

	RETURN
END
GO

IF EXISTS(SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[RoleMembership]'))
DROP VIEW [RoleMembership]
GO

CREATE VIEW [RoleMembership]
AS
	SELECT rm.[IDRole], rm.[MemberSID]
	FROM dbo.udfRoleMembers() rm
GO
