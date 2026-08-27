-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

DECLARE @CurrentUtcTime DATETIME
SET @CurrentUtcTime = GETUTCDATE()

-- Step 1:
--   Delete all recording_servers from ServerGroupMembership
DELETE [ServerGroupMembership]
WHERE [ObjectType] = 'RECORDING_SERVER'

-- Step 2:
--   Create new MANAGEMENT_SERVER group if not already exists
--   If already exists, update with correct data

DECLARE @managementServerGroupId [GuidType]
SELECT @managementServerGroupId = [IDServerGroup]
FROM [ServerGroup]
WHERE [StructureType] = 'MANAGEMENT_SERVER'

IF(@managementServerGroupId IS NULL)
BEGIN
	INSERT [ServerGroup] ([IDServerGroup], [Name], [Description], [IDParentGroup], [StructureType], [OrderNo], [Level], [Path], [CreatedDate], [LastModified])
	SELECT 
		'909749AE-88C4-4780-A381-383D691E98FA',
		'MANAGEMENT_SERVER',
		'',
		[IDSite],
		'MANAGEMENT_SERVER',
		0,
		0,
		'\',
		@CurrentUtcTime,
		@CurrentUtcTime
	FROM [Sites] WHERE [IDSiteType] = 1
END
ELSE
BEGIN
  UPDATE [ServerGroup]
  SET [IDServerGroup] = '909749AE-88C4-4780-A381-383D691E98FA',
			[Name] = 'MANAGEMENT_SERVER',
			[IDParentGroup] = (SELECT [IDSite] FROM [Sites] WHERE [IDSiteType] = 1),
			[StructureType] = 'MANAGEMENT_SERVER',
			[OrderNo] = 0,
			[Level] = 0,
			[Path] = '\'
  WHERE [IDServerGroup] = @managementServerGroupId
END

-- Step 3:
--   Remove RECORDINGSERVER_GROUP
DELETE [ServerGroup]
WHERE [IDServerGroup] = 'E0BE6096-A1D6-464e-B028-B45FF2BA28AF'

DELETE [ServerGroup]
WHERE [StructureType] = 'RECORDING_SERVER'
	AND [IDParentGroup] = ''

-- Step 4:
--   Add all existing recording servers to ServerGroupMembership with MANAGEMENT_SERVER as parent
INSERT [ServerGroupMembership] ([IDServerGroup], [IDObject], [ObjectType], [LastModified])
SELECT 
	'909749AE-88C4-4780-A381-383D691E98FA',
	r.[IDRecorder],
	'RECORDING_SERVER',
	GETUTCDATE()
FROM [Recorders] r
LEFT JOIN [ServerGroupMembership] sgm ON sgm.[IDObject] = r.[IDRecorder]
WHERE sgm.[IDObject] IS NULL

-- Step 5:
--   Update Failover and SmartClient to childs of MANAGEMENT_SERVER
--UPDATE [ServerGroup]
--SET [IDParentGroup] = '909749AE-88C4-4780-A381-383D691E98FA'
--WHERE [IDServerGroup] IN
--(
--	'6A85FFC3-C134-4338-A072-024817CB5820',
--	'430C2FE2-B233-42DA-B508-1A22EF03383D'
--)

-- Step 6:
--   Delete devices group - obsolete
DELETE [ServerGroup]
WHERE [IDServerGroup] = '9178D2CE-888E-4058-9992-C99D506BE63A'
GO
