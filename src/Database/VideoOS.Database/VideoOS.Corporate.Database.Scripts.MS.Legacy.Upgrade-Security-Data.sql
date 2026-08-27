-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- namespace_id: 'E9914E39-A67A-496C-A3C1-5B4AAAB28F15'
-- security_object_id: '85082DD6-A02C-4D29-B99E-B39DA68A8E49'
-- object_id: '/'

-- ***************************************
-- Migrate legacy application permissions
-- ***************************************

INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
SELECT 
	'85082DD6-A02C-4D29-B99E-B39DA68A8E49' AS [security_object_id],
	[IDRole] AS [user_sid],
	CASE SecurityAttributeID
		WHEN 'APPLICATIONACCESSSTATUSAPI' THEN 'STATUS_API'
		WHEN 'APPLICATIONACCESSREPORTS' THEN 'EXEC_CONFIG_REPORT'
	END,
	0 AS [access_deny],
	0 AS [deleted]
FROM SecurityPermissions
WHERE [SecurityAttributeID] IN (
	'APPLICATIONACCESSSTATUSAPI',
	'APPLICATIONACCESSREPORTS'
)

-- ***************************************
-- Migrate recording storage permissions
-- ***************************************

-- RECORDING_STORAGE;GENERIC_READ  -> Give GENERIC_READ on recording server
INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
SELECT
	so1.[security_object_id], 
	sae.[SID], 
	sae.[IDAction],
	sae.[AccessDeny], 
	sae.[Deleted]
FROM [SecurityAccessEntries] sae
INNER JOIN [SecurityObjects] so on sae.[IDSecurityObject] = so.[IDSecurityObject]
INNER JOIN [RecordingStorage] rs ON so.[IDObject] = rs.[pkid]
INNER JOIN [SecurityObject] so1 on so1.[object_id] = rs.[recordingserver_id]
WHERE so.IDClass = 'RECORDING_STORAGE' AND sae.[IDAction] = 'GENERIC_READ'

-- RECORDING_STORAGE;GENERIC_WRITE or DELETE -> Give ADMIN_STORAGE on recording server
INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
SELECT
	so1.[security_object_id], 
	sae.[SID], 
	CASE sae.[IDAction]
		WHEN 'GENERIC_WRITE' THEN 'ADMIN_STORAGE'
		WHEN 'DELETE' THEN 'ADMIN_STORAGE'
	END,
	sae.[AccessDeny], 
	sae.[Deleted]
FROM [SecurityAccessEntries] sae
INNER JOIN [SecurityObjects] so on sae.[IDSecurityObject] = so.[IDSecurityObject]
INNER JOIN [RecordingStorage] rs ON so.[IDObject] = rs.[pkid]
INNER JOIN [SecurityObject] so1 on so1.[object_id] = rs.[recordingserver_id]
WHERE so.IDClass = 'RECORDING_STORAGE' AND (sae.[IDAction] = 'GENERIC_WRITE' OR sae.[IDAction] = 'DELETE')
GO

-- ***************************************
-- Migrate user-defined event permission
-- ***************************************

INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
SELECT 
	so1.[security_object_id], 
	sae.[SID], 
	CASE sae.[IDAction]
		WHEN 'TRIGGER_EVENT' THEN 'TRIGGER'
		ELSE sae.[IDAction]
	END,
	sae.[AccessDeny], 
	sae.[Deleted]
FROM [SecurityAccessEntries] sae
INNER JOIN [SecurityObjects] so ON sae.[IDSecurityObject] = so.[IDSecurityObject]
INNER JOIN [SecurityObject] so1 ON so1.[object_id] = so.[IDObject] AND so1.[namespace_id] = 'D4FF31CD-D9AC-46DF-8E92-5B545D191BAF'
WHERE so.[IDClass] = 'USER_DEFINED_EVENT'
GO

-- ***************************************
-- Migrate Smart Wall permission
-- ***************************************

INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
SELECT DISTINCT
	so1.[security_object_id], 
	sae.[SID], 
	CASE sae.[IDAction]
		WHEN 'CREATE_MONITOR' THEN 'GENERIC_WRITE'
		WHEN 'CREATE_PRESET' THEN 'GENERIC_WRITE'
		ELSE sae.[IDAction]
	END,
	sae.[AccessDeny], 
	sae.[Deleted]
FROM [SecurityAccessEntries] sae
INNER JOIN [SecurityObjects] so ON sae.[IDSecurityObject] = so.[IDSecurityObject]
INNER JOIN [SecurityObject] so1 ON so1.[object_id] = so.[IDObject] AND so1.[namespace_id] = '8732FCAC-A63C-46EA-9683-532B4C6644E7'
WHERE so.[IDClass] = 'VIDEO_WALL' AND sae.[IDAction] IN (
	'GENERIC_READ',
	'GENERIC_WRITE',
	'DELETE',
	'CREATE_MONITOR',
	'CREATE_PRESET'
)

INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
SELECT 
	so1.[security_object_id], 
	sae.[SID], 
	CASE sae.[IDAction]
		WHEN 'APPLYLAYOUT' THEN 'OPERATE'
		ELSE sae.[IDAction]
	END,
	sae.[AccessDeny], 
	sae.[Deleted]
FROM [SecurityAccessEntries] sae
INNER JOIN [SecurityObjects] so ON sae.[IDSecurityObject] = so.[IDSecurityObject]
INNER JOIN [VideoWallMonitor] vwm ON so.[IDObject] = vwm.[pkid]
INNER JOIN [SecurityObject] so1 ON so1.[object_id] = vwm.[videowall_id] AND so1.[namespace_id] = '8732FCAC-A63C-46EA-9683-532B4C6644E7'
WHERE so.[IDClass] = 'VIDEO_WALL_MONITOR' AND sae.[IDAction] IN (
	'GENERIC_READ',
	'GENERIC_WRITE',
	'DELETE',
	'APPLYLAYOUT'
)

INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
SELECT 
	so1.[security_object_id], 
	sae.[SID], 
	CASE sae.[IDAction]
		WHEN 'ACTIVATE' THEN 'OPERATE'
		ELSE sae.[IDAction]
	END,
	sae.[AccessDeny], 
	sae.[Deleted]
FROM [SecurityAccessEntries] sae
INNER JOIN [SecurityObjects] so ON sae.[IDSecurityObject] = so.[IDSecurityObject]
INNER JOIN [VideoWallPreset] vwp ON so.[IDObject] = vwp.[pkid]
INNER JOIN [SecurityObject] so1 ON so1.[object_id] = vwp.[videowall_id] AND so1.[namespace_id] = '8732FCAC-A63C-46EA-9683-532B4C6644E7'
WHERE so.[IDClass] = 'VIDEO_WALL_PRESET' AND sae.[IDAction] IN (
	'GENERIC_READ',
	'GENERIC_WRITE',
	'DELETE',
	'ACTIVATE'
)
GO

-- *******************************************
-- Migrate Public ViewGroup & View permission
-- *******************************************

DECLARE @security_object_id UNIQUEIDENTIFIER
DECLARE @object_id NVARCHAR(300)

