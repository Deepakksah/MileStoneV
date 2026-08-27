-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentUtcTime DATETIME
SET @CurrentUtcTime = GETUTCDATE()

DECLARE @object_type INT = 2
DECLARE @AccessControlTopGroup [GuidType] = '6ACA51C5-A716-48A1-A2DC-AEEADA59C771'
DECLARE @AccessControlUnGrouped [GuidType] = '9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF'
DECLARE @AccessControlSecurityNamespace [GuidType] = '6d3bdb33-d913-470f-8784-3dd4ee94db8c'
DECLARE @AccessControlTopSecurityObjectId [GuidType] = 'E2FEE54C-7C00-4F1E-A482-76D2526FE555'
DECLARE @AccessControlUngroupedSecurityObjectId [GuidType] = 'B6B820C2-F1CE-4FCE-881A-1413F6CA1FD3'
DECLARE @AccessControlItemKind uniqueidentifier = '6D3BDB33-D913-470F-8784-3DD4EE94DB8C'
DECLARE @UngroupItemGroupPath NVARCHAR(400) = '/9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF/'
DECLARE @UngroupId NVARCHAR(300) = '9EA76B67-E0FF-4A8F-A904-F4DB1DB6A9CF'
DECLARE @UngroupGuid uniqueidentifier = @UngroupId

-- Create AccessControlGroup Root
IF NOT EXISTS(SELECT * FROM [ServerGroup] WHERE [IDServerGroup] = @AccessControlTopGroup AND [StructureType] = 'ACCESSCONTROL_GROUP')
BEGIN
	INSERT [ServerGroup] ([IDServerGroup], [Name], [Description], [IDParentGroup], [StructureType], [OrderNo], [Level], [Path], [CreatedDate], [LastModified])
	VALUES(
		@AccessControlTopGroup,
		'ACCESSCONTROL_GROUP',
		'',
		'',
		'ACCESSCONTROL_GROUP',
		1,
		0,
		'\ACCESSCONTROLGROUP',
		@CurrentUtcTime,
		@CurrentUtcTime
	)

	UPDATE [ServerGroup]
	SET [RelationIds] = dbo.GetServerGroupRelations(@AccessControlTopGroup)
	WHERE [IDServerGroup] = @AccessControlTopGroup
END

-- Create AccessControlGroup for non-grouped Access Control units
IF NOT EXISTS(SELECT * FROM [ServerGroup] WHERE [IDServerGroup] = @AccessControlUnGrouped AND [StructureType] = 'ACCESSCONTROL_GROUP')
BEGIN
	INSERT [ServerGroup] ([IDServerGroup], [Name], [Description], [IDParentGroup], [StructureType], [OrderNo], [Level], [Path], [CreatedDate], [LastModified])
	VALUES(
		@AccessControlUnGrouped,
		'Ungrouped items',
		'',
		@AccessControlTopGroup,
		'ACCESSCONTROL_GROUP',
		1,
		1,
		'\ACCESSCONTROLGROUP\' + @AccessControlUnGrouped,
		@CurrentUtcTime,
		@CurrentUtcTime
	)

	UPDATE [ServerGroup]
	SET [RelationIds] = dbo.GetServerGroupRelations(@AccessControlUnGrouped + ';' + @AccessControlTopGroup)
	WHERE [IDServerGroup] = @AccessControlUnGrouped
END

IF NOT EXISTS(SELECT * FROM [dbo].[SecurityNamespace] WHERE namespace_id = @AccessControlSecurityNamespace)
BEGIN
	DECLARE @order_number INT
	SET @order_number = (SELECT COALESCE(MAX([order_no]), 0) FROM [SecurityNamespace])	
	SET @order_number = @order_number + 1
	INSERT [dbo].[SecurityNamespace] ([namespace_id], [name], [display_name], [separator], [structure], [order_no], [user_defined]) 
		VALUES (@AccessControlSecurityNamespace, N'Access Control Units', N'Access Control Units', N'/', 1, @order_number, 1)

INSERT [dbo].[SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name]) 
	VALUES (@AccessControlSecurityNamespace, N'ADMIN_SECURITY', N'Manage security', 1, 0, CAST(1 AS Decimal(20, 0)), NULL)
INSERT [dbo].[SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name]) 
	VALUES (@AccessControlSecurityNamespace, N'GENERIC_READ', N'View', 2, 1, CAST(2 AS Decimal(20, 0)), NULL)
INSERT [dbo].[SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name]) 
	VALUES (@AccessControlSecurityNamespace, N'GENERIC_WRITE', N'Manage', 3, 1, CAST(4 AS Decimal(20, 0)), NULL)
INSERT [dbo].[SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name]) 
	VALUES (@AccessControlSecurityNamespace, N'USE', N'Client operations', 4, 1, CAST(8 AS Decimal(20, 0)), NULL)
INSERT [dbo].[SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible], [security_bit], [legacy_security_name]) 
	VALUES (@AccessControlSecurityNamespace, N'NOTIFICATION', N'Receive notifications', 5, 1, CAST(16 AS Decimal(20, 0)), NULL)

END

IF (NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'AccessControlMembers'))
BEGIN
	CREATE TABLE [dbo].[AccessControlMembers] (memberId uniqueidentifier REFERENCES MIPObject(IDMIPObject), groupId GuidType REFERENCES ServerGroup(IDServerGroup), LastModified datetime)
END

IF NOT EXISTS(SELECT * FROM [dbo].[SecurityObject] WHERE namespace_id = @AccessControlSecurityNamespace AND object_id = @AccessControlUnGrouped)
BEGIN
	INSERT [dbo].[SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id]) 
		VALUES (@AccessControlSecurityNamespace, N'', N'/', 1, N'Access Control Top', @AccessControlTopSecurityObjectId, NULL)
	INSERT [dbo].[SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id]) 
		VALUES (@AccessControlSecurityNamespace, N'/', @AccessControlUnGrouped, 2, N'Ungrouped AC-Objects', @AccessControlUngroupedSecurityObjectId, @AccessControlTopSecurityObjectId)

		
	DECLARE @UngroupSecurityObjectId uniqueidentifier

	SET @UngroupSecurityObjectId = (SELECT security_object_id FROM SecurityObject WHERE object_id = @UngroupId)

	INSERT [dbo].[SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id]) 
	SELECT @AccessControlSecurityNamespace, @UngroupItemGroupPath, [IDMIPObject], 2, [Name], NEWID(), @UngroupSecurityObjectId
		FROM [dbo].[MIPObject] WHERE Kind = @AccessControlItemKind

	INSERT INTO [dbo].[AccessControlMembers] (memberId, groupId, LastModified) 
		SELECT [IDMIPObject], @UngroupId, @CurrentUtcTime FROM [dbo].[MIPObject] WHERE Kind = @AccessControlItemKind

END
GO
