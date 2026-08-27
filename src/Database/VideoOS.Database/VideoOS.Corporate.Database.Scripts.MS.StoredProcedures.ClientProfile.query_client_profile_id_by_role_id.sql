-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[query_client_profile_id_by_role_id]') AND type in (N'P', N'PC'))
DROP PROCEDURE [query_client_profile_id_by_role_id]
GO

CREATE PROCEDURE [query_client_profile_id_by_role_id]
	@IDRole [GuidType]
	, @profile_type VARCHAR(50)
AS
BEGIN
	DECLARE @rowcount INT

	-- Initialize the ProcedureName for error messages.
	DECLARE @procedureName SYSNAME
	SELECT  @procedureName = @@SERVERNAME + '.' + db_name() + '..' + object_name(@@PROCID)
	
	SELECT TOP 1 [IDClientProfile]
	FROM ClientProfile
	WHERE (IDClientProfile IN 
	(	SELECT IDClientProfile
		FROM ClientProfileRoleRelation cprr
		WHERE cprr.IDRole = @IDRole
	)
	OR [DefaultProfile]= 1)
	AND [profile_type] = @profile_type
	ORDER BY Priority DESC
END
GO