DECLARE publicviewgroup_cursor CURSOR
FOR 
	SELECT [security_object_id], [object_id]
	FROM [SecurityObject]
	WHERE [inherits_from_id] = 'E930CDDB-841D-49B9-A0D8-14906EF65C48' AND [object_id] <> '4BF8A8B0-54AB-4E82-994F-98D8E9584176/'

	OPEN publicviewgroup_cursor

	FETCH NEXT FROM publicviewgroup_cursor
	INTO @security_object_id, @object_id

	WHILE(@@FETCH_STATUS = 0)
	BEGIN
		;WITH SecCTE(IDObject, IDInheritsFrom, IDSecurityObject, IDClass, RootObjectId, [Level]) AS
			(
				SELECT IDObject, IDInheritsFrom, IDSecurityObject, IDClass, IDObject AS RootObjectId, 1 AS [Level]
				FROM [SecurityObjects]
				WHERE IDObject = @object_id

				UNION ALL

				SELECT so.IDObject, so.IDInheritsFrom, so.IDSecurityObject, so.IDClass, cte.RootObjectId, cte.[Level] + 1
				FROM [SecurityObjects] so
				INNER JOIN SecCTE cte ON cte.IDObject = so.IDInheritsFrom
			)

		INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
		SELECT DISTINCT
			@security_object_id, -- security_object_id for root group
			sae.[SID] as [user_sid],
			CASE so.IDClass
				WHEN 'SERVER_GROUP' THEN
					CASE sae.IDAction 
						WHEN 'CREATE_CHILDREN' THEN 'OPERATE'
						ELSE sae.IDAction
					END
				WHEN 'VIEW' THEN
					CASE sae.IDAction
						WHEN 'GENERIC_READ' THEN 'GENERIC_READ'
						ELSE sae.IDAction
					END
			END,
			sae.[AccessDeny], 
			sae.[Deleted]

		FROM SecCTE cte
		INNER JOIN SecurityObjects so ON so.IDSecurityObject = cte.IDSecurityObject
		INNER JOIN SecurityAccessEntries sae ON sae.IDSecurityObject = cte.IDSecurityObject

		FETCH NEXT FROM publicviewgroup_cursor
		INTO @security_object_id, @object_id
	END

CLOSE publicviewgroup_cursor
DEALLOCATE publicviewgroup_cursor
GO


-- ********************************************
-- Migrate Private ViewGroup & View permission
-- ********************************************

DECLARE @security_object_id UNIQUEIDENTIFIER
DECLARE @object_id NVARCHAR(300)

DECLARE privateviewgroup_cursor CURSOR
FOR 
	SELECT [security_object_id], [object_id]
	FROM [SecurityObject]
	WHERE [parent_path] = '/4BF8A8B0-54AB-4E82-994F-98D8E9584176/'

	OPEN privateviewgroup_cursor

	FETCH NEXT FROM privateviewgroup_cursor
	INTO @security_object_id, @object_id

	WHILE(@@FETCH_STATUS = 0)
	BEGIN
		;WITH SecCTE(IDObject, IDInheritsFrom, IDSecurityObject, IDClass, RootObjectId, [Level]) AS
			(
				SELECT IDObject, IDInheritsFrom, IDSecurityObject, IDClass, IDObject AS RootObjectId, 1 AS [Level]
				FROM [SecurityObjects]
				WHERE IDObject = @object_id

				UNION ALL

				SELECT so.IDObject, so.IDInheritsFrom, so.IDSecurityObject, so.IDClass, cte.RootObjectId, cte.[Level] + 1
				FROM [SecurityObjects] so
				INNER JOIN SecCTE cte ON cte.IDObject = so.IDInheritsFrom
			)

		INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
		SELECT DISTINCT
			@security_object_id, -- security_object_id for root group
			sae.[SID] as [user_sid],
			CASE so.IDClass
				WHEN 'SERVER_GROUP' THEN
					CASE sae.IDAction 
						WHEN 'CREATE_CHILDREN' THEN 'OPERATE'
						ELSE sae.IDAction
					END
				WHEN 'VIEW' THEN
					CASE sae.IDAction
						WHEN 'GENERIC_READ' THEN 'GENERIC_READ'
						ELSE sae.IDAction
					END
			END,
			sae.[AccessDeny], 
			sae.[Deleted]

		FROM SecCTE cte
		INNER JOIN SecurityObjects so ON so.IDSecurityObject = cte.IDSecurityObject
		INNER JOIN SecurityAccessEntries sae ON sae.IDSecurityObject = cte.IDSecurityObject

		FETCH NEXT FROM privateviewgroup_cursor
		INTO @security_object_id, @object_id
	END

	CLOSE privateviewgroup_cursor
	DEALLOCATE privateviewgroup_cursor
GO

-- ********************************************
-- Migrate User-defined security classes (MIP)
-- ********************************************

