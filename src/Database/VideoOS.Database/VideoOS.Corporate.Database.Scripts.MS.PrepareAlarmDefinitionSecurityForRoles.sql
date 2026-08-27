-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @AlarmPluginId UNIQUEIDENTIFIER = '46BB0392-FD06-465B-8776-B90AC16639B3' 
DECLARE @AlarmDefinitionKindId UNIQUEIDENTIFIER = '9807CA8A-46C2-45ED-9446-6423DAE2CD88'


-- Modify the mip Kind to add the security actions
UPDATE [Mip].[Kinds] SET SecurityAction = 'GENERIC_WRITE,GENERIC_READ,ADMIN_SECURITY' WHERE KindId = @AlarmDefinitionKindId


-- Add the Alarm definitions security Namespace
IF NOT EXISTS (SELECT * FROM dbo.SecurityNamespace
				WHERE namespace_id = @AlarmDefinitionKindId)
BEGIN
	DECLARE @AlarmDefinitionsOrderNo INT
	SET @AlarmDefinitionsOrderNo = 1 + (SELECT MAX(order_no) FROM [dbo].[SecurityNamespace])
	INSERT INTO [dbo].[SecurityNamespace] (namespace_id, name, display_name, separator, structure, order_no, user_defined) 
		VALUES (@AlarmDefinitionKindId, @AlarmDefinitionKindId, 'Alarm Definitions', '/', 1, @AlarmDefinitionsOrderNo, 1)
END


-- Add the Alarm definitions overall security object.
DECLARE @alarmDefinitionsOverallSecurityObjectId UNIQUEIDENTIFIER
SET	@alarmDefinitionsOverallSecurityObjectId 
	= (SELECT  security_object_id FROM dbo.SecurityObject 
		WHERE namespace_id = @AlarmDefinitionKindId 
		AND object_id = '/')

IF @alarmDefinitionsOverallSecurityObjectId IS NULL
BEGIN
	SET @alarmDefinitionsOverallSecurityObjectId = NEWID()
	--- object_type 1 and object_id '/' means overall
	INSERT INTO [dbo].[SecurityObject] (namespace_id, parent_path, object_id, object_type, object_name, security_object_id, inherits_from_id) 
		VALUES (@AlarmDefinitionKindId, '', '/', 1, @AlarmDefinitionKindId, @AlarmDefinitionsOverallSecurityObjectId , NULL)
END


-- Add the Alarm definitions parent node security object.
IF NOT EXISTS (SELECT * FROM [dbo].[SecurityObject] 
				where namespace_id = @AlarmDefinitionKindId
				AND parent_path = '/')
BEGIN
	--- object_type 2 means is not in overall. parent_path '/' means it's the overall direct children.
	INSERT INTO [dbo].[SecurityObject] (namespace_id, parent_path, object_id, object_type, object_name, security_object_id, inherits_from_id) 
		VALUES (@AlarmDefinitionKindId, '/', @AlarmDefinitionKindId, 2, '', NEWID(), convert(nvarchar(300), @AlarmDefinitionsOverallSecurityObjectId))
END

	
-- Add the Alarm definitions Security Actions
INSERT INTO  [dbo].[SecurityAction] (namespace_id, name, display_name, order_no, visible, security_bit, legacy_security_name)
SELECT *
FROM (VALUES
	(@AlarmDefinitionKindId, 'GENERIC_READ', 'View', 1, 1, 1, NULL),
	(@AlarmDefinitionKindId, 'GENERIC_WRITE', 'Write', 2, 1, 2, NULL),
	(@AlarmDefinitionKindId, 'ADMIN_SECURITY', 'Manage security', 3, 0, 4, NULL)
) AS to_be_inserted(namespace_id, name, display_name, order_no, visible, security_bit, legacy_security_name)
WHERE NOT EXISTS (
	SELECT *
	FROM dbo.[SecurityAction]
	WHERE securityAction.namespace_id = to_be_inserted.namespace_id 
	AND securityAction.name = to_be_inserted.name 
)

-- Ensure Alarm definitions security namespace appears after Alarms namespace
  DECLARE @AlarmOrderNo INT = (SELECT order_no FROM dbo.SecurityNamespace WHERE namespace_id = @AlarmPluginId)
  UPDATE dbo.SecurityNamespace SET order_no = order_no + 1 WHERE order_no > @AlarmOrderNo
  UPDATE dbo.SecurityNamespace SET order_no = @AlarmOrderNo + 1 WHERE namespace_id = @AlarmDefinitionKindId

GO