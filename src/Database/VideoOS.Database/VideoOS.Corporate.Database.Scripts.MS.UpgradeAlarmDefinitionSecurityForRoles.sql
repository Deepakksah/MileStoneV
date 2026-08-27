-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @AlarmPluginId UNIQUEIDENTIFIER = '46BB0392-FD06-465B-8776-B90AC16639B3' 
DECLARE @AlarmDefinitionKindId UNIQUEIDENTIFIER = '9807CA8A-46C2-45ED-9446-6423DAE2CD88'

-- Recover the Alarm Definitions security object id
DECLARE @AlarmDefinitionsSecurityObjectId UNIQUEIDENTIFIER
SET @AlarmDefinitionsSecurityObjectId = (SELECT TOP 1 security_object_id FROM [dbo].[SecurityObject]
	WHERE namespace_id = @AlarmDefinitionKindId 
	AND parent_path = '/')
	
IF @AlarmDefinitionsSecurityObjectId IS NULL
BEGIN
	RETURN;
END


---- Add the security objects for existing alarm definitions
IF OBJECT_ID(N'Central.Configuration', N'U') IS NOT NULL
BEGIN
	Declare @ConfigurationData XML
	SET @ConfigurationData = TRY_CAST( (SELECT [Data] FROM [Central].[Configuration]) AS XML)
		
	IF @ConfigurationData IS NOT NULL
	BEGIN
		DECLARE @TempTableWithAlarmIds TABLE (alarmId NVARCHAR(100))
		INSERT INTO @TempTableWithAlarmIds (alarmId) (SELECT 
			x.Rec.query('./guid').value('.', 'nvarchar(MAX)') AS UNIQUEIDENTIFIER
		FROM @ConfigurationData.nodes('//*:alarmlist/*:alarm') as x(Rec))

		MERGE INTO [dbo].[SecurityObject] AS Target
		USING (SELECT DISTINCT alarmId FROM @TempTableWithAlarmIds) AS Source 
		ON Target.object_id = Source.alarmId
		WHEN NOT MATCHED THEN
			-- object type 2 means is not overall security
			INSERT (namespace_id, parent_path, object_id, object_type, object_name, security_object_id, inherits_from_id) 
				VALUES (@AlarmDefinitionKindId, '/' + convert(nvarchar(100), @AlarmDefinitionKindId) + '/', alarmId, 2, '', NEWID(), convert(nvarchar(100),  @AlarmDefinitionsSecurityObjectId));
	END
END

-- Retrieve the Alarm Overall security object id
DECLARE @AlarmOverallSecurityObjectId UNIQUEIDENTIFIER
SET @AlarmOverallSecurityObjectId = (SELECT TOP 1 security_object_id FROM [dbo].[SecurityObject]
	WHERE namespace_id = @AlarmPluginId
	AND object_id = '/')

IF @AlarmOverallSecurityObjectId IS NULL
BEGIN
	RETURN;
END

-- Retrieve the Alarm security object id
DECLARE @AlarmSecurityObjectId UNIQUEIDENTIFIER
SET @AlarmSecurityObjectId = (SELECT TOP 1 security_object_id FROM [dbo].[SecurityObject]
	WHERE namespace_id = @AlarmPluginId
	AND parent_path = '/')

IF @AlarmSecurityObjectId IS NULL
BEGIN
	RETURN;
END


---- Apply Alarm definitions view permission to the roles if they already have Alarms view permission
	
-- Recover the Alarm Overall Definitions security object id
DECLARE @AlarmDefinitionsOverallSecurityObjectId UNIQUEIDENTIFIER
SET @AlarmDefinitionsOverallSecurityObjectId = (SELECT TOP 1 security_object_id FROM [dbo].[SecurityObject]
	WHERE namespace_id = @AlarmDefinitionKindId 
	AND object_id = '/')

IF @AlarmDefinitionsOverallSecurityObjectId IS NULL
BEGIN
	RETURN;
END

DECLARE @TempTableWithRoles TABLE (
	security_object_id UNIQUEIDENTIFIER,
	roleId NVARCHAR(260), 
	action_name NVARCHAR(260),
	access_deny bit
)

Declare @TempTableWithActionNames TABLE (
	action_name NVARCHAR(260)
)
INSERT INTO @TempTableWithActionNames (action_name) 
	VALUES ( 'GENERIC_READ'), ('GENERIC_WRITE'), ('ADMIN_SECURITY')

INSERT INTO @TempTableWithRoles (roleId, action_name, access_deny, security_object_id)
	SELECT user_sid, action_name, access_deny, SecurityAccessEntry.security_object_id
		FROM dbo.SecurityAccessEntry
		INNER JOIN dbo.SecurityObject
		ON SecurityAccessEntry.security_object_id = dbo.SecurityObject.security_object_id
		WHERE dbo.SecurityObject.namespace_id = @AlarmPluginId
		AND dbo.SecurityAccessEntry.action_name IN (SELECT action_name FROM @TempTableWithActionNames)

UPDATE @TempTableWithRoles SET security_object_id = @AlarmDefinitionsOverallSecurityObjectId WHERE security_object_id = @AlarmOverallSecurityObjectId
UPDATE @TempTableWithRoles SET security_object_id = @AlarmDefinitionsSecurityObjectId WHERE security_object_id = @AlarmSecurityObjectId


MERGE INTO dbo.SecurityAccessEntry AS Target
	USING (SELECT DISTINCT roleid, action_name, access_deny, security_object_id  FROM @TempTableWithRoles) AS Source 
	ON Target.user_sid = Source.roleid 
	AND Target.security_object_id = Source.security_object_id
	AND Target.action_name = Source.action_name
	WHEN NOT MATCHED THEN
		INSERT (security_object_id, user_sid, action_name, access_deny)
			VALUES (Source.security_object_id, roleid, Source.action_name, Source.access_deny);
GO