-- Insert user-defined security namespaces
INSERT [SecurityNamespace] ([namespace_id], [name], [display_name], [separator], [structure], [order_no], [user_defined])
SELECT NEWID(), [ObjectClass].[IDClass], [ObjectClass].[IDClass], '/', 1, 99, 1
FROM 
(
	SELECT DISTINCT [IDClass]
	FROM [SecurityActions]
	WHERE [IDClass] NOT IN 
	(
		'APPLICATION',
		'RECORDING_STORAGE',
		'REPORT',
		'SERVER_GROUP',
		'SITES',
		'USER_DEFINED_EVENT',
		'VIDEO_WALL',
		'VIDEO_WALL_MONITOR',
		'VIDEO_WALL_PRESET',
		'VIEW',
		'VIEW_GROUP'
	)
) [ObjectClass]

-- Insert security actions
INSERT [SecurityAction] ([namespace_id], [name], [display_name], [order_no], [visible])
SELECT sn.[namespace_id], sa.[IDAction], sa.[IDAction], 99, 1
FROM [SecurityActions] sa
INNER JOIN [SecurityNamespace] sn ON sn.[name] = sa.[IDClass]
WHERE [IDClass] NOT IN 
(
	'APPLICATION',
	'RECORDING_STORAGE',
	'REPORT',
	'SERVER_GROUP',
	'SITES',
	'USER_DEFINED_EVENT',
	'VIDEO_WALL',
	'VIDEO_WALL_MONITOR',
	'VIDEO_WALL_PRESET',
	'VIEW',
	'VIEW_GROUP'
)

-- Insert Root folders for user-defined security namespaces
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT sn.[namespace_id], '', '/', 1, [ObjectClass].[IDClass], NEWID(), NULL
FROM 
(
	SELECT DISTINCT [IDClass]
	FROM [SecurityActions]
	WHERE [IDClass] NOT IN 
	(
		'APPLICATION',
		'RECORDING_STORAGE',
		'REPORT',
		'SERVER_GROUP',
		'SITES',
		'USER_DEFINED_EVENT',
		'VIDEO_WALL',
		'VIDEO_WALL_MONITOR',
		'VIDEO_WALL_PRESET',
		'VIEW',
		'VIEW_GROUP'
	)
) [ObjectClass]
INNER JOIN [SecurityNamespace] sn ON sn.name = [ObjectClass].[IDClass]

-- Insert security objects for user-defined security namespaces
INSERT [SecurityObject] ([namespace_id], [parent_path], [object_id], [object_type], [object_name], [security_object_id], [inherits_from_id])
SELECT sn.namespace_id, '/', so.IDObject, 2, '', so.IDSecurityObject, so.IDInheritsFrom
FROM [SecurityObjects] so
INNER JOIN [SecurityNamespace] sn ON so.[IDClass] = sn.[name]
WHERE so.[IDClass] NOT IN 
(
	'APPLICATION',
	'RECORDING_STORAGE',
	'REPORT',
	'SERVER_GROUP',
	'SITES',
	'USER_DEFINED_EVENT',
	'VIDEO_WALL',
	'VIDEO_WALL_MONITOR',
	'VIDEO_WALL_PRESET',
	'VIEW',
	'VIEW_GROUP'
)

-- Insert security access entries for user-defined security namespaces
INSERT [SecurityAccessEntry] ([security_object_id], [user_sid], [action_name], [access_deny], [deleted])
SELECT sae.IDSecurityObject, sae.SID, sae.IDAction, sae.AccessDeny, sae.Deleted
FROM SecurityAccessEntries sae
INNER JOIN SecurityObjects so ON so.IDSecurityObject = sae.IDSecurityObject
WHERE so.[IDClass] NOT IN 
(
	'APPLICATION',
	'RECORDING_STORAGE',
	'REPORT',
	'SERVER_GROUP',
	'SITES',
	'USER_DEFINED_EVENT',
	'VIDEO_WALL',
	'VIDEO_WALL_MONITOR',
	'VIDEO_WALL_PRESET',
	'VIEW',
	'VIEW_GROUP'
)


/*
  NOTE: At this time it should be acceptable to delete obsolete security tables...
	These are:

	  - SecurityObjects
		- SecurityAccessEntries
		- SecurityActions

*/

GO
