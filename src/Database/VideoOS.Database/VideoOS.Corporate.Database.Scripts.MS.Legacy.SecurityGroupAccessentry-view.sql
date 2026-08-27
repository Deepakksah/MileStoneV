-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF EXISTS(SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[SecurityGroupAccessEntry]'))
DROP VIEW [SecurityGroupAccessEntry]
GO

CREATE VIEW [dbo].[SecurityGroupAccessEntry]
AS
SELECT
	so.[object_id],
	sn.[namespace_id],
	sae.[action_name],
	sae.[user_sid],
	sae.[access_deny]
FROM [SecurityAccessEntry] sae
	INNER JOIN [SecurityAction] sa ON sa.[name] = sae.[action_name]
	INNER JOIN [SecurityNamespace] sn ON sn.[namespace_id] = sa.[namespace_id]
	INNER JOIN [SecurityIdentity] si ON si.[SID] = sae.[user_sid] AND si.[Deleted] = 0
	INNER JOIN [SecurityObject] so ON so.[namespace_id] = sn.[namespace_id] AND so.[security_object_id] = sae.[security_object_id] AND so.[object_id] = '/'
WHERE 
	sae.[deleted] = 0
GO


