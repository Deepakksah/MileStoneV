-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[security_query_roles_by_user_sid]') AND type in (N'P', N'PC'))
DROP PROCEDURE [security_query_roles_by_user_sid]
GO

CREATE PROCEDURE [security_query_roles_by_user_sid]
	@SID [SIDType]
AS
BEGIN
	SET NOCOUNT ON

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
		WHERE MemberSID = @SID

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
	SELECT sr.[IDRole], si.[DisplayName] AS [Name], si.[Description], si.[Type], sr.[DualRequired], sr.[PtzReservedAsAnonymous], sr.[AllowMobileClientLogOn], sr.[AllowSmartClientLogOn], sr.[AllowWebClientLogOn], si.[LastModified]
	FROM GroupList
	INNER JOIN [SecurityIdentity] si ON GroupList.[IDRole] = si.[SID] AND si.[Type] = 4
	INNER JOIN [SecurityRoles] sr ON sr.[IDRole] = si.[SID]
	WHERE [MemberSID] = @SID
END
GO