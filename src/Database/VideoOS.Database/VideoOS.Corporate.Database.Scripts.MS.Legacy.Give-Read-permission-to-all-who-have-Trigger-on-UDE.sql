-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

UPDATE [SecurityAccessEntry]
SET [deleted] = 0
WHERE [deleted] = 1
AND [action_name] = 'GENERIC_READ'
AND [security_object_id] IN
(
	SELECT sae.[security_object_id]
	FROM [SecurityAccessEntry] sae
	INNER JOIN [SecurityObject] so ON sae.[security_object_id] = so.[security_object_id]
	WHERE so.[namespace_id] = 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF'
	AND so.[object_id] <> '/'
	AND sae.[action_name] = 'TRIGGER'
	AND sae.[deleted] = 0
)

INSERT INTO [SecurityAccessEntry]([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
SELECT sae.[security_object_id], sae.[user_sid], 'GENERIC_READ', 0,0
FROM [SecurityAccessEntry] sae
INNER JOIN [SecurityObject] so ON sae.[security_object_id] = so.[security_object_id]
WHERE  so.namespace_id = 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF'
AND sae.[action_name] = 'TRIGGER'
AND sae.[deleted] = 0
AND NOT EXISTS
(
	SELECT sae2.[security_object_id]
	FROM [SecurityAccessEntry] sae2
	WHERE sae2.[action_name] = 'GENERIC_READ'
	AND sae2.[security_object_id] = sae.[security_object_id]
	AND sae2.[user_sid] =  sae.[user_sid]
)
GO