-- This header is used by the dbupdate tool.
-- The TARGET property is used to indicate which database the script is
-- intended to be executed on. The value can be either 'Surveillance' or
-- 'SurveillanceLog' without the quotes.
-- =========================================================================
-- TARGET: Surveillance
-- =========================================================================

-- Create a stored procedure used for setting security on newly created recording storages during the upgrade.
-- This stored procedure is deleted again at the end of this script.
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[set_recordingstorage_security_upgrade]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[set_recordingstorage_security_upgrade]
GO
CREATE PROCEDURE [set_recordingstorage_security_upgrade]
	@RecordingStorageId [GuidType],
	@CurrentUtcTime DATETIME
AS
BEGIN
	DECLARE @status INT
	
	-- Ensure that the RECORDING_STORAGE class exists
	DECLARE @class_id VARCHAR(128)
	SET @class_id = 'RECORDING_STORAGE'

	DECLARE @IDAction	VARCHAR(128)
	SET @IDAction = 'GENERIC_READ'
	
	IF NOT EXISTS(SELECT * FROM [SecurityActions] WHERE [IDClass] = @class_id AND [IDAction] = @IDAction)
	INSERT [SecurityActions] ([IDClass], [IDAction], [OrderNo], [ShowAction])
	VALUES(@class_id, @IDAction, 0, 1)

	SET @IDAction = 'GENERIC_WRITE'

	IF NOT EXISTS(SELECT * FROM [SecurityActions] WHERE [IDClass] = @class_id AND [IDAction] = @IDAction)
	INSERT [SecurityActions] ([IDClass], [IDAction], [OrderNo], [ShowAction])
	VALUES(@class_id, @IDAction, 1, 1)

	SET @IDAction = 'DELETE'

	IF NOT EXISTS(SELECT * FROM [SecurityActions] WHERE [IDClass] = @class_id AND [IDAction] = @IDAction)
	INSERT [SecurityActions] ([IDClass], [IDAction], [OrderNo], [ShowAction])
	VALUES(@class_id, @IDAction, 2, 1)

	DECLARE @adminRoleId AS [SIDType]
	SET @adminRoleId = '1f30d14b-ba91-4e8a-8838-eda17931f8fc'

	-- Register the object in the security system
	EXEC @status = [security_register_object_transaction]
		@SID = @adminRoleId,
		@IDObject = @RecordingStorageId,
		@IDClass = @class_id,
		@IDParentObject	= NULL,
		@CurrentUtcTime	= @CurrentUtcTime

	IF(@status <> 0)
	BEGIN
		RETURN @status
	END

	-- Then we give DELETE to the administrator
	INSERT [TempSecurityAccessEntries] ([SPID], [SID], [IDAction], [AccessDeny])
	VALUES (@@SPID, @adminRoleId, 'DELETE', 0)

	SET @status = @@ERROR
	IF(@status <> 0)
	BEGIN
		RETURN @status
	END
	
	EXEC [security_create_access_entry_transaction]
		@IDObject = @RecordingStorageId,
		@Replace = 0,
		@CurrentUtcTime = @CurrentUtcTime

	RETURN 0
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateUniqueRecordingStorageName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CreateUniqueRecordingStorageName]
GO

CREATE FUNCTION [dbo].[CreateUniqueRecordingStorageName]( @IDRecorder [GuidType], @BaseName [NameType] )
RETURNS [NameType]
AS
BEGIN
	DECLARE @StorageName AS [NameType]
	DECLARE @StorageCount AS INT
	DECLARE @StorageNameIndex AS INT

	SET @StorageName = @BaseName
	SET @StorageNameIndex = 0
	SELECT @StorageCount = COUNT(*) FROM [RecordingStorage] WHERE [recordingserver_id] = @IDrecorder AND [name] = @StorageName
	WHILE(@StorageCount > 0)
	BEGIN
		SET @StorageNameIndex = @StorageNameIndex + 1
		SET @StorageName = @BaseName + ' (' + CAST(@StorageNameIndex AS NVARCHAR(5)) + ')'
		SELECT @StorageCount = COUNT(*) FROM [RecordingStorage] WHERE [recordingserver_id] = @IDrecorder AND [name] = @StorageName
	END
	
	RETURN @StorageName
END
GO

IF EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CreateUniqueArchiveStorageName]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[CreateUniqueArchiveStorageName]
GO

