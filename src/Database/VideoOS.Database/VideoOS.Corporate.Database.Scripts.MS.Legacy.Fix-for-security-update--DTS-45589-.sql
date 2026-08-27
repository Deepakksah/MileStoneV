-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DELETE [SecurityAccessEntry]
WHERE [access_entry_id] NOT IN
(
	SELECT MIN(access_entry_id)
	FROM [SecurityAccessEntry]
	GROUP BY [security_object_id], [user_sid], [action_name], [access_deny], [deleted]
	HAVING COUNT(*) = 1

	UNION 

	SELECT MIN(access_entry_id)
	FROM [SecurityAccessEntry]
	GROUP BY [security_object_id], [user_sid], [action_name], [access_deny], [deleted]
	HAVING COUNT(*) > 1
)
GO
