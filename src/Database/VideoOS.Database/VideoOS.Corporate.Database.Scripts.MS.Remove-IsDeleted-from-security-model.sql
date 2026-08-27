-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

IF  EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[SecurityAccessEntry]') AND name = N'deleted')
BEGIN
	DELETE FROM  [SecurityAccessEntryTimeProfile]
	WHERE [access_entry_id] IN
	(
		SELECT [access_entry_id]
		FROM [SecurityAccessEntry]
		WHERE [deleted] = 1
	)

	DELETE FROM [SecurityAccessEntry]
	WHERE [deleted] = 1
END


IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SecurityAccessEntry]') AND name = N'IX_SAE_Name_Deny_Deleted')
DROP INDEX [IX_SAE_Name_Deny_Deleted] ON [dbo].[SecurityAccessEntry] WITH ( ONLINE = OFF )
GO


IF EXISTS(SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[SecurityGroupAccessEntry]'))
DROP VIEW [SecurityGroupAccessEntry]
GO

IF  EXISTS (
	SELECT * 
	FROM sys.columns 
	WHERE object_id = OBJECT_ID(N'[SecurityAccessEntry]') AND name = N'deleted')
BEGIN
	ALTER TABLE [SecurityAccessEntry] DROP COLUMN [deleted]
END
GO


IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[SecurityAccessEntry]') AND name = N'IX_SAE_Name_Deny')
CREATE NONCLUSTERED INDEX [IX_SAE_Name_Deny] ON [SecurityAccessEntry] ([action_name], [access_deny])
INCLUDE ([security_object_id], [user_sid])
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
GO