CREATE FUNCTION [dbo].[CreateUniqueArchiveStorageName]( @IDRecorder [GuidType], @BaseName [NameType] )
RETURNS [NameType]
AS
BEGIN
	DECLARE @StorageName AS [NameType]
	DECLARE @StorageCount AS INT
	DECLARE @StorageNameIndex AS INT

	SET @StorageName = @BaseName
	SET @StorageNameIndex = 0
	SELECT @StorageCount = COUNT(*) FROM [ArchiveStorage] a INNER JOIN [RecordingStorage] r ON a.[recordingstorage_id] = r.[pkid] WHERE r.[recordingserver_id] = @IDrecorder AND a.[name] = @StorageName
	WHILE @StorageCount > 0
	BEGIN
		SET @StorageNameIndex = @StorageNameIndex + 1
		SET @StorageName = @BaseName + ' (' + CAST(@StorageNameIndex AS NVARCHAR(5)) + ')'
		SELECT @StorageCount = COUNT(*) FROM [ArchiveStorage] a INNER JOIN [RecordingStorage] r ON a.[recordingstorage_id] = r.[pkid] WHERE r.[recordingserver_id] = @IDrecorder AND a.[name] = @StorageName
	END

	RETURN @StorageName
END
GO


-- Create a view used only during the upgrade (deleted at the end)
IF EXISTS(SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[DeviceDatabaseUpgradeView]'))
DROP VIEW [dbo].[DeviceDatabaseUpgradeView]
GO

CREATE VIEW DeviceDatabaseUpgradeView
AS
	SELECT
		d.[IDDevice],
		d.[DbEnabled] AS [Device_recording_enabled],
		d.[DBSettings].value('(/properties/settings/setting[name=''Fps'']/value)[1]', 'FLOAT') AS [Device_recording_framerate],
		CASE WHEN d.[DBSettings].value('(/properties/settings/setting[name=''BufferingEnabled'']/value)[1]', 'NVARCHAR(10)') = 'True' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS [Device_prebuffer_enabled],
		d.[DBSettings].value('(/properties/settings/setting[name=''BufferedMillisecondsToKeep'']/value)[1]', 'INT') / 1000 AS [Device_prebuffer_seconds],
		md.[IDRecorder],
		md.[IDMediaDatabase],
		md.[Name] AS [MediaDatabaseName],
		md.[Description]  AS [MediaDatabaseDescription],
		md.[Path] AS [MediaDatabasePath],
		md.[IsDefault] AS [MediaDatabaseIsDefault],
		d.[DBSettings].value('(/properties/settings/setting[name=''MaxMinutes'']/value)[1]', 'INT') AS [MediaDatabaseMaxMinutes],
		CASE 
			COALESCE (CAST(ad.[Definition] AS XML).value('(/archive/skip_hours)[1]', 'INT') * 60, 
						d.[DBSettings].value('(/properties/settings/setting[name=''MaxMinutes'']/value)[1]', 'INT') )
		WHEN 0 THEN
			d.[DBSettings].value('(/properties/settings/setting[name=''MaxMinutes'']/value)[1]', 'INT')
		ELSE
			COALESCE (CAST(ad.[Definition] AS XML).value('(/archive/skip_hours)[1]', 'INT') * 60, 
						d.[DBSettings].value('(/properties/settings/setting[name=''MaxMinutes'']/value)[1]', 'INT') )
		END AS [RS_retain_minutes],
		d.[DBSettings].value('(/properties/settings/setting[name=''MaxSizeMb'']/value)[1]', 'INT') AS [RS_max_size_mb],
		ad.[IDArchiveDatabase],
		ad.[Name] AS [ArchiveDatabaseName],
		ad.[Description]  AS [ArchiveDatabaseDescription],
		CAST(ad.[Definition] AS XML).value('(/archive/path)[1]', 'nvarchar(512)') AS [ArchiveDatabasePath],
		CAST(CAST(ad.[Definition] AS XML).query('archive/schedule') AS NVARCHAR(MAX)) AS [ArchiveDatabaseSchedule],
		CAST(ad.[Definition] AS XML).value('(/archive/cleanup/max_hours)[1]', 'INT') * 60 AS [AS_retain_minutes],
		CAST(ad.[Definition] AS XML).value('(/archive/cleanup/max_mb)[1]', 'INT') AS [AS_max_size_mb]
	FROM
		[Devices] d INNER JOIN [MediaDatabases] md ON d.[IDMediaDatabase] = md.[IDMediaDatabase]
				  LEFT JOIN [ArchiveDatabases] ad ON d.[IDArchiveDatabase] = ad.[IDArchiveDatabase]
GO

DECLARE @status INT

DECLARE @CurrentUtcTime DATETIME
SET @CurrentUtcTime = GETUTCDATE()

DECLARE @IDRecorder AS [GuidType]
DECLARE @RecorderName AS [NameType]

DECLARE @IDMediaDatabase AS [GuidType]
DECLARE @MediaDatabaseName AS [NameType]
DECLARE @MediaDatabaseDescription AS NVARCHAR(MAX)
DECLARE @MediaDatabasePath AS NVARCHAR(512)
DECLARE @MediaDatabaseIsDefault AS BIT
DECLARE @MediaDatabaseMaxMinutes AS INT

DECLARE @RecordingStorageRetainMinutes AS INT
DECLARE @RecordingStorageMaxSizeMB AS BIGINT

DECLARE @IDArchiveDatabase AS [GuidType]
DECLARE @ArchiveDatabaseName AS [NameType]
DECLARE @ArchiveDatabaseDescription AS NVARCHAR(MAX)
DECLARE @ArchiveDatabasePath AS NVARCHAR(512)
DECLARE @ArchiveDatabaseSchedule AS NVARCHAR(MAX)

DECLARE @ArchiveStorageRetainMinutes AS INT
DECLARE @ArchiveStorageMaxSizeMB AS BIGINT

DECLARE @RecordingStorageId AS [GuidType]
DECLARE @StorageName AS [NameType]
DECLARE @StorageCount AS INT
DECLARE @StorageNameIndex AS INT
DECLARE @StorageIsDefault AS BIT
DECLARE @ArchiveStorageId AS [GuidType]
DECLARE @DeviceCount AS INT

DECLARE @DefaultRecordingStorageRetainMinutes AS INT
DECLARE @DefaultRecordingStorageMaxSizeMB AS BIGINT

SET @DefaultRecordingStorageRetainMinutes = 365 * 24 * 60 -- 365 Days
SET @DefaultRecordingStorageMaxSizeMB = 1024000 -- 1000 GB

DECLARE @MaxStorageRetainMinutes AS INT
DECLARE @MaxStorageMaxSizeMB AS BIGINT

SET @MaxStorageRetainMinutes = 100 * 365 * 24 * 60 -- 100 Years
SET @MaxStorageMaxSizeMB = 1099511627776 -- 1 exabyte = 1073741824 GB


-- Create legacy archive storages and corresponding recording storages for the existing archive databases
DECLARE archives_cursor CURSOR FOR 
	SELECT 
		ad.[IDRecorder],
		ad.[IDArchiveDatabase],
		ad.[Name] AS [ArchiveDatabaseName],
		ad.[Description] AS [Description],
		CAST(ad.[Definition] AS XML).value('(/archive/path)[1]', 'nvarchar(512)') AS [Path],
		CAST(ad.[Definition] AS XML).value('(/archive/cleanup/max_hours)[1]', 'INT') * 60 AS [AS_retain_minutes],
		CAST(ad.[Definition] AS XML).value('(/archive/cleanup/max_mb)[1]', 'INT') AS [AS_max_size_mb],
		CAST(CAST(ad.[Definition] AS XML).query('archive/schedule') AS NVARCHAR(MAX)) AS [ArchiveDatabaseSchedule] -- casting to make DISTINCT possible later
	FROM [ArchiveDatabases] ad

OPEN archives_cursor

FETCH NEXT FROM archives_cursor
INTO @IDRecorder, @IDArchiveDatabase, @ArchiveDatabaseName, @ArchiveDatabaseDescription, @ArchiveDatabasePath, 
     @ArchiveStorageRetainMinutes, @ArchiveStorageMaxSizeMB, @ArchiveDatabaseSchedule

WHILE(@@FETCH_STATUS = 0)
BEGIN
	-- Create dummy storage for the archive storage
	SET @RecordingStorageId = NEWID()
	SET @StorageName = [dbo].[CreateUniqueRecordingStorageName](@IDRecorder, @ArchiveDatabaseName) -- Yes - we do use the archive database name as the new name for the dummy recording storage
	SET @StorageIsDefault = 0 -- Dummy storage should not be default - it is just created to have a parent for the existing archive
	SET @RecordingStorageRetainMinutes = 60
	SET @RecordingStorageMaxSizeMB = @MaxStorageMaxSizeMB
	
	-- The path for the dummy storage will be the same as for the default recording storage for the recording server
	SELECT TOP 1 @MediaDatabasePath = [Path] FROM [MediaDatabases] WHERE [IDRecorder] = @IDRecorder AND [IsDefault] = 1
	IF @MediaDatabasePath IS NULL
	BEGIN
		SELECT TOP 1 @MediaDatabasePath = [Path] FROM [MediaDatabases] WHERE [IDRecorder] = @IDRecorder
		IF @MediaDatabasePath IS NULL
		BEGIN
			SET	@MediaDatabasePath = 'C:\MediaDB\' -- Very unlikely - if not impossible (no storage configured for the recording server already)
		END
	END

	-- Create the dummy recording storage
	INSERT INTO [RecordingStorage] (
		[pkid],
		[recordingserver_id],
		[name],
		[description],
		[path],
		[is_default],
		[retain_minutes],
		[max_size_mb],
		[created_date],
		[last_modified],
		[encryption_method],
		[encryption_password],
		[encryption_password_new]
	)
	VALUES (
		@RecordingStorageId,
		@IDRecorder,
		@StorageName,
		'',
		@MediaDatabasePath,
		@StorageIsDefault,
		@RecordingStorageRetainMinutes,
		@RecordingStorageMaxSizeMB,
		@CurrentUtcTime,
		@CurrentUtcTime,
		'None',
		'',
		''
	)

	EXEC @status = [set_recordingstorage_security_upgrade] @RecordingStorageId, @CurrentUtcTime
	IF @status <> 0
	BEGIN
		-- Upgrade will fail!
		CLOSE archives_cursor
		DEALLOCATE archives_cursor
		RAISERROR(N'Failed to set security on recording storage created during upgrade. Error code: %i', 18, 0, @status)
		GOTO cleanup_label
	END
	
	IF @ArchiveStorageRetainMinutes = 0
	BEGIN
		SET @ArchiveStorageRetainMinutes = @MaxStorageRetainMinutes
	END
	
	IF @ArchiveStorageRetainMinutes <= @RecordingStorageRetainMinutes
	BEGIN
		SET @ArchiveStorageRetainMinutes = @RecordingStorageRetainMinutes + 60
	END

	IF @ArchiveStorageMaxSizeMB = 0
	BEGIN
		SET @ArchiveStorageMaxSizeMB = @MaxStorageMaxSizeMB
	END

	-- Create the archive storage
	SET @StorageName = [dbo].[CreateUniqueArchiveStorageName](@IDRecorder, @ArchiveDatabaseName)
	INSERT INTO [ArchiveStorage] (
		[pkid],
		[recordingstorage_id],
		[name],
		[description],
		[path],
		[retain_minutes],
		[max_size_mb],
		[target_framerate],
		[schedule],
		[created_date],
		[last_modified],
		[folder],
		[framerate_reduction_enabled]
	)
	VALUES (
		@IDArchiveDatabase,
		@RecordingStorageId,
		@StorageName,
		@ArchiveDatabaseDescription,
		@ArchiveDatabasePath,
		@ArchiveStorageRetainMinutes,
		@ArchiveStorageMaxSizeMB,
		5,
		CAST(@ArchiveDatabaseSchedule AS XML),
		@CurrentUtcTime,
		@CurrentUtcTime,
		@IDRecorder,
		0
	)

	FETCH NEXT FROM archives_cursor
	INTO @IDRecorder, @IDArchiveDatabase, @ArchiveDatabaseName, @ArchiveDatabaseDescription, @ArchiveDatabasePath, 
		 @ArchiveStorageRetainMinutes, @ArchiveStorageMaxSizeMB, @ArchiveDatabaseSchedule
END
CLOSE archives_cursor
DEALLOCATE archives_cursor



-- Create recording storages for media databases without any associated devices
DECLARE mediadatabases_cursor CURSOR FOR
	SELECT [IDMediaDatabase], [IDRecorder], [Name], [Description], [Path], [IsDefault]
	FROM [MediaDatabases] m 
	WHERE NOT EXISTS (SELECT * FROM [Devices] d WHERE d.[IDMediaDatabase] = m.IDMediaDatabase)

OPEN mediadatabases_cursor

FETCH NEXT FROM mediadatabases_cursor
INTO @IDMediaDatabase, @IDRecorder, @MediaDatabaseName, @MediaDatabaseDescription, @MediaDatabasePath, @MediaDatabaseIsDefault

WHILE(@@FETCH_STATUS = 0)
BEGIN
	SET @RecordingStorageId = NEWID()
	SET @StorageName = [dbo].[CreateUniqueRecordingStorageName](@IDRecorder, @MediaDatabaseName)
	SET @RecordingStorageRetainMinutes = @DefaultRecordingStorageRetainMinutes
	SET @RecordingStorageMaxSizeMB = @DefaultRecordingStorageMaxSizeMB

	-- Create the recording storage
	INSERT INTO [RecordingStorage] (
		[pkid],
		[recordingserver_id],
		[name],
		[description],
		[path],
		[is_default],
		[retain_minutes],
		[max_size_mb],
		[created_date],
		[last_modified],
		[encryption_method],
		[encryption_password],
		[encryption_password_new]
	)
	VALUES (
		@RecordingStorageId,
		@IDRecorder,
		@StorageName,
		@MediaDatabaseDescription,
		@MediaDatabasePath,
		@MediaDatabaseIsDefault,
		@RecordingStorageRetainMinutes,
		@RecordingStorageMaxSizeMB,
		@CurrentUtcTime,
		@CurrentUtcTime,
		'None',
		'',
		''
	)

	EXEC @status = [set_recordingstorage_security_upgrade] @RecordingStorageId, @CurrentUtcTime
	IF @status <> 0
	BEGIN
		-- Upgrade will fail!
		CLOSE mediadatabases_cursor
		DEALLOCATE mediadatabases_cursor
		RAISERROR(N'Failed to set security on recording storage created during upgrade. Error code: %i', 18, 1, @status)
		GOTO cleanup_label
	END
	
	-- We do NOT register a job for moving data : from 'path\{recorder id}' to 'path\storage id}' as we
	-- do not expect there to be any valuable data in 'path\{recorder id}'. There can only be data if
	-- devices have previously been deleted - in which case we don't expect data to be valuable.
	
	FETCH NEXT FROM mediadatabases_cursor
	INTO @IDMediaDatabase, @IDRecorder, @MediaDatabaseName, @MediaDatabaseDescription, @MediaDatabasePath, @MediaDatabaseIsDefault
END
CLOSE mediadatabases_cursor
DEALLOCATE mediadatabases_cursor



-- Create recording storages - archive storages based on device permutations
DECLARE permutations_cursor CURSOR FOR 
	SELECT DISTINCT
		[IDRecorder],
		[IDMediaDatabase],
		[MediaDatabaseName],
		[MediaDatabaseDescription],
		[MediaDatabasePath],
		[MediaDatabaseIsDefault],
		[MediaDatabaseMaxMinutes],
		[RS_retain_minutes],
		[RS_max_size_mb],
		[IDArchiveDatabase],
		[ArchiveDatabaseName],
		[ArchiveDatabaseDescription],
		[ArchiveDatabasePath],
		[ArchiveDatabaseSchedule],
		[AS_retain_minutes],
		[AS_max_size_mb],
		COUNT(*) AS DeviceCount
	FROM [DeviceDatabaseUpgradeView]
	WHERE [IDMediaDatabase] IS NOT NULL
	GROUP BY 
		[IDRecorder],
		[IDMediaDatabase],
		[MediaDatabaseName],
		[MediaDatabaseDescription],
		[MediaDatabasePath],
		[MediaDatabaseIsDefault],
		[MediaDatabaseMaxMinutes],
		[RS_retain_minutes],
		[RS_max_size_mb],
		[IDArchiveDatabase],
		[ArchiveDatabaseName],
		[ArchiveDatabaseDescription],
		[ArchiveDatabasePath],
		[ArchiveDatabaseSchedule],
		[AS_retain_minutes],
		[AS_max_size_mb]

OPEN permutations_cursor

FETCH NEXT FROM permutations_cursor
INTO @IDRecorder, @IDMediaDatabase, @MediaDatabaseName, @MediaDatabaseDescription, @MediaDatabasePath, @MediaDatabaseIsDefault, @MediaDatabaseMaxMinutes,
     @RecordingStorageRetainMinutes, @RecordingStorageMaxSizeMB, 
     @IDArchiveDatabase, @ArchiveDatabaseName, @ArchiveDatabaseDescription, @ArchiveDatabasePath, @ArchiveDatabaseSchedule, 
     @ArchiveStorageRetainMinutes, @ArchiveStorageMaxSizeMB, @DeviceCount

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @RecordingStorageId = NEWID()
	SET @StorageName = [dbo].[CreateUniqueRecordingStorageName](@IDRecorder, @MediaDatabaseName)

	SET @StorageIsDefault = 0
	IF @MediaDatabaseIsDefault = 1
	BEGIN
		IF NOT EXISTS (SELECT * FROM [RecordingStorage] WHERE [recordingserver_id] = @IDRecorder AND [is_default] = 1)
		BEGIN
			SET @StorageIsDefault = 1
		END
	END
	
	-- Temp values used when finding devices for the storage/archive config later
	DECLARE @RecordingStorageRetainMinutes_Original AS INT
	DECLARE @RecordingStorageMaxSizeMB_Original AS BIGINT
	DECLARE @ArchiveStorageRetainMinutes_Original AS INT
	DECLARE @ArchiveStorageMaxSizeMB_Original AS BIGINT

	SET @RecordingStorageRetainMinutes_Original = @RecordingStorageRetainMinutes
	SET @RecordingStorageMaxSizeMB_Original = @RecordingStorageMaxSizeMB
	SET @ArchiveStorageRetainMinutes_Original = @ArchiveStorageRetainMinutes
	SET @ArchiveStorageMaxSizeMB_Original = @ArchiveStorageMaxSizeMB

	-- Calculate retention time for storage and archive
	SET @ArchiveStorageRetainMinutes = @MediaDatabaseMaxMinutes + @ArchiveStorageRetainMinutes
	
	IF @RecordingStorageRetainMinutes <= 0 OR @RecordingStorageRetainMinutes >= @MaxStorageRetainMinutes - 60
	BEGIN
		SET @RecordingStorageRetainMinutes = @MaxStorageRetainMinutes - 60
	END
	
	IF @ArchiveStorageRetainMinutes_Original = 0 OR @ArchiveStorageRetainMinutes <= 0 OR @ArchiveStorageRetainMinutes >= @MaxStorageRetainMinutes
	BEGIN
		SET @ArchiveStorageRetainMinutes = @MaxStorageRetainMinutes
	END
	
	IF @ArchiveStorageRetainMinutes <= @RecordingStorageRetainMinutes
	BEGIN
		SET @ArchiveStorageRetainMinutes = @RecordingStorageRetainMinutes + 60
	END

	-- Calculate max sizes
	SET @RecordingStorageMaxSizeMB = @RecordingStorageMaxSizeMB * @DeviceCount

	IF @RecordingStorageMaxSizeMB = 0
	BEGIN
		SET @RecordingStorageMaxSizeMB = @MaxStorageMaxSizeMB
	END

	IF @RecordingStorageMaxSizeMB > @MaxStorageMaxSizeMB
	BEGIN
		SET @RecordingStorageMaxSizeMB = @MaxStorageMaxSizeMB
	END

	IF @ArchiveStorageMaxSizeMB = 0
	BEGIN
		SET @ArchiveStorageMaxSizeMB = @MaxStorageMaxSizeMB
	END

	-- Create the recording storage
	INSERT INTO [RecordingStorage] (
		[pkid],
		[recordingserver_id],
		[name],
		[description],
		[path],
		[is_default],
		[retain_minutes],
		[max_size_mb],
		[created_date],
		[last_modified],
		[encryption_method],
		[encryption_password],
		[encryption_password_new]
	)
	VALUES (
		@RecordingStorageId,
		@IDRecorder,
		@StorageName,
		@MediaDatabaseDescription,
		@MediaDatabasePath,
		@StorageIsDefault,
		@RecordingStorageRetainMinutes,
		@RecordingStorageMaxSizeMB,
		@CurrentUtcTime,
		@CurrentUtcTime,
		'None',
		'',
		''
	)
		
	EXEC @status = [set_recordingstorage_security_upgrade] @RecordingStorageId, @CurrentUtcTime
	IF @status <> 0
	BEGIN
		-- Upgrade will fail!
		CLOSE permutations_cursor
		DEALLOCATE permutations_cursor
		RAISERROR(N'Failed to set security on recording storage created during upgrade. Error code: %i', 18, 2, @status)
		GOTO cleanup_label
	END

	-- Create the archive storage if configured
	IF @IDArchiveDatabase IS NOT NULL
	BEGIN
		SET @ArchiveStorageId = NEWID()
		SET @StorageName = [dbo].[CreateUniqueArchiveStorageName](@IDRecorder, @ArchiveDatabaseName)

		INSERT INTO [ArchiveStorage] (
			[pkid],
			[recordingstorage_id],
			[name],
			[description],
			[path],
			[retain_minutes],
			[max_size_mb],
			[target_framerate],
			[schedule],
			[created_date],
			[last_modified],
			[folder],
			[framerate_reduction_enabled]
		)
		VALUES (
			@ArchiveStorageId,
			@RecordingStorageId,
			@StorageName,
			@ArchiveDatabaseDescription,
			@ArchiveDatabasePath,
			@ArchiveStorageRetainMinutes,
			@ArchiveStorageMaxSizeMB,
			5,
			CAST(@ArchiveDatabaseSchedule AS XML),
			@CurrentUtcTime,
			@CurrentUtcTime,
			@ArchiveStorageId,
			0
		)
	END

	-- Now create the RecordingStorageDevice entries
	INSERT INTO [RecordingStorageDevice] (
		[recordingstorage_id],
		[device_id],
		[recording_enabled],
		[recording_framerate],
		[prebuffer_enabled],
		[prebuffer_seconds],
		[edgestorage_enabled]
	)
	SELECT
		@RecordingStorageId,
		[IDDevice],
		[Device_recording_enabled],
		[Device_recording_framerate],
		[Device_prebuffer_enabled],
		[Device_prebuffer_seconds],
		0
	FROM [DeviceDatabaseUpgradeView]
	WHERE [IDMediaDatabase] = @IDMediaDatabase
			AND [MediaDatabaseMaxMinutes] = @MediaDatabaseMaxMinutes
			AND [RS_retain_minutes] = @RecordingStorageRetainMinutes_Original
			AND [RS_max_size_mb] = @RecordingStorageMaxSizeMB_Original
			AND (([IDArchiveDatabase] IS NULL AND @IDArchiveDatabase IS NULL) OR ([IDArchiveDatabase] = @IDArchiveDatabase))
			AND (([AS_retain_minutes] IS NULL AND @ArchiveStorageRetainMinutes_Original IS NULL) OR ([AS_retain_minutes] = @ArchiveStorageRetainMinutes_Original))
			AND (([AS_max_size_mb] IS NULL AND @ArchiveStorageMaxSizeMB_Original IS NULL) OR ([AS_max_size_mb] = @ArchiveStorageMaxSizeMB_Original))

	DECLARE @IDDevice AS [GuidType]
	DECLARE @DeviceName AS [NameType]
	
	DECLARE recordingstoragedevice_cursor CURSOR FOR 
		SELECT rsd.[device_id], d.[Name]
		FROM [RecordingStorageDevice] rsd 
		INNER JOIN [Devices] d ON rsd.[device_id] = d.[IDDevice]
		WHERE rsd.[recordingstorage_id] = @RecordingStorageId

	OPEN recordingstoragedevice_cursor
	
	FETCH NEXT FROM recordingstoragedevice_cursor INTO @IDDevice, @DeviceName

	WHILE @@FETCH_STATUS = 0
	BEGIN
		/*
		<task type="DatabaseUpgrade.UpgradeTo40MoveDevice" id="NEWID()">
			<target>@IDRecorder</target>
			<idStorage>@RecordingStorageId</idStorage> 
			<idDevice>@IDDevice</idDevice>
			<deviceName>@DeviceName</deviceName>
			<sourcePath>@MediaDatabasePath</sourcePath>
		</task>
		*/
		
		-- Create the task for the recorder
		INSERT [Task] (
			[type],
			[target],
			[data_a],
			[data_b],
			[percent_completed],
			[status_details], 
			[task_status],
			[start_time],
			[end_time]
		)
		VALUES (
			'DatabaseUpgrade.UpgradeTo40MoveDevice',
			@IDRecorder,
			'',
			NULL,
			0,
			'',
			0,
			@CurrentUtcTime,
			NULL
		)

		DECLARE @task_id BIGINT
		SET @task_id = @@IDENTITY

		DECLARE @taskType NVARCHAR(200)
		SET @taskType = 'DatabaseUpgrade.UpgradeTo40MoveDevice'

		DECLARE @upgradeDatabaseXml XML
		SET @upgradeDatabaseXml = '<task/>'

		SET @upgradeDatabaseXml.modify('insert attribute type {sql:variable("@taskType")} as first into (/task)[1]')
		SET @upgradeDatabaseXml.modify('insert attribute id {sql:variable("@task_id")} as last into (/task)[1]')

		SET @upgradeDatabaseXml.modify('insert element target {sql:variable("@IDRecorder")} as last into (/task)[1]')
		SET @upgradeDatabaseXml.modify('insert element idStorage {sql:variable("@RecordingStorageId")} as last into (/task)[1]')
		SET @upgradeDatabaseXml.modify('insert element idDevice {sql:variable("@IDDevice")} as last into (/task)[1]')
		SET @upgradeDatabaseXml.modify('insert element deviceName {sql:variable("@DeviceName")} as last into (/task)[1]')
		SET @upgradeDatabaseXml.modify('insert element sourcePath {sql:variable("@MediaDatabasePath")} as last into (/task)[1]')

		UPDATE [Task] SET [data_a] = @upgradeDatabaseXml WHERE [pkid] = @task_id

		FETCH NEXT FROM recordingstoragedevice_cursor INTO @IDDevice, @DeviceName
	END
	CLOSE recordingstoragedevice_cursor
	DEALLOCATE recordingstoragedevice_cursor

	FETCH NEXT FROM permutations_cursor
	INTO @IDRecorder, @IDMediaDatabase, @MediaDatabaseName, @MediaDatabaseDescription, @MediaDatabasePath, @MediaDatabaseIsDefault, @MediaDatabaseMaxMinutes,
		 @RecordingStorageRetainMinutes, @RecordingStorageMaxSizeMB, 
		 @IDArchiveDatabase, @ArchiveDatabaseName, @ArchiveDatabaseDescription, @ArchiveDatabasePath, @ArchiveDatabaseSchedule, 
		 @ArchiveStorageRetainMinutes, @ArchiveStorageMaxSizeMB, @DeviceCount
END
CLOSE permutations_cursor
DEALLOCATE permutations_cursor

-- Ensure that all recording servers have at least one recording storage
DECLARE recorder_cursor CURSOR FOR 
	SELECT [IDRecorder], [Name] FROM [Recorders] r

OPEN recorder_cursor

FETCH NEXT FROM recorder_cursor INTO @IDRecorder, @RecorderName

WHILE @@FETCH_STATUS = 0
BEGIN
	-- Ensure that the recording server has at least one recording storage configured
	IF NOT EXISTS (SELECT * FROM [RecordingStorage] WHERE [recordingserver_id] = @IDRecorder)
	BEGIN
		SET @RecordingStorageId = NEWID()

		SET @StorageName = [dbo].[CreateUniqueRecordingStorageName](@IDRecorder, @RecorderName)
		SET @RecordingStorageRetainMinutes = @DefaultRecordingStorageRetainMinutes
		SET @RecordingStorageMaxSizeMB = @DefaultRecordingStorageMaxSizeMB
		
		-- Create the recording storage
		INSERT INTO [RecordingStorage] (
			[pkid],
			[recordingserver_id],
			[name],
			[description],
			[path],
			[is_default],
			[retain_minutes],
			[max_size_mb],
			[created_date],
			[last_modified],
			[encryption_method],
			[encryption_password],
			[encryption_password_new]
		)
		VALUES (
			@RecordingStorageId,
			@IDRecorder,
			@StorageName,
			'',
			'C:\MediaDB',
			1,
			@RecordingStorageRetainMinutes,
			@RecordingStorageMaxSizeMB,
			@CurrentUtcTime,
			@CurrentUtcTime,
			'None',
			'',
			''
		)

		EXEC @status = [set_recordingstorage_security_upgrade] @RecordingStorageId, @CurrentUtcTime
		IF @status <> 0
		BEGIN
			-- Upgrade will fail!
			CLOSE recorder_cursor
			DEALLOCATE recorder_cursor
			RAISERROR(N'Failed to set security on recording storage created during upgrade. Error code: %i', 18, 3, @status)
			GOTO cleanup_label
		END
	END
	
	-- Ensure that one of the recording storages for the recordig server is set to default
	IF NOT EXISTS (SELECT * FROM [RecordingStorage] WHERE [recordingserver_id] = @IDRecorder AND [is_default] = 1)
	BEGIN
		SELECT TOP 1 @RecordingStorageId = [pkid] FROM [RecordingStorage] WHERE [recordingserver_id] = @IDRecorder ORDER BY [name]
		UPDATE [RecordingStorage] SET [is_default] = 1 WHERE [pkid] = @RecordingStorageId
	END
	
	FETCH NEXT FROM recorder_cursor INTO @IDRecorder, @RecorderName
END
CLOSE recorder_cursor 
DEALLOCATE recorder_cursor

-- TODO: Remove old media- and archive database stuff
ALTER TABLE [dbo].[Devices] DROP CONSTRAINT [FK_Devices_MediaDatabases]
ALTER TABLE [dbo].[Devices] DROP CONSTRAINT [FK_Devices_ArchiveDatabases]

ALTER TABLE [dbo].[Devices] DROP COLUMN [IDMediaDatabase]
ALTER TABLE [dbo].[Devices] DROP COLUMN [IDArchiveDatabase]
ALTER TABLE [dbo].[Devices] DROP COLUMN [ArchiveEnabled]
ALTER TABLE [dbo].[Devices] DROP COLUMN [DbEnabled]
ALTER TABLE [dbo].[Devices] DROP COLUMN [DbSettings] --  ensure that Description,ArchiveEnabled and ArchivePath are set when applying to MediaDB lib

IF  EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[Devices]') AND  name = N'EdgeStorageEnabled')
BEGIN
	ALTER TABLE [dbo].[Devices] DROP COLUMN [EdgeStorageEnabled]
END

DROP TABLE [MediaDatabases]
DROP TABLE [ArchiveDatabases]

cleanup_label:
DROP VIEW [dbo].[DeviceDatabaseUpgradeView]
DROP PROCEDURE [dbo].[set_recordingstorage_security_upgrade]
DROP FUNCTION [dbo].[CreateUniqueRecordingStorageName]
DROP FUNCTION [dbo].[CreateUniqueArchiveStorageName]
GO
