-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.objects WHERE [object_id] = OBJECT_ID(N'[security_query_aces]') AND [type] IN (N'P', N'PC'))
DROP PROCEDURE [security_query_aces]
GO

CREATE PROCEDURE [security_query_aces]
	@namespace_id UNIQUEIDENTIFIER,
	@get_inherit_information BIT
AS
BEGIN
	SET NOCOUNT ON

	SELECT [security_token], [identity_sid], SUM([allow_permission]) AS [allow_permission], SUM([deny_permission]) AS [deny_permission]  
	FROM
	(
		SELECT 
			'XPCO' + pivot_table.[security_token] AS [security_token], 
			pivot_table.[identity_sid], 
			(CASE WHEN pivot_table.[1] IS NULL THEN 0 ELSE pivot_table.[1] END) AS [allow_permission], 
			(CASE WHEN pivot_table.[0] IS NULL THEN 0 ELSE pivot_table.[0] END) AS [deny_permission]
		FROM
		(
			SELECT so.[full_path] AS [security_token], sae.[user_sid] AS [identity_sid], 1 AS [allow], SUM(sa.security_bit) AS [permission]
			FROM [SecurityAccessEntry] sae
			INNER JOIN [SecurityObject] so ON so.[security_object_id] = sae.[security_object_id]
			INNER JOIN [SecurityAction] sa ON sa.[namespace_id] = so.[namespace_id] AND sa.[name] = sae.[action_name]
			WHERE so.[namespace_id] = @namespace_id AND sae.[access_deny] = 0
			GROUP BY so.[full_path], sae.[user_sid]

			UNION

			SELECT so.[full_path] AS [security_token], sae.[user_sid] AS [identity_sid], 0 AS [allow], SUM(sa.security_bit) AS [permission]
			FROM [SecurityAccessEntry] sae
			INNER JOIN [SecurityObject] so ON so.[security_object_id] = sae.[security_object_id]
			INNER JOIN [SecurityAction] sa ON sa.[namespace_id] = so.[namespace_id] AND sa.[name] = sae.[action_name]
			WHERE so.[namespace_id] = @namespace_id AND sae.[access_deny] = 1
			GROUP BY so.[full_path], sae.[user_sid]
		) AS permission
		PIVOT
		(
			SUM(permission) FOR [allow] IN ([0], [1])
		) AS pivot_table

		UNION

		SELECT 'XPCO/' + IDDevice as [security_token], 
			IDRole as [identity_sid], 
			SUM(sa.security_bit) AS [allow_permission], 
			0 AS [deny_permission]
		FROM SecurityPermissionsDevice spd
		INNER JOIN [SecurityAction] sa ON spd.SecurityAttributeID = sa.legacy_security_name
		WHERE sa.namespace_id = @namespace_id
		GROUP BY 'XPCO/' + IDDevice, IDRole, Value
	
		UNION
	
		SELECT 'XPCO/' + IDMatrix as [security_token], 
			IDRole as [identity_sid], 
			SUM(sa.security_bit) AS [allow_permission], 
			0 AS [deny_permission]
		FROM SecurityPermissionsMatrix spm
		INNER JOIN [SecurityAction] sa ON spm.SecurityAttributeID = sa.legacy_security_name
		WHERE sa.namespace_id = @namespace_id
		GROUP BY 'XPCO/' + IDMatrix, IDRole, Value
	) AS SecurityDerivedTable
	GROUP BY [security_token], [identity_sid]
	ORDER BY [security_token]
	
	IF(@get_inherit_information = 1)
	BEGIN
		SELECT 'XPCO' + so.[full_path] AS [security_token]
		FROM [SecurityObject] so
		WHERE so.[namespace_id] = @namespace_id AND inherits_from_id IS NULL 
	END
END
GO