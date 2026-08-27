-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_client_profile_id_by_user_sid]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_client_profile_id_by_user_sid]
GO

CREATE PROCEDURE [query_client_profile_id_by_user_sid]
	@userSID [SIDType],
	@profile_type VARCHAR(50)
AS
BEGIN
	-- Select the identity - this we be ResultSet 0
	DECLARE @currentLevel INT
	SET @currentLevel = 0
	
	SELECT TOP 1 [IDClientProfile]
	FROM ClientProfile
	WHERE (IDClientProfile IN (
		SELECT cprr.[IDClientProfile]
		FROM ufnRoleMembers(@userSID) rm
		INNER JOIN [ClientProfileRoleRelation] cprr ON cprr.[IDRole] = rm.[IDRole]
	)
	OR [DefaultProfile]= 1)
	AND [profile_type] = @profile_type
	ORDER BY [Priority] DESC
END
GO